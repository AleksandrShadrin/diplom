from dataclasses import dataclass
from models.TimeSeries import TimeSeries
from typing import List

@dataclass
class Dataset:
    """Class represent dataset"""
    time_series: List[TimeSeries]
    name: str
    