using System;
using System.IO;
using Xunit;
using Winsomnia.Utility;

namespace Winsomnia.Tests
{
    public class AppSettingsTests
    {
        private const string FileName = "appsettings.json";

        [Fact]
        public void LoadsValuesFromFile()
        {
            var file = CreateTempFile(
                """
                {
                  "VirtualInputTimer": 7,
                  "ActivateOnStart": false,
                  "VirtualMouseMoveActivated": true,
                  "VirtualKeyPressActivated": false,
                  "SystemStateIdlePreventionActivated": true
                }
                """);

            var settings = new AppSettings(file);

            Assert.Equal(7, settings.VirtualInputTimer);
            Assert.False(settings.ActivateOnStart);
            Assert.True(settings.VirtualMouseMoveActivated);
            Assert.False(settings.VirtualKeyPressActivated);
            Assert.True(settings.SystemStateIdlePreventionActivated);
        }

        [Fact]
        public void MissingFileUsesDefaults()
        {
            var settings = new AppSettings(NonExistentPath());

            Assert.Equal(4, settings.VirtualInputTimer);
            Assert.True(settings.ActivateOnStart);
            Assert.False(settings.VirtualMouseMoveActivated);
            Assert.True(settings.VirtualKeyPressActivated);
            Assert.False(settings.SystemStateIdlePreventionActivated);
        }

        [Fact]
        public void MissingKeysUseDefaults()
        {
            var file = CreateTempFile("{ }");

            var settings = new AppSettings(file);

            Assert.Equal(4, settings.VirtualInputTimer);
            Assert.True(settings.ActivateOnStart);
        }

        [Fact]
        public void InvalidValuesUseDefaults()
        {
            var file = CreateTempFile(
                """
                {
                  "VirtualInputTimer": "not-a-number",
                  "ActivateOnStart": "nope"
                }
                """);

            var settings = new AppSettings(file);

            Assert.Equal(4, settings.VirtualInputTimer);
            Assert.True(settings.ActivateOnStart);
        }

        private static string CreateTempFile(string content)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), FileName);
            var directory = Path.GetDirectoryName(path);
            if (directory is not null)
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, content);

            return path;
        }

        private static string NonExistentPath()
        {
            return Path.Combine(Path.GetTempPath(), "does-not-exist-" + Path.GetRandomFileName(), FileName);
        }
    }
}
