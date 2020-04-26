using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// The actual script creating the matrix we're going to display to the user.
/// </summary>
public class MahjongMatrixCreator : MonoBehaviour
{
    /// <summary>
    /// The script displaying the matrix on the screen
    /// </summary>
    [SerializeField] private MatrixDisplayer _matrixDisplayer;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        // We check the level's cache, or we parse the layout document
        if (CurrentLayoutIsCached() || LevelLayoutParser.GetLevelLayout())
        {
            if (GenerateMatrix())
                _matrixDisplayer.DisplayMatrix();
            else
                _matrixDisplayer.DisplayError();
        }
        else
        {
            _matrixDisplayer.DisplayError();
        }
    }

    private bool CurrentLayoutIsCached()
    {
        return GameManager.CurrentLevel.LevelLayout != null && GameManager.CurrentLevel.LevelLayout.Length > 0;
    }

    /// <summary>
    /// Method generating the full matrix based on the layout provided in the txt file
    /// </summary>
    /// <returns>True if we could correctly generate the matrix</returns>
    private bool GenerateMatrix()
    {
        var layout = GameManager.CurrentLevel.LevelLayout;

        _gameManager.TilesMatrix = new Tile[layout.Length][];

        Dictionary<int, int> usedIcons = new Dictionary<int, int>();
        int addedTiles = 0;
        var tilesIcons = TileIconsFetcher.FetchIcons();

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
                    tilesIcons = TileIconsFetcher.GenerateTextureArray(GameManager.CurrentLevel.BaseTileAmount - addedTiles, (GameManager.CurrentLevel.BaseTileAmount - addedTiles) / 2);
            }
        }

        return true;
    }
}