from multiprocessing import Pool
from services.BaseDatasetService import BaseDatasetService
from models.TimeSeries import TimeSeries
from models.Dataset import Dataset
from models.Result import Result
from models.AppConfig import AppConfig
import uuid
from appdirs import user_data_dir
import os
import shutil
from dacite import from_dict
from typing import List, Union
import gzip
import orjson


class JsonDatasetService(BaseDatasetService):

    def __init__(self, config: AppConfig) -> None:
        super().__init__(config)

    def save_dataset(self, **kwargs) -> Result:
        """
        Save dataset as json files.

        Keyword arguments:
        rewrite - bool, rewrite dataset, default: False
        """
        path = os.path.join(self._get_datasets_folder(), self.dataset.name)

        rewrite = kwargs.get('rewrite', False)

        if rewrite:
            res = self._try_recreate_folder(path)
            if res == Result.ERROR:
                return Result.ERROR
        elif os.path.exists(path):
            return Result.OK

        return self._write_dataset_on_disk(path, self.dataset)

    def load_dataset(self, **kwargs) -> Union[Dataset, None]:
        """
        Load dataset from storage

        Keyword arguments:
        name - str, name of dataset

        returns:
        Dataset - dataset loaded from storage or None 
        when name don't specified or dataset don't exist
        """
        if 'name' not in kwargs.keys():
            return None

        dataset_name = kwargs['name']
        path = os.path.join(self._get_datasets_folder(), dataset_name)

        if os.path.exists(path) is False:
            return None

        timeseries_names = os.listdir(path)
        path_to_ts = [os.path.join(path, name) for name in timeseries_names]

        pool = Pool(processes=6)
        timeseries_list = pool.map(self._load_file, path_to_ts)

        return Dataset(timeseries_list, dataset_name)

    def get_dataset_names(self) -> List[str]:
        """get names of datasets in storage

        returns:
        List[str] - list of dataset names
        """

        datasets_path = self._get_datasets_folder()
        if os.path.exists(datasets_path) is False:
            os.makedirs(datasets_path)

        return os.listdir(datasets_path)

    def _load_file(self, path: str) -> TimeSeries:
        """Load TimeSeries from file

        Parameters:
        path - str: path to file
        """
        data = None
        with gzip.open(path, 'rb') as fr:
            data = orjson.loads(fr.read())

        timeseries = from_dict(data_class=TimeSeries, data=data)
        return timeseries

    def _write_dataset_on_disk(self, path: str, dataset: Dataset) -> Result:
        """
        Write dataset on disk as json files

        params:
        path: str - path to save folder
        dataset: Dataset - dataset that should be saved

        returns:
        Result - ERROR or OK state of operation
        """
        for ts in dataset.time_series:
            try:
                with gzip.open(
                        os.path.join(path,
                                     str(uuid.uuid1()) + '.json.gz'),
                        'wb') as fw:
                    fw.write(orjson.dumps(ts))

            except OSError:
                return Result.ERROR

        return Result.OK

    def _get_datasets_folder(self) -> str:
        return os.path.join(user_data_dir(), self.config.get_appname(),
                            'datasets')

    def _try_recreate_folder(self, path: str) -> Result:
        """
        Try delete folder

        params:
        path: str - path of folder

        returns:
        Result - OK or ERROR state of operation

        """
        if os.path.exists(path):
            try:
                shutil.rmtree(path)
            except OSError:
                return Result.ERROR

        os.makedirs(path)

        return Result.OK