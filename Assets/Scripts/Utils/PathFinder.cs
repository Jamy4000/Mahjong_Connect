using System.Collections.Generic;

public static class PathFinder
{
    private static List<Path> _availablePaths = new List<Path>();

    public static bool PathValidExist(Tile thisTile, Tile currentlyClickedTile)
    {
        return true;

        foreach (var path in _availablePaths)
        {
            if (path.From == thisTile && path.To == currentlyClickedTile)
                return true;
        }

        return false;
    }

    internal static void UpdateAvailablePaths()
    {
        var tileMatrix = GameManager.Instance.TilesMatrix;
        for (int i = 1; i < tileMatrix.Length - 1; i++)
        {
            for (int j = 1; j < tileMatrix[i].Length - 1; j++)
            {
                if (tileMatrix[i][j] != null)
                {
                    //Tile neighbor = CheckNeighbors(i - 1, j, i, j, tileMatrix[i][j].ID, 0);
                    //if (neighbor != null)
                    //    _availablePaths.Add(new Path(tileMatrix[i][j], neighbor));

                    //neighbor = CheckNeighbors(i, j - 1, i, j, tileMatrix[i][j].ID, 0);
                    //if (neighbor != null)
                    //    _availablePaths.Add(new Path(tileMatrix[i][j], neighbor));

                    //neighbor = CheckNeighbors(i + 1, j, i, j, tileMatrix[i][j].ID, 0);
                    //if (neighbor != null)
                    //    _availablePaths.Add(new Path(tileMatrix[i][j], neighbor));

                    //neighbor = CheckNeighbors(i, j + 1, i, j, tileMatrix[i][j].ID, 0);
                    //if (neighbor != null)
                    //    _availablePaths.Add(new Path(tileMatrix[i][j], neighbor));
                }
            }
        }
    }

    private static bool CheckNeighbors(int i, int j, int originalI, int originalJ, string tileIndex, int cornerSinceBeginning, ref List<Tile> toReturn)
    {
        return false;

        var tileMatrix = GameManager.Instance.TilesMatrix;

        // if the current neighbor isn't an empty tile
        if (!tileMatrix[i][j].IsEmpty) 
        {
            if (tileMatrix[i][j].ID == tileMatrix[originalI][originalJ].ID) 
            {
                toReturn.Add(tileMatrix[i][j]);
                return true;
            }
            else 
            {
                return false;
            }
        }
        else 
        {
            for (int x = -1; x < 2; x += 2)
            {

            }
        }
    }
}

public struct Path 
{
    public Tile From;
    public Tile To;

    public Path(Tile from, Tile to) 
    {
        From = from;
        To = to;
    }
}