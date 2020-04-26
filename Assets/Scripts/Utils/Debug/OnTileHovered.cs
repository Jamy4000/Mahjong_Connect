/// <summary>
/// Used in debug mode to show all possible path of the hovered tile
/// </summary>
public class OnTileHovered : EventCallbacks.Event<OnTileHovered>
{
    public readonly Tile HoveredTile;

    /// <summary>
    /// Used in debug mode to show all possible path of the hovered tile
    /// </summary>
    public OnTileHovered(Tile hoveredTile) : base("Used in debug mode to show all possible path of the hovered tile")
    {
        HoveredTile = hoveredTile;
        FireEvent(this);
    }
}
