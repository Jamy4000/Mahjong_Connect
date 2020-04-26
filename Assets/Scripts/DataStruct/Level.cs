using UnityEngine;

/// <summary>
/// Scriptable object representing a level in our game. 
/// Create a new one every time you want to add a new level in the game, and add it in the Levels Holder.
/// </summary>
[CreateAssetMenu()]
public class Level : ScriptableObject
{
    /// <summary>
    /// The level index
    /// </summary>
    public int Index;

    /// <summary>
    /// THe level name, displayed in game
    /// </summary>
    public string Name;

    /// <summary>
    /// The name of the file for the layout of the level
    /// </summary>
    public string LayoutFileName;

    /// <summary>
    /// Whether this level was already succeeded
    /// </summary>
    public bool IsDone = false;

    /// <summary>
    /// The score of the level if it was succeeded
    /// </summary>
    public int Score = 0;

    /// <summary>
    /// The amount of tiles when starting the level
    /// </summary>
    public int BaseTileAmount = 0;

    /// <summary>
    /// A simple cache for the level's layout
    /// </summary>
    public bool[][] LevelLayout;
}