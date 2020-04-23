/// <summary>
/// Event raised whenever the user make a mistake
/// </summary>
public class OnUserError : EventCallbacks.Event<OnUserError>
{
    /// <summary>
    /// Event raised whenever the user make a mistake
    /// </summary>
    public OnUserError() : base("Event raised whenever the user make a mistake")
    {
        FireEvent(this);
    }
}
