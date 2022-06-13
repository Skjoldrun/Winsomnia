using System;
using System.Runtime.InteropServices;

namespace Winsomnia.Utility
{
    public static class SystemStateManager
    {
        /// <summary>
        /// Forces the system to stay awake.
        /// </summary>
        public static void ForceSystemAwake()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS |
                                    EXECUTION_STATE.ES_DISPLAY_REQUIRED |
                                    EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                    EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
        }

        /// <summary>
        /// Resets to system default.
        /// </summary>
        public static void ResetSystemDefault()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        }

        /// <summary>
        /// Sets the execution state to given flags.
        /// For more information, see: https://docs.microsoft.com/de-de/windows/win32/api/winbase/nf-winbase-setthreadexecutionstate
        /// </summary>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        /// <summary>
        /// Flags to be set as execution state.
        /// </summary>
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001

            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }
    }
}