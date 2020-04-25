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

        if (LevelLayoutParser.GetLevelLayout(out bool[][] layout))
        {
            Texture2D[] tilesIcons = TileIconsFetcher.FetchIcons();

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
        int addedTiles = 0;

        // We go through all the lines in the layout
        for (int x = 0; x < layout.Length; x++)
        {
            _gameManager.TilesMatrix[x] = new Tile[layout[0].Length];

            // we go through all characters in the current line
            for (int y = 0; y < _gameManager.TilesMatrix[x].Length; y++)
            {
                if (layout[x][y]) 
                {
                    _gameManager.TilesMatrix[x][y] = Tile.CreateNewTile(ref usedIcons, tilesIcons, new Unity.Mathematics.int2(x, y));
                    addedTiles++;
                }
                else
                {
                    _gameManager.TilesMatrix[x][y] = new Tile(new Unity.Mathematics.int2(x, y));
                }

                // when we reset the usedIcons dictionary
                if (usedIcons.Count == 0) 
                    tilesIcons = TileIconsFetcher.GenerateTextureArray(_gameManager.TileAmount - addedTiles, (_gameManager.TileAmount - addedTiles) / 2);
            }
        }

        return true;
    }
}