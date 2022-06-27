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
        private bool _isKeyPressActivated;
        private bool _isSystemStateIdlePreventionActivated;
        private Timer _virtualInputTimer;
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

        public bool IsKeyPressActivated
        {
            get
            {
                return _isKeyPressActivated;
            }
            set
            {
                _isKeyPressActivated = value;
                OnPropertyChanged();
            }
        }

        public bool IsSystemStateIdlePreventionActivated
        {
            get
            {
                return _isSystemStateIdlePreventionActivated;
            }
            set
            {
                _isSystemStateIdlePreventionActivated = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Switches between default and activated mode.
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
            _isMouseMoveActivated = Properties.Settings.Default.VirtualMouseMoveActivated;
            _isKeyPressActivated = Properties.Settings.Default.VirtualKeyPressActivated;
            _isSystemStateIdlePreventionActivated = Properties.Settings.Default.SystemStateIdlePreventionActivated;

            _virtualInputTimer = new Timer();
            _virtualInputTimer.Interval = TimeSpan.FromMinutes(Properties.Settings.Default.VirtualInputTimer).TotalMilliseconds;
            _virtualInputTimer.Elapsed += VirtualInputEvent;
            _virtualInputTimer.AutoReset = true;
        }

        /// <summary>
        /// Switches between the system modes default and insomnia and mousemovement if configured.
        /// </summary>
        public void SwitchMode()
        {
            if (SystemMode == SystemMode.Default)
                SetInsomniaMode();
            else
                SetDefaultMode();
        }

        /// <summary>
        /// Sets the insomnia mode with configured methods.
        /// </summary>
        public void SetInsomniaMode()
        {
            _virtualInputTimer.Enabled = true;
            if (_isSystemStateIdlePreventionActivated)
                SystemStateManager.ForceSystemAwake();

            SystemMode = SystemMode.Insomnia;
            NotifyIcon.TrayIcon.Icon = _activeIcon;
            Debug.WriteLine($"Set mode to SystemMode.Insomnia");
        }

        /// <summary>
        /// Sets the default mode.
        /// </summary>
        public void SetDefaultMode()
        {
            _virtualInputTimer.Enabled = false;
            if (_isSystemStateIdlePreventionActivated)
                SystemStateManager.ResetSystemDefault();

            SystemMode = SystemMode.Default;
            NotifyIcon.TrayIcon.Icon = _defaultIcon;
            Debug.WriteLine($"Set mode to SystemMode.Default");
        }

        /// <summary>
        /// Keeps System awake with virtual input:
        /// - virtually pressing a button
        /// - Mousemovement
        /// </summary>
        private void VirtualInputEvent(object? sender, ElapsedEventArgs e)
        {
            if (_isKeyPressActivated)
            {
                // virtually pressed button works fine, if the timer is set between 2 to 5 min
                KeyPress.PressKey(Keys.F18);
                KeyPress.ReleaseKey(Keys.F18);
                Debug.WriteLine($"Virtual Key pressed");
            }

            if (_isMouseMoveActivated)
            {
                MouseMove.Move(0, 100);
                MouseMove.Move(100, 0);
                MouseMove.Move(0, -100);
                MouseMove.Move(-100, 0);
                Debug.WriteLine($"Virtual Mouse moved");
            }
        }
    }
}