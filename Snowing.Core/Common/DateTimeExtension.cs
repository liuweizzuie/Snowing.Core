using System;
using System.Collections.Generic;
using System.Text;

namespace Snowing.Common
{
    public static class DateTimeExtension
    {
        public static DateTime ToDateTime(this Int64 value)
        {
            return new DateTime(1970, 1, 1).AddSeconds(value);
        }

        public static DateTime ToDateTime(this int value)
        {
            return new DateTime(1970, 1, 1).AddSeconds(value);
        }

        public static DateTime MsToDateTime(this ulong value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(value);
        }

        public static DateTime MsToDateTime(this long value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(value);
        }

        public static DateTime MsToDateTime(this int value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(value);
        }

        public static ulong ToInt64(this DateTime value)
        {
            return (ulong)(value - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static int ToInt(this DateTime value)
        {
            return (int)(value - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static ulong ToInt64Ms(this DateTime value)
        {
            return (ulong)(value - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static int TomonthDays(this DateTime date)
        {
            DateTime dtStart = new DateTime(date.Year, date.Month, 1);
            DateTime dtStop = dtStart.AddMonths(1);
            return (dtStop - dtStart).Days;
        }
    }
}
