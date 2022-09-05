using Corund.Engine.Config;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Corund.Engine;

/// <summary>
/// IContentProvider implementation that returns data from an embedded resource stream.
/// </summary>
public class EmbeddedContentProvider : IContentProvider
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
    public Stream GetResource(string name)
    {
        var resources = _assembly.GetManifestResourceNames();
        foreach(var r in resources)
            Debug.WriteLine(r);

        var stream = _assembly.GetManifestResourceStream($@"{_prefix}.{name.Replace('/', '.')}.xnb");

        if (stream == null)
            throw new ArgumentException($"Embedded resource '{name}' is not available on this platform!");

        return stream;
    }
}