// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModEasy : Mod, IApplicableToDifficulty
    {
        public override string Name => "Easy";
        public override string Acronym => "EZ";
        public override ModType Type => ModType.DifficultyReduction;
        public virtual void ReadFromDifficulty(BeatmapDifficulty difficulty)
        {
        }

        public virtual void ApplyToDifficulty(BeatmapDifficulty difficulty)
        {
            const float ratio = 0.5f;
            difficulty.CircleSize *= ratio;
            difficulty.ApproachRate *= ratio;
            difficulty.DrainRate *= ratio;
            difficulty.OverallDifficulty *= ratio;
        }
    }
}
