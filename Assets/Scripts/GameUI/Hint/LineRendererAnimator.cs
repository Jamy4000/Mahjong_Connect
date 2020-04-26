using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Animate the line renderer, because it's pretty
/// </summary>
public class LineRendererAnimator : MonoBehaviour
{
    /// <summary>
    /// The speed of the animation to display the line renderer
    /// </summary>
    [SerializeField] private float _lineAnimSpeed = 15.0f;

    /// <summary>
    /// Used in lerp, Since when did the line anim started ?
    /// </summary>
    private float _lineAnimTime;

    /// <summary>
    /// The list of waypoints the line need to go through
    /// </summary>
    private List<Vector3> _wayPoints = new List<Vector3>();

    /// <summary>
    /// The previous waypoint of our line
    /// </summary>
    private Vector3 _previousWayPoint;

    /// <summary>
    /// The current way point to which we want to reach
    /// </summary>
    private Vector3 _currentWayPoint;

    /// <summary>
    /// The currently animated line renderer
    /// </summary>
    private LineRenderer _lineRenderer;

    /// <summary>
    /// The particle system attached to the line renderer, to help the user see where the hint is
    /// </summary>
    private Transform _lineRendererPS;


    private void Update()
    {
        // Next lines are here to animate the line renderer using a lerp
        var newPos = new Vector3(
            Mathf.Lerp(_previousWayPoint.x, _currentWayPoint.x, _lineAnimTime),
            Mathf.Lerp(_previousWayPoint.y, _currentWayPoint.y, _lineAnimTime), _currentWayPoint.z);

        // animate the position of the line
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, newPos);
        _lineRendererPS.position = newPos;

        // .. and increase the t interpolater
        _lineAnimTime += _lineAnimSpeed * Time.deltaTime;

        // check if we've reached our target waypoint
        if (_lineRenderer.GetPosition(_lineRenderer.positionCount - 1) == _currentWayPoint)
        {
            _lineAnimTime = 0.0f;
            _wayPoints.Remove(_currentWayPoint);

            // if there's still waypoints to go through
            if (_wayPoints.Count > 0)
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _currentWayPoint);
                _previousWayPoint = _currentWayPoint;
                _currentWayPoint = _wayPoints[0];
            }
            else
            {
                Destroy(_lineRendererPS.gameObject);
                Destroy(this);
            }
        }
    }

    /// <summary>
    /// Init all necessary value for the animation
    /// </summary>
    /// <param name="path">The path the line need to go from/to</param>
    public void Init(Path<Tile> path) 
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

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRendererPS = GetComponentInChildren<ParticleSystem>().transform;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPositions(new Vector3[2] { firstWayPoint, firstWayPoint });
    }
}
