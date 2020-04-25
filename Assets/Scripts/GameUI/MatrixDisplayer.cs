using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class MatrixDisplayer : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private GameObject _horizontalLayoutPrefab;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _emptyTilePrefab;

    private void Awake()
    {
        OnUserValideAnswer.Listeners += ReplaceTiles;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
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
        for (int i = 0; i < _gameManager.TilesMatrix.Length; i++)
        {
            var horizontalLayout = Instantiate(_horizontalLayoutPrefab, transform).transform;

            for (int j = 0; j < _gameManager.TilesMatrix[i].Length; j++)
            {
                // If empty tile, we don't display anything
                if (_gameManager.TilesMatrix[i][j].IsEmpty) 
                {
                    var newTile = Instantiate(_emptyTilePrefab, horizontalLayout);
                    _gameManager.TilesMatrix[i][j].GameObjectRepresentation = newTile;
                }
                else 
                {
                    var newTile = Instantiate(_tilePrefab, horizontalLayout);
                    _gameManager.TilesMatrix[i][j].GameObjectRepresentation = newTile;
                    newTile.transform.GetChild(0).GetComponent<Image>().sprite = _gameManager.TilesMatrix[i][j].Icon;
                    newTile.GetComponent<TileClickHandler>().ThisTile = _gameManager.TilesMatrix[i][j];
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
