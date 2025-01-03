// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Objects.Types
{
    /// <summary>
    /// A HitObject that spans some length.
    /// </summary>
    public interface IHasRepeats : IHasDuration
    {
        /// <summary>
        /// The amount of times the HitObject repeats.
        /// </summary>
        int RepeatCount { get; set; }
    }

    public static class HasRepeatsExtensions
    {
        /// <summary>
        /// The amount of times the length of this <see cref="IHasRepeats"/> spans.
        /// </summary>
        /// <param name="obj">The object that has repeats.</param>
        public static int SpanCount(this IHasRepeats obj) => obj.RepeatCount + 1;
    }
}
