using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day23
    {
        int[] r = new int[2];

        public Day23()
        {
            List<string> program = new List<string>();
            FileUtil.Parse(23, s => program.Add(s));

            Console.WriteLine("Running Day 23 - a");

            RunProgram(program);

            Console.WriteLine("Final Register Values=[" + String.Join(",", r) + "]");

            Console.WriteLine("Running Day 23 - b");

            r[0] = 1;
            r[1] = 0;

            RunProgram(program);

            Console.WriteLine("Final Register Values=[" + String.Join(",", r) + "]");
        }

        private void RunProgram(List<string> program)
        {
            int line = 0;

            while (0 <= line && line < program.Count)
            {
                string instr = program[line];
                string cmd = instr.Substring(0, 3);
                int offset = 1;

                switch (cmd)
                {
                    case "hlf":
                        r[ri(instr)] /= 2;
                        break;
                    case "tpl":
                        r[ri(instr)] *= 3;
                        break;
                    case "inc":
                        ++r[ri(instr)];
                        break;
                    case "jmp":
                        offset = go(instr, 4);
                        break;
                    case "jie":
                        if (r[ri(instr)] % 2 == 0)
                            offset = go(instr, 7);
                        break;
                    case "jio":
                        if (r[ri(instr)] == 1)
                            offset = go(instr, 7);
                        break;
                }

                line += offset;
            }
        }

        private int ri(string instruction)
        {
            return instruction.ToCharArray()[4] - 97;
        }

        private int go(string instr, int idx)
        {
            int offset = int.Parse(instr.Substring(idx + 1));
            if (instr.ToCharArray()[idx] == '-')
                offset *= -1;
            return offset;
        }
    }
}
