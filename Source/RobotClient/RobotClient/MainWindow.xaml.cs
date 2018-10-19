﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using SharpDX.XInput;
using System.IO;

namespace RobotClient
{
    public partial class MainWindow
    {
        public List<PiCarConnection> deviceListMain = new List<PiCarConnection>();
        public string LeaderIp { set; get; }
        public string FollowerIP { set; get; }

        private string _leftAxis;
        private string _rightAxis;
        private string _buttons;
        private Controller _controller;
        private DispatcherTimer _timer = new DispatcherTimer();
        private readonly int _deadzoneValue = 2500;
        private double _motor1Controller;
        private double _motor2Controller;
        private Gamepad _previousState;

        /**
         * Method that runs when the main window launches
         */
        public MainWindow()
        {
            InitializeComponent();
            LeaderIp = "Empty";
            Title = "Welcome " + Environment.UserName;

            //Adds shortcut to open the registration window with Ctrl + R
            var newKeybind = new RoutedCommand();
            newKeybind.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newKeybind, Register_Click));

            //Checks if a controller is plugged into the current OS
            _controller = new Controller(UserIndex.One);
            if (!_controller.IsConnected)
            {
                LogField.AppendText(DateTime.Now + ":\tNo controller found\n");
            }
            else
            {
                //Uses a timer to loop a method that checks the status of the controller
                LogField.AppendText(DateTime.Now + ":\tController detected\n");
                _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
                _timer.Tick += _timer_Tick;
                _timer.Start();
                _motor1Controller = 0.0;
                _motor2Controller = 0.0;
            }
        }

        /**
         * Timer method that calls the method that checks the controller status
         */
        private void _timer_Tick(object sender, EventArgs e)
        {
            ControllerMovement();
        }

        /**
         * Method that opens the registration window
         */
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var register = new Registration();
            register.Show();
        }

        /**
         * Method that handles exporting data written to the log, and outputs a text file with timestamps
         */
        private void ExportLog_Click(object sender, RoutedEventArgs e)
        {
            //TODO Make the Log Export to the Application Path and Work a Time Stamp into Log Export's file Name
            var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var filename = "Log " + DateTime.Now.ToString("dddd, dd MMMM yyyy") + ".txt";
            File.WriteAllText(Path.Combine(documentsLocation, filename), LogField.Text);
        }

        /**
         *
         */
        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;
            LogField.Text = File.ReadAllText(dlg.FileName);
        }

        /**
         *
         */
        private void ControllerMovement()
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            var state = _controller.GetState().Gamepad;
            if (state.LeftThumbX.Equals(_previousState.LeftThumbX) &&
                state.LeftTrigger.Equals(_previousState.LeftTrigger) &&
                state.RightTrigger.Equals(_previousState.RightTrigger))
                return;

            //_Motor1 produces either -1.0 for left or 1.0 for right motion
            _motor1Controller = Math.Abs((double)state.LeftThumbX) < _deadzoneValue
                ? 0
                : (double)state.LeftThumbX / short.MinValue * -1;
            _motor1Controller = Math.Round(_motor1Controller, 3);

            /**
             * These variables produce either 1.0 for forward motion, or -1 for backwards.
             * If the values are both non-zero, then there will be no motion in either direction
             */
            var forwardSpeed = Math.Round(state.RightTrigger / 255.0, 3);
            var backwardSpeed = Math.Round(state.LeftTrigger / 255.0 * -1.0, 3);


            if (forwardSpeed > 0 && backwardSpeed == 0)
                _motor2Controller = forwardSpeed;
            else if (backwardSpeed < 0 && forwardSpeed == 0)
                _motor2Controller = backwardSpeed;
            else
                _motor2Controller = 0.0;

            LogField.AppendText(DateTime.Now + ":\tMotor 1: " + _motor1Controller + "\tMotor 2: " + _motor2Controller + "\n");
            LogField.ScrollToEnd();
            picar.setMotion(_motor1Controller, _motor2Controller);
            _previousState = state;
        }

        /**
         *
         */
        private void Key_down(object sender, KeyEventArgs e)
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            if (e.IsRepeat) return;

            var motorOne = 0.0;
            var motorTwo = 0.0;

            string[] motorTwoDir = { "Moving backwards", "In Neutral", "Moving forwards" };
            string[] motorOneDir = { "and left", "", "and right" };


            if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up)) motorTwo++;

            if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down)) motorTwo--;

            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left)) motorOne--;

            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right)) motorOne++;

            LogField.AppendText(DateTime.Now + ":\t" + motorTwoDir[(int)motorTwo + 1] + " " +
                                motorOneDir[(int)motorOne + 1] + "\n");
            picar.setMotion(motorOne, motorTwo);
            LogField.ScrollToEnd();
        }

        /**
         *
         */
        private void Key_up(object sender, KeyEventArgs e)
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;

            var motorOne = 0.0;
            var motorTwo = 0.0;

            string[] motorTwoDir = { "Now Moving backwards", "Now In Neutral", "Now Moving forwards" };
            string[] motorOneDir = { "and left", "", "and right" };

            if (Keyboard.IsKeyUp(Key.W) && Keyboard.IsKeyUp(Key.Up)) motorTwo--;

            if (Keyboard.IsKeyUp(Key.S) && Keyboard.IsKeyUp(Key.Down)) motorTwo++;

            if (Keyboard.IsKeyUp(Key.A) && Keyboard.IsKeyUp(Key.Left)) motorOne++;

            if (Keyboard.IsKeyUp(Key.D) && Keyboard.IsKeyUp(Key.Right)) motorOne--;

            LogField.AppendText(DateTime.Now + ":\t" + motorTwoDir[(int)motorTwo + 1] + " " +
                                motorOneDir[(int)motorOne + 1] + "\n");
            picar.setMotion(motorOne, motorTwo);
            LogField.ScrollToEnd();
        }

        /**
         *
         */
        private void ButtonPress_Event(object sender, RoutedEventArgs e)
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            var button = (RepeatButton)sender;
            switch (button.Name)
            {
                case "Forward":
                    LogField.AppendText(DateTime.Now + ":\tMoving forward\n");
                    picar.setMotion(1.0, 0.0);
                    break;

                case "Backwards":
                    LogField.AppendText(DateTime.Now + ":\tMoving backwards\n");
                    picar.setMotion(-1.0, 0.0);
                    break;

                case "Left":
                    LogField.AppendText(DateTime.Now + ":\tMoving left\n");
                    picar.setMotion(0.0, -1.0);
                    break;

                case "Right":
                    LogField.AppendText(DateTime.Now + ":\tMoving right\n");
                    picar.setMotion(0.0, 1.0);
                    break;

                default:
                    Console.WriteLine(DateTime.Now + ":\tThis wasn't supposed to happen..");
                    break;
            }
            LogField.ScrollToEnd();
        }

        /**
         *
         */
        private void About_Click(object sender, RoutedEventArgs e)
        {
            //TODO rewrite for tutorial information/team
            MessageBox.Show(
                "Created by Team Robot Follower, of Capstone Project Class 4999 of Fall 2018 \nThe Team consists of:\nEric Ramocki and Sean Ramocki on Desktop Application Developer\nAlex Alwardt and Scott Dudley on Network/Desktop Application Developer \nChristian Nickolaou and Anton Cantoldo on Vehicle Application/Systems Developer",
                "About");
        }

        /**
         *
         */
        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this program", "Confirmation", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) ==
                MessageBoxResult.Yes)

                Application.Current.Shutdown();
        }

        /**
         *
         */
        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            Console.WriteLine("Selected " + picar);

            //Update ipBox and deviceStatus with it's info
            IpBox.Text = picar.ipAddress;
            DeviceStatus.Text = picar.mode.ToString();
        }

        /**
         *
         */
        private void SetLeader(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            Console.WriteLine("Setting " + picar + "as Leader");

            //Send message to picar to change modes
            picar.setMode(ModeRequest.Types.Mode.Lead);
            //Update deviceStatus
            DeviceStatus.Text = picar.mode.ToString();
        }

        /**
         *
         */
        private void SetFollower(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            Console.WriteLine("Setting " + picar + "as Follower");

            //Send message to picar to change modes
            picar.setMode(ModeRequest.Types.Mode.Follow);
            //Update deviceStatus
            DeviceStatus.Text = picar.mode.ToString();
        }

        /**
         *
         */
        private void SetDefault(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            Console.WriteLine("Setting " + picar + "as Idle");

            //Send message to picar to change modes
            picar.setMode(ModeRequest.Types.Mode.Idle);
            //Update deviceStatus
            DeviceStatus.Text = picar.mode.ToString();
        }

        #region Properties

        public string LeftAxis
        {
            get => _leftAxis;
            set
            {
                if (value == _leftAxis) return;
                _leftAxis = value;
                OnPropertyChanged();
            }
        }

        public string RightAxis
        {
            get => _rightAxis;
            set
            {
                if (value == _rightAxis) return;
                _rightAxis = value;
                OnPropertyChanged();
            }
        }

        public string Buttons
        {
            get => _buttons;
            set
            {
                if (value == _buttons) return;
                _buttons = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
