using Corund.Tools;
using Corund.Tools.Interpolation;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Tweening
{
    /// <summary>
    /// Animation for vector2 values.
    /// </summary>
    public class Vector2Animation<TObject> : PropertyAnimationBase<TObject, Vector2>
       where TObject : DynamicObject
    {
        public Vector2Animation(PropertyDescriptor<TObject, Vector2> descriptor, Vector2 targetValue, float duration, InterpolationMethod interpolation = null)
            : base(descriptor, targetValue, duration, interpolation)
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
