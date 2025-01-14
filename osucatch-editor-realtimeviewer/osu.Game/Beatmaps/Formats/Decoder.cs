// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.IO;
using osucatch_editor_realtimeviewer;

namespace osu.Game.Beatmaps.Formats
{
    public abstract class Decoder<TOutput>
        where TOutput : new()
    {
        protected virtual TOutput CreateTemplateObject() => new TOutput();

        public TOutput Decode(BeatmapInfoCollection thisReaderData, List<string>? colourLines)
        {
            var output = CreateTemplateObject();
            ParseStreamInto(thisReaderData, output, colourLines);
            return output;
        }

        protected abstract void ParseStreamInto(BeatmapInfoCollection thisReaderData, TOutput output, List<string>? colourLines);
    }
}
