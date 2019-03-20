using System.Web;

namespace AmberMeet.Infrastructure.Utilities
{
    public static class HtmlHelper
    {
        public static string Encode(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return HttpUtility.HtmlEncode(value);
        }
    }
}