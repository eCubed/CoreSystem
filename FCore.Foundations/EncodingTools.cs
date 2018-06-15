using System;
using System.Text;

namespace FCore.Foundations
{
    public static class EncodingTools
    {
        public static string ToBase64String(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        public static string FromBase64String(string base64String)
        {
            int pad = 4 - (base64String.Length % 4);
            pad = pad > 2 ? 0 : pad;

            base64String = base64String.PadRight(base64String.Length + pad, '=');
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64String));
        }
    }
}
