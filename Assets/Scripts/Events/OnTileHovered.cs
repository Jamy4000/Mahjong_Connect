/// <summary>
/// Used in debug mode
/// </summary>
public class OnTileHovered : EventCallbacks.Event<OnTileHovered>
{
    public readonly Tile HoveredTile;

    /// <summary>
    /// Used in debug mode
    /// </summary>
    public OnTileHovered(Tile hoveredTile) : base("Used in debug mode")
    {
        HoveredTile = hoveredTile;
        FireEvent(this);
    }
}
