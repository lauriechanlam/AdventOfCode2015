using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    public class Problem14
    {
        public static void part1()
        {
            var reindeers = GetReindeers(text());
            var distances = reindeers.Select(reindeer => reindeer.DistanceAfter(2503));
            Console.WriteLine(distances.Max());
        }

        class Reindeer
        {
            public readonly string name;
            public readonly int speed;
            public readonly int time;
            public readonly int rest_time;
            public int score;

            public Reindeer(string name, int speed, int time, int rest_time)
            {
                this.name = name;
                this.speed = speed;
                this.time = time;
                this.rest_time = rest_time;
                this.score = 0;
            }

            public int DistanceAfter(int seconds)
            {
                int cycle_time = time + rest_time;
                int cycle_count = seconds / cycle_time;
                var remaining_seconds = seconds % cycle_time;
                return speed * (cycle_count * time + Math.Min(remaining_seconds, time));
            }
        }

        public static void part2()
        {
            var reindeers = GetReindeers(text());
            for (var seconds = 1; seconds <= 2503; seconds++)
            {
                var distances = reindeers.Select(reindeer => reindeer.DistanceAfter(seconds)).ToList();
                var maxDistance = distances.Max();
                reindeers
                    .Zip(distances)
                    .ToList()
                    .FindAll(pair => pair.Item2 == maxDistance)
                    .ForEach(pair => pair.Item1.score += 1);
            }
            
            Console.WriteLine(reindeers.Max(reindeer => reindeer.score));
        }

        static string[] text()
        {
                return System.IO.File.ReadAllLines("resources/14.txt");
        }

        static List<Reindeer> GetReindeers(string[] lines)
        {
            var regex = new Regex("(?<name>[\\w ?]+) can fly (?<speed>\\d+) km/s for (?<time>\\d+) seconds, but then must rest for (?<rest_time>\\d+) seconds.");
            return lines.Select(line =>
            {
                var groups = regex.Matches(line)[0].Groups;
                return new Reindeer(
                    groups["name"].Value,
                    int.Parse(groups["speed"].Value),
                    int.Parse(groups["time"].Value),
                    int.Parse(groups["rest_time"].Value));
            }).ToList();
        }
    }
}
