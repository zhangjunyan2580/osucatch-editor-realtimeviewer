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
        private int CircleDiameter { get; set; }
        public float State_ARMul { get; set; }
        public DistanceType DistanceType { get; set; }


        public ViewerManager(string beatmap, int modsWhenOnlyBeatmap = 0)
        {
            currentTime = 0;
            NearbyHitObjects = new List<WithDistancePalpableCatchHitObject>();

            State_ARMul = 2.7f;
            LoadBeatmap(beatmap, modsWhenOnlyBeatmap);
        }

        public void LoadBeatmap(string beatmap, int mods = 0)
        {

            Beatmap = CatchBeatmapAPI.GetBeatmap(beatmap, mods);
            CatchHitObjects = CatchBeatmapAPI.GetPalpableObjects(Beatmap, (DistanceType != DistanceType.None));

            float moddedAR = Beatmap.Difficulty.ApproachRate;
            ApproachTime = (int)((moddedAR < 5) ? 1800 - moddedAR * 120 : 1200 - (moddedAR - 5) * 150);
            float moddedCS = Beatmap.Difficulty.CircleSize;
            CircleDiameter = (int)(108.848 - moddedCS * 8.9646);

        }



        public void BuildNearby()
        {
            NearbyHitObjects = new List<WithDistancePalpableCatchHitObject>();
            if (this.CatchHitObjects == null) return;
            int startIndex = this.HitObjectsLowerBound(currentTime);
            int endIndex = this.HitObjectsUpperBound(currentTime);
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

        private int HitObjectsLowerBound(float target)
        {
            if (this.CatchHitObjects == null) return 0;
            int first = 0;
            int last = this.CatchHitObjects.Count;
            int count = last - first;
            while (count > 0)
            {
                int step = count / 2;
                int it = first + step;
                var hitObject = this.CatchHitObjects[it];
                float endTime = (float)hitObject.currentObject.StartTime;
                float animationEnd = endTime + this.ApproachTime * this.State_ARMul;
                if (animationEnd < target)
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

        private int HitObjectsUpperBound(float target)
        {
            if (this.CatchHitObjects == null) return 0;
            int first = 0;
            int last = this.CatchHitObjects.Count;
            int count = last - first;
            while (count > 0)
            {
                int step = count / 2;
                int it = first + step;
                float animationStart = (float)(this.CatchHitObjects[it].currentObject.StartTime - this.ApproachTime * this.State_ARMul);
                if (!(target < animationStart))
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
