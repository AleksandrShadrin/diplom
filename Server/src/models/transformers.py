from models.TimeSeries import TimeSeries


class BaseTransformer:
    """Base TimeSeries transformer"""

    def transform(self, time_series: TimeSeries):
        """Transform TimeSeries"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def load(self, **kwargs):
        """Load transformer parameters"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def save(self, **kwargs):
        """Save transformer parameters"""
        raise NotImplementedError("BaseTransformer don't imlement this method")