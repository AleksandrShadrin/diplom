from proto_generated.SendingService_pb2_grpc import DatasetSenderServicer
from models.Result import Result
from models.Dataset import Dataset
from models.TimeSeries import TimeSeries
from services.BaseDatasetService import BaseDatasetService
from typing import List, Iterable
from models.SendResponse import SendResponse, Status
import proto_generated.SendingService_pb2


class SendingService(DatasetSenderServicer):

    def __init__(self, dataset_service: BaseDatasetService) -> None:
        self.dataset_service = dataset_service
        super().__init__()

    async def SendDataset(
            self, request_iterator: Iterable[
                proto_generated.SendingService_pb2.TimeSeries],
            context) -> proto_generated.SendingService_pb2.SendResponse:

        time_series_list: List[TimeSeries] = []
        dataset_name: str = ''
        update_dataset: bool = False

        async for dshard in request_iterator:
            if dshard.HasField('dataset_name'):
                dataset_name = dshard.dataset_name
            if dshard.HasField('update_dataset'):
                update_dataset = dshard.update_dataset

            time_series = TimeSeries(
                [value for value in dshard.time_series.values],
                dshard.time_series.class_name, dshard.time_series.id)

            time_series_list.append(time_series)

        dataset = Dataset(time_series_list, dataset_name)

        self.dataset_service.set_dataset(dataset)
        result = self.dataset_service.save_dataset(rewrite=update_dataset)

        if result == Result.ERROR:
            return SendResponse(
                message="something went wrong, try again",
                status=Status.ERROR).convert_to_grpc_send_response()

        elif result == Result.OK:
            return SendResponse(
                message="dataset successfully loaded",
                status=Status.OK).convert_to_grpc_send_response()

    def GetLoadedDatasetNames(self, request, context):
        return proto_generated.SendingService_pb2.DatasetNames(
            names=self.dataset_service.get_dataset_names())
