using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Corund.Tools.UI;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals;

/// <summary>
/// A simple block of text.
/// </summary>
public class TextObject: InteractiveObject
{
    #region Constructor
    
    public TextObject(string text)
        : this(GameEngine.EmbeddedContent.Load<SpriteFont>("Fonts/default"), text)
    {

    }

    public TextObject(string fontAsset, string text)
        : this(GameEngine.Content.Load<SpriteFont>(fontAsset), text)
    {

    }

    public TextObject(SpriteFont font, string text)
    {
        _font = font;
        _originalText = text;
        _preparedText = text.Split('\n');

        BlendState = BlendState.AlphaBlend;

        RefreshGeometry();
    }

    #endregion

    #region Fields

    /// <summary>
    /// Sprite font used to render the string.
    /// </summary>
    private SpriteFont _font;

    /// <summary>
    /// Original text as passed by the user.
    /// </summary>
    private string _originalText;

    /// <summary>
    /// Geometry for current text.
    /// </summary>
    private GeometryRectGroup _geometry;

    /// <summary>
    /// Text string prepared for rendering.
    /// </summary>
    private string[] _preparedText;

    /// <summary>
    /// Horizontal alignment of each line.
    /// </summary>
    private HorizontalAlignment _horizontalAlignment;

    /// <summary>
    /// Vertical alignment of the entire block of text.
    /// </summary>
    private VerticalAlignment _verticalAlignment;

    #endregion

    #region Properties

    /// <summary>
    /// Sprite font used to render the string.
    /// </summary>
    public SpriteFont Font
    {
        get => _font;
        set
        {
            if (_font == value)
                return;

            _font = value;
            RefreshGeometry();
        }
    }

    /// <summary>
    /// Original text.
    /// </summary>
    public string Text
    {
        get => _originalText;
        set
        {
            if (_originalText == value)
                return;

            _originalText = value;
            _preparedText = value?.Split('\n') ?? new string[0];

            RefreshGeometry();
        }
    }

    /// <summary>
    /// Horizontal alignment of each line.
    /// </summary>
    public HorizontalAlignment HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            if (_horizontalAlignment == value)
                return;

            _horizontalAlignment = value;
            RefreshGeometry();
        }
    }

    /// <summary>
    /// Vertical alignment of the entire block of text.
    /// </summary>
    public VerticalAlignment VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            if (_verticalAlignment == value)
                return;

            _verticalAlignment = value;
            RefreshGeometry();
        }
    }

    /// <summary>
    /// Geometry of the text.
    /// </summary>
    public override IGeometry Geometry => _geometry;

    /// <summary>
    /// Blending mode to use.
    /// </summary>
    public BlendState BlendState;

    #endregion

    #region Methods

    protected override void DrawInternal()
    {
        var transform = GetTransformInfo(true);
        var tint = GetMixedTintColor();

        GameEngine.Render.TryBeginBatch(BlendState);
        for (var idx = 0; idx < _preparedText.Length; idx++)
        {
            var line = _preparedText[idx];
            var position = _geometry.Rectangles[idx].Position;

            GameEngine.Render.SpriteBatch.DrawString(
                _font,
                line,
                transform.Translate(position),
                tint,
                transform.Angle,
                Vector2.Zero,
                transform.ScaleVector,
                SpriteEffects.None,
                GameEngine.Current.ZOrderFunction(this)
            );
        }
    }

    /// <summary>
    /// Recalculates the text when a property has been changed.
    /// </summary>
    private void RefreshGeometry()
    {
        if (_preparedText.Length == 0)
        {
            _geometry = null;
            return;
        }

        var rects = new List<GeometryRect>();
        var lineHeight = _font.MeasureString(_originalText[0].ToString()).Y;
        var blockHeight = lineHeight*_preparedText.Length;

        var y = 0f;
        if (_verticalAlignment == VerticalAlignment.Center)
            y -= blockHeight/2;
        else if (_verticalAlignment == VerticalAlignment.Bottom)
            y -= blockHeight;

        foreach (var line in _preparedText)
        {
            var size = _font.MeasureString(line);
            var x = 0f;
            if (_horizontalAlignment == HorizontalAlignment.Center)
                x -= size.X/2;
            else if (_horizontalAlignment == HorizontalAlignment.Right)
                x -= size.X;

            rects.Add(new GeometryRect(x, y, size.X, size.Y));

            y += lineHeight;
        }

        _geometry = new GeometryRectGroup(rects.ToArray());
    }

    #endregion
}