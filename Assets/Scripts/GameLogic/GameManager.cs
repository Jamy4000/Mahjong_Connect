using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance 
    {
        get;
        private set;
    }

    public Level CurrentLevel;
    public Tile[][] TilesMatrix;
    public Dictionary<string, List<Vector2>> SameTilesCoordinates = new Dictionary<string, List<Vector2>>();
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
        if (SameTilesCoordinates.ContainsKey(info.FirstTile.ID))
        {
            SameTilesCoordinates[info.FirstTile.ID].Remove(info.FirstTile.Coordinates);
            SameTilesCoordinates[info.FirstTile.ID].Remove(info.SecondTile.Coordinates);
        }
        else 
        {
            Debug.LogError("TODO Change error message, but this shouldn't happen");
        }
    }

    private void ResetCurrentClickedTile(OnUserError info)
    {
        CurrentlyClickedTile = null;
    }

    internal void AddNewTileToDicitonary(Tile newTile)
    {
        if (SameTilesCoordinates.ContainsKey(newTile.ID))
            SameTilesCoordinates[newTile.ID].Add(newTile.Coordinates);
        else
            SameTilesCoordinates.Add(newTile.ID, new List<Vector2> { newTile.Coordinates });
    }
}
