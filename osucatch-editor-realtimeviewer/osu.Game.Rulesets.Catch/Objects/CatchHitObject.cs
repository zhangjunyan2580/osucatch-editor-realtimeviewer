// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Catch.UI;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Legacy;
using osu.Game.Rulesets.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Catch.Objects
{
    public abstract class CatchHitObject : HitObject, IHasPosition, IHasComboInformation, IHasTimePreempt
    {
        public const float OBJECT_RADIUS = 64;

        private float originalX;

        /// <summary>
        /// The horizontal position of the hit object between 0 and <see cref="CatchPlayfield.WIDTH"/>.
        /// </summary>
        /// <remarks>
        /// Only setter is exposed.
        /// Use <see cref="OriginalX"/> or <see cref="EffectiveX"/> to get the horizontal position.
        /// </remarks>
        public float X
        {
            set => originalX = value;
        }

        private float xOffset;

        /// <summary>
        /// A random offset applied to the horizontal position, set by the beatmap processing.
        /// </summary>
        public float XOffset
        {
            get => xOffset;
            set => xOffset = value;
        }

        /// <summary>
        /// The horizontal position of the hit object between 0 and <see cref="CatchPlayfield.WIDTH"/>.
        /// </summary>
        /// <remarks>
        /// This value is the original <see cref="X"/> value specified in the beatmap, not affected by the beatmap processing.
        /// Use <see cref="EffectiveX"/> for a gameplay.
        /// </remarks>
        public float OriginalX
        {
            get => originalX;
            set => originalX = value;
        }

        /// <summary>
        /// The effective horizontal position of the hit object between 0 and <see cref="CatchPlayfield.WIDTH"/>.
        /// </summary>
        /// <remarks>
        /// This value is the original <see cref="X"/> value plus the offset applied by the beatmap processing.
        /// Use <see cref="OriginalX"/> if a value not affected by the offset is desired.
        /// </remarks>
        public float EffectiveX => Math.Clamp(OriginalX + XOffset, 0, CatchPlayfield.WIDTH);

        public double TimePreempt { get; set; } = 1000;

        private int indexInBeatmap;

        public int IndexInBeatmap
        {
            get => indexInBeatmap;
            set => indexInBeatmap = value;
        }

        public virtual bool NewCombo { get; set; }

        public int ComboOffset { get; set; }

        private int indexInCurrentCombo;

        public int IndexInCurrentCombo
        {
            get => indexInCurrentCombo;
            set => indexInCurrentCombo = value;
        }

        private int comboIndex;

        public int ComboIndex
        {
            get => comboIndex;
            set => comboIndex = value;
        }

        private int comboIndexWithOffsets;

        public int ComboIndexWithOffsets
        {
            get => comboIndexWithOffsets;
            set => comboIndexWithOffsets = value;
        }

        private bool lastInCombo;

        /// <summary>
        /// The next fruit starts a new combo. Used for explodey.
        /// </summary>
        public virtual bool LastInCombo
        {
            get => lastInCombo;
            set => lastInCombo = value;
        }

        private float scale = 1;

        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        /// <summary>
        /// The seed value used for visual randomness such as fruit rotation.
        /// The value is <see cref="HitObject.StartTime"/> truncated to an integer.
        /// </summary>
        public int RandomSeed => (int)StartTime;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = (float)IBeatmapDifficultyInfo.DifficultyRange(difficulty.ApproachRate, PREEMPT_MAX, PREEMPT_MID, PREEMPT_MIN);

            Scale = LegacyRulesetExtensions.CalculateScaleFromCircleSize(difficulty.CircleSize);
        }

        public void UpdateComboInformation(IHasComboInformation? lastObj)
        {
            // Note that this implementation is shared with the osu! ruleset's implementation.
            // If a change is made here, OsuHitObject.cs should also be updated.
            ComboIndex = lastObj?.ComboIndex ?? 0;
            ComboIndexWithOffsets = lastObj?.ComboIndexWithOffsets ?? 0;
            IndexInCurrentCombo = (lastObj?.IndexInCurrentCombo + 1) ?? 0;

            if (this is BananaShower)
            {
                // For the purpose of combo colours, spinners never start a new combo even if they are flagged as doing so.
                return;
            }

            // At decode time, the first hitobject in the beatmap and the first hitobject after a banana shower are both enforced to be a new combo,
            // but this isn't directly enforced by the editor so the extra checks against the last hitobject are duplicated here.
            if (NewCombo || lastObj == null || lastObj is BananaShower)
            {
                IndexInCurrentCombo = 0;
                ComboIndex++;
                ComboIndexWithOffsets += ComboOffset + 1;

                if (lastObj != null)
                    lastObj.LastInCombo = true;
            }
        }

        #region Hit object conversion

        // The half of the height of the osu! playfield.
        public const float DEFAULT_LEGACY_CONVERT_Y = 192;

        /// <summary>
        /// Minimum preempt time at AR=10.
        /// </summary>
        public const double PREEMPT_MIN = 450;

        /// <summary>
        /// Median preempt time at AR=5.
        /// </summary>
        public const double PREEMPT_MID = 1200;

        /// <summary>
        /// Maximum preempt time at AR=0.
        /// </summary>
        public const double PREEMPT_MAX = 1800;

        /// <summary>
        /// The Y position of the hit object is not used in the normal osu!catch gameplay.
        /// It is preserved to maximize the backward compatibility with the legacy editor, in which the mappers use the Y position to organize the patterns.
        /// </summary>
        public float LegacyConvertedY { get; set; } = DEFAULT_LEGACY_CONVERT_Y;

        float IHasXPosition.X => OriginalX;

        float IHasYPosition.Y => LegacyConvertedY;

        Vector2 IHasPosition.Position => new Vector2(OriginalX, LegacyConvertedY);

        #endregion
    }
}
