using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            List<String> ipStringList = new List<string>();
            for (int i = 1; i < 20; i++)
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply rep = p.Send("192.168.1."+i);
                if (rep.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    Console.WriteLine("IP Found at 192.168.1." + i);
                    ipStringList.Add("192.168.1." + i);
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
