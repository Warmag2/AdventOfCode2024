using AdventOfCode2024.Attributes;
using AdventOfCode2024.Extensions;

namespace AdventOfCode2024.Problems;

[ProblemIndex(Index = 9)]
public class Problem09 : ProblemBase
{
    private readonly Random random = new Random();

    public Problem09(string input) : base(input)
    {
    }

    public override string Solve()
    {
        var diskImage = new List<int>();

        var actualInput = string.Empty;

        for (int ii = 0; ii < 15; ii++)
        {
            actualInput += (random.Next(9) + 1).ToString(); // File
            actualInput += random.Next(10).ToString(); // Empty
        }

        var operationType = true; // File is true, empty space is false
        var fileIndex = 0;

        foreach (var character in Input.Trim().ToArray())
        {
            if (operationType)
            {
                AddFile(diskImage, int.Parse(character.ToString()), fileIndex);
                fileIndex++;
                operationType = false;
            }
            else
            {
                AddFile(diskImage, int.Parse(character.ToString()), -1); // File index -1 here is empty space
                operationType = true;
            }
        }

        // Defragment
        var diskImage2 = diskImage.ShallowClone();
        Defragment(diskImage);
        DefragmentBonus(diskImage2);

        //Console.WriteLine(DiskImageToString(diskImage));

        return $"Solution: {GetCheckSum(diskImage)} Bonus: {GetCheckSum(diskImage2)}";
    }

    private static long GetCheckSum(List<int> diskImage)
    {
        long checkSum = 0;

        for (var ii = 0; ii < diskImage.Count; ii++)
        {
            if (diskImage[ii] == -1)
            {
                continue;
            }

            checkSum += diskImage[ii] * ii;
        }

        return checkSum;
    }

    private static void Defragment(List<int> diskImage)
    {
        var lastPosFromStart = 0;
        var lastPosFromEnd = diskImage.Count - 1;

        while (true)
        {
            //Console.WriteLine(DiskImageToString(diskImage));
            var freeSpaceIndex = diskImage.IndexOf(-1, lastPosFromStart);
            var filledSpaceIndex = GetLastFilledSpot(diskImage, lastPosFromEnd);

            if (freeSpaceIndex >= filledSpaceIndex)
            {
                break;
            }
            else
            {
                diskImage[freeSpaceIndex] = diskImage[filledSpaceIndex];
                diskImage[filledSpaceIndex] = -1;
            }
        }
    }

    private static void DefragmentBonus(List<int> diskImage)
    {
        var fileIndexToMove = diskImage.Last(i => i != -1);

        while (fileIndexToMove > 0)
        {
            //Console.WriteLine(DiskImageToString(diskImage));
            var lastPosFromEnd = diskImage.LastIndexOf(fileIndexToMove);
            var fileLength = FileLength(diskImage, fileIndexToMove, lastPosFromEnd);

            var spotToMoveTo = FindFreeSpaceBefore(diskImage, fileLength, lastPosFromEnd - fileLength);

            if (spotToMoveTo != -1)
            {
                for (var ii = 0; ii < fileLength; ii++)
                {
                    diskImage[spotToMoveTo + ii] = fileIndexToMove;
                    diskImage[lastPosFromEnd - ii] = -1;
                }
            }

            fileIndexToMove--;
        }
    }

    private static int GetLastFilledSpot(List<int> diskImage, int lastPosFromEnd)
    {
        for (var ii = lastPosFromEnd; ii >= 0; ii--)
        {
            if (diskImage[ii] != -1)
            {
                return ii;
            }
        }

        return -1;
    }

    private static void AddFile(List<int> input, int amount, int fileIndex)
    {
        for (int ii = 0; ii < amount; ii++)
        {
            input.Add(fileIndex);
        }
    }

    private static string DiskImageToString(List<int> diskImage)
    {
        var result = string.Empty;

        foreach (var spot in diskImage)
        {
            result += spot == -1 ? "." : ToStringHex(spot);
        }

        return result;
    }

    private static string ToStringHex(int spot)
    {
        if (spot >= 0 && spot < 10)
        {
            return spot.ToString();
        }

        switch (spot)
        {
            case 10: return "A";
            case 11: return "B";
            case 12: return "C";
            case 13: return "D";
            case 14: return "E";
            case 15: return "F";
        }

        throw new NotImplementedException();
    }

    private static int FileLength(List<int> diskImage, int fileIndex, int lastPosFromEnd)
    {
        int? endSpot = null;
        int? startSpot = null;

        for (var ii = lastPosFromEnd; ii >= 0; ii--)
        {
            if (!endSpot.HasValue && diskImage[ii] == fileIndex)
            {
                endSpot = ii;
            }

            if (endSpot.HasValue && !startSpot.HasValue && diskImage[ii] != fileIndex)
            {
                startSpot = ii;
                return endSpot.Value - startSpot.Value;
            }
        }

        throw new InvalidOperationException("File length not determined!");
    }

    private static int FindFreeSpaceBefore(List<int> diskImage, int count, int lastValidPos)
    {
        var contingousFree = 0;

        for (var ii = 0; ii <= lastValidPos; ii++)
        {
            if (diskImage[ii] == -1)
            {
                contingousFree++;

                if (contingousFree == count)
                {
                    return ii - (count - 1);
                }
            }
            else
            {
                contingousFree = 0;
            }
        }

        return -1;
    }
}
