using System;
using Corund.Visuals.Primitives;

namespace Corund.Shaders;

/// <summary>
/// Interface for shader implementations.
/// </summary>
public interface IShader
{
    /// <summary>
    /// Draws the object's contents onto an intermediate texture.
    /// </summary>
    void DrawWrapper(DynamicObject obj, Action innerDraw);

    /// <summary>
    /// Updates the shader's inner state, if necessary.
    /// </summary>
    void Update();
}