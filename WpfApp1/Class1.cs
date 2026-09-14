using System;
using System.Text;

namespace WpfApp1
{
    public static class SecurityHelper
    {
        private const int Key = 3;

        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            char[] buffer = text.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (char)(buffer[i] + Key);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(new string(buffer)));
        }

        public static string Decrypt(string base64)
        {
            try
            {
                string text = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                char[] buffer = text.ToCharArray();
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = (char)(buffer[i] - Key);
                return new string(buffer);
            }
            catch { return "Ошибка данных"; }
        }
    }
}