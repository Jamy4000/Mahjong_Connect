using UnityEngine;
using UnityEngine.UI;

public class TileClickHandler : MonoBehaviour
{
    public Tile ThisTile;

    private GameManager _gameManager;
    private Animator _animator;
    private ParticleSystem _ps;

    private bool _isSelected;

    private void Awake()
    {
        OnUserError.Listeners += UserMadeAnError;
        OnUserValideAnswer.Listeners += UserMadeAValidAnswer;
        _animator = GetComponent<Animator>();
        _ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnDestroy()
    {
        OnUserError.Listeners -= UserMadeAnError;
        OnUserValideAnswer.Listeners -= UserMadeAValidAnswer;
    }

    public void OnClick()
    {
        if (_gameManager.CurrentlyClickedTile == null)
        {
            _animator.SetTrigger("Select");
            _gameManager.CurrentlyClickedTile = ThisTile;
            _isSelected = true;
        }
        else if (_gameManager.CurrentlyClickedTile == ThisTile)
        {
            CancelClick(false);
        }
        else if (string.Equals(_gameManager.CurrentlyClickedTile.ID, ThisTile.ID))
        {
            _isSelected = true;
            CheckClickedTiles();
        }
        else
        {
            new OnUserError();
            CancelClick(true);
        }
    }

    private void CheckClickedTiles() 
    {
        if (PathFinder.FetchPath(ThisTile, _gameManager.CurrentlyClickedTile, true) != null) 
            new OnUserValideAnswer(ThisTile, _gameManager.CurrentlyClickedTile);
        else
            new OnUserError();
    }

    private void UserMadeAnError(OnUserError _) 
    {
        if (_isSelected)
            CancelClick(true);
    }

    private void UserMadeAValidAnswer(OnUserValideAnswer _)
    {
        if (_isSelected)
        {
            _isSelected = false;
            _animator.SetTrigger("Disappear");
            _ps.Play();
        }
    }

    /// <summary>
    /// Called from animator system when valid answer anim is done
    /// </summary>
    public void DestroyTileElements() 
    {
        // Destroy all components and gameObject that we don't need anymore
        Destroy(transform.GetChild(0).gameObject);
        Destroy(GetComponent<Button>());
        Destroy(GetComponent<Animator>());

        // set empty tile to transparent
        GetComponent<Image>().color = new Color(0, 0, 0, 0);

        Destroy(this);
    }

    private void CancelClick(bool isError)
    {
        _gameManager.CurrentlyClickedTile = null;
        _animator.SetTrigger(isError ? "Error" : "Deselect");
        _isSelected = false;
    }
}