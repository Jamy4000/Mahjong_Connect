using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Fetch all available icons in the Resources/TileIcons folder
/// </summary>
public static class TileIconsFetcher
{
    /// <summary>
    /// The array for all icons
    /// </summary>
    private static Texture2D[] _allIcons;

    /// <summary>
    /// Fetch the textures in the Resources folder, and generate a new texture array using the method below
    /// </summary>
    /// <returns></returns>
    public static Texture2D[] FetchIcons()
    {
        // no need to load the texture every time
        if (_allIcons == null || _allIcons.Length == 0)
            _allIcons = Resources.LoadAll<Texture2D>("TileIcons");

        return GenerateTextureArray(GameManager.CurrentLevel.BaseTileAmount, GameManager.CurrentLevel.BaseTileAmount / 10);
    }

    /// <summary>
    /// Generate a random array of textures based on the available icons in the resources folder
    /// </summary>
    /// <param name="fullTileAmount">The amount of tiles on the board</param>
    /// <param name="minIconAmount"></param>
    /// <returns></returns>
    public static Texture2D[] GenerateTextureArray(int fullTileAmount, int minIconAmount)
    {
        // Generate random amount of possible tiles between fullTileAmount / 10 and fullTileAmount / 2. 
        // This will increase the possibility to have multiple possibilities of tile pairs
        var tilesIcons = new Texture2D[Random.Range(minIconAmount < fullTileAmount / 2 ? fullTileAmount / 4 : minIconAmount, fullTileAmount / 2)];
        var usedIndex = new List<int>(_allIcons.Length > fullTileAmount ? fullTileAmount : _allIcons.Length);

        for (int i = 0; i < tilesIcons.Length; i++)
        {
            int randomIndex = Random.Range(0, _allIcons.Length);

            while (usedIndex.Contains(randomIndex))
                randomIndex = Random.Range(0, _allIcons.Length);

            usedIndex.Add(randomIndex);

            if (usedIndex.Count == _allIcons.Length)
                usedIndex.Clear();

            tilesIcons[i] = _allIcons[randomIndex];
        }

        return tilesIcons;
    }
}
