using System;
using Corund.Engine;
using Corund.Frames;
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
    public class ScrollView: InteractiveObject, IView
    {
        #region Constants

        /// <summary>
        /// Coefficient for slowing down the scroll.
        /// </summary>
        private const float FRICTION = 6f;

        /// <summary>
        /// Minimum squared distance of a swipe to consider the scroll inertial.
        /// </summary>
        private const float MIN_INERTIAL_DISTANCE = 10;

        /// <summary>
        /// Minimum squared distance to drag before activating scroll mode.
        /// </summary>
        private const float MIN_CAPTURE_DISTANCE = 10;

        #endregion

        #region Constructor

        public ScrollView(int width, int height, ScrollDirection dir = ScrollDirection.Vertical)
        {
            ViewSize = new Vector2(width, height);
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
        private bool _isCaptured;
        private Vector2 _scrollSpeed;
        private ObjectBase _content;
        private Vector2 _contentSize;

        #endregion

        #region Properties

        /// <summary>
        /// Size of the window.
        /// </summary>
        public readonly Vector2 ViewSize;

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
                _content.Position = Vector2.Zero;
                _contentSize = GetContentSize(value);
            }
        }

        /// <summary>
        /// Offset of the scroll.
        /// </summary>
        public Vector2 Offset
        {
            get => -_content.Position;
            set => _content.Position = LimitOffset(Vector2.Zero, LimitDirection(-value));
        }

        /// <summary>
        /// Size of the content.
        /// </summary>
        public Vector2 ContentSize => _contentSize;

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the offset required to bring the specified child object to the top of the view.
        /// </summary>
        public Vector2 GetOffsetForChild(ObjectBase obj)
        {
            var curr = obj;
            var offset = Vector2.Zero;
            while (true)
            {
                offset -= curr.Position;
                curr = curr.Parent;

                if (ReferenceEquals(curr, this))
                    break;

                if(curr is FrameBase || curr is null)
                    throw new ArgumentException("Object is not a descendant of this view!");
            }

            return offset;
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
                    if (!_scrollSpeed.LengthSquared().IsAlmostZero())
                    {
                        _isCaptured = true;
                        GameEngine.Touch.Capture(touch, this);
                    }

                    // stop inertial scroll on touch
                    _scrollSpeed = Vector2.Zero;
                    _origTouch = t;
                    _origPosition = Content.Position;
                }
                else
                {
                    if (touch.State == TouchLocationState.Moved)
                    {
                        var rawOffset = LimitDirection(touch.Position - _origTouch.Value.Position);
                        if (_isCaptured)
                        {
                            // freely move object with direction limit and elasticity
                            var offset = LimitOffset(_origPosition.Value, rawOffset);
                            Content.Position = _origPosition.Value + offset;
                        }
                        else if(rawOffset.LengthSquared() >= MIN_CAPTURE_DISTANCE)
                        {
                            _isCaptured = true;
                            GameEngine.Touch.Capture(touch, this);
                        }
                    }
                    else if (touch.State == TouchLocationState.Released)
                    {
                        _origTouch = null;
                        _origPosition = null;
                        _isCaptured = false;

                        GameEngine.Defer(() => GameEngine.Touch.Release(touch));

                        // start inertial scroll
                        var rawSwipe = LimitDirection(touch.Position - _prevTouch.Value.Position);
                        if (rawSwipe.LengthSquared() >= MIN_INERTIAL_DISTANCE)
                            _scrollSpeed = rawSwipe;
                    }
                }
            }
            else
            {
                _origTouch = null;
                _origPosition = null;
                _isCaptured = false;
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
            else if (bottomRight.X < ViewSize.X)
                offset.X += (ViewSize.X - bottomRight.X);

            if (topLeft.Y > 0)
                offset.Y -= topLeft.Y;
            else if (bottomRight.Y < ViewSize.Y)
                offset.Y += (ViewSize.Y - bottomRight.Y);

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

            if (objSize.X < ViewSize.X)
                objSize.X = ViewSize.X;

            if (objSize.Y < ViewSize.Y)
                objSize.Y = ViewSize.Y;

            return objSize;
        }

        #endregion

        #region IView implementation

        /// <summary>
        /// Checks if the point in inside the view.
        /// </summary>
        public bool IsPointInView(Vector2 point)
        {
            var transform = GetTransformInfo(true);
            return Geometry.ContainsPoint(point, transform);
        }

        #endregion
    }
}
