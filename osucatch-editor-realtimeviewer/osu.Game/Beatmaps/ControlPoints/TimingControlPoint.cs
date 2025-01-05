// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Game.Beatmaps.Timing;

namespace osu.Game.Beatmaps.ControlPoints
{
    public class TimingControlPoint : ControlPoint, IEquatable<TimingControlPoint>
    {
        /// <summary>
        /// Default length of a beat in milliseconds. Used whenever there is no beatmap or track playing.
        /// </summary>
        private const double default_beat_length = 60000.0 / 60.0;

        public static readonly TimingControlPoint DEFAULT = new TimingControlPoint
        {
        };

        private TimeSignature timeSignature = TimeSignature.SimpleQuadruple;

        /// <summary>
        /// The time signature at this control point.
        /// </summary>
        public TimeSignature TimeSignature
        {
            get => timeSignature;
            set => timeSignature = value;
        }

        private bool omitFirstBarLine = false;

        /// <summary>
        /// Whether the first bar line of this control point is ignored.
        /// </summary>
        public bool OmitFirstBarLine
        {
            get => omitFirstBarLine;
            set => omitFirstBarLine = value;
        }

        public const double DEFAULT_BEAT_LENGTH = 1000;

        private double beatLength = DEFAULT_BEAT_LENGTH;

        /// <summary>
        /// The beat length at this control point.
        /// </summary>
        public double BeatLength
        {
            get => beatLength;
            set => beatLength = Math.Clamp(value, 6, 60000);
        }

        /// <summary>
        /// The BPM at this control point.
        /// </summary>
        public double BPM => 60000 / BeatLength;

        public TimingControlPoint()
        {
        }

        // Timing points are never redundant as they can change the time signature.
        public override bool IsRedundant(ControlPoint? existing) => false;

        public override void CopyFrom(ControlPoint other)
        {
            TimeSignature = ((TimingControlPoint)other).TimeSignature;
            OmitFirstBarLine = ((TimingControlPoint)other).OmitFirstBarLine;
            BeatLength = ((TimingControlPoint)other).BeatLength;

            base.CopyFrom(other);
        }

        public override bool Equals(ControlPoint? other)
            => other is TimingControlPoint otherTimingControlPoint
               && Equals(otherTimingControlPoint);

        public bool Equals(TimingControlPoint? other)
            => base.Equals(other)
               && TimeSignature.Equals(other.TimeSignature)
               && OmitFirstBarLine == other.OmitFirstBarLine
               && BeatLength.Equals(other.BeatLength);

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), TimeSignature, BeatLength, OmitFirstBarLine);
    }
}
