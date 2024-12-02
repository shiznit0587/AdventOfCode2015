using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class Day24
    {
        public Day24()
        {
            Console.WriteLine("Running Day 24 - a");

            List<int> weights = new List<int>();
            FileUtil.Parse(24, s => weights.Add(int.Parse(s)));
            weights.Reverse();

            long bestQuantumEntanglement = FindBestQuantumEntanglement(weights, 3);

            Console.WriteLine("Best Quantum Entanglement = " + bestQuantumEntanglement);

            Console.WriteLine("Running Day 24 - b");

            bestQuantumEntanglement = FindBestQuantumEntanglement(weights, 4);

            Console.WriteLine("Best Quantum Entanglement = " + bestQuantumEntanglement);
        }

        private long FindBestQuantumEntanglement(List<int> weights, int divisions)
        {
            int targetWeight = weights.Sum() / divisions;
            int bestCount = int.MaxValue;
            long bestQuantumEntanglement = long.MaxValue;

            HashSet<int> all = new HashSet<int>();
            all.UnionWith(weights);

            for (int count = 1; count <= weights.Count; ++count)
            {
                foreach (HashSet<int> group1 in Choices(all, count))
                {
                    int weight1 = group1.Sum();

                    if (weight1 != targetWeight)
                        continue;

                    //Console.WriteLine("group1 = " + SetString(group1));

                    HashSet<int> remaining = new HashSet<int>();
                    remaining.UnionWith(all);
                    remaining.ExceptWith(group1);

                    if (CanBeDividedEvenly(remaining, divisions - 1, targetWeight))
                    {
                        bestCount = count;
                        long quantumEntanglement1 = 1;
                        foreach (int weight in group1)
                        {
                            quantumEntanglement1 *= (long)weight;
                        }
                        
                        //Console.WriteLine("QE=" + quantumEntanglement1);

                        if (quantumEntanglement1 < bestQuantumEntanglement)
                        {
                            bestQuantumEntanglement = quantumEntanglement1;
                            //Console.WriteLine("Setting best QE = " + bestQuantumEntanglement);
                        }
                    }
                }

                if (bestQuantumEntanglement < long.MaxValue)
                    break;
            }

            return bestQuantumEntanglement;
        }

        private bool CanBeDividedEvenly(HashSet<int> source, int divisions, int targetWeight)
        {
            if (divisions == 1)
            {
                return source.Sum() == targetWeight;
            }

            foreach (HashSet<int> group in Choices(source))
            {
                int weight = group.Sum();

                if (weight != targetWeight)
                    continue;

                HashSet<int> remaining = new HashSet<int>();
                remaining.UnionWith(source);
                remaining.ExceptWith(group);

                if (CanBeDividedEvenly(remaining, divisions - 1, targetWeight))
                {
                    return true;
                }
            }

            return false;
        }

        private IEnumerable<HashSet<T>> Choices<T>(HashSet<T> source)
        {
            //Console.WriteLine("Choices1 :: source = " + SetString(source));
            for (int count = 0; count <= source.Count; ++count)
            {
                //Console.WriteLine("Choices1 :: count = " + count);
                foreach (HashSet<T> choices in Choices(source, count))
                {
                    yield return choices;
                }
                //Console.ReadLine();
            }
        }

        private IEnumerable<HashSet<T>> Choices<T>(HashSet<T> source, int count)
        {
            //Console.WriteLine("Choices2 :: source = " + SetString(source) + ", count = " + count);
            if (count <= 0)
            {
                //Console.WriteLine("Choices2 :: returning set = {}");
                yield return new HashSet<T>();
                yield break;
            }

            HashSet<T> sourceCopy = new HashSet<T>();
            sourceCopy.UnionWith(source);

            HashSet<T> result = new HashSet<T>();
            foreach (T item in source)
            {
                result.Add(item);
                sourceCopy.Remove(item);
                foreach (HashSet<T> choices in Choices(sourceCopy, count - 1))
                {
                    result.UnionWith(choices);

                    //Console.WriteLine("Choices2 :: returning set = " + SetString(result));
                    yield return result;
                    result.ExceptWith(choices);
                }
                result.Remove(item);
            }
        }

        private string SetString<T>(HashSet<T> source)
        {
            List<T> list = source.ToList();
            list.Sort();
            return "{" + String.Join(",", list) + "}";
        }
    }
}
