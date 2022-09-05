using System.Collections.Generic;
using System.Linq;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement;

/// <summary>
/// Base class for path-related movements behaviours.
/// </summary>
public abstract class PathBehaviourBase<TSegment>: BehaviourBase, IEffect
    where TSegment: IPathSegment
{
    #region Constructor

    protected PathBehaviourBase(IEnumerable<Vector2> points, float duration)
    {
        _points = points.ToList();
        _segments = GetSegments(_points);
        _length = _segments.Sum(x => x.Length);
        _speed = _length/duration;

        Duration = duration;
    }

    #endregion

    #region Fields

    /// <summary>
    /// List of points in this path.
    /// </summary>
    protected readonly IList<Vector2> _points;

    /// <summary>
    /// List of segments.
    /// </summary>
    private readonly IList<TSegment> _segments;

    /// <summary>
    /// ID of the current segment in the path.
    /// </summary>
    private int _currentSegmentId;

    /// <summary>
    /// Elapsed distance for current segment.
    /// </summary>
    private float _currentSegmentElapsedDistance;

    /// <summary>
    /// Total length of the path.
    /// </summary>
    private readonly float _length;

    /// <summary>
    /// Total elapsed distance for the path.
    /// </summary>
    private float _elapsedDistance;

    /// <summary>
    /// Speed at which the path must be traversed.
    /// </summary>
    private readonly float _speed;

    #endregion

    #region Properties

    /// <summary>
    /// Duration of the movement in seconds.
    /// </summary>
    public float Duration { get; }

    /// <summary>
    /// State of the movement (0 = just started, 1 = finished).
    /// </summary>
    public float? Progress => _elapsedDistance/_length;

    #endregion

    #region Methods

    /// <summary>
    /// Places the object to the start of the line.
    /// </summary>
    public override void Bind(DynamicObject obj)
    {
        obj.Position = _points[0];
    }

    /// <summary>
    /// Positions the object on the path.
    /// </summary>
    public override void UpdateObjectState(DynamicObject obj)
    {
        var oldPosition = GetCurrentPosition();

        var dist = _speed*GameEngine.Delta;
        _elapsedDistance += dist;
        _currentSegmentElapsedDistance += dist;

        // path is complete
        if (_elapsedDistance >= _length)
        {
            _elapsedDistance = _length;
            obj.Position = _points[^1];
            return;
        }

        // skip one or more segments
        while (_currentSegmentElapsedDistance >= _segments[_currentSegmentId].Length)
        {
            _currentSegmentElapsedDistance -= _segments[_currentSegmentId].Length;
            _currentSegmentId++;
        }

        // update 
        var newPosition = GetCurrentPosition();
        obj.Momentum = newPosition - oldPosition;
    }

    /// <summary>
    /// Returns the current position in the line.
    /// </summary>
    private Vector2 GetCurrentPosition()
    {
        var segment = _segments[_currentSegmentId];
        var state = _currentSegmentElapsedDistance / segment.Length;
        return segment.GetPosition(state);
    }

    /// <summary>
    /// Creates segments from points.
    /// </summary>
    protected abstract List<TSegment> GetSegments(IList<Vector2> points);

    #endregion
}