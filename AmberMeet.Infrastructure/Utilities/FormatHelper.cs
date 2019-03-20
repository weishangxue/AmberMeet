using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AmberMeet.Infrastructure.Utilities
{
    public static class FormatHelper
    {
        public static string GetString(string value)
        {
            return value ?? string.Empty;
        }

        public static string GetLengthString(string value, int length = 50)
        {
            if (length < 8 || value.Length < length)
                return value;
            return $"{value.Substring(0, length - 7)}......";
        }

        public static string GetDecimalString(decimal? value)
        {
            if (value == null)
                return string.Empty;

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static int GetInt(int? value)
        {
            return value ?? 0;
        }

        public static string GetIsoDateString(DateTime? value)
        {
            if (value == null)
                return string.Empty;

            return value.Value.ToString("yyyy-MM-dd");
        }

        public static string GetNonNullIsoDateString(DateTime value)
        {
            if (value == DateTime.MinValue)
                return string.Empty;
            return value.ToString("yyyy-MM-dd");
        }

        public static string GetIsoDateTimeString(DateTime? value)
        {
            if (value == null)
                return string.Empty;

            return value.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetNonNullIntString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetIntString(int? value)
        {
            if (value == null)
                return string.Empty;

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetLongString(long? value)
        {
            if (value == null)
                return string.Empty;

            return value.Value.ToString(CultureInfo.InvariantCulture);
        }

        public static string GetBooleanString(bool value)
        {
            if (value)
                return "ÊÇ";

            return "·ñ";
        }

        public static string GetJsonString(string value, string key)
        {
            try
            {
                return JObject.Parse(value)[key].ToString();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return null;
            }
        }

        public static string[] GetJsonStrings(string value, string arrayKey, string key)
        {
            try
            {
                IList<string> result = new List<string>();
                var arrayJson = JObject.Parse(value)[arrayKey].ToString();
                var objects = JArray.Parse(arrayJson);
                foreach (var objectStr in objects)
                {
                    result.Add(((JObject) objectStr)[key].ToString());
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return null;
            }
        }

        public static string[] GetArrayJsonStrings(string arrayJson)
        {
            try
            {
                IList<string> result = new List<string>();
                var objects = JArray.Parse(arrayJson);
                foreach (var objectStr in objects)
                {
                    result.Add(objectStr.ToString());
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return null;
            }
        }
    }
}