using System.Runtime.InteropServices;

namespace TimeFixer.App.Model;

    [StructLayout(LayoutKind.Sequential)]
    public struct MySystemTime
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }
