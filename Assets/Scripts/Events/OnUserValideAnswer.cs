/// <summary>
/// Event raised whenever the user has given a valid answer
/// </summary>
public class OnUserValideAnswer : EventCallbacks.Event<OnUserValideAnswer>
{
    public readonly Tile FirstTile;
    public readonly Tile SecondTile;

    /// <summary>
    /// Event raised whenever the user has given a valid answer
    /// </summary>
    public OnUserValideAnswer(Tile firstTile, Tile secondTile) : base("Event raised whenever the user has given a valid answer")
    {
        FirstTile = firstTile;
        SecondTile = secondTile;
        FireEvent(this);
    }
}
