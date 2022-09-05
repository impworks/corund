using System;
using System.Collections.Generic;

namespace Corund.Tools.Helpers;

/// <summary>
/// A collection of helper methods for getting random values.
/// </summary>
public class RandomHelper
{
    static RandomHelper()
    {
        _random = new Random();
    }

    /// <summary>
    /// Backing random generator.
    /// </summary>
    private static readonly Random _random;

    /// <summary>
    /// Creates a random number between 0.0 and 1.0.
    /// </summary>
    public static float Float()
    {
        return (float) _random.NextDouble();
    }

    /// <summary>
    /// Creates a random number between two values.
    /// </summary>
    public static float Float(float from, float to)
    {
        var scale = to - from;
        return from + (float)_random.NextDouble() * scale;
    }

    /// <summary>
    /// Creates a random integer in the given range.
    /// </summary>
    public static int Int(int from, int to)
    {
        return _random.Next(from, to);
    }

    /// <summary>
    /// Flips a virtual coin.
    /// </summary>
    public static bool Bool()
    {
        return _random.NextDouble() > 0.5;
    }

    /// <summary>
    /// Random sign: plus or minus.
    /// </summary>
    public static int Sign()
    {
        return _random.NextDouble() > 0.5 ? 1 : -1;
    }

    /// <summary>
    /// Pick a random item from the array.
    /// </summary>
    public static T Pick<T>(params T[] items)
    {
        if (items.Length == 0)
            return default;

        // works better on bigger numbers
        var id = _random.Next(items.Length * 100);
        return items[id / 100];
    }

    /// <summary>
    /// Pick a random item from the list.
    /// </summary>
    public static T Pick<T>(List<T> items)
    {
        if (items.Count == 0)
            return default;

        // works better on bigger numbers
        var id = _random.Next(items.Count * 100);
        return items[id / 100];
    }

    /// <summary>
    /// Shuffles the array or a list.
    /// </summary>
    public static void Shuffle<T>(IList<T> source)
    {
        for (var i = 0; i < source.Count; i++)
        {
            var j = _random.Next(i);
            var tmp = source[i];
            source[i] = source[j];
            source[j] = tmp;
        }
    }
}