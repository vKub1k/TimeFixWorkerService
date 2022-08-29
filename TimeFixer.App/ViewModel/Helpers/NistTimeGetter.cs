using System;
using System.Globalization;
using System.Net;
using System.Net.Cache;
using System.Threading.Tasks;

namespace TimeFixer.App.ViewModel.Helpers;

public class NistTimeGetter
{
    [Obsolete("Obsolete")]
    public static async Task<DateTime> GetNistTime()
    {
        DateTime dateTime = default;

        var request = (HttpWebRequest)WebRequest.Create("https://www.microsoft.com");
        request.Method = "GET";
        request.Accept = "text/html, application/xhtml+xml, */*";
        request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
        request.ContentType = "application/x-www-form-urlencoded";
        request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
        var response =  (HttpWebResponse) await request.GetResponseAsync();
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
}