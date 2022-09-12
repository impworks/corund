using System;
using Corund.Engine;
using Corund.Visuals.Primitives;

namespace Corund.Behaviours.Movement;

/// <summary>
/// Gradually accelerates or slows down.
/// </summary>
public class InertialBehaviour: IBehaviour
{
    #region Constructor

    /// <summary>
    /// Creates a new inertial movement behaviour.
    /// </summary>
    /// <param name="coefficient">Inertia coefficient. Accelerates if value is greater than zero, slows down otherwise.</param>
    /// <param name="isRelative">If true, coefficient is considered a fraction of the actual value. Otherwise, it's an actual value.</param>
    public InertialBehaviour(float coefficient, bool isRelative = false)
    {
        Coefficient = coefficient;
        IsRelative = isRelative;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The friction coefficient.
    /// 0 = no friction.
    /// 1 = complete stop.
    /// </summary>
    public float Coefficient;

    /// <summary>
    /// Flag indicating that the coefficient is a fraction of the actual value, otherwise it's an actual value.
    /// </summary>
    public bool IsRelative;

    #endregion

    #region Methods

    public void UpdateObjectState(DynamicObject obj)
    {
        var source = IsRelative ? obj.Speed * Coefficient : Coefficient;
        var speed = obj.Speed * (1 + (source * GameEngine.Delta));
        obj.Speed = MathF.Max(speed, 0);
    }

    #endregion
}