using UnityEngine;

/// <summary>
/// Display how much stars the user got until now at the bottom of the screen
/// </summary>
public class StarAmountDisplayer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _userScore;
    [SerializeField] private LevelsHolder _levelsHolder;

    private void Start()
    {
        int starAmount = 0;
        foreach (var level in _levelsHolder.Levels)
        {
            if (level.IsDone)
                starAmount++;
        }
        _userScore.text = "Stars Collected: " + starAmount + "/" + _levelsHolder.LevelAmount;
    }
}
