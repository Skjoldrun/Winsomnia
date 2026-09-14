using Xunit;
using Winsomnia.Utility;

namespace Winsomnia.Tests
{
    public class MouseMoveTests
    {
        [Fact]
        public void NextPosition_AddsDeltasToCurrentPosition()
        {
            var (x, y) = MouseMove.NextPosition(fromX: 10, fromY: 20, dx: 5, dy: -3);

            Assert.Equal(15, x);
            Assert.Equal(17, y);
        }

        [Fact]
        public void NextPosition_HandlesNegativeDeltas()
        {
            var (x, y) = MouseMove.NextPosition(fromX: 0, fromY: 0, dx: -100, dy: 100);

            Assert.Equal(-100, x);
            Assert.Equal(100, y);
        }

        [Fact]
        public void NextPosition_HandlesZeroDeltas()
        {
            var (x, y) = MouseMove.NextPosition(fromX: 42, fromY: 84, dx: 0, dy: 0);

            Assert.Equal(42, x);
            Assert.Equal(84, y);
        }
    }
}
