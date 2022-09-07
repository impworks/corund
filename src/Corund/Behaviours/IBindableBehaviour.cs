using Corund.Visuals.Primitives;

namespace Corund.Behaviours
{
    /// <summary>
    /// Extended interface for behaviours that need to execute additional logic when attaching to / detaching from the object.
    /// </summary>
    internal interface IBindableBehaviour
    {
        /// <summary>
        /// Invoked once when the behaviour is added to the object.
        /// Can initialize both the object and the behaviour properties.
        /// </summary>
        void Bind(DynamicObject obj);

        /// <summary>
        /// Invoked once when the behaviour is removed from the object.
        /// Can discard changes made to object properties.
        /// </summary>
        void Unbind(DynamicObject obj);
    }
}
