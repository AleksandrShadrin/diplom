import os
from typing import Dict
from models.TimeSeries import TimeSeries, TimeSeriesLearningParameters, \
    FillParameters, CutParameters
from models.predictors import BasePredictor
from models.transformers import BaseTransformer
from models.Dataset import Dataset
from models.Result import Result
import numpy as np
import orjson


class BaseModel:
    """BaseModel of intellectual model for timeseries analysis"""

    def train(self, dataset: Dataset) -> Result:
        """Train intellectual model using dataset
        returns: 
        Dict[str, float] - statistics of train
        """
        raise NotImplementedError("BaseModel don't imlement this method")

    def load(self, **kwargs) -> Result:
        """Load model parameters"""
        raise NotImplementedError("BaseModel don't imlement this method")

    def save(self, **kwargs) -> Result:
        """Save model parameters"""
        raise NotImplementedError("BaseModel don't imlement this method")

    def predict_timeseries(self, time_series: TimeSeries) -> str:
        """Predict TimeSeries class"""
        raise NotImplementedError("BaseModel don't imlement this method")

    def get_stats(self) -> Dict[str, float]:
        """get model stats"""
        raise NotImplementedError("BaseModel don't imlement this method")


class ClassificationModel(BaseModel):
    """Model that classify timeseries
    Attributes:
    time_series_transformer: BaseTransformer - time_series transformer
    time_series_predictor: BasePredictor - time_series predictor
    time_series_learning_parameters: TimeSeriesLearningParameters - parameters that 
    define how model should be trained
    """
    time_series_transformer: BaseTransformer
    time_series_predictor: BasePredictor
    time_series_learning_parameters: TimeSeriesLearningParameters
    path: str
    id: str

    def __init__(
            self, predictor: BasePredictor, transformer: BaseTransformer,
            time_series_learning_parameters: TimeSeriesLearningParameters
    ) -> None:
        self.time_series_predictor = predictor
        self.time_series_transformer = transformer
        self.time_series_learning_parameters = time_series_learning_parameters

    def train(self, dataset: Dataset) -> Result:
        prepared_dataset = self._prepare_dataset(
            dataset, self.time_series_learning_parameters)

        self.time_series_transformer.fit(prepared_dataset)

        transformed_time_series = [
            self.time_series_transformer.transform(time_series)
            for time_series in dataset.time_series
        ]

        transformed_dataset = Dataset(time_series=transformed_time_series,
                                      name=dataset.name)

        res = self.time_series_predictor.train(transformed_dataset)

        return res

    def load(self, **kwargs) -> Result:
        res = self.time_series_predictor.load()
        if res == Result.ERROR:
            return res

        res = self._load_learning_parameters()

        if res == Result.ERROR:
            return res

        return self.time_series_transformer.load()

    def predict_timeseries(self, time_series: TimeSeries) -> str:
        ts = self._prepare_time_series(time_series,
                                       self.time_series_learning_parameters)

        ts = self.time_series_transformer.transform(time_series)
        return self.time_series_predictor.predict(ts)

    def save(self, **kwargs):
        res = self.time_series_predictor.save()
        if res == Result.ERROR:
            return res

        res = self.time_series_transformer.save()
        if res == Result.ERROR:
            return res

        return self._save_learning_parameters()

    def get_stats(self) -> Dict[str, float]:
        return self.time_series_predictor.get_stats()

    def _load_learning_parameters(self) -> Result:
        params = None
        try:
            with open(os.path.join(self.path, self.id, 'learning_params.json'),
                      'rb') as fr:
                params = orjson.loads(fr.read())
        except OSError:
            return Result.ERROR

        self.time_series_learning_parameters = TimeSeriesLearningParameters(
            cut_parameters=CutParameters(params['cut_parameters']),
            fill_parameters=FillParameters(params['fill_parameters']),
            length=params['length'])

        return Result.OK

    def _save_learning_parameters(self) -> Result:

        try:
            with open(os.path.join(self.path, self.id, 'learning_params.json'),
                      'wb') as fw:
                fw.write(orjson.dumps(self.time_series_learning_parameters))
        except OSError:
            return Result.ERROR

        return Result.OK

    def _prepare_dataset(self, dataset: Dataset,
                         learning_parameters: TimeSeriesLearningParameters):
        """Prepare dataset using LearningParameters
        params:
        dataset: Dataset - dataset for preparation
        learning_parameters: TimeSeriesLearningParameters - parameters that
        should be used for preparation
        """

        prepared_time_series = [
            self._prepare_time_series(ts, learning_parameters)
            for ts in dataset.time_series
        ]

        return Dataset(time_series=prepared_time_series, name=dataset.name)

    def _prepare_time_series(
            self, time_series: TimeSeries,
            learning_parameters: TimeSeriesLearningParameters) -> TimeSeries:
        prepared_ts: TimeSeries

        if len(time_series.values) > learning_parameters.length:
            prepared_ts = self._when_time_series_length_more(
                length=learning_parameters.length,
                parameters=learning_parameters.cut_parameters,
                time_series=time_series)
        else:
            prepared_ts = self._when_time_series_length_less(
                length=learning_parameters.length,
                parameters=learning_parameters.fill_parameters,
                time_series=time_series)

        return prepared_ts

    def _when_time_series_length_more(self, length: int,
                                      parameters: CutParameters,
                                      time_series: TimeSeries):
        """cut values when length is not same as defined"""
        values = time_series.values
        values_count = len(values) - length

        if parameters == CutParameters.CUT_BOTH:
            values = np.array(values)
            values = values[int(values_count / 2):int(values.shape[0] -
                                                      (values_count / 2))]
        elif parameters == CutParameters.CUT_LEFT:
            values = np.array(values)
            values = values[int(values_count):]
        elif parameters == CutParameters.CUT_RIGHT:
            values = np.array(values)
            values = values[:int(values.shape[0] - values_count)]

        return TimeSeries(values=values,
                          timeseries_class=time_series.timeseries_class,
                          id=time_series.id)

    def _when_time_series_length_less(self, length: int,
                                      parameters: FillParameters,
                                      time_series: TimeSeries):
        """add values when length is not same as defined"""
        values = time_series.values
        values_count = length - len(values)

        if parameters == FillParameters.FILL_ZEROES_BOTH:
            values = np.concatenate([
                np.repeat(0, values_count / 2), values,
                np.repeat(0, values_count - values_count / 2)
            ])
        elif parameters == FillParameters.FILL_ZEROES_LEFT:
            values = np.concatenate([np.repeat(0, values_count), values])
        elif parameters == FillParameters.FILL_ZEROES_RIGHT:
            values = np.concatenate([values, np.repeat(0, values_count)])

        return TimeSeries(values=values,
                          timeseries_class=time_series.timeseries_class,
                          id=time_series.id)
