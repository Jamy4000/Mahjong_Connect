using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Level CurrentLevel;

    public static GameManager Instance;

    public Tile[][] TilesMatrix;
    public Dictionary<string, List<Tile>> SameTilesDictionary = new Dictionary<string, List<Tile>>();
    public Tile CurrentlyClickedTile;

    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("A game manager already exist. Destroying this one.");
            Destroy(this);
            return;
        }

        Instance = this;
        OnUserValideAnswer.Listeners += ResetCurrentClickedTile;
        OnUserError.Listeners += ResetCurrentClickedTile;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;

            OnUserValideAnswer.Listeners -= ResetCurrentClickedTile;
            OnUserError.Listeners -= ResetCurrentClickedTile;
        }
    }

    private void ResetCurrentClickedTile(OnUserValideAnswer info)
    {
        CurrentlyClickedTile = null;
    }

    private void ResetCurrentClickedTile(OnUserError info)
    {
        CurrentlyClickedTile = null;
    }
}
