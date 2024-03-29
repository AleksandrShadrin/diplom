import grpc
from services.SendingService import SendingService
import proto_generated.SendingService_pb2_grpc
import proto_generated.LearningService_pb2_grpc
import proto_generated.Health_pb2_grpc
import proto_generated.ModelsService_pb2_grpc
import asyncio
from services.health import HealthService
from services.learning_grpc import LearningGrpcService
from services.models_grpc import ModelsService


class Server:

    def __init__(self, sending_service: SendingService,
                 health_service: HealthService,
                 learning_grpc_service: LearningGrpcService,
                 models_grpc_service: ModelsService) -> None:
        self.sending_service = sending_service
        self.health_service = health_service
        self.learning_service = learning_grpc_service
        self.models_service = models_grpc_service

        self.server = grpc.aio.server(maximum_concurrent_rpcs=2,
                                      options=[
                                          ('grpc.max_send_message_length',
                                           12 * 1024 * 1024 * 8),
                                          ('grpc.max_receive_message_length',
                                           12 * 1024 * 1024 * 8),
                                      ])
        self._cleanup_coroutines = []

    def launch(self) -> None:
        loop = asyncio.get_event_loop()
        try:
            loop.run_until_complete(self.start())
        except KeyboardInterrupt:
            pass
        finally:
            loop.run_until_complete(*self._cleanup_coroutines)
            loop.close()

    async def start(self) -> None:
        proto_generated.SendingService_pb2_grpc.add_DatasetSenderServicer_to_server(
            self.sending_service, self.server)

        proto_generated.Health_pb2_grpc.add_HealthServicer_to_server(
            self.health_service, self.server)

        proto_generated.LearningService_pb2_grpc.add_LearningServiceServicer_to_server(
            self.learning_service, self.server)

        proto_generated.ModelsService_pb2_grpc.add_ModelsServiceServicer_to_server(
            self.models_service, self.server)

        self.server.add_insecure_port('[::]:5000')

        await self.server.start()

        self.__on_launch()

        self._cleanup_coroutines.append(self.__shutdown())
        await self.server.wait_for_termination()

    async def __shutdown(self):
        print('Server stopping...')
        await self.server.stop(255)
        print('Server stopped')

    def __on_launch(self):
        print('server launched')
