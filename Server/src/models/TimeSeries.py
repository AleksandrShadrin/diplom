from dataclasses import dataclass
from enum import Enum
from typing import List
import proto_generated.SendingService_pb2 as proto_generated


@dataclass
class TimeSeries:
    """Class represent timeseries"""
    values: List[float]
    timeseries_class: str
    id: str

    def to_grpc_timeseries(self) -> proto_generated.TimeSeries:
        return proto_generated.TimeSeries(class_name=self.timeseries_class,
                                          id=self.id,
                                          values=self.values)


class FillParameters(Enum):
    """How fill TimeSeries"""
    FILL_ZEROES_LEFT = 0
    FILL_ZEROES_RIGHT = 1
    FILL_ZEROES_BOTH = 2


class CutParameters(Enum):
    CUT_LEFT = 0
    CUT_RIGHT = 1
    CUT_BOTH = 2


@dataclass
class TimeSeriesLearningParameters:
    """TimeSeries parameters to learn"""
    cut_parameters: CutParameters
    fill_parameters: FillParameters
    length: int