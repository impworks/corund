using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Tweening
{
    public interface IPropertyAnimation: IEffect
    {
        /// <summary>
        /// Updates the property value.
        /// The property animator stores the property, easing, time and value ranges internally.
        /// </summary>
        void UpdateObjectState(DynamicObject obj);

        /// <summary>
        /// Skips the animation, setting the property to target value.
        /// </summary>
        void SkipAnimation(DynamicObject obj);
    }
}
