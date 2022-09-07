using System;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Visuals.UI;

/// <summary>
/// A window that allows scrolling its content per page.
/// </summary>
public class PagedScrollView: ScrollViewBase
{
    #region Constants

    /// <summary>
    /// Minimum distance to either side required for a page flip.
    /// </summary>
    private const int MIN_SCROLL_DISTANCE = 50;

    #endregion

    #region Constructor

    public PagedScrollView(int width, int height, ScrollDirection dir = ScrollDirection.Horizontal)
        : base(width, height, dir)
    {
        if (_direction == ScrollDirection.All)
            throw new ArgumentOutOfRangeException(nameof(dir), "Only Vertical or Horizontal scroll is supported.");

        _content = _contentGroup = new ObjectGroup();
        Attach(_contentGroup);
    }

    #endregion

    #region Fields

    private int _page;
    private ObjectGroup _contentGroup;

    #endregion

    #region Properties

    /// <summary>
    /// Total number of pages available.
    /// </summary>
    public int PageCount => _contentGroup.Count;

    /// <summary>
    /// Currently selected page (0-based).
    /// </summary>
    public int Page
    {
        get => _page;
        set
        {
            _page = value;
            _contentGroup.Position = -GetPageOffset(value, center: false);
        }
    }

    #endregion

    #region Overrides

    protected override void OnTouchReleased(TouchLocation touch)
    {
        var drag = LimitDirection(touch.Position - _origTouch.Value.Position);
        var delta = _direction == ScrollDirection.Horizontal ? drag.X : drag.Y;
        var page = Page + (Math.Abs(delta) < MIN_SCROLL_DISTANCE ? 0 : delta < 0 ? 1 : -1);
            
        if (page >= 0 && page < PageCount)
            ScrollToPage(page);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a new page to the end of the scroll.
    /// </summary>
    public void Add(ObjectBase obj)
    {
        _contentGroup.Position = Vector2.Zero;
        _contentGroup.Add(obj);
        _contentSize = GetContentSize();
        obj.Position = GetPageOffset(_contentGroup.Count - 1, center: true);
    }

    /// <summary>
    /// Replaces the page's contents.
    /// </summary>
    public void Set(int id, ObjectBase obj)
    {
        _contentGroup[id] = obj;
        obj.Position = GetPageOffset(id, center: true);
    }

    /// <summary>
    /// Inserts a new page at the specified index.
    /// </summary>
    public void Insert(ObjectBase obj, int id)
    {
        _contentGroup.Insert(obj, id);
        obj.Position = GetPageOffset(id, center: true);
        _contentSize = GetContentSize();
    }

    /// <summary>
    /// Removes the specified page.
    /// </summary>
    public void Remove(int id)
    {
        _contentGroup.RemoveAt(id);
        _contentSize = GetContentSize();
        for (var idx = id; idx < _contentGroup.Count; idx++)
            _contentGroup[idx].Position = GetPageOffset(idx, center: true);
    }

    /// <summary>
    /// Returns the ID of the page on which an object is located.
    /// </summary>
    public int GetObjectPage(ObjectBase obj)
    {
        for (var i = 0; i < _contentGroup.Count; i++)
        {
            var curr = _contentGroup[i];
            if (ReferenceEquals(curr, obj))
                return i;
        }

        throw new ArgumentException("Object is not a descendant of this view!");
    }

    /// <summary>
    /// Scrolls to a page smoothly.
    /// </summary>
    public void ScrollToPage(int page, float time = 0.25f)
    {
        var pos = -GetPageOffset(page, center: false);

        _isDisabled = true;
        _contentGroup.Tween(Property.Position, pos, time, Interpolate.EaseOutMedium);
        GameEngine.Current.Timeline.Add(time, () =>
        {
            _page = page;
            _isDisabled = false;
        });
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Returns the position for a center of the page.
    /// </summary>
    private Vector2 GetPageOffset(int page, bool center)
    {
        page = MathHelper.Clamp(page, 0, PageCount - 1);

        var pos = _direction == ScrollDirection.Horizontal
            ? new Vector2(ViewSize.X * page, 0)
            : new Vector2(0, ViewSize.Y * page);

        if (center)
            pos += ViewSize / 2;

        return pos;
    }

    /// <summary>
    /// Returns the updated content size.
    /// </summary>
    private Vector2 GetContentSize()
    {
        var size = ViewSize;
        var pages = _contentGroup.Count;

        if (_direction == ScrollDirection.Horizontal)
            size.X *= pages;
        else
            size.Y *= pages;

        return size;
    }

    #endregion
}