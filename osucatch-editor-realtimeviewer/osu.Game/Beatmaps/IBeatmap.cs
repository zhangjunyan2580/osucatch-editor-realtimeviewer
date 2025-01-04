// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using OpenTK.Graphics;
using osu.Framework.Lists;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Beatmaps.Timing;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Beatmaps
{
    /// <summary>
    /// A materialised beatmap.
    /// Generally this interface will be implemented alongside <see cref="IBeatmap{T}"/>, which exposes the ruleset-typed hit objects.
    /// </summary>
    public interface IBeatmap
    {
        /// <summary>
        /// This beatmap's info.
        /// </summary>
        BeatmapInfo BeatmapInfo { get; set; }

        /// <summary>
        /// This beatmap's metadata.
        /// </summary>
        BeatmapMetadata Metadata { get; }

        /// <summary>
        /// This beatmap's difficulty settings.
        /// </summary>
        public BeatmapDifficulty Difficulty { get; set; }

        /// <summary>
        /// The control points in this beatmap.
        /// </summary>
        ControlPointInfo ControlPointInfo { get; set; }

        /// <summary>
        /// The breaks in this beatmap.
        /// </summary>
        SortedList<BreakPeriod> Breaks { get; set; }

        /// <summary>
        /// All lines from the [Events] section which aren't handled in the encoding process yet.
        /// These lines should be written out to the beatmap file on save or export.
        /// </summary>
        List<string> UnhandledEventLines { get; }

        List<Color4> CustomComboColours { get; set; }

        List<BarLine> BarLines { get; set; }

        /// <summary>
        /// Total amount of break time in the beatmap.
        /// </summary>
        double TotalBreakTime { get; }

        /// <summary>
        /// The hitobjects contained by this beatmap.
        /// </summary>
        IReadOnlyList<HitObject> HitObjects { get; }

        int[] Bookmarks { get; internal set; }

        /// <summary>
        /// Creates a shallow-clone of this beatmap and returns it.
        /// </summary>
        /// <returns>The shallow-cloned beatmap.</returns>
        IBeatmap Clone();
    }

    /// <summary>
    /// A materialised beatmap containing converted HitObjects.
    /// </summary>
    public interface IBeatmap<out T> : IBeatmap
        where T : HitObject
    {
        /// <summary>
        /// The hitobjects contained by this beatmap.
        /// </summary>
        new IReadOnlyList<T> HitObjects { get; }
    }

    public static class BeatmapExtensions
    {

        /// <summary>
        /// Find the absolute end time of the latest <see cref="HitObject"/> in a beatmap. Will throw if beatmap contains no objects.
        /// </summary>
        /// <remarks>
        /// This correctly accounts for rulesets which have concurrent hitobjects which may have durations, causing the .Last() object
        /// to not necessarily have the latest end time.
        ///
        /// It's not super efficient so calls should be kept to a minimum.
        /// </remarks>
        /// <exception cref="InvalidOperationException">If <paramref name="beatmap"/> has no objects.</exception>
        public static double GetLastObjectTime(this IBeatmap beatmap) => beatmap.HitObjects.Max(h => h.GetEndTime());
    }
}
