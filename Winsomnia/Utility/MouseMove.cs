using System.Runtime.InteropServices;

namespace Winsomnia.Utility
{
    public class MouseMove
    {
        /// <summary>
        /// Sets the mouse cursor to the new coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("User32.Dll")]
        private static extern long SetCursorPos(int x, int y);

        /// <summary>
        /// Gets the current position of the mouse cursor.
        /// </summary>
        /// <param name="point"></param>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out Point point);

        /// <summary>
        /// Coordinates with x and y.
        /// </summary>
        public struct Point
        {
            public int x;
            public int y;
        }

        /// <summary>
        /// Computes the next cursor position for a given offset.
        /// Pure function, no Win32 calls, so it can be unit tested.
        /// </summary>
        /// <param name="fromX">current x coordinate</param>
        /// <param name="fromY">current y coordinate</param>
        /// <param name="dx">delta in x, move right ( &gt; 0 ) or left ( &lt; 0 )</param>
        /// <param name="dy">delta in y, move down ( &gt; 0 ) or up ( &lt; 0 )</param>
        /// <returns>The next cursor position.</returns>
        public static (int x, int y) NextPosition(int fromX, int fromY, int dx, int dy)
        {
            return (fromX + dx, fromY + dy);
        }

        /// <summary>
        /// Moves the mouse cursor by a given delta.
        /// </summary>
        /// <param name="dx">delta in x, move right ( &gt; 0 ) or left ( &lt; 0 )</param>
        /// <param name="dy">delta in y, move down ( &gt; 0 ) or up ( &lt; 0 )</param>
        public static void Move(int dx, int dy)
        {
            GetCursorPos(out Point currentPos);

            (int x, int y) next = NextPosition(currentPos.x, currentPos.y, dx, dy);

            SetCursorPos(next.x, next.y);
        }
    }
}