from models.TimeSeries import TimeSeries
from models.Dataset import Dataset


class BasePredictor:
    """Base predictor for TimeSeries"""

    def train(self, dataset: Dataset):
        """Train predictor using dataset"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def predict(self, time_series: TimeSeries):
        raise NotImplementedError("BasePredictor don't imlement this method")

    def load(self, **kwargs):
        """Load predictor parameters"""
        raise NotImplementedError("BasePredictor don't imlement this method")

    def save(self, **kwargs):
        """Save predictor parameters"""
        raise NotImplementedError("BasePredictor don't imlement this method")