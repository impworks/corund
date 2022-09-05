using System;
using Corund.Engine;
using Corund.Frames;
using Corund.Tools.Helpers;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Visuals.UI;

/// <summary>
/// A window that allows scrolling its content freely.
/// </summary>
public class ScrollView: ScrollViewBase
{
    #region Constants

    /// <summary>
    /// Coefficient for slowing down the scroll.
    /// </summary>
    protected const float FRICTION = 6f;

    /// <summary>
    /// Minimum squared distance of a swipe to consider the scroll inertial.
    /// </summary>
    protected const float MIN_INERTIAL_DISTANCE = 10;

    #endregion

    #region Constructor

    public ScrollView(int width, int height, ScrollDirection dir = ScrollDirection.Vertical)
        : base(width, height, dir)
    {
    }

    #endregion

    #region Fields

    private Vector2 _scrollSpeed;

    #endregion

    #region Properties

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

    protected override void OnTouchPressed(TouchLocation touch)
    {
        if (!_scrollSpeed.LengthSquared().IsAlmostZero())
        {
            _isCaptured = true;
            GameEngine.Touch.Capture(touch, this);
        }

        _scrollSpeed = Vector2.Zero;
    }

    protected override void OnTouchReleased(TouchLocation touch)
    {
        var rawSwipe = LimitDirection(touch.Position - _prevTouch.Value.Position);
        if (rawSwipe.LengthSquared() >= MIN_INERTIAL_DISTANCE)
            _scrollSpeed = rawSwipe;
    }

    public override void Update()
    {
        base.Update();

        if (!_scrollSpeed.LengthSquared().IsAlmostZero())
        {
            _scrollSpeed = _scrollSpeed * (1 - (FRICTION * GameEngine.Delta));
            Content.Position += LimitOffset(Content.Position, _scrollSpeed);

            if (_scrollSpeed.LengthSquared() <= 0.1)
                _scrollSpeed = Vector2.Zero;
        }
    }

    #endregion
}