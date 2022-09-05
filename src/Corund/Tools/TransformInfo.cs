using System.Diagnostics;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;

namespace Corund.Tools;

/// <summary>
/// Information about an object's position relative to the screen.
/// </summary>
[DebuggerDisplay("Transform (Pos: {Position}, Angle: {Angle}, Scale: {ScaleVector})")]
public struct TransformInfo
{
    #region Constructor

    public TransformInfo(Vector2 position, float angle, Vector2 scale)
    {
        Position = position;
        Angle = angle;
        ScaleVector = scale;
    }

    #endregion

    #region Constants

    /// <summary>
    /// Empty transform.
    /// </summary>
    public static TransformInfo None = new TransformInfo(Vector2.Zero, 0, Vector2.One);

    #endregion

    #region Fields

    /// <summary>
    /// Total position offset.
    /// </summary>
    public readonly Vector2 Position;

    /// <summary>
    /// Total angle offset.
    /// </summary>
    public readonly float Angle;

    /// <summary>
    /// Total scale offset.
    /// </summary>
    public readonly Vector2 ScaleVector;

    #endregion

    #region Methods

    /// <summary>
    /// Translates a point according to transform info.
    /// </summary>
    public Vector2 Translate(Vector2 point)
    {
        return point.Rotate(Angle)*ScaleVector + Position;
    }

    /// <summary>
    /// Cancels a point transformation.
    /// </summary>
    public Vector2 TranslateBack(Vector2 point)
    {
        return ((point - Position) / ScaleVector).Rotate(-Angle);
    }

    #endregion
}