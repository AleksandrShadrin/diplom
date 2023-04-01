from dependency_injector import containers, providers

from factories.config_factory import build_config
from models.AppConfig import AppConfig
from services.JsonDatasetService import JsonDatasetService
from services.SendingService import SendingService
from services.health import HealthService
from services.learning_services import LearningService
from services.learning_grpc import LearningGrpcService
from server import Server


class AppContainer(containers.DeclarativeContainer):

    config = providers.Configuration()

    base_config = providers.Resource(build_config, path=config.path)

    app_config = providers.Singleton(AppConfig, config=base_config)

    json_dataset_service = providers.Singleton(JsonDatasetService,
                                               config=app_config)

    sending_service = providers.Singleton(SendingService,
                                          dataset_service=json_dataset_service)

    health_service = providers.Singleton(HealthService)

    learning_service = providers.Singleton(LearningService,
                                           app_config=app_config)

    learning_grpc_service = providers.Singleton(
        LearningGrpcService,
        dataset_service=json_dataset_service,
        learning_service=learning_service)

    server = providers.Factory(Server,
                               sending_service=sending_service,
                               health_service=health_service,
                               learning_grpc_service=learning_grpc_service)
