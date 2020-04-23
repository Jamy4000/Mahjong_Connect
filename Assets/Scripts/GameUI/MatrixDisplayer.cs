using UnityEngine;
using UnityEngine.UI;

public class MatrixDisplayer : MonoBehaviour
{
    [SerializeField] private MahjongMatrixCreator _matrixCreator;
    [SerializeField] private GameObject _verticalLayoutPrefab;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _emptyTilePrefab;

    private void Start()
    {
        DisplayMatrix();
    }

    private void DisplayMatrix()
    {
        for (int i = 0; i < GameManager.CurrentLevel.Length + 2; i++)
        {
            var verticalLayout = Instantiate(_verticalLayoutPrefab, transform).transform;

            for (int j = 0; j < GameManager.CurrentLevel.Height + 2; j++)
            {
                // If empty tile, we don't display anything
                if (_matrixCreator.TilesMatrix[i][j] == null) 
                {
                    Instantiate(_emptyTilePrefab, verticalLayout);
                }
                else 
                {
                    var newTile = Instantiate(_tilePrefab, verticalLayout);
                    newTile.transform.GetChild(0).GetComponent<Image>().sprite = _matrixCreator.TilesMatrix[i][j].Icon;

                    newTile.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        newTile.GetComponent<TileStatusHandler>().OnClick();
                        new OnTileClicked(_matrixCreator.TilesMatrix[i][j], new Vector2(i, j));
                    });
                }
            }
        }
    }
}
