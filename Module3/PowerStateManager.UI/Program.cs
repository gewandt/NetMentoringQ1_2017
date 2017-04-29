using System;

namespace PowerManager.UI
{
    class Program
    {
        private static readonly IPowerManager _pm = new PowerManager(); 

        static void Main(string[] args)
        {
            var powerInfo = _pm.SystemPowerInformation();
            var batteryState = _pm.BatteryState();

            Console.WriteLine($"Last sleep time: {_pm.LastSleepTime()}");
            Console.WriteLine($"Last wake time: {_pm.LastWakeTime()}");

            Console.WriteLine($"Battery state: {batteryState.ToString()}");
            Console.WriteLine($"Power info: {powerInfo.ToString()}");

            // Hibernate state
            //_pm.SetSuspendState(true, false, true);

            _pm.SaveHibenationFile();
            _pm.RemoveHibenationFile();

            Console.ReadKey();
        }
    }
}
