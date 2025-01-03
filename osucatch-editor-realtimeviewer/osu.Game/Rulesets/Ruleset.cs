// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Legacy;
using osu.Game.Rulesets.Mods;
using System.Collections.Concurrent;

namespace osu.Game.Rulesets
{
    public abstract class Ruleset
    {

        /// <summary>
        /// Version history:
        /// 2022.205.0   FramedReplayInputHandler.CollectPendingInputs renamed to FramedReplayHandler.CollectReplayInputs.
        /// 2022.822.0   All strings return values have been converted to LocalisableString to allow for localisation support.
        /// </summary>
        public const string CURRENT_RULESET_API_VERSION = "2022.822.0";

        /// <summary>
        /// Define the ruleset API version supported by this ruleset.
        /// Ruleset implementations should be updated to support the latest version to ensure they can still be loaded.
        /// </summary>
        /// <remarks>
        /// Generally, all ruleset implementations should point this directly to <see cref="CURRENT_RULESET_API_VERSION"/>.
        /// This will ensure that each time you compile a new release, it will pull in the most recent version.
        /// See https://github.com/ppy/osu/wiki/Breaking-Changes for full details on required ongoing changes.
        /// </remarks>
        public virtual string RulesetAPIVersionSupported => string.Empty;

        /// <summary>
        /// Creates a <see cref="IBeatmapConverter"/> to convert a <see cref="IBeatmap"/> to one that is applicable for this <see cref="Ruleset"/>.
        /// </summary>
        /// <param name="beatmap">The <see cref="IBeatmap"/> to be converted.</param>
        /// <returns>The <see cref="IBeatmapConverter"/>.</returns>
        public abstract IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap);

        /// <summary>
        /// Optionally creates a <see cref="IBeatmapProcessor"/> to alter a <see cref="IBeatmap"/> after it has been converted.
        /// </summary>
        /// <param name="beatmap">The <see cref="IBeatmap"/> to be processed.</param>
        /// <returns>The <see cref="IBeatmapProcessor"/>.</returns>
        public virtual IBeatmapProcessor? CreateBeatmapProcessor(IBeatmap beatmap) => null;

        public abstract string Description { get; }


        /// <summary>
        /// A unique short name to reference this ruleset in online requests.
        /// </summary>
        public abstract string ShortName { get; }

        /// <summary>
        /// The playing verb to be shown in the <see cref="UserActivity.InGame"/> activities.
        /// </summary>
        public virtual string PlayingVerb => "Playing";

        /// <summary>
        /// Applies changes to difficulty attributes for presenting to a user a rough estimate of how rate adjust mods affect difficulty.
        /// Importantly, this should NOT BE USED FOR ANY CALCULATIONS.
        ///
        /// It is also not always correct, and arguably is never correct depending on your frame of mind.
        /// </summary>
        /// <param name="difficulty">>The <see cref="IBeatmapDifficultyInfo"/> that will be adjusted.</param>
        /// <param name="rate">The rate adjustment multiplier from mods. For example 1.5 for DT.</param>
        /// <returns>The adjusted difficulty attributes.</returns>
        public virtual BeatmapDifficulty GetRateAdjustedDisplayDifficulty(IBeatmapDifficultyInfo difficulty, double rate) => new BeatmapDifficulty(difficulty);

        /// <summary>
        /// Can be overridden to avoid showing scroll speed changes in the editor.
        /// </summary>
        public virtual bool EditorShowScrollSpeed => true;
    }
}
