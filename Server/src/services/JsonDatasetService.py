from python_json_config import Config
from BaseDatasetService import BaseDatasetService
from models.Dataset import Dataset
from models.Result import Result
from models.AppConfig import AppConfig
from appdirs import user_data_dir
from tempfile import gettempdir
import json
import os


class JsonDatasetService(BaseDatasetService):

    def __init__(self, config: AppConfig) -> None:
        super().__init__(config)

    def save_dataset(self, **kwargs) -> Result:
        """
        Save dataset as json files.

        Keyword arguments:
        rewrite - bool, rewrite dataset, default: False
        """
        path = os.path.join(user_data_dir(), self.config.get_appname(),
                            'datasets', self.dataset.name)

        rewrite = kwargs.get('rewrite', False)

        if rewrite:
            res = self._try_delete_folder(path)
            if res == Result.ERROR:
                return Result.ERROR
        elif os.path.exist(path):
            return Result.OK

        return self._try_delete_folder(path, self.dataset)

    def _write_dataset_on_disk(path: str, dataset: Dataset) -> Result:
        """
        Write dataset on disk as json files
        
        params:
        path: str - path to save folder
        dataset: Dataset - dataset that should be saved
        
        returns:
        Result - ERROR or OK state of operation
        """
        for (i, ts) in enumerate(dataset.time_series):
            try:
                with open(os.path.join(path, i), 'w') as fw:
                    json.dump(ts, fw)
            except OSError:
                return Result.ERROR

        Result.OK

    def _try_delete_folder(path: str) -> Result:
        """
        Try delete folder
        
        params:
        path: str - path of folder
        
        returns:
        Result - OK or ERROR state of operation
        
        """
        if os.path.exists(path):
            try:
                os.removedirs()
            except OSError:
                return Result.ERROR

        return Result.OK