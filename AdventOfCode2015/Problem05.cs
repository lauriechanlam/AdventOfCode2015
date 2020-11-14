using System;
using System.Linq;

namespace AdventOfCode2015
{
    public class Problem05
    {
        public static void part1()
        {
            string[] kids = Problem05.text();
            var niceKidsCount = kids.Count(isKidNice1);
            Console.WriteLine(niceKidsCount);
        }

        public static void part2()
        {
            string[] kids = Problem05.text();
            var niceKidsCount = kids.Count(isKidNice2);
            Console.WriteLine(niceKidsCount);
        }

        static bool isKidNice1(string name)
        {
            var vowels = "aeiou";
            var badSubstrings = new string[] { "ab", "cd", "pq", "xy" };
            var vowelCount = 0;
            var containsConsecutive = false;
            var previousChar = '0';
            foreach (var c in name) {
                var substring = new String(new char[] {previousChar, c});
                if (badSubstrings.Contains(substring))
                {
                    return false;
                }
                if (previousChar == c)
                {
                    containsConsecutive = true;
                }
                if (vowels.Contains(c)) {
                    vowelCount += 1;
                }
                previousChar = c;
            }
            return containsConsecutive && vowelCount >= 3;
        }

        static bool isKidNice2(string name)
        {
            bool condition1(string name)
            {
                // It contains a pair of any two letters that appears at least twice in the string without overlapping,
                // like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
                for (int i = 1; i < name.Count(); i += 1)
                {
                    var substring = new String(new char[] { name[i-1], name[i] });
                    for (int j = i + 2; j < name.Count(); j += 1) {
                        var otherSubstring = new String(new char[] { name[j-1], name[j] });
                        if (substring == otherSubstring)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            bool condition2(string name)
            {
                // It contains at least one letter which repeats with exactly one letter between them,
                // like xyx, abcdefeghi (efe), or even aaa.
                for (int i = 2; i < name.Count(); i += 1)
                {
                    if (name[i] == name[i - 2])
                    {
                        return true;
                    }
                }
                return false;
            }
            return condition1(name) && condition2(name);
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/05.txt");
        }
    }
}
