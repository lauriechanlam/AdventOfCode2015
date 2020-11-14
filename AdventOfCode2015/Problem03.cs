using System;
using System.Collections.Generic;

namespace AdventOfCode2015
{
    public class Problem03
    {
        public static void part1()
        {
            string text = Problem03.text();
            var currentLocation = (0, 0);
            var visitedLocations = new HashSet<(int, int)>();
            visitedLocations.Add(currentLocation);

            foreach (var c in text)
            {
                currentLocation = move(currentLocation, c);
                visitedLocations.Add(currentLocation);
            }
            
            Console.WriteLine(visitedLocations.Count);
        }

        public static void part2()
        {
            string text = Problem03.text();
            var santa = (0, 0);
            var roboSanta = (0, 0);
            var visitedLocations = new HashSet<(int, int)>();
            visitedLocations.Add(santa);
            visitedLocations.Add(roboSanta);
            var isRoboSanta = false;

            foreach (var c in text)
            {
                if (isRoboSanta)
                {
                    roboSanta = move(roboSanta, c);
                    visitedLocations.Add(roboSanta);
                } else
                {
                    santa = move(santa, c);
                    visitedLocations.Add(santa);
                }
                isRoboSanta = !isRoboSanta;
            }
            
            Console.WriteLine(visitedLocations.Count);
        }

        static (int, int) move((int, int) location, char c)
        {
            switch (c)
            {
                case '<': location.Item1 -= 1; break;
                case '>': location.Item1 += 1; break;
                case '^': location.Item2 -= 1; break;
                case 'v': location.Item2 += 1; break;
            }
            return location;
        }

        static String text()
        {
            return System.IO.File.ReadAllText("resources/03.txt");
        }
    }
}
