using System;
using System.Collections.Generic;
using Corund.Behaviours;
using Corund.Behaviours.Jitter;
using Corund.Behaviours.Movement;
using Corund.Behaviours.Tween;
using Corund.Engine;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers;

/// <summary>
/// Various helper methods for using a DynamicObject.
/// </summary>
public static class DynamicObjectHelper
{
    #region Tweened properties

    /// <summary>
    /// Tweens a float property.
    /// </summary>
    public static void Tween<TObject, TPropBase>(this TObject obj, IPropertyDescriptor<TPropBase, float> descriptor, float target, float duration, InterpolationMethod interpolation = null, bool tweenBack = false, bool loop = false)
        where TObject : DynamicObject, TPropBase
    {
        ApplyTween(
            obj,
            descriptor,
            duration,
            loop,
            tweenBack,
            () => new FloatTween<TObject, TPropBase>(descriptor, target, duration, interpolation)
        );
    }

    /// <summary>
    /// Tweens a vector property.
    /// </summary>
    public static void Tween<TObject, TPropBase>(this TObject obj, IPropertyDescriptor<TPropBase, Vector2> descriptor, Vector2 target, float duration, InterpolationMethod interpolation = null, bool tweenBack = false, bool loop = false)
        where TObject : DynamicObject, TPropBase
    {
        ApplyTween(
            obj,
            descriptor,
            duration,
            loop,
            tweenBack,
            () => new Vector2Tween<TObject, TPropBase>(descriptor, target, duration, interpolation)
        );
    }

    /// <summary>
    /// Tweens a color property.
    /// </summary>
    public static void Tween<TObject, TPropBase>(this TObject obj, IPropertyDescriptor<TPropBase, Color> descriptor, Color target, float duration, InterpolationMethod interpolation = null, bool tweenBack = false, bool loop = false)
        where TObject : DynamicObject, TPropBase
    {
        ApplyTween(
            obj,
            descriptor,
            duration,
            loop,
            tweenBack,
            () => new ColorTween<TObject, TPropBase>(descriptor, target, duration, interpolation)
        );
    }

    /// <summary>
    /// Stops tweening a property.
    /// </summary>
    public static void StopTweening<TObject, TPropBase, TProperty>(this TObject obj, IPropertyDescriptor<TPropBase, TProperty> descriptor, bool skipToFinalValue = true)
        where TObject: DynamicObject, TPropBase
    {
        var name = descriptor.Name;
        foreach (var behaviour in obj.Behaviours)
        {
            if (behaviour is not IPropertyTween tween || tween.PropertyName != name)
                continue;

            tween.StopTween(obj, skipToFinalValue);
        }
    }

    /// <summary>
    /// Stops tweening all properties.
    /// </summary>
    public static void StopTweeningAll<T>(this T obj, bool skipToFinalValue = true)
        where T : DynamicObject
    {
        foreach (var behaviour in obj.Behaviours)
        {
            var tween = behaviour as IPropertyTween;
            tween?.StopTween(obj, skipToFinalValue);
        }
    }

    /// <summary>
    /// Applies a complex tweening effect with optional tween reverse and looping.
    /// </summary>
    /// <param name="obj">Object to animate.</param>
    /// <param name="descriptor">Tween property descriptor.</param>
    /// <param name="duration">Effect duration.</param>
    /// <param name="loop">Flag indicating that animation must be repeated until it is cancelled.</param>
    /// <param name="tweenBack">Flag indicating that after tweening to target value, property must be tweened back (before stopping or looping).</param>
    /// <param name="tweenFactory">Tween factory.</param>
    private static void ApplyTween<TObject, TPropBase, TProperty, TTween>(
        TObject obj,
        IPropertyDescriptor<TPropBase, TProperty> descriptor,
        float duration,
        bool loop,
        bool tweenBack,
        Func<TTween> tweenFactory
    )
        where TObject: DynamicObject, TPropBase
        where TTween: IBehaviour, IReversible<TTween>
    {
        if (!tweenBack && !loop)
        {
            AddBehaviour(obj, tweenFactory());
            return;
        }

        var origValue = descriptor.Getter(obj);

        void ApplyTweenInternal()
        {
            var tween = tweenFactory();
            AddBehaviour(obj, tween);

            if (tweenBack)
            {
                GameEngine.Current.Timeline.Add(
                    duration,
                    () =>
                    {
                        AddBehaviour(obj, tween.Reverse());

                        if (loop)
                            GameEngine.Current.Timeline.Add(duration, ApplyTweenInternal);
                    }
                );
            }
            else if (loop)
            {
                GameEngine.Current.Timeline.Add(
                    duration,
                    () =>
                    {
                        descriptor.Setter(obj, origValue);

                        ApplyTweenInternal();
                    }
                );
            }
        }

        ApplyTweenInternal();
    }

    #endregion

    #region Jitter

    /// <summary>
    /// Add jitter effect to a float property.
    /// </summary>
    public static void Jitter<TObject, TPropBase>(this TObject obj, IPropertyDescriptor<TPropBase, float> descriptor, float delay, float range, bool isRelative = false)
        where TObject : DynamicObject, TPropBase
    {
        AddBehaviour(obj, new FloatJitter<TObject, TPropBase>(descriptor, delay, range, isRelative));
    }

    /// <summary>
    /// Add jitter effect to a vector property.
    /// </summary>
    public static void Jitter<TObject, TPropBase>(this TObject obj, IPropertyDescriptor<TPropBase, Vector2> descriptor, float delay, float xRange, float yRange, bool isRelative = false)
        where TObject : DynamicObject, TPropBase
    {
        AddBehaviour(obj, new Vector2Jitter<TObject, TPropBase>(descriptor, delay, new Vector2(xRange, yRange), isRelative));
    }

