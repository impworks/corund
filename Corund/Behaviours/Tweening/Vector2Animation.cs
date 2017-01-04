using System;
using Corund.Tools.Interpolation;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Animation for vector2 values.
    /// </summary>
    public class Vector2Animation: PropertyAnimationBase<Vector2>
    {
        public Vector2Animation(Vector2 initial, Vector2 target, float duration, Action<Vector2> setter, InterpolationMethod interpolation = null)
            : base(initial, target, duration, setter, interpolation)
        { }

        protected override Vector2 getValue()
        {
            return new Vector2(
                getFloat(_initialValue.X, _targetValue.X),
                getFloat(_initialValue.Y, _targetValue.Y)
            );  
        }
    }
}
