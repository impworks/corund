using System;
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
    /// Base class for window-like views that allow scrolling the contents.
    /// </summary>
    public abstract class ScrollViewBase: InteractiveObject, IView
    {
        #region Constants

        /// <summary>
        /// Minimum squared distance to drag before activating scroll mode.
        /// </summary>
        protected const float MIN_CAPTURE_DISTANCE = 10;

        #endregion

        #region Constructor

        public ScrollViewBase(int width, int height, ScrollDirection dir)
        {
            if(width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), "View width must be at least 1");

            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height), "View height must be at least 1");

            ViewSize = new Vector2(width, height);
            Geometry = new GeometryRect(0, 0, width, height);
            _renderTarget = GameEngine.Render.CreateRenderTarget(width, height);
            _direction = dir;
        }

        #endregion

        #region Fields

        protected readonly RenderTarget2D _renderTarget;
        protected readonly ScrollDirection _direction;

        protected ObjectBase _content;
        protected bool _isCaptured;
        protected bool _isDisabled;
        protected Vector2 _contentSize;
        protected Vector2? _origPosition;
        protected TouchLocation? _origTouch;
        protected TouchLocation? _prevTouch;

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
        /// Size of the content.
        /// </summary>
        public Vector2 ContentSize => _contentSize;

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

        #region Drawing

        protected override void DrawInternal()
        {
            if (_content == null)
                return;

            GameEngine.Render.PushContext(_renderTarget, Color.Transparent);
            _content.Draw();
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

        #endregion

        #region Scrolling core

        public override void Update()
        {
            if (_content == null || _isDisabled)
            {
                base.Update();
                return;
            }

            var t = this.TryGetTouch(true);
            if (t is TouchLocation touch)
            {
                if (_origTouch == null)
                {
                    OnTouchPressed(touch);

                    _origTouch = t;
                    _origPosition = _content.Position;
                }
                else
                {
                    if (touch.State == TouchLocationState.Moved)
                    {
                        OnTouchMoved(touch);
                    }
                    else if (touch.State == TouchLocationState.Released)
                    {
                        OnTouchReleased(touch);

                        _origTouch = null;
                        _origPosition = null;
                        _isCaptured = false;

                        GameEngine.Defer(() => GameEngine.Touch.Release(touch));
                    }
                }
            }
            else
            {
                _origTouch = null;
                _origPosition = null;
                _isCaptured = false;
            }

            _prevTouch = t;

            base.Update();
            _content.Update();
        }

        /// <summary>
        /// Handles the first touch to this scroll view.
        /// </summary>
        protected virtual void OnTouchPressed(TouchLocation touch)
        {

        }

        /// <summary>
        /// Handles touch moving.
        /// </summary>
        protected virtual void OnTouchMoved(TouchLocation touch)
        {
            var rawOffset = LimitDirection(touch.Position - _origTouch.Value.Position);
            if (_isCaptured)
            {
                // freely move object with direction limit
                var offset = LimitOffset(_origPosition.Value, rawOffset);
                _content.Position = _origPosition.Value + offset;
            }
            else if (rawOffset.LengthSquared() >= MIN_CAPTURE_DISTANCE)
            {
                _isCaptured = true;
                GameEngine.Touch.Capture(touch, this);
            }
        }

        /// <summary>
        /// Handles the touch release.
        /// </summary>
        protected virtual void OnTouchReleased(TouchLocation touch)
        {

        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Applies the offset with limits to avoid overscrolling.
        /// </summary>
        protected Vector2 LimitOffset(Vector2 orig, Vector2 offset)
        {
            var topLeft = orig + offset;
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
        protected Vector2 LimitDirection(Vector2 vector)
        {
            if (_direction == ScrollDirection.Horizontal)
                return new Vector2(vector.X, 0);

            if (_direction == ScrollDirection.Vertical)
                return new Vector2(0, vector.Y);

            return vector;
        }

        /// <summary>
        /// Returns the content size, taking the entire view if it is smaller.
        /// </summary>
        protected Vector2 GetContentSize(ObjectBase obj)
        {
            var objSize = (obj as IGeometryObject)?.Geometry.GetBoundingBox(null).GetSize() ?? Vector2.Zero;

            if (objSize.X < ViewSize.X)
                objSize.X = ViewSize.X;

            if (objSize.Y < ViewSize.Y)
                objSize.Y = ViewSize.Y;

            return objSize;
        }

        #endregion
    }
}
