using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day3
    {
        HashSet<House> houses;
        House santa;
        House robo;

        public Day3()
        {
            Console.WriteLine("Running Day 3 - a");

            _InitState();

            FileUtil.Parse(3, delegate(string input)
            {
                for (int i = 0; i < input.Length; ++i)
                {
                    santa = santa.Move(input[i]);
                    _VisitHouse(santa);
                }
            });

            Console.WriteLine("houses visited = " + houses.Count);

            Console.WriteLine("Running Day 3 - b");

            _InitState();

            FileUtil.Parse(3, delegate(string input)
            {
                for (int i = 0; i < input.Length; ++i)
                {
                    House currentSanta = (i % 2 == 0) ? santa : robo;

                    currentSanta = currentSanta.Move(input[i]);
                    _VisitHouse(currentSanta);

                    if (i % 2 == 0)
                        santa = currentSanta;
                    else
                        robo = currentSanta;
                }
            });

            Console.WriteLine("houses visited = " + houses.Count);
        }

        private void _InitState()
        {
            santa = new House();
            robo = new House();
            houses = new HashSet<House>();
            houses.Add(santa);
        }

        private void _VisitHouse(House house)
        {
            if (!houses.Contains(house))
            {
                houses.Add(house);
            }
        }

        private class House : Tuple<int,int>
        {
            public House() : base(0,0) {}
            public House(int x, int y) : base(x, y) {}

            public House Move(char direction)
            {
                switch (direction)
                {
                    case '<':
                        return new House(Item1-1, Item2);
                    case '>':
                        return new House(Item1+1, Item2);
                    case '^':
                        return new House(Item1, Item2-1);
                    case 'v':
                        return new House(Item1, Item2+1);
                    default:
                        return null;
                }
            }
        }
    }            
}
