using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using SharpDX.XInput;

namespace RobotClient
{

    public partial class MainWindow
    {

        //public List<String[]> deviceListMain = new List<string[]>();
        public List<PiCarConnection> deviceListMain = new List<PiCarConnection>();
        public string LeaderIp { set; get; }
        public string FollowerIP { set; get; }
        private bool _keyHoldW;
        private bool _keyHoldS;
        private bool _keyHoldA;
        private bool _keyHoldD;
        private bool _keyHoldGeneric;

        private string _leftAxis;
        private string _rightAxis;
        private string _buttons;
        private Controller _controller;
        DispatcherTimer _timer = new DispatcherTimer();
        private int deadband = 2500;
        private double _Motor1;
        private double _Motor2;


        public MainWindow()
        {
            InitializeComponent();
            LeaderIp = "Empty";
            Title = "Welcome " + Environment.UserName;

            var newKeybind = new RoutedCommand();
            newKeybind.InputGestures.Add(new KeyGesture(Key.R, ModifierKeys.Control));
            CommandBindings.Add(new CommandBinding(newKeybind, Register_Click)); ;

            _controller = new Controller(UserIndex.One);
            if (!_controller.IsConnected)
            {
                LogField.AppendText(DateTime.Now + ":\tNo controller found\n");
            }
            else
            {
                LogField.AppendText(DateTime.Now + ":\tController detected\n");
                _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
                _timer.Tick += _timer_Tick;
                _timer.Start();
                _Motor1 = 0;
                _Motor2 = 0.0;
            }

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            ControllerMovement();
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
            _keyHoldGeneric = false;
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            if (e.Key == Key.W)
            {
                _keyHoldW = false;
                LogField.AppendText(DateTime.Now + ":\tStopped moving forward!\n");
                picar.setMotion(0.0, 1.0);
            }
            if (e.Key == Key.S)
            {
                _keyHoldS = false;
                LogField.AppendText(DateTime.Now + ":\tStopped moving backwards!\n");
                picar.setMotion(0.0, -1.0);
            }
            if (e.Key == Key.D)
            {
                _keyHoldD = false;
                LogField.AppendText(DateTime.Now + ":\tStopped moving right!\n");
                picar.setMotion(1.0, 0.0);
            }
            if (e.Key == Key.A)
            {
                _keyHoldA = false;
                LogField.AppendText(DateTime.Now + ":\tStopped moving left\n");
                picar.setMotion(-1.0, 0.0);
            }
            LogField.ScrollToEnd();
        }

        private void ControllerMovement()
        {
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            var state = _controller.GetState().Gamepad;

            //_Motor1 produces either -1.0 for left or 1.0 for right motion
            _Motor1 = (Math.Abs((double)state.LeftThumbX) < deadband) ? 0 : (double)state.LeftThumbX / short.MinValue * -1;
            _Motor1 = Math.Round(_Motor1, 3);

            /**
             * These variables produce either 1.0 for forward motion, or -1 for backwards.
             * If the values are both non-zero, then there will be no motion in either direction
             */
            var _forwardSpeed = Math.Round(state.RightTrigger / 255.0, 3);
            var _BackwardSpeed = Math.Round(state.LeftTrigger / 255.0 * -1.0, 3);

            
            if (_forwardSpeed > 0 && _BackwardSpeed == 0)
            {
                _Motor2 = _forwardSpeed;

            }
            else if (_BackwardSpeed < 0 && _forwardSpeed == 0)
            {
                _Motor2 = _BackwardSpeed;
            }
            else
            {
                _Motor2 = 0.0;
            }
            LogField.AppendText(DateTime.Now + ":\tMotor 1: " + _Motor1 + "\tMotor 2: " + _Motor2 + "\n");
            LogField.ScrollToEnd();
            picar.setMotion(_Motor1, _Motor2);
        }

