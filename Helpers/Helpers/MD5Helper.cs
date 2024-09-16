using System.Security.Cryptography;
using System.Text;

namespace Helpers.Helpers
{
    public class MD5Helper
    {
        public static string HashString(string input)
        {
            var fullKeyBytes = Encoding.ASCII.GetBytes(input);
            var hash = MD5.HashData(fullKeyBytes);
            return Convert.ToHexString(hash);
        }
    }
}
