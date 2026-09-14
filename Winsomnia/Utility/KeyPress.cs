using System;
using System.Runtime.InteropServices;

namespace Winsomnia.Utility
{
    public class KeyPress
    {
        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardInput
        {
            public ushort vk;
            public ushort scan;
            public uint flags;
            public uint time;
            public IntPtr extraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Input
        {
            public uint type;
            public KeyboardInput keyboard;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, ref Input pInput, int cbSize);

        /// <summary>
        /// Returns true if the virtual key code requires the extended key flag.
        /// Keys F13..F24 (VK 0x7C..0x87) need it to be sent correctly.
        /// </summary>
        public static bool RequiresExtendedFlag(byte keyCode)
        {
            return keyCode is >= 0x7C and <= 0x87;
        }

        /// <summary>
        /// Sends a virtual key down event.
        /// </summary>
        public static void PressKey(byte keyCode)
        {
            var input = CreateInput(keyCode, flags: 0);
            SendInput(1, ref input, Marshal.SizeOf<Input>());
        }

        /// <summary>
        /// Sends a virtual key up event.
        /// </summary>
        public static void ReleaseKey(byte keyCode)
        {
            var input = CreateInput(keyCode, KEYEVENTF_KEYUP);
            SendInput(1, ref input, Marshal.SizeOf<Input>());
        }

        private static Input CreateInput(byte keyCode, uint flags)
        {
            uint extended = RequiresExtendedFlag(keyCode) ? KEYEVENTF_EXTENDEDKEY : 0;

            return new Input
            {
                type = INPUT_KEYBOARD,
                keyboard = new KeyboardInput
                {
                    vk = keyCode,
                    scan = 0,
                    flags = flags | extended,
                    time = 0,
                    extraInfo = IntPtr.Zero,
                },
            };
        }
    }
}
