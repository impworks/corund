﻿using System;
using System.IO;
using System.Reflection;
using Corund.Engine.Config;

namespace Corund.Platform.Android.Content
{
    /// <summary>
    /// IContentProvider implementation for Android.
    /// </summary>
    public class AndroidContentProvider: IContentProvider
    {
        #region Constants

        /// <summary>
        /// Assembly-specific prefix for all resource names.
        /// </summary>
        private const string RESOURCE_PREFIX = "Corund.Platform.Android.Content.Corund.Content";

        #endregion

        #region Constructor

        public AndroidContentProvider()
        {
            _assembly = typeof(AndroidContentProvider).GetTypeInfo().Assembly;
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
            var stream = _assembly.GetManifestResourceStream($@"{RESOURCE_PREFIX}.{name.Replace('/', '.')}.xnb");

            if(stream == null)
                throw new ArgumentException($"Embedded resource '{name}' is not available on this platform!");

            return stream;
        }
    }
}
