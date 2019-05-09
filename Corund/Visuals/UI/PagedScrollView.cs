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
        }

        #endregion
    }
}
