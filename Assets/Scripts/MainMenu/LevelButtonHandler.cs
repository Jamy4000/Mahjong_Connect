using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handle the callbacks for when the user select a level
/// </summary>
public class LevelButtonHandler : MonoBehaviour
{
    [HideInInspector] public Level _thisLevel;
    [SerializeField] private Image _starIcon;
    [SerializeField] private Sprite _doneLevelBackground;
    [SerializeField] private TMPro.TextMeshProUGUI _levelName;
    [SerializeField] private string _gameSceneName = "MainGame";

    public void Init(Level thisLevel)
    {
        _thisLevel = thisLevel;
        _levelName.text = _thisLevel.Name;
        _starIcon.color = _thisLevel.IsDone ? Color.white : Color.black;
        if (_thisLevel.IsDone)
            GetComponent<Image>().sprite = _doneLevelBackground;
    }

    public void LoadLevel()
    {
        GameManager.CurrentLevel = _thisLevel;
        SceneManager.LoadSceneAsync(_gameSceneName);
    }
}
