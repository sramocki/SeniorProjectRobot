"""Server run on each PiCar to listen to desktop application commands"""

from concurrent import futures
from time import sleep

import grpc
import cv2

import picar_pb2
import picar_pb2_grpc

#Variables to control this picar's behavior
mode = 0
throttle = 0
direction = 0

class PiCarServicer(picar_pb2_grpc.PiCarServicer):
	"""Provides methods that implement functionality of PiCar server."""
	
	global mode
	global throttle
	global direction
	
	def __init__(self):
		self.streaming = False
		self.camera = cv2.VideoCapture(0)
	
	def ReceiveConnection(self, request, context):
		"""Handshake between PiCar and desktop application"""
		print('Received connection request from %s' % request.message)
		#Send a ConnectAck message showing success
		return picar_pb2.ConnectAck(success=True)

	def SwitchMode(self, request, context):
		"""Changes the operating mode of the PiCar"""
		if (mode != request.mode):
			#If the reuqest iss for a different mode, send a success ack
			print('Switching mode from %s to %s' % (mode, request.mode))
			mode = request.mode
			return picar_pb2.ModeAck(success=True)
		else:
			#If the request is for the mode already in, send a failure ack
			print('Request received for mode %s, but already in that mode!' % (request.mode))
			return picar_pb2.ModeAck(success=False)

	def RemoteControl(self, request, context):
		"""Receive control data from desktop application"""
		#Clamp the input throttle and direction to [-1, 1]
		throttle = max(-1, min(request.throttle, 1))
		direction = max(-1, min(request.direction, 1))
		print('Setting wheels to %f throttle and %f steering' % (throttle, direction))
		return picar_pb2.Empty()
		
	def VideoStream(self, request, context):
		"""Send back images captured from webcam, encoded as jpeg"""
		self.streaming = True
		print('Starting video stream')
		
		while(self.streaming):
			grabbed, frame = self.camera.read() #Get the current frame
			frame = cv2.resize(frame, (640, 480)) #Resize it to 640x480
			encoded, buffer = cv2.imencode('.jpg', frame)
			bytes = buffer.tobytes()
			
			message = picar_pb2.ImageCapture(image=bytes) #Create message with image
			#print('Sending frame')
			yield message #Send it				
			sleep(1 / 30) #Wait for 1/30th of a sec
	
	def StopStream(self, request, context):
		"""Stop the sending of a video stream"""
		self.streaming = False
		print('Stopping video stream')
		return picar_pb2.Empty()
		
def serve():
	server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
	picar_pb2_grpc.add_PiCarServicer_to_server(
		PiCarServicer(), server)
	server.add_insecure_port('[::]:50051')
	server.start()
	print('PiCar server started.')
			
if __name__ == '__main__':
	serve();