// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
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

        public virtual string ExtendedIconInformation => string.Empty;


        public virtual ModType Type => ModType.Fun;

        /// <summary>
        /// Returns true if this mod is implemented (and playable).
        /// </summary>
        public virtual bool HasImplementation => this is IApplicableMod;

        /// <summary>
        /// Whether this mod can be played by a real human user.
        /// Non-user-playable mods are not viable for single-player score submission.
        /// </summary>
        /// <example>
        /// <list type="bullet">
        /// <item><see cref="ModDoubleTime"/> is user-playable.</item>
        /// <item><see cref="ModAutoplay"/> is not user-playable.</item>
        /// </list>
        /// </example>
        public virtual bool UserPlayable => true;

        /// <summary>
        /// Whether this mod can be specified as a "required" mod in a multiplayer context.
        /// </summary>
        /// <example>
        /// <list type="bullet">
        /// <item><see cref="ModHardRock"/> is valid for multiplayer.</item>
        /// <item>
        /// <see cref="ModDoubleTime"/> is valid for multiplayer as long as it is a <b>required</b> mod,
        /// as that ensures the same duration of gameplay for all users in the room.
        /// </item>
        /// <item>
        /// <see cref="ModAdaptiveSpeed"/> is not valid for multiplayer, as it leads to varying
        /// gameplay duration depending on how the users in the room play.
        /// </item>
        /// <item><see cref="ModAutoplay"/> is not valid for multiplayer.</item>
        /// </list>
        /// </example>
        public virtual bool ValidForMultiplayer => true;

        /// <summary>
        /// Whether this mod can be specified as a "free" or "allowed" mod in a multiplayer context.
        /// </summary>
        /// <example>
        /// <list type="bullet">
        /// <item><see cref="ModHardRock"/> is valid for multiplayer as a free mod.</item>
        /// <item>
        /// <see cref="ModDoubleTime"/> is <b>not</b> valid for multiplayer as a free mod,
        /// as it could to varying gameplay duration between users in the room depending on whether they picked it.
        /// </item>
        /// <item><see cref="ModAutoplay"/> is not valid for multiplayer as a free mod.</item>
        /// </list>
        /// </example>
        public virtual bool ValidForMultiplayerAsFreeMod => true;

        /// <inheritdoc/>
        public virtual bool AlwaysValidForSubmission => false;

        /// <summary>
        /// Whether this mod requires configuration to apply changes to the game.
        /// </summary>
        public virtual bool RequiresConfiguration => false;

        /// <summary>
        /// Whether scores with this mod active can give performance points.
        /// </summary>
        public virtual bool Ranked => false;

        /// <summary>
        /// The mods this mod cannot be enabled with.
        /// </summary>
        public virtual Type[] IncompatibleMods => Array.Empty<Type>();

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
