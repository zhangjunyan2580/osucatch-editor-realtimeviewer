// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Mods
{
    /// <summary>
    /// An interface for <see cref="Mod"/>s that can be applied to <see cref="DrawableRuleset"/>s.
    /// </summary>
    public interface IApplicableToDrawableRuleset<TObject> : IApplicableMod
        where TObject : HitObject
    {
    }
}
