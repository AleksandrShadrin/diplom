# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: ModelsService.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x13ModelsService.proto\x12\nprediction\"\x15\n\x05Model\x12\x0c\n\x04uuid\x18\x01 \x01(\t\"t\n\x16ModelPredictionRequest\x12\x38\n\x12time_series_values\x18\x01 \x01(\x0b\x32\x1c.prediction.TimeSeriesValues\x12 \n\x05model\x18\x02 \x01(\x0b\x32\x11.prediction.Model\"\"\n\x10TimeSeriesValues\x12\x0e\n\x06values\x18\x01 \x03(\x01\"0\n\x17ModelPredictionResponse\x12\x15\n\rclassified_as\x18\x01 \x01(\t2\xd2\x01\n\rModelsService\x12]\n\x12GetModelPrediction\x12\".prediction.ModelPredictionRequest\x1a#.prediction.ModelPredictionResponse\x12\x62\n\x13GetModelPredictions\x12\".prediction.ModelPredictionRequest\x1a#.prediction.ModelPredictionResponse(\x01\x30\x01\x62\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'ModelsService_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  _MODEL._serialized_start=35
  _MODEL._serialized_end=56
  _MODELPREDICTIONREQUEST._serialized_start=58
  _MODELPREDICTIONREQUEST._serialized_end=174
  _TIMESERIESVALUES._serialized_start=176
  _TIMESERIESVALUES._serialized_end=210
  _MODELPREDICTIONRESPONSE._serialized_start=212
  _MODELPREDICTIONRESPONSE._serialized_end=260
  _MODELSSERVICE._serialized_start=263
  _MODELSSERVICE._serialized_end=473
# @@protoc_insertion_point(module_scope)
