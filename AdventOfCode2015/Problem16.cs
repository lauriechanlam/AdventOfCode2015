using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    using Properties = Dictionary<string, int>;

    public class Problem16
    {
        public static void part1()
        {
            var ticket = TicketContent();
            var sues = Sues();
            var sue = sues.FindIndex(sue => sue.All(property => ticket[property.Key] == property.Value));
            Console.WriteLine(sue + 1);
        }

        public static void part2()
        {
            var ticket = TicketContent();
            var compare = new Dictionary<string, Func<int, bool>>()
            {
                { "cats", value => ticket["cats"] < value },
                { "trees", value => ticket["trees"] < value },
                { "pomeranians", value => ticket["pomeranians"] > value },
                { "goldfish", value => ticket["goldfish"] > value }
            };
            
            var sues = Sues();
            var sue = sues.FindIndex(sue =>
                sue.All(property =>
                {
                    if (compare.Keys.Contains(property.Key))
                    {
                        return compare[property.Key](property.Value);
                    }
                    return ticket[property.Key] == property.Value;
                })
            );
            Console.WriteLine(sue + 1);
        }

        static Properties TicketContent()
        {
            var text = System.IO.File.ReadAllLines("resources/test.txt");
            var regex = new Regex("(?<key>\\w+): (?<value>\\d+)");
            return text.Select(line =>
            {
                var groups = regex.Matches(line)[0].Groups;
                return (groups["key"].Value, int.Parse(groups["value"].Value));
            })
                .ToDictionary(pair => pair.Item1, pair => pair.Item2);
        }

        static List<Properties> Sues()
        {
            var text = System.IO.File.ReadAllLines("resources/16.txt");
            var regex = new Regex("(?<key>\\w+): (?<value>\\d+)");
            return text.Select(line =>
            {
                return regex.Matches(line)
                .Select(match => (match.Groups["key"].Value, int.Parse(match.Groups["value"].Value)))
                .ToDictionary(pair => pair.Item1, pair => pair.Item2);
            }).ToList();
        }
    }
}