        private void Key_down(object sender, KeyEventArgs e)
        {
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            _keyHoldGeneric = true;
            switch (e.Key)
            {
                case Key.W:
                case Key.Up:
                    _keyHoldW = true;
                    break;
                case Key.A:
                case Key.Left:
                    _keyHoldA = true;
                    break;
                case Key.D:
                case Key.Right:
                    _keyHoldD = true;
                    break;
                case Key.S:
                case Key.Down:
                    _keyHoldS = true;
                    break;
            }

            if (_keyHoldW && _keyHoldD)
            {
                LogField.AppendText(DateTime.Now + ":\tMoving forward and right\n");
                picar.setMotion(1.0, 1.0);
            }
            else if (_keyHoldW && _keyHoldA)
            {
                LogField.AppendText(DateTime.Now + ":\tMoving forward and left\n");
                picar.setMotion(-1.0, 1.0);
            }
            else if (_keyHoldW)
            {
                LogField.AppendText(DateTime.Now + ":\tMoving forward\n");
                picar.setMotion(0.0, 1.0);
            }
            else if (_keyHoldS && _keyHoldA)
            {
                LogField.AppendText(DateTime.Now + ":\tMoving backwards and left\n");
                picar.setMotion(1.0, -1.0);
            }
            else if (_keyHoldS && _keyHoldD)
            {
                LogField.AppendText(DateTime.Now + ":\tMoving backwards and right\n");
                picar.setMotion(-1.0, -1.0);
            }
            else if (_keyHoldS)
            {
                LogField.AppendText(DateTime.Now + ":\tMoving backwards\n");
                picar.setMotion(0.0, -1.0);
            }
            LogField.ScrollToEnd();
        }

        private void ButtonPress_Event(object sender, RoutedEventArgs e)
        {

            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            var button = (RepeatButton)sender;
            switch (button.Content)
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
                    Console.WriteLine(DateTime.Now + ":\tMistakes were made");
                    //add exception here
                    break;

            }
            LogField.ScrollToEnd();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            //TODO rewrite for tutorial information/team
            MessageBox.Show("Created by various people", "About");
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this program", "Confirmation", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) ==
                MessageBoxResult.Yes)

                Application.Current.Shutdown();
        }

        private void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            Console.WriteLine("Selected " + picar);

            //Update ipBox and deviceStatus with it's info
            ipBox.Text = picar.ipAddress;
            deviceStatus.Text = picar.mode.ToString();
        }

        private void SetLeader(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            Console.WriteLine("Setting " + picar + "as Leader");

            //Send message to picar to change modes
            picar.setMode(ModeRequest.Types.Mode.Lead);
            //Update deviceStatus
            deviceStatus.Text = picar.mode.ToString();
        }

        private void SetFollower(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            PiCarConnection picar = (PiCarConnection)deviceListMn.SelectedItem;
            Console.WriteLine("Setting " + picar + "as Follower");

            //Send message to picar to change modes
            picar.setMode(ModeRequest.Types.Mode.Follow);
            //Update deviceStatus
            deviceStatus.Text = picar.mode.ToString();
        }

        private void SetDefault(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)deviceListMn.SelectedItem;
            if (picar == null) return;
            PiCarConnection picar = (PiCarConnection)deviceListMn.SelectedItem;
            Console.WriteLine("Setting " + picar + "as Idle");

            //Send message to picar to change modes
            picar.setMode(ModeRequest.Types.Mode.Idle);
            //Update deviceStatus
            deviceStatus.Text = picar.mode.ToString();
        }

        /**
         *
         */
        #region Properties

        public string LeftAxis
        {
            get
            {
                return _leftAxis;
            }
            set
            {
                if (value == _leftAxis) return;
                _leftAxis = value;
                OnPropertyChanged();
            }
        }

        public string RightAxis
        {
            get
            {
                return _rightAxis;
            }
            set
            {
                if (value == _rightAxis) return;
                _rightAxis = value;
                OnPropertyChanged();
            }
        }

        public string Buttons
        {
            get
            {
                return _buttons;
            }
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
