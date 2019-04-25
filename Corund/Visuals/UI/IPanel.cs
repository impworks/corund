namespace Corund.Visuals.UI
{
    /// <summary>
    /// Interface for panels that align UI objects.
    /// </summary>
    public interface IPanel
    {
        /// <summary>
        /// Force the recalculation of a layout.
        /// </summary>
        void RefreshLayout();
    }
}
