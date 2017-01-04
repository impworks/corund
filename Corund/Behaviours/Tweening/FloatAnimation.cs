using System;
using Corund.Tools.Interpolation;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Animation for float values.
    /// </summary>
    public class FloatAnimation: PropertyAnimationBase<float>
    {
        public FloatAnimation(float initial, float target, float duration, Action<float> setter, InterpolationMethod interpolation = null)
            : base(initial, target, duration, setter, interpolation)
        { }

        protected override float getValue()
        {
            return getFloat(_initialValue, _targetValue);
        }
    }
}
