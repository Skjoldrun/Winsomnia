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
        /// Moves the mouse cursor for +/- given values.
        /// </summary>
        /// <param name="y">move px up ( < 0 ) or down ( > 0 )</param>
        /// <param name="x">move px right ( < 0 ) or left ( > 0 )</param>
        public static void Move(int y, int x)
        {
            GetCursorPos(out Point currentPos);

            currentPos.y += y;
            currentPos.x += x;

            SetCursorPos(currentPos.x, currentPos.y);
        }
    }
}