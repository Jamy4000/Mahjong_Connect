using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MatrixDisplayer : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private GameObject _verticalLayoutPrefab;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _emptyTilePrefab;

    private void Awake()
    {
        OnUserValideAnswer.Listeners += ReplaceTiles;
    }

    internal void DisplayError()
    {
        Debug.LogError("TODO");
    }

    private void OnDestroy()
    {
        OnUserValideAnswer.Listeners -= ReplaceTiles;
    }

    public void DisplayMatrix()
    {
        _gameManager = GameManager.Instance;

        for (int x = 0; x < _gameManager.TilesMatrix.Length; x++)
        {
            var verticalLayout = Instantiate(_verticalLayoutPrefab, transform).transform;

            for (int y = 0; y < _gameManager.TilesMatrix[0].Length; y++)
            {
                // If empty tile, we don't display anything
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

    private void ReplaceTiles(OnUserValideAnswer info)
    {
        ReplaceOneTile(info.FirstTile.Coordinates);
        ReplaceOneTile(info.SecondTile.Coordinates);


        void ReplaceOneTile(int2 coordinates)
        {
            var tileObject = _gameManager.TilesMatrix[coordinates.x][coordinates.y].GameObjectRepresentation;

            // Destroy all components and gameObject that we don't need anymore
            Destroy(tileObject.transform.GetChild(0).gameObject);
            Destroy(tileObject.GetComponent<Button>());
            Destroy(tileObject.GetComponent<TileClickHandler>());
            Destroy(tileObject.GetComponent<Animator>());

            // set empty tile to transparent
            tileObject.GetComponent<Image>().color = new Color(0, 0, 0, 0);

            // Set the tile in the matrix as a new empty tile
            _gameManager.TilesMatrix[coordinates.x][coordinates.y] = new Tile(coordinates)
            {
                GameObjectRepresentation = tileObject
            };
        }
    }
}
