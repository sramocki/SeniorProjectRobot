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
using System.ComponentModel;

namespace RobotClient
{
    public partial class Registration
    {
        private List<string[]> deviceStringList = new List<string[]>();
        private readonly MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;

        private readonly string _defaultGateway;
        private string _scanningIp;

        private readonly int _timeout = 2000;
        private int _devicesFound;

        private static readonly object LockObj = new object();
        private TimeSpan _ts;
        private readonly BackgroundWorker _backgroundWorker1 = new BackgroundWorker();


        /**
         *
         */
        private void InitializeBackgroundWorker()
        {
            _backgroundWorker1.DoWork += BackgroundWorker1_DoWorkAsync;
            
        }

        /**
         *
         */
        public Registration()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            _backgroundWorker1.WorkerSupportsCancellation = true;
            

            CancelButton.IsEnabled = false;

            //Finds default gateway IP
            DeviceList.ItemsSource = null;
            DeviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
            try
            {
                foreach (var curInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (curInterface.OperationalStatus != OperationalStatus.Up) continue;
                    foreach (var gatewayOutput in curInterface.GetIPProperties()
                        .GatewayAddresses)
                    {
                        _defaultGateway = gatewayOutput.Address.ToString();
                    }
                }
            }
            catch(NetworkInformationException exception)
            {
                LogFieldReg.AppendText(DateTime.Now + ":\tDevice not connected to the internet! " + exception);
                this.Close();
            }
            
            _defaultGateway = _defaultGateway.Substring(0, _defaultGateway.Length - 1);
            LogFieldReg.AppendText("The gateway IP is " + _defaultGateway + "x\n");
            _mainWindow.LogField.AppendText(DateTime.Now + ":\tThe gateway IP is " + _defaultGateway + "x\n");

        }

        /**
         *
         */
        private void ButtonScan(object sender, RoutedEventArgs e)
        {
            deviceStringList.Clear();
            DeviceList.ItemsSource = null;
            deviceStringList.Add(new[] { "Dummy1", "N/A", "Default" });
            deviceStringList.Add(new[] { "Dummy2", "N/A", "Default" });
            deviceStringList.Add(new[] { "Dummy3", "N/A", "Default" });
            deviceStringList.Add(new[] { "Local Server", "127.0.0.1", "Default" });
            DeviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
            _ts = TimeSpan.Zero;
            _backgroundWorker1.RunWorkerAsync();
        }

        /**
         *
         */
        private async void BackgroundWorker1_DoWorkAsync(object sender, DoWorkEventArgs e)
        {
            var tasks = new List<Task>();
            _devicesFound = 0;
            Dispatcher.Invoke(() =>
            {
                LogFieldReg.AppendText("Starting scan\n");
                _mainWindow.LogField.AppendText(DateTime.Now + ":\tStarting scan\n");
                buttonScan.IsEnabled = false;
                CancelButton.IsEnabled = true;
            });
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (var i = 1; i <= 255; i++)
            {
                _scanningIp = _defaultGateway + i;
                var p = new Ping();

                var task = AsyncUpdate(p, _scanningIp);
                tasks.Add(task);

                if (!_backgroundWorker1.CancellationPending) continue;
                Dispatcher.Invoke(() =>
                {
                    tasks.Clear();
                    LogFieldReg.AppendText("Scan aborted \n");
                    _mainWindow.LogField.AppendText("Scan aborted\n");
                });
                break;
            }
            await Task.WhenAll(tasks);

            stopWatch.Stop();
            _ts = stopWatch.Elapsed;           

            Dispatcher.Invoke(() =>
            {
                LogFieldReg.AppendText("Finsished scan (" + _ts.TotalSeconds + " seconds)\nDetected " + _devicesFound + " devices\n");
                _mainWindow.LogField.AppendText(DateTime.Now + ":\tFinsished scan: " + _ts.TotalSeconds + " seconds\n" + DateTime.Now + "\tDetected " + _devicesFound + " devices\n");
                buttonScan.IsEnabled = true;
                CancelButton.IsEnabled = false;
            });

        }

