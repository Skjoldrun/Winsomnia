using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using Winsomnia.Utility;
using Winsomnia.ViewModel;

namespace Winsomnia
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NotifyIcon.TrayIcon = (TaskbarIcon)FindResource("NotifyIcon");

            var notifyIconVM = new NotifyIconViewModel();
            NotifyIcon.TrayIcon.DataContext = notifyIconVM;

            if (Winsomnia.Properties.Settings.Default.ActivateOnStart)
                notifyIconVM.SwitchMode();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NotifyIcon.TrayIcon.Dispose();
            base.OnExit(e);
        }
    }
}