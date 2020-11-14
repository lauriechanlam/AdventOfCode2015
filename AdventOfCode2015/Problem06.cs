using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    public class Problem06
    {
        public static void part1()
        {
            var lights = Enumerable.Repeat(false, 1000*1000).ToArray();
            string[] instructions = Problem06.text();
            var regex = new Regex("(?<action>turn on|turn off|toggle) (?<minX>\\d*),(?<minY>\\d*) through (?<maxX>\\d*),(?<maxY>\\d*)");
            foreach (var instruction in instructions)
            {
                var groups = regex.Matches(instruction)[0].Groups;
                for (var x = int.Parse(groups["minX"].Value); x <= int.Parse(groups["maxX"].Value); x += 1)
                {
                    for (var y = int.Parse(groups["minY"].Value); y <= int.Parse(groups["maxY"].Value); y += 1)
                    {
                        int index = 1000 * x + y;
                        switch (groups["action"].Value)
                        {
                            case "turn on": lights[index] = true; break;
                            case "turn off": lights[index] = false; break;
                            case "toggle": lights[index] = !lights[index]; break;
                        }
                    }
                }
            }
            Console.WriteLine(lights.Count(light => light == true));
        }

        public static void part2()
        {
            var lights = Enumerable.Repeat(0, 1000*1000).ToArray();
            string[] instructions = Problem06.text();
            var regex = new Regex("(?<action>turn on|turn off|toggle) (?<minX>\\d*),(?<minY>\\d*) through (?<maxX>\\d*),(?<maxY>\\d*)");
            foreach (var instruction in instructions)
            {
                var groups = regex.Matches(instruction)[0].Groups;
                for (var x = int.Parse(groups["minX"].Value); x <= int.Parse(groups["maxX"].Value); x += 1)
                {
                    for (var y = int.Parse(groups["minY"].Value); y <= int.Parse(groups["maxY"].Value); y += 1)
                    {
                        int index = 1000 * x + y;
                        switch (groups["action"].Value)
                        {
                            case "turn on": lights[index] += 1; break;
                            case "turn off": lights[index] = Math.Max(0, lights[index] - 1); break;
                            case "toggle": lights[index] += 2; break;
                        }
                    }
                }
            }
            var brightness = 0;
            foreach (var light in lights)
            {
                brightness += light;
            }
            Console.WriteLine(brightness);
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/06.txt");
        }
    }
}
