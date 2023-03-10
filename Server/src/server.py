import grpc
from services.SendingService import SendingService
import proto_generated.SendingService_pb2_grpc
import asyncio


class Server:

    def __init__(self, sending_service: SendingService) -> None:
        self.sending_service = sending_service
        self.server = grpc.aio.server()
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
        self.server.add_insecure_port('[::]:50051')

        await self.server.start()

        self.__on_launch()

        self._cleanup_coroutines.append(self.__shutdown())
        await self.server.wait_for_termination()

    async def __shutdown(self):
        print('Server stopping...')
        await self.server.stop(5)
        print('Server stopped')

    def __on_launch(self):
        print('server launched')
