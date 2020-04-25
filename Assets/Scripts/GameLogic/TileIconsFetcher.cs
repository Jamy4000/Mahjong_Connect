using UnityEngine;
using System.Collections.Generic;

public static class TileIconsFetcher
{
    private static Texture2D[] allIcons;

    public static Texture2D[] FetchIcons()
    {
        allIcons = Resources.LoadAll<Texture2D>("TileIcons");
        return GenerateTextureArray(GameManager.Instance.TileAmount, GameManager.Instance.TileAmount / 10);
    }

    public static Texture2D[] GenerateTextureArray(int fullTileAmount, int minIconAmount)
    {
        // Generate random amount of possible tiles between fullTileAmount / 10 and fullTileAmount / 2. 
        // This will increase the possibility to have multiple possibilities of tile pairs
        var tilesIcons = new Texture2D[Random.Range(minIconAmount, fullTileAmount / 2)];
        var usedIndex = new List<int>(allIcons.Length > fullTileAmount ? fullTileAmount : allIcons.Length);

        for (int i = 0; i < tilesIcons.Length; i++)
        {
            int randomIndex = Random.Range(0, allIcons.Length);

            while (usedIndex.Contains(randomIndex))
                randomIndex = Random.Range(0, allIcons.Length);

            usedIndex.Add(randomIndex);

            if (usedIndex.Count == allIcons.Length)
                usedIndex.Clear();

            tilesIcons[i] = allIcons[randomIndex];
        }

        return tilesIcons;
    }
}
