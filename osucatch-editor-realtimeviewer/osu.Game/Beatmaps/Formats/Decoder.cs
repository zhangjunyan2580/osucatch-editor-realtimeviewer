// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Game.IO;

namespace osu.Game.Beatmaps.Formats
{
    public abstract class Decoder<TOutput>
        where TOutput : new()
    {
        protected virtual TOutput CreateTemplateObject() => new TOutput();

        public TOutput Decode(LineBufferedReader stream)
        {
            var output = CreateTemplateObject();
            ParseStreamInto(stream, output);
            return output;
        }

        protected abstract void ParseStreamInto(LineBufferedReader stream, TOutput output);
    }
}
