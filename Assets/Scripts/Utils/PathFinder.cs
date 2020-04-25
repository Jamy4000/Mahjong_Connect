using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public static class PathFinder
{
    private static List<Path<Tile>> _availablePaths = new List<Path<Tile>>();
    private const int WEIGHT_FACTOR = 1000;

    public static Path<Tile> ValidPathExist(Tile thisTile, Tile currentlyClickedTile)
    {
        int index = _availablePaths.FindIndex(p => IsPath(p.Last(), p.LastStep) || IsPath(p.LastStep, p.Last()));
        return index >= 0 ? _availablePaths[index] : null;

        bool IsPath(Tile start, Tile end) 
        {
            return start == currentlyClickedTile && end == thisTile;
        }
    }

    /// <summary>
    /// Using A* search algorithmn, and Manhattan heutistic for distance estimation
    /// </summary>
    public static void CalculateAllAvailablePaths()
    {
        var gameManager = GameManager.Instance;
        var tileMatrix = gameManager.TilesMatrix;

        List<SimplifiedPath> createdPaths = new List<SimplifiedPath>(); 

        // for all tiles in the dictionary
        foreach (var lists in gameManager.SameTilesDictionary)
        {
            UnityEngine.Debug.Log("");
            UnityEngine.Debug.Log("<Color=red>XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX</Color>");
            UnityEngine.Debug.Log("<Color=red>Checking Tile with id </Color>" + lists.Key);
            UnityEngine.Debug.Log("<Color=red>XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX</Color>");
            UnityEngine.Debug.Log("");

            // check all possible start
            for (int i = 0; i < lists.Value.Count; i++)
            {
                // check all possible destinations
                for (int j = 0; j < lists.Value.Count; j++)
                {
                    UnityEngine.Debug.Log("Checking " + lists.Value[j].Coordinates + " with " + lists.Value[i].Coordinates);

                    // Check if we're not checking the same tile and if the path wasn't already created, but the other way around
                    int index = createdPaths.FindIndex(p => p.From == lists.Value[j] && p.To == lists.Value[i]);
                    if (i == j || index >= 0)
                        continue;

                    Path<Tile> path = FindAStarPath(lists.Value[i], lists.Value[j]);
                    
                    if (path != null)
                    {
                        createdPaths.Add(new SimplifiedPath(lists.Value[i], lists.Value[j]));
                        _availablePaths.Add(path);
                    }
                }
            }

            UnityEngine.Debug.Log("Found " + createdPaths.Count);
            createdPaths.Clear();
        }
    }

    public static Path<Tile> FindAStarPath(Tile start, Tile destination)
    {
        var closed = new HashSet<Tile>();
        var queue = new PriorityQueue<double, Path<Tile>>();

        queue.Enqueue(0, new Path<Tile>(start));

        Tile previousTile = start;

        while (!queue.IsEmpty)
        {
            var path = queue.Dequeue();
            Debug.Log("Path now on " + path.LastStep.Coordinates);

            if (closed.Contains(path.LastStep))
                continue;

            if (path.LastStep.Equals(destination))
                return path;

            closed.Add(path.LastStep);

            foreach (Tile n in path.LastStep.Neighbours)
            {
                Debug.Log("Checking tile " + n.Coordinates);
                // We only go forward if the next tile is empty or is the destination
                if (n.IsEmpty || n == destination)
                {
                    Debug.Log("n.IsEmpty " + n.IsEmpty);
                    int cost = IsTurning(path.LastStep, n, previousTile);
                    if ((path.SubCost + cost) / WEIGHT_FACTOR < 3)
                    {
                        Debug.Log("YES ");
                        var newPath = path.AddStep(n, cost);
                        queue.Enqueue(newPath.TotalCost + ManhattanHeuristic(n, destination), newPath);
                    }
                }
            }

            previousTile = path.LastStep;
        }

        return null;
    }

    private static int IsTurning(Tile currentNode, Tile nextNode, Tile previousNode)
    {
        bool isGoingHorizontal = (currentNode.Coordinates.x == nextNode.Coordinates.x) && (currentNode.Coordinates.x == previousNode.Coordinates.x);
        bool isGoingVertical = (currentNode.Coordinates.y == nextNode.Coordinates.y) && (currentNode.Coordinates.y == previousNode.Coordinates.y);
        return isGoingHorizontal || isGoingVertical ? 0 : 1 * WEIGHT_FACTOR;
    }

    private static double ManhattanHeuristic(Tile newNode, Tile end)
    {
        return Math.Abs(newNode.Coordinates.x - end.Coordinates.x) + Math.Abs(newNode.Coordinates.y - end.Coordinates.y);
    }
}