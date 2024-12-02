using System;

namespace AdventOfCode
{
    public class Day2
    {
        public Day2()
        {
            Console.WriteLine("Running Day 2 - a");

            int totalPaper = 0;
            int totalRibbon = 0;
            FileUtil.Parse(2, delegate(string gift)
            {
                String[] dimensions = gift.Split('x');

                int w = Int32.Parse(dimensions[0]);
                int h = Int32.Parse(dimensions[1]);
                int l = Int32.Parse(dimensions[2]);

                int slack = 0;
                int ribbon = w * h * l;

                if (w >= h && w >= l)
                {
                    slack = h * l;
                    ribbon += h + h + l + l;
                }
                else if (h >= w && h >= l)
                {
                    slack = w * l;
                    ribbon += w + w + l + l;
                }
                else if (l >= w && l >= h)
                {
                    slack = w * h;
                    ribbon += w + w + h + h;
                }

                int paper = w * h * 2 + w * l * 2 + h * l * 2 + slack;
                totalPaper += paper;

                totalRibbon += ribbon;
            });

            Console.WriteLine("total feet of wrapping paper = " + totalPaper);

            Console.WriteLine("Running Day 2 - b");

            Console.WriteLine("total feet of ribbon = " + totalRibbon);
        }
    }
}
