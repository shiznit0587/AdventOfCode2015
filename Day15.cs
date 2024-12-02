using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day15
    {
        private List<Ingredient> Ingredients;
        private int[] tspns;

        private int bestScore = 0;
        private int[] bestDistribution;

        public Day15()
        {
            Console.WriteLine("Running Day 15 - a");

            Ingredients = new List<Ingredient>();

            FileUtil.Parse(15, delegate(string line)
            {
                string[] parts = line.Split(new char[3]{':', ' ', ','});
                Ingredients.Add(new Ingredient(parts[0], Int32.Parse(parts[3]), Int32.Parse(parts[6]), Int32.Parse(parts[9]), Int32.Parse(parts[12]), Int32.Parse(parts[15])));
            });

            tspns = new int[Ingredients.Count];

            FindBestDistribution(0, 100, false);

            Console.WriteLine("Best Distribution - [" + string.Join(",", bestDistribution) + "] score = " + bestScore);

            Console.WriteLine("Running Day 15 - b");

            bestScore = 0;

            tspns = new int[Ingredients.Count];

            FindBestDistribution(0, 100, true);

            Console.WriteLine("Best Distribution - [" + string.Join(",", bestDistribution) + "] score = " + bestScore);
        }

        private void FindBestDistribution(int idx, int remaining, bool checkCalories)
        {
            if (remaining == 0 || idx == tspns.Length - 1)
            {
                tspns[idx] = remaining;

                int capacity = 0;
                int durability = 0;
                int flavor = 0;
                int texture = 0;
                int calories = 0;

                for (int i = 0; i < Ingredients.Count; ++i)
                {
                    capacity += Ingredients[i].Capacity * tspns[i];
                    durability += Ingredients[i].Durability * tspns[i];
                    flavor += Ingredients[i].Flavor * tspns[i];
                    texture += Ingredients[i].Texture * tspns[i];
                    calories += Ingredients[i].Calories * tspns[i];
                }

                capacity = Math.Max(capacity, 0);
                durability = Math.Max(durability, 0);
                flavor = Math.Max(flavor, 0);
                texture = Math.Max(texture, 0);
                calories = Math.Max(calories, 0);

                if (!checkCalories || calories == 500)
                {
                    int score = capacity * durability * flavor * texture;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestDistribution = tspns.ToArray();
                    }
                }

                tspns[idx] = 0;
            }
            else
            {
                for (int i = 0; i < remaining; ++i)
                {
                    ++tspns[idx];
                    --remaining;
                    FindBestDistribution(idx + 1, remaining, checkCalories);
                }

                tspns[idx] = 0;
            }            
        }
    }

    class Ingredient
    {
        public readonly string Name;
        public readonly int Capacity;
        public readonly int Durability;
        public readonly int Flavor;
        public readonly int Texture;
        public readonly int Calories;

        public Ingredient(string name, int capacity, int durability, int flavor, int texture, int calories)
        {
            Name = name;
            Capacity = capacity;
            Durability = durability;
            Flavor = flavor;
            Texture = texture;
            Calories = calories;
        }
    }
}