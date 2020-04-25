using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HintDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _lineRenderer;

    private Dictionary<Tuple<Tile, Tile>, GameObject> _instantiatedLr = new Dictionary<Tuple<Tile, Tile>, GameObject>();

    private void Awake()
    {
        OnUserValideAnswer.Listeners += RemovePath;
    }

    private void OnDestroy()
    {
        OnUserValideAnswer.Listeners -= RemovePath;
    }

    private void RemovePath(OnUserValideAnswer info)
    {
        if (HintsIsDisplayed(info.FirstTile, info.SecondTile, out Tuple<Tile, Tile> tuple))
        {
            Destroy(_instantiatedLr[tuple]);
            _instantiatedLr.Remove(tuple);
            return;
        }
    }

    private bool HintsIsDisplayed(Tile firstTile, Tile secondTile, out Tuple<Tile, Tile> hint)
    {
        foreach (var tuple in _instantiatedLr.Keys)
        {
            if ((tuple.Item1 == firstTile && tuple.Item2 == secondTile) ||
                (tuple.Item1 == secondTile && tuple.Item2 == firstTile))
            {
                hint = tuple;
                return true;
            }
        }

        hint = null;
        return false;
    }

    public void DisplayPath()
    {
        foreach (var neighbors in GameManager.Instance.SameTilesDictionary.Values)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                for (int j = 0; j < neighbors.Count; j++)
                {
                    if (i == j || HintsIsDisplayed(neighbors[i], neighbors[j], out Tuple<Tile, Tile> _))
                        continue;

                    var path = PathFinder.FetchPath(neighbors[i], neighbors[j], false);
                    if (path != null)
                    {
                        Debug.Log("TODO ANIMATION");
                        ShowLine(path);
                        return;
                    }
                }
            }
        }
    }

    private void ShowLine(Path<Tile> path)
    {
        var newLrObject = Instantiate(_lineRenderer);
        _instantiatedLr.Add(new Tuple<Tile, Tile>(path.LastStep, path.Last()), newLrObject);

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
