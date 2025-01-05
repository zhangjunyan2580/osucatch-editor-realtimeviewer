// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Beatmaps
{
    /// <summary>
    /// Metadata representing a beatmap. May be shared between multiple beatmap difficulties.
    /// </summary>
    public interface IBeatmapMetadataInfo : IEquatable<IBeatmapMetadataInfo>
    {
        /// <summary>
        /// The romanised title of this beatmap.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The unicode title of this beatmap.
        /// </summary>
        string TitleUnicode { get; }

        /// <summary>
        /// The romanised artist of this beatmap.
        /// </summary>
        string Artist { get; }

        /// <summary>
        /// The unicode artist of this beatmap.
        /// </summary>
        string ArtistUnicode { get; }

        /// <summary>
        /// The author of this beatmap.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// The source of this beatmap.
        /// </summary>
        string Source { get; }

        /// <summary>
        /// The tags of this beatmap.
        /// </summary>
        string Tags { get; }

        bool IEquatable<IBeatmapMetadataInfo>.Equals(IBeatmapMetadataInfo? other)
        {
            if (other == null)
                return false;

            return Title == other.Title
                   && TitleUnicode == other.TitleUnicode
                   && Artist == other.Artist
                   && ArtistUnicode == other.ArtistUnicode
                   && Author == other.Author
                   && Source == other.Source
                   && Tags == other.Tags;
        }
    }
}
