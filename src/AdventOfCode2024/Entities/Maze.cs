﻿using AdventOfCode2024.Enums;
using AdventOfCode2024.Extensions;

namespace AdventOfCode2024.Entities;

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
        _costData.Reset(long.MaxValue);
        _playerPos = map.FirstInstanceOf(Player) ?? throw new InvalidDataException("No player found.");
        _exitPos = map.FirstInstanceOf(Exit) ?? throw new InvalidDataException("No exit found.");
    }

    public List<List<Vertex2>> Crawl()
    {
        _costData.Set(_playerPos, 0);
        List<Crawler> crawlers = [new Crawler(_playerPos, Direction.East, Guid.NewGuid(), 0)];
        Dictionary<Guid, List<Vertex2>> paths = new() { { crawlers[0].Id, new List<Vertex2>() { crawlers[0].Position } } };
        List<(long Cost, List<Vertex2> finalPath)> finalPaths = new();

        while (true)
        {
            var potentialCrawlersNextIteration = new List<Crawler>();

            foreach (var crawler in crawlers)
            {
                var id = crawler.Id;
                var currentPath = paths[id].ShallowClone();

                var nextCrawlers = GenerateNextCrawlers(crawler).Where(c => _map.Get(c.Position) != Impassable).ToList();

                // Discard crawlers which end up in a wall
                if (!nextCrawlers.Any(c => c.Id == id))
                {
                    paths.Remove(id);
                }

                foreach (var nextCrawler in nextCrawlers)
                {
                    if (paths.TryGetValue(nextCrawler.Id, out var value))
                    {
                        value.Add(nextCrawler.Position);
                    }
                    else
                    {
                        var newPath = currentPath.ShallowClone();
                        newPath.Add(nextCrawler.Position);
                        paths.Add(nextCrawler.Id, newPath);
                    }

                    potentialCrawlersNextIteration.Add(nextCrawler);
                }
            }

            //crawlers.Select(GenerateNextCrawlers).SelectMany(n => n).Where(c => _map.Get(c.Position) != Impassable).ToList();

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
                if (_costData.Get(crawler.Position) < crawler.Cost - _turningCost)
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
                    finalPaths.Add((crawler.Cost, paths[crawler.Id]));
                    paths.Remove(crawler.Id);
                }
                else
                {
                    crawlers.Add(crawler);
                }
            }

            Console.WriteLine($"Crawlers: {crawlers.Count} FinalPaths: {finalPaths.Count}");

            if (crawlers.Count == 0)
            {
                break;
            }
        }

        return finalPaths.Where(f => f.Cost == finalPaths.Min(p => p.Cost)).Select(a => a.finalPath).ToList();
    }

    public long GetCost()
    {
        return _costData.Get(_exitPos)!.Value;
    }

    private List<Crawler> GenerateNextCrawlers(Crawler input)
    {
        var left = input.Facing.Left();
        var right = input.Facing.Right();

        return new List<Crawler>
        {
            new Crawler(input.Position + Vertex2.DirectionVertex(input.Facing), input.Facing, input.Id, input.Cost + _forwardCost),
            new Crawler(input.Position + Vertex2.DirectionVertex(left), left, Guid.NewGuid(), input.Cost + _forwardCost + _turningCost),
            new Crawler(input.Position + Vertex2.DirectionVertex(right), right, Guid.NewGuid(), input.Cost + _forwardCost + _turningCost)
        };
    }
}
