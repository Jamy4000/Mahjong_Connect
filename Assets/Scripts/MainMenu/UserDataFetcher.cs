using UnityEngine;

/// <summary>
/// Fetch the data from the user, stored in the Players Prefs
/// </summary>
[RequireComponent(typeof(LevelsHolder))]
public class UserDataFetcher : MonoBehaviour
{
    /// <summary>
    /// The script holding the Levels as Scriptable Objects
    /// </summary>
    private LevelsHolder _levelsHolder;

    /// <summary>
    /// The key of the score for each level in the players pref. Add the Level index alongside.
    /// </summary>
    public const string USER_LEVEL_SCORE = "UserLevelScore";

    private void Awake()
    {
        _levelsHolder = GetComponent<LevelsHolder>();
        GetUserData();
    }

    /// <summary>
    /// Fetch the user data from the players prefs and save them in th Levels SO
    /// </summary>
    private void GetUserData() 
    {
        for (int i = 0; i < _levelsHolder.LevelAmount; i++)
        {
            var level = _levelsHolder.Levels[i];

            if (PlayerPrefs.HasKey(USER_LEVEL_SCORE + i)) 
            {
                level.Score = PlayerPrefs.GetInt(USER_LEVEL_SCORE + i);
                level.IsDone = level.Score > 0;
            }
            else
            {
                level.IsDone = false;
                level.Score = 0;
                PlayerPrefs.SetInt(USER_LEVEL_SCORE + i, 0);
            }
        }
    }
}
