using Corund.Behaviours.Tweening;
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
        #region Animated properties

        /// <summary>
        /// Animates a float property.
        /// </summary>
        public static void Animate<T>(this T obj, IPropertyDescriptor<T, float> descriptor, float target, float duration, InterpolationMethod interpolation = null)
            where T: DynamicObject
        {
            obj.Behaviours.Add(new FloatAnimation<T>(descriptor, target, duration, interpolation));
        }

        /// <summary>
        /// Animates a vector property.
        /// </summary>
        public static void Animate<T>(this T obj, IPropertyDescriptor<T, Vector2> descriptor, Vector2 target, float duration, InterpolationMethod interpolation = null)
            where T : DynamicObject
        {
            obj.Behaviours.Add(new Vector2Animation<T>(descriptor, target, duration, interpolation));
        }

        /// <summary>
        /// Animates a color property.
        /// </summary>
        public static void Animate<T>(this T obj, IPropertyDescriptor<T, Color> descriptor, Color target, float duration, InterpolationMethod interpolation = null)
            where T : DynamicObject
        {
            obj.Behaviours.Add(new ColorAnimation<T>(descriptor, target, duration, interpolation));
        }

        /// <summary>
        /// Stops animating a property.
        /// </summary>
        public static void StopAnimating<T, TProperty>(this T obj, IPropertyDescriptor<T, TProperty> descriptor, bool skipToFinalValue = true)
            where T: DynamicObject
        {
            var name = descriptor.Name;
            foreach (var behaviour in obj.Behaviours)
            {
                var ptyAnimator = behaviour as IPropertyAnimation;
                if (ptyAnimator == null || ptyAnimator.PropertyName != name)
                    continue;

                ptyAnimator.StopAnimation(obj, skipToFinalValue);
            }
        }

        /// <summary>
        /// Stops animating all properties.
        /// </summary>
        public static void StopAnimatingAll<T>(this T obj, bool skipToFinalValue = true)
             where T : DynamicObject
        {
            foreach (var behaviour in obj.Behaviours)
            {
                var ptyAnimator = behaviour as IPropertyAnimation;
                ptyAnimator?.StopAnimation(obj, skipToFinalValue);
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
            obj.StopAnimating(AnimatedProperty.Position);
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
            obj.Animate(AnimatedProperty.Position, point, time, interpolation);
        }

        #endregion
    }
}
