# Winsomnia

Prevents windows to change to sleep or idle state and messengers to change your status to away.

This can be done with multiple possible options:

* Changing the System Execution State to prevent the system to be set to "idle"
* Virtually moving the mouse
* Virtually pressing and releasing a keyboard key

After some tests, it seems that the key press is the most reliable version and works best with function keys above `F13`, e.g. `F18`. These keys do exist for the OS but are most likely not used by the system or applications anymore.


# Winsomnia.VBS

This is a rudimentary Visual Basic Script to press a virtual key with a loop and timer. This can be used to test virtual keys on the system. It is scripted to press the `F18` every 4 minutes. Once started you have to kill the Process to stop the script from running.


# Build and Test

Open the solution in your IDE and build it. 


# Built with

* [WPF NotifyIcon](https://github.com/hardcodet/wpf-notifyicon) - WPF Notifyicon implementation, licensed with Code Project Open License (CPOL) 1.02 by Philipp Sumi, Robin Krom and Jan Karger.


# Author

David T. Halletz - [Skjoldrun](https://github.com/Skjoldrun)