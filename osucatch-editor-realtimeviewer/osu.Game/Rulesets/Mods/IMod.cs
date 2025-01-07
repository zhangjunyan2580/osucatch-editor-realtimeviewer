// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets.Mods
{
    public interface IMod : IEquatable<IMod>
    {
        /// <summary>
        /// The shortened name of this mod.
        /// </summary>
        string Acronym { get; }

        /// <summary>
        /// The name of this mod.
        /// </summary>
        string Name { get; }
    }
}
