using System;
using System.Diagnostics;

namespace Corund.Engine;

public partial class TimelineManager
{
    [DebuggerDisplay("Record: #{RecordId} ({Time})")]
    private class TimelineRecord
    {
        /// <summary>
        /// The record's unique ID.
        /// </summary>
        public int RecordId;

        /// <summary>
        /// Action to execute.
        /// </summary>
        public Action Action;

        /// <summary>
        /// Action's desired time.
        /// </summary>
        public float Time;
    }
}