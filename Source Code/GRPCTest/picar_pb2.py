# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: picar.proto

import sys
_b=sys.version_info[0]<3 and (lambda x:x) or (lambda x:x.encode('latin1'))
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()




DESCRIPTOR = _descriptor.FileDescriptor(
  name='picar.proto',
  package='',
  syntax='proto3',
  serialized_options=None,
  serialized_pb=_b('\n\x0bpicar.proto\"\x07\n\x05\x45mpty\"!\n\x0e\x43onnectRequest\x12\x0f\n\x07message\x18\x01 \x01(\t\"\x1d\n\nConnectAck\x12\x0f\n\x07success\x18\x01 \x01(\x08\"V\n\x0bModeRequest\x12\x1f\n\x04mode\x18\x01 \x01(\x0e\x32\x11.ModeRequest.Mode\"&\n\x04Mode\x12\x08\n\x04IDLE\x10\x00\x12\x08\n\x04LEAD\x10\x01\x12\n\n\x06\x46OLLOW\x10\x02\"\x1a\n\x07ModeAck\x12\x0f\n\x07success\x18\x01 \x01(\x08\"0\n\tSetMotion\x12\x10\n\x08throttle\x18\x01 \x01(\x01\x12\x11\n\tdirection\x18\x02 \x01(\x01\x32\x8b\x01\n\x05PiCar\x12\x33\n\x11ReceiveConnection\x12\x0f.ConnectRequest\x1a\x0b.ConnectAck\"\x00\x12&\n\nSwitchMode\x12\x0c.ModeRequest\x1a\x08.ModeAck\"\x00\x12%\n\rRemoteControl\x12\n.SetMotion\x1a\x06.Empty\"\x00\x62\x06proto3')
)



_MODEREQUEST_MODE = _descriptor.EnumDescriptor(
  name='Mode',
  full_name='ModeRequest.Mode',
  filename=None,
  file=DESCRIPTOR,
  values=[
    _descriptor.EnumValueDescriptor(
      name='IDLE', index=0, number=0,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='LEAD', index=1, number=1,
      serialized_options=None,
      type=None),
    _descriptor.EnumValueDescriptor(
      name='FOLLOW', index=2, number=2,
      serialized_options=None,
      type=None),
  ],
  containing_type=None,
  serialized_options=None,
  serialized_start=138,
  serialized_end=176,
)
_sym_db.RegisterEnumDescriptor(_MODEREQUEST_MODE)


_EMPTY = _descriptor.Descriptor(
  name='Empty',
  full_name='Empty',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=15,
  serialized_end=22,
)


_CONNECTREQUEST = _descriptor.Descriptor(
  name='ConnectRequest',
  full_name='ConnectRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='message', full_name='ConnectRequest.message', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=_b("").decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=24,
  serialized_end=57,
)


_CONNECTACK = _descriptor.Descriptor(
  name='ConnectAck',
  full_name='ConnectAck',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='success', full_name='ConnectAck.success', index=0,
      number=1, type=8, cpp_type=7, label=1,
      has_default_value=False, default_value=False,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=59,
  serialized_end=88,
)


_MODEREQUEST = _descriptor.Descriptor(
  name='ModeRequest',
  full_name='ModeRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='mode', full_name='ModeRequest.mode', index=0,
      number=1, type=14, cpp_type=8, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
    _MODEREQUEST_MODE,
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=90,
  serialized_end=176,
)


_MODEACK = _descriptor.Descriptor(
  name='ModeAck',
  full_name='ModeAck',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='success', full_name='ModeAck.success', index=0,
      number=1, type=8, cpp_type=7, label=1,
      has_default_value=False, default_value=False,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=178,
  serialized_end=204,
)


_SETMOTION = _descriptor.Descriptor(
  name='SetMotion',
  full_name='SetMotion',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='throttle', full_name='SetMotion.throttle', index=0,
      number=1, type=1, cpp_type=5, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='direction', full_name='SetMotion.direction', index=1,
      number=2, type=1, cpp_type=5, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=206,
  serialized_end=254,
)

_MODEREQUEST.fields_by_name['mode'].enum_type = _MODEREQUEST_MODE
_MODEREQUEST_MODE.containing_type = _MODEREQUEST
DESCRIPTOR.message_types_by_name['Empty'] = _EMPTY
DESCRIPTOR.message_types_by_name['ConnectRequest'] = _CONNECTREQUEST
DESCRIPTOR.message_types_by_name['ConnectAck'] = _CONNECTACK
DESCRIPTOR.message_types_by_name['ModeRequest'] = _MODEREQUEST
DESCRIPTOR.message_types_by_name['ModeAck'] = _MODEACK
DESCRIPTOR.message_types_by_name['SetMotion'] = _SETMOTION
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

Empty = _reflection.GeneratedProtocolMessageType('Empty', (_message.Message,), dict(
  DESCRIPTOR = _EMPTY,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:Empty)
  ))
_sym_db.RegisterMessage(Empty)

ConnectRequest = _reflection.GeneratedProtocolMessageType('ConnectRequest', (_message.Message,), dict(
  DESCRIPTOR = _CONNECTREQUEST,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:ConnectRequest)
  ))
_sym_db.RegisterMessage(ConnectRequest)

ConnectAck = _reflection.GeneratedProtocolMessageType('ConnectAck', (_message.Message,), dict(
  DESCRIPTOR = _CONNECTACK,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:ConnectAck)
  ))
_sym_db.RegisterMessage(ConnectAck)

ModeRequest = _reflection.GeneratedProtocolMessageType('ModeRequest', (_message.Message,), dict(
  DESCRIPTOR = _MODEREQUEST,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:ModeRequest)
  ))
_sym_db.RegisterMessage(ModeRequest)

ModeAck = _reflection.GeneratedProtocolMessageType('ModeAck', (_message.Message,), dict(
  DESCRIPTOR = _MODEACK,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:ModeAck)
  ))
_sym_db.RegisterMessage(ModeAck)

SetMotion = _reflection.GeneratedProtocolMessageType('SetMotion', (_message.Message,), dict(
  DESCRIPTOR = _SETMOTION,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SetMotion)
  ))
_sym_db.RegisterMessage(SetMotion)



_PICAR = _descriptor.ServiceDescriptor(
  name='PiCar',
  full_name='PiCar',
  file=DESCRIPTOR,
  index=0,
  serialized_options=None,
  serialized_start=257,
  serialized_end=396,
  methods=[
  _descriptor.MethodDescriptor(
    name='ReceiveConnection',
    full_name='PiCar.ReceiveConnection',
    index=0,
    containing_service=None,
    input_type=_CONNECTREQUEST,
    output_type=_CONNECTACK,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='SwitchMode',
    full_name='PiCar.SwitchMode',
    index=1,
    containing_service=None,
    input_type=_MODEREQUEST,
    output_type=_MODEACK,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='RemoteControl',
    full_name='PiCar.RemoteControl',
    index=2,
    containing_service=None,
    input_type=_SETMOTION,
    output_type=_EMPTY,
    serialized_options=None,
  ),
])
_sym_db.RegisterServiceDescriptor(_PICAR)

DESCRIPTOR.services_by_name['PiCar'] = _PICAR

# @@protoc_insertion_point(module_scope)
