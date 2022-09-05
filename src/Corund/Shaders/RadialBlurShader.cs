using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders;

/// <summary>
/// A radial blur with specified blur point.
/// </summary>
public class RadialBlurShader: SinglePassShaderBase
{
    #region Constructor

    public RadialBlurShader()
        : this(GameEngine.Screen.Center, 1)
    {
            
    }

    public RadialBlurShader(Vector2 center, float amount)
    {
        Center = center;
        Amount = amount;

        _effect = GameEngine.EmbeddedContent.Load<Effect>("Effects/radial-blur");
    }

    #endregion

    #region Fields

    /// <summary>
    /// Amount of the zoom (0...?).
    /// 0 = no zoom.
    /// 1 = normal zoom
    /// </summary>
    private float _amount;

    #endregion

    #region Properties

    /// <summary>
    /// Center of the zoom, in screen coordinates.
    /// </summary>
    public Vector2 Center;

    /// <summary>
    /// Amount of the zoom (0...10).
    /// 0 = no zoom.
    /// 1 = normal zoom
    /// </summary>
    public float Amount
    {
        get => _amount;
        set => _amount = MathHelper.Clamp(value, 0, 10);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets shader configuration for rendering.
    /// </summary>
    protected override void ConfigureShader(DynamicObject obj)
    {
        _effect.Parameters["Center"].SetValue(Center / GameEngine.Screen.Size);
        _effect.Parameters["Amount"].SetValue(Amount / 10);
    }

    #endregion
}