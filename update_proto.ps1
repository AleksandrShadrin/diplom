.\Server\venv\Scripts\activate
python -m grpc_tools.protoc -I.\Protobuf --python_out=.\Server\src\proto_generated --pyi_out=.\Server\src\proto_generated --grpc_python_out=.\Server\src\proto_generated .\Protobuf\SendingService.proto
deactivate