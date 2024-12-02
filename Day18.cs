using System;

namespace AdventOfCode
{
    public class Day18
    {
        private const int GRID_SIZE = 100;
        private const int STEPS = 100;

        private bool[,] lightsNow;
        private bool[,] lightsNext;

        public Day18()
        {
            Console.WriteLine("Running Day 18 - a");

            lightsNow = new bool[GRID_SIZE, GRID_SIZE];
            lightsNext = new bool[GRID_SIZE, GRID_SIZE];

            InitLights();
            RunSteps(false);
            int numLights = CountLights();

            Console.WriteLine("Number of Lights: " + numLights);

            Console.WriteLine("Running Day 18 - b");

            InitLights();
            RunSteps(true);
            TurnOnCorners();
            numLights = CountLights();

            Console.WriteLine("Number of Lights: " + numLights);
        }

        private void InitLights()
        {
            int row = 0;
            FileUtil.Parse(18, delegate(string line)
            {
                for (int col = 0; col < line.Length; ++col)
                {
                    lightsNow[row,col] = false;
                    lightsNext[row,col] = false;
                    
                    if (line[col] == '#')
                    {
                        lightsNow[row,col] = true;
                    }
                }

                ++row;
            });
        }

        private void TurnOnCorners()
        {
            lightsNow[0,0] = true;
            lightsNow[0, GRID_SIZE - 1] = true;
            lightsNow[GRID_SIZE - 1, 0] = true;
            lightsNow[GRID_SIZE - 1, GRID_SIZE - 1] = true;
        }

        private void RunSteps(bool stuckCorners)
        {
            //Console.WriteLine("Original:");
            //PrintLights();

            for (int step = 0; step < STEPS; ++step)
            {
                if (stuckCorners)
                {
                    TurnOnCorners();
                }

                for (int row = 0; row < GRID_SIZE; ++row)
                {
                    for (int col = 0; col < GRID_SIZE; ++col)
                    {
                        int numLights = 0;
                        for (int i = -1; i <= 1; ++i)
                        {
                            for (int j = -1; j <= 1; ++j)
                            {
                                if (i != 0 || j != 0)
                                {
                                    numLights += (IsLightOn(row + i, col + j) ? 1 : 0);
                                }
                            }
                        }

                        if (lightsNow[row,col])
                        {
                            lightsNext[row,col] = (numLights == 2 || numLights == 3);
                        }
                        else
                        {
                            lightsNext[row,col] = (numLights == 3);
                        }
                    }
                }

                bool[,] temp = lightsNow;
                lightsNow = lightsNext;
                lightsNext = temp;

                /*if (stuckCorners)
                {
                    TurnOnCorners();
                }

                Console.WriteLine("Step " + (step + 1) + ":");
                PrintLights();*/
            }
        }

        private bool IsLightOn(int row, int col)
        {
            if (0 <= row && row < GRID_SIZE && 0 <= col && col < GRID_SIZE)
            {
                return lightsNow[row, col];
            }
            return false;
        }

        private int CountLights()
        {
            int numLights = 0;
            for (int row = 0; row < GRID_SIZE; ++row)
            {
                for (int col = 0; col < GRID_SIZE; ++col)    
                {
                    if (lightsNow[row,col])
                    {
                        ++numLights;
                    }
                }
            }
            return numLights;
        }

        private void PrintLights()
        {
            for (int row = 0; row < GRID_SIZE; ++row)
            {
                for (int col = 0; col < GRID_SIZE; ++col)
                {
                    Console.Out.Write(lightsNow[row,col] ? '#' : '.');
                }
                Console.Out.WriteLine();
            }
            Console.Out.WriteLine();
        }
    }
}
