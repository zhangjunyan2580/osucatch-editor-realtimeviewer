// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Game.Rulesets.Objects;

namespace osu.Game.Rulesets.Objects
{
    public class BarLine : IBarLine
    {
        private bool major;

        private double startTime;

        public bool Major
        {
            get => major;
            set => major = value;
        }

        public double StartTime
        {
            get => startTime;
            set => startTime = value;
        }

    }
}
