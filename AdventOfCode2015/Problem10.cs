using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    public class Problem10
    {
        public static void part1()
        {
            var text = Problem10.text();
            for (var i = 0; i < 40; i++)
            {
                text = Problem10.step(text);
            }
            Console.WriteLine(text.Length);
        }

        public static void part2()
        {
            var text = Problem10.text();
            for (var i = 0; i < 50; i++)
            {
                text = Problem10.step(text);
            }
            Console.WriteLine(text.Length);
        }

        static String text()
        {
            return "1113122113";
        }

        static string step(string input)
        {
            var regex = new Regex(@"(\d)\1*");

            var matches = regex.Matches(input);
            var sequences = matches.Select(match =>
            {
                var count = match.Length;
                var digit = match.Groups[1].Value;
                return count.ToString() + digit;
            });
            return String.Join("", sequences);
        }
    }
}
