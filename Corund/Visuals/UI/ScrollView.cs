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
        private const float FRICTION = 6f;

        /// <summary>
        /// Minimum squared distance of a swipe to consider the scroll inertial.
        /// </summary>
        private const float MIN_DISTANCE = 10;

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

        private Vector2? _origPosition;
        private TouchLocation? _origTouch;
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
                _contentSize = GetContentSize(value);
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
            {
                base.Update();
                return;
            }

            var t = this.TryGetTouch(true);
            if (t is TouchLocation touch)
            {
                if (_origTouch == null)
                {
                    // stop inertial scroll on touch
                    _scrollSpeed = Vector2.Zero;
                    _origTouch = t;
                    _origPosition = Content.Position;
                }
                else
                {
                    if (touch.State == TouchLocationState.Moved)
                    {
                        // freely move object with direction limit and elasticity
                        var rawOffset = LimitDirection(touch.Position - _origTouch.Value.Position);
                        var offset = LimitOffset(_origPosition.Value, rawOffset);
                        Content.Position = _origPosition.Value + offset;
                    }
                    else if (touch.State == TouchLocationState.Released)
                    {
                        _origTouch = null;
                        _origPosition = null;

                        // start inertial scroll
                        var rawSwipe = LimitDirection(touch.Position - _prevTouch.Value.Position);
                        if (rawSwipe.LengthSquared() >= MIN_DISTANCE)
                            _scrollSpeed = rawSwipe;
                    }
                }
            }
            else
            {
                _origTouch = null;
                _origPosition = null;
            }

            if (!_scrollSpeed.LengthSquared().IsAlmostZero())
            {
                _scrollSpeed = _scrollSpeed * (1 - (FRICTION * GameEngine.Delta));
                Content.Position += LimitOffset(Content.Position, _scrollSpeed);

                if (_scrollSpeed.LengthSquared() <= 0.1)
                    _scrollSpeed = Vector2.Zero;
            }

            _prevTouch = t;

            base.Update();
            Content.Update();
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Applies the offset with limits to avoid overscrolling.
        /// </summary>
        private Vector2 LimitOffset(Vector2 orig, Vector2 offset)
        {
            var topLeft = orig  + offset;
            var bottomRight = topLeft + _contentSize;

            if (topLeft.X > 0)
                offset.X -= topLeft.X;
            else if (bottomRight.X < Size.X)
                offset.X += (Size.X - bottomRight.X);

            if (topLeft.Y > 0)
                offset.Y -= topLeft.Y;
            else if (bottomRight.Y < Size.Y)
                offset.Y += (Size.Y - bottomRight.Y);

            return offset;
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

        /// <summary>
        /// Returns the content size, taking the entire view if it is smaller.
        /// </summary>
        private Vector2 GetContentSize(ObjectBase obj)
        {
            var objSize = (obj as IGeometryObject)?.Geometry.GetBoundingBox(null).GetSize() ?? Vector2.Zero;

            if (objSize.X < Size.X)
                objSize.X = Size.X;

            if (objSize.Y < Size.Y)
                objSize.Y = Size.Y;

            return objSize;
        }

        #endregion
    }
}
