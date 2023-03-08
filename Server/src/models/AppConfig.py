from python_json_config import Config


class AppConfig:

    def __init__(self, config: Config) -> None:
        self.__config = config

    def get_appname(self) -> str:
        return 'PSOSH.Server' if self.__config.appname is None else self.__config.appname