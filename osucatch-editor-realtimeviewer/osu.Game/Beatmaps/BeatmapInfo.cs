// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets;
using System.Diagnostics;

namespace osu.Game.Beatmaps
{
    /// <summary>
    /// A realm model containing metadata for a single beatmap difficulty.
    /// This should generally include anything which is required to be filtered on at song select, or anything pertaining to storage of beatmaps in the client.
    /// </summary>
    /// <remarks>
    /// There are some legacy fields in this model which are not persisted to realm. These are isolated in a code region within the class and should eventually be migrated to `Beatmap`.
    /// </remarks>
    [Serializable]
    public class BeatmapInfo : IBeatmapInfo, ICloneable
    {
        public Guid ID { get; set; }

        public string DifficultyName { get; set; } = string.Empty;

        public RulesetInfo Ruleset { get; set; } = null!;

        public BeatmapDifficulty Difficulty { get; set; } = null!;

        public BeatmapMetadata Metadata { get; set; } = null!;

        
        public BeatmapInfo(RulesetInfo? ruleset = null, BeatmapDifficulty? difficulty = null, BeatmapMetadata? metadata = null)
        {
            ID = Guid.NewGuid();
            Ruleset = ruleset ?? new RulesetInfo
            {
                OnlineID = 0,
                ShortName = @"osu",
                Name = @"null placeholder ruleset"
            };
            Difficulty = difficulty ?? new BeatmapDifficulty();
            Metadata = metadata ?? new BeatmapMetadata();
        }

        protected BeatmapInfo()
        {
        }

        public int OnlineID { get; set; } = -1;

        public double Length { get; set; }

        public double BPM { get; set; }

        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Defaults to -1 (meaning not-yet-calculated).
        /// Will likely be superseded with a better storage considering ruleset/mods.
        /// </summary>
        public double StarRating { get; set; } = -1;
        public string MD5Hash { get; set; } = string.Empty;

        public int EndTimeObjectCount { get; set; } = -1;

        public int TotalObjectCount { get; set; } = -1;


        public int BeatDivisor { get; set; } = 4;


        IBeatmapMetadataInfo IBeatmapInfo.Metadata => Metadata;
        IRulesetInfo IBeatmapInfo.Ruleset => Ruleset;
        IBeatmapDifficultyInfo IBeatmapInfo.Difficulty => Difficulty;

        #region Compatibility properties

        public int BeatmapVersion;

        public object Clone() => MemberwiseClone();

        #endregion
    }
}
