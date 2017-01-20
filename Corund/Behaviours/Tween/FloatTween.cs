using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tween
{
    /// <summary>
    /// Animation for float values.
    /// </summary>
    public class FloatTween<TObject>: PropertyTweenBase<TObject, float>
        where TObject: DynamicObject
    {
        public FloatTween(IPropertyDescriptor<TObject, float> descriptor, float targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        protected override float getValue()
        {
            return getFloat(_initialValue, _targetValue);
        }
    }
}
