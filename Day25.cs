using System;

namespace AdventOfCode
{
    public class Day25
    {
        public Day25()
        {
            Console.WriteLine("Running Day 25 - a");

            int endRow = 2947 - 1;
            int endColumn = 3029 - 1;

            int finalRow = endRow + endColumn + 1;

            //long[,] test = new long[finalRow,finalRow];

            int row = 0;
            int column = -1;
            long prev = 20151125;

            while (column < finalRow)
            {
                row = column;
                column = 0;

                while (row >= 0)
                {
                    if (row != 0 || column != 0)
                    {
                        prev = (prev * 252533) % 33554393;
                    }

                    //test[row, column] = prev;

                    if (row == endRow && column == endColumn)
                    {
                        row = -1;
                        column = finalRow;
                    }

                    ++column;
                    --row;
                }
            }

            /*Console.WriteLine("   |    0         1         2         3         4         5         6         7         8         9        10");
            Console.WriteLine("---+---------+---------+---------+---------+---------+---------+---------+---------+---------+---------+---------+");

            for (int i = 0; i < 11; ++i)
            {
                Console.WriteLine("{0,2} | {1,9} {2,9} {3,9} {4,9} {5,9} {6,9} {7,9} {8,9} {9,9} {10,9} {11,9}", 
                    i, test[i,0], test[i,1], test[i,2], test[i,3], test[i,4], test[i,5], test[i,6], test[i,7], test[i,8], test[i,9], test[i,10]);
            }*/

            Console.WriteLine("Code = " + /*test[endRow, endColumn]*/ prev);
        }
    }
}
