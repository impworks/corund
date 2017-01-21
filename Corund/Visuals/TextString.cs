using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Visuals
{
    /// <summary>
    /// A simple block of text.
    /// </summary>
    public class TextString: InteractiveObject
    {
        #region Constructor

        public TextString(SpriteFont font, string text, float? maxWidth = null)
        {
            _font = font;
            _originalText = text;
            _maxWidth = maxWidth;

            BlendState = BlendState.AlphaBlend;

            Refresh();
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
        /// Maximum allowed width.
        /// </summary>
        private float? _maxWidth;

        /// <summary>
        /// Geometry for current text.
        /// </summary>
        private GeometryRectGroup _geometry;

        /// <summary>
        /// Text string prepared for rendering.
        /// </summary>
        private List<string> _preparedText;

        #endregion

        #region Properties

        /// <summary>
        /// Sprite font used to render the string.
        /// </summary>
        public SpriteFont Font
        {
            get { return _font; }
            set
            {
                if (_font == value)
                    return;

                _font = value;
                Refresh();
            }
        }

        /// <summary>
        /// Original text.
        /// </summary>
        public string Text
        {
            get { return _originalText; }
            set
            {
                if (_originalText == value)
                    return;

                _originalText = value;
                Refresh();
            }
        }

        /// <summary>
        /// Maximum allowed width before the text wraps.
        /// </summary>
        public float? MaxWidth
        {
            get { return _maxWidth; }
            set
            {
                if (_maxWidth == value)
                    return;

                _maxWidth = value;
                Refresh();
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

        public override void Draw()
        {
            base.Draw();

            GameEngine.Render.TryBeginBatch(BlendState);

            var transform = GetTransformInfo(true);
            var offset = new Vector2(0, 0);

            for (var idx = 0; idx < _preparedText.Count; idx++)
            {
                var line = _preparedText[idx];
                GameEngine.Render.SpriteBatch.DrawString(
                    _font,
                    line,
                    transform.Translate(offset),
                    Tint,
                    transform.Angle,
                    Vector2.Zero,
                    transform.ScaleVector,
                    SpriteEffects.None,
                    GameEngine.Current.ZOrderFunction(this)
                );

                var lineSize = _font.MeasureString(line);
                offset.Y += lineSize.Y;
            }
        }

        /// <summary>
        /// Recalculates the text when a property has been changed.
        /// </summary>
        private void Refresh()
        {
            // todo...
        }

        #endregion
    }
}
