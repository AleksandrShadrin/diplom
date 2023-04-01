import os
from services.BaseDatasetService import BaseDatasetService
from containers.AppContainers import AppContainer
from dependency_injector.wiring import Provide, inject
from server import Server


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
