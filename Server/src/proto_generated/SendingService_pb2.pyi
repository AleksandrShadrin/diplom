from google.protobuf import empty_pb2 as _empty_pb2
from google.protobuf.internal import containers as _containers
from google.protobuf.internal import enum_type_wrapper as _enum_type_wrapper
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor
ERROR: SendingStatus
OK: SendingStatus

class DatasetNames(_message.Message):
    __slots__ = ["names"]
    NAMES_FIELD_NUMBER: _ClassVar[int]
    names: _containers.RepeatedScalarFieldContainer[str]
    def __init__(self, names: _Optional[_Iterable[str]] = ...) -> None: ...

class DatasetShard(_message.Message):
    __slots__ = ["dataset_name", "time_series", "update_dataset"]
    DATASET_NAME_FIELD_NUMBER: _ClassVar[int]
    TIME_SERIES_FIELD_NUMBER: _ClassVar[int]
    UPDATE_DATASET_FIELD_NUMBER: _ClassVar[int]
    dataset_name: str
    time_series: TimeSeries
    update_dataset: bool
    def __init__(self, dataset_name: _Optional[str] = ..., update_dataset: bool = ..., time_series: _Optional[_Union[TimeSeries, _Mapping]] = ...) -> None: ...

class SendResponse(_message.Message):
    __slots__ = ["message", "status"]
    MESSAGE_FIELD_NUMBER: _ClassVar[int]
    STATUS_FIELD_NUMBER: _ClassVar[int]
    message: str
    status: SendingStatus
    def __init__(self, message: _Optional[str] = ..., status: _Optional[_Union[SendingStatus, str]] = ...) -> None: ...

class TimeSeries(_message.Message):
    __slots__ = ["class_name", "id", "values"]
    CLASS_NAME_FIELD_NUMBER: _ClassVar[int]
    ID_FIELD_NUMBER: _ClassVar[int]
    VALUES_FIELD_NUMBER: _ClassVar[int]
    class_name: str
    id: str
    values: _containers.RepeatedScalarFieldContainer[float]
    def __init__(self, class_name: _Optional[str] = ..., id: _Optional[str] = ..., values: _Optional[_Iterable[float]] = ...) -> None: ...

class SendingStatus(int, metaclass=_enum_type_wrapper.EnumTypeWrapper):
    __slots__ = []
