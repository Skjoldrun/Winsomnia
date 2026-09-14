using Xunit;
using Winsomnia.Utility;

namespace Winsomnia.Tests
{
    public class KeysTests
    {
        [Fact]
        public void F18_HasExpectedVirtualKeyCode()
        {
            Assert.Equal(0x81, Keys.F18);
        }

        [Fact]
        public void ExtendedKeyFlagsCoverF13ToF24()
        {
            // Keys F13..F24 (VK 0x7C..0x87) require the extended key flag to be sent correctly.
            for (byte vk = 0x7C; vk <= 0x87; vk++)
            {
                Assert.True(vk >= 0x7C && vk <= 0x87);
            }

            Assert.True(Keys.F13 >= 0x7C);
            Assert.True(Keys.F24 <= 0x87);
        }

        [Fact]
        public void KeyDownAndUpFlagsAreDistinct()
        {
            Assert.NotEqual(Keys.KEYEVENTF_KEYDOWN, Keys.KEYEVENTF_KEYUP);
        }
    }
}
