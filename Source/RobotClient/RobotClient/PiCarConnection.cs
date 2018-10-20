﻿using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace RobotClient
{
    public class PiCarConnection
    {
        private class PiCarClient
        {
            private readonly PiCar.PiCarClient _client;

            public PiCarClient(PiCar.PiCarClient client)
            {
                _client = client;
            }

            //Request a connection to the PiCar server. Return success
            public bool RequestConnect()
            {
                try
                {
                    //Attempt connection to PiCar server
                    var request = new ConnectRequest {Message = "Desktop App"};
                    var ack = _client.ReceiveConnection(request);

                    return ack.Success;
                }
                catch (RpcException e)
                {
                    Console.Write("RPC failed " + e);
                    throw;
                }
            }

            //Set the mode of the PiCar, return success
            public bool SetMode(ModeRequest.Types.Mode mode)
            {
                try
                {
                    var request = new ModeRequest {Mode = mode};
                    var ack = _client.SwitchMode(request);

                    return ack.Success;
                }
                catch (RpcException e)
                {
                    Console.Write("RPC failed " + e);
                    throw;
                }
            }

            //Send a signal to the PiCar telling it how to move its wheels
            public void SetMotion(double throttle, double direction)
            {
                try
                {
                    //Send a control signal to the PiCar
                    var request = new SetMotion {Throttle = throttle, Direction = direction};
                    _client.RemoteControl(request);
                }
                catch (RpcException e)
                {
                    Console.Write("RPC failed " + e);
                    throw;
                }
            }
        }

        private Channel _channel;
        private PiCarClient _client;
        public string Name;
        public string ipAddress;
        public ModeRequest.Types.Mode Mode;

        public PiCarConnection()
        {
            Name = "Default";
            ipAddress = "127.0.0.1";
            Mode = ModeRequest.Types.Mode.Idle;
        }

        public PiCarConnection(string name, string ipAddress)
        {
            this.Name = name;
            this.ipAddress = ipAddress;
            _channel = new Channel(ipAddress + ":50051", ChannelCredentials.Insecure);
            _client = new PiCarClient(new PiCar.PiCarClient(_channel));
            Mode = ModeRequest.Types.Mode.Idle; //Start in Idle mode
        }

        public virtual bool RequestConnect()
        {
            return _client.RequestConnect();
        }

        public virtual void SetMode(ModeRequest.Types.Mode mode)
        {
            var success = _client.SetMode(mode);
            //Change local mode if successful
            if (success)
                Mode = mode;
        }

        public virtual void SetMotion(double throttle, double direction)
        {
            _client.SetMotion(throttle, direction);
        }

        public override string ToString()
        {
            return Name;
        }

        public async Task Shutdown()
        {
            await _channel.ShutdownAsync();
        }
    }

    public class DummyConnection : PiCarConnection
    {
        public DummyConnection(string name, string ipAddress)
        {
            Name = name;
            this.ipAddress = ipAddress;
            Mode = ModeRequest.Types.Mode.Idle;
        }

        public override bool RequestConnect()
        {
            return true;
        }

        public override void SetMode(ModeRequest.Types.Mode mode)
        {
            Mode = mode;
        }

        public override void SetMotion(double throttle, double direction)
        {
            Console.WriteLine(Name + " Moving");
        }
    }
}