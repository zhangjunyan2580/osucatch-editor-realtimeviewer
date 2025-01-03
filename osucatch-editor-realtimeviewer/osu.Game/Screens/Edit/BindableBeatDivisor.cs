// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;

namespace osu.Game.Screens.Edit
{
    public class BindableBeatDivisor : BindableInt
    {
        public static readonly int[] PREDEFINED_DIVISORS = { 1, 2, 3, 4, 6, 8, 12, 16 };

        public const int MINIMUM_DIVISOR = 1;
        public const int MAXIMUM_DIVISOR = 64;

    }
}
