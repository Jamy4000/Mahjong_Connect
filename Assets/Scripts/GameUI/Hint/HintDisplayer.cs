using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Display an hint on the screen as a line renderer progressively appearing.
/// </summary>
public class HintDisplayer : MonoBehaviour
{
    /// <summary>
    /// The prefab for our line renderer
    /// </summary>
    [SerializeField] private GameObject _lineRenderer;

    /// <summary>
    /// The button to give an int to the user
    /// </summary>
    private UnityEngine.UI.Button _hintButton;

    /// <summary>
    /// The list of line renderer that were instantiated based on a tuple of tiles (from / to)
    /// </summary>
    private Dictionary<Tuple<Tile, Tile>, GameObject> _instantiatedLr = new Dictionary<Tuple<Tile, Tile>, GameObject>();

    private void Awake()
    {
        OnUserValideAnswer.Listeners += RemovePath;
        _hintButton = GetComponent<UnityEngine.UI.Button>();
    }

    private void OnDestroy()
    {
        OnUserValideAnswer.Listeners -= RemovePath;
    }

    /// <summary>
    /// Check if a hint was displayed for the two tiles that were removed. If so, delete the line renderer
    /// </summary>
    /// <param name="info"></param>
    private void RemovePath(OnUserValideAnswer info)
    {
        if (HintsIsDisplayed(info.FirstTile, info.SecondTile, out Tuple<Tile, Tile> tuple))
        {
            Destroy(_instantiatedLr[tuple]);
            _instantiatedLr.Remove(tuple);
            _hintButton.interactable = ANewPathExist(out Path<Tile> _);
            return;
        }
    }

    /// <summary>
    /// Check if an hint was already displayed for two tiles.
    /// </summary>
    /// <param name="firstTile">The tile from which the line should start</param>
    /// <param name="secondTile">The tile to which the line should end</param>
    /// <param name="hint">The tuple regrouping those two tiles in the dictionary</param>
    /// <returns></returns>
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

    /// <summary>
    /// Callback for the Hint button in the scene.
    /// </summary>
    public void DisplayPath()
    {
        if (ANewPathExist(out Path<Tile> path)) 
        {
            InstantiateLine(path);
            _hintButton.interactable = ANewPathExist(out Path<Tile> _);
        }
    }

    /// <summary>
    /// Check if at least one new possible path (that is still not shown) exist on the screen
    /// </summary>
    /// <param name="path">one available path, if possible</param>
    /// <returns>false if no new, not-shown path is available</returns>
    private bool ANewPathExist(out Path<Tile> path)
    {
        foreach (var neighbors in GameManager.Instance.SameTilesDictionary.Values)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                for (int j = 0; j < neighbors.Count; j++)
                {
                    if (i == j || HintsIsDisplayed(neighbors[i], neighbors[j], out Tuple<Tile, Tile> _))
                        continue;

                    path = PathFinder.FetchPath(neighbors[i], neighbors[j], false);
                    if (path != null)
                        return true;
                }
            }
        }

        path = null;
        return false;
    }

    /// <summary>
    /// Instantiate a new line renderer to show a hint to the user
    /// </summary>
    /// <param name="path">The path linking the two tiles for the hint</param>
    private void InstantiateLine(Path<Tile> path)
    {
        var newLrObject = Instantiate(_lineRenderer);
        _instantiatedLr.Add(new Tuple<Tile, Tile>(path.LastStep, path.Last()), newLrObject);
        newLrObject.GetComponent<LineRendererAnimator>().Init(path);
    }
}
