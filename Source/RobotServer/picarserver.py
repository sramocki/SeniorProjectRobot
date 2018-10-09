"""Server run on each PiCar to listen to desktop application commands"""

from concurrent import futures
import time

import grpc

import picar_pb2
import picar_pb2_grpc

class PiCarServicer(picar_pb2_grpc.PiCarServicer):
	"""Provides methods that implement functionality of route guide server."""
	
	def __init__(self):
		self.mode = 'IDLE'
		self.throttle = 0
		self.direction = 0
	
	def ReceiveConnection(self, request, context):
		"""Handshake between PiCar and desktop application"""
		print('Received connection request from %s' % request.message)
		#Send a ConnectAck message showing success
		return picar_pb2.ConnectAck(success=True)

	def SwitchMode(self, request, context):
		"""Changes the operating mode of the PiCar"""
		if (self.mode != request.mode):
			#If the reuqest is for a different mode, send a success ack
			print('Switching mode from %s to %s' % (self.mode, request.mode))
			self.mode = request.mode
			return picar_pb2.ModeAck(success=True)
		else:
			#If the request is for the mode already in, send a failure ack
			print('Request received for mode %s, but already in that mode!' % (request.mode))
			return picar_pb2.ModeAck(success=False)

	def RemoteControl(self, request, context):
		"""Receive control data from desktop application"""
		#Clamp the input throttle and direction to [-1, 1]
		self.throttle = max(-1, min(request.throttle, 1))
		self.direction = max(-1, min(request.direction, 1))
		print('Setting wheels to %f throttle and %f steering' % (self.throttle, self.direction))
		return picar_pb2.Empty()
		
def serve():
	server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
	picar_pb2_grpc.add_PiCarServicer_to_server(
		PiCarServicer(), server)
	server.add_insecure_port('[::]:50051')
	server.start()
	print('PiCar server started.')
	try:
		while True:
			time.sleep(60 * 60 * 24)
	except KeyboardInterrupt:
			server.stop(0)
			
if __name__ == '__main__':
	serve();