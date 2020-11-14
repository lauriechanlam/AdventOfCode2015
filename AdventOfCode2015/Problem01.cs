using System;
using System.Linq;

namespace AdventOfCode2015
{
    public class Problem01
    {
        public static void part1()
        {
            string text = Problem01.text();
            int count = text.Count(c => (c == '(')) - text.Count(c => (c == ')'));
            Console.WriteLine(count);
        }

        public static void part2()
        {
            string text = Problem01.text();
            var floor = 0;
            var move_count = 1;
            foreach (var c in text)
            {
                if (c == '(')
                {
                    floor += 1;
                }
                else if (c == ')')
                {
                    floor -= 1;
                }
                if (floor < 0)
                {
                    Console.WriteLine(move_count);
                    break;
                }
                move_count += 1;
            }  
        }

        static String text()
        {
            return System.IO.File.ReadAllText("resources/01.txt");
        }
    }
}
