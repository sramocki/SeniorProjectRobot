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
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public Registration()
        {
            InitializeComponent();
            ipList.ItemsSource = null;
        }

        private void ExampleButton(object sender, RoutedEventArgs e)
        {
            mainWindow.IPTextBlock.Text = "POOP";
            mainWindow.LeaderIp = "POOP";

            mainWindow.FollowerIP = "";
        }

        private void ButtonScan(object sender, RoutedEventArgs e)
        {
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
            List<String> ipStringList = new List<string>();
            for (int i = 1; i < 20; i++)
            {
                Ping p = new System.Net.NetworkInformation.Ping();
                PingReply rep = p.Send(defaultGateway + i);
                if (rep.Status == IPStatus.Success)
                {
                    Console.WriteLine("IP Found at " + defaultGateway + i);
                    ipStringList.Add(defaultGateway + i);
                }
            }
            ipList.ItemsSource = ipStringList;
        }

        private void ipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                String ipAddress = ipList.SelectedItem.ToString();
                String machineName = Dns.GetHostEntry(ipAddress).HostName.ToString();
                macAddressBox.Text = machineName;
            }
            catch
            {
                macAddressBox.Text = "unknown";

            }

        }
        private void LeaderButton(object sender, RoutedEventArgs e)
        {


            mainWindow.IPTextBlock.Text = ipList.SelectedItem.ToString();
            mainWindow.LeaderIp = ipList.SelectedItem.ToString();


        }

        private void FollowerRoleButton(object sender, RoutedEventArgs e)
        {


            mainWindow.IPTextBlock.Text = ipList.SelectedItem.ToString();
            mainWindow.FollowerIP = ipList.SelectedItem.ToString();


        }

        private void DefaultRoleButton(object sender, RoutedEventArgs e)
        {


            mainWindow.IPTextBlock.Text = ipList.SelectedItem.ToString();
            mainWindow.FollowerIP = ipList.SelectedItem.ToString();


        }
    }

}