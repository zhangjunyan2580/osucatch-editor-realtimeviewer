// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osucatch_editor_realtimeviewer;

namespace osu.Game.Beatmaps.Formats
{
    public abstract class LegacyDecoder<T> : Decoder<T>
        where T : new()
    {
        public const int LATEST_VERSION = 14;

        /// <summary>
        /// The .osu format (beatmap) version.
        ///
        /// osu!stable's versions end at <see cref="LATEST_VERSION"/>.
        /// osu!lazer's versions starts at <see cref="LegacyBeatmapEncoder.FIRST_LAZER_VERSION"/>.
        /// </summary>
        protected readonly int FormatVersion;

        protected LegacyDecoder(int version)
        {
            FormatVersion = version;
        }

        protected override void ParseStreamInto(BeatmapInfoCollection thisReaderData, T output, List<string>? colourLines)
        {
        }

        protected KeyValuePair<string, string> SplitKeyVal(string line, char separator = ':', bool shouldTrim = true)
        {
            string[] split = line.Split(separator, 2, shouldTrim ? StringSplitOptions.TrimEntries : StringSplitOptions.None);

            return new KeyValuePair<string, string>
            (
                split[0],
                split.Length > 1 ? split[1] : string.Empty
            );
        }
    }
}
