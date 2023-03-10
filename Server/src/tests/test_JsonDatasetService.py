import random
from assertpy import assert_that
from models.AppConfig import AppConfig
from services.JsonDatasetService import JsonDatasetService
from models.Dataset import Dataset
from models.TimeSeries import TimeSeries
from python_json_config import ConfigBuilder
import os
from appdirs import user_data_dir


def get_AppConfig() -> AppConfig:
    builder = ConfigBuilder()
    dict = {'appname': 'PSOSH.server.tests'}
    return AppConfig(builder.parse_config(dict))


def test_JsonDatasetService_should_save_dataset_as_json_files_in_specified_directory_by_AppConfig(
):
    # ARRANGE
    appconfig = get_AppConfig()
    dataset_service = JsonDatasetService(appconfig)
    time_series_list = [
        TimeSeries([0., 0., 0.], str(1), str(i)) for i in range(1, 10)
    ]
    dataset_name = 'flawers'
    dataset = Dataset(time_series_list, dataset_name)

    # ACT
    dataset_service.set_dataset(dataset)
    dataset_service.save_dataset(rewrite=True)

    # ASSERT
    path = os.path.join(user_data_dir(), appconfig.get_appname(), 'datasets',
                        dataset_name)

    assert_that(os.path.exists(path)).is_true()


def test_JsonDatasetService_should_load_dataset_correctly():
    # ARRANGE
    appconfig = get_AppConfig()
    dataset_service = JsonDatasetService(appconfig)
    time_series_list = [
        TimeSeries([0., 0., 0.], str(1), str(i)) for i in range(1, 10)
    ]
    dataset_name = 'flawers'
    dataset = Dataset(time_series_list, dataset_name)

    # ACT
    dataset_service.set_dataset(dataset)
    dataset_service.save_dataset(rewrite=True)
    loaded_dataset = dataset_service.load_dataset(name=dataset_name)

    # ASSERT
    assert_that(dataset).is_equal_to(loaded_dataset)


def test_JsonDatasetService_should_return_None_when_dataset_dont_exists():
    # ARRANGE
    appconfig = get_AppConfig()
    dataset_service = JsonDatasetService(appconfig)
    dataset_name = 'unexisted_name'

    # ACT
    loaded_dataset = dataset_service.load_dataset(name=dataset_name)

    # ASSERT
    assert_that(loaded_dataset).is_none()


def test_JsonDatasetService_should_return_None_when_name_keyword_argument_not_specified(
):
    # ARRANGE
    appconfig = get_AppConfig()
    dataset_service = JsonDatasetService(appconfig)

    # ACT
    loaded_dataset = dataset_service.load_dataset()

    # ASSERT
    assert_that(loaded_dataset).is_none()


def test_JsonDatasetService_should_overwrite_dataset_when_rewrite_argument_is_True(
):
    # ARRANGE
    appconfig = get_AppConfig()
    dataset_service = JsonDatasetService(appconfig)
    time_series_list_first = [
        TimeSeries(
            [2 + random.random(), 2 + random.random(), 2 + random.random()],
            str(1), str(i)) for i in range(1, 10)
    ]
    time_series_list_second = [
        TimeSeries(
            [1 + random.random(), 1 + random.random(), 1 + random.random()],
            str(1), str(i)) for i in range(1, 10)
    ]

    dataset_name = 'flawers'
    dataset_first = Dataset(time_series_list_first, dataset_name)
    dataset_second = Dataset(time_series_list_second, dataset_name)

    # ACT
    dataset_service.set_dataset(dataset_first)
    dataset_service.save_dataset(rewrite=True)

    dataset_service.set_dataset(dataset_second)
    dataset_service.save_dataset(rewrite=True)

    # ASSERT
    loaded_dataset = dataset_service.load_dataset(name=dataset_name)
    assert_that(loaded_dataset).is_equal_to(dataset_second)


def test_JsonDatasetService_should_not_overwrite_dataset_when_rewrite_argument_is_False(
):
    # ARRANGE
    appconfig = get_AppConfig()
    dataset_service = JsonDatasetService(appconfig)
    time_series_list_first = [
        TimeSeries(
            [2 + random.random(), 2 + random.random(), 2 + random.random()],
            str(1), str(i)) for i in range(1, 10)
    ]
    time_series_list_second = [
        TimeSeries(
            [1 + random.random(), 1 + random.random(), 1 + random.random()],
            str(1), str(i)) for i in range(1, 10)
    ]

    dataset_name = 'flawers'
    dataset_first = Dataset(time_series_list_first, dataset_name)
    dataset_second = Dataset(time_series_list_second, dataset_name)

    # ACT
    dataset_service.set_dataset(dataset_first)
    dataset_service.save_dataset(rewrite=True)

    dataset_service.set_dataset(dataset_second)
    dataset_service.save_dataset(rewrite=False)

    # ASSERT
    loaded_dataset = dataset_service.load_dataset(name=dataset_name)
    assert_that(loaded_dataset).is_equal_to(dataset_first)
