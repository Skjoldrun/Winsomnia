using System;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using Winsomnia.Command;
using Winsomnia.Utility;

namespace Winsomnia.ViewModel
{
    public class NotifyIconViewModel : ObservableObject
    {
        private SystemMode _systemMode;
        private bool _isMouseMoveActivated;
        private Timer _mouseTimer;
        private Icon _defaultIcon = Properties.Resource.Default;
        private Icon _activeIcon = Properties.Resource.Active;

        public SystemMode SystemMode
        {
            get
            {
                return _systemMode;
            }
            set
            {
                _systemMode = value;
                OnPropertyChanged();
            }
        }

        public bool IsMouseMoveActivated
        {
            get
            {
                return _isMouseMoveActivated;
            }
            set
            {
                _isMouseMoveActivated = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Switches between default and activated mode.
        /// Also switches mouse movement, if configured.
        /// </summary>
        public ICommand SwitchModeCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => true,
                    CommandAction = () =>
                    {
                        SwitchMode();
                    }
                };
            }
        }

        /// <summary>
        /// Shows the about window if not opened yet.
        /// </summary>
        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => Application.Current.MainWindow == null || Application.Current.MainWindow.IsActive == false,
                    CommandAction = () =>
                    {
                        Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.DataContext = new MainWindowViewModel(this);
                        Application.Current.MainWindow.Show();
                    }
                };
            }
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }

        /// <summary>
        /// Constructor with setting systemMode flag to default and preparing the timer for mouse movement.
        /// </summary>
        public NotifyIconViewModel()
        {
            _systemMode = SystemMode.Default;
            _isMouseMoveActivated = Properties.Settings.Default.MouseMoveActivated;
            _mouseTimer = new Timer();
            _mouseTimer.Interval = TimeSpan.FromMinutes(Properties.Settings.Default.MoveMouseInMin).TotalMilliseconds;
            _mouseTimer.Elapsed += MoveMouse;
            _mouseTimer.AutoReset = true;
        }

        /// <summary>
        /// Switches between the system modes default and insomnia and mousemovement if configured.
        /// </summary>
        public void SwitchMode()
        {
            if (SystemMode == SystemMode.Default)
            {
                if (_isMouseMoveActivated)
                {
                    _mouseTimer.Enabled = true;
                }

                Insomnia.ForceSystemAwake();
                SystemMode = SystemMode.Insomnia;
                NotifyIcon.TrayIcon.Icon = _activeIcon;
                Debug.WriteLine($"Changed Mode to SystemMode.Insomnia");
            }
            else
            {
                _mouseTimer.Enabled = false;
                Insomnia.ResetSystemDefault();
                SystemMode = SystemMode.Default;
                NotifyIcon.TrayIcon.Icon = _defaultIcon;
                Debug.WriteLine($"Changed Mode to SystemMode.Default");
            }
        }

        /// <summary>
        /// Moves the mouse cursor down, right, up and left to its original position again.
        /// </summary>
        private void MoveMouse(object? sender, ElapsedEventArgs e)
        {
            MouseMove.Move(1, 0);
            Debug.WriteLine($"Moved mousecursor down.");

            MouseMove.Move(0, 1);
            Debug.WriteLine($"Moved mousecursor right.");

            MouseMove.Move(-1, 0);
            Debug.WriteLine($"Moved mousecursor up.");

            MouseMove.Move(0, -1);
            Debug.WriteLine($"Moved mousecursor left.");
        }
    }
}