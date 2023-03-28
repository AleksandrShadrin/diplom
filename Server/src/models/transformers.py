from models.TimeSeries import TimeSeries
from models.Dataset import Dataset


class BaseTransformer:
    """Base TimeSeries transformer"""

    def train(self, dataset: Dataset):
        """Train transformer using dataset"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def transform(self, time_series: TimeSeries):
        """Transform TimeSeries"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def load(self, **kwargs):
        """Load transformer parameters"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def save(self, **kwargs):
        """Save transformer parameters"""
        raise NotImplementedError("BaseTransformer don't imlement this method")
