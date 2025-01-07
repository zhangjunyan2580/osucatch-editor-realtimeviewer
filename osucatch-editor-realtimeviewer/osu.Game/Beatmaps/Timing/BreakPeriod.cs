// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Beatmaps.Timing
{
    public class BreakPeriod : IComparable<BreakPeriod>
    {
        /// <summary>
        /// The minimum gap between the start of the break and the previous object.
        /// </summary>
        public const double GAP_BEFORE_BREAK = 200;

        /// <summary>
        /// The minimum gap between the end of the break and the next object.
        /// Based on osu! preempt time at AR=10.
        /// See also: https://github.com/ppy/osu/issues/14330#issuecomment-1002158551
        /// </summary>
        public const double GAP_AFTER_BREAK = 450;

        /// <summary>
        /// The minimum duration required for a break to have any effect.
        /// </summary>
        public const double MIN_BREAK_DURATION = 650;

        /// <summary>
        /// The minimum required duration of a gap between two objects such that a break can be placed between them.
        /// </summary>
        public const double MIN_GAP_DURATION = GAP_BEFORE_BREAK + MIN_BREAK_DURATION + GAP_AFTER_BREAK;

        /// <summary>
        /// The break start time.
        /// </summary>
        public double StartTime { get; }

        /// <summary>
        /// The break end time.
        /// </summary>
        public double EndTime { get; }

        /// <summary>
        /// The break duration.
        /// </summary>
        public double Duration => EndTime - StartTime;

        /// <summary>
        /// Constructs a new break period.
        /// </summary>
        /// <param name="startTime">The start time of the break period.</param>
        /// <param name="endTime">The end time of the break period.</param>
        public BreakPeriod(double startTime, double endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public int CompareTo(BreakPeriod? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            int result = StartTime.CompareTo(other.StartTime);
            if (result != 0)
                return result;

            return EndTime.CompareTo(other.EndTime);
        }
    }
}
