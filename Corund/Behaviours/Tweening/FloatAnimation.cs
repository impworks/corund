using Corund.Tools;
using Corund.Tools.Interpolation;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Animation for float values.
    /// </summary>
    public class FloatAnimation<TObject>: PropertyAnimationBase<TObject, float>
        where TObject: DynamicObject
    {
        public FloatAnimation(PropertyDescriptor<TObject, float> descriptor, float targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        protected override float getValue()
        {
            return getFloat(_initialValue, _targetValue);
        }
    }
}
