SET wsc = CreateObject("WScript.Shell")
Do
    WScript.Sleep(4*60*1000)
    wsc.SendKeys("{F18}")
Loop