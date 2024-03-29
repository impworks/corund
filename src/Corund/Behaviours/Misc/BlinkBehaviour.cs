﻿using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Misc;

/// <summary>
/// Blink the object via opacity during a specified timespan.
/// </summary>
public class BlinkBehaviour: IBehaviour, IBindableBehaviour, IEffect
{
    #region Constructor

    public BlinkBehaviour(int blinkCount, float duration)
    {
        _blinkCount = blinkCount;
        Duration = duration;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Number of times the object must hide and appear.
    /// </summary>
    private readonly int _blinkCount;

    /// <summary>
    /// Saved opacity value.
    /// </summary>
    private float _originalOpacity;

    /// <summary>
    /// Time elapsed since the effect start.
    /// </summary>
    private float _elapsedTime;

    /// <summary>
    /// Timer key for the next event.
    /// </summary>
    private TimelineManager.TimelineRecord _timerKey;

    /// <summary>
    /// Number of times the object has blinked already.
    /// </summary>
    private int _elapsedBlinks;

    #endregion

    #region Properties

    /// <summary>
    /// Duration of the effect.
    /// </summary>
    public float Duration { get; }

    /// <summary>
    /// Current progress of the effect (0..1).
    /// </summary>
    public float? Progress => _elapsedTime/Duration;

    #endregion

    #region Methods

    /// <summary>
    /// Applies animation.
    /// </summary>
    public void Bind(DynamicObject obj)
    {
        _originalOpacity = obj.Opacity;
        var span = Duration / _blinkCount;

        void Blink()
        {
            obj.Tween(Property.Opacity, 0, span / 2, null, true);
            _elapsedBlinks++;
            _timerKey = _elapsedBlinks < _blinkCount
                ? GameEngine.Current.Timeline.Add(span, Blink)
                : null;
        }

        Blink();
    }

    /// <summary>
    /// Updates effect state.
    /// </summary>
    public void UpdateObjectState(DynamicObject obj)
    {
        _elapsedTime += GameEngine.Delta;
    }

    /// <summary>
    /// Cancels all pending timer keyframes and sets value to original.
    /// </summary>
    public void Unbind(DynamicObject obj)
    {
        GameEngine.Current.Timeline.Remove(_timerKey);

        obj.StopTweening(Property.Opacity);
        obj.Opacity = _originalOpacity;
    }

    #endregion
}