using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    public class Problem11
    {
        public static void part1()
        {
            var password = new Password(Problem11.text());
            Console.WriteLine(password.NextValidPassword());
        }

        public static void part2()
        {
            var password = new Password(Problem11.text());
            Console.WriteLine(new Password(password.NextValidPassword()).NextValidPassword());
        }

        static String text()
        {
            return "cqjxjnds";
        }

        class Password
        {
            static string forbiddenChars = "iol";
            string pwd;

            public Password(string pwd)
            {
                this.pwd = pwd;
            }

            public string NextValidPassword()
            {
                var password = this;
                do
                {
                    password = new Password(password.Next());
                }
                while (!password.IsValid());
                return password.pwd;
            }

            Boolean IsValid()
            {
                return SatisfiesRule1() && SatisfiesRule2() && SatisfiesRule3();
            }

            Boolean SatisfiesRule1()
            {
                // Passwords must include one increasing straight of at least three letters,
                // like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
                var start = 0;
                for (var i = 1; i < pwd.Length; i++)
                {
                    if (pwd[i] - pwd[i-1] == 1)
                    {
                        if (i - start == 3)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        start = i-1;
                    }
                }
                return false;
            }

            Boolean SatisfiesRule2()
            {
                // Passwords may not contain the letters i, o, or l,
                // as these letters can be mistaken for other characters and are therefore confusing.
                foreach (var c in forbiddenChars)
                {
                    if (pwd.Contains(c))
                    {
                        return false;
                    }
                }
                return true;
            }
            
            Boolean SatisfiesRule3()
            {
                // Passwords must contain at least two different, non-overlapping pairs of letters, like aa, bb, or zz.
                var regex = new Regex(@"(\w)\1*");

                var matches = regex.Matches(pwd);
                var count = matches.Aggregate(0, (count, match) =>
                {
                    count += match.Groups[0].Length >> 1;
                    return count;
                });
                return count > 1;
            }

            string Next()
            {
                var next = pwd.ToCharArray();
                for (var i = pwd.Length - 1; i >= 0; i--)
                {
                    var c = next[i];
                    if (c != 'z')
                    {
                        next[i]++;
                        if (forbiddenChars.Contains(next[i]))
                        {
                            next[i]++;
                        }
                        break;
                    }
                    next[i] = 'a';
                }
                return new string(next);
            }
        }
    }
}
