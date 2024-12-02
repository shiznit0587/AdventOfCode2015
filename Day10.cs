using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day10
    {
        public Day10()
        {
            string input = "1321131112";

            Console.WriteLine("Running Day 10 - a");

            List<int> result = new List<int>();

            for (int i = 0; i < input.Length; ++i)
            {
                result.Add(Int32.Parse(input[i].ToString()));
            }

            for (int i = 1; i <= 50; ++i)
            {
                List<int> previousResult = result;
                result = new List<int>();

                int lastSeenDigit = previousResult[0];
                int count = 1;
                for (int j = 1; j < previousResult.Count; ++j)
                {
                    if (previousResult[j] == lastSeenDigit)
                    {
                        ++count;
                    }
                    else
                    {
                        result.Add(count);
                        result.Add(lastSeenDigit);
                        lastSeenDigit = previousResult[j];
                        count = 1;
                    }
                }

                result.Add(count);
                result.Add(lastSeenDigit);

                if (i == 40)
                {
                    Console.WriteLine("result length = " + result.Count);
                    Console.WriteLine("Running Day 10 - b");
                }
            }

            Console.WriteLine("result length = " + result.Count);
        }
    }
}
