using UnityEngine;

/// <summary>
/// Display the panel at the end of the game to the user
/// </summary>
[RequireComponent(typeof(Animator))]
public class EndOfGamePanelHandler : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private TMPro.TextMeshProUGUI _resultText;
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        _animator.SetTrigger("ShowPanel");
    }
}
