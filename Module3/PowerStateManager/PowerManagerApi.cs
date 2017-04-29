using System;
using System.Runtime.InteropServices;

namespace PowerManager
{
    public class PowerManagerApi
    {
        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        public static extern int LastSleepOrWakeTime(int informationLevel, IntPtr inputBuffer, int inputBufferSize, out long outputBuffer, int outputBufferSize);

        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        public static extern int PowerInformation(int informationLevel, IntPtr inputBuffer, int inputBufferSize, out SYSTEM_POWER_INFORMATION outputBuffer, int outputBufferSize);

        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        public static extern int ToggleHibernationFile(int informationLevel, bool reserveHibernationFile, int inputBufferSize, IntPtr outputBuffer, int outputBufferSize);

        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        public static extern int BatteryState(int informationLevel, IntPtr inputBuffer, int inputBufferSize, out SYSTEM_BATTERY_STATE outputBuffer, int outputBufferSize);

        [DllImport("powrprof.dll")]
        public static extern bool SetSuspendState(bool Hibernate, bool ForceCritical, bool DisableWakeEvent);
    }
}
