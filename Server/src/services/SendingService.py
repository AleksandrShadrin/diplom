from proto_generated.SendingService_pb2_grpc import DatasetSenderServicer
from proto_generated.SendingService_pb2 import DatasetShard, Response, SendingStatus
from models.Result import Result
from models.Dataset import Dataset
from models.TimeSeries import TimeSeries
from BaseDatasetService import BaseDatasetService
from typing import Iterator, List


class SendingService(DatasetSenderServicer):

    def __init__(self, dataset_service: BaseDatasetService) -> None:
        self.dataset_service = dataset_service
        super().__init__()

    async def SendDataset(self, request_iterator: Iterator[DatasetShard],
                          context) -> Response:
        time_series_list: List[TimeSeries] = []
        dataset_name: str = ''
        update_dataset: bool = False

        for dshard in request_iterator:
            if dshard.HasField('dataset_name'):
                dataset_name = dshard.dataset_name
            if dshard.HasField('update_dataset'):
                update_dataset = dshard.update_dataset

            time_series = TimeSeries(dshard.time_series.values,
                                     dshard.time_series.class_name)

            time_series_list.append(time_series)

        dataset = Dataset(time_series_list, dataset_name)

        self.dataset_service.set_dataset(dataset)
        result = self.dataset_service.save_dataset()

        if result == Result.ERROR:
            return Response("something went wrong, try again",
                            SendingStatus.ERROR)
        elif result == Result.OK:
            return Response("dataset successfully loaded", SendingStatus.OK)
