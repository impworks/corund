﻿using System.Collections.Generic;
using Corund.Engine;
using Corund.Geometry;
using Corund.Visuals.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Corund.Tools.Helpers;

/// <summary>
/// Helper methods for working with geometry.
/// </summary>
public static class GeometryObjectHelper
{
    #region Bounds

    /// <summary>
    /// Returns the bounding box for the sprite.
    /// </summary>
    public static Rectangle GetBoundingBox(this IGeometryObject obj, bool toScreen = false)
    {
        return obj.Geometry.GetBoundingBox(obj.GetTransformInfo(toScreen));
    }

    #endregion

    #region Collision detection

    /// <summary>
    /// Checks if two objects overlap via their geometries.
    /// </summary>
    public static bool Overlaps(this IGeometryObject obj, IGeometryObject other)
    {
        if (obj.Geometry == null || other.Geometry == null)
            return false;

        var transform = obj.GetTransformInfo(false);
        var otherTransform = other.GetTransformInfo(false);

        return obj.Geometry.Overlaps(other.Geometry, transform, otherTransform);
    }

    #endregion

    #region Bounds

    /// <summary>
    /// Checks if the object is completely inside bounds.
    /// </summary>
    public static bool IsInside(this IGeometryObject obj, Rectangle bounds)
    {
        return obj.Geometry?.IsInsideBounds(bounds, obj.GetTransformInfo(false)) ?? false;
    }

    /// <summary>
    /// Checks if the object is completely outside bounds.
    /// </summary>
    public static bool IsOutside(this IGeometryObject obj, Rectangle bounds)
    {
        return obj.Geometry?.IsOutsideBounds(bounds, obj.GetTransformInfo(false)) ?? false;
    }

    /// <summary>
    /// Checks if the current object is completely inside the frame.
    /// </summary>
    public static bool IsInsideFrame(this IGeometryObject obj)
    {
        return obj.IsInside(GameEngine.Current.Frame.Bounds);
    }

    /// <summary>
    /// Checks if the current object is completely outside the frame.
    /// </summary>
    public static bool IsOutsideFrame(this IGeometryObject obj)
    {
        return obj.IsOutside(GameEngine.Current.Frame.Bounds);
    }

    /// <summary>
    /// Checks if the object crosses the specified side of the bounds.
    /// </summary>
    public static bool CrossesBounds(this IGeometryObject obj, Rectangle bounds, RectSide side)
    {
        return obj.Geometry?.CrossesBounds(bounds, side, obj.GetTransformInfo(false), null, null) ?? false;
    }

    /// <summary>
    /// Checks if the object crosses the specified side of the frame.
    /// </summary>
    public static bool CrossesFrameBounds(this IGeometryObject obj, RectSide side)
    {
        return obj.Geometry?.CrossesBounds(GameEngine.Current.Frame.Bounds, side, obj.GetTransformInfo(false), null, null) ?? false;
    }

    /// <summary>
    /// Checks if the object moves out of the bounds on the given side.
    /// </summary>
    public static bool LeavesBounds<T>(this T obj, Rectangle bounds, RectSide side)
        where T : MovingObject, IGeometryObject
    {
        return obj.Geometry?.CrossesBounds(bounds, side, obj.GetTransformInfo(), true, obj.Momentum) ?? false;
    }

    /// <summary>
    /// Checks if the object moves into the bounds on the given side.
    /// </summary>
    public static bool EntersBounds<T>(this T obj, Rectangle bounds, RectSide side)
        where T : MovingObject, IGeometryObject
    {
        return obj.Geometry?.CrossesBounds(bounds, side, obj.GetTransformInfo(), false, obj.Momentum) ?? false;
    }

    /// <summary>
    /// Checks if the object moves out of the frame bounds on the given side.
    /// </summary>
    public static bool LeavesFrameBounds<T>(this T obj, RectSide side)
        where T : MovingObject, IGeometryObject
    {
        return obj.Geometry?.CrossesBounds(GameEngine.Current.Frame.Bounds, side, obj.GetTransformInfo(), true, obj.Momentum) ?? false;
    }

    /// <summary>
    /// Checks if the object moves into the frame bounds on the given side.
    /// </summary>
    public static bool EntersFrameBounds<T>(this T obj, RectSide side)
        where T : MovingObject, IGeometryObject
    {
        return obj.Geometry?.CrossesBounds(GameEngine.Current.Frame.Bounds, side, obj.GetTransformInfo(), false, obj.Momentum) ?? false;
    }

    #endregion

    #region Touch

    /// <summary>
    /// Attempts to get a touch location for current object.
    /// </summary>
    /// <param name="obj">Object with a geometry definition.</param>
    /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
    public static TouchLocation? TryGetTouch(this IGeometryObject obj, bool tapThrough = false)
    {
        return GameEngine.Touch.TryGetTouch(obj, tapThrough);
    }

    /// <summary>
    /// Attempts to get all touch locations for current object.
    /// </summary>
    /// <param name="obj">Object with a geometry definition.</param>
    /// <param name="tapThrough">Flag indicating that touch should be available for underlying objects as well.</param>
    public static IList<TouchLocation> TryGetTouches(this IGeometryObject obj, bool tapThrough = false)
    {
        if (obj.Geometry == null)
            return null;

        List<TouchLocation> result = null;
        var transform = obj.GetTransformInfo(false);
        foreach (var touch in GameEngine.Touch.LocalTouches)
        {
            if (!obj.Geometry.ContainsPoint(touch.Position, transform))
                continue;

            if (!tapThrough)
                GameEngine.Touch.Handle(touch, obj);

            result ??= new List<TouchLocation>();
            result.Add(touch);
        }

        return result;
    }

    /// <summary>
    /// Checks if the object has an active touch with the specified state.
    /// </summary>
    public static bool HasTouch(this IGeometryObject obj, TouchLocationState? state = TouchLocationState.Pressed, bool tapThrough = false)
    {
        return obj.TryGetTouch(tapThrough) is {State: var s} && (state == null || state == s);
    }

    #endregion
}