using System.Globalization;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace TimeFixWorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    [Obsolete("Obsolete")]
    private static DateTime GetNistTime()
    {
        DateTime dateTime = default;
        
        var request = (HttpWebRequest)WebRequest.Create("https://www.microsoft.com");
        request.Method ="GET";
        request.Accept ="text/html, application/xhtml+xml, */*";
        request.UserAgent ="Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
        request.ContentType ="application/x-www-form-urlencoded";
        request.CachePolicy =new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        var response = (HttpWebResponse)request.GetResponse();
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var todaysDates = response.Headers["date"];

            if (todaysDates != null)
                dateTime = DateTime.ParseExact(todaysDates, "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                    CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
        }
        else
        {
            dateTime = DateTime.MinValue;
        }

        return dateTime;
    }
    [StructLayout(LayoutKind.Sequential)]
    private struct MySystemTime
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
    
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetSystemTime(ref MySystemTime st);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var st = new MySystemTime();
            var onlineTime = GetNistTime();
            onlineTime = onlineTime.AddHours(-2);
            
            st.wYear = Convert.ToInt16(onlineTime.Year);
            st.wMonth = Convert.ToInt16(onlineTime.Month);
            st.wDay = Convert.ToInt16(onlineTime.Day);
            st.wDayOfWeek = Convert.ToInt16(onlineTime.DayOfWeek);
            st.wHour = Convert.ToInt16(onlineTime.Hour);
            st.wMinute = Convert.ToInt16(onlineTime.Minute);
            st.wSecond = Convert.ToInt16(onlineTime.Second);
            st.wMilliseconds = Convert.ToInt16(onlineTime.Millisecond);

            SetSystemTime(ref st);
            _logger.LogInformation(message: $"Time changed to: {onlineTime}");
            
            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(30000, stoppingToken);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}