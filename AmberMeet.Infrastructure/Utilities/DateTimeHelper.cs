using System;
using System.Globalization;

namespace AmberMeet.Infrastructure.Utilities
{
    public class DateTimeHelper
    {
        /// <summary>
        ///     yyyy-MM-dd HH:mm:ss格式获取时间
        /// </summary>
        public static DateTime GetNonNullIsoDateValue(string value)
        {
            if (value == null)
                return default(DateTime);

            var result = GetIsoDateValue(value);
            if (result == null)
            {
                return default(DateTime);
            }
            return result.Value;
        }

        /// <summary>
        ///     yyyy-MM-dd HH:mm:ss格式获取时间
        /// </summary>
        public static DateTime? GetIsoDateValue(string value)
        {
            try
            {
                if (value.Length == 8)
                {
                    return DateTime.ParseExact(value, "yyyy-M-d", CultureInfo.InvariantCulture);
                }
                if (value.Length == 9)
                {
                    return DateTime.ParseExact(value, "yyyy-M-dd", CultureInfo.InvariantCulture);
                }
                if (value.Length == 10)
                {
                    return DateTime.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                if (value.Length == 16)
                {
                    return DateTime.ParseExact(value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                }
                return DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     MM/dd/yyyy格式获取时间
        /// </summary>
        public static DateTime GetNonNullableDateValue(string value)
        {
            if (value == null)
                return default(DateTime);

            return DateTime.ParseExact(value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        }

        public static DateTime GetNonNullNumDateValue(string value)
        {
            if (value == null)
                return default(DateTime);
            if (value.Length == 6)
            {
                return DateTime.ParseExact(value, "yyyyMd", CultureInfo.InvariantCulture);
            }
            if (value.Length == 8)
            {
                return DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            return DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     MM/dd/yyyy格式获取时间
        /// </summary>
        public static DateTime GetNonNullNumDateValueNotErro(string value)
        {
            try
            {
                return GetNonNullNumDateValue(value);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        ///     时间格式2时间戳
        /// </summary>
        public static int GetDateTimeStamp(DateTime time)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int) (time - startTime).TotalSeconds;
        }

        /// <summary>
        ///     时间戳2时间格式
        /// </summary>
        public static DateTime GetDateTimeFromStamp(int stamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse(stamp + "0000000");
            var toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
    }
}