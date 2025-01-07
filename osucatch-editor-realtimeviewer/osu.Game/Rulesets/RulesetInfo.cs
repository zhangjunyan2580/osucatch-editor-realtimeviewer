// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Game.Rulesets
{
    public class RulesetInfo : IRulesetInfo
    {
        public string ShortName { get; set; } = string.Empty;

        public int OnlineID { get; set; } = -1;

        public string Name { get; set; } = string.Empty;

        public string InstantiationInfo { get; set; } = string.Empty;

        public RulesetInfo()
        {
        }
    }
}
