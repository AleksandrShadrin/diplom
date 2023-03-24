from models.TimeSeries import TimeSeries, TimeSeriesLearningParameters
from models.predictors import BasePredictor
from models.transformers import BaseTransformer
from models.Dataset import Dataset


class BaseModel:
    """BaseModel of intellectual model for timeseries analysis"""

    def train(self, dataset: Dataset):
        """Train intellectual model using dataset"""
        raise NotImplementedError("BaseModel don't imlement this method")

    def load(self, **kwargs):
        """Load model parameters"""
        raise NotImplementedError("BaseModel don't imlement this method")

    def save(self, **kwargs):
        """Save model parameters"""
        raise NotImplementedError("BaseModel don't imlement this method")

    def predict_timeseries(self, time_series: TimeSeries):
        """Predict TimeSeries class"""
        raise NotImplementedError("BaseModel don't imlement this method")


class ClassificationModel(BaseModel):
    """Model that classify timeseries"""
    time_series_transformer: BaseTransformer
    time_series_predictor: BasePredictor
    time_series_learning_parameters: TimeSeriesLearningParameters

    def __init__(
            self, predictor: BasePredictor, transformer: BaseTransformer,
            time_series_learning_parameters: TimeSeriesLearningParameters
    ) -> None:
        self.time_series_predictor = predictor
        self.time_series_transformer = transformer
        self.time_series_learning_parameters = time_series_learning_parameters

    def load(self, **kwargs):
        return super().load(**kwargs)

    def predict_timeseries(self, time_series: TimeSeries):
        return super().predict_timeseries(time_series)

    def save(self, **kwargs):
        return super().save(**kwargs)