using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplicitAboutDateTime
{
    public class LocationDateTime
    {
        private DateTimeOffset internalDateTimeOffset;

        public static Dictionary<string, TimeZoneInfo> LocationTimezoneMap = new Dictionary<string, TimeZoneInfo>()
        {
            { "TRV", TimeZoneInfo.FindSystemTimeZoneById("India Standard Time") },
            { "SYD", TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time") },
            { "SEA", TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time") }
        };

        private LocationDateTime(DateTimeOffset dateTimeOffset)
        {
            this.internalDateTimeOffset = dateTimeOffset;
        }

        public static bool TryCreateDateFromUTC(string dateCandidate, out LocationDateTime date)
        {
            date = null;
            DateTimeOffset temp;
            var isDate = DateTimeOffset.TryParse(dateCandidate, out temp);
            var isOnlyDate = isDate && temp.TimeOfDay.TotalMilliseconds == 0;

            if (isOnlyDate)
                date = new LocationDateTime(temp);

            return isOnlyDate;
        }

        public override bool Equals(object obj)
        {
            var objAsLocationDateTime = obj as LocationDateTime;
            if (objAsLocationDateTime == null || this == null)
                return false;

            return this.internalDateTimeOffset.Equals(objAsLocationDateTime.internalDateTimeOffset);
        }
    }
}
