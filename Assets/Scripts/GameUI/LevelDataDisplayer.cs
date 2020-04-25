using UnityEngine;

public class LevelDataDisplayer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _levelName;
    [SerializeField] private TMPro.TextMeshProUGUI _userHighestScore;

    private void Start()
    {
        _levelName.text = GameManager.CurrentLevel.Name;
        _userHighestScore.text = GameManager.CurrentLevel.Score.ToString();
    }
}