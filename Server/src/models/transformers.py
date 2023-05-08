import os
from models.TimeSeries import TimeSeries
from models.Dataset import Dataset
import pandas as pd
from sktime.datatypes._panel._convert import from_2d_array_to_nested
from sktime.transformations.panel.rocket import MiniRocket
from sktime.transformations.panel.catch22 import Catch22
import numpy as np
import pycatch22


class BaseTransformer:
    """Base TimeSeries transformer"""

    id: str
    path: str
    """
    Parameters: 
    id: str - id of dataset
    path: str - path to application models storage
    """

    def __init__(self, id: str, path: str):
        self.id = id
        self.path = path

    def fit(self, dataset: Dataset):
        """Train transformer using dataset"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def transform(self, time_series: TimeSeries) -> TimeSeries:
        """Transform TimeSeries"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def load(self, **kwargs):
        """Load transformer parameters"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def save(self, **kwargs):
        """Save transformer parameters"""
        raise NotImplementedError("BaseTransformer don't imlement this method")

    def _get_path(self) -> str:
        """Return path to transformer of model"""
        return os.path.join(self.path, self.id)

    def _get_transformer_name():
        """Should return transformer filename"""
        raise NotImplementedError("BaseTransformer don't imlement this method")


class MiniRocketTimeSeriesTransformer(BaseTransformer):
    """
    Class transform time_series using
    MiniRocket transfomation
    """
    """
    Parameters: 
    id: str - id of dataset
    path: str - path to application models storage
    """

    def __init__(self, id: str, path: str):
        super().__init__(id, path)
        self.transformer = MiniRocket(n_jobs=6)

    def transform(self, time_series: TimeSeries) -> TimeSeries:
        values = np.array(time_series.values)

        transformed_values = self.transformer.transform(values)

        timeSeries = TimeSeries(timeseries_class=time_series.timeseries_class,
                                id=time_series.id,
                                values=transformed_values.values[0])

        return timeSeries

    def fit(self, dataset: Dataset):
        time_series_list = dataset.time_series
        values = [time_series.values for time_series in time_series_list]

        time_series_df = pd.DataFrame(values)

        nested_array = from_2d_array_to_nested(time_series_df)

        return self.transformer.fit(nested_array)

    def load(self, **kwargs):
        path = os.path.join(self._get_path(),
                            self._get_transformer_name() + '.zip')
        self.transformer = self.transformer.load_from_path(path)

    def save(self, **kwargs):
        path = self._get_path()
        path_with_file_name = os.path.join(path, self._get_transformer_name())
        if os.path.exists(path):
            self.transformer.save(path_with_file_name)
        else:
            os.makedirs(path)
            self.transformer.save(path_with_file_name)

    def _get_transformer_name(self):
        return 'transformer'


class Catch22TimeSeriesTransformer(BaseTransformer):
    """
    Class transform time_series using
    Catch22 transfomation
    """
    """
    Parameters: 
    id: str - id of dataset
    path: str - path to application models storage
    """

    def __init__(self, id: str, path: str):
        super().__init__(id, path)

    def transform(self, time_series: TimeSeries) -> TimeSeries:
        values = np.array(time_series.values)

        transformed_values = pycatch22.catch22_all(values)['values']

        timeSeries = TimeSeries(timeseries_class=time_series.timeseries_class,
                                id=time_series.id,
                                values=transformed_values)

        return timeSeries

    def fit(self, dataset: Dataset):
        pass

    def load(self, **kwargs):
        pass

    def save(self, **kwargs):
        pass


class NoneTimeSeriesTransformer(BaseTransformer):
    """
    No transformer, just return timeseries values
    """
    """
    Parameters: 
    id: str - id of dataset
    path: str - path to application models storage
    """

    def __init__(self, id: str, path: str):
        super().__init__(id, path)

    def transform(self, time_series: TimeSeries) -> TimeSeries:
        values = np.array(time_series.values)

        timeSeries = TimeSeries(timeseries_class=time_series.timeseries_class,
                                id=time_series.id,
                                values=values)

        return timeSeries

    def fit(self, dataset: Dataset):
        pass

    def load(self, **kwargs):
        pass

    def save(self, **kwargs):
        pass

    def _get_transformer_name(self):
        return 'transformer'