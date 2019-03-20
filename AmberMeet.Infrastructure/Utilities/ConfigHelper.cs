using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace AmberMeet.Infrastructure.Utilities
{
    public class ConfigHelper
    {
        /// <summary>
        ///     是否属于调式环境
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                return false;
                //if (ConfigurationManager.AppSettings["IsDebug"] != null)
                //    return bool.Parse(ConfigurationManager.AppSettings["IsDebug"]);
                //return true;
            }
        }

        /// <summary>
        ///     AmberMeet数据库链接
        /// </summary>
        public static string Conn
        {
            get
            {
                if (ConfigurationManager.AppSettings["Conn"] != null)
                    return ConfigurationManager.AppSettings["Conn"];
                return "Data Source=.;Initial Catalog=AmberMeet;Integrated Security=True";
            }
        }

        /// <summary>
        ///     master数据库链接
        /// </summary>
        public static string MasterConn
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["MasterContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["MasterContext"].ToString();
                }
                return Conn.Replace("AmberMeet", "master");
            }
        }

        /// <summary>
        ///     新用户默认密码
        /// </summary>
        public static string DefaultUserPwd
        {
            get
            {
                if (ConfigurationManager.AppSettings["DefaultUserPwd"] != null)
                    return ConfigurationManager.AppSettings["DefaultUserPwd"];
                return "123456";
            }
        }

        /// <summary>
        ///     域名
        /// </summary>
        public static string BaseUrl
        {
            get
            {
                if (ConfigurationManager.AppSettings["BaseUrl"] != null)
                {
                    return ConfigurationManager.AppSettings["BaseUrl"];
                }
                if (HttpContext.Current.Request.Url.ToString().StartsWith("https://"))
                {
                    return $"https://{HttpContext.Current.Request.Url.Host}";
                }
                return $"http://{HttpContext.Current.Request.Url.Host}";
            }
        }

        /// <summary>
        ///     文件路径(自定义)
        /// </summary>
        public static string CustomFilesDir
        {
            get
            {
                string dir;
                if (ConfigurationManager.AppSettings["CustomFilesDir"] != null)
                {
                    dir = ConfigurationManager.AppSettings["CustomFilesDir"];
                }
                else
                {
                    dir = Path.Combine(SiteContentDir, "_Files") + "\\";
                }
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

        /// <summary>
        ///     文件保存文件夹是否在本机
        ///     winfrom使用，在本机则直接存储文件，不调用webservice
        /// </summary>
        public static bool CustomFilesDirIsLocal
        {
            get
            {
                if (ConfigurationManager.AppSettings["CustomFilesDirIsLocal"] != null)
                {
                    return bool.Parse(ConfigurationManager.AppSettings["CustomFilesDirIsLocal"]);
                }
                if (ConfigurationManager.AppSettings["CustomFilesDir"] != null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     文件存储路径网址
        /// </summary>
        public static string CustomFilesDirUrl => $"{BaseUrl}/_Files/";

        /// <summary>
        ///     获取新的Guid
        /// </summary>
        public static string NewGuid => Guid.NewGuid().ToString("N");

        /// <summary>
        ///     获取带时间的新的Guid(yyyyMMddHHmm+NewGuid)
        /// </summary>
        public static string NewTimeGuid
        {
            get { return $"{DateTime.Now:yyyyMMddHHmm}_{Guid.NewGuid():N}"; }
        }

        /// <summary>
        ///     获取当前时间全数字形式(24小时制-yyyyMMddHHmmssSSS)
        /// </summary>
        public static string CurrentTimeNumber => DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //public static string CurrentTimeNumber => DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        ///     基目录
        /// </summary>
        public static string SiteContentDir => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     根据Key获取AppSetting信息
        /// </summary>
        /// <param name="key">app setting key</param>
        /// <returns></returns>
        public static string GetAppSettings(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
                return ConfigurationManager.AppSettings[key].Trim();
            return null;
        }
    }
}