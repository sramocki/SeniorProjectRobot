using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RobotClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Registration : Window
    {
        List<String[]> deviceStringList = new List<string[]>();
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private String selectedIP;
        public Registration()
        {
            InitializeComponent();
            deviceList.ItemsSource = null;

            
            deviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());
        }



        private void ButtonScan(object sender, RoutedEventArgs e)
        {

            //Send Packet to all cars to be idle

            deviceStringList.Clear();
            
            mainWindow.deviceListMain.Clear();
            var defaultGateway = "";
            foreach (NetworkInterface curInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (curInterface.OperationalStatus != OperationalStatus.Up) continue;
                foreach (GatewayIPAddressInformation gatewayOutput in curInterface.GetIPProperties()
                    .GatewayAddresses)
                {
                    defaultGateway = gatewayOutput.Address.ToString();
                }

            }

            defaultGateway = defaultGateway.Substring(0, (defaultGateway.Length - 1));
            Console.WriteLine(defaultGateway);

            //adjust as needed
            for (int i = 10; i < 20; i++)
            {
                Ping p = new Ping();
                PingReply rep = p.Send(defaultGateway + i);
                if (rep == null || rep.Status != IPStatus.Success) continue;
                Console.WriteLine("IP Found at " + defaultGateway + i);
                string deviceName;
                try
                {

                    deviceName = Dns.GetHostEntry(defaultGateway + i).HostName;

                }
                catch
                {
                    deviceName = "unknown"+i;

                }

                Console.WriteLine("Device Name is: " + deviceName);

                deviceStringList.Add(new[] { deviceName,defaultGateway + i, "Default" });
            }


            deviceList.ItemsSource = deviceStringList.Select(array => array.FirstOrDefault());

        }

        private void deviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            String deviceString = deviceList.SelectedItem.ToString();
            Console.WriteLine(deviceString);

            int index = deviceStringList.FindIndex(array => array[0] == deviceString);
            SelectedIP_Box.Text = deviceStringList[index][1].ToString();
            Console.WriteLine(index);

        }

        private void TryConnect(object sender, RoutedEventArgs e)
        {
            //try connect to selectedIP
            //if acknowledged, add to list on mainView
            String deviceString = deviceList.SelectedItem.ToString();

            int index = deviceStringList.FindIndex(array => array[0] == deviceString);

            mainWindow.deviceListMain.Add(new[] { deviceStringList[index][0], deviceStringList[index][1], deviceStringList[index][2] });
            mainWindow.deviceListMn.ItemsSource = mainWindow.deviceListMain.Select(array => array.FirstOrDefault());
        }
    }

}