using System;

namespace Corund.Visuals.UI;

/// <summary>
/// Allowed directions of the scroll.
/// </summary>
[Flags]
public enum ScrollDirection
{
    Vertical = 1,
    Horizontal = 2,

    All = Vertical | Horizontal
}