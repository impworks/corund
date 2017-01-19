using System;
using System.Diagnostics;
using Corund.Geometry;
using Corund.Tools;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Engine
{
    /// <summary>
    /// Methods to help debug the game.
    /// </summary>
    public class DebugManager
    {
        #region Constructor

        public DebugManager(GraphicsDevice device)
        {
            _boxTexture = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            _boxTexture.SetData(new[] { Color.Red });
        }

        #endregion

        #region Fields and properties

        /// <summary>
        /// Texture used to draw bounding boxes.
        /// </summary>
        private readonly Texture2D _boxTexture;

        /// <summary>
        /// Current FPS counter.
        /// </summary>
        private int _fps;

        /// <summary>
        /// Time elapsed since last FPS update.
        /// </summary>
        private float _fpsElapsedTime;

        /// <summary>
        /// Flag indicating that FPS must be calculated and written to the debug console.
        /// </summary>
        public bool DisplayFPS;

        #endregion

        #region Update

        /// <summary>
        /// Dumps the FPS counter into debug console.
        /// </summary>
        public void Update()
        {
            if (DisplayFPS)
            {
                _fps++;
                _fpsElapsedTime += GameEngine.Delta;

                if (_fpsElapsedTime >= 1)
                {
                    Debug.WriteLine($"FPS: {_fps}");

                    _fps = 0;
                    _fpsElapsedTime = 0;
                }
            }
        }

        #endregion

        #region Geometry visualization

        /// <summary>
        /// Renders the current geometry to screen.
        /// </summary>
        public void DrawGeometry(InteractiveObject obj)
        {
            var geometry = obj.Geometry;
            if (geometry == null)
                return;

            var transform = obj.GetTransformInfo();

            var rect = geometry as GeometryRect;
            if (rect != null)
            {
                var poly = rect.CreateRectPolygon(transform);
                DrawRectPolygon(poly);
                return;
            }

            var group = geometry as GeometryRectGroup;
            if (group != null)
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
        private void DrawRectPolygon(RectPolygon rect)
        {
            DrawLine(rect.LeftUpper, rect.RightUpper);
            DrawLine(rect.RightUpper, rect.RightLower);
            DrawLine(rect.RightLower, rect.LeftLower);
            DrawLine(rect.LeftLower, rect.LeftUpper);
        }

        /// <summary>
        /// Renders a line.
        /// </summary>
        private void DrawLine(Vector2 from, Vector2 to)
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
}
