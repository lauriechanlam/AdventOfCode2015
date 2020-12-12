using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2015
{
    using Ingredient = Dictionary<string, int>;

    public class Problem15
    {
        public static void part1()
        {
            var max = FindRecipes()
                .Select(recipe => recipe.Value())
                .Max();

            Console.WriteLine(max);
        }

        public static void part2()
        {
            var max = FindRecipes()
                .FindAll(recipe => recipe.TotalCalories() == 500)
                .Select(recipe => recipe.Value())
                .Max();
            Console.WriteLine(max);
        }

        static string[] Text()
        {
            return System.IO.File.ReadAllLines("resources/15.txt");
        }

        private static List<Ingredient> GetIngredients(string[] lines)
        {
            var regex = new Regex("(?<key>\\w+) (?<value>-?\\d+)");
            return lines.Select(line =>
                regex
                    .Matches(line)
                    .ToDictionary(
                        match => match.Groups["key"].Value,
                        match => int.Parse(match.Groups["value"].Value))
            ).ToList();
        }

        static List<Recipe> FindRecipes()
        {
            int targetSpoonfulCount = 100;
            var ingredients = GetIngredients(Text());
            List<Recipe> recipes = new List<Recipe> { new Recipe() };
            foreach (var ingredient in ingredients)
            {
                var newRecipes = recipes.SelectMany(recipe =>
                {
                    int maxSpoonful = targetSpoonfulCount - recipe.SpoonfulsCount();
                    return Enumerable.Range(1, maxSpoonful)
                        .Select(spoonfulCount => new Recipe(recipe, ingredient, spoonfulCount));
                }).ToList();
                recipes.AddRange(newRecipes);
            }
            return recipes
                .FindAll(recipe => recipe.SpoonfulsCount() == targetSpoonfulCount);
        }

        class Recipe
        {
            readonly Dictionary<Ingredient, int> ingredients;

            public Recipe()
            {
                ingredients = new Dictionary<Ingredient, int>();
            }

            public Recipe(Recipe other, Ingredient ingredient, int quantity)
            {
                ingredients = new Dictionary<Ingredient, int>(other.ingredients)
                {
                    { ingredient, quantity }
                };
            }

            public int SpoonfulsCount()
            {
                return ingredients.Values.Sum();
            }

            public int Value()
            {
                var properties = new List<string> { "capacity", "durability", "flavor", "texture" };
                var values = properties
                    .Select(property =>
                        ingredients
                            .Select(pair => pair.Value * pair.Key[property])
                            .Sum()
                ).ToList();
                var value = 1;
                values.ForEach(v => value *= Math.Max(v, 0));
                return value;
            }

            public int TotalCalories()
            {
                return ingredients.Select(pair => pair.Key["calories"] * pair.Value).Sum();
            }
        }
    }
}
