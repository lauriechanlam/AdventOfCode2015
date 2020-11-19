using System;
using System.Linq;

namespace AdventOfCode2015
{
    public class Problem08
    {
        public static void part1()
        {
            var sum = 0;
            var lines = Problem08.text();
            foreach (var line in lines)
            {
                sum += line.Count();
                var ptr = 1;
                while (ptr < line.Count() - 1) {
                    sum--;
                    if (line[ptr] == '\\')
                    {
                        switch (line[ptr+1])
                        {
                            case '\\':
                            case '"':
                                ptr++;
                                break;
                            case 'x':
                                ptr += 3;
                                break;
                        }
                    }
                    ptr++;
                }
            }
            Console.WriteLine(sum);
        }

        public static void part2()
        {
            var sum = 0;
            var lines = Problem08.text();
            foreach (var line in lines)
            {
                var encodedCharCount = 2;
                foreach (var c in line)
                {
                    switch (c)
                    {
                        case '\\':
                        case '"':
                            encodedCharCount += 2;
                            break;
                        default:
                            encodedCharCount++;
                            break;
                    }
                }
                sum += encodedCharCount - line.Count();
            }
            Console.WriteLine(sum);
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/08.txt");
        }
    }
}
