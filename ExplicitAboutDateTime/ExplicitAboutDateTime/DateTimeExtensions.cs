using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplicitAboutDateTime
{
    public static class DateTimeExtensions
    {
        public static LocationDateTime ToLocationDateTime(this DateTime dateTime, Location location)
        {
            if (dateTime == null)
                return null;

            if (location == null)
                throw new ArgumentNullException(nameof(location));

            return LocationDateTime.AtLocation(dateTime, location);
        }
    }
}
