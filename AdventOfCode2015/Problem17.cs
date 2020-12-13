using System;
using System.Linq;
using System.Collections.Generic;
using Bitset = System.UInt32;


namespace AdventOfCode2015
{
    public class Problem17
    {
        public static void Part1()
        {
            Console.WriteLine(Combinations().Count());
        }

        public static void Part2()
        {
            var combinations = Combinations();
            var min = combinations.Min(combi => combi.CountBits());
            var count = combinations.Count(combi => combi.CountBits() == min);
            Console.WriteLine(count);
        }

        static ISet<Bitset> Combinations()
        {
            var TOTAL = 150;
            var containers = Containers();

            // combinations[total] = possible combinations in order to reach total liters
            // a combination is the bitset of indices corresponding to a given container
            var combinations = new List<ISet<Bitset>>();
            Enumerable.Range(0, TOTAL + 1)
                .ToList()
                .ForEach(_ => combinations.Add(new SortedSet<Bitset>()));
            combinations[0].Add(0);

            for (int size = 0; size < TOTAL; size++)
            {
                foreach (var combination in combinations[size])
                {
                    Enumerable.Range(0, containers.Count())
                        .ToList()
                        .FindAll(index => !combination.GetBit(index) && size + containers[index] <= TOTAL)
                        .ForEach(index => combinations[size + containers[index]].Add(combination.SettingBit(index)));
                }
            }
            return combinations[TOTAL];
        }

        static List<int> Containers()
        {
            var list = System.IO.File.ReadAllLines("resources/17.txt")
                .Select(line => int.Parse(line))
                .ToList();
            list.Sort();
            return new List<int>(list);
        }
    }
}

static class BitsetExtensions
{
    public static int CountBits(this Bitset value) =>
        Enumerable.Range(0, 32).Count(index => value.GetBit(index));

    public static bool GetBit(this Bitset value, int index) =>
        (value & (1u << index)) != 0;      

    public static Bitset SettingBit(this Bitset value, int index) =>
        value | (1u << index);   
}
