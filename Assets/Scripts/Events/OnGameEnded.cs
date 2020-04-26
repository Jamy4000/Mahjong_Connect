/// <summary>
/// Event raised when the user is done, by either losing or winning
/// </summary>
public class OnGameEnded : EventCallbacks.Event<OnGameEnded>
{
    /// <summary>
    /// Did the player win or lose ?
    /// </summary>
    public readonly bool HasWon;

    /// <summary>
    /// Event raised when the user is done, by either losing or winning
    /// </summary>
    /// <param name="hasWon">Did the player win or lose ?</param>
    public OnGameEnded(bool hasWon) : base("Event raised when the user is done, by either losing or winning")
    {
        HasWon = hasWon;
        FireEvent(this);
    }
}
