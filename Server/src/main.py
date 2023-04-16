import multiprocessing
import os
import sys
from containers.AppContainers import AppContainer
from dependency_injector.wiring import Provide, inject
from server import Server


@inject
def main(server: Server = Provide[AppContainer.server]):
    multiprocessing.freeze_support()

    server.launch()


def resource_path(relative_path):
    if getattr(sys, 'frozen', False):  # Bundle Resource
        base_path = sys._MEIPASS
    else:
        base_path = os.path.abspath(".")
    return os.path.join(base_path, relative_path)


if __name__ == '__main__':

    filename = resource_path('./config.json')
    app_container = AppContainer()

    app_container.config.from_dict({'path': filename})

    app_container.init_resources()
    app_container.wire(modules=[__name__])
    main()
