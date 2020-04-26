using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle the click callback on the Tiles
/// </summary>
public class TileClickHandler : MonoBehaviour
{
    /// <summary>
    /// The tile linked to this script
    /// </summary>
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

    /// <summary>
    /// Callback for when the tile is clicked, called from the button component
    /// </summary>
    public void OnClick()
    {
        // if no tile was selected until now
        if (_gameManager.CurrentlyClickedTile == null)
        {
            // this become the selected tile
            _animator.SetTrigger("Select");
            _gameManager.CurrentlyClickedTile = ThisTile;
            _isSelected = true;
        }
        // if the player want to deselect this tile
        else if (_gameManager.CurrentlyClickedTile == ThisTile)
        {
            CancelClick(false);
        }
        // if the player try to match two tiles together with the same id
        else if (string.Equals(_gameManager.CurrentlyClickedTile.ID, ThisTile.ID))
        {
            _isSelected = true;
            CheckClickedTiles();
        }
        // if the user matched two tiles that aren't the same
        else
        {
            new OnUserError();
            CancelClick(true);
        }
    }

    /// <summary>
    /// Check if a path exist between the two clicked tiles 
    /// </summary>
    private void CheckClickedTiles() 
    {
        if (PathFinder.FetchPath(ThisTile, _gameManager.CurrentlyClickedTile, true) != null) 
            new OnUserValideAnswer(ThisTile, _gameManager.CurrentlyClickedTile);
        else
            new OnUserError();
    }

    /// <summary>
    /// Callback for when the user makes a mistake
    /// </summary>
    private void UserMadeAnError(OnUserError _) 
    {
        if (_isSelected)
            CancelClick(true);
    }

    /// <summary>
    /// Callback for when the user was right by matching two tiles
    /// </summary>
    /// <param name="_"></param>
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
        Destroy(_ps);

        // set empty tile to transparent
        GetComponent<Image>().color = new Color(0, 0, 0, 0);

        Destroy(this);
    }

    /// <summary>
    /// Cancel a click on the current tile
    /// </summary>
    /// <param name="isError">Is the cancel happening because the user made an error ?</param>
    private void CancelClick(bool isError)
    {
        _gameManager.CurrentlyClickedTile = null;
        _animator.SetTrigger(isError ? "Error" : "Deselect");
        _isSelected = false;
    }
}