using System.Collections.Generic;
using Corund.Behaviours.Fade;
using Corund.Engine;
using Corund.Tools;
using Corund.Tools.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Corund.Frames;

/// <summary>
/// The manager class for all frames currently running in the game.
/// </summary>
public class FrameManager
{
    #region Constructor

    public FrameManager()
    {
        _frames = new List<FrameBase>();
    }

    #endregion

    #region Fields and properties

    /// <summary>
    /// The list of currently executed frames.
    /// </summary>
    private readonly List<FrameBase> _frames;

    #endregion

    #region Frame stack handling

    /// <summary>
    /// Adds a frame to the top of current stack.
    /// </summary>
    public void Add(FrameBase frame)
    {
        _frames.Add(frame);
    }

    /// <summary>
    /// Removes the frame from current stack.
    /// </summary>
    public void Remove(FrameBase frame)
    {
        _frames.Remove(frame);
        frame.Dispose();
    }

    /// <summary>
    /// Fades the new frame in, replacing all current frames.
    /// </summary>
    public void TransitionTo(FrameBase frame)
    {
        if (_frames.Count == 0)
        {
            Add(frame);
            return;
        }

        var fadeInTimeout = 0f;
        foreach (var behaviour in frame.Behaviours)
        {
            var duration = (behaviour as IFadeInEffect)?.Duration;
            if (duration > fadeInTimeout)
                fadeInTimeout = duration.Value;
        }

        GameEngine.Defer(() => Add(frame));

        foreach (var oldFrame in _frames)
            oldFrame.Timeline.Add(fadeInTimeout, () => oldFrame.RemoveSelf());
    }

    #endregion

    #region Update & draw

    /// <summary>
    /// Updates all frames in the list.
    /// </summary>
    public void Update()
    {
        GameEngine.Current.PauseMode = PauseMode.None;

        // update frames from top to bottom
        for (var idx = _frames.Count - 1; idx >= 0; idx--)
        {
            var curr = GameEngine.Current.Frame = _frames[idx];
            curr.Update();
        }
    }

    /// <summary>
    /// Draws all frames to the device's screen.
    /// </summary>
    public void Draw()
    {
        // pass 1: Render all frames to their respective render targets (bottom to top)
        foreach (var frame in _frames)
        {
            GameEngine.Current.Frame = frame;
            GameEngine.Current.ZOrderFunction = frame.ZOrderFunction;

            frame.Draw();
        }

        // pass 2: draw rendertargets to screen
        using (new RenderContext(null, Color.Black))
        {
            GameEngine.Render.TryBeginBatch(BlendState.AlphaBlend);

            var count = _frames.Count;
            for (var idx = 0; idx < count; idx++)
            {
                var frame = _frames[idx];
                var zOrder = idx / count;
                frame.FinalizeDraw(zOrder);
            }
        }
    }

    #endregion
}