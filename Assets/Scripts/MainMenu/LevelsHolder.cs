using UnityEngine;

/// <summary>
/// Simply holds all levels that are available in the game
/// </summary>
public class LevelsHolder : MonoBehaviour
{
    [SerializeField] private Level[] _levels = new Level[5];

    public Level[] Levels 
    {
        get { return _levels; }
    }

    public int LevelAmount
    {
        get
        {
            return _levels.Length;
        }
    }
}
