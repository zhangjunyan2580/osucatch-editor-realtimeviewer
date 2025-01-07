// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using System.Diagnostics.CodeAnalysis;

namespace osu.Game.Rulesets.Mods
{
    /// <summary>
    /// The base class for gameplay modifiers.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public abstract class Mod : IMod
    {
        public abstract string Name { get; }

        public abstract string Acronym { get; }

        public bool Equals(IMod? other) => other is Mod them && Equals(them);

        public bool Equals(Mod? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return GetType() == other.GetType();
        }

        /// <summary>
        /// Create a fresh <see cref="Mod"/> instance based on this mod.
        /// </summary>
        public Mod CreateInstance() => (Mod)Activator.CreateInstance(GetType())!;

    }
}
