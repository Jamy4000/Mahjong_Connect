using UnityEngine;
using System.Collections.Generic;

public static class TileIconsFetcher
{
    public static Texture2D[] FetchIcons(int fullTileAmount)
    {
        var allIcons = Resources.LoadAll<Texture2D>("TileIcons");
        var tilesIcons = new Texture2D[fullTileAmount / 2];
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
