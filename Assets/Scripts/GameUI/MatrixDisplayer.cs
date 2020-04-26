using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Display th ematrix on the screen once it has been created
/// </summary>
public class MatrixDisplayer : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private Animator _errorPanelAnimator;
    [SerializeField] private GameObject _verticalLayoutPrefab;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _emptyTilePrefab;

    private void Awake()
    {
        OnUserValideAnswer.Listeners += ReplaceTiles;
    }

    private void OnDestroy()
    {
        OnUserValideAnswer.Listeners -= ReplaceTiles;
    }

    /// <summary>
    /// Method called from the MahjongMatrixCreator, instantiate all tiles
    /// </summary>
    public void DisplayMatrix()
    {
        _gameManager = GameManager.Instance;

        for (int x = 0; x < _gameManager.TilesMatrix.Length; x++)
        {
            var verticalLayout = Instantiate(_verticalLayoutPrefab, transform).transform;

            for (int y = 0; y < _gameManager.TilesMatrix[0].Length; y++)
            {
                // If empty tile, we display an empty tile 
                if (_gameManager.TilesMatrix[x][y].IsEmpty) 
                {
                    var newTile = Instantiate(_emptyTilePrefab, verticalLayout);
                    _gameManager.TilesMatrix[x][y].GameObjectRepresentation = newTile;
                }
                else 
                {
                    var newTile = Instantiate(_tilePrefab, verticalLayout);
                    _gameManager.TilesMatrix[x][y].GameObjectRepresentation = newTile;
                    newTile.transform.GetChild(0).GetComponent<Image>().sprite = _gameManager.TilesMatrix[x][y].Icon;
                    newTile.GetComponent<TileClickHandler>().ThisTile = _gameManager.TilesMatrix[x][y];
                }
            }
        }
    }

    /// <summary>
    /// Display an error panel if necessary, like if the layout couldn't be parse properly
    /// </summary>
    public void DisplayError()
    {
        _errorPanelAnimator.SetTrigger("ShowPanel");
    }

    /// <summary>
    /// Replace the two tiles that were successfully joined by two empty tiles
    /// </summary>
    /// <param name="info"></param>
    private void ReplaceTiles(OnUserValideAnswer info)
    {
        ReplaceOneTile(info.FirstTile.Coordinates);
        ReplaceOneTile(info.SecondTile.Coordinates);


        void ReplaceOneTile(int2 coordinates)
        {
            var tileObject = _gameManager.TilesMatrix[coordinates.x][coordinates.y].GameObjectRepresentation;

            // Set the tile in the matrix as a new empty tile
            _gameManager.TilesMatrix[coordinates.x][coordinates.y] = new Tile(coordinates)
            {
                GameObjectRepresentation = tileObject
            };
        }
    }
}
