using System;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Behaviours.Movement;

/// <summary>
/// Positions object relative to camera with a coefficient.
/// </summary>
public class ParallaxBehaviour: BehaviourBase
{
    #region Constructor

    /// <summary>
    /// Creates a new ParallaxBehaviour.
    /// Can only be applied to direct children of the frame (no nesting).
    /// </summary>
    /// <param name="coefficient">
    /// Parallax coefficient.
    /// 0 = object is static, never moves.
    /// 1 = object moves as usual.
    /// 2 = object moves twice as fast.
    /// </param>
    /// <param name="affectX">Flag indicating that horizontal movement is parallaxed.</param>
    /// <param name="affectY">Flag indicating that vertical movement is parallaxed.</param>
    public ParallaxBehaviour(float coefficient, bool affectX = true, bool affectY = true)
    {
        if(coefficient < 0)
            throw new ArgumentException("Parallax coefficient cannot be less than zero.", nameof(coefficient));

        _parallax = new Vector2(
            affectX ? coefficient : 1,
            affectY ? coefficient : 1
        );

        _lastOffset = Vector2.Zero;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Parallax coefficient.
    /// </summary>
    private readonly Vector2 _parallax;

    /// <summary>
    /// The previously applied offset.
    /// </summary>
    private Vector2 _lastOffset;

    #endregion

    #region Behaviour methods

    /// <summary>
    /// Checks that object is the direct child of the frame.
    /// </summary>
    public override void Bind(DynamicObject obj)
    {
        var isDirectChild = obj.Parent == null;
        if (!isDirectChild)
            throw new ArgumentException("Parallax behaviour can only be applied to a direct child of the frame!");

        ApplyParallax(obj);
    }

    /// <summary>
    /// Cancels out the parallax effect.
    /// </summary>
    public override void Unbind(DynamicObject obj)
    {
        CancelParallax(obj);
    }

    /// <summary>
    /// Updates the object's position relative to the camera.
    /// </summary>
    public override void UpdateObjectState(DynamicObject obj)
    {
        // to allow object's movement in parallaxed state, we reapply the effect each time

        CancelParallax(obj);
        ApplyParallax(obj);
    }

    #endregion

    #region Private helpers

    /// <summary>
    /// Adjusts the object's position using parallax.
    /// </summary>
    private void ApplyParallax(DynamicObject obj)
    {
        var cam = GameEngine.Current.Frame.Camera;
        var offset = (Vector2.One - _parallax) * cam.Position;
        obj.Position += offset;
        _lastOffset = offset;
    }

    /// <summary>
    /// Resets the object's original position.
    /// </summary>
    private void CancelParallax(DynamicObject obj)
    {
        obj.Position -= _lastOffset;
    }

    #endregion
}