using Corund.Tools;
using Corund.Tools.Interpolation;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Animation for color properties.
    /// </summary>
    public class ColorAnimation<TObject> : PropertyAnimationBase<TObject, Color>
        where TObject : DynamicObject
    {
        public ColorAnimation(PropertyDescriptor<TObject, Color> descriptor, Color targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
        { }

        protected override Color getValue()
        {
            return new Color(
                getFloat(_initialValue.R, _targetValue.R),
                getFloat(_initialValue.G, _targetValue.G),
                getFloat(_initialValue.B, _targetValue.B),
                getFloat(_initialValue.A, _targetValue.A)
            );
        }
    }
}
