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
        /// The game works at actual screen size.
        /// </summary>
        Adjust
    }
}
