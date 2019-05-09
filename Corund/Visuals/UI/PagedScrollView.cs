using System;
using Corund.Engine;
using Corund.Tools.Helpers;
using Corund.Tools.Interpolation;
using Corund.Tools.Properties;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;

namespace Corund.Visuals.UI
{
    /// <summary>
    /// A window that allows scrolling its content per page.
    /// </summary>
    public class PagedScrollView: ScrollViewBase
    {
        #region Constructor

        public PagedScrollView(int width, int height, ScrollDirection dir = ScrollDirection.Vertical)
            : base(width, height, dir)
        {
            if (_direction == ScrollDirection.All)
                throw new ArgumentOutOfRangeException(nameof(dir), "Only Vertical or Horizontal scroll is supported.");

            _content = _contentGroup = new ObjectGroup();
        }

        #endregion

        #region Fields

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
        public int Page { get; set; }

        #endregion

        #region Overrides

        // todo

        #endregion

        #region Methods

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
        public void ScrollToPage(int page, float time = 0.5f)
        {
            page = MathHelper.Clamp(page, 0, PageCount - 1);
            var pos = _direction == ScrollDirection.Horizontal
                ? new Vector2(ViewSize.X * page, 0)
                : new Vector2(0, ViewSize.Y * page);

            _isDisabled = true;
            _contentGroup.Tween(Property.Position, pos, time, Interpolate.EaseBothMedium);
            GameEngine.Current.Timeline.Add(time, () => _isDisabled = false);
        }

        #endregion
    }
}
