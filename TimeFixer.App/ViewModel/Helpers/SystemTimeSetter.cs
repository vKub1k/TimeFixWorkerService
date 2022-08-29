using System.Runtime.InteropServices;
using TimeFixer.App.Model;

namespace TimeFixer.App.ViewModel.Helpers;

public class SystemTimeSetter
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetSystemTime(ref MySystemTime st);
}