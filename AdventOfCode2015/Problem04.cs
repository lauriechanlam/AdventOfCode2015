using System;

namespace AdventOfCode2015
{
    public class Problem04
    {
        public static void part1()
        {
            string text = Problem04.text();
            for (var i = 0; i < 1000000; i += 1)
            {
                var input = text + i.ToString("D5");
                var output = md5(input);
                if (output.StartsWith("00000"))
                {
                    Console.WriteLine(i);
                    break;
                }
            }
        }

        public static void part2()
        {
            string text = Problem04.text();
            for (var i = 0; i < 10000000; i += 1)
            {
                var input = text + i.ToString("D5");
                var output = md5(input);
                if (output.StartsWith("000000"))
                {
                    Console.WriteLine(i);
                    break;
                }
            }
        }

        static String text()
        {
            return "bgvyzdsv";
        }

        static string md5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
