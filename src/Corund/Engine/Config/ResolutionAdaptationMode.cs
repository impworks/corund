namespace Corund.Engine.Config
{
    /// <summary>
    /// Provides a way to handle various screen sizes.
    /// </summary>
    public enum ResolutionAdaptationMode
    {
        /// <summary>
        /// The game works at desired screen size. It is then centered in post-processing stage.
        /// </summary>
        Center,

        /// <summary>
        /// The game works at desired screen size. Then it is resized to fit the screen in post-processing state.
        /// If the screen's ratio does not match scene ratio, black padding is added.
        /// </summary>
        CenterAndResize,

        /// <summary>
        /// The game works at actual screen size.
        /// </summary>
        Adjust
    }
}
