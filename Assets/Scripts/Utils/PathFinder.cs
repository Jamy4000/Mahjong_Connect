using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Path Finder class, generating all available paths 
/// </summary>
public static class PathFinder
{
    /// <summary>
    /// Here to give a bit more weight to each turn the path is taking
    /// </summary>
    private const int WEIGHT_FACTOR = 100;

    private static List<Path<Tile>> _cachedPaths = new List<Path<Tile>>();

    /// <summary>
    /// Check if the path from thisTile to currentlyClickedTile was cached, or can be generated.
    /// </summary>
    /// <param name="thisTile">The tile where we want to go to</param>
    /// <param name="currentlyClickedTile">The tile where we start the path</param>
    /// <param name="updateCacheOnFound">Do we clear the cache when the path is found ?</param>
    /// <returns>The path, if available</returns>
    public static Path<Tile> FetchPath(Tile thisTile, Tile currentlyClickedTile, bool updateCacheOnFound)
    {
        // Check in the cached path contains a path with the two provided tiles
        var index = _cachedPaths.FindIndex(p => IsPath(p.Last(), p.LastStep) || IsPath(p.LastStep, p.Last()));

        // if the path was already generated, no need to calculate it again
        if (index >= 0) 
        {
            if (updateCacheOnFound)
            {
                // copy the path if we want to clear the cache before returning
                var path = _cachedPaths[index];
                _cachedPaths.Clear();
                return path;
            }
            else
            {
                return _cachedPaths[index];
            }
        }
        else
        {
            // Try to generate a new path
            var newPath = FindAStarPath(thisTile, currentlyClickedTile);
            
            if (newPath != null) 
            {
                // Cache it if we don't want to clear the cache
                if (updateCacheOnFound)
                    _cachedPaths.Clear();
                else
                    _cachedPaths.Add(newPath);
            }

            return newPath;
        }

        bool IsPath(Tile start, Tile end) 
        {
            return start == currentlyClickedTile && end == thisTile;
        }
    }

    /// <summary>
    /// A* search algorithm, but without closedNode list, as they don't take into account the weight of the turns
    /// </summary>
    /// <param name="start">The start position for our algorithm</param>
    /// <param name="destination">The final tile we want to go to</param>
    /// <returns>The shortest path, if available</returns>
    private static Path<Tile> FindAStarPath(Tile start, Tile destination)
    {
        var queue = new PriorityQueue<double, Path<Tile>>();

        // Enqueue the start tile 
        queue.Enqueue(0, new Path<Tile>(start));

        // As long as we've got possible moves
        while (!queue.IsEmpty)
        {
            // We get the next available move, based on its priority
            var path = queue.Dequeue();

            // If the new step is our destination, we're good and can return the current path
            if (path.LastStep.Equals(destination))
                return path;

            // We go through all tiles that are neighbours of our current tile
            foreach (Tile n in path.LastStep.Neighbours)
            {
                // We only go forward if the next tile is empty or is the destination
                if (n.IsEmpty || n == destination)
                {
                    int cost = 0;

                    // If this is the first step we're taking, no need to check for a cost
                    if (path.PreviousSteps != null)
                    {
                        // Checking if we already came on this tile in the past
                        if (path.PreviousSteps.Contains(n))
                            continue;

                        // if the user is turning, we change the cost for this step
                        cost = GetMovementCost(path.PreviousSteps.LastStep, path.LastStep, n);
                    }

                    // We limit the movements to 2 turns only
                    if ((path.TotalCost + cost) < 3 * WEIGHT_FACTOR)
                    {
                        var newPath = path.AddStep(n, cost);
                        queue.Enqueue(newPath.TotalCost + EuclideanHeuristic(n, destination), newPath);
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Return the weight of the movement based on whether the nextNode is creating a turn
    /// </summary>
    /// <param name="previousNode">The step before the current node</param>
    /// <param name="currentNode">The current node on which the path is</param>
    /// <param name="nextNode">The next theoric node to check</param>
    /// <returns>0 if the step is going forward, 1 * WEIGHT_FACTOR if turning</returns>
    private static int GetMovementCost(Tile previousNode, Tile currentNode, Tile nextNode)
    {
        bool isGoingHorizontal = (currentNode.Coordinates.x == nextNode.Coordinates.x) && (currentNode.Coordinates.x == previousNode.Coordinates.x);
        bool isGoingVertical = (currentNode.Coordinates.y == nextNode.Coordinates.y) && (currentNode.Coordinates.y == previousNode.Coordinates.y);
        return isGoingHorizontal || isGoingVertical ? 0 : 1 * WEIGHT_FACTOR;
    }

    /// <summary>
    /// Simple implementation of an Euclidean Heuristic
    /// </summary>
    /// <param name="newNode">The next node we want to go to</param>
    /// <param name="end">The destination of our path</param>
    /// <returns>The euclidean distance between those two nodes</returns>
    private static double EuclideanHeuristic(Tile newNode, Tile end)
    {
        return Math.Sqrt(Math.Pow(newNode.Coordinates.x - end.Coordinates.x, 2.0) + Math.Pow(newNode.Coordinates.y - end.Coordinates.y, 2.0));
    }
}