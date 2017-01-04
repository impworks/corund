using System;

namespace Corund.Managers
{
    public partial class TimelineManager
    {
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
}
