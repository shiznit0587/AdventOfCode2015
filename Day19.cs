using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day19
    {
        private List<Tuple<string, string>> _rules = new List<Tuple<string, string>>();

        private HashSet<string> _attempts = new HashSet<string>();

        private int _minSteps = int.MaxValue;
    
        public Day19()
        {
            Console.WriteLine("Running Day 19 - a");

            string source = null;

            bool endOfRules = false;
            FileUtil.Parse(19, delegate(string line)
            {
                if (line.Length == 0)
                {
                    endOfRules = true;
                }
                else if (endOfRules)
                {
                    source = line;
                    Console.WriteLine("Source=\"" + source + "\"");
                }
                else
                {
                    string ruleLHS = line.Substring(0, line.IndexOf(" => "));
                    string ruleRHS = line.Substring(line.IndexOf(" => ") + 4);
                    //Console.WriteLine("Rule: LHS=\"" + ruleLHS + "\", RHS=\"" + ruleRHS + "\"");
                    _rules.Add(new Tuple<string, string>(ruleLHS, ruleRHS));
                }
            });

            HashSet<string> results = FindMoleculesAfterReplacement(source, true);

            Console.WriteLine("Distict Molecules = " + results.Count);

            Console.WriteLine("Running Day 19 - b");

            int steps = CalculateSteps(source);

            Console.WriteLine("Minimumm Steps = " + steps);
        }

        private int CalculateSteps(string molecule)
        {
            // All rules are of one of the following forms:
            //   x => x x
            //   x => x Rn x Ar
            //   x => x Rn x Y x Ar
            //   x => x Rn x Y x Y x Ar
            // Where x is any atom not Rn, Ar, or Y.
            
            // The molecule can be broken up into sub-molecules that conform to these forms.
            // Any sequence of n atoms not Rn, Ar, or Y can be reduced to a single atom in n - 1 steps.
            // All Rn/Ar pairs reduce the number of steps by 2, since they are always reduced in conjunction with 2 other atoms (one before the Rn and one between Rn/Ar).
            // Each Y atom reduces the number of steps futher by 2, since each is always reduced in conjunction with 2 other atoms (one on either side of Y).

            // Therefore: Min Steps = TotalAtoms - Rn - Ar - 2 * Y - 1

            // For the rules above:
            //   x => x x                   n=2, Rn=0, Ar=0, Y=0, steps=(2 - 0 - 0 - 2 * 0 - 1)=1
            //   x => x Rn x Ar             n=4, Rn=1, Ar=1, Y=0, steps=(4 - 1 - 1 - 2 * 0 - 1)=1
            //   x => x Rn x Y x Ar         n=6, Rn=1, Ar=1, Y=1, steps=(6 - 1 - 1 - 2 * 1 - 1)=1
            //   x => x Rn x Y x Y x Ar     n=8, Rn=1, Ar=1, Y=2, steps=(8 - 1 - 1 - 2 * 2 - 1)=1

            // I could validate that a solution can be found with the given rules that takes the calculated number of steps.
            // But given the grammar rules, all solutions would take the same number of steps, or no solution exists for the source molecule.

            int totalAtoms = 0;
            int countRn = 0;
            int countAr = 0;
            int countY = 0;

            for (int i = 0; i < molecule.Length; ++i)
            {
                if (molecule[i] == 'R' && molecule[i+1] == 'n')
                {
                    ++countRn;
                    ++i;
                    ++totalAtoms;
                }
                else if (molecule[i] == 'A' && molecule[i+1] == 'r')
                {
                    ++countAr;
                    ++i;
                    ++totalAtoms;
                }
                else if (molecule[i] == 'Y')
                {
                    ++countY;
                    ++totalAtoms;
                }
                else if ('A' <= molecule[i] && molecule[i] <= 'Z')
                {
                    ++totalAtoms;
                }
            }

            //Console.WriteLine("TotalAtoms=" + totalAtoms + ", Rn=" + countRn + ", Ar=" + countAr + ", Y=" + countY);

            return totalAtoms - countRn - countAr - 2 * countY - 1;
        }

        private int _prints = 0;

        private void RunReplacements(string molecule, int step)
        {
            // THIS METHOD WAS A FAILED ATTEMPT. Kept around for posterity.
            _attempts.Add(molecule);
            if (_minSteps != int.MaxValue)
            {
                if (++_prints % 10000 == 0)
                    Console.WriteLine("Running replacements: molecule=" + molecule + ", step=" + step + ", minSteps=" + _minSteps);   
            }

            //Console.WriteLine("Running replacements: molecule=" + molecule);
            HashSet<string> results = FindMoleculesAfterReplacement(molecule, false);

            foreach (string result in results)
            {
                if (result == "e")
                {
                    //Console.WriteLine("Found a solution: " + step + " steps");
                    //return Math.Min(step, minSteps);
                    if (step < _minSteps)
                    {
                        Console.WriteLine("Found a new step minimum: steps=" + step);
                        _minSteps = step;
                    }
                    return;
                }
            }

            foreach (string result in results)
            {
                if (step + 1 < _minSteps && !_attempts.Contains(result) && !result.Contains("e")) // Optimization: Don't recurse on a molecule with an 'e'.
                {
                    RunReplacements(result, step + 1);

                    if (_minSteps <= step + 1)
                    {
                        return;
                    }
                }
            }
        }

        private HashSet<string> FindMoleculesAfterReplacement(string source, bool ltr)
        {
            HashSet<string> results = new HashSet<string>();

            foreach (Tuple<string,string> rule in _rules)
            {
                string lhs = ltr ? rule.Item1 : rule.Item2;
                string rhs = ltr ? rule.Item2 : rule.Item1;
                
                int cursor = 0;

                while (true)
                {
                    int idx = source.IndexOf(lhs, cursor);

                    if (idx == -1)
                    {
                        break;
                    }

                    string sourceLHS = source.Substring(0, idx);
                    string sourceRHS = source.Substring(idx + lhs.Length);

                    string result = sourceLHS + rhs + sourceRHS;

                    //Console.WriteLine("replacementRule=" + rule.Item1 + " => " + rule.Item2 + ", idx=" + idx + ", sourceLHS=" + sourceLHS + ", sourceRHS=" + sourceRHS + ", result=" + result);

                    results.Add(result);

                    cursor = idx + 1;
                }
            }

            return results;
        }
    }
}
