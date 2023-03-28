from proto_generated.Health_pb2_grpc import HealthServicer
from proto_generated.Health_pb2 import HealthCheckResponse, SERVING


class HealthService(HealthServicer):

    async def Check(self, request, context):
        """Return status of server"""

        return HealthCheckResponse(status=SERVING)