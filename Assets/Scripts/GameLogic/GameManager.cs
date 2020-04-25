using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance 
    {
        get;
        private set;
    }

    /// <summary>
    /// The level we're currently in. Set as static as it's used in both scenes.
    /// </summary>
    public static Level CurrentLevel;

    public Tile[][] TilesMatrix;
    public Dictionary<string, List<Tile>> SameTilesDictionary = new Dictionary<string, List<Tile>>();
    public Tile CurrentlyClickedTile;

    public int CurrentScore
    {
        get;
        private set;
    }

    public int TileAmount;
    private int _correctAnswerAmount = 0;

    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("A game manager already exist. Destroying this one.");
            Destroy(this);
            return;
        }

        Instance = this;
        OnUserValideAnswer.Listeners += UserGaveValidAnswerCallback;
        OnUserError.Listeners += ResetCurrentClickedTile;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;

            OnUserValideAnswer.Listeners -= UserGaveValidAnswerCallback;
            OnUserError.Listeners -= ResetCurrentClickedTile;
        }
    }

    private void UserGaveValidAnswerCallback(OnUserValideAnswer info)
    {
        CurrentlyClickedTile = null;

        // Update Same Tiles Dictionary
        SameTilesDictionary[info.FirstTile.ID].Remove(info.FirstTile);
        SameTilesDictionary[info.FirstTile.ID].Remove(info.SecondTile);
        if (SameTilesDictionary[info.FirstTile.ID].Count == 0)
            SameTilesDictionary.Remove(info.FirstTile.ID);

        // Update Current Score
        CurrentScore += 15;
        _correctAnswerAmount++;

        CheckRemainingPairs();
    }

    private void CheckRemainingPairs()
    {
        if (SameTilesDictionary.Count == 0)
            new OnGameEnded(true);
        else
            CheckIfGameIsLost();
    }

    private void CheckIfGameIsLost()
    {
        bool hasLost = true;

        foreach (var sameTiles in SameTilesDictionary.Values)
        {
            for (int i = 0; i < sameTiles.Count; i++)
            {
                for (int j = 0; j < sameTiles.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (PathFinder.FetchPath(sameTiles[i], sameTiles[j], false) != null)
                    {
                        hasLost = false;
                        break;
                    }
                }

                if (!hasLost)
                    break;
            }

            if (!hasLost)
                break;
        }

        if (hasLost)
            new OnGameEnded(false);
    }

    private void ResetCurrentClickedTile(OnUserError info)
    {
        CurrentlyClickedTile = null;
        CurrentScore = Mathf.Clamp(CurrentScore - 10, 0, CurrentScore);
    }

    internal void AddNewTileToDictionary(Tile newTile)
    {
        if (SameTilesDictionary.ContainsKey(newTile.ID))
            SameTilesDictionary[newTile.ID].Add(newTile);
        else
            SameTilesDictionary.Add(newTile.ID, new List<Tile> { newTile });
    }
}
