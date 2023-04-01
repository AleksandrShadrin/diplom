from google.protobuf import empty_pb2 as _empty_pb2
from google.protobuf.internal import containers as _containers
from google.protobuf.internal import enum_type_wrapper as _enum_type_wrapper
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

CUT_BOTH: CutParameters
CUT_LEFT: CutParameters
CUT_RIGHT: CutParameters
DESCRIPTOR: _descriptor.FileDescriptor
ERROR: TrainStatus
FILL_ZEROES_BOTH: FillParameters
FILL_ZEROES_LEFT: FillParameters
FILL_ZEROES_RIGHT: FillParameters
SUCCESS: TrainStatus

class LearningModelNames(_message.Message):
    __slots__ = ["names"]
    NAMES_FIELD_NUMBER: _ClassVar[int]
    names: _containers.RepeatedScalarFieldContainer[str]
    def __init__(self, names: _Optional[_Iterable[str]] = ...) -> None: ...

class Model(_message.Message):
    __slots__ = ["dataset_name", "model_name", "model_parameters"]
    DATASET_NAME_FIELD_NUMBER: _ClassVar[int]
    MODEL_NAME_FIELD_NUMBER: _ClassVar[int]
    MODEL_PARAMETERS_FIELD_NUMBER: _ClassVar[int]
    dataset_name: str
    model_name: str
    model_parameters: ModelParameters
    def __init__(self, dataset_name: _Optional[str] = ..., model_name: _Optional[str] = ..., model_parameters: _Optional[_Union[ModelParameters, _Mapping]] = ...) -> None: ...

class ModelParameters(_message.Message):
    __slots__ = ["cut_parameters", "fill_parameters", "time_series_length"]
    CUT_PARAMETERS_FIELD_NUMBER: _ClassVar[int]
    FILL_PARAMETERS_FIELD_NUMBER: _ClassVar[int]
    TIME_SERIES_LENGTH_FIELD_NUMBER: _ClassVar[int]
    cut_parameters: CutParameters
    fill_parameters: FillParameters
    time_series_length: int
    def __init__(self, time_series_length: _Optional[int] = ..., cut_parameters: _Optional[_Union[CutParameters, str]] = ..., fill_parameters: _Optional[_Union[FillParameters, str]] = ...) -> None: ...

class TrainResponse(_message.Message):
    __slots__ = ["status"]
    STATUS_FIELD_NUMBER: _ClassVar[int]
    status: TrainStatus
    def __init__(self, status: _Optional[_Union[TrainStatus, str]] = ...) -> None: ...

class TrainedModel(_message.Message):
    __slots__ = ["dataset_name", "model_name", "statistics", "uuid"]
    class StatisticsEntry(_message.Message):
        __slots__ = ["key", "value"]
        KEY_FIELD_NUMBER: _ClassVar[int]
        VALUE_FIELD_NUMBER: _ClassVar[int]
        key: str
        value: float
        def __init__(self, key: _Optional[str] = ..., value: _Optional[float] = ...) -> None: ...
    DATASET_NAME_FIELD_NUMBER: _ClassVar[int]
    MODEL_NAME_FIELD_NUMBER: _ClassVar[int]
    STATISTICS_FIELD_NUMBER: _ClassVar[int]
    UUID_FIELD_NUMBER: _ClassVar[int]
    dataset_name: str
    model_name: str
    statistics: _containers.ScalarMap[str, float]
    uuid: str
    def __init__(self, dataset_name: _Optional[str] = ..., model_name: _Optional[str] = ..., uuid: _Optional[str] = ..., statistics: _Optional[_Mapping[str, float]] = ...) -> None: ...

class TrainedModels(_message.Message):
    __slots__ = ["models"]
    MODELS_FIELD_NUMBER: _ClassVar[int]
    models: _containers.RepeatedCompositeFieldContainer[TrainedModel]
    def __init__(self, models: _Optional[_Iterable[_Union[TrainedModel, _Mapping]]] = ...) -> None: ...

class FillParameters(int, metaclass=_enum_type_wrapper.EnumTypeWrapper):
    __slots__ = []

class CutParameters(int, metaclass=_enum_type_wrapper.EnumTypeWrapper):
    __slots__ = []

class TrainStatus(int, metaclass=_enum_type_wrapper.EnumTypeWrapper):
    __slots__ = []
