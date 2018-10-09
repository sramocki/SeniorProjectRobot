import code

import grpc 

import picar_pb2
import picar_pb2_grpc

def requestconnect(stub, name):
	response = stub.ReceiveConnection(picar_pb2.ConnectRequest(message=name))
	print(response.success)	

def switchmode(stub, newmode):
	response = stub.SwitchMode(picar_pb2.ModeRequest(mode = newmode))
	print(response.success)

def setmotion(stub, newthrottle, newdirection):
	response = stub.RemoteControl(picar_pb2.SetMotion(throttle=newthrottle, direction=newdirection))

def run():
	stub = picar_pb2_grpc.PiCarStub(grpc.insecure_channel('localhost:50051'))
	requestconnect(stub, 'TESTING')
	switchmode(stub, 'LEAD')
	setmotion(stub, 1, 0)
	setmotion(stub, 1, 0.5)
	setmotion(stub, 0, 0)
	setmotion(stub, -1, 0)
		
if __name__ == '__main__':
	run()