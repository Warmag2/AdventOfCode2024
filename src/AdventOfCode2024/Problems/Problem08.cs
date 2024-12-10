using AdventOfCode2024.Attributes;
using AdventOfCode2024.Entities;
using AdventOfCode2024.Helpers;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 8)]
public class Problem08 : ProblemBase
{
    public Problem08(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var map = new Map2D<char>(Input.ToArray2D("\r\n".ToArray()));
        var mapForResult = new Map2D<char>(map.SizeX, map.SizeY, '.');

        var antennas = map.GetNonEmptySquares('.');
        foreach (var (_, antennaLocations) in antennas)
        {
            for (var ii = 0; ii < antennaLocations.Count; ii++)
            {
                for (var jj = ii + 1; jj < antennaLocations.Count; jj++)
                {
                    var (dir, dist) = map.IsAtDirectionFrom(antennaLocations[ii], antennaLocations[jj]);

                    if (dir.HasValue)
                    {
                        // Nothing possible if distance is 0
                        if (dist < 1)
                        {
                            continue;
                        }

                        // Only possibility for in-between antinodes
                        if (dist % 3 == 0)
                        {
                            mapForResult.Set(antennaLocations[ii] + Vertex2.DirectionVertex(dir.Value) * (dist / 3), 'X');
                            mapForResult.Set(antennaLocations[ii] + Vertex2.DirectionVertex(dir.Value) * (2 * dist / 3), 'X');
                        }

                        // Outer antinodes always possible
                        mapForResult.Set(antennaLocations[ii] + Vertex2.DirectionVertex(dir.Value) * (2 * dist), 'X');
                        mapForResult.Set(antennaLocations[ii] + Vertex2.AntiDirectionVertex(dir.Value) * dist, 'X');
                    }
                    else // Not in a cardinal direction, but still produces outside antinodes
                    {
                        var trueDist = antennaLocations[jj] - antennaLocations[ii];
                        mapForResult.Set(antennaLocations[ii] - trueDist, 'X');
                        mapForResult.Set(antennaLocations[jj] + trueDist, 'X');
                    }
                }
            }
        }

        Console.WriteLine(map.ToString());
        Console.WriteLine(mapForResult.ToString());

        var totalXs = mapForResult.GetInstancesOf('X').Count;

        // Bonus
        var mapForSecondResult = new Map2D<char>(map.SizeX, map.SizeY, '.');

        foreach (var (_, antennaLocations) in antennas)
        {
            for (var ii = 0; ii < antennaLocations.Count; ii++)
            {
                for (var jj = ii + 1; jj < antennaLocations.Count; jj++)
                {
                    var trueDist = antennaLocations[jj] - antennaLocations[ii];

                    var normalizedDir = trueDist.NormalizeToDirection();

                    if (normalizedDir.HasValue)
                    {
                        // Draw until off-map in any direction
                        mapForSecondResult.SetLine(antennaLocations[ii], normalizedDir.Value, 'X');
                        mapForSecondResult.SetLine(antennaLocations[ii], normalizedDir.Value * (-1), 'X');
                    }
                    else
                    {
                        // Solve smallest grid-hitting step for difference
                        var larger = Math.Max(Math.Abs(trueDist.X), Math.Abs(trueDist.Y));
                        var smaller = Math.Min(Math.Abs(trueDist.X), Math.Abs(trueDist.Y));

                        Vertex2 step = trueDist;

                        if (larger % smaller == 0)
                        {
                            step = trueDist / smaller;
                        }

                        mapForSecondResult.SetLine(antennaLocations[ii], step, 'X');
                        mapForSecondResult.SetLine(antennaLocations[jj], step * (-1), 'X');
                    }
                }
            }
        }

        Console.WriteLine(mapForSecondResult.ToString());

        var totalXsBonus = mapForSecondResult.GetInstancesOf('X').Count;

        return $"Solution: {totalXs}\nBonus solution: {totalXsBonus}";
    }
}
