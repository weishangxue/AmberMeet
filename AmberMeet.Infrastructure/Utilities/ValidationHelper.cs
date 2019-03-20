using System.Text.RegularExpressions;

namespace AmberMeet.Infrastructure.Utilities
{
    internal class ValidationHelper
    {
        /// <summary>
        ///     用户登录名验证(字母数字下划线)
        /// </summary>
        public static bool IsLoginName(string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z_][A-Za-z0-9_]*$");
        }

        /// <summary>
        ///     验证电话号码
        /// </summary>
        public static bool IsTelephone(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        ///     验证手机号码
        /// </summary>
        public static bool IsMobile(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^(13|14|15|16|17|18|19)\d{9}$");
        }

        /// <summary>
        ///     验证身份证号
        /// </summary>
        public static bool IsIDcard(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"(^\d{18}$)|(^\d{15}$)");
        }

        /// <summary>
        ///     验证输入为数字
        /// </summary>
        public static bool IsNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^[0-9]*$");
        }

        /// <summary>
        ///     验证邮编
        /// </summary>
        public static bool IsPostalcode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            return Regex.IsMatch(value, @"^\d{6}$");
        }
    }
}