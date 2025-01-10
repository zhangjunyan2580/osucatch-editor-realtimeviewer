using OpenTK.Graphics;
using osu.Game.Beatmaps;

namespace osucatch_editor_realtimeviewer
{

    public class ViewerManager
    {
        public float currentTime { get; set; }
        public IBeatmap? Beatmap { get; set; }
        public List<WithDistancePalpableCatchHitObject>? CatchHitObjects { get; set; }
        public List<WithDistancePalpableCatchHitObject> NearbyHitObjects { get; set; }
        public int ApproachTime { get; set; }
        public float TimePerPixels { get; set; }
        private int CircleDiameter { get; set; }
        public DistanceType DistanceType { get; set; }
        public List<Color4>? CustomComboColours { get; set; }



        public ViewerManager(string beatmap, int modsWhenOnlyBeatmap = 0)
        {
            currentTime = 0;
            NearbyHitObjects = new List<WithDistancePalpableCatchHitObject>();

            LoadBeatmap(beatmap, modsWhenOnlyBeatmap);
        }

        public void LoadBeatmap(string beatmap, int mods = 0)
        {
            Beatmap = CatchBeatmapAPI.GetBeatmap(beatmap, mods);
            CatchHitObjects = CatchBeatmapAPI.GetPalpableObjects(Beatmap, (DistanceType != DistanceType.None));

            float moddedAR = Beatmap.Difficulty.ApproachRate;
            ApproachTime = (int)((moddedAR < 5) ? 1800 - moddedAR * 120 : 1200 - (moddedAR - 5) * 150);
            TimePerPixels = ApproachTime / 384.0f;
            float moddedCS = Beatmap.Difficulty.CircleSize;
            CircleDiameter = (int)(108.848 - moddedCS * 8.9646);
            CustomComboColours = Beatmap.CustomComboColours;
        }



        public void BuildNearby(int screensContain)
        {
            NearbyHitObjects.Clear();
            if (this.CatchHitObjects == null) return;
            double timeSpan = screensContain * ApproachTime * 1.25 + CircleDiameter * TimePerPixels * 2;
            int startIndex = (screensContain <= 1) ? this.HitObjectsLowerBound(currentTime - ApproachTime / 4 - CircleDiameter * TimePerPixels) : this.HitObjectsLowerBound(currentTime - timeSpan / 2);
            int endIndex = (screensContain <= 1) ? this.HitObjectsUpperBound(currentTime + ApproachTime + CircleDiameter * TimePerPixels) : this.HitObjectsUpperBound(currentTime + timeSpan / 2);
            // Console.WriteLine(startIndex + "->" + endIndex);
            for (int k = startIndex; k <= endIndex; k++)
            {
                if (k < 0)
                {
                    continue;
                }
                else if (k >= this.CatchHitObjects.Count)
                {
                    break;
                }
                this.NearbyHitObjects.Add(this.CatchHitObjects[k]);
            }
        }

        private int HitObjectsLowerBound(double target)
        {
            if (this.CatchHitObjects == null) return 0;
            int first = 0;
            int last = this.CatchHitObjects.Count - 1;
            int count = last - first;
            while (count > 0)
            {
                int step = count / 2;
                int it = first + step;
                var hitObject = this.CatchHitObjects[it];
                float endTime = (float)hitObject.currentObject.StartTime;
                if (endTime < target)
                {
                    first = ++it;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }
            return first;
        }

        private int HitObjectsUpperBound(double target)
        {
            if (this.CatchHitObjects == null) return 0;
            int first = 0;
            int last = this.CatchHitObjects.Count - 1;
            int count = last - first;
            while (count > 0)
            {
                int step = count / 2;
                int it = first + step;
                float startTime = (float)(this.CatchHitObjects[it].currentObject.StartTime);
                if (!(target < startTime))
                {
                    first = ++it;
                    count -= step + 1;
                }
                else
                {
                    count = step;
                }
            }
            return first;
        }


    }
}
