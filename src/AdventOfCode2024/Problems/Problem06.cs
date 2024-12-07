using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Enums;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 6)]
public class Problem06 : ProblemBase
{
    private static readonly Dictionary<char, Direction> GuardInitDirection = new Dictionary<char, Direction>() { { '^', Direction.North }, { '>', Direction.East }, { 'v', Direction.South }, { '<', Direction.West } };

    public Problem06(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var (map, _) = WalkGuard(Input);

        var result = map.GetInstancesOf('X');

        int possibleLoops = 0;

        foreach (var position in result)
        {
            var (mapBonus, loopFound) = WalkGuard(Input, position);

            if (loopFound)
            {
                possibleLoops++;
                //Console.WriteLine(mapBonus.ToString());
            }
        }

        return $"Solution: {result.Count}\nBonus solution: {possibleLoops}";
    }

    private static (Map2D<char> map, bool loopExists) WalkGuard(string input, Vertex2? addObstacle = null)
    {
        var map = new Map2D<char>(input.ToArray2D(['\r', '\n']));

        var guardPos = map.FirstInstanceOf('^')!.Value;
        map.Set(guardPos, 'X');
        var guardDir = GuardInitDirection['^'];

        if (addObstacle != null && addObstacle != guardPos)
        {
            map.Set(addObstacle.Value, 'O');
        }

        var guardPositions = new HashSet<long>();
        //var guardPositions = new List<(Vertex2 pos, Direction dir)>();

        var loopDetected = false;

        while (true)
        {
            // Loop detection
            var hash = CalculateHash(guardPos, guardDir);
            if (guardPositions.Contains(hash))
            {
                loopDetected = true;
            }

            /*foreach (var positionItem in guardPositions)
            {
                if (positionItem.pos == guardPos && positionItem.dir == guardDir)
                {
                    loopDetected = true;
                    break;
                }
            }*/

            if (loopDetected)
            {
                break;
            }
            else
            {
                guardPositions.Add(hash);
            }

            var guardNextPos = guardPos.Travel(guardDir);
            var nextSquare = map.Get(guardNextPos);

            if (nextSquare == null)
            {
                break;
            }

            if (nextSquare == '.' || nextSquare == 'X')
            {
                guardPos = guardNextPos;
                map.Set(guardPos, 'X');
            }
            else
            {
                guardDir = guardDir.TurnRight();
            }

            //Console.ReadKey();
        }

        return (map, loopDetected);
    }

    private static long CalculateHash(Vertex2 pos, Direction dir)
    {
        return 1000000 * (int)dir + 1000 * pos.X + pos.Y;
    }
}
