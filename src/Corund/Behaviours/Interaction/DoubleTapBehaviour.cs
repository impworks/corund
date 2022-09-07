using System;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Behaviours.Interaction;

/// <summary>
/// Fires a callback when the object has been double-tapped.
/// </summary>
public class DoubleTapBehaviour: IBehaviour
{
    #region Constants

    /// <summary>
    /// The amount of time during which the double tap must be performed.
    /// </summary>
    private const float STREAK_TIME = 0.6f;

    #endregion

    #region Constructor

    public DoubleTapBehaviour(Action callback)
    {
        _callback = callback;
    }

    #endregion

    #region Fields

    /// <summary>
    /// The action to execute when the object has been double-tapped.
    /// </summary>
    private readonly Action _callback;

    /// <summary>
    /// Number of taps in the streak.
    /// </summary>
    private int _tapCount;

    /// <summary>
    /// Flag indicating that the current tap is still active.
    /// </summary>
    private bool _isTapActive;

    /// <summary>
    /// Time of the first tap.
    /// </summary>
    private float _streakStartTime;

    #endregion

    #region Methods

    public void UpdateObjectState(DynamicObject obj)
    {
        if (obj is not InteractiveObject iobj)
            throw new ArgumentException("Object must inherit InteractiveObject to be used with this behaviour!");

        var time = GameEngine.Current.Timeline.CurrentTime;
        if (_streakStartTime > 0 && time - _streakStartTime > STREAK_TIME)
        {
            Reset();
            return;
        }

        var touch = iobj.TryGetTouch();
        if (touch == null)
            return;

        var state = touch.Value.State;
        if (state == TouchLocationState.Pressed && !_isTapActive)
        {
            _tapCount++;
            _isTapActive = true;
            _streakStartTime = time;
        }
        else if (state == TouchLocationState.Released)
        {
            _isTapActive = false;
        }

        if (_tapCount == 2)
        {
            _callback();
            Reset();
        }
    }

    /// <summary>
    /// Reverts the internal counters to initial states.
    /// </summary>
    private void Reset()
    {
        _tapCount = 0;
        _streakStartTime = 0;
        _isTapActive = false;
    }

    #endregion
}