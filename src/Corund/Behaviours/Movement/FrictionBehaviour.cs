using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Movement;

/// <summary>
/// Gradually decreases the object's speed.
/// </summary>
public class FrictionBehaviour: BehaviourBase
{
    #region Constructor

    public FrictionBehaviour(float friction)
    {
        Friction = friction;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The friction coefficient.
    /// 0 = no friction.
    /// 1 = complete stop.
    /// </summary>
    public float Friction;

    #endregion

    #region Methods

    public override void UpdateObjectState(DynamicObject obj)
    {
        if (obj.Speed.IsAlmostZero())
            return;

        obj.Speed *= (1 - Friction);
    }

    #endregion
}