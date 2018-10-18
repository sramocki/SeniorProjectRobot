using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Grpc.Core;


namespace RobotClient
{
    public partial class Registration
    {
        List<string[]> deviceStringList = new List<string[]>();
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

        private string _defaultGateway;
        private string _scanningIp;

        private int timeout = 100;
        private int _devicesFound;

        static object _lockObj = new object();
        Stopwatch _stopWatch = new Stopwatch();
        TimeSpan _ts;


        /**
         *
         */
        public Registration()
        {
            InitializeComponent();
            DeviceList.ItemsSource = null;
            deviceStringList.Add(new[] { "Dummy", "127.0.0.1", "Default" });
            deviceStringList.Add(new[] { "Local Server", "127.0.0.1", "Default" });

            //Finds default gateway IP
            DeviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
            foreach (var curInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (curInterface.OperationalStatus != OperationalStatus.Up) continue;
                foreach (var gatewayOutput in curInterface.GetIPProperties()
                    .GatewayAddresses)
                {
                    _defaultGateway = gatewayOutput.Address.ToString();
                }
            }
            _defaultGateway = _defaultGateway.Substring(0, _defaultGateway.Length - 1);
            LogFieldReg.AppendText("The gateway IP is " + _defaultGateway + "\n");
        }

        /**
         *
         */
        private void ButtonScan(object sender, RoutedEventArgs e)
        {
            //Send cancellation token
            ScanDevicesAsync();
        }

        /**
         *
         */
        private void ButtonScanCancel(object sender, RoutedEventArgs e)
        {
            //TODO
            /* while (tasks.Any())
             {
                 tasks[0].Dispose();
                 tasks.RemoveAt(0);
             }
             LogFieldReg.AppendText(DateTime.Now + ":\t" + "Scan aborted \n");*/
        }

        /**
         *
         */
        public async void ScanDevicesAsync()
        {
            _devicesFound = 0;
            LogFieldReg.AppendText("Starting Scan for Devices: \n");

            var tasks = new List<Task>();

            _stopWatch.Start();

            for (var i = 1; i <= 255; i++)
            {
                _scanningIp = _defaultGateway + i;

                var p = new Ping();
                var task = AsyncUpdate(p, _scanningIp);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks).ContinueWith(t =>
            {
                _stopWatch.Stop();
                _ts = _stopWatch.Elapsed;
                MessageBox.Show(_devicesFound.ToString() + " local devices found. Scan time: " + _ts.ToString(), "Asynchronous");
            });
        }

        /**
         *
         */
        private async Task AsyncUpdate(Ping ping, string ip)
        {
            var response = await ping.SendPingAsync(ip, timeout);

            if (response.Status == IPStatus.Success)
            {
                string deviceName;
                try
                {
                    deviceName = Dns.GetHostEntry(ip).HostName;
                }
                catch (SocketException e)
                {
                    deviceName = "Unknown Device at " + ip;
                    Console.WriteLine(e);
                }
                LogFieldReg.AppendText(deviceName + " at " + ip + "\n");
                deviceStringList.Add(new[] { deviceName, ip, "Default" });
                DeviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
                lock (_lockObj)
                {
                    _devicesFound++;
                }
            }
        }

        /**
         *
         */
        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var deviceString = DeviceList.SelectedItem.ToString();
            Console.WriteLine(deviceString);
            var index = deviceStringList.FindIndex(array => array[0] == deviceString);
            SelectedIpBox.Text = deviceStringList[index][1];
            Console.WriteLine(index);

        }

        /**
         *
         */
        private void TryConnect(object sender, RoutedEventArgs e)
        {

            var selectedDevice = DeviceList.SelectedItem.ToString();
            var index = deviceStringList.FindIndex(array => array[0] == selectedDevice);

            //This is the currently selected IP to check
            var selectedIP = deviceStringList[index][1];
            var selectedName = deviceStringList[index][0];

            //Handle the dummy connection
            if (selectedIP == "127.0.0.1" & selectedName == "Dummy")
            {
                _mainWindow.LogField.AppendText(DateTime.Now + ":\tAdded dummy device for testing\n");
                var dummyConnection = new DummyConnection("Dummy", "127.0.0.1");
                _mainWindow.deviceListMain.Add(dummyConnection);
                _mainWindow.DeviceListMn.ItemsSource = _mainWindow.deviceListMain;
                return;
            }

            //Handle a regular connection
            var canConnect = false;
            PiCarConnection newConnection = null;

            try
            {
                newConnection = new PiCarConnection(selectedName, selectedIP);
                canConnect = newConnection.requestConnect();
            }
            catch (RpcException rpcE)
            {
                Console.WriteLine(rpcE);
            }

            if (canConnect)
            {
                _mainWindow.LogField.AppendText(DateTime.Now + ":\t" + "Connected to " + selectedName + " with IP: " + selectedIP + "\n");
                _mainWindow.deviceListMain.Add(newConnection);
                _mainWindow.DeviceListMn.ItemsSource = _mainWindow.deviceListMain;
            }
            else
            {
                _mainWindow.LogField.AppendText(DateTime.Now + ":\t" + "Failed to connect to " + selectedName + " with IP: " + selectedIP + "\n");
            }
        }
    }
}