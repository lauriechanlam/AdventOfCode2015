using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    public class Problem09
    {
        public static void part1()
        {
            var lines = Problem09.text();
            var network = new Network(lines);
            var node = new Node(network.CityCount());
            var visitor = new Visitor(network);
            var shortest = visitor.Visit(node, true);
            Console.WriteLine(shortest);
        }

        public static void part2()
        {
            var lines = Problem09.text();
            var network = new Network(lines);
            var node = new Node(network.CityCount());
            var visitor = new Visitor(network);
            var longest = visitor.Visit(node, false);
            Console.WriteLine(longest);
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/09.txt");
        }

        class Network
        {
            List<string> cities;
            Dictionary<(int, int), int> distances;

            public Network(string[] lines)
            {
                var regex = new Regex(@"(?<city1>\w+) to (?<city2>\w+) = (?<distance>\d+)");
                var cities = new List<string>();
                var distances = new Dictionary<(int, int), int>();
                foreach (var line in lines)
                {
                    var groups = regex.Matches(line)[0].Groups;
                    var city1Index = cities.IndexOf(groups["city1"].Value);
                    if (city1Index == -1)
                    {
                        city1Index = cities.Count();
                        cities.Add(groups["city1"].Value);
                    }
                    var city2Index = cities.IndexOf(groups["city2"].Value);
                    if (city2Index == -1)
                    {
                        city2Index = cities.Count();
                        cities.Add(groups["city2"].Value);
                    }

                    distances[(Math.Min(city1Index, city2Index), Math.Max(city1Index, city2Index))] = int.Parse(groups["distance"].Value);
                }
                this.cities = cities;
                this.distances = distances;
            }

            public int CityCount()
            {
                return this.cities.Count();
            }

            public int Distance(int city1, int city2)
            {
                var min = Math.Min(city1, city2);
                var max = Math.Max(city1, city2);
                if (min < 0 || max >= CityCount())
                {
                    return 0;
                }
                return this.distances[(min, max)];
            }
        }

        class Node
        {
            public readonly int city;
            public readonly Node parent;
            public readonly List<Node> children;

            public Node(int cityCount): this(-1, null, cityCount)
            {
            }

            Node(int city, Node parent, int cityCount)
            {
                this.city = city;
                this.parent = parent;
                this.children = new List<Node>();
                var availableCities = this.GetAvailableCities(cityCount);
                foreach (var c in availableCities)
                {
                    this.children.Add(new Node(c, this, cityCount));
                }
            }

            ISet<int> GetAvailableCities(int cityCount)
            {
                var availableCities = new HashSet<int>();
                for (var c = 0; c < cityCount; c++)
                {
                    availableCities.Add(c);
                }
                var node = this;
                while (node != null)
                {
                    availableCities.Remove(node.city);
                    node = node.parent;
                }
                return availableCities;
            }
        }

        class Visitor
        {
            Network network;

            public Visitor(Network network)
            {
                this.network = network;
            }

            public int Visit(Node node, Boolean isMin)
            {
                if (node.children.Count() == 0)
                {
                    return 0;
                }
                if (isMin)
                {
                    return node.children.Min(child => Visit(child, isMin) + network.Distance(node.city, child.city));
                }
                return node.children.Max(child => Visit(child, isMin) + network.Distance(node.city, child.city));
            }
        }
    }
}
