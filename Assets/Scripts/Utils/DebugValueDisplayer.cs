using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugValueDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _lineRenderer;
    [SerializeField] private TMPro.TextMeshProUGUI _positionValue;
    [SerializeField] private TMPro.TextMeshProUGUI _idValue;

    private GameManager _gameManager;
    private List<GameObject> _instantiatedLr = new List<GameObject>();

    private void Awake() 
    {
        OnTileHovered.Listeners += DisplayTileInfoAndPaths;
        OnTileUnhovered.Listeners += HideInfo;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnDestroy()
    {
        OnTileHovered.Listeners -= DisplayTileInfoAndPaths;
        OnTileUnhovered.Listeners -= HideInfo;
    }

    private void DisplayTileInfoAndPaths(OnTileHovered info)
    {
        DisplayPath(info.HoveredTile);
        _positionValue.text = info.HoveredTile.Coordinates.ToString();
        _idValue.text = info.HoveredTile.ID;
    }

    private void HideInfo(OnTileUnhovered _)
    {
        foreach (var lr in _instantiatedLr)
        {
            Destroy(lr);
        }
        _instantiatedLr.Clear();
    }

    private void DisplayPath(Tile tile)
    {
        if (tile == null)
            tile = GetComponent<TileClickHandler>().ThisTile;

        foreach (var neighbors in _gameManager.SameTilesDictionary)
        {
            if (neighbors.Key == tile.ID)
            {
                foreach (var neighbor in neighbors.Value)
                {
                    var path = PathFinder.ValidPathExist(tile, neighbor);
                    if (path != null)
                        ShowLine(path);
                }
            }
        }
    }

    private void ShowLine(Path<Tile> path)
    {
        var newLrObject = Instantiate(_lineRenderer);
        _instantiatedLr.Add(newLrObject);

        var newLr = newLrObject.GetComponent<LineRenderer>();
        List<Vector3> positions = new List<Vector3>();
        foreach (var tile in path)
        {
            positions.Add(tile.GameObjectRepresentation.transform.position);
        }
        newLr.positionCount = positions.Count;
        newLr.SetPositions(positions.ToArray());
    }
}
