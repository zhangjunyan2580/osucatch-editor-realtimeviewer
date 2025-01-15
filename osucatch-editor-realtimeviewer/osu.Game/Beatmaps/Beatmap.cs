// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using OpenTK.Graphics;
using osu.Framework.Lists;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Beatmaps.Timing;
using osu.Game.Rulesets.Objects;

namespace osu.Game.Beatmaps
{
    public class Beatmap<T> : IBeatmap<T>
        where T : HitObject
    {
        private BeatmapDifficulty difficulty = new BeatmapDifficulty();

        public BeatmapDifficulty Difficulty
        {
            get => difficulty;
            set
            {
                difficulty = value;

                beatmapInfo.Difficulty = difficulty.Clone();
            }
        }

        private BeatmapInfo beatmapInfo;

        public BeatmapInfo BeatmapInfo
        {
            get => beatmapInfo;
            set
            {
                beatmapInfo = value;

                Difficulty = beatmapInfo.Difficulty.Clone();
            }
        }

        public Beatmap()
        {
            beatmapInfo = new BeatmapInfo
            {
                Metadata = new BeatmapMetadata
                {
                    Artist = @"Unknown",
                    Title = @"Unknown",
                    Author = @"Unknown Creator",
                },
                DifficultyName = @"Normal",
                Difficulty = Difficulty,
            };
        }


        public BeatmapMetadata Metadata => BeatmapInfo.Metadata;

        public ControlPointInfo ControlPointInfo { get; set; } = new ControlPointInfo();

        public List<Color4> CustomComboColours { get; set; } = new List<Color4>();

        public List<BarLine> BarLines { get; set; } = new List<BarLine>();

        public List<T> HitObjects { get; set; } = new List<T>();

        IReadOnlyList<T> IBeatmap<T>.HitObjects => HitObjects;

        IReadOnlyList<HitObject> IBeatmap.HitObjects => HitObjects;

        public int[] Bookmarks { get; set; } = Array.Empty<int>();

        IBeatmap IBeatmap.Clone() => Clone();

        public Beatmap<T> Clone() => (Beatmap<T>)MemberwiseClone();
    }

    public class Beatmap : Beatmap<HitObject>
    {
    }
}
