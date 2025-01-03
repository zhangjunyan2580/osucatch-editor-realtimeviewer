// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions;
using osu.Game.IO;
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

        protected override void ParseStreamInto(LineBufferedReader stream, T output)
        {
            Section section = Section.General;

            string? line;

            while ((line = stream.ReadLine()) != null)
            {
                if (ShouldSkipLine(line))
                    continue;

                if (section != Section.Metadata)
                {
                    // comments should not be stripped from metadata lines, as the song metadata may contain "//" as valid data.
                    line = StripComments(line);
                }

                line = line.TrimEnd();

                if (line.StartsWith('[') && line.EndsWith(']'))
                {
                    if (!Enum.TryParse(line[1..^1], out section))
                        Form1.ConsoleLog($"Unknown section \"{line}\" in \"{output}\"", Form1.LogType.BeatmapParser, Form1.LogLevel.Warning);

                    OnBeginNewSection(section);
                    continue;
                }

                try
                {
                    ParseLine(output, section, line);
                }
                catch (Exception e)
                {
                    Form1.ConsoleLog($"Failed to process line \"{line}\" into \"{output}\": {e.Message}", Form1.LogType.BeatmapParser, Form1.LogLevel.Warning);
                }
            }
        }

        protected virtual bool ShouldSkipLine(string line) => string.IsNullOrWhiteSpace(line) || line.AsSpan().TrimStart().StartsWith("//".AsSpan(), StringComparison.Ordinal);

        /// <summary>
        /// Invoked when a new <see cref="Section"/> has been entered.
        /// </summary>
        /// <param name="section">The entered <see cref="Section"/>.</param>
        protected virtual void OnBeginNewSection(Section section)
        {
        }

        protected virtual void ParseLine(T output, Section section, string line)
        {
            switch (section)
            {
                case Section.Colours:
                    return;
            }
        }

        protected string StripComments(string line)
        {
            int index = line.AsSpan().IndexOf("//".AsSpan());
            if (index > 0)
                return line.Substring(0, index);

            return line;
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

        protected string CleanFilename(string path) => path
                                                       // User error which is supported by stable (https://github.com/ppy/osu/issues/21204)
                                                       .Replace(@"\\", @"\")
                                                       .Trim('"')
                                                       .ToStandardisedPath();

        public enum Section
        {
            General,
            Editor,
            Metadata,
            Difficulty,
            Events,
            TimingPoints,
            Colours,
            HitObjects,
            Variables,
            Fonts,
            CatchTheBeat,
            Mania,
        }
    }
}
