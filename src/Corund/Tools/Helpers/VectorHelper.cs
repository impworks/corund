﻿using System;
using Corund.Tools.UI;
using Microsoft.Xna.Framework;

namespace Corund.Tools.Helpers;

/// <summary>
/// A collection of helper methods for vectors.
/// </summary>
public static class VectorHelper
{
    /// <summary>
    /// Rotates the vector around its origin point by given amount of degrees.
    /// </summary>
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        if (angle.IsAlmostZero())
            return vector;

        var x = vector.X;
        var y = vector.Y;
        var distance = MathF.Sqrt(x*x + y*y);
        var newAngle = MathF.Atan2(y, x) + angle;

        return FromLength(distance, newAngle);
    }

    /// <summary>
    /// Creates a vector by angle and direction.
    /// </summary>
    public static Vector2 FromLength(float length, float angle)
    {
        return new Vector2(
            MathF.Cos(angle) * length,
            MathF.Sin(angle) * length
        );
    }

    /// <summary>
    /// Calculates the direction between two points.
    /// </summary>
    public static float AngleTo(this Vector2 p1, Vector2 p2)
    {
        var vec = p2 - p1;
        return MathF.Atan2(vec.Y, vec.X);
    }

    /// <summary>
    /// Limits the vector size within two vectors.
    /// </summary>
    public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
    {
        return new Vector2(
            MathHelper.Clamp(value.X, min.X, max.X),
            MathHelper.Clamp(value.Y, min.Y, max.Y)
        );
    }

    /// <summary>
    /// Returns the vector size as rectangle.
    /// </summary>
    public static Vector2 GetSize(this Rectangle rect)
    {
        return new Vector2(rect.Width, rect.Height);
    }

    /// <summary>
    /// Gets the vector with coordinates either 0, 0.5 or 1 depending on alignments.
    /// </summary>
    public static Vector2 GetAlignmentVector(HorizontalAlignment halign, VerticalAlignment valign)
    {
        return new Vector2(
            (int)halign * 0.5f,
            (int)valign * 0.5f
        );
    }
}