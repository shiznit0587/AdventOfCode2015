using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day16
    {
        public Day16()
        {
            Console.WriteLine("Running Day 16 - a");

            Sue gift = new Sue(-1);
            gift.Stats[0] = 3;
            gift.Stats[1] = 7;
            gift.Stats[2] = 2;
            gift.Stats[3] = 3;
            gift.Stats[4] = 0;
            gift.Stats[5] = 0;
            gift.Stats[6] = 5;
            gift.Stats[7] = 3;
            gift.Stats[8] = 2;
            gift.Stats[9] = 1;

            List<Sue> aunts = new List<Sue>();

            FileUtil.Parse(16, delegate(string line)
            {
                string[] parts = line.Split(new char[3]{':', ' ', ','});
                Sue sue = new Sue(Int32.Parse(parts[1]));
                sue.SetStat(parts[3], Int32.Parse(parts[5]));
                sue.SetStat(parts[7], Int32.Parse(parts[9]));
                sue.SetStat(parts[11], Int32.Parse(parts[13]));
                aunts.Add(sue);
            });

            Sue match = aunts.Find(a => a.MatchesA(gift));

            Console.WriteLine("Matching Aunt Sue - # " + match.ID);

            Console.WriteLine("Running Day 16 - b");

            match = aunts.Find(a => a.MatchesB(gift));
            
            Console.WriteLine("Matching Aunt Sue - # " + match.ID);
        }
    }

    class Sue
    {
        private static readonly string[] StatNames = new string[10] {"children", "cats", "samoyeds", "pomeranians", "akitas", "vizslas", "goldfish", "trees", "cars", "perfumes"};

        public readonly int ID = 0;
        public readonly int[] Stats = new int[10]{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        public Sue(int id)
        {
            ID = id;
        }

        public void SetStat(string name, int value)
        {
            Stats[Array.IndexOf(StatNames, name)] = value;
        }
        
        public bool MatchesA(Sue gift)
        {
            for (int i = 0; i < 10; ++i)
            {
                if (Stats[i] != -1 && Stats[i] != gift.Stats[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool MatchesB(Sue gift)
        {
            for (int i = 0; i < 10; ++i)
            {
                if (Stats[i] != -1)
                {
                    switch (i)
                    {
                        case 1:
                        case 7:
                            if (Stats[i] <= gift.Stats[i])
                            {
                                return false;
                            }
                            break;
                        case 3:
                        case 6:
                            if (Stats[i] >= gift.Stats[i])
                            {
                                return false;
                            }
                            break;
                        default:
                            if (Stats[i] != gift.Stats[i])
                            {
                                return false;
                            }
                            break;
                    }
                }
            }

            return true;
        }
    }
}