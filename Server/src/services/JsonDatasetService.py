from BaseDatasetService import BaseDatasetService


class JsonDatasetService(BaseDatasetService):

    def save_dataset(self, **kwargs):
        """Save dataset as json files.

        Keyword arguments:
        path -- str path to save folder
        """
        return super().save_dataset(**kwargs)
