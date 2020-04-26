using UnityEngine;

/// <summary>
/// Animate the star of the End Of Game panel, or at least set its color correcly
/// </summary>
[RequireComponent(typeof(Animator))]
public class EndOfGameStarAnimator : MonoBehaviour
{
    /// <summary>
    /// Was the level already done when the user started the game ?
    /// </summary>
    private bool _levelStatusOnStart;

    /// <summary>
    /// Animator linked to this script
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// The image component for our star
    /// </summary>
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

    /// <summary>
    /// Check if the game was already won before, or if the user lost. In that case, no need for an animation.
    /// </summary>
    private void OnUserIsDone(OnGameEnded info)
    {
        if (_levelStatusOnStart)
        {
            _starImage.enabled = true;
            _starImage.color = Color.white;
        }
        else if (!info.HasWon)
        {
            _starImage.enabled = true;
            _starImage.color = Color.black;
        }
        else
        {
            _starImage.enabled = false;
        }
    }

    /// <summary>
    /// Called from the ShowPanel animation of this animator. Check if the current level has been won AND was a new win.
    /// In that case, display the star animation.
    /// </summary>
    public void OnEoGPanelShown() 
    {
        if (!_levelStatusOnStart && GameManager.CurrentLevel.IsDone)
        {
            _starImage.color = Color.white;
            _animator.SetTrigger("AnimateStar");
        }
    }
}
