namespace AdventOfCode2024.Entities;

public class NTree
{
    public string Id { get; set; } = string.Empty;

    public Dictionary<string, NTree> Branches { get; set; } = new();

    public long? Cost { get; set; } = null;

    public NTree? Walk(List<string> ids, int pos)
    {
        if (pos == ids.Count)
        {
            return this;
        }

        var next = ids[pos];

        if (Branches.TryGetValue(next, out var nextTree))
        {
            if (nextTree.Cost == 0)
            {
                return null;
            }

            return nextTree.Walk(ids, pos + 1);
        }
        else
        {
            // By default subtrees are not pruned
            var branch = TryAdd(next, 1);
            return branch.Walk(ids, pos + 1);
        }
    }

    public bool Prune(List<string> ids)
    {
        var tree = Walk(ids, 0);

        if (tree != null)
        {
            tree.Cost = 0;

            return true;
        }

        return false;
    }

    public long GetSubTreeCost()
    {
        if (Branches.Count == 0)
        {
            return Cost!.Value;
        }

        long subCost = 0;

        foreach (var branch in Branches.Select(b => b.Value))
        {
            subCost += branch.GetSubTreeCost();
        }

        if (Cost.HasValue)
        {
            return Cost!.Value * subCost;
        }
        else
        {
            return subCost;
        }
    }

    public NTree TryAddForce(string id, long cost)
    {
        if (!Branches.TryAdd(id, new NTree() { Id = id, Cost = cost }))
        {
            Branches[id].Cost = cost;
        }

        return Branches[id];
    }

    public NTree TryAdd(string id, long cost)
    {
        Branches.TryAdd(id, new NTree() { Id = id, Cost = cost });

        return Branches[id];
    }
}
