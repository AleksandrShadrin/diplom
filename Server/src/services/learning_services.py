import os
from typing import Dict, List, Union
from models.Dataset import Dataset
from models.Result import Result
from models.intellectual_models import BaseModel, ClassificationModel
from models.TimeSeries import TimeSeriesLearningParameters
from models.transformers import BaseTransformer, MiniRocketTimeSeriesTransformer
from models.AppConfig import AppConfig
from models.predictors import BasePredictor, SkLearnPredictor
from sklearn.ensemble import RandomForestClassifier
from sklearn.neural_network import MLPClassifier
from itertools import product
from appdirs import user_data_dir
import orjson
import uuid


class BaseLearningService:
    """Base learning service class"""

    def get_model_names(self) -> List[str]:
        """get model names
        
        returns: list of models names
        """
        raise NotImplementedError("BaseLearningService don't implement it.")

    def train_model(
            self, model_name: str, dataset: Dataset,
            learning_parameters: TimeSeriesLearningParameters) -> Result:
        """train selected mode by name for specified dataset
        
        params: 
        model_name: str - name of model
        dataset: Dataset - selected dataset
        learning_parameters: TimeSeriesLearningParameters - parameters for learning
        
        returns: result of train
        """
        raise NotImplementedError("BaseLearningService don't implement it.")

    def load_model(self, model_id: str) -> Union[BaseModel, None]:
        """Load model from storage
        
        params:
        model_id: str - id of model
        
        returns: intellectual model or Nones
        """
        raise NotImplementedError("BaseLearningService don't implement it.")

    def get_trained_models(
            self) -> List[Dict[str, Union[str, Dict[str, float]]]]:
        """Get trained models information
        
        returns: dictionary with keys model_name and model_id
        """
        raise NotImplementedError("BaseLearningService don't implement it.")


class LearningService(BaseLearningService):

    config: AppConfig
    __predictor_transformer_delimeter: str = ':'

    def __init__(self, app_config: AppConfig) -> None:
        self.config = app_config

    def get_model_names(self) -> List[str]:

        products = product(self._get_predictors_names(),
                           self._get_transformers_names())

        return [
            self.__predictor_transformer_delimeter.join(prod)
            for prod in products
        ]

    def train_model(
            self, model_name: str, dataset: Dataset,
            learning_parameters: TimeSeriesLearningParameters) -> Result:

        id = str(uuid.uuid1())

        (predictor_name, transformer_name) = model_name.split(
            self.__predictor_transformer_delimeter)

        predictor = self._init_predictor(predictor_name,
                                         id,
                                         path=self._get_models_folder())

        transformer = self._init_transformer(transformer_name,
                                             id,
                                             path=self._get_models_folder())

        intellectual_model = ClassificationModel(
            predictor,
            transformer,
            learning_parameters,
            path=self._get_models_folder(),
            id=id)

        res = intellectual_model.train(dataset)
        if res == Result.ERROR:
            return res

        res = intellectual_model.save()

        if res == Result.ERROR:
            return res

        stats = intellectual_model.get_stats()

        return self._save_info_of_model(id, model_name, dataset.name, stats)

    def load_model(self, model_id: str) -> Union[BaseModel, None]:

        model_info = self._load_info_of_model(model_id)
        path = self._get_models_folder()

        if 'predictor' not in model_info.keys(
        ) or 'transformer' not in model_info.keys(
        ) or 'dataset_name' not in model_info.keys(
        ) or 'stats' not in model_info.keys():
            return None

        predictor = self._init_predictor(model_info['predictor'], model_id,
                                         path)
        transformer = self._init_predictor(model_info['transformer'], model_id,
                                           path)

        model = ClassificationModel(predictor, transformer, None)

        if model.load() == Result.ERROR:
            return None

        return model

    def get_trained_models(
            self) -> List[Dict[str, Union[str, Dict[str, float]]]]:

        path = self._get_models_folder()
        ids = os.listdir(path)

        info_list = [{'model_id': id} | self._load_info_of_model(id) for id in ids]

        info_list = [
            info for info in info_list
            if 'predictor' in info.keys() and 'transformer' in info.keys()
            and 'dataset_name' in info.keys() and 'stats' in info.keys()
        ]
        
        for info in info_list:
            predictor = info['predictor']
            transformer = info['transformer']
            
            info['model_name'] = self.__predictor_transformer_delimeter \
                .join([predictor, transformer])

        return info_list

    def _get_predictors_names(self) -> List[str]:
        return ['RandomForestClassifier', 'MLPClassifier']

    def _init_predictor(self, name: str, id: str, path: str) -> BasePredictor:
        classifier = None

        if name == 'RandomForestClassifier':
            classifier = RandomForestClassifier(400, n_jobs=6)
        elif name == 'MLPClassifier':
            classifier = MLPClassifier()

        return SkLearnPredictor(id=id, path=path, classifier=classifier)

    def _get_transformers_names(self) -> List[str]:
        return ['MiniRocket']

    def _init_transformer(self, name: str, id: str,
                          path: str) -> BaseTransformer:
        """Initialize transformer by it's name and id"""
        if name == 'MiniRocket':
            return MiniRocketTimeSeriesTransformer(id=id, path=path)
        else:
            return None

    def _load_info_of_model(self, model_id: str) -> Dict[str, str]:
        """loads info of intellectual model"""
        path = os.path.join(self._get_models_folder(), model_id)

        info = {}

        try:
            with open(os.path.join(path, 'model_info.json'), 'rb') as f:
                info = orjson.loads(f.read())
        except OSError:
            return info

        return info

    def _save_info_of_model(self, model_id: str, model_name: str,
                            dataset_name: str, stats: Dict[str,
                                                           float]) -> Result:
        """save model info"""
        path = os.path.join(self._get_models_folder(), model_id)

        if os.path.exists(path) is False:
            os.makedirs(path)

        (predictor_name, transformer_name) = model_name.split(
            self.__predictor_transformer_delimeter)

        info = {
            'transformer': transformer_name,
            'predictor': predictor_name,
            'dataset_name': dataset_name,
            'stats': stats
        }

        try:
            with open(os.path.join(path, 'model_info.json'), 'wb') as f:
                f.write(orjson.dumps(info, option=orjson.OPT_SERIALIZE_NUMPY))
        except OSError:
            return Result.ERROR

        return Result.OK

    def _get_models_folder(self) -> str:
        return os.path.join(user_data_dir(), self.config.get_appname(),
                            'models')