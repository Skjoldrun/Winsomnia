using System.Drawing;
using System.IO;
using Xunit;
using Winsomnia.Utility;
using Winsomnia.ViewModel;

namespace Winsomnia.Tests
{
    public class NotifyIconViewModelTests
    {
        private sealed class FakeTrayIcon : ITrayIcon
        {
            public Icon? Icon { get; set; }
        }

        [Fact]
        public void SwitchModeFromDefaultSetsTrayIcon()
        {
            var tray = new FakeTrayIcon();

            using var vm = new NotifyIconViewModel(new AppSettings(NonExistentPath()), tray);
            vm.SwitchMode();

            Assert.NotNull(tray.Icon);
        }

        [Fact]
        public void SecondSwitchModeResetsTrayIcon()
        {
            var tray = new FakeTrayIcon();

            using var vm = new NotifyIconViewModel(new AppSettings(NonExistentPath()), tray);
            vm.SwitchMode();
            vm.SwitchMode();

            Assert.NotNull(tray.Icon);
        }

        [Fact]
        public void SetInsomniaModeSetsTrayIcon()
        {
            var tray = new FakeTrayIcon();

            using var vm = new NotifyIconViewModel(new AppSettings(NonExistentPath()), tray);
            vm.SetInsomniaMode();

            Assert.NotNull(tray.Icon);
        }

        [Fact]
        public void SetDefaultModeSetsTrayIcon()
        {
            var tray = new FakeTrayIcon();

            using var vm = new NotifyIconViewModel(new AppSettings(NonExistentPath()), tray);
            vm.SetDefaultMode();

            Assert.NotNull(tray.Icon);
        }

        private static string NonExistentPath()
        {
            return Path.Combine(Path.GetTempPath(), "does-not-exist-" + Path.GetRandomFileName(), "appsettings.json");
        }
    }
}
