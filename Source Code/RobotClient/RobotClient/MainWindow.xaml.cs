using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RobotClient
{

    public partial class MainWindow : Window
    {

        public List<String[]> LeaderIpList = new List<string[]>();
        public string LeaderIp { set; get; }
        public string FollowerIP { set; get; }
        private bool _keyHold = false;

        //TODO allow multi direction keyboard presses
        private bool keyone = false;
        private bool keytwo = false;

        //TODO
        public string LeaderMac;
        public string FollowerMac;

        private bool _dragging;
        private double _sliderSpeed = 0;


        public MainWindow()
        {
            InitializeComponent();
            LeaderIp = "Empty";
            //Title = "Welcome " + Environment.UserName;

            var newKeybind = new RoutedCommand();
            newKeybind.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newKeybind, Register_Click));;

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

        private void Key_up(object sender, KeyEventArgs e)
        {
            _keyHold = false;
            LogField.AppendText("Stopped moving!\n");
            LogField.ScrollToEnd();
        }

        private void Key_down(object sender, KeyEventArgs e)
        {
            //TODO XInput and allow multi direction key presses
            if (e.Key == Key.W || e.Key == Key.Up)
            {
                _keyHold = true;
                LogField.AppendText("Moving forward at " + _sliderSpeed + "\n");
                //Send packet L_0.0_1.0
                LogField.ScrollToEnd();
            }
            else if (e.Key == Key.S || e.Key == Key.Down)
            {
                _keyHold = true;
                LogField.AppendText("Moving backwards at " + _sliderSpeed + "\n");
                //Send packet L_0.0_-1.0
                LogField.ScrollToEnd();
            }
            else if (e.Key == Key.A || e.Key == Key.Left)
            {
                _keyHold = true;
                LogField.AppendText("Moving left at " + _sliderSpeed + "\n");
                //Send packet L_-1.0_0.0
                LogField.ScrollToEnd();
            }
            else if (e.Key == Key.D || e.Key == Key.Right)
            {
                _keyHold = true;
                LogField.AppendText("Moving right at " + _sliderSpeed + "\n");
                //Send packet L_1.0_0.0
                LogField.ScrollToEnd();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void ButtonPress_Event(object sender, RoutedEventArgs e)
        {
            var button = (RepeatButton)sender;
            switch (button.Content)
            {
                case "Forward":
                    LogField.AppendText("Moving forward at " + _sliderSpeed + "\n");
                    //Send packet L_0.0_1.0
                    break;

                case "Backwards":
                    LogField.AppendText("Moving backwards at " + _sliderSpeed + "\n");
                    //Send packet L_0.0_-1.0
                    break;

                case "Left":
                    LogField.AppendText("Moving left at " + _sliderSpeed + "\n");
                    //Send packet L_-1.0_0.0
                    break;

                case "Right":
                    LogField.AppendText("Moving right at " + _sliderSpeed + "\n");
                    //Send packet L_1.0_0.0
                    break;

                default:
                    Console.WriteLine("Mistakes were made");
                    //add exception here
                    break;

            }
            LogField.ScrollToEnd();
        }

/*        static void SendUdp(int srcPort, string dstIp, int dstPort, byte[] data)
        {
            using (UdpClient c = new UdpClient(srcPort))
                c.Send(data, data.Length, dstIp, dstPort);
        }*/

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _dragging = false;
            if (!_dragging)
            {
                if (sender is Slider slider) _sliderSpeed = Math.Ceiling(slider.Value);
                LogField.AppendText("Setting speed at " + _sliderSpeed + "\n");
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
                if (sender is Slider slider) _sliderSpeed = Math.Ceiling(slider.Value);
                LogField.AppendText("Setting speed at " + _sliderSpeed + "\n");
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