        /**
         *
         */
        private async Task AsyncUpdate(Ping ping, string ip)
        {
            var response = await ping.SendPingAsync(ip, _timeout);

            if (response.Status == IPStatus.Success)
            {
                string deviceName;
                try
                {
                    deviceName = Dns.GetHostEntry(ip).HostName;
                }
                catch (SocketException eS)
                {
                    deviceName = "Unknown @ " + ip;
                    Console.WriteLine(eS);
                }

                Dispatcher.Invoke(() =>
                {
                    deviceStringList.Add(new[] { deviceName, ip, "Default" });
                    DeviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
                    lock (LockObj)
                    {
                        _devicesFound++;
                    }
                });
        }
        }

        /**
         *
         */
        private void ButtonScanCancel(object sender, RoutedEventArgs e) => _backgroundWorker1.CancelAsync();

        /**
         *
         */
        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DeviceList.SelectedItem == null)
                return;
            var deviceString = DeviceList.SelectedItem.ToString();
            Console.WriteLine(deviceString);
            var index = deviceStringList.FindIndex(array => array[0] == deviceString);
            SelectedIpBox.Text = deviceStringList[index][1];
        }

        /**
         *
         */
        private void TryConnect(object sender, RoutedEventArgs e)
        {
            var selectedIP = SelectedIpBox.Text;
            var random = new Random();
            var randomNumber = random.Next(1, 256);
            var selectedName = "Unknown" + randomNumber;
            if (DeviceList.SelectedItem != null)
            {
                var selectedDevice = DeviceList.SelectedItem.ToString();
                var index = deviceStringList.FindIndex(array => array[0] == selectedDevice);
                selectedIP = deviceStringList[index][1];
                selectedName = deviceStringList[index][0];
            }

            //Handle the dummy connection
            if (selectedIP == "N/A")
            {
                _mainWindow.LogField.AppendText(DateTime.Now + ":\tAdded " + selectedName + "for testing\n");
                var dummyConnection = new DummyConnection(selectedName, selectedIP);
                _mainWindow.deviceListMain.Add(dummyConnection);
            }

            if (!CheckIfValidIP(selectedIP))
            {
                LogFieldReg.AppendText("Invalid IP used, try again!\n");
                _mainWindow.LogField.AppendText(DateTime.Now + ":\tInvalid IP used, try again!\n");
            }

            else
            {
                PiCarConnection newConnection = null;
                var canConnect = false;
                try
                {
                    newConnection = new PiCarConnection(selectedName, selectedIP);
                    canConnect = newConnection.RequestConnect();
                }
                catch (RpcException rpcE)
                {
                    _mainWindow.LogField.AppendText(DateTime.Now + ":\tError! " + rpcE);
                }
                catch(Exception exception)
                {
                    _mainWindow.LogField.AppendText(DateTime.Now + ":\tError! " + exception);
                }

                if (canConnect)
                {
                    _mainWindow.LogField.AppendText(DateTime.Now + ":\t" + "Connected to " + selectedName + " with IP: " + selectedIP + "\n");
                    LogFieldReg.AppendText("Connected to " + selectedName + " with IP: " + selectedIP + "\n");
                    _mainWindow.deviceListMain.Add(newConnection);
                }
                else
                {
                    _mainWindow.LogField.AppendText(DateTime.Now + ":\t" + "Failed to connect to " + selectedName + " with IP: " + selectedIP + "\n");
                    LogFieldReg.AppendText("Failed to connect to " + selectedName + " with IP: " + selectedIP + "\n");
                }
            }

            _mainWindow.DeviceListMn.ItemsSource = null;
            _mainWindow.DeviceListMn.ItemsSource = _mainWindow.deviceListMain;
            _mainWindow.LogField.ScrollToEnd();
        }

        private static bool CheckIfValidIP(string localIP)
        {
            if (string.IsNullOrWhiteSpace(localIP))
                return false;

            var temp = localIP.Split('.');
            return temp.Length == 4 && temp.All(r => byte.TryParse(r, out var tempForParsing));
        }
    }
}