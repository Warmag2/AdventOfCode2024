using System.Text.RegularExpressions;
using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 14)]
public class Problem14 : ProblemBase
{
    public Problem14(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var map = new Map2D<char>(101, 103);

        var (robotPositions, robotVelocities) = ParseRobots(Input);

        for (var ii = 0; ii < 100; ii++)
        {
            robotPositions = ProcessRobotMovement(map, robotPositions, robotVelocities, 1);
        }

        var solution = CalculateQuadrants(map, robotPositions);

        (robotPositions, robotVelocities) = ParseRobots(Input);

        var newMap = new Map2D<char>(101, 103);
        var iterations = 0;
        var treeFound = false;

        while (!treeFound)
        {
            robotPositions = ProcessRobotMovement(newMap, robotPositions, robotVelocities, 1);
            iterations++;

            foreach (var line in newMap.Lines())
            {
                if (IsSuscpiciousLine(line))
                {
                    treeFound = true;
                    Console.WriteLine(newMap.ToString());
                    Console.WriteLine($"Iteration: {iterations} Treefound: {treeFound}");
                    break;
                }
            }

            Console.WriteLine($"Iteration: {iterations}");
        }

        return $"Solution: {solution} Bonus: {iterations} ";
    }

    private bool IsSuscpiciousLine(char[] line)
    {
        var suspicion = 0;

        for (var ii = 0; ii < line.Length; ii++)
        {
            if (line[ii] != '.')
            {
                suspicion++;

                if(suspicion > 7)
                {
                    return true;
                }
            }
            else
            {
                suspicion = 0;
            }
        }

        return false;
    }

    private int CalculateQuadrants(Map2D<char> map, List<Vertex2> robotFinalLocations)
    {
        var q1 = 0;
        var q2 = 0;
        var q3 = 0;
        var q4 = 0;

        foreach (var mapLocation in map.AllPositions())
        {
            if (mapLocation.X < map.SizeX / 2 && mapLocation.Y < map.SizeY/2)
            {
                q1 += robotFinalLocations.Count(r => r == mapLocation);
            }

            if (mapLocation.X > map.SizeX / 2 && mapLocation.Y < map.SizeY / 2)
            {
                q2 += robotFinalLocations.Count(r => r == mapLocation);
            }

            if (mapLocation.X < map.SizeX / 2 && mapLocation.Y > map.SizeY / 2)
            {
                q3 += robotFinalLocations.Count(r => r == mapLocation);
            }

            if (mapLocation.X > map.SizeX / 2 && mapLocation.Y > map.SizeY / 2)
            {
                q4 += robotFinalLocations.Count(r => r == mapLocation);
            }
        }

        return q1 * q2 * q3 * q4;
    }

    private List<Vertex2> ProcessRobotMovement(Map2D<char> map, List<Vertex2> robotPositions, List<Vertex2> robotVelocities, int iterations)
    {
        var robotFinalPositions = new List<Vertex2>();

        for (var ii = 0; ii < robotPositions.Count; ii++)
        {
            var posX = (robotPositions[ii].X + robotVelocities[ii].X * iterations) % map.SizeX;
            var posY = (robotPositions[ii].Y + robotVelocities[ii].Y * iterations) % map.SizeY;

            if (posX < 0)
            {
                posX = map.SizeX + posX;
            }

            if (posY < 0)
            {
                posY = map.SizeY + posY;
            }

            robotFinalPositions.Add(new Vertex2(posX, posY));
        }

        // Reset map
        map.Reset('.');

        // Draw robots on map
        foreach (var robotPosition in robotFinalPositions)
        {
            var value = map.Get(robotPosition) ?? '.';

            if (map.Get(robotPosition) == '.')
            {
                map.Set(robotPosition, '1');
            }
            else
            {
                map.Set(robotPosition, (char)(value + 1));
            }
        }

        return robotFinalPositions;
    }

    private (List<Vertex2> robotPositions, List<Vertex2> robotVelocities) ParseRobots(string input)
    {
        var pos = new List<Vertex2>();
        var vel = new List<Vertex2>();

        var splitLines = input.Split("\r\n");

        var regex = new Regex("[-]*[0-9]+");

        foreach (var line in splitLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var variables = regex.Matches(line);
            pos.Add(new Vertex2(int.Parse(variables[0].ToString()), int.Parse(variables[1].ToString())));
            vel.Add(new Vertex2(int.Parse(variables[2].ToString()), int.Parse(variables[3].ToString())));
        }

        return (pos, vel);
    }

    private int GetEntropy(Map2D<char> map)
    {
        var totalEntropy = 0;

        foreach (var location in map.AllPositions())
        {
            totalEntropy += map.GetNeighborsNotOfType(
                location,
                [Direction.North, Direction.NorthEast, Direction.East, Direction.SouthEast, Direction.South, Direction.SouthWest, Direction.West, Direction.NorthWest],
                '.')
                .Count;
        }

        return totalEntropy;
    }
}
