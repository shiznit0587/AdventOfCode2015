using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day9
    {
        Dictionary<string, int> cityNameToIdMap;
        Dictionary<int, string> cityIdToNameMap;
        Dictionary<Tuple<int, int>, int> weightMap;
        Stack<int> cityStack;
        int[,] weights;
        int shortestRouteLength = Int32.MaxValue;
        int longestRouteLength = Int32.MinValue;

        public Day9()
        {
            Console.WriteLine("Running Day 9 - a");

            cityNameToIdMap = new Dictionary<string, int>();
            cityIdToNameMap = new Dictionary<int, string>();
            weightMap = new Dictionary<Tuple<int, int>, int>();

            FileUtil.Parse(9, delegate(string input)
            {
                string[] parts = input.Split(' ');
                addConnection(parts[0], parts[2], Int32.Parse(parts[4]));
            });

            int numCities = cityNameToIdMap.Count;
            weights = new int[numCities, numCities];

            for (int i = 0; i < numCities; ++i)
            {
                for (int j = 0; j < numCities; ++j)
                {
                    weights[i, j] = Int32.MaxValue;
                }
            }

            foreach (Tuple<int, int> tuple in weightMap.Keys)
            {
                weights[tuple.Item1, tuple.Item2] = weightMap[tuple];
                weights[tuple.Item2, tuple.Item1] = weightMap[tuple];
            }

            shortestRouteLength = Int32.MaxValue;

            bool[] visitedCities = new bool[numCities];

            int currentRouteLength = 0;

            cityStack = new Stack<int>();

            for (int i = 0; i < numCities; ++i)
            {
                visitedCities[i] = true;
                cityStack.Push(i);
                visitNextCity(visitedCities, currentRouteLength, i);
                cityStack.Pop();
                visitedCities[i] = false;
            }

            Console.WriteLine("shortest route = " + shortestRouteLength);

            Console.WriteLine("Running Day 9 - b");

            Console.WriteLine("longest route = " + longestRouteLength);
        }

        private void addConnection(string city1, string city2, int weight)
        {
            int city1Index = 0;
            int city2Index = 0;

            getCityIndex(city1, out city1Index);
            getCityIndex(city2, out city2Index);

            weightMap[new Tuple<int, int>(city1Index, city2Index)] = weight;
        }

        private void getCityIndex(string city, out int cityIndex)
        {
            if (cityNameToIdMap.ContainsKey(city))
            {
                cityIndex = cityNameToIdMap[city];
            }
            else
            {
                cityIndex = cityNameToIdMap.Count;
                cityNameToIdMap.Add(city, cityIndex);
                cityIdToNameMap.Add(cityIndex, city);
            }
        }

        private bool allVisited(bool[] citiesVisited)
        {
            foreach (bool cityVisited in citiesVisited)
            {
                if (!cityVisited)
                {
                    return false;
                }
            }
            return true;
        }

        private void visitNextCity(bool[] visitedCities, int currentRouteLength, int currentCity)
        {
            int numCities = visitedCities.Length;

            for (int i = 0; i < numCities; ++i)
            {
                if (!visitedCities[i])
                {   
                    visitedCities[i] = true;
                    cityStack.Push(i);
                    currentRouteLength += weights[currentCity, i];

                    if (allVisited(visitedCities))
                    {
                        if (currentRouteLength < shortestRouteLength)
                        {
                            shortestRouteLength = currentRouteLength;
                        }
                        if (currentRouteLength > longestRouteLength)
                        {
                            longestRouteLength = currentRouteLength;
                        }
                    }
                    else
                    {
                        visitNextCity(visitedCities, currentRouteLength, i);
                    }

                    currentRouteLength -= weights[currentCity, i];
                    cityStack.Pop();
                    visitedCities[i] = false;
                }
            }
        }
    }
}
