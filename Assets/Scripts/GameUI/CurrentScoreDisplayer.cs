using System.Collections;
using UnityEngine;

/// <summary>
/// Display the current score on every updates on the screen
/// </summary>
public class CurrentScoreDisplayer : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _scoreValueField;

    private void Awake()
    {
        OnUserValideAnswer.Listeners += UpdateScore;
        OnUserError.Listeners += UpdateScore;
    }

    private void OnDestroy()
    {
        OnUserValideAnswer.Listeners -= UpdateScore;
        OnUserError.Listeners -= UpdateScore;
    }

    private void UpdateScore(OnUserError info)
    {
        StartCoroutine(DisplayNewScore());
    }

    private void UpdateScore(OnUserValideAnswer info)
    {
        StartCoroutine(DisplayNewScore());
    }

    /// <summary>
    /// Wait one frame to be sure that the score as been updated in the Game Manager
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayNewScore() 
    {
        yield return new WaitForEndOfFrame();
        _scoreValueField.text = GameManager.Instance.CurrentScore.ToString();
    }
}
