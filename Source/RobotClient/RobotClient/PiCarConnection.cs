using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace RobotClient
{
    class PiCarConnection
    {
        private class PiCarClient
        {
            readonly PiCar.PiCarClient client;

            public PiCarClient(PiCar.PiCarClient client)
            {
                this.client = client;
            }

            //Request a connection to the PiCar server. Return success
            public bool requestConnect()
            {
                try
                {
                    //Attempt connection to PiCar server
                    ConnectRequest request = new ConnectRequest { Message = "Desktop App" };
                    ConnectAck ack = client.ReceiveConnection(request);

                    return ack.Success;
                }
                catch (RpcException e)
                {
                    Console.Write("RPC failed " + e);
                    throw;
                }
            }

            //Set the mode of the PiCar, return success
            public bool setMode(ModeRequest.Types.Mode mode)
            {
                try
                {
                    ModeRequest request = new ModeRequest { Mode = mode };
                    ModeAck ack = client.SwitchMode(request);

                    return ack.Success;
                }
                catch (RpcException e)
                {
                    Console.Write("RPC failed " + e);
                    throw;
                }
            }

            //Send a signal to the PiCar telling it how to move its wheels
            public void setMotion(double throttle, double direction)
            {
                try
                {
                    //Send a control signal to the PiCar
                    SetMotion request = new SetMotion { Throttle = throttle, Direction = direction };
                    client.RemoteControl(request);
                }
                catch (RpcException e)
                {
                    Console.Write("RPC failed " + e);
                    throw;
                }
            }
        }

        private Channel channel;
        private PiCarClient client;
        public string name;
        public ModeRequest.Types.Mode mode;

        public PiCarConnection(string name, string ipAddress)
        {
            this.name = name;
            channel = new Channel(ipAddress + ":50051", ChannelCredentials.Insecure);
            client = new PiCarClient(new PiCar.PiCarClient(channel));
            mode = ModeRequest.Types.Mode.Idle; //Start in Idle mode
        }

        public bool requestConnect()
        {
            return client.requestConnect();
        }

        public void setMode(ModeRequest.Types.Mode mode)
        {
            bool success = client.setMode(mode);
            //Change local mode if successful
            if (success)
                this.mode = mode;
        }

        public void setMotion(double throttle, double direction)
        {
            client.setMotion(throttle, direction);
        }

        public override string ToString()
        {
            return name;
        }
    }
}
