from python_json_config import Config, ConfigBuilder


def build_config(path: str) -> Config:
    print(path)

    builder = ConfigBuilder()
    builder.add_optional_field('appname')

    return builder.parse_config(path)
