// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Lists;
using osu.Game.Beatmaps.ControlPoints;

namespace osu.Game.Beatmaps.Legacy
{
    public class LegacyControlPointInfo : ControlPointInfo
    {
        /// <summary>
        /// All difficulty points.
        /// </summary>

        public IReadOnlyList<DifficultyControlPoint> DifficultyPoints => difficultyPoints;

        private readonly SortedList<DifficultyControlPoint> difficultyPoints = new SortedList<DifficultyControlPoint>(Comparer<DifficultyControlPoint>.Default);

        /// <summary>
        /// Finds the difficulty control point that is active at <paramref name="time"/>.
        /// </summary>
        /// <param name="time">The time to find the difficulty control point at.</param>
        /// <returns>The difficulty control point.</returns>
        public DifficultyControlPoint DifficultyPointAt(double time) => BinarySearchWithFallback(DifficultyPoints, time, DifficultyControlPoint.DEFAULT);

        public override void Clear()
        {
            base.Clear();
            difficultyPoints.Clear();
        }

        protected override bool CheckAlreadyExisting(double time, ControlPoint newPoint)
        {
            switch (newPoint)
            {
                case DifficultyControlPoint:
                    return newPoint.IsRedundant(DifficultyPointAt(time));

                default:
                    return base.CheckAlreadyExisting(time, newPoint);
            }
        }

        protected override void GroupItemAdded(ControlPoint controlPoint)
        {
            switch (controlPoint)
            {
                case DifficultyControlPoint typed:
                    difficultyPoints.Add(typed);
                    return;

                default:
                    base.GroupItemAdded(controlPoint);
                    break;
            }
        }

        protected override void GroupItemRemoved(ControlPoint controlPoint)
        {
            switch (controlPoint)
            {
                case DifficultyControlPoint typed:
                    difficultyPoints.Remove(typed);
                    break;
            }

            base.GroupItemRemoved(controlPoint);
        }
    }
}
