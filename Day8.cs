using System;

namespace AdventOfCode
{
    public class Day8
    {
        public Day8()
        {
            Console.WriteLine("Running Day 8 - a");

            int totalLiteral = 0;
            int totalMemory = 0;
            int totalQuoted = 0;

            FileUtil.Parse(8, delegate(string line)
            {
                int literalCount = line.Length;
                int quotedCount = line.Length + 4;
                int memoryCount = line.Length - 2;

                for (int i = 1; i < line.Length - 1; ++i)
                {
                    if (line[i] == '\\')
                    {
                        ++quotedCount;

                        switch (line[i+1])
                        {
                            case '\\':
                            case '\"':
                                --memoryCount;
                                ++quotedCount;
                                ++i;
                                break;
                            case 'x':
                                memoryCount -= 3;
                                i += 3;
                                break;
                        }
                    }
                }

                totalLiteral += literalCount;
                totalMemory += memoryCount;
                totalQuoted += quotedCount;
            });

            Console.WriteLine("literal = " + totalLiteral + ", memory = " + totalMemory + ", diff = " + (totalLiteral - totalMemory));

            Console.WriteLine("Running Day 8 - b");

            Console.WriteLine("quoted = " + totalQuoted + ", literal = " + totalLiteral + ", diff = " + (totalQuoted - totalLiteral));
        }
    }
}
