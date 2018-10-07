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
        List<String[]> ipStringList = new List<string[]>();
        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public Registration()
        {
            InitializeComponent();
            ipList.ItemsSource = null;

            ipStringList = mainWindow.LeaderIpList;
            ipList.ItemsSource = ipStringList.Select(array => array.FirstOrDefault());
        }



        private void ButtonScan(object sender, RoutedEventArgs e)
        {

            ipStringList.Clear();
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
            
            for (int i = 1; i < 20; i++)
            {
                Ping p = new System.Net.NetworkInformation.Ping();
                PingReply rep = p.Send(defaultGateway + i);
                if (rep.Status == IPStatus.Success)
                {
                    Console.WriteLine("IP Found at " + defaultGateway + i);
                    String deviceName = "";
                    try
                    {
                       
                        deviceName = Dns.GetHostEntry(defaultGateway + i).HostName.ToString();
                        
                    }
                    catch
                    {
                        deviceName = "unknown";

                    }

                    Console.WriteLine("Device Name is: " + deviceName);

                    ipStringList.Add(new String[] { defaultGateway + i, deviceName, "Default" });
                }
            }


            ipList.ItemsSource = ipStringList.Select(array => array.FirstOrDefault());
            mainWindow.LeaderIpList = ipStringList;

        }

        private void ipList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


                String ipString = ipList.SelectedItem.ToString();

            int index = ipStringList.FindIndex(array => array[0] == ipString);
            //Console.WriteLine(index); Line used for Testing

            macAddressBox.Text= ipStringList[index][1].ToString();
            roleBox.Text = ipStringList[index][2].ToString();


        }
        private void LeaderButton(object sender, RoutedEventArgs e)
        {
            String ipString = ipList.SelectedItem.ToString();

            int index = ipStringList.FindIndex(array => array[0] == ipString);

            ipStringList[index][2] = "Leader";

            roleBox.Text = ipStringList[index][2].ToString();

            mainWindow.IPTextBlock.Text = ipList.SelectedItem.ToString();
            mainWindow.LeaderIpList = ipStringList;


        }

        private void FollowerRoleButton(object sender, RoutedEventArgs e)
        {

            String ipString = ipList.SelectedItem.ToString();

            int index = ipStringList.FindIndex(array => array[0] == ipString);

            ipStringList[index][2] = "Follower";

            roleBox.Text = ipStringList[index][2].ToString();
            // mainWindow.IPTextBlock.Text = ipList.SelectedItem.ToString();
            // mainWindow.FollowerIP = ipList.SelectedItem.ToString();
            mainWindow.LeaderIpList = ipStringList;

        }

        private void DefaultRoleButton(object sender, RoutedEventArgs e)
        {

            String ipString = ipList.SelectedItem.ToString();

            int index = ipStringList.FindIndex(array => array[0] == ipString);

            ipStringList[index][2] = "Default";

            roleBox.Text = ipStringList[index][2].ToString();
            // mainWindow.IPTextBlock.Text = ipList.SelectedItem.ToString();
            // mainWindow.FollowerIP = ipList.SelectedItem.ToString();
            mainWindow.LeaderIpList = ipStringList;

        }
    }

}