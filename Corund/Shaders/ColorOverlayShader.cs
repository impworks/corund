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
        /// Overlay color.
        /// </summary>
        private Color _color;

        /// <summary>
        /// Overlay texture.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Alpha effect.
        /// </summary>
        private readonly AlphaTestEffect _alphaEffect;

        private Func<ObjectBase, float> _funcBackup;
        private SpriteBatch _batchBackup;

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
                _texture.SetData(new[] {value});
            }
        }

        #endregion

        #region Methods

        public override void DrawWrapper(Action innerDraw)
        {
            var render = GameEngine.Render;

            var funcBackup = GameEngine.Current.ZOrderFunction;
            GameEngine.Current.ZOrderFunction = x => 0f;

            // using temporary target
            render.PushContext(_renderTarget);
            {
                render.Device.Clear(Color.Transparent);

                // PASS 1: render object to stencil buffer
                {
                    render.Device.Clear(ClearOptions.Stencil, Color.Black, 0f, 0);

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
                render.SpriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
            }
        }

        protected override RenderTarget2D CreateRenderTarget()
        {
            var pp = GameEngine.Render.Device.PresentationParameters;
            return new RenderTarget2D(
                GameEngine.Render.Device,
                pp.BackBufferWidth,
                pp.BackBufferHeight,
                false,
                SurfaceFormat.Color,
                (DepthFormat)3      // Actual DepthFormat.Depth24Stencil8
            );
        }

        #endregion
    }
}
