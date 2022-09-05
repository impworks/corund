using System.IO;

namespace Corund.Engine.Config
{
    /// <summary>
    /// Platform-specific resource provider interface.
    /// </summary>
    public interface IContentProvider
    {
        /// <summary>
        /// Returns a resource stream.
        /// </summary>
        Stream GetResource(string name);
    }
}
