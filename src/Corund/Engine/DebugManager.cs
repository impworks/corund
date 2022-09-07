using System;
using Corund.Geometry;
using Corund.Tools.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Engine;

/// <summary>
/// Methods to help debug the game.
/// </summary>
public class DebugManager
{
    #region Constructor

    public DebugManager(GraphicsDevice device)
    {
        _boxTexture = TextureHelper.CreateColorTexture(device, Color.Red);
    }

    #endregion

    #region Fields

    /// <summary>
    /// Texture used to draw bounding boxes.
    /// </summary>
    private readonly Texture2D _boxTexture;

    /// <summary>
    /// Time elapsed since last FPS update.
    /// </summary>
    private float _fpsElapsedTime;

    /// <summary>
    /// Immediate FPS value.
    /// </summary>
    private int _fpsCounter;

    #endregion

    #region Properties

    /// <summary>
    /// Current FPS counter.
    /// </summary>
    public int FPS { get; private set; }

    #endregion

    #region Update

    /// <summary>
    /// Dumps the FPS counter into debug console.
    /// </summary>
    public void Update()
    {
        _fpsCounter++;
        _fpsElapsedTime += GameEngine.Delta;

        if (_fpsElapsedTime >= 1)
        {
            FPS = _fpsCounter;
            _fpsElapsedTime -= 1;
            _fpsCounter = 0;
        }
    }

    #endregion

    #region Geometry visualization

    /// <summary>
    /// Renders the current geometry to screen.
    /// </summary>
    public void DrawGeometry(IGeometryObject obj)
    {
        var geometry = obj?.Geometry;
        if (geometry == null)
            return;

        var transform = obj.GetTransformInfo(true);

        if (geometry is GeometryRect rect)
        {
            var poly = rect.CreateRectPolygon(transform);
            DrawRectPolygon(poly);
            return;
        }

        if (geometry is GeometryRectGroup group)
        {
            foreach (var groupRect in group.Rectangles)
            {
                var poly = groupRect.CreateRectPolygon(transform);
                DrawRectPolygon(poly);
            }

            return;
        }

        throw new ArgumentException("Unknown geometry type!");
    }

    /// <summary>
    /// Renders a single rectangle.
    /// </summary>
    public void DrawRectPolygon(RectPolygon rect)
    {
        DrawLine(rect.LeftUpper, rect.RightUpper);
        DrawLine(rect.RightUpper, rect.RightLower);
        DrawLine(rect.RightLower, rect.LeftLower);
        DrawLine(rect.LeftLower, rect.LeftUpper);
    }

    /// <summary>
    /// Renders a line.
    /// </summary>
    public void DrawLine(Vector2 from, Vector2 to)
    {
        var angle = (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        var length = Vector2.Distance(from, to);

        GameEngine.Render.TryBeginBatch(BlendState.AlphaBlend);
        GameEngine.Render.SpriteBatch.Draw(
            _boxTexture,
            from,
            null,
            Color.White,
            angle,
            Vector2.Zero,
            new Vector2(length, 1),
            SpriteEffects.None,
            0
        );
    }

    #endregion
}