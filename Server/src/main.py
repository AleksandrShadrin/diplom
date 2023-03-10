import os
from containers.AppContainers import AppContainer
from dependency_injector.wiring import Provide, inject
from server import Server


@inject
def main(server: Server = Provide[AppContainer.server]):
    server.launch()


if __name__ == '__main__':
    app_container = AppContainer()

    app_container.config.from_dict({'path': os.path.join('.', 'config.json')})

    app_container.init_resources()
    app_container.wire(modules=[__name__])

    main()
