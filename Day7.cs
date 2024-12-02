using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class Day7
    {
        Dictionary<String, int> wireValues;
        List<Instruction> instructions;

        public Day7()
        {
            Console.WriteLine("Running Day 7 - a");

            wireValues = new Dictionary<string, int>();

            instructions = new List<Instruction>();

            FileUtil.Parse(7, delegate(string rawInstruction)
            {
                instructions.Add(new Instruction(rawInstruction));
            });

            int idx = 0;

            // TODO: Actually construct a list of which instructions to execute by walking backwards from the instruction that outputs a.

            while (!wireValues.ContainsKey("a"))
            {
                if (instructions[idx].readyToRun(wireValues))
                    instructions[idx].run(wireValues);

                if (++idx == instructions.Count)
                    idx = 0;
            }

            Console.WriteLine("Value on Wire A = " + wireValues["a"]);

            Console.WriteLine("Running Day 7 - b");

            int wireA = wireValues["a"];

            foreach (Instruction instruction in instructions)
            {
                instruction.reset();
            }

            wireValues = new Dictionary<string, int>();
            wireValues["b"] = wireA;
            idx = 0;

            while (!wireValues.ContainsKey("a"))
            {
                if (instructions[idx].OutputWire != "b") // The fact I have to even check this means I'm doing this wrong.
                    if (instructions[idx].readyToRun(wireValues))
                        instructions[idx].run(wireValues);

                if (++idx == instructions.Count)
                    idx = 0;
            }

            Console.WriteLine("Value on Wire A = " + wireValues["a"]);
        }

        class Instruction
        {
            string instruction;
            InstructionEndpoint[] inputs = new InstructionEndpoint[2];
            InstructionEndpoint output;
            bool hasRun = false;

            public string OutputWire { get { return output.Wire; } }

            public Instruction(string input)
            {
                string[] parts = input.Split(' ');

                bool nextPartIsOutput = false;
                int inputIdx = 0;

                for (int i = 0; i < parts.Length; ++i)
                {
                    string part = parts[i];

                    switch (part)
                    {
                        case "AND":
                        case "OR":
                        case "RSHIFT":
                        case "LSHIFT":
                        case "NOT":
                            instruction = part;
                            break;
                        case "->":
                            nextPartIsOutput = true;
                            break;
                        default:
                            if (nextPartIsOutput)
                                output = new InstructionEndpoint(part);
                            else
                                inputs[inputIdx++] = new InstructionEndpoint(part);
                            break;
                    }
                }

                switch (instruction)
                {
                    case "AND":
                    case "OR":
                    case "RSHIFT":
                    case "LSHIFT":
                        checkEndpoint(inputs[0]);
                        checkEndpoint(inputs[1]);
                        break;
                    case "NOT":
                    default:
                        checkEndpoint(inputs[0]);
                        break;
                }

                checkEndpoint(output);
            }

            public bool readyToRun(Dictionary<String, int> wires)
            {
                if (hasRun)
                    return false;
                    
                foreach (InstructionEndpoint input in inputs)
                {
                    int value;
                    if (input != null && !input.getValue(wires, out value))
                    {
                        return false;
                    }
                }

                return true;
            }

            public void reset()
            {
                hasRun = false;
            }

            public void run(Dictionary<String, int> wires)
            {
                int input1 = Int32.MinValue;
                int input2 = Int32.MinValue;

                bool success = (inputs[0] != null) ? inputs[0].getValue(wires, out input1) : true;
                if (!success)
                {
                    throw new ArgumentException();
                }

                success = (inputs[1] != null) ? inputs[1].getValue(wires, out input2) : true;
                if (!success)
                {
                    throw new ArgumentException();
                }

                switch (instruction)
                {
                    case "AND":
                        checkVal(input1);
                        checkVal(input2);
                        wires[output.Wire] = input1 & input2;
                        break;
                    case "OR":
                        checkVal(input1);
                        checkVal(input2);
                        wires[output.Wire] = input1 | input2;
                        break;
                    case "RSHIFT":
                        checkVal(input1);
                        checkVal(input2);
                        wires[output.Wire] = input1 >> input2;
                        break;
                    case "LSHIFT":
                        checkVal(input1);
                        checkVal(input2);
                        wires[output.Wire] = input1 << input2;
                        break;
                    case "NOT":
                        checkVal(input1);
                        wires[output.Wire] = ~input1;
                        break;
                    default:
                        checkVal(input1);
                        wires[output.Wire] = input1;
                        break;
                }

                hasRun = true;
            }

            private void checkEndpoint(InstructionEndpoint val)
            {
                if (val == null || !val.isValid())
                {
                    throw new ArgumentException();
                }
            }

            private void checkVal(int val)
            {
                if (val == Int32.MinValue)
                {
                    throw new ArgumentException();
                }
            }
        }

        class InstructionEndpoint
        {
            int constant = Int32.MinValue;
            string wire;

            public InstructionEndpoint(string input)
            {
                int result;
                bool success = Int32.TryParse(input, out result);
                if (success)
                    constant = result;
                else 
                    wire = input;
            }

            public bool isValid()
            {
                return (constant != Int32.MinValue && wire == null) || (constant == Int32.MinValue && wire != null);
            }

            public bool getValue(Dictionary<string, int> wires, out int value)
            {
                if (wire != null)
                {
                    if (wires.ContainsKey(wire))
                    {
                        value = wires[wire];
                        return true;
                    }
                    value = Int32.MinValue;
                    return false;
                }
                else if (constant != Int32.MinValue)
                {
                    value = constant;
                    return true;
                }
                value = Int32.MinValue;
                return false;
            }

            public string Wire 
            {
                get 
                {
                    if (wire == null)
                        throw new ArgumentException();
                    return wire;
                }
            }
        }
    }
}
