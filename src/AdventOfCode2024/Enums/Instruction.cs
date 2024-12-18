namespace AdventOfCode2024.Enums;

public enum Instruction
{
    Adv = 0, // Division Ax/(2^Combo) -> Aa
    Bxl = 1, // XOR(Bx,literal) -> Bx
    Bst = 2, // Combo % 8 -> Bx
    Jnz = 3, // If A != 0, JML to literal
    Bxc = 4, // XOR(Bx, Cx) -> Bx
    Out = 5, // Combo % 8 -> print
    Bdv = 6, // Division Ax/2^Combo -> Bx
    Cdv = 7, // Division Ax/2^Combo -> Cx
}
