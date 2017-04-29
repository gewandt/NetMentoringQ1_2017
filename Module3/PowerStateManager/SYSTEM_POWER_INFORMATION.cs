using System;

namespace PowerManager
{
    /// <summary>
    /// Contains information about the idleness of the system.
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa373217(v=vs.85).aspx
    /// </summary>
    public struct SYSTEM_POWER_INFORMATION
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;
        public override string ToString() => $"MaxIdlenessAllowed: {MaxIdlenessAllowed}, " +
                                             $"Idleness: {Idleness}, " +
                                             $"TimeRemaining: {TimeSpan.FromTicks(TimeRemaining)}, " +
                                             $"CoolingMode: {CoolingMode}.";
    }
}
