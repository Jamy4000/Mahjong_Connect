using UnityEngine;

public class TileClickHandler : MonoBehaviour
{
    private GameManager _gameManager;
    private Animator _animator;
    public Tile ThisTile;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _gameManager = GameManager.Instance;
    }

    public void OnClick()
    {
        if (_gameManager.CurrentlyClickedTile == null)
        {
            _animator.SetTrigger("Select");
            _gameManager.CurrentlyClickedTile = ThisTile;
        }
        else if (_gameManager.CurrentlyClickedTile == ThisTile)
        {
            CancelClick();
        }
        else if (string.Equals(_gameManager.CurrentlyClickedTile.ID, ThisTile.ID))
        {
            CheckClickedTiles();
        }
        else
        {
            new OnUserError();
            CancelClick();
        }
    }

    private void CheckClickedTiles() 
    {
        if (PathFinder.PathValidExist(ThisTile, _gameManager.CurrentlyClickedTile)) 
        {
            new OnUserValideAnswer(ThisTile, _gameManager.CurrentlyClickedTile);
            PathFinder.UpdateAvailablePaths();
        }
        else
        {
            new OnUserError();
            CancelClick();
        }
    }

    private void CancelClick()
    {
        _animator.SetTrigger("Deselect");
    }
}