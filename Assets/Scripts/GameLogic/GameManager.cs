using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Store the board and some important piece of info for our game, and handle soe base logic as well
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager Instance 
    {
        get;
        private set;
    }

    /// <summary>
    /// The level we're currently in. Set as static as it's used in both scenes.
    /// </summary>
    public static Level CurrentLevel;

    /// <summary>
    /// The board of our game
    /// </summary>
    public Tile[][] TilesMatrix;

    /// <summary>
    /// Dictionary containing the ID of a tile as key, and a list of tiles that have this ID on the board.
    /// </summary>
    public Dictionary<string, List<Tile>> SameTilesDictionary = new Dictionary<string, List<Tile>>();

    /// <summary>
    /// The tile that is currently selected.
    /// </summary>
    public Tile CurrentlyClickedTile;

    /// <summary>
    /// The current score for this level
    /// </summary>
    public int CurrentScore
    {
        get;
        private set;
    }

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
        OnUserError.Listeners += SetCurrentScore;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            OnUserValideAnswer.Listeners -= UserGaveValidAnswerCallback;
            OnUserError.Listeners -= SetCurrentScore;
        }
    }

    /// <summary>
    /// callback when two tiles have been correctly matched
    /// </summary>
    /// <param name="info"></param>
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

        CheckRemainingPairs();
    }

    /// <summary>
    /// Check if there's still some pairs to do. if not, user has won / lost.
    /// </summary>
    private void CheckRemainingPairs()
    {
        if (SameTilesDictionary.Count == 0)
            new OnGameEnded(true);
        else
            CheckIfGameIsLost();
    }

    /// <summary>
    /// Check if there's still some avilable paths on the board
    /// </summary>
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

    /// <summary>
    /// Set the current score if the user make a mistake
    /// </summary>
    private void SetCurrentScore(OnUserError _)
    {
        CurrentScore = Mathf.Clamp(CurrentScore - 10, 0, CurrentScore);
    }

    /// <summary>
    /// Add a new tile to the SameTilesDictionary when its created
    /// </summary>
    /// <param name="newTile">The tile that was created</param>
    public void AddNewTileToDictionary(Tile newTile)
    {
        if (SameTilesDictionary.ContainsKey(newTile.ID))
            SameTilesDictionary[newTile.ID].Add(newTile);
        else
            SameTilesDictionary.Add(newTile.ID, new List<Tile> { newTile });
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
