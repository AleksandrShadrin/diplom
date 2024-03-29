# Generated by the gRPC Python protocol compiler plugin. DO NOT EDIT!
"""Client and server classes corresponding to protobuf-defined services."""
import grpc

from proto_generated import SendingService_pb2 as SendingService__pb2
from google.protobuf import empty_pb2 as google_dot_protobuf_dot_empty__pb2


class DatasetSenderStub(object):
    """Missing associated documentation comment in .proto file."""

    def __init__(self, channel):
        """Constructor.

        Args:
            channel: A grpc.Channel.
        """
        self.SendDataset = channel.stream_unary(
            '/sending.DatasetSender/SendDataset',
            request_serializer=SendingService__pb2.DatasetShard.
            SerializeToString,
            response_deserializer=SendingService__pb2.SendResponse.FromString,
        )
        self.GetLoadedDatasetNames = channel.unary_unary(
            '/sending.DatasetSender/GetLoadedDatasetNames',
            request_serializer=google_dot_protobuf_dot_empty__pb2.Empty.
            SerializeToString,
            response_deserializer=SendingService__pb2.DatasetNames.FromString,
        )


class DatasetSenderServicer(object):
    """Missing associated documentation comment in .proto file."""

    def SendDataset(self, request_iterator, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')

    def GetLoadedDatasetNames(self, request, context):
        """Missing associated documentation comment in .proto file."""
        context.set_code(grpc.StatusCode.UNIMPLEMENTED)
        context.set_details('Method not implemented!')
        raise NotImplementedError('Method not implemented!')


def add_DatasetSenderServicer_to_server(servicer, server):
    rpc_method_handlers = {
        'SendDataset':
        grpc.stream_unary_rpc_method_handler(
            servicer.SendDataset,
            request_deserializer=SendingService__pb2.DatasetShard.FromString,
            response_serializer=SendingService__pb2.SendResponse.
            SerializeToString,
        ),
        'GetLoadedDatasetNames':
        grpc.unary_unary_rpc_method_handler(
            servicer.GetLoadedDatasetNames,
            request_deserializer=google_dot_protobuf_dot_empty__pb2.Empty.
            FromString,
            response_serializer=SendingService__pb2.DatasetNames.
            SerializeToString,
        ),
    }
    generic_handler = grpc.method_handlers_generic_handler(
        'sending.DatasetSender', rpc_method_handlers)
    server.add_generic_rpc_handlers((generic_handler, ))


# This class is part of an EXPERIMENTAL API.
class DatasetSender(object):
    """Missing associated documentation comment in .proto file."""

    @staticmethod
    def SendDataset(request_iterator,
                    target,
                    options=(),
                    channel_credentials=None,
                    call_credentials=None,
                    insecure=False,
                    compression=None,
                    wait_for_ready=None,
                    timeout=None,
                    metadata=None):
        return grpc.experimental.stream_unary(
            request_iterator, target, '/sending.DatasetSender/SendDataset',
            SendingService__pb2.DatasetShard.SerializeToString,
            SendingService__pb2.SendResponse.FromString, options,
            channel_credentials, insecure, call_credentials, compression,
            wait_for_ready, timeout, metadata)

    @staticmethod
    def GetLoadedDatasetNames(request,
                              target,
                              options=(),
                              channel_credentials=None,
                              call_credentials=None,
                              insecure=False,
                              compression=None,
                              wait_for_ready=None,
                              timeout=None,
                              metadata=None):
        return grpc.experimental.unary_unary(
            request, target, '/sending.DatasetSender/GetLoadedDatasetNames',
            google_dot_protobuf_dot_empty__pb2.Empty.SerializeToString,
            SendingService__pb2.DatasetNames.FromString, options,
            channel_credentials, insecure, call_credentials, compression,
            wait_for_ready, timeout, metadata)
