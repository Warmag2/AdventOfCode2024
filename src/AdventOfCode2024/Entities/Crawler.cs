using AdventOfCode2024.Enums;

namespace AdventOfCode2024.Entities;

public struct Crawler
{
    public Crawler(Vertex2 pos, Direction facing, Guid id, long cost = 0)
    {
        Cost = cost;
        Facing = facing;
        Id = id;
        Position = pos;
    }

    public long Cost { get; set; }

    public Direction Facing { get; set; }

    public Guid Id { get; }

    public Vertex2 Position { get; set; }

    public static bool operator !=(Crawler a, Crawler b)
    {
        return a.Cost != b.Cost || a.Facing != b.Facing || a.Position != b.Position;
    }

    public static bool operator ==(Crawler a, Crawler b)
    {
        return a.Cost == b.Cost && a.Facing == b.Facing && a.Position == b.Position;
    }

    /// <inheritdoc />
    public override readonly bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is Crawler crawler)
        {
            return Equals(crawler);
        }

        return false;
    }

    public readonly bool Equals(Crawler other)
    {
        return this == other;
    }
}
