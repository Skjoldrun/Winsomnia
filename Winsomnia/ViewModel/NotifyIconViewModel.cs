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
    public class NotifyIconViewModel : ObservableObject, IDisposable
    {
        private SystemMode _systemMode;
        private readonly AppSettings _settings;
        private bool _isMouseMoveActivated;
        private bool _isKeyPressActivated;
        private bool _isSystemStateActivated;
        private Timer _virtualInputTimer;
        private Window? _aboutWindow;
        private readonly ITrayIcon _trayIcon;
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

        public bool IsSystemStateActivated
        {
            get
            {
                return _isSystemStateActivated;
            }
            set
            {
                _isSystemStateActivated = value;
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
        /// Shows the about window modal if none is open yet.
        /// The window reference is cleared once it is closed to avoid leaking windows.
        /// </summary>
        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CanExecuteFunc = () => _aboutWindow == null,
                    CommandAction = () =>
                    {
                        _aboutWindow = new MainWindow();
                        _aboutWindow.DataContext = new MainWindowViewModel(this);
                        Application.Current.MainWindow = _aboutWindow;
                        _aboutWindow.ShowDialog();
                        _aboutWindow = null;
                        Application.Current.MainWindow = null;
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
        public NotifyIconViewModel(AppSettings settings, ITrayIcon trayIcon)
        {
            _settings = settings;
            _trayIcon = trayIcon;
            _systemMode = SystemMode.Default;
            _isMouseMoveActivated = settings.VirtualMouseMoveActivated;
            _isKeyPressActivated = settings.VirtualKeyPressActivated;
            _isSystemStateActivated = settings.SystemStateIdlePreventionActivated;

            _virtualInputTimer = new Timer();
            _virtualInputTimer.Interval = TimeSpan.FromMinutes(settings.VirtualInputTimer).TotalMilliseconds;
            _virtualInputTimer.Elapsed += VirtualInputEvent;
            _virtualInputTimer.AutoReset = true;
        }

        /// <summary>
        /// Releases the virtual input timer.
        /// </summary>
        public void Dispose()
        {
            _virtualInputTimer?.Dispose();
        }

        /// <summary>
        /// Switches between the system modes default and insomnia and mouse movement if configured.
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
            if (_isSystemStateActivated)
                SystemStateManager.ForceSystemAwake();

            SystemMode = SystemMode.Insomnia;
            _trayIcon.Icon = _activeIcon;
            Debug.WriteLine($"Set mode to SystemMode.Insomnia");
        }

        /// <summary>
        /// Sets the default mode.
        /// </summary>
        public void SetDefaultMode()
        {
            _virtualInputTimer.Enabled = false;
            if (_isSystemStateActivated)
                SystemStateManager.ResetSystemDefault();

            SystemMode = SystemMode.Default;
            _trayIcon.Icon = _defaultIcon;
            Debug.WriteLine($"Set mode to SystemMode.Default");
        }

        /// <summary>
        /// Keeps System awake with virtual input:
        /// - virtually pressing a button
        /// - Mouse movement
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
                MouseMove.Move(100, 0);
                MouseMove.Move(0, 100);
                MouseMove.Move(-100, 0);
                MouseMove.Move(0, -100);
                Debug.WriteLine($"Virtual Mouse moved");
            }
        }
    }
}