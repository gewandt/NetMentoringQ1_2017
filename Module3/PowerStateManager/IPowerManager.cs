using System;
using System.Runtime.InteropServices;

namespace PowerManager
{
    [Guid("73ea2fd0-d5d4-4ec8-ac55-a28fab862383"), InterfaceType(ComInterfaceType.InterfaceIsDual), ComVisible(true)]
    public interface IPowerManager
    {
        long LastSleepTime();
        long LastWakeTime();
        SYSTEM_BATTERY_STATE BatteryState();
        SYSTEM_POWER_INFORMATION SystemPowerInformation();
        void RemoveHibenationFile();
        void SaveHibenationFile();
        bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
    }
}
