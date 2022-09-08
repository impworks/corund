using System;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals.Particles;

/// <summary>
/// Base class for "thin" particles.
/// </summary>
public class ParticleObject : MovingObject
{
    #region Constructor

    public ParticleObject(string assetName, Vector2? hotSpot = null)
        : this(GameEngine.Content.Load<Texture2D>(assetName), hotSpot)
    {
    }

    public ParticleObject(Texture2D texture, Vector2? hotSpot = null)
    {
        _texture = texture;
        _hotSpot = hotSpot ?? new Vector2(texture.Width / 2f, texture.Height / 2f);
    }

    #endregion

    #region Fields

    /// <summary>
    /// Current texture.
    /// </summary>
    private readonly Texture2D _texture;

    /// <summary>
    /// Offset from texture's top left coordinate that is the origin point for drawing.
    /// </summary>
    private readonly Vector2 _hotSpot;

    #endregion

    #region Properties

    /// <summary>
    /// Particle's lifespan in seconds before it fades out.
    /// </summary>
    public float LifeDuration;

    /// <summary>
    /// Time elapsed since the particle's creation.
    /// </summary>
    public float ElapsedTime;

    /// <summary>
    /// Particle age in 0..1 range.
    /// </summary>
    public float Age => MathF.Min(ElapsedTime / LifeDuration, 1f);

    #endregion

    #region Drawing

    public override void Update()
    {
        base.Update();

        ElapsedTime += GameEngine.Delta;
    }

    /// <summary>
    /// Draws the particle to the screen.
    /// </summary>
    public override void Draw()
    {
        // sic! SpriteBatch.Begin is called once in ParticleGroup.DrawInternal

        var transform = GetTransformInfo(true);
        var tint = GetMixedTintColor();
        GameEngine.Render.SpriteBatch.Draw(
            _texture,
            transform.Position,
            null,
            tint,
            transform.Angle,
            _hotSpot,
            transform.ScaleVector,
            SpriteEffects.None,
            GameEngine.Current.ZOrderFunction(this)
        );
    }

    #endregion
}