from google.protobuf.internal import containers as _containers
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class Model(_message.Message):
    __slots__ = ["uuid"]
    UUID_FIELD_NUMBER: _ClassVar[int]
    uuid: str
    def __init__(self, uuid: _Optional[str] = ...) -> None: ...

class ModelPredictionRequest(_message.Message):
    __slots__ = ["model", "time_series_values"]
    MODEL_FIELD_NUMBER: _ClassVar[int]
    TIME_SERIES_VALUES_FIELD_NUMBER: _ClassVar[int]
    model: Model
    time_series_values: TimeSeriesValues
    def __init__(self, time_series_values: _Optional[_Union[TimeSeriesValues, _Mapping]] = ..., model: _Optional[_Union[Model, _Mapping]] = ...) -> None: ...

class ModelPredictionResponse(_message.Message):
    __slots__ = ["classified_as"]
    CLASSIFIED_AS_FIELD_NUMBER: _ClassVar[int]
    classified_as: str
    def __init__(self, classified_as: _Optional[str] = ...) -> None: ...

class TimeSeriesValues(_message.Message):
    __slots__ = ["values"]
    VALUES_FIELD_NUMBER: _ClassVar[int]
    values: _containers.RepeatedScalarFieldContainer[float]
    def __init__(self, values: _Optional[_Iterable[float]] = ...) -> None: ...
