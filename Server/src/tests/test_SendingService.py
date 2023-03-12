from unittest.mock import MagicMock, create_autospec
import pytest
import grpc

from python_json_config import ConfigBuilder
from models.TimeSeries import TimeSeries
from services.SendingService import SendingService
from models.Dataset import Dataset
from models.Result import Result
from models.AppConfig import AppConfig
from services.BaseDatasetService import BaseDatasetService
from proto_generated.SendingService_pb2 import DatasetShard, SendingStatus
from assertpy import assert_that


def get_AppConfig() -> AppConfig:
    builder = ConfigBuilder()
    dict = {'appname': 'PSOSH.server.tests'}
    return AppConfig(builder.parse_config(dict))


@pytest.mark.asyncio
async def test_SendingService_should_save_sended_dataset_correctly():

    # ARRANGE
    mock_dataset_service = BaseDatasetService(get_AppConfig())
    mock_dataset_service.set_dataset = MagicMock()
    mock_dataset_service.save_dataset = MagicMock(return_value=Result.OK)

    context = create_autospec(spec=grpc.aio.ServicerContext)

    sending_service = SendingService(mock_dataset_service)

    dataset = Dataset(name='flawers',
                      time_series=[
                          TimeSeries([1, 2, 3], str(i), str(i))
                          for i in range(500)
                      ])

    dataset_shards = map(
        lambda ts: DatasetShard(dataset_name='flawers',
                                update_dataset=True,
                                time_series=ts.to_grpc_timeseries()),
        dataset.time_series)

    # ACT

    response = await sending_service.SendDataset(dataset_shards, context)

    # ASSERT
    mock_dataset_service.set_dataset.assert_called_once()
    mock_dataset_service.set_dataset.assert_called_with(dataset)
    mock_dataset_service.save_dataset.assert_called_with(rewrite=True)
    assert_that(response.message).is_equal_to('dataset successfully loaded')
    assert_that(response.status).is_equal_to(SendingStatus.OK)
