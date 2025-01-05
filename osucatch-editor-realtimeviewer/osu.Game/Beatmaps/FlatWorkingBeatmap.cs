// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps.Formats;
using osu.Game.IO;

namespace osu.Game.Beatmaps
{
    /// <summary>
    /// A <see cref="WorkingBeatmap"/> which can be constructed directly from an .osu file (via <see cref="FlatWorkingBeatmap(string, int?)"/>)
    /// or an <see cref="IBeatmap"/> instance (via <see cref="FlatWorkingBeatmap(IBeatmap)"/>,
    /// providing an implementation for
    /// <see cref="WorkingBeatmap.GetPlayableBeatmap(osu.Game.Rulesets.IRulesetInfo,System.Collections.Generic.IReadOnlyList{osu.Game.Rulesets.Mods.Mod})"/>.
    /// </summary>
    public class FlatWorkingBeatmap : WorkingBeatmap
    {
        private readonly IBeatmap beatmap;

        public FlatWorkingBeatmap(IBeatmap beatmap)
            : base(beatmap.BeatmapInfo)
        {
            this.beatmap = beatmap;
        }

        protected override IBeatmap GetBeatmap() => beatmap;
    }
}