    /// <summary>
    /// Add jitter effect to a color property.
    /// </summary>
    public static void Jitter<TObject, TPropBase>(this TObject obj, IPropertyDescriptor<TPropBase, Color> descriptor, float delay, Vector4 range, bool isRelative = false)
        where TObject : DynamicObject, TPropBase
    {
        AddBehaviour(obj, new ColorJitter<TObject, TPropBase>(descriptor, delay, range, isRelative));
    }

    /// <summary>
    /// Stops jittering a property.
    /// </summary>
    public static void StopJittering<TObject, TPropBase, TProperty>(this TObject obj, IPropertyDescriptor<TPropBase, TProperty> descriptor)
        where TObject : DynamicObject, TPropBase
    {
        var name = descriptor.Name;

        foreach (var behaviour in obj.Behaviours)
            if (behaviour is IPropertyJitter jitter && jitter.PropertyName == name)
                RemoveBehaviour(obj, behaviour);
    }

    /// <summary>
    /// Stops jittering all properties.
    /// </summary>
    public static void StopJitteringAll(this DynamicObject obj)
    {
        foreach (var behaviour in obj.Behaviours)
            if(behaviour is IPropertyJitter)
                RemoveBehaviour(obj, behaviour);
    }

    #endregion

    #region Movement

    /// <summary>
    /// Stops the object's movements.
    /// </summary>
    public static void Stop(this DynamicObject obj)
    {
        obj.Momentum = Vector2.Zero;
        obj.StopTweening(Property.Position);
    }

    /// <summary>
    /// Moves with given speed in the direction specified by the angle (absolute).
    /// </summary>
    public static void Move(this DynamicObject obj, float speed, float angle)
    {
        obj.Momentum = VectorHelper.FromLength(speed, angle);
    }

    /// <summary>
    /// Turns the movement direction to the other object.
    /// </summary>
    public static void MoveTowards(this DynamicObject obj, DynamicObject other)
    {
        var objPos = obj.GetTransformInfo(false).Position;
        var otherPos = other.GetTransformInfo(false).Position;
        obj.Momentum = VectorHelper.FromLength(obj.Momentum.Length(), objPos.AngleTo(otherPos));
    }

    /// <summary>
    /// Moves to given point over the specified timespan.
    /// </summary>
    /// <param name="obj">Object to apply movement to.</param>
    /// <param name="point">Desired point (in local coordinates).</param>
    /// <param name="time">Desired duration of travel.</param>
    /// <param name="interpolation">Interpolation method to use.</param>
    public static void MoveTo(this DynamicObject obj, Vector2 point, float time, InterpolationMethod interpolation = null)
    {
        obj.Tween(Property.Position, point, time, interpolation);
    }

    /// <summary>
    /// Initiates the movement along a jagged line of points with current speed.
    /// </summary>
    public static void MoveAlongLine(this DynamicObject obj, params Vector2[] points)
    {
        obj.Behaviours.Add(new LineMovementBehaviour(points));
    }

    /// <summary>
    /// Initiates the movement along a jagged line of points, completing the movement in given time.
    /// </summary>
    public static void MoveAlongLine(this DynamicObject obj, float duration, params Vector2[] points)
    {
        obj.Behaviours.Add(new LineMovementBehaviour(points, duration));
    }

    /// <summary>
    /// Initiates the movement along a jagged line of points with current speed.
    /// </summary>
    public static void MoveAlongLine(this DynamicObject obj, IEnumerable<Vector2> points)
    {
        obj.Behaviours.Add(new LineMovementBehaviour(points));
    }

    /// <summary>
    /// Initiates the movement along a jagged line of points, completing the movement in given time.
    /// </summary>
    public static void MoveAlongLine(this DynamicObject obj, float duration, IEnumerable<Vector2> points)
    {
        obj.Behaviours.Add(new LineMovementBehaviour(points, duration));
    }

    /// <summary>
    /// Initiates the movement along a curve with current speed.
    /// </summary>
    public static void MoveAlongCurve(this DynamicObject obj, params Vector2[] points)
    {
        obj.Behaviours.Add(new BezierMovementBehaviour(points));
    }

    /// <summary>
    /// Initiates the movement along a curve, completing the movement in given time.
    /// </summary>
    public static void MoveAlongCurve(this DynamicObject obj, float duration, params Vector2[] points)
    {
        obj.Behaviours.Add(new BezierMovementBehaviour(points, duration));
    }

    /// <summary>
    /// Initiates the movement along a curve with current speed.
    /// </summary>
    public static void MoveAlongCurve(this DynamicObject obj, IEnumerable<Vector2> points)
    {
        obj.Behaviours.Add(new BezierMovementBehaviour(points));
    }

    /// <summary>
    /// Initiates the movement along a curve, completing the movement in given time.
    /// </summary>
    public static void MoveAlongCurve(this DynamicObject obj, float duration, IEnumerable<Vector2> points)
    {
        obj.Behaviours.Add(new BezierMovementBehaviour(points, duration));
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Adds a behaviour in a deferred manner.
    /// </summary>
    private static void AddBehaviour(DynamicObject obj, IBehaviour behaviour)
    {
        GameEngine.Defer(() => obj.Behaviours.Add(behaviour));
    }

    /// <summary>
    /// Adds a behaviour in a deferred manner.
    /// </summary>
    private static void RemoveBehaviour(DynamicObject obj, IBehaviour behaviour)
    {
        GameEngine.Defer(() => obj.Behaviours.Remove(behaviour));
    }

    #endregion
}