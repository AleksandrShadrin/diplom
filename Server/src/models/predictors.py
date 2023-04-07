import os
from typing import Any, Dict
from models.TimeSeries import TimeSeries
from models.Dataset import Dataset
from sklearn.preprocessing import LabelEncoder
import pickle
import gzip
from models.Result import Result
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score, balanced_accuracy_score, f1_score
import joblib
import orjson
import numpy as np


class BasePredictor:
    """Base predictor for TimeSeries"""
    id: str
    path: str
    _label_encoder: LabelEncoder
    """
    Parameters: 
    id: str - id of dataset
    path: str - path to application models storage
    """

    def __init__(self, id: str, path: str) -> None:
        self._label_encoder = LabelEncoder()
        self.path = path
        self.id = id

    def train(self, dataset: Dataset) -> Result:
        """Train predictor using dataset"""
        res = self.fit_label_encoder(dataset)

        if res == Result.ERROR:
            return res

        time_series_with_encoded_labels = [
            TimeSeries(values=ts.values,
                       id=ts.id,
                       timeseries_class=self._label_encoder.transform(
                           [ts.timeseries_class]))
            for ts in dataset.time_series
        ]

        dataset_with_encoded_labels = Dataset(
            time_series=time_series_with_encoded_labels, name=dataset.name)

        res = self._train_model(dataset_with_encoded_labels)

        return Result.OK

    def predict(self, time_series: TimeSeries) -> str:
        """Make prediction"""

        prediction = self._make_model_prediction(time_series)

        return self._label_encoder.inverse_transform([prediction])[0]

    def load(self, **kwargs) -> Result:
        """Load predictor parameters"""
        try:
            with gzip.open(os.path.join(self.path, self.id, 'encoder.data'),
                           'rb') as f:
                self._label_encoder = pickle.load(f)
        except Exception:
            return Result.ERROR

        return self._load_model()

    def save(self, **kwargs) -> Result:
        """Save predictor parameters"""
        path = os.path.join(self.path, self.id)

        if os.path.exists(path) is False:
            os.makedirs(path)

        try:
            with gzip.open(os.path.join(path, 'encoder.data'), 'wb') as f:
                pickle.dump(self._label_encoder, f)
        except OSError:
            return Result.ERROR

        return self._save_model()

    def _make_model_prediction(self, time_series: TimeSeries) -> int:
        """model prediction"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def _train_model(self, dataset: Dataset) -> Result:
        """Train model.
        Should be implemented"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def _load_model(self) -> Result:
        """Load model parameters.
        Should be implemented"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def _save_model(self) -> Result:
        """Save model parameters.
        Should be implemented"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def fit_label_encoder(self, dataset: Dataset) -> Result:
        labels = [ts.timeseries_class for ts in dataset.time_series]
        try:
            self._label_encoder.fit(labels)
        except Exception:
            return Result.ERROR

        return Result.OK

    def get_stats(self) -> Dict[str, float]:
        """return dict stats of trained model"""
        raise NotImplementedError("BasePredictor don't imlement this method")


class SkLearnPredictor(BasePredictor):
    """Predictor that based on random forest classifier"""
    classifier: Any
    _train_stats: Dict[str, float]

    def __init__(self, id: str, path: str, classifier: Any) -> None:
        """Initialize of SkLearnPredictor
        params: 
        id: str - id of predictor
        path: str - path to folder of application
        classifier: Any - classifier from sklearn
        """
        self.classifier = classifier
        super().__init__(id, path)

    def _make_model_prediction(self, time_series: TimeSeries) -> int:

        return self.classifier.predict([time_series.values])[0]

    def _save_model(self) -> Result:
        """Save RandomForestClassifier"""
        filename = os.path.join(self.path, self.id, 'classifier.gz')

        try:
            joblib.dump(self.classifier, filename)
        except Exception:
            return Result.ERROR

        if self.__model_stats is not None:
            with open(os.path.join(self.path, self.id, 'stats.json'),
                      'wb') as fw:
                fw.write(
                    orjson.dumps(self.__model_stats,
                                 option=orjson.OPT_SERIALIZE_NUMPY))

        return Result.OK

    def _load_model(self) -> Result:
        """Load RandomForestClassifier"""
        filename = os.path.join(self.path, self.id, 'classifier.gz')

        try:
            self.classifier = joblib.load(filename)
        except Exception:
            return Result.ERROR

        with open(os.path.join(self.path, self.id, 'stats.json'), 'rb') as fr:
            self.__model_stats = orjson.loads(fr.read())

        return Result.OK

    def _train_model(self, dataset: Dataset) -> Result:
        """Train RandomForestClassifier"""

        ts_values = [ts.values for ts in dataset.time_series]
        ts_labels = [ts.timeseries_class for ts in dataset.time_series]

        X_train, X_test, y_train, y_test = train_test_split(ts_values,
                                                            ts_labels,
                                                            test_size=0.15)

        self.classifier.fit(X_train, np.ravel(y_train))

        self.__model_stats = {
            'точность':
            accuracy_score(np.ravel(y_test), self.classifier.predict(X_test)),
            'сбалансированная точность':
            balanced_accuracy_score(np.ravel(y_test),
                                    self.classifier.predict(X_test)),
            'F-score(macro)':
            f1_score(np.ravel(y_test),
                     self.classifier.predict(X_test),
                     average='macro'),
            'F-score(micro)':
            f1_score(np.ravel(y_test),
                     self.classifier.predict(X_test),
                     average='micro')
        }

        return Result.OK

    def get_stats(self) -> Dict[str, float]:
        return self.__model_stats