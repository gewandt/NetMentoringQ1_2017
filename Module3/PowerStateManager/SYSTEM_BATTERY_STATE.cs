namespace PowerManager
{
    /// <summary>
    /// Contains information about the current state of the system battery.
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/aa373212(v=vs.85).aspx
    /// </summary>
    public struct SYSTEM_BATTERY_STATE
    {
        public bool AcOnLine;
        public bool BatteryPresent;
        public bool Charging;
        public bool Discharging;
        public bool[] Spare1;
        public long MaxCapacity;
        public long RemainingCapacity;
        public long Rate;
        public long EstimatedTime;
        public long DefaultAlert1;
        public long DefaultAlert2;
        public override string ToString() => $"AcOnLine: {AcOnLine}, " + 
                                             $"BatteryPresent: {BatteryPresent}, " +
                                             $"Charging: {Charging}, " + 
                                             $"Discharging: {Discharging}, " +
                                             $"MaxCapacity: {MaxCapacity}, " + 
                                             $"RemainingCapacity: {RemainingCapacity}, " +
                                             $"Rate: {Rate}, " + 
                                             $"EstimatedTime: {EstimatedTime}, " +
                                             $"DefaultAlert1: {DefaultAlert1}, DefaultAlert2: {DefaultAlert2}";
    }
}
