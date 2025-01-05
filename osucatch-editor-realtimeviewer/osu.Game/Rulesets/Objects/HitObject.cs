// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Framework.Extensions.ListExtensions;
using osu.Framework.Lists;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects.Types;


namespace osu.Game.Rulesets.Objects
{
    /// <summary>
    /// A HitObject describes an object in a Beatmap.
    /// <para>
    /// HitObjects may contain more properties for which you should be checking through the IHas* types.
    /// </para>
    /// </summary>
    public class HitObject
    {
        /// <summary>
        /// A small adjustment to the start time of control points to account for rounding/precision errors.
        /// </summary>
        private const double control_point_leniency = 1;

        /// <summary>
        /// Invoked after <see cref="ApplyDefaults"/> has completed on this <see cref="HitObject"/>.
        /// </summary>
        // TODO: This has no implicit unbind flow. Currently, if a Playfield manages HitObjects it will leave a bound event on this and cause the
        // playfield to remain in memory.
        public event Action<HitObject> DefaultsApplied;

        /// <summary>
        /// The time at which the HitObject starts.
        /// </summary>
        public double StartTime = 0;

        /// <summary>
        /// Whether this <see cref="HitObject"/> is in Kiai time.
        /// </summary>

        public bool Kiai { get; private set; }

        /// <summary>
        /// The hit windows for this <see cref="HitObject"/>.
        /// </summary>


        private readonly List<HitObject> nestedHitObjects = new List<HitObject>();


        public SlimReadOnlyListWrapper<HitObject> NestedHitObjects => nestedHitObjects.AsSlimReadOnly();

        /// <summary>
        /// Applies default values to this HitObject.
        /// </summary>
        /// <param name="controlPointInfo">The control points.</param>
        /// <param name="difficulty">The difficulty settings to use.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void ApplyDefaults(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty, CancellationToken cancellationToken = default)
        {
            ApplyDefaultsToSelf(controlPointInfo, difficulty);

            nestedHitObjects.Clear();

            CreateNestedHitObjects(cancellationToken);

            if (this is IHasComboInformation hasCombo)
            {
                foreach (HitObject hitObject in nestedHitObjects)
                {
                    if (hitObject is IHasComboInformation n)
                    {
                        n.ComboIndex = hasCombo.ComboIndex;
                        n.ComboIndexWithOffsets = hasCombo.ComboIndexWithOffsets;
                        n.IndexInCurrentCombo = hasCombo.IndexInCurrentCombo;
                    }
                }
            }

            nestedHitObjects.Sort((h1, h2) => h1.StartTime.CompareTo(h2.StartTime));

            foreach (var h in nestedHitObjects)
                h.ApplyDefaults(controlPointInfo, difficulty, cancellationToken);

            DefaultsApplied?.Invoke(this);
        }

        protected virtual void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, IBeatmapDifficultyInfo difficulty)
        {
            Kiai = controlPointInfo.EffectPointAt(StartTime + control_point_leniency).KiaiMode;
        }

        protected virtual void CreateNestedHitObjects(CancellationToken cancellationToken)
        {
        }

        protected void AddNested(HitObject hitObject) => nestedHitObjects.Add(hitObject);


    }

    public static class HitObjectExtensions
    {
        /// <summary>
        /// Returns the end time of this object.
        /// </summary>
        /// <remarks>
        /// This returns the <see cref="IHasDuration.EndTime"/> where available, falling back to <see cref="HitObject.StartTime"/> otherwise.
        /// </remarks>
        /// <param name="hitObject">The object.</param>
        /// <returns>The end time of this object.</returns>
        public static double GetEndTime(this HitObject hitObject) => (hitObject as IHasDuration)?.EndTime ?? hitObject.StartTime;
    }
}
