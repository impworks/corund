using System;
using Corund.Behaviours;
using Corund.Behaviours.Jitter;
using Corund.Behaviours.Tween;
using Corund.Engine;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// Various helper methods for using a DynamicObject.
    /// </summary>
    public static class DynamicObjectHelper
    {
        #region Tweened properties

        /// <summary>
        /// Tweens a float property.
        /// </summary>
        public static void Tween<T>(this T obj, IPropertyDescriptor<T, float> descriptor, float target, float duration, InterpolationMethod interpolation = null, bool tweenBack = false, bool loop = false)
            where T : DynamicObject
        {
            if (!tweenBack && !loop)
            {
                GameEngine.InvokeDeferred(() => obj.Behaviours.Add(new FloatTween<T>(descriptor, target, duration, interpolation)));
                return;
            }

            ApplyComplexTween(
                obj,
                descriptor,
                duration,
                loop,
                tweenBack,
                () => new FloatTween<T>(descriptor, target, duration, interpolation)
            );
        }

        /// <summary>
        /// Tweens a vector property.
        /// </summary>
        public static void Tween<T>(this T obj, IPropertyDescriptor<T, Vector2> descriptor, Vector2 target, float duration, InterpolationMethod interpolation = null, bool tweenBack = false, bool loop = false)
            where T : DynamicObject
        {
            if (!tweenBack && !loop)
            {
                GameEngine.InvokeDeferred(() => obj.Behaviours.Add(new Vector2Tween<T>(descriptor, target, duration, interpolation)));
                return;
            }

            ApplyComplexTween(
                obj,
                descriptor,
                duration,
                loop,
                tweenBack,
                () => new Vector2Tween<T>(descriptor, target, duration, interpolation)
            );
        }

        /// <summary>
        /// Tweens a color property.
        /// </summary>
        public static void Tween<T>(this T obj, IPropertyDescriptor<T, Color> descriptor, Color target, float duration, InterpolationMethod interpolation = null, bool tweenBack = false, bool loop = false)
            where T : DynamicObject
        {
            if (!tweenBack && !loop)
            {
                GameEngine.InvokeDeferred(() => obj.Behaviours.Add(new ColorTween<T>(descriptor, target, duration, interpolation)));
                return;
            }

            ApplyComplexTween(
                obj,
                descriptor,
                duration,
                loop,
                tweenBack,
                () => new ColorTween<T>(descriptor, target, duration, interpolation)
            );
        }

        /// <summary>
        /// Stops tweening a property.
        /// </summary>
        public static void StopTweening<T, TProperty>(this T obj, IPropertyDescriptor<T, TProperty> descriptor, bool skipToFinalValue = true)
            where T: DynamicObject
        {
            var name = descriptor.Name;
            foreach (var behaviour in obj.Behaviours)
            {
                var tween = behaviour as IPropertyTween;
                if (tween == null || tween.PropertyName != name)
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
        private static void ApplyComplexTween<TObject, TProperty, TTween>(
            TObject obj,
            IPropertyDescriptor<TObject, TProperty> descriptor,
            float duration,
            bool loop,
            bool tweenBack,
            Func<TTween> tweenFactory
        )
            where TObject: DynamicObject
            where TTween: BehaviourBase, IReversible<TTween>
        {
            var origValue = descriptor.Getter(obj);
            Action anim = null;

            anim = () =>
            {
                var tween = tweenFactory();
                GameEngine.InvokeDeferred(() => obj.Behaviours.Add(tween));

                if (tweenBack)
                {
                    GameEngine.Current.Timeline.Add(duration, () =>
                    {
                        var reverse = tween.Reverse();
                        GameEngine.InvokeDeferred(() => obj.Behaviours.Add(reverse));

                        if(loop)
                            GameEngine.Current.Timeline.Add(duration, anim);
                    });
                }
                else if (loop)
                {
                    GameEngine.Current.Timeline.Add(duration, () =>
                    {
                        descriptor.Setter(obj, origValue);

                        anim();
                    });
                }
            };

            anim();
        }

        #endregion

        #region Jitter

        /// <summary>
        /// Add jitter effect to a float property.
        /// </summary>
        public static void Jitter<T>(this T obj, IPropertyDescriptor<T, float> descriptor, float delay, float range, bool isRelative = false)
            where T : DynamicObject
        {
            GameEngine.InvokeDeferred(() => obj.Behaviours.Add(new FloatJitter<T>(descriptor, delay, range, isRelative)));
        }

        /// <summary>
        /// Add jitter effect to a vector property.
        /// </summary>
        public static void Jitter<T>(this T obj, IPropertyDescriptor<T, Vector2> descriptor, float delay, float xRange, float yRange, bool isRelative = false)
            where T : DynamicObject
        {
            GameEngine.InvokeDeferred(() => obj.Behaviours.Add(new Vector2Jitter<T>(descriptor, delay, xRange, yRange, isRelative)));
        }

        /// <summary>
        /// Add jitter effect to a color property.
        /// </summary>
        public static void Jitter<T>(this T obj, IPropertyDescriptor<T, Color> descriptor, float delay, Vector4 range, bool isRelative = false)
            where T : DynamicObject
        {
            GameEngine.InvokeDeferred(() => obj.Behaviours.Add(new ColorJitter<T>(descriptor, delay, range, isRelative)));
        }

        /// <summary>
        /// Stops jittering a property.
        /// </summary>
        public static void StopJittering<T, TProperty>(this T obj, IPropertyDescriptor<T, TProperty> descriptor)
            where T : DynamicObject
        {
            var name = descriptor.Name;
            foreach (var behaviour in obj.Behaviours)
            {
                var jitter = behaviour as IPropertyJitter;
                if (jitter == null || jitter.PropertyName != name)
                    continue;

                GameEngine.InvokeDeferred(() => obj.Behaviours.Remove(behaviour));
            }
        }

        /// <summary>
        /// Stops jittering all properties.
        /// </summary>
        public static void StopJitteringAll<T>(this T obj, bool skipToFinalValue = true)
             where T : DynamicObject
        {
            foreach (var behaviour in obj.Behaviours)
            {
                var jitter = behaviour as IPropertyJitter;
                if(jitter != null)
                    GameEngine.InvokeDeferred(() => obj.Behaviours.Remove(behaviour));
            }
        }

        #endregion

        #region Movement

        /// <summary>
        /// Stops the object's movements.
        /// </summary>
        public static void Stop(this DynamicObject obj)
        {
            obj.Momentum = Vector2.Zero;
            obj.Acceleration = 0;
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
        /// Moves to given point over the specified timespan.
        /// </summary>
        /// <param name="obj">Object to apply movement to.</param>
        /// <param name="point">Desired point (in local coordinates).</param>
        /// <param name="time">Desired duration of travel.</param>
        /// <param name="interpolation">Interpolation method to use.</param>
        public static void MoveTo(this DynamicObject obj, Vector2 point, float time, InterpolationMethod interpolation = null)
        {
            obj.Direction = obj.Position.AngleTo(point);
            obj.Tween(Property.Position, point, time, interpolation);
        }

        #endregion
    }
}
