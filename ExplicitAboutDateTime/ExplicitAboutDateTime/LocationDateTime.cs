using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplicitAboutDateTime
{
    public class LocationDateTime
    {
        public static Dictionary<string, TimeZoneInfo> LocationTimezoneMap = new Dictionary<string, TimeZoneInfo>()
        {
            { "TRV", TimeZoneInfo.FindSystemTimeZoneById("India Standard Time") },
            { "SYD", TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time") },
            { "SEA", TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time") }
        };
    }
}
