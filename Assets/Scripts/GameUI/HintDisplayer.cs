using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HintDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _lineRenderer;
    [SerializeField] private float _lineAnimSpeed = 1.0f;

    private UnityEngine.UI.Button _hintButton;
    private float _lineAnimTime;
    private List<Vector3> _wayPoints = new List<Vector3>();
    private Vector3 _previousWayPoint;
    private Vector3 _currentWayPoint;
    private LineRenderer _currentLr;

    private Dictionary<Tuple<Tile, Tile>, GameObject> _instantiatedLr = new Dictionary<Tuple<Tile, Tile>, GameObject>();

    private void Awake()
    {
        OnUserValideAnswer.Listeners += RemovePath;
        _hintButton = GetComponent<UnityEngine.UI.Button>();
    }

    private void Update()
    {
        if (_currentLr != null)
        {
            // animate the position of the line
            _currentLr.SetPosition(_currentLr.positionCount-1, new Vector3(
                Mathf.Lerp(_previousWayPoint.x, _currentWayPoint.x, _lineAnimTime), 
                Mathf.Lerp(_previousWayPoint.y, _currentWayPoint.y, _lineAnimTime), _currentWayPoint.z));

            // .. and increase the t interpolater
            _lineAnimTime += _lineAnimSpeed * Time.deltaTime;

            if (_currentLr.GetPosition(_currentLr.positionCount-1) == _currentWayPoint)
            {
                _lineAnimTime = 0.0f;
                _wayPoints.Remove(_currentWayPoint);
                if (_wayPoints.Count > 0) 
                {
                    _currentLr.positionCount++;
                    _currentLr.SetPosition(_currentLr.positionCount - 1, _currentWayPoint);
                    _previousWayPoint = _currentWayPoint;
                    _currentWayPoint = _wayPoints[0];
                }
                else 
                {
                    _currentWayPoint = Vector3.zero;
                    _previousWayPoint = Vector3.zero;
                    _currentLr = null;
                    _hintButton.interactable = APathExist(out Path<Tile> _);
                }
            }
        }
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
            _hintButton.interactable = APathExist(out Path<Tile> _);
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
        if (APathExist(out Path<Tile> path)) 
        {
            _hintButton.interactable = false;
            ShowLine(path);
        }
    }

    private bool APathExist(out Path<Tile> path)
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

    private void ShowLine(Path<Tile> path)
    {
        var firstWayPoint = path.ElementAt(0).GameObjectRepresentation.transform.position;
        _currentWayPoint = path.ElementAt(1).GameObjectRepresentation.transform.position;
        _previousWayPoint = firstWayPoint;
        _lineAnimTime = 0.0f;

        Vector3 nextWP;
        foreach (var tile in path)
        {
            nextWP = tile.GameObjectRepresentation.transform.position;
            
            if (nextWP == firstWayPoint || nextWP == _currentWayPoint)
                continue;

            _wayPoints.Add(nextWP);
        }

        var newLrObject = Instantiate(_lineRenderer);
        _instantiatedLr.Add(new Tuple<Tile, Tile>(path.LastStep, path.Last()), newLrObject);

        _currentLr = newLrObject.GetComponent<LineRenderer>();
        _currentLr.positionCount = 2;
        _currentLr.SetPositions(new Vector3[2] { firstWayPoint, firstWayPoint } );
    }
}
