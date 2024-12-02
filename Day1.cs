using System;

namespace AdventOfCode
{
    public class Day1
    {
        public Day1()
        {
            Console.WriteLine("Running Day 1 - a");

            int floor = 0;
            int firstBasementPosition = -1;

            FileUtil.Parse(1, delegate(string input)
            {
                for (int i = 0; i < input.Length; ++i)
                {
                    if (input[i] == '(')
                    {
                        ++floor;
                    }
                    else
                    {
                        --floor;
                    }

                    if (firstBasementPosition == -1 && floor < 0)
                    {
                        firstBasementPosition = i + 1;
                    }
                }
            });

            Console.WriteLine("final floor = " + floor);

            Console.WriteLine("Running Day 1 - b");

            Console.WriteLine("Entered the basement at position: " + firstBasementPosition);
        }
    }
}
