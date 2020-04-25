/// <summary>
/// Used in debug mode
/// </summary>
public class OnTileUnhovered : EventCallbacks.Event<OnTileUnhovered>
{
    /// <summary>
    /// Used in debug mode
    /// </summary>
    public OnTileUnhovered() : base("Used in debug mode")
    {
        FireEvent(this);
    }
}
