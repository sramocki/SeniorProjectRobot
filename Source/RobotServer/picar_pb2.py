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
  package='SeniorProjectRobot',
  syntax='proto3',
  serialized_options=_b('\252\002\013RobotClient'),
  serialized_pb=_b('\n\x0bpicar.proto\x12\x12SeniorProjectRobot\"\x07\n\x05\x45mpty\"!\n\x0e\x43onnectRequest\x12\x0f\n\x07message\x18\x01 \x01(\t\"\x1d\n\nConnectAck\x12\x0f\n\x07success\x18\x01 \x01(\x08\"i\n\x0bModeRequest\x12\x32\n\x04mode\x18\x01 \x01(\x0e\x32$.SeniorProjectRobot.ModeRequest.Mode\"&\n\x04Mode\x12\x08\n\x04IDLE\x10\x00\x12\x08\n\x04LEAD\x10\x01\x12\n\n\x06\x46OLLOW\x10\x02\"\x1a\n\x07ModeAck\x12\x0f\n\x07success\x18\x01 \x01(\x08\"0\n\tSetMotion\x12\x10\n\x08throttle\x18\x01 \x01(\x01\x12\x11\n\tdirection\x18\x02 \x01(\x01\"\x12\n\x10StartVideoStream\"\x1d\n\x0cImageCapture\x12\r\n\x05image\x18\x01 \x01(\x0c\"\x10\n\x0e\x45ndVideoStream2\xa7\x03\n\x05PiCar\x12Y\n\x11ReceiveConnection\x12\".SeniorProjectRobot.ConnectRequest\x1a\x1e.SeniorProjectRobot.ConnectAck\"\x00\x12L\n\nSwitchMode\x12\x1f.SeniorProjectRobot.ModeRequest\x1a\x1b.SeniorProjectRobot.ModeAck\"\x00\x12K\n\rRemoteControl\x12\x1d.SeniorProjectRobot.SetMotion\x1a\x19.SeniorProjectRobot.Empty\"\x00\x12Y\n\x0bVideoStream\x12$.SeniorProjectRobot.StartVideoStream\x1a .SeniorProjectRobot.ImageCapture\"\x00\x30\x01\x12M\n\nStopStream\x12\".SeniorProjectRobot.EndVideoStream\x1a\x19.SeniorProjectRobot.Empty\"\x00\x42\x0e\xaa\x02\x0bRobotClientb\x06proto3')
)



_MODEREQUEST_MODE = _descriptor.EnumDescriptor(
  name='Mode',
  full_name='SeniorProjectRobot.ModeRequest.Mode',
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
  serialized_start=177,
  serialized_end=215,
)
_sym_db.RegisterEnumDescriptor(_MODEREQUEST_MODE)


_EMPTY = _descriptor.Descriptor(
  name='Empty',
  full_name='SeniorProjectRobot.Empty',
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
  serialized_start=35,
  serialized_end=42,
)


_CONNECTREQUEST = _descriptor.Descriptor(
  name='ConnectRequest',
  full_name='SeniorProjectRobot.ConnectRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='message', full_name='SeniorProjectRobot.ConnectRequest.message', index=0,
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
  serialized_start=44,
  serialized_end=77,
)


_CONNECTACK = _descriptor.Descriptor(
  name='ConnectAck',
  full_name='SeniorProjectRobot.ConnectAck',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='success', full_name='SeniorProjectRobot.ConnectAck.success', index=0,
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
  serialized_start=79,
  serialized_end=108,
)


_MODEREQUEST = _descriptor.Descriptor(
  name='ModeRequest',
  full_name='SeniorProjectRobot.ModeRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='mode', full_name='SeniorProjectRobot.ModeRequest.mode', index=0,
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
  serialized_start=110,
  serialized_end=215,
)


_MODEACK = _descriptor.Descriptor(
  name='ModeAck',
  full_name='SeniorProjectRobot.ModeAck',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='success', full_name='SeniorProjectRobot.ModeAck.success', index=0,
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
  serialized_start=217,
  serialized_end=243,
)


_SETMOTION = _descriptor.Descriptor(
  name='SetMotion',
  full_name='SeniorProjectRobot.SetMotion',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='throttle', full_name='SeniorProjectRobot.SetMotion.throttle', index=0,
      number=1, type=1, cpp_type=5, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR),
    _descriptor.FieldDescriptor(
      name='direction', full_name='SeniorProjectRobot.SetMotion.direction', index=1,
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
  serialized_start=245,
  serialized_end=293,
)


_STARTVIDEOSTREAM = _descriptor.Descriptor(
  name='StartVideoStream',
  full_name='SeniorProjectRobot.StartVideoStream',
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
  serialized_start=295,
  serialized_end=313,
)


