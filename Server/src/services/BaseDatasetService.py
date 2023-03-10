from models.AppConfig import AppConfig
from models.Dataset import Dataset
from models.Result import Result


class BaseDatasetService:

    def __init__(self, config: AppConfig) -> None:
        self.config = config

    def set_dataset(self, dataset: Dataset):
        self.dataset = dataset

    def save_dataset(self, **kwargs) -> Result:
        raise NotImplementedError(
            "BaseDatasetService don't imlement this method")

    def load_dataset(self, **kwargs) -> Dataset:
        raise NotImplementedError(
            "BaseDatasetService don't imlement this method")
