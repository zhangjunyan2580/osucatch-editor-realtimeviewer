// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Utils
{
    public static class FormatUtils
    {
        /// <summary>
        /// Finds the number of digits after the decimal.
        /// </summary>
        /// <param name="d">The value to find the number of decimal digits for.</param>
        /// <returns>The number decimal digits.</returns>
        public static int FindPrecision(decimal d)
        {
            int precision = 0;

            while (d != Math.Round(d))
            {
                d *= 10;
                precision++;
            }

            return precision;
        }

        /// <summary>
        /// Applies rounding to the given BPM value.
        /// </summary>
        /// <remarks>
        /// Double-rounding is applied intentionally (see https://github.com/ppy/osu/pull/18345#issue-1243311382 for rationale).
        /// </remarks>
        /// <param name="baseBpm">The base BPM to round.</param>
        /// <param name="rate">Rate adjustment, if applicable.</param>
        public static int RoundBPM(double baseBpm, double rate = 1) => (int)Math.Round(Math.Round(baseBpm) * rate);
    }
}
