﻿using System;
using System.IO;
using System.Reflection;
using Corund.Engine.Config;

namespace Corund.Effects.WP8.Content
{
    /// <summary>
    /// IContentProvider implementation for WP8.
    /// </summary>
    public class WP8ContentProvider: IContentProvider
    {
        #region Constants

        /// <summary>
        /// Assembly-specific prefix for all resource names.
        /// </summary>
        private const string RESOURCE_PREFIX = "Corund.Effects.WP8.Content.Corund.Effects";

        #endregion

        #region Constructor

        public WP8ContentProvider()
        {
            _assembly = typeof(WP8ContentProvider).GetTypeInfo().Assembly;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Reference to current assembly.
        /// </summary>
        private readonly Assembly _assembly;

        #endregion

        /// <summary>
        /// Returns the stream for specified resource, if any.
        /// </summary>
        public Stream GetResource(string name)
        {
            var stream = _assembly.GetManifestResourceStream($@"{RESOURCE_PREFIX}.{name}.xnb");

            if(stream == null)
                throw new ArgumentException($"Embedded resource '{name}' is not available on this platform!");

            return stream;
        }
    }
}
