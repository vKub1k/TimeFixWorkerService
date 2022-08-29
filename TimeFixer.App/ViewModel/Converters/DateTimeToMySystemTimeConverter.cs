using System;
using TimeFixer.App.Model;

namespace TimeFixer.App.ViewModel.Converters;

public static class DateTimeToMySystemTimeConverter
{
    public static MySystemTime ConvertToMySystemTime(DateTime onlineTime)
    {
        MySystemTime conversionResult;
        conversionResult.wYear = Convert.ToInt16(onlineTime.Year);
        conversionResult.wMonth = Convert.ToInt16(onlineTime.Month);
        conversionResult.wDay = Convert.ToInt16(onlineTime.Day);
        conversionResult.wDayOfWeek = Convert.ToInt16(onlineTime.DayOfWeek);
        conversionResult.wHour = Convert.ToInt16(onlineTime.Hour);
        conversionResult.wMinute = Convert.ToInt16(onlineTime.Minute);
        conversionResult.wSecond = Convert.ToInt16(onlineTime.Second);
        conversionResult.wMilliseconds = Convert.ToInt16(onlineTime.Millisecond);

        return conversionResult;
    }
}