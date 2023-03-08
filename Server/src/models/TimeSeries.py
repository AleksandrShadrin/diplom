from dataclasses import dataclass
from typing import List


@dataclass
class TimeSeries:
    """Class represent timeseries"""
    values: List[float]
    timeseries_class: str
    id: str
