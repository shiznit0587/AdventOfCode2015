using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day13
    {
        private List<string> names = new List<string>();
        
        public Day13()
        {
            Console.WriteLine("Running Day 13 - a");

            List<Instruction> instructions = new List<Instruction>();

            FileUtil.Parse(13, delegate(string line)
            {
                string[] parts = line.Split(new char[2]{' ', '.'});

                int weight = Int32.Parse(parts[3]);

                if (parts[2] == "lose")
                {
                    weight = -weight;
                }

                int fromId = GetNameId(parts[0]);
                int toId = GetNameId(parts[10]);

                instructions.Add(new Instruction(fromId, toId, weight));
            });

            int optimalHappiness = CalculateOptimalHappiness(instructions, names.Count);

            Console.WriteLine("Optimal Happiness - " + optimalHappiness);

            Console.WriteLine("Running Day 13 - b");

            int myId = GetNameId("Philip");

            for (int i = 0; i < names.Count; ++i)
            {
                if (i != myId)
                {
                    instructions.Add(new Instruction(i, myId, 0));
                    instructions.Add(new Instruction(myId, i, 0));
                }
            }

            optimalHappiness = CalculateOptimalHappiness(instructions, names.Count);

            Console.WriteLine("Optimal Happiness - " + optimalHappiness);
        }

        private int CalculateOptimalHappiness(List<Instruction> instructions, int numAttendees)
        {
            // Convert the instructions into a table.
            int[,] weights = new int[numAttendees, numAttendees];

            foreach (Instruction instr in instructions)
            {
                weights[instr.FromId, instr.ToId] = instr.Weight;
            }

            // Optimization: only permute n-1 attendees, as the table is circular.
            //int[] ids = Enumerable.Range(0, numAttendees).ToArray();
            int[] ids = Enumerable.Range(1, numAttendees - 1).ToArray();

            int optimalHappiness = Int32.MinValue;

            foreach (int[] arrangement in Permutations(ids))
            {
                int happiness = 0;

                for (int i = 0; i < arrangement.Length - 1; ++i)
                {
                    happiness += weights[arrangement[i], arrangement[i+1]];
                    happiness += weights[arrangement[i+1], arrangement[i]];
                }

                //happiness += weights[arrangement[0], arrangement.Last()];
                //happiness += weights[arrangement.Last(), arrangement[0]];

                happiness += weights[0, arrangement[0]];
                happiness += weights[arrangement[0], 0];
                happiness += weights[0, arrangement.Last()];
                happiness += weights[arrangement.Last(), 0];

                if (happiness > optimalHappiness)
                {
                    optimalHappiness = happiness;
                }
            }

            return optimalHappiness;
        }

        private int GetNameId(string name)
        {
            int id = names.IndexOf(name);
            if (id == -1)
            {
                id = names.Count;
                names.Add(name);
            }
            return id;
        }

        public IEnumerable<T[]> Permutations<T>(T[] values, int fromInd = 0)
        {
            if (fromInd + 1 == values.Length)
            {
                yield return values;
            }
            else
            {
                foreach (T[] v in Permutations(values, fromInd + 1))
                {
                    yield return v;
                }

                for (int i = fromInd + 1; i < values.Length; i++)
                {
                    SwapValues(values, fromInd, i);
                    foreach (T[] v in Permutations(values, fromInd + 1))
                    {
                        yield return v;
                    }
                    SwapValues(values, fromInd, i);
                }
            }
        }

        private void SwapValues<T>(T[] values, int pos1, int pos2)
        {
            if (pos1 != pos2)
            {
                T tmp = values[pos1];
                values[pos1] = values[pos2];
                values[pos2] = tmp;
            }
        }

        private class Instruction
        {
            public readonly int FromId;
            public readonly int ToId;
            public readonly int Weight;

            public Instruction(int fromId, int toId, int weight)
            {
                FromId = fromId;
                ToId = toId;
                Weight = weight;
            }
        }
    }
}