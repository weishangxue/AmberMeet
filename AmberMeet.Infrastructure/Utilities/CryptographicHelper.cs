using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AmberMeet.Infrastructure.Utilities
{
    public static class CryptographicHelper
    {
        private static readonly byte[] RgbIv = {33, 45, 160, 77, 232, 3, 12, 89};
        private static readonly byte[] RgbKey = {67, 108, 56, 245, 23, 8, 188, 21};

        public static string ToBase64String(long source)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source.ToString(CultureInfo.InvariantCulture)));
        }

        public static string ToBase64String(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        public static string FromBase64String(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            return Encoding.UTF8.GetString(Convert.FromBase64String(source));
        }

        public static string Hash(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            var sourceBytes = Encoding.UTF8.GetBytes(source);

            var sha1 = SHA1.Create();

            var hash = sha1.ComputeHash(sourceBytes);

            return Convert.ToBase64String(hash);
        }

        public static string Encrypt(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            var des = DES.Create();

            var encryptor = des.CreateEncryptor(RgbKey, RgbIv);

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(source);
                    }
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static string Decrypt(string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;

            var sourceBytes = Convert.FromBase64String(source);

            var des = DES.Create();

            var decryptor = des.CreateDecryptor(RgbKey, RgbIv);

            using (var memoryStream = new MemoryStream(sourceBytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var streamReader = new StreamReader(cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }
}