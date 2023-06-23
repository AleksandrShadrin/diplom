from typing import Dict
from proto_generated.ModelsService_pb2_grpc import ModelsServiceServicer
from services.learning_services import BaseLearningService
from proto_generated.ModelsService_pb2 import ModelPredictionRequest, ModelPredictionResponse
from models.intellectual_models import BaseModel
from models.TimeSeries import TimeSeries


class ModelsService(ModelsServiceServicer):

    learning_service: BaseLearningService
    cached_models: Dict[str, BaseModel]

    def __init__(self, learning_service: BaseLearningService) -> None:
        self.learning_service = learning_service
        self.cached_models = {}
        super().__init__()

    def GetModelPrediction(self, request: ModelPredictionRequest, context):
        if request.model.uuid not in self.cached_models.keys():
            self.cached_models[
                request.model.uuid] = self.learning_service.load_model(
                    request.model.uuid)

        model = self.cached_models[request.model.uuid]

        if len(self.cached_models) > 20:
            del self.cached_models[0]

        ts = TimeSeries(id='',
                        timeseries_class='',
                        values=request.time_series_values.values)

        prediction = model.predict_timeseries(ts)

        return ModelPredictionResponse(classified_as=prediction)

    def GetModelPredictions(self, request_iterator, context):
        return super().GetModelPredictions(request_iterator, context)