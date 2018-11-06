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
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace RobotClient
{
    public partial class MainWindow
    {
        public List<PiCarConnection> deviceListMain = new List<PiCarConnection>();
        public string LeaderIp { set; get; }
        public string FollowerIP { set; get; }

        private readonly SynchronizationContext synchronizationContext;

        private string _leftAxis;
        private string _rightAxis;
        private string _buttons;
        private readonly Controller _controller;
        private const int DeadzoneValue = 2500;
        private double _directionController;
        private double _throttleController;
        private Gamepad _previousState;
        private int _distanceValue;

        /**
         * Method that runs when the main window launches
         */
        public MainWindow()
        {
            InitializeComponent();

            //Setup sync context
            synchronizationContext = SynchronizationContext.Current;
            
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
                LogField.AppendText(DateTime.Now + ":\tNo controller found!\n");
            }
            else
            {
                //Uses a timer to loop a method that checks the status of the controller
                LogField.AppendText(DateTime.Now + ":\tController detected!\n");
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
                timer.Tick += _timer_Tick;
                timer.Start();
                _directionController = 0.0;
                _throttleController = 0.0;
            }
        }

        /**
         * Image update method to update video stream image asynchronously
         */
        public void UpdateStream(ImageSource image)
		{
			try
			{
				synchronizationContext.Post(new SendOrPostCallback(o =>
				{
					StreamImage.Source = (ImageSource)o;
				}), image);
			}
			
			//TODO add specific exception cases
			catch(Exception e)
			{
				Console.WriteLine("Error " + e.ToString());
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
            try
            {
                //TODO Make the Log Export to the Application Path and Work a Time Stamp into Log Export's file Name
                var documentsLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filename = "Log " + DateTime.Now.ToString("dddd, dd MMMM yyyy") + ".txt";
                File.WriteAllText(Path.Combine(documentsLocation, filename), LogField.Text);
            }
            catch(IOException exception)
            {
                MessageBox.Show("Problem exporting log data " + exception.ToString(), "Error!");  
            }

        }

        /**
         * Method that handles importing data written from a locally saved log file, and outputs it into the current log field.
         */
        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    DefaultExt = ".txt"
                };
                var result = dlg.ShowDialog();
                if (result != true) return;
                LogField.Text = File.ReadAllText(dlg.FileName);
            }
            catch(IOException exception)
            {
                MessageBox.Show("Problem importing log data " + exception.ToString(), "Error!");
            }

        }

        /**
         * Method that handles the controller input for variable speed and direction
         */
        private void ControllerMovement()
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null || picar.Mode != ModeRequest.Types.Mode.Lead) return;
            var state = _controller.GetState().Gamepad;
            if (state.LeftThumbX.Equals(_previousState.LeftThumbX) &&
                state.LeftTrigger.Equals(_previousState.LeftTrigger) &&
                state.RightTrigger.Equals(_previousState.RightTrigger) && state.Buttons.Equals(_previousState.Buttons))
                return;

            //_Motor1 produces either -1.0 for left or 1.0 for right motion
            _directionController = Math.Abs((double)state.LeftThumbX) < DeadzoneValue
                ? 0
                : (double)state.LeftThumbX / short.MinValue * -1;
            _directionController = Math.Round(_directionController, 3);

            /**
             * These variables produce either 1.0 for forward motion, or -1 for backwards.
             * If the values are both non-zero, then there will be no motion in either direction
             */
            var forwardSpeed = Math.Round(state.RightTrigger / 255.0, 3);
            var backwardSpeed = Math.Round(state.LeftTrigger / 255.0 * -1.0, 3);


            if (forwardSpeed > 0 && backwardSpeed == 0)
                _throttleController = forwardSpeed;
            else if (backwardSpeed < 0 && forwardSpeed == 0)
                _throttleController = backwardSpeed;
            else
                _throttleController = 0.0;

            switch(state.Buttons)
            {
                case GamepadButtonFlags.DPadDown:
                    _distanceValue--;
                    break;

                case GamepadButtonFlags.DPadUp:
                    _distanceValue++;
                    break;
            }

            if (_distanceValue < 0)
                _distanceValue = 0;

            if (_distanceValue > 10)
                _distanceValue = 10;

            string[] throttleStrings = { "Moving backwards", "In Neutral", "Moving forwards" };
            string[] directionStrings = { "and left", "", "and right" };
            LogField.AppendText(DateTime.Now + ":\t" + throttleStrings[(int)_throttleController + 1] + " " +
                                directionStrings[(int)_directionController + 1] + "\n");

            LogField.AppendText(DateTime.Now + ":\tSet follower distance to " + _distanceValue + "\n");
            LogField.ScrollToEnd();

            picar.SetMotion(_throttleController,_directionController);
            _previousState = state;
        }

        /**
         * Method that handles when one or more key is pressed down (Vehicle is moving in one or more directions)
         */
        private void Key_down(object sender, KeyEventArgs e)
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null || picar.Mode != ModeRequest.Types.Mode.Lead) return;
            if (e.IsRepeat) return;

            var directionMotor = 0.0;
            var throttleMotor = 0.0;

            string[] throttleStrings = { "Moving backwards", "In Neutral", "Moving forwards" };
            string[] directionStrings = { "and left", "", "and right" };


            if (Keyboard.IsKeyDown(Key.W) || Keyboard.IsKeyDown(Key.Up)) throttleMotor++;

            if (Keyboard.IsKeyDown(Key.S) || Keyboard.IsKeyDown(Key.Down)) throttleMotor--;

            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left)) directionMotor--;

            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right)) directionMotor++;

            LogField.AppendText(DateTime.Now + ":\t" + throttleStrings[(int)throttleMotor + 1] + " " +
                                directionStrings[(int)directionMotor + 1] + "\n");
            picar.SetMotion(throttleMotor, directionMotor);
            LogField.ScrollToEnd();
        }

        /**
         * Method that handles when one or more key is released (Vehicle is stopping in one or more directions)
         */
        private void Key_up(object sender, KeyEventArgs e)
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null || picar.Mode != ModeRequest.Types.Mode.Lead) return;

            var directionMotor = 0.0;
            var throttleMotor = 0.0;

            string[] throttleStrings = { "Now Moving backwards", "Now In Neutral", "Now Moving forwards" };
            string[] directionString = { "and left", "", "and right" };

            if (Keyboard.IsKeyUp(Key.W) && Keyboard.IsKeyUp(Key.Up)) throttleMotor--;

            if (Keyboard.IsKeyUp(Key.S) && Keyboard.IsKeyUp(Key.Down)) throttleMotor++;

            if (Keyboard.IsKeyUp(Key.A) && Keyboard.IsKeyUp(Key.Left)) directionMotor++;

            if (Keyboard.IsKeyUp(Key.D) && Keyboard.IsKeyUp(Key.Right)) directionMotor--;

            LogField.AppendText(DateTime.Now + ":\t" + throttleStrings[(int)throttleMotor + 1] + " " +
                                directionString[(int)directionMotor + 1] + "\n");
            picar.SetMotion(throttleMotor, directionMotor);
            LogField.ScrollToEnd();
        }

        /**
         * Method that handles when the GUI buttons are held down (Vehicle is moving a single direction)
         */
        private void ButtonPress_Event(object sender, RoutedEventArgs e)
        {
            //TODO add button up event for stop command
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null || picar.Mode != ModeRequest.Types.Mode.Lead) return;
            var button = (RepeatButton)sender;
            switch (button.Name)
            {
                case "Forward":
                    LogField.AppendText(DateTime.Now + ":\tMoving forward\n");
                    picar.SetMotion(1.0, 0.0);
                    break;

                case "Backwards":
                    LogField.AppendText(DateTime.Now + ":\tMoving backwards\n");
                    picar.SetMotion(-1.0, 0.0);
                    break;

                case "Left":
                    LogField.AppendText(DateTime.Now + ":\tMoving left\n");
                    picar.SetMotion(0.0, -1.0);
                    break;

                case "Right":
                    LogField.AppendText(DateTime.Now + ":\tMoving right\n");
                    picar.SetMotion(0.0, 1.0);
                    break;

                default:
                    Console.WriteLine("Mistakes were made");
                    break;
            }
            LogField.ScrollToEnd();
        }

        /**
         *  Method that handles when the GUI button is released (Vehicle is stopped)
         */
        private void ButtonPress_Released(object sender, RoutedEventArgs e)
        {
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null || picar.Mode != ModeRequest.Types.Mode.Lead) return;
            LogField.AppendText(DateTime.Now + ":\tNow In Neutral\n");
            picar.SetMotion(0.0, 0.0);
            LogField.ScrollToEnd();
        }

        private void DistanceSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _distanceValue = (int)DistanceSlider.Value;
            LogField.AppendText(DateTime.Now + ":\tSet follower distance to " + _distanceValue + "\n");
            LogField.ScrollToEnd();
        }

        /**
         * Method that opens a message box with 'About' information
         */
        private void About_Click(object sender, RoutedEventArgs e)
        {
            //TODO rewrite for tutorial information/team
            MessageBox.Show(
                "Created by Team Robot Follower, of Capstone Project Class 4999 of Fall 2018 \nThe Team consists of:\nEric Ramocki and Sean Ramocki on Desktop Application Developer\nAlex Alwardt and Scott Dudley on Network/Desktop Application Developer \nChristian Nickolaou and Anton Cantoldo on Vehicle Application/Systems Developer",
                "About");
        }

        /**
         * Method that handles shutdown confirmation
         */
        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to close this program", "Confirmation", MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                Application.Current.Shutdown();
            }        
        }

        /**
         * Method that handles shutdown confirmation
         */
        private void Window_Closing(object sender, CancelEventArgs e)
        {

                foreach (var t in DeviceListMn.Items)
                {
                    try
                    {
                        if (t is PiCarConnection temp && temp.Mode == ModeRequest.Types.Mode.Lead)
                        {

                            LogField.AppendText(DateTime.Now + ":\t" + temp.Name + " is stopping");
                            temp.StopStream();
                            temp.SetMotion(0.0, 0.0);
                            temp.SetMode(ModeRequest.Types.Mode.Idle);
                        }
                    }
                    catch(Exception exception)
                    {
                    LogField.AppendText(DateTime.Now + ":\tSomething went wrong: " + exception.ToString());
                    }
                }

            Application.Current.Shutdown();
        }

        /**
         *
         */
        private async void DeviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Stop the stream of the previously selected event
                foreach (PiCarConnection oldPicar in e.RemovedItems)
                {

                    oldPicar.StopStream();
                }
            }
            catch(Exception exception)
            {
                //TODO Remove vehicles that throw exceptions
                LogField.AppendText(DateTime.Now + ":\tSomething went wrong " + exception.ToString());
            }
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;

            try
            {
                var streamTask = picar.StartStream();
                await streamTask;
            }
            catch (NullReferenceException nre)
            {
                LogField.AppendText(DateTime.Now + ":\tSomething went wrong " + nre.ToString());
            }
            catch(Exception exception)
            {
                LogField.AppendText(DateTime.Now + ":\tSomething went wrong " + exception.ToString());
            }

            //Update ipBox and deviceStatus with it's info
            IpBox.Text = picar.ipAddress;
            DeviceStatus.Text = picar.Mode.ToString();
        }

        /**
         *
         */
        private void SetLeader(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            LogField.AppendText(DateTime.Now + ":\tSetting " + picar + "as Leader");

            //Send message to picar to change modes
            picar.SetMode(ModeRequest.Types.Mode.Lead);
            //Update deviceStatus
            DeviceStatus.Text = picar.Mode.ToString();
        }

        /**
         *
         */
        private void SetFollower(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            LogField.AppendText(DateTime.Now + ":\tSetting " + picar + "as Follower");

            //Send message to picar to change modes
            picar.SetMode(ModeRequest.Types.Mode.Follow);
            //Update deviceStatus
            DeviceStatus.Text = picar.Mode.ToString();
        }

        /**
         *
         */
        private void SetDefault(object sender, RoutedEventArgs e)
        {
            //Get the picar from the device List
            var picar = (PiCarConnection)DeviceListMn.SelectedItem;
            if (picar == null) return;
            LogField.AppendText(DateTime.Now + ":\tSetting " + picar + "as Idle");

            //Send message to picar to change modes
            picar.SetMode(ModeRequest.Types.Mode.Idle);
            //Update deviceStatus
            DeviceStatus.Text = picar.Mode.ToString();
        }

        private void SelectLeaders_Click(object sender, RoutedEventArgs e)
        {
            foreach (var t in DeviceListMn.Items)
            {
                if (t is PiCarConnection temp && temp.Mode == ModeRequest.Types.Mode.Lead)
                {
                    DeviceListMn.SelectedItems.Add(temp);
                }
            }
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
