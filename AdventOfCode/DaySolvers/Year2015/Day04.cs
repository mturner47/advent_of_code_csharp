using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2015
{
    internal class Day04 : IDaySolver
    {
        public object EasySolution(IList<string> lines)
        {
            var key = lines[0];
            var i = 1;
            while (true)
            {
                var fullKey = key + i.ToString();
                var fullKeyBytes = Encoding.ASCII.GetBytes(fullKey);
                var hash = MD5.HashData(fullKeyBytes);
                var hexString = Convert.ToHexString(hash);
                if (hexString.StartsWith("00000")) break;
                i++;
            }
            var expectedResult = 117946;
            var result = i;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }

        public object HardSolution(IList<string> lines)
        {
            var key = lines[0];
            var i = 1;
            while (true)
            {
                var fullKey = key + i.ToString();
                var fullKeyBytes = Encoding.ASCII.GetBytes(fullKey);
                var hash = MD5.HashData(fullKeyBytes);
                var hexString = Convert.ToHexString(hash);
                if (hexString.StartsWith("000000")) break;
                i++;
            }
            var expectedResult = 3938038;
            var result = i;
            var pass = expectedResult == result ? "Pass" : "Fail";
            return $"{pass} - {result}";
        }
    }
}
