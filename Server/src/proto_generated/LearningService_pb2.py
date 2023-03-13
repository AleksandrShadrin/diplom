# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: LearningService.proto
"""Generated protocol buffer code."""
from google.protobuf.internal import builder as _builder
from google.protobuf import descriptor as _descriptor
from google.protobuf import descriptor_pool as _descriptor_pool
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


from google.protobuf import empty_pb2 as google_dot_protobuf_dot_empty__pb2


DESCRIPTOR = _descriptor_pool.Default().AddSerializedFile(b'\n\x15LearningService.proto\x12\x08learning\x1a\x1bgoogle/protobuf/empty.proto\"#\n\x12LearningModelNames\x12\r\n\x05names\x18\x01 \x03(\t\"\x1d\n\x0c\x44\x61tasetNames\x12\r\n\x05names\x18\x01 \x03(\t\"0\n\rTrainedModels\x12\x1f\n\x06models\x18\x01 \x03(\x0b\x32\x0f.learning.Model\"\x1b\n\x0b\x44\x61tasetName\x12\x0c\n\x04name\x18\x01 \x01(\t\"F\n\x0cTrainedModel\x12\x14\n\x0c\x64\x61taset_name\x18\x01 \x01(\t\x12\x12\n\nmodel_name\x18\x02 \x01(\t\x12\x0c\n\x04uuid\x18\x03 \x01(\t\"1\n\x05Model\x12\x14\n\x0c\x64\x61taset_name\x18\x01 \x01(\t\x12\x12\n\nmodel_name\x18\x02 \x01(\t\"6\n\rTrainResponse\x12%\n\x06status\x18\x01 \x01(\x0e\x32\x15.learning.TrainStatus*%\n\x0bTrainStatus\x12\x0b\n\x07SUCCESS\x10\x00\x12\t\n\x05\x45RROR\x10\x01\x32\xa1\x02\n\x0fLearningService\x12N\n\x16GetLearningModelsNames\x12\x16.google.protobuf.Empty\x1a\x1c.learning.LearningModelNames\x12\x41\n\x0fGetDatasetNames\x12\x16.google.protobuf.Empty\x1a\x16.learning.DatasetNames\x12\x43\n\x10GetTrainedModels\x12\x16.google.protobuf.Empty\x1a\x17.learning.TrainedModels\x12\x36\n\nTrainModel\x12\x0f.learning.Model\x1a\x17.learning.TrainResponseb\x06proto3')

_builder.BuildMessageAndEnumDescriptors(DESCRIPTOR, globals())
_builder.BuildTopDescriptorsAndMessages(DESCRIPTOR, 'LearningService_pb2', globals())
if _descriptor._USE_C_DESCRIPTORS == False:

  DESCRIPTOR._options = None
  _TRAINSTATUS._serialized_start=390
  _TRAINSTATUS._serialized_end=427
  _LEARNINGMODELNAMES._serialized_start=64
  _LEARNINGMODELNAMES._serialized_end=99
  _DATASETNAMES._serialized_start=101
  _DATASETNAMES._serialized_end=130
  _TRAINEDMODELS._serialized_start=132
  _TRAINEDMODELS._serialized_end=180
  _DATASETNAME._serialized_start=182
  _DATASETNAME._serialized_end=209
  _TRAINEDMODEL._serialized_start=211
  _TRAINEDMODEL._serialized_end=281
  _MODEL._serialized_start=283
  _MODEL._serialized_end=332
  _TRAINRESPONSE._serialized_start=334
  _TRAINRESPONSE._serialized_end=388
  _LEARNINGSERVICE._serialized_start=430
  _LEARNINGSERVICE._serialized_end=719
# @@protoc_insertion_point(module_scope)