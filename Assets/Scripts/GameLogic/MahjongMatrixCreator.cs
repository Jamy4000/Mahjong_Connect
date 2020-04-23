using UnityEngine;
using System;
using System.Collections.Generic;

public class MahjongMatrixCreator : MonoBehaviour
{
    public Tile[][] TilesMatrix;

    private Tile[] _tilesToPresent;

    private void Awake()
    {
        CreateTiles();
        GetCurrentLayout();
    }

    private void CreateTiles()
    {
        // Set the amount of tiles to present to the matrix size divided by 2 (as we work in pairs)
        _tilesToPresent = new Tile[GameManager.CurrentLevel.Height * GameManager.CurrentLevel.Length / 2];

        var textures = Resources.LoadAll<Texture2D>("TileIcons");
        List<int> usedImages = new List<int>();

        for (int i = 0; i < _tilesToPresent.Length; i++) 
        {
            int randomImageIndex = UnityEngine.Random.Range(0, textures.Length);
            while (usedImages.Contains(randomImageIndex)) 
            {
                randomImageIndex = UnityEngine.Random.Range(0, textures.Length);
            }

            usedImages.Add(randomImageIndex);

            // If we already got all images, we reset the list
            if (usedImages.Count == textures.Length)
                usedImages.Clear();

            var newSprite = Sprite.Create(textures[randomImageIndex], new Rect(0.0f, 0.0f, textures[randomImageIndex].width, textures[randomImageIndex].height), new Vector2(0.5f, 0.5f), 100.0f);
            _tilesToPresent[i] = new Tile(textures[randomImageIndex].name, newSprite);
        }
    }

    private void GetCurrentLayout()
    {
        // Fetch the layout file
        string pathToLayout = System.IO.Path.Combine("Layouts", GameManager.CurrentLevel.LayoutFileName);
        Debug.Log("pathToLayout " + pathToLayout);
        TextAsset txt = (TextAsset)Resources.Load(pathToLayout);

        // Split the file line by line
        var lines = txt.text.Split("\n"[0]);

        // Create the first row of the matrix + 2 for the 2 empty rows surrounding the matrix 
        TilesMatrix = new Tile[GameManager.CurrentLevel.Length + 2][];

        for (int x = 0; x < GameManager.CurrentLevel.Length + 2; x++)
        {
            TilesMatrix[x] = new Tile[GameManager.CurrentLevel.Height + 2];
        }

        // Check that we have the same length as number of lines
        if (lines.Length != GameManager.CurrentLevel.Height) 
        {
            Debug.LogError("The amount of lines in the layout file is different than the current level Length level. That's not possible. Not creating the Game.");
            return;
        }

        Dictionary<int, int> usedTiles = new Dictionary<int, int>();

        for (int i = 0; i < GameManager.CurrentLevel.Height; i++)
        {
            for (int j = 0; j < GameManager.CurrentLevel.Length ; j++)
            {
                // No tile here
                if (lines[i][j] == '0') 
                {
                    TilesMatrix[j + 1][i + 1] = null;
                }
                else
                {
                    int randomIndex = UnityEngine.Random.Range(0, _tilesToPresent.Length);

                    // If we already used this tile and this tile was used 2 times, we generate a new random index
                    while (usedTiles.ContainsKey(randomIndex) && usedTiles[randomIndex] == 2)
                    {
                        randomIndex = UnityEngine.Random.Range(0, _tilesToPresent.Length);
                    }

                    TilesMatrix[j + 1][i + 1] = _tilesToPresent[randomIndex];
                    CheckUsedTiles(randomIndex);
                }
            }
        }


        void CheckUsedTiles(int randomIndex)
        {
            // The tile was already chosen 
            if (usedTiles.ContainsKey(randomIndex))
            {
                usedTiles[randomIndex]++;

                // if we used all possible tiles to present
                if (usedTiles.Count == _tilesToPresent.Length)
                {
                    bool shouldResetDictionary = true;

                    // We check for each tiles if the value is 2. if all tiles were used 2 times, we can reset the dictionary.
                    foreach (var kvp in usedTiles)
                    {
                        if (kvp.Value == 1)
                        {
                            shouldResetDictionary = false;
                            break;
                        }
                    }

                    if (shouldResetDictionary)
                        usedTiles.Clear();
                }
            }
            else
            {
                usedTiles.Add(randomIndex, 1);
            }
        }
    }
}
