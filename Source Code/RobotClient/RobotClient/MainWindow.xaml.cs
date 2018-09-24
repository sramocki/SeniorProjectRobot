using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RobotClient
{

    public partial class MainWindow : Window
    {
        public string LeaderIp { set; get; }
        public string FollowerIP { set; get; }

        //unused for now
        public string LeaderMAC;
        public string FollowerMAC;

        private bool _dragging;
        private double sliderSpeed = 0;


        public MainWindow()
        {
            InitializeComponent();
            LeaderIp = "Empty";
            //Title = "Welcome " + Environment.UserName;

            var newKeybind = new RoutedCommand();
            newKeybind.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newKeybind, Register_Click));
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var register = new Registration();
            register.Show();
        }

        private void ExportData_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }

        private void ButtonPress_Event(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            switch (button.Content)
            {
                case "Forward":
                    LogField.AppendText("Moving forward at " + sliderSpeed + "\n");
                    //send data here
                    break;

                case "Backwards":
                    LogField.AppendText("Moving backwards at " + sliderSpeed + "\n");
                    break;

                case "Left":
                    LogField.AppendText("Moving left at " + sliderSpeed + "\n");
                    break;

                case "Right":
                    LogField.AppendText("Moving right at " + sliderSpeed + "\n");
                    break;

                default:
                    Console.WriteLine("Mistakes were made");
                    //add exception here
                    break;

            }

            //make leaderIP an array since there can be more than one leader at a time
            //this transmission needs to be on its own thread so the packets can be sent simultaneously
            //https://gist.github.com/leandrosilva/656054
            //https://stackoverflow.com/questions/21549963/tcp-server-c-sharp-on-windows-tcp-client-python-raspberry-pi
            //https://stackoverflow.com/questions/2637697/sending-udp-packet-in-c-sharp
            //https://codereview.stackexchange.com/questions/147606/simple-multi-client-echo-server

            SendUdp(11000, LeaderIp, 11000, Encoding.ASCII.GetBytes("Hello!"));
            LogField.ScrollToEnd();
        }

        static void SendUdp(int srcPort, string dstIp, int dstPort, byte[] data)
        {
            using (UdpClient c = new UdpClient(srcPort))
                c.Send(data, data.Length, dstIp, dstPort);
        }

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _dragging = false;
            if (!_dragging)
            {
                var slider = sender as Slider;
                sliderSpeed = Math.Ceiling(slider.Value);
                LogField.AppendText("Setting speed at " + sliderSpeed + "\n");
                LogField.ScrollToEnd();
            }
        }

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            _dragging = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_dragging)
            {
                var slider = sender as Slider;
                sliderSpeed = Math.Ceiling(slider.Value);
                LogField.AppendText("Setting speed at " + sliderSpeed + "\n");
                LogField.ScrollToEnd();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            //TODO rewrite for tutorial information/team
            MessageBox.Show("Created by Sean Ramocki\n\nsramocki@gmail.com", "About");
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this program", "Confirmation", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) ==
                MessageBoxResult.Yes)
                Application.Current.Shutdown();
        }
    }
}
