using AdventOfCode2024.Entities;
using AdventOfCode2024.Enums;
using AdventOfCode2024.Extensions;

namespace AdventOfCode2024.Logic;

public class Maze
{
    private const char Impassable = '#';
    private const char Player = 'S';
    private const char Exit = 'E';

    private Vertex2 _playerPos;
    private Vertex2 _exitPos;

    private Map2D<char> _map;
    private Map2D<long> _costData;
    private readonly long _forwardCost;
    private readonly long _turningCost;

    public Maze(Map2D<char> map, long forwardCost, long turningCost)
    {
        _map = map;
        _forwardCost = forwardCost;
        _turningCost = turningCost;
        _costData = new Map2D<long>(map.SizeX, map.SizeY);
        Reset();
        _playerPos = map.FirstInstanceOf(Player) ?? throw new InvalidDataException("No player found.");
        _exitPos = map.FirstInstanceOf(Exit) ?? throw new InvalidDataException("No exit found.");
    }

    public List<List<Vertex2>> Crawl(bool pathMatters = true)
    {
        _costData.Set(_playerPos, 0);
        List<Crawler> crawlers = [new Crawler(_playerPos, Direction.East, Guid.NewGuid(), 0)];
        Dictionary<Guid, List<Vertex2>> paths = new() { { crawlers[0].Id, new List<Vertex2>() { crawlers[0].Position } } };
        SortedList<long, List<List<Vertex2>>> finalPaths = new();
        var minimumFinalCost = long.MaxValue;

        while (true)
        {
            var potentialCrawlersNextIteration = new List<Crawler>();

            foreach (var crawler in crawlers)
            {
                var id = crawler.Id;
                var pathToClone = paths[id];

                var nextCrawlers = GenerateNextCrawlers(crawler);

                // Discard crawlers which hit a wall
                if (!nextCrawlers.Any(c => c.Id == id))
                {
                    paths.Remove(id);
                }

                foreach (var nextCrawler in nextCrawlers)
                {
                    if (paths.TryGetValue(nextCrawler.Id, out var value))
                    {
                        // Optimization, avoid cloning path unless we really need to
                        if (nextCrawlers.Count > 1)
                        {
                            pathToClone = pathToClone.ShallowClone();
                        }

                        value.Add(nextCrawler.Position);
                    }
                    else
                    {
                        // Optimization, avoid cloning path unless we really need to
                        if (nextCrawlers.Count > 1)
                        {
                            var newPath = pathToClone.ShallowClone();
                            newPath.Add(nextCrawler.Position);
                            paths.Add(nextCrawler.Id, newPath);
                        }
                        else
                        {
                            pathToClone.Add(nextCrawler.Position);
                            paths.Add(nextCrawler.Id, pathToClone);
                        }
                    }

                    potentialCrawlersNextIteration.Add(nextCrawler);
                }
            }

            foreach (var crawler in potentialCrawlersNextIteration)
            {
                if (_costData.Get(crawler.Position) > crawler.Cost)
                {
                    _costData.Set(crawler.Position, crawler.Cost);
                }
            }

            crawlers.Clear();

            foreach (var crawler in potentialCrawlersNextIteration)
            {
                // Discard crawlers which cannot recover anymore, i.e. they are in the same place as another crawler, but with higher cost than it would take to turn
                if (crawler.Cost > minimumFinalCost ||
                    _costData.Get(crawler.Position) < crawler.Cost - _turningCost)
                {
                    paths.Remove(crawler.Id);
                    continue;
                }

                // Discard crawlers which cross themselves - NOT NEEDED because of first criteria
                /*if (paths[crawler.Id].Count > paths[crawler.Id].Distinct().Count())
                {
                    paths.Remove(crawler.Id);
                    continue;
                }*/

                if (crawler.Position == _exitPos)
                {
                    if (crawler.Cost <= minimumFinalCost)
                    {
                        finalPaths.AddInstance(crawler.Cost, paths[crawler.Id]);
                        minimumFinalCost = crawler.Cost;
                    }

                    paths.Remove(crawler.Id);
                }
                else
                {
                    crawlers.Add(crawler);
                }
            }

            // Discard identical crawlers if a specific path does not matter
            if (!pathMatters)
            {
                crawlers = crawlers.Distinct().ToList();
            }

            Console.WriteLine($"Crawlers: {crawlers.Count} MinimumPaths: {finalPaths.FirstOrDefault().Value?.Count ?? 0}" + (minimumFinalCost != long.MaxValue ? $" Cost: {minimumFinalCost}" : string.Empty));

            if (crawlers.Count == 0)
            {
                break;
            }
        }

        return finalPaths.FirstOrDefault().Value ?? new();
    }

    public long GetCost()
    {
        return _costData.Get(_exitPos)!.Value;
    }

    public void Reset()
    {
        _costData.Reset(long.MaxValue);
    }

    private List<Crawler> GenerateNextCrawlers(Crawler input)
    {
        var left = input.Facing.Left();
        var right = input.Facing.Right();
        var forwardPos = input.Position + Vertex2.DirectionVertex(input.Facing);
        var leftPos = input.Position + Vertex2.DirectionVertex(left);
        var rightPos = input.Position + Vertex2.DirectionVertex(right);
        var forwardValue = _map.Get(forwardPos);
        var leftValue = _map.Get(leftPos);
        var rightValue = _map.Get(rightPos);

        var crawlers = new List<Crawler>(3);

        if (forwardValue.HasValue && forwardValue != Impassable)
        {
            crawlers.Add(new Crawler(forwardPos, input.Facing, input.Id, input.Cost + _forwardCost));
        }

        if (leftValue.HasValue && leftValue != Impassable)
        {
            crawlers.Add(new Crawler(input.Position + Vertex2.DirectionVertex(left), left, Guid.NewGuid(), input.Cost + _forwardCost + _turningCost));
        }

        if (rightValue.HasValue && rightValue != Impassable)
        {
            crawlers.Add(new(input.Position + Vertex2.DirectionVertex(right), right, Guid.NewGuid(), input.Cost + _forwardCost + _turningCost));
        }

        return crawlers;
    }
}
