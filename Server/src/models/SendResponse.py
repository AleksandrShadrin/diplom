from dataclasses import dataclass
from enum import Enum
from proto_generated.SendingService_pb2 import SendResponse as grpc_SendResponse, SendingStatus


class Status(Enum):
    OK = 1,
    ERROR = 2


def convert_status(status: Status):
    if status == Status.OK:
        return SendingStatus.OK
    else:
        return SendingStatus.ERROR


@dataclass
class SendResponse:
    message: str
    status: Status

    def convert_to_grpc_send_response(self) -> grpc_SendResponse:
        response = grpc_SendResponse(message=self.message,
                                     status=convert_status(self.status))
        return response