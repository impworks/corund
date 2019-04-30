using Corund.Engine;
using Corund.Geometry;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Visuals.UI
{
    /// <summary>
    /// A window that allows scrolling its content.
    /// </summary>
    public class ScrollView: InteractiveObject
    {
        #region Constants

        /// <summary>
        /// Coefficient for slowing down the scroll.
        /// </summary>
        private const float FRICTION = 0.2f;

        /// <summary>
        /// Minimum squared distance of a swipe to consider the scroll inertial.
        /// </summary>
        private const float MIN_DISTANCE = 5;

        #endregion

        #region Constructor

        public ScrollView(int width, int height, ScrollDirection dir = ScrollDirection.Vertical)
        {
            Size = new Vector2(width, height);
            Geometry = new GeometryRect(0, 0, width, height);
            _renderTarget = GameEngine.Render.CreateRenderTarget(width, height);
            _direction = dir;
        }

        #endregion

        #region Fields

        private readonly RenderTarget2D _renderTarget;
        private readonly ScrollDirection _direction;

        private TouchLocation? _prevTouch;
        private Vector2 _scrollSpeed;
        private ObjectBase _content;
        private Vector2 _contentSize;

        #endregion

        #region Properties

        /// <summary>
        /// Size of the window.
        /// </summary>
        public readonly Vector2 Size;

        /// <summary>
        /// Geometry.
        /// </summary>
        public override IGeometry Geometry { get; }

        /// <summary>
        /// Scrollable content.
        /// </summary>
        public ObjectBase Content
        {
            get => _content;
            set
            {
                if (_content == value)
                    return;

                Attach(value);
                _content = value;
                _contentSize = (value as IGeometryObject)?.Geometry.GetBoundingBox(null).GetSize() ?? Vector2.Zero;
            }
        }

        #endregion

        #region Overrides

        protected override void DrawInternal()
        {
            if (Content == null)
                return;

            GameEngine.Render.PushContext(_renderTarget, Color.Transparent);
            Content.Draw();
            GameEngine.Render.PopContext();

            GameEngine.Render.TryBeginBatch(BlendState.AlphaBlend);
            var transform = GetTransformInfo(true);
            var z = GameEngine.Current.ZOrderFunction(this);
            GameEngine.Render.SpriteBatch.Draw(
                _renderTarget,
                transform.Position,
                null,
                Tint,
                transform.Angle,
                Vector2.Zero,
                transform.ScaleVector,
                SpriteEffects.None,
                z
            );
        }

        public override void Update()
        {
            if (Content == null)
                return;

            var t = this.TryGetTouch(true);
            if (t is TouchLocation touch)
            {
                if (_prevTouch == null)
                {
                    // stop inertial scroll on touch
                    _scrollSpeed = Vector2.Zero;
                }
                else
                {
                    var mvmt = LimitDirection(touch.Position - _prevTouch.Value.Position);
                    if (touch.State == TouchLocationState.Moved)
                    {
                        // adjust the content's position
                        MoveContent(mvmt);
                    }
                    else if (touch.State == TouchLocationState.Released)
                    {
                        // start inertial scroll
                        if (mvmt.LengthSquared() >= MIN_DISTANCE)
                            _scrollSpeed = mvmt;
                    }
                }
            }

            if (!_scrollSpeed.LengthSquared().IsAlmostZero())
            {
                _scrollSpeed = _scrollSpeed * (1 - (FRICTION * GameEngine.Delta));
                MoveContent(_scrollSpeed);
            }

            base.Update();

            _prevTouch = t;
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Moves the content.
        /// </summary>
        private void MoveContent(Vector2 offset)
        {
            Content.Position += offset;
            // todo: bounds!
        }

        /// <summary>
        /// Limits the vector according to the allowed scroll direction.
        /// </summary>
        private Vector2 LimitDirection(Vector2 vector)
        {
            if (_direction == ScrollDirection.Horizontal)
                vector.Y = 0;
            else if (_direction == ScrollDirection.Vertical)
                vector.X = 0;

            return vector;
        }

        #endregion
    }
}
