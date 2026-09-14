using System.Drawing;
using Hardcodet.Wpf.TaskbarNotification;

namespace Winsomnia.Utility
{
    /// <summary>
    /// Minimal abstraction over the tray icon so the view model does not depend
    /// on the WPF TaskbarIcon directly. This keeps the view model testable.
    /// </summary>
    public interface ITrayIcon
    {
        /// <summary>
        /// The icon shown in the system tray.
        /// </summary>
        Icon? Icon { get; set; }
    }

    /// <summary>
    /// Adapts a Hardcodet TaskbarIcon to <see cref="ITrayIcon"/>.
    /// </summary>
    public sealed class TaskbarIconAdapter : ITrayIcon
    {
        private readonly TaskbarIcon _taskbarIcon;

        public TaskbarIconAdapter(TaskbarIcon taskbarIcon)
        {
            _taskbarIcon = taskbarIcon;
        }

        public Icon? Icon
        {
            get => _taskbarIcon.Icon;
            set => _taskbarIcon.Icon = value;
        }
    }
}
