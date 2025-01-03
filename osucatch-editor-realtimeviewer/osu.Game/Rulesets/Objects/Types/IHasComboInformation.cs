// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Objects.Types
{
    /// <summary>
    /// A HitObject that is part of a combo and has extended information about its position relative to other combo objects.
    /// </summary>
    public interface IHasComboInformation : IHasCombo
    {
        /// <summary>
        /// Bindable exposure of <see cref="IndexInCurrentCombo"/>.
        /// </summary>
        Bindable<int> IndexInCurrentComboBindable { get; }

        /// <summary>
        /// The index of this hitobject in the current combo.
        /// </summary>
        int IndexInCurrentCombo { get; set; }

        /// <summary>
        /// Bindable exposure of <see cref="ComboIndex"/>.
        /// </summary>
        Bindable<int> ComboIndexBindable { get; }

        /// <summary>
        /// The index of this combo in relation to the beatmap.
        ///
        /// In other words, this is incremented by 1 each time a <see cref="NewCombo"/> is reached.
        /// </summary>
        int ComboIndex { get; set; }

        /// <summary>
        /// Bindable exposure of <see cref="ComboIndexWithOffsets"/>.
        /// </summary>
        Bindable<int> ComboIndexWithOffsetsBindable { get; }

        /// <summary>
        /// The index of this combo in relation to the beatmap, with all aggregate <see cref="IHasCombo.ComboOffset"/>s applied.
        /// This should be used instead of <see cref="ComboIndex"/> only when retrieving combo colours from the beatmap's skin.
        /// </summary>
        int ComboIndexWithOffsets { get; set; }

        /// <summary>
        /// Whether the HitObject starts a new combo.
        /// </summary>
        new bool NewCombo { get; set; }

        /// <summary>
        /// Bindable exposure of <see cref="LastInCombo"/>.
        /// </summary>
        Bindable<bool> LastInComboBindable { get; }

        /// <summary>
        /// Whether this is the last object in the current combo.
        /// </summary>
        bool LastInCombo { get; set; }

        /// <summary>
        /// Given the previous object in the beatmap, update relevant combo information.
        /// </summary>
        /// <param name="lastObj">The previous hitobject, or null if this is the first object in the beatmap.</param>
        void UpdateComboInformation(IHasComboInformation? lastObj)
        {
            ComboIndex = lastObj?.ComboIndex ?? 0;
            ComboIndexWithOffsets = lastObj?.ComboIndexWithOffsets ?? 0;
            IndexInCurrentCombo = (lastObj?.IndexInCurrentCombo + 1) ?? 0;

            if (NewCombo || lastObj == null)
            {
                IndexInCurrentCombo = 0;
                ComboIndex++;
                ComboIndexWithOffsets += ComboOffset + 1;

                if (lastObj != null)
                    lastObj.LastInCombo = true;
            }
        }
    }
}
