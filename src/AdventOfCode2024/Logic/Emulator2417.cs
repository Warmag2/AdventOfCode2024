using AdventOfCode2024.Enums;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Logic;

public class Emulator2417
{
    public readonly int[] Program;

    public Emulator2417(string input)
    {
        var programPrototype = new List<int>();
        var allInput = input.ToStringLists();

        foreach (var initVector in allInput[0])
        {
            var initVectorParts = initVector.Split(":");

            switch (initVectorParts[0])
            {
                case "Register A":
                    Ax = int.Parse(initVectorParts[1]);
                    break;
                case "Register B":
                    Bx = int.Parse(initVectorParts[1]);
                    break;
                case "Register C":
                    Cx = int.Parse(initVectorParts[1]);
                    break;
            }
        }

        var allInstructions = allInput[1][0].Split(" ")[1].Split(",");

        for (var ii = 0; ii < allInstructions.Length; ii++)
        {
            programPrototype.Add(int.Parse(allInstructions[ii]));
        }

        Program = programPrototype.ToArray();
    }

    public long Ax { get; set; }

    public long Bx { get; set; }

    public long Cx { get; set; }

    public List<int> Run() //bool compareOutput, ReadOnlySpan<int> outputComparison)
    {
        List<int> output = new();
        var instructionPointer = 0;

        //int outputCompareIndex = 0;

        while (true)
        {
            if (instructionPointer >= Program.Length - 1) // Cannot read operand if we are at last place
            {
                break;
            }

            var instruction = (Instruction)Program[instructionPointer];
            var operand = Program[instructionPointer + 1];

            int divisor;

            switch (instruction)
            {
                case Instruction.Adv:
                    Ax = Ax >> (int)Combo(operand);
                    instructionPointer += 2;
                    break;
                case Instruction.Bdv:
                    Bx = Ax >> (int)Combo(operand);
                    instructionPointer += 2;
                    break;
                case Instruction.Cdv:
                    Cx = Ax >> (int)Combo(operand);
                    instructionPointer += 2;
                    break;
                /*                case Instruction.Adv:
                                    divisor = Pow2Combo(operand);
                                    Ax = Ax / divisor;
                                    instructionPointer += 2;
                                    break;
                                case Instruction.Bdv:
                                    divisor = Pow2Combo(operand);
                                    Bx = Ax / divisor;
                                    instructionPointer += 2;
                                    break;
                                case Instruction.Cdv:
                                    divisor = Pow2Combo(operand);
                                    Cx = Ax / divisor;
                                    instructionPointer += 2;
                                    break;*/
                case Instruction.Bxl:
                    Bx = Bx ^ operand;
                    instructionPointer += 2;
                    break;
                case Instruction.Bst:
                    Bx = Combo(operand) % 8;
                    instructionPointer += 2;
                    break;
                case Instruction.Jnz:
                    if (Ax != 0)
                    {
                        instructionPointer = operand;
                    }
                    else
                    {
                        instructionPointer += 2;
                    }

                    break;
                case Instruction.Bxc:
                    Bx = Bx ^ Cx;
                    instructionPointer += 2;
                    break;
                case Instruction.Out:
                    output.Add((int)(Combo(operand) % 8));
                    instructionPointer += 2;

                    /*if (compareOutput)
                    {
                        if (output[outputCompareIndex] != outputComparison![outputCompareIndex])
                        {
                            return null;
                        }

                        outputCompareIndex++;

                        if (outputCompareIndex > outputComparison.Length)
                        {
                            return null;
                        }
                    }*/

                    break;
            }
        }

        return output;
    }

    private int Pow2Combo(int operand)
    {
        return (int)Pow(2, (int)Combo(operand));
    }

    private long Combo(int operand)
    {
        switch (operand)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return operand;
            case 4:
                return Ax;
            case 5:
                return Bx;
            case 6:
                return Cx;
            default:
                throw new NotImplementedException("Invalid oprand");
        }
    }

    private static long Pow(long x, int power)
    {
        if (power == 0)
        {
            return 1;
        }

        if (power == 1)
        {
            return x;
        }

        var tmp = 2 << (power - 1);

        return tmp;
    }
}
