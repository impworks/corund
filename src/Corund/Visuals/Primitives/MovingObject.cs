using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.Primitives;

/// <summary>
/// An object that can move and rotate.
/// </summary>
public abstract class MovingObject: ObjectBase
{
    #region Properties

    /// <summary>
    /// Angle derivative.
    /// Sprite is rotated by this value each second.
    /// </summary>
    public float Rotation;

    /// <summary>
    /// Sprite momentum.
    /// Sprite is moved by this value each second.
    /// </summary>
    public Vector2 Momentum;

    #endregion

    #region Methods

    public override void Update()
    {
        var pm = GameEngine.Current.PauseMode | PauseMode;
        var delta = GameEngine.Delta;

        if ((pm & PauseMode.Momentum) == 0)
        {
            if (!Rotation.IsAlmostZero())
                Angle += delta * Rotation;

            if (!Momentum.X.IsAlmostZero() || !Momentum.Y.IsAlmostZero())
                Position += delta * Momentum;
        }
    }

    #endregion
}