using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day17
    {
        private List<int> _containers;
        private int _liters = 150;

        private int _combinations = 0;

        private Dictionary<int, int> _combinationsByContainerCount = new Dictionary<int, int>();

        public Day17()
        {
            Console.WriteLine("Running Day 17 - a");

            _containers = new List<int>();
            List<bool> filledIdxs = new List<bool>();

            FileUtil.Parse(17, delegate(string line)
            {
                _containers.Add(Int32.Parse(line));
                filledIdxs.Add(false);
            });

            FillContainer(0, 0, filledIdxs);

            Console.WriteLine("Combinations = " + _combinations);

            Console.WriteLine("Running Day 17 - b");

            int minContainers = _combinationsByContainerCount.Keys.Min();

            Console.WriteLine("Min Containers = " + minContainers + ", Combinations = " + _combinationsByContainerCount[minContainers]);
        }

        private void FillContainer(int idx, int capacity, List<bool> filledIdxs)
        {
            if (idx == _containers.Count)
            {
                return;
            }

            capacity += _containers[idx];
            filledIdxs[idx] = true;

            if (capacity == _liters)
            {
                IncrementCombinations(filledIdxs);
            }
            else
            {
                FillContainer(idx + 1, capacity, filledIdxs);
            }

            capacity -= _containers[idx];
            filledIdxs[idx] = false;

            if (capacity == _liters)
            {
                IncrementCombinations(filledIdxs);
            }
            else
            {
                FillContainer(idx + 1, capacity, filledIdxs);
            }
        }

        private void IncrementCombinations(List<bool> filledIdxs)
        {
            ++_combinations;
            //Console.WriteLine("Combination Found :: " + String.Join("", filledIdxs.Select(i => i ? "T" : "F")));
            int containerCount = filledIdxs.Where(i => i).Count();
            if (_combinationsByContainerCount.ContainsKey(containerCount))
            {
                _combinationsByContainerCount[containerCount]++;
            }
            else
            {
                _combinationsByContainerCount[containerCount] = 1;
            }
        }
    }
}
