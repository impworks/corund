namespace Corund.Behaviours.Fade;

public enum FadeStyle
{
    /// <summary>
    /// Fade in: object grows.
    /// Fade out: object shrinks.
    /// </summary>
    Zoom,

    /// <summary>
    /// Fade in: objects becomes opaque.
    /// Fade out: object becomes transparent.
    /// </summary>
    Fade,

    /// <summary>
    /// Fade in: object grows and becomes opaque.
    /// Fade out: object shrinks and becomes transparent.
    /// </summary>
    ZoomAndFade,

    /// <summary>
    /// Fade in: object shrinks from double-size to normal and becomes opaque.
    /// Fade out: object grows to double size and becomes transparent.
    /// </summary>
    InverseZoomAndFade
}