/// <summary>
/// Event raised when the user is done, by either losing or winning
/// </summary>
public class OnGameEnded : EventCallbacks.Event<OnGameEnded>
{
    public readonly bool HasWon;

    /// <summary>
    /// Event raised when the user is done, by either losing or winning
    /// </summary>
    public OnGameEnded(bool hasWon) : base("Event raised when the user is done, by either losing or winning")
    {
        HasWon = hasWon;
        FireEvent(this);
    }
}
