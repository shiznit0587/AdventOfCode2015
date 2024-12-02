using System;

namespace AdventOfCode
{
    public class Day5
    {
        public Day5()
        {
            Console.WriteLine("Running Day 5 - a");

            int niceNames = 0;

            FileUtil.Parse(5, delegate(string name)
            {
                int totalVowelCount = 0;

                char prevChar = ':';

                bool charPairFound = false;
                bool badPairFound = false;

                for (int i = 0; i < name.Length; ++i)
                {
                    char currentChar = name[i];

                    switch (currentChar)
                    {
                        case 'a':
                        case 'e':
                        case 'i':
                        case 'o':
                        case 'u':
                            ++totalVowelCount;
                            break;
                        
                        // I could look for bad pairs by calling "Contains" on name 4x, but that's expensive, and I'm already walking the string.
                        case 'b':
                            if (prevChar == 'a')
                                badPairFound = true;
                            break;
                        case 'd':
                            if (prevChar == 'c')
                                badPairFound = true;
                            break;
                        case 'q':
                            if (prevChar == 'p')
                                badPairFound = true;
                            break;
                        case 'y':
                            if (prevChar == 'x')
                                badPairFound = true;
                            break;
                    }

                    if (prevChar == currentChar)
                        charPairFound = true;

                    prevChar = currentChar;

                    if (badPairFound)
                        break;
                }

                if (totalVowelCount >= 3 && charPairFound && !badPairFound)
                    ++niceNames;
            });

            Console.WriteLine("total nice names = " + niceNames);

            Console.WriteLine("Running Day 5 - b");

            niceNames = 0;

            FileUtil.Parse(5, delegate(string name)
            {
                // 1) contains the same pair of letters twice. The pairs can't overlap (+ abab, - aaa)
                // 2) contains a letter that repeats with one char between them (xyx, abcdefeghi (efe), aaa)

                bool sameCharPairFound = false;
                bool repeatedCharFound = false;

                // Check rule 2 first, since it's cheaper.
                for (int i = 2; i < name.Length; ++i)
                {
                    if (name[i - 2] == name[i])
                    {
                        repeatedCharFound = true;
                        break;
                    }
                }

                // Skip the more expensive check if the simple rule check failed.
                if (repeatedCharFound)
                {
                    for (int i = 0; i < name.Length - 1; ++i)
                    {
                        String charPair = name.Substring(i, 2);
                        String leftSide = name.Substring(0, i);
                        String rightSide = name.Substring(i + 2);

                        if (leftSide.Contains(charPair) || rightSide.Contains(charPair))
                        {
                            sameCharPairFound = true;
                            break;
                        }
                    }
                }

                if (sameCharPairFound && repeatedCharFound)
                    ++niceNames;
            });

            Console.WriteLine("total nice names = " + niceNames);
        }
    }
}
