using UnityEngine;

[RequireComponent(typeof(LevelLoader))]
public class UserDataDisplayer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _userScore;

    [Header("WARNING: Need to be in ascending order")]
    [SerializeField] private UnityEngine.UI.Image[] _stars;

    private LevelLoader _levelLoader;

    private const string USER_SCORE_KEY = "UserScore";
    private const string USER_LEVEL_DONE = "UserLevelDone";

    private void Awake()
    {
        _levelLoader = GetComponent<LevelLoader>();
    }

    private void Start()
    {
        SetUserValues();
    }

    private void SetUserValues() 
    {
        if (PlayerPrefs.HasKey(USER_SCORE_KEY))
        {
            _userScore.text = PlayerPrefs.GetInt(USER_SCORE_KEY).ToString();
        }
        else 
        {
            PlayerPrefs.SetInt(USER_SCORE_KEY, 0);
            _userScore.text = "0";
        }

        for (int i = 0; i < _stars.Length; i++) 
        {
            if (PlayerPrefs.HasKey(USER_LEVEL_DONE + i)) 
            {
                var level = _levelLoader.GetLevel(i);
                level.IsDone = PlayerPrefs.GetInt(USER_LEVEL_DONE + i) == 1;
                _stars[i].color = level.IsDone ? Color.white : Color.black;
               
            }
            else
            {
                _levelLoader.GetLevel(i).IsDone = false;
                PlayerPrefs.SetInt(USER_LEVEL_DONE + i, 0);
                _stars[i].color = Color.black;
            }
        }
    }
}
