using System.Security.Cryptography;
using System.Text;

namespace Framework.Criptografia
{
    public static class CryptManager
    {
        public static string StringToMD5(string text)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder builder = new StringBuilder();

            for(int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
