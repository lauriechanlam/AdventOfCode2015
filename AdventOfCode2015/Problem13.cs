using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    public class Problem13
    {
        public static void part1()
        {
            var happiness = happinessMatrix(text());
            var graph = new Graph(happiness);
            var permutations = graph.Permutations();
            var values = permutations.Select(permutation => graph.ComputeDistance(permutation));
            Console.WriteLine(values.Max());
        }

        public static void part2()
        {
            var happiness = happinessMatrix(text());
            var graph = new Graph(happiness);
            graph.AddVertex("Jean Micheng");
            var permutations = graph.Permutations();
            var values = permutations.Select(permutation => graph.ComputeDistance(permutation));
            Console.WriteLine(values.Max());
        }

        static String[] text()
        {
            return System.IO.File.ReadAllLines("resources/13.txt");
        }

        class Graph
        {
            private Dictionary<(string, string), int> matrix;
            private ISet<string> vertices;

            public Graph(Dictionary<(string, string), int> matrix)
            {
                this.matrix = matrix;
                var vertices = new HashSet<string>();
                foreach (var entry in matrix)
                {
                    vertices.Add(entry.Key.Item1);
                    vertices.Add(entry.Key.Item2);
                }
                this.vertices = vertices;
            }

            public void AddVertex(string vertex)
            {
                foreach (var v in vertices)
                {
                    matrix[(v, vertex)] = 0;
                    matrix[(vertex, v)] = 0;
                }
                vertices.Add(vertex);
            }

            public struct VertexList
            {
                private readonly List<string> vertices;
                private readonly List<string> reveredVertices;

                public VertexList(List<string> vertices)
                {
                    this.vertices = vertices;
                    var reversedVertices = new List<string>(vertices);
                    reversedVertices.Reverse();
                    this.reveredVertices = reversedVertices;
                }

                public VertexList(VertexList list)
                {
                    vertices = new List<string>(list.vertices);
                    reveredVertices = new List<string>(list.reveredVertices);
                }

                public int Count()
                {
                    return vertices.Count();
                }

                public void Insert(int index, string vertex)
                {
                    vertices.Insert(index, vertex);
                    reveredVertices.Insert(reveredVertices.Count() - index, vertex);
                }

                public override int GetHashCode()
                {
                    return vertices.GetHashCode() + reveredVertices.GetHashCode();
                }

                public List<string> GetVertices()
                {
                    return vertices;
                }
            }

            public ISet<VertexList> Permutations()
            {
                ISet<VertexList> AddVertex(string vertex, VertexList list)
                {
                    var permutations = new HashSet<VertexList>();
                    for (var i = 0; i <= list.Count(); i++)
                    {
                        var newList = new VertexList(list);
                        newList.Insert(i, vertex);
                        permutations.Add(newList);
                    }
                    return permutations;
                }

                var permutations = new HashSet<VertexList>();
                permutations.Add(new VertexList(new List<string>()));
                foreach (var vertex in vertices)
                {
                    permutations = new HashSet<VertexList>(
                        permutations.SelectMany(permutation => AddVertex(vertex, permutation)));
                }

                return permutations;
            }

            public int ComputeDistance(VertexList list)
            {
                var vertices = list.GetVertices();
                var count = matrix[(vertices.First(), vertices.Last())] + matrix[(vertices.Last(), vertices.First())];
                for (int i = 1; i < vertices.Count(); i++)
                {
                    count += matrix[(vertices[i], vertices[i-1])] + matrix[(vertices[i-1], vertices[i])];
                }
                return count;
            }
        }

        static Dictionary<(string, string), int> happinessMatrix(string[] lines)
        {
            var happiness = new Dictionary<(string, string), int>();
            var regex = new Regex(@"(?<first>\w+) would (?<gain>gain|lose) (?<units>\d+) happiness units by sitting next to (?<second>\w+)\.");
            foreach (var line in lines)
            {
                var groups = regex.Matches(line)[0].Groups;
                var gain = groups["gain"].Value == "gain" ? 1 : -1;
                happiness[(groups["first"].Value, groups["second"].Value)] = gain * int.Parse(groups["units"].Value);
            }
            return happiness;
        }
    }
}
