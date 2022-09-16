using System;
using System.Collections.Generic;

namespace Corund.Geometry;

/// <summary>
/// A collection of methods to detect collisions between geometry objects.
/// </summary>
public static class GeometryHelper
{
    #region Geometry combination

    /// <summary>
    /// Combines geometries into one flat group.
    /// </summary>
    public static GeometryRectGroup Flatten(params IGeometry[] items)
    {
        var result = new List<GeometryRect>(items.Length);

        foreach (var item in items)
        {
            if (item is GeometryRect rect)
                result.Add(rect);
            else if (item is GeometryRectGroup group)
                result.AddRange(group.Rectangles);
            else
                throw new ArgumentException($"Unknown item type: '{item.GetType()}'");
        }

        return new GeometryRectGroup(result);
    }

    #endregion
}