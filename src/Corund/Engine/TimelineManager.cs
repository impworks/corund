using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Corund.Engine;

/// <summary>
/// A class that can execute events at given points of time.
/// </summary>
public class TimelineManager
{
    #region Constructor

    public TimelineManager()
    {
        _keyFrames = new List<TimelineRecord>();
    }

    #endregion

    #region Fields

    /// <summary>
    /// Flag indicating that the manager is currently on hold and no events are processed.
    /// </summary>
    public bool IsPaused;

    /// <summary>
    /// Checks if the timeline has finished.
    /// </summary>
    public bool Finished => _keyFrames.Count == 0;

    /// <summary>
    /// The current time of the timeline.
    /// </summary>
    public float CurrentTime { get; private set; }

    /// <summary>
    /// The list of events to happen in time.
    /// </summary>
    private readonly List<TimelineRecord> _keyFrames;

    #endregion

    #region Methods

    /// <summary>
    /// Add a keyframe to the list.
    /// </summary>
    /// <returns>Event handle: pass to Remove() to cancel the keyframe.</returns>
    public TimelineRecord Add(float time, Action action)
    {
        var record = new TimelineRecord(action, CurrentTime + time);

        for (var idx = 0; idx < _keyFrames.Count; idx++)
        {
            var curr = _keyFrames[idx];
            if (curr.Time > record.Time)
            {
                _keyFrames.Insert(idx, record);
                return record;
            }
        }

        // the current item is the last
        _keyFrames.Add(record);
        return record;
    }

    /// <summary>
    /// Remove a keyframe by it's ID.
    /// </summary>
    public void Remove(TimelineRecord record)
    {
        if(record != null)
            _keyFrames.Remove(record);
    }

    /// <summary>
    /// Update the timeline and execute current action.
    /// </summary>
    public void Update()
    {
        if (IsPaused)
            return;

        CurrentTime += GameEngine.Delta;

        if (Finished)
            return;

        int idx;
        var count = _keyFrames.Count;
        for (idx = 0; idx < count; idx++)
        {
            var curr = _keyFrames[idx];
            if (curr.Time <= CurrentTime)
                curr.Action();
            else
                break;
        }

        if (idx > 0)
            _keyFrames.RemoveRange(0, idx);
    }

    #endregion

    #region TimelineRecord

    [DebuggerDisplay("TimelineRecord ({Time})")]
    public record TimelineRecord(Action Action, float Time);

    #endregion
}