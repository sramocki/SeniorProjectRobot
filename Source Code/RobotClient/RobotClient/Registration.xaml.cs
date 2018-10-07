using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace RobotClient
{
    public partial class Registration : Window
    {
        List<String[]> deviceStringList = new List<string[]>();
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

        private string _defaultGateway;
        private string _scanningIp;

        private int timeout = 100;
        private int _devicesFound;

        static object _lockObj = new object();
        Stopwatch _stopWatch = new Stopwatch();
        TimeSpan _ts;


        public Registration()
        {
            InitializeComponent();
            deviceList.ItemsSource = null;

            //Finds default gateway IP
            deviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
            foreach (NetworkInterface curInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (curInterface.OperationalStatus != OperationalStatus.Up) continue;
                foreach (GatewayIPAddressInformation gatewayOutput in curInterface.GetIPProperties()
                    .GatewayAddresses)
                {
                    _defaultGateway = gatewayOutput.Address.ToString();
                }
            }
            _defaultGateway = _defaultGateway.Substring(0, (_defaultGateway.Length - 1));
            Console.WriteLine("The gateway IP is " + _defaultGateway);
        }

        private void ButtonScan(object sender, RoutedEventArgs e)
        {
            ScanDevicesAsync();  
        }

        public async void ScanDevicesAsync()
        {
            _devicesFound = 0;

            var tasks = new List<Task>();

            _stopWatch.Start();

            for (int i = 1; i <= 255; i++)
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
                catch (Exception e)
                {
                    deviceName = "unknown" + ip;
                }
                Console.WriteLine("Device Name is: " + deviceName);
                deviceStringList.Add(new[] { deviceName, ip, "Default" });
                deviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
                lock (_lockObj)
                {
                    _devicesFound++;
                }
            }
        }

        private void deviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            string deviceString = deviceList.SelectedItem.ToString();
            Console.WriteLine(deviceString);

            int index = deviceStringList.FindIndex(array => array[0] == deviceString);
            SelectedIP_Box.Text = deviceStringList[index][1].ToString();
            Console.WriteLine(index);

        }

        private void TryConnect(object sender, RoutedEventArgs e)
        {
            string selectedDevice = deviceList.SelectedItem.ToString();
            int index = deviceStringList.FindIndex(array => array[0] == selectedDevice);

            //This is the currently selected IP to check
            string selectedIP = deviceStringList[index][1];

            /**
             * Add logic for GRPC here.
             *
             * Need to confirm the client can connect to the pi server for the selected IP.
             * If it can connect, then the if statement runs and the pi server gets added to the main view for controlling.
             */
            bool CanConnect = true;
            if (CanConnect)
            {
                //Adds the device to the main view since it can connect
                _mainWindow.deviceListMain.Add(new[] { deviceStringList[index][0], deviceStringList[index][1], deviceStringList[index][2] });
                _mainWindow.deviceListMn.ItemsSource = _mainWindow.deviceListMain.Select(array => array.FirstOrDefault());
            }
        }
    }
}