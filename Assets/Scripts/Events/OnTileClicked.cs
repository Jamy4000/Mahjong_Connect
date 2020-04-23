/// <summary>
/// Event raised whenever a tile is clicked by the user
/// </summary>
public class OnTileClicked : EventCallbacks.Event<OnTileClicked>
{
    public readonly Tile ClickedTile;

    public readonly UnityEngine.Vector2 TileCoordinates;

    /// <summary>
    /// Event raised whenever a tile is clicked by the user
    /// </summary>
    /// <param name="clickedTile">The tile that was clicked</param>
    /// <param name="coordinates">The coordinate of the tile in the matrix</param>
    public OnTileClicked(Tile clickedTile, UnityEngine.Vector2 coordinates) : base("Event raised whenever a tile is clicked by the user") 
    {
        ClickedTile = clickedTile;
        ClickedTile.IsSelected = true;

        TileCoordinates = coordinates;

        FireEvent(this);
    }
}
