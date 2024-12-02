using System;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode
{
    public class Day4
    {
        private MD5 md5;
        private StringBuilder sb;

        public Day4()
        {
            string secret = "ckczppom";

            md5 = MD5.Create();
            sb = new StringBuilder();

            Console.WriteLine("Running Day 4 - a");
            _FindSaltWithPrefixForSecret(secret, "00000");

            Console.WriteLine("Running Day 4 - b");
            _FindSaltWithPrefixForSecret(secret, "000000");
        }

        private void _FindSaltWithPrefixForSecret(string secret, string prefix)
        {
            for (int i=0; ; ++i)
            {
                string result = _CalculateMD5Hash(secret + i.ToString());
                if (result.Substring(0, prefix.Length).Equals(prefix))
                {
                    Console.WriteLine("i = " + i + ", md5 = " + result);
                    return;
                }
            }
        }

        private string _CalculateMD5Hash(string input)
        {
            sb.Clear();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
