using System.Collections.Generic;

public static class PathFinder
{
    private static List<Path> _availablePaths = new List<Path>();

    public static bool PathValidExist(Tile thisTile, Tile currentlyClickedTile)
    {
        return true;

        foreach (var path in _availablePaths)
        {
            if (path.From == currentlyClickedTile && path.To.Contains(thisTile))
                return true;
        }

        return false;
    }

    public static void UpdateAvailablePaths()
    {
        return;

        var tileMatrix = GameManager.Instance.TilesMatrix;

        for (int i = 1; i < tileMatrix.Length - 1; i++)
        {
            for (int j = 1; j < tileMatrix[i].Length - 1; j++)
            {
                List<Tile> neighbors = new List<Tile>();

                if (tileMatrix[i][j] != null)
                {
                    CheckNeighbors(i - 1, j, i, j, 0, ref neighbors);
                    if (neighbors.Count > 0)
                        _availablePaths.Add(new Path(tileMatrix[i][j], neighbors));

                    neighbors.Clear();
                    CheckNeighbors(i, j - 1, i, j, 0, ref neighbors);
                    if (neighbors.Count > 0)
                        _availablePaths.Add(new Path(tileMatrix[i][j], neighbors));

                    neighbors.Clear();
                    CheckNeighbors(i + 1, j, i, j, 0, ref neighbors);
                    if (neighbors.Count > 0)
                        _availablePaths.Add(new Path(tileMatrix[i][j], neighbors));

                    neighbors.Clear();
                    CheckNeighbors(i, j + 1, i, j, 0, ref neighbors);
                    if (neighbors.Count > 0)
                        _availablePaths.Add(new Path(tileMatrix[i][j], neighbors));
                }
            }
        }
    }

    private static void CheckNeighbors(int i, int j, int originalI, int originalJ, int cornerSinceBeginning, ref List<Tile> toReturn)
    {
        var tileMatrix = GameManager.Instance.TilesMatrix;

        // if the current neighbor isn't an empty tile
        if (!tileMatrix[i][j].IsEmpty) 
        {
            if (tileMatrix[i][j].ID == tileMatrix[originalI][originalJ].ID && i != originalI && j != originalJ) 
            {
                toReturn.Add(tileMatrix[i][j]);
            }
        }
        else 
        {
            for (int x = -1; x < 2; x += 2)
            {
                for (int y = -1; y < 2; y += 2)
                {
                    CheckNeighbors(i + x, j + y, originalI, originalJ, cornerSinceBeginning + 1, ref toReturn);
                }
            }
        }
    }
}

public struct Path 
{
    public Tile From;
    public List<Tile> To;

    public Path(Tile from, List<Tile> to) 
    {
        From = from;
        To = to;
    }
}