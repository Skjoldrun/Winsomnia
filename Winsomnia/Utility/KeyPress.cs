using System.Runtime.InteropServices;

namespace Winsomnia.Utility
{
    public class KeyPress
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public static void PressKey(byte keyCode)
        {
            keybd_event(keyCode, 0, Keys.KEYEVENTF_KEYDOWN, 0);
        }

        public static void ReleaseKey(byte keyCode)
        {
            keybd_event(keyCode, 0, Keys.KEYEVENTF_KEYUP, 0);
        }
    }
}