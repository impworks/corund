using System;
using System.Linq.Expressions;
using Corund.Behaviours.Tweening;
using Corund.Tools.Interpolation;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// Various helper methods for using a DynamicObject.
    /// </summary>
    public static class DynamicObjectHelper
    {
        /// <summary>
        /// Animates a float property.
        /// </summary>
        public static void Animate<T>(this T obj, Expression<Func<T, float>> property, float target, float duration, InterpolationMethod interpolation = null)
            where T: DynamicObject
        {
            obj.Behaviours.Add(new FloatAnimation<T>(property, target, duration, interpolation));
        }

        /// <summary>
        /// Animates a vector property.
        /// </summary>
        public static void Animate<T>(this T obj, Expression<Func<T, Vector2>> property, Vector2 target, float duration, InterpolationMethod interpolation = null)
            where T : DynamicObject
        {
            obj.Behaviours.Add(new Vector2Animation<T>(property, target, duration, interpolation));
        }

        /// <summary>
        /// Animates a color property.
        /// </summary>
        public static void Animate<T>(this T obj, Expression<Func<T, Color>> property, Color target, float duration, InterpolationMethod interpolation = null)
            where T : DynamicObject
        {
            obj.Behaviours.Add(new ColorAnimation<T>(property, target, duration, interpolation));
        }

        /// <summary>
        /// Stops animating a property.
        /// </summary>
        public static void StopAnimating<T>(this T obj, Expression<Func<T, Color>> property, bool skipToFinalValue = true)
            where T: DynamicObject
        {
            var name = PropertyHelper.GetPropertyName(property);
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
    }
}
