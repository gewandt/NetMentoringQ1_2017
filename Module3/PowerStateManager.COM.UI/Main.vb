Module Main

    ''' <summary>
    ''' Command C:\Windows\Microsoft.NET\Framework\v4.0.30319\Regasm.exe /tlb PowerManager.dll /codebase
    ''' regasm PowerManager.dll /codebase
    ''' </summary>
    Sub Main()
        Dim powerStateManager = CreateObject("PowerManager.PowerManager")

        Dim lastSleepTime As TimeSpan = TimeSpan.FromTicks(powerStateManager.LastSleepTime())
        Dim lastWakeTime As TimeSpan = TimeSpan.FromTicks(powerStateManager.LastWakeTime())
        Dim batteryState = powerStateManager.BatteryState()
        Dim powerInfo = powerStateManager.SystemPowerInformation()

        Console.WriteLine($"Last sleep time: {lastSleepTime}")
        Console.WriteLine($"Last wake time: {lastWakeTime}")

        Console.WriteLine($"Battery state: {batteryState.ToString()}")
        Console.WriteLine($"Power info: {powerInfo.ToString()}")

        Console.ReadKey()
    End Sub

End Module