_IMAGECAPTURE = _descriptor.Descriptor(
  name='ImageCapture',
  full_name='SeniorProjectRobot.ImageCapture',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  fields=[
    _descriptor.FieldDescriptor(
      name='image', full_name='SeniorProjectRobot.ImageCapture.image', index=0,
      number=1, type=12, cpp_type=9, label=1,
      has_default_value=False, default_value=_b(""),
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
  serialized_start=315,
  serialized_end=344,
)


_ENDVIDEOSTREAM = _descriptor.Descriptor(
  name='EndVideoStream',
  full_name='SeniorProjectRobot.EndVideoStream',
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
  serialized_start=346,
  serialized_end=362,
)

_MODEREQUEST.fields_by_name['mode'].enum_type = _MODEREQUEST_MODE
_MODEREQUEST_MODE.containing_type = _MODEREQUEST
DESCRIPTOR.message_types_by_name['Empty'] = _EMPTY
DESCRIPTOR.message_types_by_name['ConnectRequest'] = _CONNECTREQUEST
DESCRIPTOR.message_types_by_name['ConnectAck'] = _CONNECTACK
DESCRIPTOR.message_types_by_name['ModeRequest'] = _MODEREQUEST
DESCRIPTOR.message_types_by_name['ModeAck'] = _MODEACK
DESCRIPTOR.message_types_by_name['SetMotion'] = _SETMOTION
DESCRIPTOR.message_types_by_name['StartVideoStream'] = _STARTVIDEOSTREAM
DESCRIPTOR.message_types_by_name['ImageCapture'] = _IMAGECAPTURE
DESCRIPTOR.message_types_by_name['EndVideoStream'] = _ENDVIDEOSTREAM
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

Empty = _reflection.GeneratedProtocolMessageType('Empty', (_message.Message,), dict(
  DESCRIPTOR = _EMPTY,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.Empty)
  ))
_sym_db.RegisterMessage(Empty)

ConnectRequest = _reflection.GeneratedProtocolMessageType('ConnectRequest', (_message.Message,), dict(
  DESCRIPTOR = _CONNECTREQUEST,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.ConnectRequest)
  ))
_sym_db.RegisterMessage(ConnectRequest)

ConnectAck = _reflection.GeneratedProtocolMessageType('ConnectAck', (_message.Message,), dict(
  DESCRIPTOR = _CONNECTACK,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.ConnectAck)
  ))
_sym_db.RegisterMessage(ConnectAck)

ModeRequest = _reflection.GeneratedProtocolMessageType('ModeRequest', (_message.Message,), dict(
  DESCRIPTOR = _MODEREQUEST,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.ModeRequest)
  ))
_sym_db.RegisterMessage(ModeRequest)

ModeAck = _reflection.GeneratedProtocolMessageType('ModeAck', (_message.Message,), dict(
  DESCRIPTOR = _MODEACK,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.ModeAck)
  ))
_sym_db.RegisterMessage(ModeAck)

SetMotion = _reflection.GeneratedProtocolMessageType('SetMotion', (_message.Message,), dict(
  DESCRIPTOR = _SETMOTION,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.SetMotion)
  ))
_sym_db.RegisterMessage(SetMotion)

StartVideoStream = _reflection.GeneratedProtocolMessageType('StartVideoStream', (_message.Message,), dict(
  DESCRIPTOR = _STARTVIDEOSTREAM,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.StartVideoStream)
  ))
_sym_db.RegisterMessage(StartVideoStream)

ImageCapture = _reflection.GeneratedProtocolMessageType('ImageCapture', (_message.Message,), dict(
  DESCRIPTOR = _IMAGECAPTURE,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.ImageCapture)
  ))
_sym_db.RegisterMessage(ImageCapture)

EndVideoStream = _reflection.GeneratedProtocolMessageType('EndVideoStream', (_message.Message,), dict(
  DESCRIPTOR = _ENDVIDEOSTREAM,
  __module__ = 'picar_pb2'
  # @@protoc_insertion_point(class_scope:SeniorProjectRobot.EndVideoStream)
  ))
_sym_db.RegisterMessage(EndVideoStream)


DESCRIPTOR._options = None

_PICAR = _descriptor.ServiceDescriptor(
  name='PiCar',
  full_name='SeniorProjectRobot.PiCar',
  file=DESCRIPTOR,
  index=0,
  serialized_options=None,
  serialized_start=365,
  serialized_end=788,
  methods=[
  _descriptor.MethodDescriptor(
    name='ReceiveConnection',
    full_name='SeniorProjectRobot.PiCar.ReceiveConnection',
    index=0,
    containing_service=None,
    input_type=_CONNECTREQUEST,
    output_type=_CONNECTACK,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='SwitchMode',
    full_name='SeniorProjectRobot.PiCar.SwitchMode',
    index=1,
    containing_service=None,
    input_type=_MODEREQUEST,
    output_type=_MODEACK,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='RemoteControl',
    full_name='SeniorProjectRobot.PiCar.RemoteControl',
    index=2,
    containing_service=None,
    input_type=_SETMOTION,
    output_type=_EMPTY,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='VideoStream',
    full_name='SeniorProjectRobot.PiCar.VideoStream',
    index=3,
    containing_service=None,
    input_type=_STARTVIDEOSTREAM,
    output_type=_IMAGECAPTURE,
    serialized_options=None,
  ),
  _descriptor.MethodDescriptor(
    name='StopStream',
    full_name='SeniorProjectRobot.PiCar.StopStream',
    index=4,
    containing_service=None,
    input_type=_ENDVIDEOSTREAM,
    output_type=_EMPTY,
    serialized_options=None,
  ),
])
_sym_db.RegisterServiceDescriptor(_PICAR)

DESCRIPTOR.services_by_name['PiCar'] = _PICAR

# @@protoc_insertion_point(module_scope)
