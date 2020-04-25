using UnityEngine;

[RequireComponent(typeof(LevelLoader))]
public class UserDataDisplayer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _userScore;

    [Header("WARNING: Need to be in ascending order")]
    [SerializeField] private UnityEngine.UI.Image[] _stars;

    private LevelLoader _levelLoader;

    public const string USER_LEVEL_SCORE = "UserLevelScore";

    private void Awake()
    {
        _levelLoader = GetComponent<LevelLoader>();
    }

    private void Start()
    {
        GetUserScore();
    }

    /// <summary>
    /// TODO Seperate this in two methods, in two different classes
    /// </summary>
    private void GetUserScore() 
    {
        int mainScore = 0;

        for (int i = 0; i < _stars.Length; i++)
        {
            var level = _levelLoader.GetLevel(i);

            if (PlayerPrefs.HasKey(USER_LEVEL_SCORE + i)) 
            {
                level.Score = PlayerPrefs.GetInt(USER_LEVEL_SCORE + i);
                level.IsDone = level.Score > 0;
                _stars[i].color = level.IsDone ? Color.white : Color.black;
                mainScore += level.Score;
            }
            else
            {
                level.IsDone = false;
                level.Score = 0;
                PlayerPrefs.SetInt(USER_LEVEL_SCORE + i, 0);
                _stars[i].color = Color.black;
            }
        }

        _userScore.text = mainScore.ToString();
    }
}
