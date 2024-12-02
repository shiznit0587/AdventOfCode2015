using System;

namespace AdventOfCode
{
    public class Day20
    {
        private int INPUT = 34000000;
        public Day20()
        {
            Console.WriteLine("Running Day 20 - a");

            // We know that since each elf drops 10x as many presents as its own number,
            // Then the "INPUT/10"th house will be guaranteed to receive INPUT presents,
            // even if no other elf visits it.
            int[] houses = new int[INPUT / 10];

            // For each elf,
            for (int i = 1; i <= houses.Length; ++i)
            {
                // For each house the elf will visit,
                for (int j = i; j <= houses.Length; j = j + i)
                {
                    // Have the elf visit the house.
                    houses[j - 1] += i * 10;
                }
            }

            for (int i = 0 ; i < houses.Length; ++i)
            {
                if (houses[i] >= INPUT)
                {
                    Console.WriteLine("House=" + (i + 1) + ", Presents=" + houses[i]);
                    break;
                }
            }

            Console.WriteLine("Running Day 20 - a");

            // For part 2, each elf is dropping 11x as many presents as its own number.
            houses = new int[INPUT / 11];

            // For each elf,
            for (int i = 1; i < houses.Length; ++i)
            {
                int housesVisited = 0;
                    
                // For each house the elf will visit,
                for (int j = i; j <= houses.Length; j = j + i)
                {
                    // Have the elf visit the house.
                    houses[j - 1] += i * 11;

                    // If the elf has visited 50 houses, it's done.
                    if (++housesVisited >= 50)
                    {
                        break;
                    }
                }
            }

            for (int i = 0 ; i < houses.Length; ++i)
            {
                if (houses[i] >= INPUT)
                {
                    Console.WriteLine("House=" + (i + 1) + ", Presents=" + houses[i]);
                    break;
                }
            }
        }
    }
}
