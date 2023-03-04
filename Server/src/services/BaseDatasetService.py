from typing import List
from models.TimeSeries import TimeSeries
from models.Dataset import Dataset


class BaseDatasetService:
    def __init__(self) -> None:
        pass

    def set_dataset(self, dataset: Dataset):
        self.dataset = dataset

    def save_dataset(self, **kwargs):
        raise NotImplemented("BaseDatasetService don't imlement this method")

    def load_dataset(self, **kwargs):
        raise NotImplemented("BaseDatasetService don't imlement this method")
