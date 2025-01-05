// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.


using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Legacy;
using osuTK;


namespace osu.Game.Rulesets.Catch.UI
{

    public partial class Catcher
    {
        /// <summary>
        /// The size of the catcher at 1x scale.
        /// </summary>
        /// <remarks>
        /// This is mainly used to compute catching range, the actual catcher size may differ based on skin implementation and sprite textures.
        /// This is also equivalent to the "catcherWidth" property in osu-stable when the game field and beatmap difficulty are set to default values.
        /// </remarks>
        /// <seealso cref="CatchPlayfield.WIDTH"/>
        /// <seealso cref="CatchPlayfield.HEIGHT"/>
        /// <seealso cref="IBeatmapDifficultyInfo.DEFAULT_DIFFICULTY"/>
        public const float BASE_SIZE = 106.75f;

        /// <summary>
        /// The width of the catcher which can receive fruit. Equivalent to "catchMargin" in osu-stable.
        /// </summary>
        public const float ALLOWED_CATCH_RANGE = 0.8f;

        /// <summary>
        /// The duration between transitioning to hyper-dash state.
        /// </summary>
        public const double HYPER_DASH_TRANSITION_DURATION = 180;

        /// <summary>
        /// Whether we are hyper-dashing or not.
        /// </summary>
        public bool HyperDashing => hyperDashModifier != 1;

        /// <summary>
        /// Whether <see cref="DrawablePalpableCatchHitObject"/> fruit should appear on the plate.
        /// </summary>
        public bool CatchFruitOnPlate { get; set; } = true;

        /// <summary>
        /// The speed of the catcher when the catcher is dashing.
        /// </summary>
        public const double BASE_DASH_SPEED = 1.0;

        /// <summary>
        /// The speed of the catcher when the catcher is not dashing.
        /// </summary>
        public const double BASE_WALK_SPEED = 0.5;

        /// <summary>
        /// The current speed of the catcher with the hyper-dash modifier applied.
        /// </summary>
        public double Speed => (Dashing ? BASE_DASH_SPEED : BASE_WALK_SPEED) * hyperDashModifier;

        private Vector2 scale = Vector2.One;

        /// <summary>
        /// Base relative scaling factor around <see cref="OriginPosition"/>.
        /// </summary>
        public Vector2 Scale
        {
            get => scale;
            set
            {
                if (scale == value)
                    return;

                if (!Validation.IsFinite(value)) throw new ArgumentException($@"{nameof(Scale)} must be finite, but is {value}.");

                scale = value;
            }
        }

        /// <summary>
        /// Whether the catcher is currently dashing.
        /// </summary>
        public bool Dashing { get; set; }

        /// <summary>
        /// Width of the area that can be used to attempt catches during gameplay.
        /// </summary>
        public float CatchWidth { get; private set; }

        private double hyperDashModifier = 1;

        public Catcher(IBeatmapDifficultyInfo? difficulty = null)
        {
            ApplyDifficulty(difficulty);
        }

        /// <summary>
        /// Calculates the width of the area used for attempting catches in gameplay.
        /// </summary>
        /// <param name="scale">The scale of the catcher.</param>
        public static float CalculateCatchWidth(Vector2 scale) => BASE_SIZE * Math.Abs(scale.X) * ALLOWED_CATCH_RANGE;

        /// <summary>
        /// Calculates the width of the area used for attempting catches in gameplay.
        /// </summary>
        /// <param name="difficulty">The beatmap difficulty.</param>
        public static float CalculateCatchWidth(IBeatmapDifficultyInfo difficulty) => CalculateCatchWidth(calculateScale(difficulty));


        /// <summary>
        /// Set the scale and catch width.
        /// </summary>
        public void ApplyDifficulty(IBeatmapDifficultyInfo? difficulty)
        {
            if (difficulty != null)
                Scale = calculateScale(difficulty);

            CatchWidth = CalculateCatchWidth(Scale);
        }

        /// <summary>
        /// Calculates the scale of the catcher based off the provided beatmap difficulty.
        /// </summary>
        private static Vector2 calculateScale(IBeatmapDifficultyInfo difficulty) => new Vector2(LegacyRulesetExtensions.CalculateScaleFromCircleSize(difficulty.CircleSize) * 2);
    }
}
