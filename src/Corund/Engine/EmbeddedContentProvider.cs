using System;
using System.IO;
using System.Reflection;

namespace Corund.Engine;

/// <summary>
/// IContentProvider implementation that returns data from an embedded resource stream.
/// </summary>
public class EmbeddedContentProvider
{
    #region Constructor

    public EmbeddedContentProvider(Assembly assembly, string prefix)
    {
        _assembly = assembly;
        _prefix = prefix;
    }

    #endregion

    #region Fields

    /// <summary>
    /// Reference to current assembly.
    /// </summary>
    private readonly Assembly _assembly;

    /// <summary>
    /// Assembly-specific prefix for all resource names.
    /// </summary>
    private readonly string _prefix;

    #endregion

    /// <summary>
    /// Returns the stream for specified resource, if any.
    /// </summary>
    public Stream GetResource(string name, string extension = "xnb")
    {
        var stream = _assembly.GetManifestResourceStream($@"{_prefix}.{name.Replace('/', '.')}.{extension}");

        if (stream == null)
            throw new ArgumentException($"Embedded resource '{name}' is not available on this platform!");

        return stream;
    }
}