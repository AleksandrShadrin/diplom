from dataclasses import dataclass
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
