using UnityEngine;

public class EndOfGamePanelHandler : MonoBehaviour
{
    [SerializeField] private Animator _endOfGamePanel;
    [SerializeField] private TMPro.TextMeshProUGUI _resultText;
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;
    [SerializeField] private UnityEngine.UI.Image _starImage;

    private void Awake()
    {
        OnGameEnded.Listeners += ShowPanel;
    }

    private void OnDestroy()
    {
        OnGameEnded.Listeners -= ShowPanel;
    }

    private void ShowPanel(OnGameEnded info)
    {
        _resultText.text = info.HasWon ? "Well done, you did it !" : "Oops, you losed ...";
        _scoreText.text = "Your final score is " + GameManager.Instance.CurrentScore;
        _starImage.color = info.HasWon || GameManager.CurrentLevel.IsDone ? Color.white : Color.black;
        _endOfGamePanel.gameObject.SetActive(true);
        _endOfGamePanel.SetTrigger("ShowPanel");

        Debug.Log("TODO ANIMATION");
    }
}
