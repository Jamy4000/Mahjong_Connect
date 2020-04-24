using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Sprite Icon;
    public string ID;

    public bool IsEmpty = false;
    public Vector2 Coordinates;

    public GameObject GameObjectRepresentation;

    public Tile(string iconName, Texture2D iconTexture, Vector2 coordinates) 
    {
        ID = iconName;
        Icon = Sprite.Create(iconTexture, new Rect(0.0f, 0.0f, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        Coordinates = coordinates;
    }

    public Tile()
    {
        IsEmpty = true;
    }

    public static Tile CreateNewTile(ref Dictionary<int, int> usedIcons, Texture2D[] tilesIcons, Vector2 coordinates)
    {
        int randomIndex = Random.Range(0, tilesIcons.Length);

        // If we already used this tile and this tile was used 2 times, we generate a new random index
        while (usedIcons.ContainsKey(randomIndex) && usedIcons[randomIndex] == 2)
            randomIndex = Random.Range(0, tilesIcons.Length);

        var newTile = new Tile(tilesIcons[randomIndex].name, tilesIcons[randomIndex], coordinates);
        GameManager.Instance.AddNewTileToDicitonary(newTile);

        CheckUsedTiles(ref usedIcons, tilesIcons.Length, randomIndex);
        return newTile;
    }

    private static void CheckUsedTiles(ref Dictionary<int, int> usedIcons, int tilesIconLength, int randomIndex)
    {
        // The tile was already chosen 
        if (usedIcons.ContainsKey(randomIndex))
        {
            usedIcons[randomIndex]++;

            // if we used all possible tiles to present
            if (usedIcons.Count == tilesIconLength)
            {
                bool shouldResetDictionary = true;

                // We check for each tiles if the value is 2. if all tiles were used 2 times, we can reset the dictionary.
                foreach (var kvp in usedIcons)
                {
                    if (kvp.Value == 1)
                    {
                        shouldResetDictionary = false;
                        break;
                    }
                }

                if (shouldResetDictionary)
                    usedIcons.Clear();
            }
        }
        else
        {
            usedIcons.Add(randomIndex, 1);
        }
    }
}
