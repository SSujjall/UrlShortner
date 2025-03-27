using System.Text;

namespace UrlShortner.Data.Extensions
{
    public static class StringExtension
    {
        public static string Encrypt(this string data)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var encryptedData = Convert.ToBase64String(dataBytes);
            return encryptedData;
        }

        public static string Decrypt(this string data)
        {
            var dataBytes = Convert.FromBase64String(data);
            var decryptedData = Encoding.UTF8.GetString(dataBytes);
            return decryptedData;
        }
    }
}
