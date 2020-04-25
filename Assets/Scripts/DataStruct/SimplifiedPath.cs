/// <summary>
/// A simplified version of the path, simply for calculations purposes.
/// </summary>
public struct SimplifiedPath
{
    public Tile From;
    public Tile To;

    public SimplifiedPath(Tile from, Tile to)
    {
        From = from;
        To = to;
    }
}
