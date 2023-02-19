import grpc;
import proto_generated.SendingService_pb2_grpc as SendingService_pb2_grpc;
from concurrent import futures;

def run():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers = 4))
    SendingService_pb2_grpc.add_SendingServiceServicer_to_server(none, server)
    server.add_secure_port('[::]:50051')
    server.start()
    server.wait_for_termination()
