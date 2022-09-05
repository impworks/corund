using Corund.Visuals.Primitives;

namespace Corund.Behaviours;

/// <summary>
/// An attachable class that modifies the properties of a dynamic object.
/// </summary>
public abstract class BehaviourBase
{
    /// <summary>
    /// Invoked once when the behaviour is added to the object.
    /// Can initialize both the object and the behaviour properties.
    /// To be overridden in children.
    /// </summary>
    public virtual void Bind(DynamicObject obj)
    {
    }

    /// <summary>
    /// Invoked once when the behaviour is removed from the object.
    /// Can discard changes made to object properties.
    /// To be overridden in children.
    /// </summary>
    public virtual void Unbind(DynamicObject obj)
    {
    }

    /// <summary>
    /// Apply the behaviour to the object.
    /// </summary>
    public abstract void UpdateObjectState(DynamicObject obj);
}