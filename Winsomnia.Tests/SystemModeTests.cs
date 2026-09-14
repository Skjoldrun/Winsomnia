using System;
using Xunit;
using Winsomnia.Utility;

namespace Winsomnia.Tests
{
    public class SystemModeTests
    {
        [Fact]
        public void HasExpectedMembers()
        {
            Assert.Equal(2, Enum.GetValues(typeof(SystemMode)).Length);
            Assert.True(Enum.IsDefined(typeof(SystemMode), SystemMode.Default));
            Assert.True(Enum.IsDefined(typeof(SystemMode), SystemMode.Insomnia));
        }
    }
}
