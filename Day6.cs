using System;
using System.Linq;
using Point = System.Tuple<int, int>;

namespace AdventOfCode
{
    public class Day6
    {
        bool[,] lights;
        int[,] lightsWithBrightness;

        delegate void AdjustLightsDel(int x, int y);

        public Day6()
        {
            Console.WriteLine("Running Day 6 - a");

            lights = new bool[1000, 1000];
            lightsWithBrightness = new int[1000, 1000];

            FileUtil.Parse(6, delegate(string instruction)
            {
                String[] instructionParts = null;
                AdjustLightsDel adjustLightsMethod1;
                AdjustLightsDel adjustLightsMethod2;

                if (instruction.StartsWith("turn on"))
                {
                    instructionParts = instruction.Substring(8).Split(' ');
                    adjustLightsMethod1 = _TurnLightOn;
                    adjustLightsMethod2 = _BrightenLight;
                }
                else if (instruction.StartsWith("turn off"))
                {
                    instructionParts = instruction.Substring(9).Split(' ');
                    adjustLightsMethod1 = _TurnLightOff;
                    adjustLightsMethod2 = _DimLight;
                }
                else // if (instruction.StartsWith("toggle"))
                {
                    instructionParts = instruction.Substring(7).Split(' ');
                    adjustLightsMethod1 = _ToggleLight;
                    adjustLightsMethod2 = _DoubleBrightenLight;
                }

                Point start = _ParsePoint(instructionParts[0]);
                Point end = _ParsePoint(instructionParts[2]);

                for (int x = start.Item1; x <= end.Item1; ++x)
                {
                    for (int y = start.Item2; y <= end.Item2; ++y)
                    {
                        adjustLightsMethod1(x, y);
                        adjustLightsMethod2(x, y);
                    }
                }
            });

            int lightsOn = lights.Cast<bool>().Where(l => l).Count();

            Console.WriteLine("Lights on = " + lightsOn);

            Console.WriteLine("Running Day 6 - b");

            int totalBrightness = lightsWithBrightness.Cast<int>().Sum();

            Console.WriteLine("Total Brightness = " + totalBrightness);
        }

        Point _ParsePoint(String input)
        {
            String[] tupleParts = input.Split(',');
            return new Point(Int32.Parse(tupleParts[0]), Int32.Parse(tupleParts[1]));
        }

        void _TurnLightOn(int x, int y)
        {
            lights[x, y] = true;
        }

        void _TurnLightOff(int x, int y)
        {
            lights[x, y] = false;
        }

        void _ToggleLight(int x, int y)
        {
            lights[x, y] = !lights[x, y];
        }

        void _BrightenLight(int x, int y)
        {
            ++lightsWithBrightness[x, y];
        }

        void _DimLight(int x, int y)
        {
            lightsWithBrightness[x, y] = Math.Max(--lightsWithBrightness[x, y], 0);
        }

        void _DoubleBrightenLight(int x, int y)
        {
            lightsWithBrightness[x, y] += 2;
        }
    }
}
