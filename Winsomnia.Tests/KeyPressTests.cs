using Xunit;
using Winsomnia.Utility;

namespace Winsomnia.Tests
{
    public class KeyPressTests
    {
        [Fact]
        public void RequiresExtendedFlag_IsTrueForF13ToF24()
        {
            Assert.True(KeyPress.RequiresExtendedFlag(Keys.F13));
            Assert.True(KeyPress.RequiresExtendedFlag(Keys.F18));
            Assert.True(KeyPress.RequiresExtendedFlag(Keys.F24));
        }

        [Fact]
        public void RequiresExtendedFlag_IsFalseForF1ToF12()
        {
            Assert.False(KeyPress.RequiresExtendedFlag(Keys.F1));
            Assert.False(KeyPress.RequiresExtendedFlag(Keys.F12));
        }
    }
}
