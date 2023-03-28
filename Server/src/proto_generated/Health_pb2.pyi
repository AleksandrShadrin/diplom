from google.protobuf.internal import enum_type_wrapper as _enum_type_wrapper
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor
NOT_SERVING: ServingStatus
SERVING: ServingStatus

class HealthCheckRequest(_message.Message):
    __slots__ = []
    def __init__(self) -> None: ...

class HealthCheckResponse(_message.Message):
    __slots__ = ["status"]
    STATUS_FIELD_NUMBER: _ClassVar[int]
    status: ServingStatus
    def __init__(self, status: _Optional[_Union[ServingStatus, str]] = ...) -> None: ...

class ServingStatus(int, metaclass=_enum_type_wrapper.EnumTypeWrapper):
    __slots__ = []
