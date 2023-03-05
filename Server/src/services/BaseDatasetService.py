from typing import List
from models.AppConfig import AppConfig
from models.TimeSeries import TimeSeries
from models.Dataset import Dataset
from models.Result import Result


class BaseDatasetService:

    def __init__(self, config: AppConfig) -> None:
        self.config = config

    def set_dataset(self, dataset: Dataset):
        self.dataset = dataset

    def save_dataset(self, **kwargs) -> Result:
        raise NotImplemented("BaseDatasetService don't imlement this method")

    def load_dataset(self, **kwargs) -> Dataset:
        raise NotImplemented("BaseDatasetService don't imlement this method")
