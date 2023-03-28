import os
from models.transformers import MiniRocketTimeSeriesTransformer
from models.intellectual_models import ClassificationModel
from models.predictors import RandomForestPredictor
from models.TimeSeries import TimeSeriesLearningParameters
from services.BaseDatasetService import BaseDatasetService
from models.TimeSeries import FillParameters, CutParameters
from models.TimeSeries import TimeSeries
from containers.AppContainers import AppContainer
from dependency_injector.wiring import Provide, inject
from server import Server
from models.intellectual_models import ClassificationModel


@inject
def main(server: Server = Provide[AppContainer.server],
         ds_service: BaseDatasetService = Provide[
             AppContainer.json_dataset_service]):

    server.launch()


if __name__ == '__main__':
    file_path = os.path.dirname(__file__)

    app_container = AppContainer()

    app_container.config.from_dict(
        {'path': os.path.join(file_path, 'config.json')})

    app_container.init_resources()
    app_container.wire(modules=[__name__])
    main()
