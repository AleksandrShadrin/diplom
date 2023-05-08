import asyncio
from concurrent.futures import ThreadPoolExecutor
from functools import partial
from proto_generated.LearningService_pb2 import FillParameters, TrainResponse
from proto_generated.LearningService_pb2 import TrainStatus
from proto_generated.LearningService_pb2_grpc import LearningServiceServicer
from proto_generated.LearningService_pb2 import LearningModelNames, \
    TrainedModel, TrainedModels, Model, ModelParameters, CutParameters

from services.learning_services import BaseLearningService
from services.BaseDatasetService import BaseDatasetService
from models.Result import Result
import models.TimeSeries


class LearningGrpcService(LearningServiceServicer):

    learning_service: BaseLearningService
    dataset_service: BaseDatasetService

    def __init__(self, learning_service: BaseLearningService,
                 dataset_service: BaseDatasetService) -> None:

        self.dataset_service = dataset_service
        self.learning_service = learning_service
        super().__init__()

    def GetLearningModelsNames(self, request, context):

        names = self.learning_service.get_model_names()
        return LearningModelNames(names=names)

    def GetTrainedModels(self, request, context):

        trained_models = self.learning_service.get_trained_models()

        trained_models = [
            TrainedModel(dataset_name=trained_model['dataset_name'],
                         model_name=trained_model['model_name'],
                         uuid=trained_model['model_id'],
                         statistics=trained_model['stats'])
            for trained_model in trained_models
        ]

        return TrainedModels(models=trained_models)

    async def TrainModel(self, request: Model, context):

        # dataset = self.dataset_service.load_dataset(name=request.dataset_name)
        loop = asyncio.get_event_loop()
        executor = ThreadPoolExecutor()

        get_dataset = partial(self.dataset_service.load_dataset,
                              name=request.dataset_name)

        dataset = await loop.run_in_executor(executor, get_dataset)

        learning_params = self._convert_from_grpc_to_learn_parameters(
            request.model_parameters)

        print('start train')

        # res = self.learning_service.train_model(request.model_name, dataset,
        #                                         learning_params)

        train = partial(self.learning_service.train_model, request.model_name,
                        dataset, learning_params)

        res = await loop.run_in_executor(executor, train)

        print('end train')

        if res == Result.ERROR:
            return TrainResponse(status=TrainStatus.ERROR)

        return TrainResponse(status=TrainStatus.SUCCESS)

    def _convert_from_grpc_to_learn_parameters(
        self, params: ModelParameters
    ) -> models.TimeSeries.TimeSeriesLearningParameters:

        cut_params = None
        fill_params = None

        if params.cut_parameters == CutParameters.CUT_BOTH:
            cut_params = models.TimeSeries.CutParameters.CUT_BOTH
        elif params.cut_parameters == CutParameters.CUT_RIGHT:
            cut_params = models.TimeSeries.CutParameters.CUT_RIGHT
        else:
            cut_params = models.TimeSeries.CutParameters.CUT_LEFT

        if params.fill_parameters == FillParameters.FILL_ZEROES_BOTH:
            fill_params = models.TimeSeries.FillParameters.FILL_ZEROES_BOTH
        elif params.fill_parameters == FillParameters.FILL_ZEROES_LEFT:
            fill_params = models.TimeSeries.FillParameters.FILL_ZEROES_LEFT
        else:
            fill_params = models.TimeSeries.FillParameters.FILL_ZEROES_RIGHT

        return models.TimeSeries.TimeSeriesLearningParameters(
            cut_parameters=cut_params,
            fill_parameters=fill_params,
            length=params.time_series_length)