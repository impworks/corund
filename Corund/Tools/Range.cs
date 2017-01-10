namespace Corund.Tools
{
    /// <summary>
    /// A range of float values.
    /// </summary>
    public struct Range
    {
        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public readonly float Min;
        public readonly float Max;
    }
}
