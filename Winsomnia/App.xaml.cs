using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using Winsomnia.Utility;
using Winsomnia.ViewModel;

namespace Winsomnia
{
    public partial class App : Application
    {
        private TaskbarIcon? _taskbarIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _taskbarIcon = (TaskbarIcon)FindResource("NotifyIcon");

            var settings = new AppSettings();
            var notifyIconVM = new NotifyIconViewModel(settings, new TaskbarIconAdapter(_taskbarIcon));
            _taskbarIcon.DataContext = notifyIconVM;

            if (settings.ActivateOnStart)
                notifyIconVM.SwitchMode();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _taskbarIcon?.Dispose();
            base.OnExit(e);
        }
    }
}