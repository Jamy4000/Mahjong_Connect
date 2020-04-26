using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Represent a Tile in our game
/// </summary>
public class Tile
{
    /// <summary>
    /// THe Icon we display on our tile
    /// </summary>
    public Sprite Icon;

    /// <summary>
    /// THe ID of our tile, equal to the name of the icon's texture
    /// </summary>
    public string ID;

    /// <summary>
    /// Is this tile empty ?
    /// </summary>
    public bool IsEmpty = false;

    /// <summary>
    /// THe coordinates of the tile on the board
    /// </summary>
    public int2 Coordinates;

    /// <summary>
    /// The actual gameObject representing this tile
    /// </summary>
    public GameObject GameObjectRepresentation;

    public Tile(string iconName, Texture2D iconTexture, int2 coordinates) 
    {
        ID = iconName;
        Icon = Sprite.Create(iconTexture, new Rect(0.0f, 0.0f, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        Coordinates = coordinates;
        IsEmpty = false;
    }

    public Tile(int2 coordinates)
    {
        Coordinates = coordinates;
        IsEmpty = true;
    }

    /// <summary>
    /// Helper's method to create a new tile 
    /// </summary>
    /// <param name="usedIcons">Reference all previously used icons on the board</param>
    /// <param name="tilesIcons">The array of all icons we need to place on the board</param>
    /// <param name="coordinates">The coordinates at which our tile is placed on the board</param>
    /// <returns>The new created tile</returns>
    public static Tile CreateNewTile(ref Dictionary<int, int> usedIcons, Texture2D[] tilesIcons, int2 coordinates)
    {
        // Fetch a random icons texture in the array
        int randomIndex = Random.Range(0, tilesIcons.Length);

        // If we already used this tile and this tile was used 2 times, we generate a new random index
        while (usedIcons.ContainsKey(randomIndex) && usedIcons[randomIndex] == 2)
            randomIndex = Random.Range(0, tilesIcons.Length);

        // Generate the new tile
        var newTile = new Tile(tilesIcons[randomIndex].name, tilesIcons[randomIndex], coordinates);
        
        // Make sure that we reference it in our GameManager dictionary
        GameManager.Instance.AddNewTileToDictionary(newTile);

        CheckUsedIcons(ref usedIcons, tilesIcons, randomIndex);
        return newTile;
    }

    /// <summary>
    /// Check if the last icon used for our tile was already used 1 or 2 times.
    /// This make sure that we do work by pairs.
    /// </summary>
    /// <param name="usedIcons">Reference all previously used icons on the board</param>
    /// <param name="tilesIcon">The array of all icons we need to place on the board</param>
    /// <param name="randomIndex">The index used for our last tile</param>
    private static void CheckUsedIcons(ref Dictionary<int, int> usedIcons, Texture2D[] tilesIcon, int randomIndex)
    {
        // The icon was already used before
        if (usedIcons.ContainsKey(randomIndex))
        {
            usedIcons[randomIndex]++;

            // if we used all possible tiles to present
            if (usedIcons.Count == tilesIcon.Length)
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

    /// <summary>
    /// Provide an enumerator for all neighbors around this tile
    /// </summary>
    public IEnumerable<Tile> Neighbours
    {
        get
        {
            var tileMatrix = GameManager.Instance.TilesMatrix;

            int neighborValue = this.Coordinates.x - 1;
            if (neighborValue > 0)
                yield return tileMatrix[neighborValue][Coordinates.y];

            neighborValue = this.Coordinates.x + 1;
            if (neighborValue < tileMatrix.Length)
                yield return tileMatrix[neighborValue][Coordinates.y];

            neighborValue = this.Coordinates.y - 1;
            if (neighborValue > 0)
                yield return tileMatrix[Coordinates.x][neighborValue];

            neighborValue = this.Coordinates.y + 1;
            if (neighborValue < tileMatrix[0].Length)
                yield return tileMatrix[Coordinates.x][neighborValue];
        }
    }
}
