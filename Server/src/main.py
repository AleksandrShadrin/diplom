import grpc
import proto_generated.SendingService_pb2_grpc
from appdirs import user_data_dir
from python_json_config import ConfigBuilder
import asyncio


async def run():
    server = grpc.aio.server()
    proto_generated.SendingService_pb2_grpc.add_DatasetSenderServicer_to_server(
        None, server)
    await server.add_secure_port('[::]:50051')
    await server.start()
    await server.wait_for_termination()


if __name__ == '__main__':
    # asyncio.run(run())
    builder = ConfigBuilder()
    builder.add_optional_field('appname')
    config = builder.parse_config('./config.json')
    print(config.appname)
