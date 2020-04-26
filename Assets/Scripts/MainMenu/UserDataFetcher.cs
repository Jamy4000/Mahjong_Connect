using UnityEngine;

[RequireComponent(typeof(LevelsHolder))]
public class UserDataFetcher : MonoBehaviour
{
    private LevelsHolder _levelLoader;
    [SerializeField] private bool _clearPlayersPref;
    public const string USER_LEVEL_SCORE = "UserLevelScore";

    private void Awake()
    {
        if (_clearPlayersPref)
            PlayerPrefs.DeleteAll();
        _levelLoader = GetComponent<LevelsHolder>();
        GetUserData();
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetUserData() 
    {
        for (int i = 0; i < _levelLoader.LevelAmount; i++)
        {
            var level = _levelLoader.Levels[i];

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
