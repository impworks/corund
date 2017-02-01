using System;
using Corund.Engine;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Shaders
{
    /// <summary>
    /// A shader that draws an object filled with specified color.
    /// </summary>
    public class ColorOverlayShader: ShaderBase
    {
        #region Constructor

        static ColorOverlayShader()
        {
            _stencilBefore = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Always,
                StencilPass = StencilOperation.Replace,
                ReferenceStencil = 1
            };

            _stencilAfter = new DepthStencilState
            {
                StencilEnable = true,
                StencilFunction = CompareFunction.Equal,
                ReferenceStencil = 1
            };
        }

        public ColorOverlayShader(Color color)
        {
            _texture = new Texture2D(GameEngine.Render.Device, 1, 1);
            Color = color;

            _alphaEffect = new AlphaTestEffect(GameEngine.Render.Device)
            {
                DiffuseColor = Color.White.ToVector3(),
                AlphaFunction = CompareFunction.Greater,
                ReferenceAlpha = 0,
                World = Matrix.Identity,
                View = Matrix.Identity,
                Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0) *
                    Matrix.CreateOrthographicOffCenter(0, GameEngine.Render.Device.Viewport.Width, GameEngine.Render.Device.Viewport.Height, 0, 0, 1)
            };
        }

        #endregion

        #region Fields

        /// <summary>
        /// Stencil state for drawing to intermediate texture.
        /// </summary>
        private static DepthStencilState _stencilBefore;

        /// <summary>
        /// Stencil state for drawing to screen.
        /// </summary>
        private static DepthStencilState _stencilAfter;

        /// <summary>
        /// Original color specified by user.
        /// </summary>
        private Color _color;

        /// <summary>
        /// Opaque overlay color.
        /// </summary>
        private Color _overlayColor;

        /// <summary>
        /// Opacity container color.
        /// </summary>
        private Color _opacityColor;

        /// <summary>
        /// Overlay texture.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Alpha effect.
        /// </summary>
        private readonly AlphaTestEffect _alphaEffect;

        #endregion

        #region Properties

        /// <summary>
        /// Overlay color.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set
            {
                if (_color == value)
                    return;

                _color = value;
                _overlayColor = new Color(value, 1f);
                _texture.SetData(new[] { _overlayColor });

                Opacity = value.A/255f;
            }
        }

        /// <summary>
        /// Overlay opacity.
        /// </summary>
        public float Opacity
        {
            get { return _opacityColor.A/255f; }
            set { _opacityColor = new Color(value, value, value, value); }
        }

        #endregion

        #region Methods

        public override void DrawWrapper(DynamicObject obj, Action innerDraw)
        {
            var render = GameEngine.Render;

            var funcBackup = GameEngine.Current.ZOrderFunction;
            GameEngine.Current.ZOrderFunction = x => 0f;

            // using temporary target
            render.PushContext(_renderTarget, Color.Transparent);
            {
                // PASS 1: render object to stencil buffer
                {
                    render.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, _stencilBefore, null, _alphaEffect);
                    render.IsSpriteBatchLocked = true;

                    innerDraw();

                    render.IsSpriteBatchLocked = false;
                    render.SpriteBatch.End();
                }

                // PASS 2: render overlay to RT using stencil
                {
                    render.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, _stencilAfter, null);
                    render.SpriteBatch.Draw(_texture, GameEngine.Render.Device.Viewport.Bounds, Color.White);
                    render.SpriteBatch.End();
                }

                render.PopContext();
            }

            GameEngine.Current.ZOrderFunction = funcBackup;

            // PASS 3: render result to outer context
            {
                render.TryBeginBatch(BlendState.AlphaBlend);
                render.SpriteBatch.Draw(_renderTarget, Vector2.Zero, _opacityColor);
            }
        }

        protected override RenderTarget2D CreateRenderTarget()
        {
            var pp = GameEngine.Render.Device.PresentationParameters;
            return new RenderTarget2D(
                GameEngine.Render.Device,
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                true,
                SurfaceFormat.Color,
                (DepthFormat)3,      // Actual DepthFormat.Depth24Stencil8,
                1,
                RenderTargetUsage.PreserveContents
            );
        }

        #endregion
    }
}
