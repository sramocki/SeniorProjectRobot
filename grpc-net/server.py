import time
from concurrent import futures

import grpc

import service_pb2 as service_pb2
import service_pb2_grpc as service_pb2_grpc

_ONE_DAY_IN_SECONDS = 60 * 60 * 24


class gRPCServer(service_pb2_grpc.MyServiceServicer):
    def __init__(self):
        print('initialization')

    def MyMethod1(self, request, context):
        print(request.name)
        print(request.code)
        return service_pb2.MyResponse(name=request.name, sex='M', code=123)

    def MyMethod2(self, request, context):
        print(request.name)
        print(request.code * 12)
        return service_pb2.MyResponse(name=request.name, sex='F', code=1234)

    def MyMethod3(self, request_iterator, context):
        for req in request_iterator:
            print(req.name)
            print(req.code)

            yield service_pb2.MyResponse(name=req.name, sex='M', code=123)


def serve():
    server = grpc.server(futures.ThreadPoolExecutor(max_workers=10))
    service_pb2_grpc.add_MyServiceServicer_to_server(
        gRPCServer(), server)
    server.add_insecure_port('[::]:50051')
    server.start()
    try:
        while True:
            time.sleep(_ONE_DAY_IN_SECONDS)
    except KeyboardInterrupt:
        server.stop(0)


if __name__ == '__main__':
    serve()