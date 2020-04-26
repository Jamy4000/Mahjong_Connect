/// <summary>
/// Used in debug mode to show all possible path of the hovered tile
/// </summary>
public class OnTileUnhovered : EventCallbacks.Event<OnTileUnhovered>
{
    /// <summary>
    /// Used in debug mode to show all possible path of the hovered tile
    /// </summary>
    public OnTileUnhovered() : base("Used in debug mode to show all possible path of the hovered tile")
    {
        FireEvent(this);
    }
}
