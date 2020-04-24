using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// We need to go 2 times through the layout as we need the amount of tiles to present in order to know how many pairs we can form
/// </summary>
public class MahjongMatrixCreator : MonoBehaviour
{
    [SerializeField] private MatrixDisplayer _matrixDisplayer;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        if (LevelLayoutParser.GetLevelLayout(out bool[][] layout, out int fullTileAmount))
        {
            Texture2D[] tilesIcons = TileIconsFetcher.FetchIcons(fullTileAmount);

            if (GenerateMatrix(tilesIcons, layout))
                _matrixDisplayer.DisplayMatrix();
            else
                _matrixDisplayer.DisplayError();
        }
        else
        {
            _matrixDisplayer.DisplayError();
        }
    }

    private bool GenerateMatrix(Texture2D[] tilesIcons, bool[][] layout)
    {
        _gameManager.TilesMatrix = new Tile[layout.Length][];
        Dictionary<int, int> usedIcons = new Dictionary<int, int>();

        // We go through all the lines in the document + 2 (first and last rows are empty)
        for (int i = 0; i < _gameManager.TilesMatrix.Length; i++)
        {
            _gameManager.TilesMatrix[i] = new Tile[layout[i].Length];

            // we go through all characters in the current line, + 2 (first and last columns are empty)
            for (int j = 0; j < _gameManager.TilesMatrix[i].Length; j++)
            {
                _gameManager.TilesMatrix[i][j] = layout[i][j] ? Tile.CreateNewTile(ref usedIcons, tilesIcons, new Vector2(i, j)) : new Tile();
            }
        }

        return true;
    }
}
