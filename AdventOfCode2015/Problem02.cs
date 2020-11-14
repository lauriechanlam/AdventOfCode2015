using System;
using System.Linq;

namespace AdventOfCode2015
{
    public class Problem02 {
        public static void part1()
        {
            string[] presents = Problem02.text();
            int surface = 0;
            foreach (var present in presents)
            {
              var dimensions = present.Split('x');
              if (dimensions.Count() != 3)
              {
                continue;
              }
              var w = int.Parse(dimensions[0]);
              var l = int.Parse(dimensions[1]);
              var h = int.Parse(dimensions[2]);
              surface += 2*w*l + 2*w*h + 2*l*h + Math.Min(Math.Min(w*l, w*h), l*h);
            }
            Console.WriteLine(surface);
        }

        public static void part2()
        {
            string[] presents = Problem02.text();
            var ribbon = 0;
            foreach (var present in presents)
            {
              var dimensions = present.Split('x');
              if (dimensions.Count() != 3)
              {
                continue;
              }
              var w = int.Parse(dimensions[0]);
              var l = int.Parse(dimensions[1]);
              var h = int.Parse(dimensions[2]);
              ribbon += 2*Math.Min(Math.Min(w+l, w+h), l+h) + w*l*h;
            }
            Console.WriteLine(ribbon);
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/02.txt");
        }
    }
}
