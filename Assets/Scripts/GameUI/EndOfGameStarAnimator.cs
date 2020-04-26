using UnityEngine;

public class EndOfGameStarAnimator : MonoBehaviour
{
    private bool _levelStatusOnStart;
    private Animator _animator;
    [SerializeField] private UnityEngine.UI.Image _starImage;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _levelStatusOnStart = GameManager.CurrentLevel.IsDone;
        OnGameEnded.Listeners += OnUserIsDone;
    }

    private void OnDestroy()
    {
        OnGameEnded.Listeners -= OnUserIsDone;
    }

    private void OnUserIsDone(OnGameEnded info)
    {
        if (_levelStatusOnStart)
        {
            _starImage.transform.localScale = Vector3.one;
            _starImage.color = Color.white;
        }
        else if (!info.HasWon)
        {
            _starImage.transform.localScale = Vector3.one;
            _starImage.color = Color.black;
        }
    }

    public void OnEoGPanelShown() 
    {
        if (!_levelStatusOnStart && GameManager.CurrentLevel.IsDone)
        {
            _starImage.color = Color.white;
            _animator.SetTrigger("AnimateStar");
        }
    }
}
