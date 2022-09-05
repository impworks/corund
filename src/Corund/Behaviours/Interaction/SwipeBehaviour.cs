using System;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Behaviours.Interaction;

/// <summary>
/// A callback to be executed when a swipe has been detected.
/// </summary>
public class SwipeBehaviour : BehaviourBase
{
    #region Constants

    /// <summary>
    /// Direction detector strictness.
    /// 1 = absolutely strict.
    /// 0 = any direction fits.
    /// </summary>
    private const float STRICTNESS = 0.5f;

    /// <summary>
    /// Maximum duration after which the swipe candidate is discarded.
    /// </summary>
    private const float MAX_DURATION = 0.3f;

    /// <summary>
    /// Minimum distance between swipe start and end.
    /// </summary>
    private const float MIN_DISTANCE = 30;

    #endregion

    #region Constructor

    public SwipeBehaviour(KnownDirection direction, Action<SwipeInfo> callback)
    {
        _callback = callback;
        _direction = direction;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Function to execute when the swipe has been registered.
    /// </summary>
    private readonly Action<SwipeInfo> _callback;

    /// <summary>
    /// Allowed direction(s) to detect.
    /// </summary>
    private readonly KnownDirection _direction;

    /// <summary>
    /// Origin of the swipe gesture.
    /// </summary>
    private TouchLocation? _start;

    /// <summary>
    /// Duration of the swipe.
    /// </summary>
    private float _duration;

    #endregion

    #region Methods

    public override void Bind(DynamicObject obj)
    {
        if (obj is not InteractiveObject)
            throw new ArgumentException("Object must derive from InteractiveObject to handle swipes.");
    }

    public override void UpdateObjectState(DynamicObject obj)
    {
        if (_start == null)
        {
            // find swipe
            var touch = ((InteractiveObject)obj).TryGetTouch();
            if (touch?.State == TouchLocationState.Pressed)
            {
                _start = touch;
                _duration = 0;
            }
        }
        else
        {
            // update
            _duration += GameEngine.Delta;
            if (_duration > MAX_DURATION)
            {
                _start = null;
                return;
            }

            var touch = GameEngine.Touch.FindById(_start.Value.Id);
            if (touch == null || touch?.State == TouchLocationState.Moved)
                return;

            if (touch?.State == TouchLocationState.Released)
            {
                var vec = touch.Value.Position - _start.Value.Position;
                var dist = vec.Length() / _duration;
                if (dist > MIN_DISTANCE)
                {
                    var dir = vec.GetDirection(STRICTNESS);
                    if (dir != null && _direction.HasFlag(dir.Value))
                    {
                        _callback(
                            new SwipeInfo(
                                _start.Value.Position,
                                touch.Value.Position - _start.Value.Position,
                                _duration
                            )
                        );
                    }
                }
            }

            _start = null;
        }
    }

    #endregion
}