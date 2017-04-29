Set WshShell = CreateObject("WScript.Shell")
RegAsmPath = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe"
WshShell.run "cmd /d " & RegAsmPath & " ~\PowerStateManager.dll /codebase", 0, True
Set powerManager = CreateObject("PowerManager.PowerManager")
Set lastSleepTime = powerManager.LastSleepTime()
MsgBox lastSleepTime