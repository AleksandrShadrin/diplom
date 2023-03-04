from proto_generated.SendingService_pb2_grpc import DatasetSenderServicer


class SendingService(DatasetSenderServicer):
    def __init__(self) -> None:
        super().__init__()
