using System;
using System.Runtime.InteropServices;

namespace PowerManager
{
    [ClassInterface(ClassInterfaceType.None), Guid("8f342e7c-f3ab-459f-b8b9-ff9dc1762956"), ComVisible(true)]
    public class PowerManager : IPowerManager
    {
        struct InformationLevel
        {
            public const int LastSleepTime = 15;
            public const int LastWakeTime = 14;
            public const int BatteryState = 5;
            public const int PowerInformation = 12;
            public const int SystemHiberFile = 10;
        }

        public long LastSleepTime()
        {
            long sleepTime;
            PowerManagerApi.LastSleepOrWakeTime(InformationLevel.LastSleepTime, IntPtr.Zero, 0, out sleepTime, Marshal.SizeOf(typeof(long)));
            return sleepTime;
        }

        public long LastWakeTime()
        {
            long wakeTime;
            PowerManagerApi.LastSleepOrWakeTime(InformationLevel.LastWakeTime, IntPtr.Zero, 0, out wakeTime, Marshal.SizeOf(typeof(long)));
            return wakeTime;
        }

        public SYSTEM_POWER_INFORMATION SystemPowerInformation()
        {
            SYSTEM_POWER_INFORMATION powerInfo;
            var result = PowerManagerApi.PowerInformation(InformationLevel.PowerInformation, IntPtr.Zero, 0, out powerInfo, Marshal.SizeOf(typeof(SYSTEM_POWER_INFORMATION)));
            return powerInfo;
        }

        public SYSTEM_BATTERY_STATE BatteryState()
        {
            SYSTEM_BATTERY_STATE batteryState;
            var result = PowerManagerApi.BatteryState(InformationLevel.BatteryState, IntPtr.Zero, 0, out batteryState, Marshal.SizeOf(typeof(SYSTEM_BATTERY_STATE)));
            return batteryState;
        }

        public void SaveHibenationFile()
        {
            PowerManagerApi.ToggleHibernationFile(InformationLevel.SystemHiberFile, true, Marshal.SizeOf(typeof(bool)), IntPtr.Zero, 0);
        }

        public void RemoveHibenationFile()
        {
            PowerManagerApi.ToggleHibernationFile(InformationLevel.SystemHiberFile, false, Marshal.SizeOf(typeof(bool)), IntPtr.Zero, 0);
        }

        public bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
        {
            return PowerManagerApi.SetSuspendState(hibernate, forceCritical, disableWakeEvent);
        }
    }
}
