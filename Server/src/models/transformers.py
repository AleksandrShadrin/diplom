from models.TimeSeries import TimeSeries
from models.Dataset import Dataset
from sktime.transformations.series.boxcox import BoxCoxTransformer
import pandas as pd
from sktime.datatypes._panel._convert import from_2d_array_to_nested
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
    
class TransformTimeSeries(BaseTransformer):
    def __init__(self):
        self.transformer = BoxCoxTransformer()
    
    def transform(self, time_series: TimeSeries):
        values = self.transformer.transform(time_series.values)
        timeSeries = TimeSeries(class_name=self.timeseries_class, id=self.id, values=values)
        return timeSeries
    
    def train(self, dataset: Dataset):
        dataset = Dataset()
        time_series_list = dataset.time_series
        values = []
        for time_series in time_series_list:
            values.append(time_series.values) 
        timeSeriesDF = pd.DataFrame(values)
        a = from_2d_array_to_nested(timeSeriesDF)
        return self.transformer.fit(a)

    def load(self, **kwargs):
        return super().load(**kwargs)
    
    def save(self, **kwargs):
        return super().save(**kwargs)
    

    
    