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
                    Instantiate(_emptyTilePrefab, horizontalLayout);
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


        void ReplaceOneTile(Vector2 coordinates)
        {
            Destroy(_gameManager.TilesMatrix[(int)coordinates.x][(int)coordinates.y].GameObjectRepresentation);
            _gameManager.TilesMatrix[(int)coordinates.x][(int)coordinates.y] = new Tile();

            var parent = transform.GetChild((int)coordinates.x);
            var newTile = Instantiate(_emptyTilePrefab, parent);
            newTile.transform.SetSiblingIndex((int)coordinates.y);
        }
    }
}
