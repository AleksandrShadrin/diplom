# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: Health.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x0cHealth.proto\x12\x06health\"\x14\n\x12HealthCheckRequest\"<\n\x13HealthCheckResponse\x12%\n\x06status\x18\x01 \x01(\x0e\x32\x15.health.ServingStatus*-\n\rServingStatus\x12\x0b\n\x07SERVING\x10\x00\x12\x0f\n\x0bNOT_SERVING\x10\x01\x32J\n\x06Health\x12@\n\x05\x43heck\x12\x1a.health.HealthCheckRequest\x1a\x1b.health.HealthCheckResponseb\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'Health_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  _SERVINGSTATUS._serialized_start=108
  _SERVINGSTATUS._serialized_end=153
  _HEALTHCHECKREQUEST._serialized_start=24
  _HEALTHCHECKREQUEST._serialized_end=44
  _HEALTHCHECKRESPONSE._serialized_start=46
  _HEALTHCHECKRESPONSE._serialized_end=106
  _HEALTH._serialized_start=155
  _HEALTH._serialized_end=229
# @@protoc_insertion_point(module_scope)
