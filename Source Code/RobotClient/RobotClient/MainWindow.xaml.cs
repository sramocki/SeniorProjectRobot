using System;
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

        private void MoveEvent_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            //Console.WriteLine(button.Content);
            switch (button.Content)
            {
                case "Forward":
                    LogField.AppendText("Moving forward\n");
                    //send data here
                    break;

                case "Backwards":
                    LogField.AppendText("Moving backwards\n");
                    break;

                case "Left":
                    LogField.AppendText("Moving left\n");
                    break;

                case "Right":
                    LogField.AppendText("Moving right\n");
                    break;

                default:
                    Console.WriteLine("Mistakes were made");
                    //add exception here
                    break;

            }
            LogField.ScrollToEnd();
        }

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _dragging = false;
            if (!_dragging)
            {
                var slider = sender as Slider;
                double value = Math.Ceiling(slider.Value);
                LogField.AppendText("Setting speed at " + value + "\n");
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
                double value = Math.Ceiling(slider.Value);
                LogField.AppendText("Setting speed at " + value + "\n");
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
