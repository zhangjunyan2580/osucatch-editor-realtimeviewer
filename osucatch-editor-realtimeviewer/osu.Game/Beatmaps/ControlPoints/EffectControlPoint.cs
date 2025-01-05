// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.



namespace osu.Game.Beatmaps.ControlPoints
{
    public class EffectControlPoint : ControlPoint, IEquatable<EffectControlPoint>
    {
        public static readonly EffectControlPoint DEFAULT = new EffectControlPoint
        {
        };

        private double scrollSpeed = 1;

        /// <summary>
        /// The relative scroll speed.
        /// </summary>
        public double ScrollSpeed
        {
            get => scrollSpeed;
            set => scrollSpeed = Math.Clamp(value, 0.01, 10);
        }

        private bool kiaiMode = false;

        /// <summary>
        /// Whether this control point enables Kiai mode.
        /// </summary>
        public bool KiaiMode
        {
            get => kiaiMode;
            set => kiaiMode = value;
        }

        public EffectControlPoint()
        {
        }

        public override bool IsRedundant(ControlPoint? existing)
            => existing is EffectControlPoint existingEffect
               && KiaiMode == existingEffect.KiaiMode
               && ScrollSpeed == existingEffect.ScrollSpeed;

        public override void CopyFrom(ControlPoint other)
        {
            KiaiMode = ((EffectControlPoint)other).KiaiMode;
            ScrollSpeed = ((EffectControlPoint)other).ScrollSpeed;

            base.CopyFrom(other);
        }

        public override bool Equals(ControlPoint? other)
            => other is EffectControlPoint otherEffectControlPoint
               && Equals(otherEffectControlPoint);

        public bool Equals(EffectControlPoint? other)
            => base.Equals(other)
               && ScrollSpeed == other.ScrollSpeed
               && KiaiMode == other.KiaiMode;

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ScrollSpeed, KiaiMode);
    }
}
