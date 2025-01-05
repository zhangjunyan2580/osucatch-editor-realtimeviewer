using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Beatmaps.Formats;
using osu.Game.Beatmaps.Legacy;
using osu.Game.IO;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Catch.Mods;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Catch.UI;
using osu.Game.Rulesets.Mods;
using System.Text;

namespace osucatch_editor_realtimeviewer
{

    public class CatchBeatmapAPI
    {
        public static Ruleset catchRulest => new CatchRuleset();

        public static Decoder<Beatmap> beatmapDecoder => new LegacyBeatmapDecoder();
        private static Beatmap readFromFile(string file)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using (var reader = new LineBufferedReader(stream))
                return beatmapDecoder.Decode(reader);
        }

        static Mod[] NoMod = Array.Empty<Mod>();

        static Mod[] EZMod = { new CatchModEasy().CreateInstance() };

        static Mod[] HRMod = { new CatchModHardRock().CreateInstance() };

        static Mod[] GetMods(string[] Mods, Ruleset ruleset)
        {
            if (Mods == null)
                return NoMod;

            foreach (var modString in Mods)
            {
                if (modString == "EZ") return EZMod;
                else if (modString == "HR") return HRMod;
            }

            return NoMod;
        }

        public static string[] GetModsString(int raw_mods)
        {
            List<string> modsArr = new List<string>();
            if ((raw_mods & 2) > 0) modsArr.Add("EZ");
            if ((raw_mods & 16) > 0) modsArr.Add("HR");
            return modsArr.ToArray();
        }

        public static IBeatmap Execute(string file, Mod[] mods)
        {
            Ruleset ruleset = catchRulest;
            Beatmap beatmap = readFromFile(file);
            FlatWorkingBeatmap workingBeatmap = new FlatWorkingBeatmap(beatmap);
            IBeatmap playableBeatmap = workingBeatmap.GetPlayableBeatmap(ruleset, mods);
            if (playableBeatmap == null) throw new Exception("This beatmap is invalid or is not a ctb beatmap.");
            return playableBeatmap;
        }

        public static IBeatmap GetBeatmap(string file, string[] mods)
        {
            Ruleset ruleset = catchRulest;
            return Execute(file, GetMods(mods, ruleset));
        }

        public static IBeatmap GetBeatmap(string file, int mods)
        {
            Ruleset ruleset = catchRulest;
            return Execute(file, GetMods(GetModsString(mods), ruleset));
        }

        public static List<WithDistancePalpableCatchHitObject> GetPalpableObjects(IBeatmap beatmap, bool isCalDistance)
        {
            List<PalpableCatchHitObject> palpableObjects = new List<PalpableCatchHitObject>();

            foreach (var currentObject in beatmap.HitObjects)
            {
                if (currentObject is Fruit fruitObject)
                    palpableObjects.Add(fruitObject);

                else if (currentObject is JuiceStream)
                {
                    foreach (var juice in currentObject.NestedHitObjects)
                    {
                        if (juice is PalpableCatchHitObject palpableObject)
                            palpableObjects.Add(palpableObject);
                    }
                }
                else if (currentObject is BananaShower)
                {
                    foreach (var banana in currentObject.NestedHitObjects)
                    {
                        if (banana is PalpableCatchHitObject palpableObject)
                            palpableObjects.Add(palpableObject);
                    }
                }
            }

            palpableObjects.Sort((h1, h2) => h1.StartTime.CompareTo(h2.StartTime));

            List<WithDistancePalpableCatchHitObject> wdpcos = new List<WithDistancePalpableCatchHitObject>();

            for (int i = 0; i < palpableObjects.Count; i++)
            {
                WithDistancePalpableCatchHitObject wdpco = new WithDistancePalpableCatchHitObject(palpableObjects[i]);
                if (isCalDistance)
                {
                    var nextObj = GetNextComboObject(palpableObjects, i + 1);
                    if (nextObj != null)
                    {
                        wdpco.CalDistance(beatmap, nextObj);
                    }
                }
                wdpcos.Add(wdpco);
            }

            return wdpcos;
        }

        private static PalpableCatchHitObject? GetNextComboObject(List<PalpableCatchHitObject> palpableObjects, int startIndex)
        {
            for (int i = startIndex; i < palpableObjects.Count; i++)
            {
                if (palpableObjects[i] is Fruit || (palpableObjects[i] is Droplet && palpableObjects[i] is not TinyDroplet))
                {
                    return palpableObjects[i];
                }
            }
            return null;
        }

    }

    public enum DistanceType
    {
        None,
        SameWithEditor,
        NoSliderVelocityMultiplier,
        CompareWithWalkSpeed,
    }

    public class WithDistancePalpableCatchHitObject
    {
        public PalpableCatchHitObject currentObject;
        public double XDistToNext_SameWithEditor = 0;
        public double XDistToNext_NoSliderVelocityMultiplier = 0;
        public double XDistToNext_CompareWithWalkSpeed = 0;

        public WithDistancePalpableCatchHitObject(PalpableCatchHitObject currentObject)
        {
            this.currentObject = currentObject;
        }

        public TimingControlPoint GetTimingPoint(IBeatmap beatmap)
        {
            return beatmap.ControlPointInfo.TimingPointAt(currentObject.StartTime);
        }

        public DifficultyControlPoint GetDifficultyControlPoint(IBeatmap beatmap)
        {
            return (beatmap.ControlPointInfo as LegacyControlPointInfo)?.DifficultyPointAt(currentObject.StartTime) ?? DifficultyControlPoint.DEFAULT;
        }

        public void CalDistance(IBeatmap beatmap, PalpableCatchHitObject nextObject)
        {
            double timeToNext = (int)nextObject.StartTime - (int)currentObject.StartTime; // - 1000f / 60f / 4; // 1/4th of a frame of grace time, taken from osu-stable
            double distanceToNext = Math.Abs(nextObject.OriginalX - currentObject.OriginalX);
            DifficultyControlPoint nextDifficultyControlPoint = (beatmap.ControlPointInfo as LegacyControlPointInfo)?.DifficultyPointAt(nextObject.StartTime) ?? DifficultyControlPoint.DEFAULT;
            var nextTimingPoint = beatmap.ControlPointInfo.TimingPointAt(nextObject.StartTime);
            if (timeToNext <= 0) return;
            XDistToNext_CompareWithWalkSpeed = distanceToNext / timeToNext / Catcher.BASE_WALK_SPEED;
            if (XDistToNext_CompareWithWalkSpeed <= 0 || XDistToNext_CompareWithWalkSpeed > 100)
            {
                XDistToNext_CompareWithWalkSpeed = 0;
                return;
            }
            if (beatmap.Difficulty.SliderMultiplier <= 0 || nextTimingPoint.BeatLength <= 0) return;
            XDistToNext_NoSliderVelocityMultiplier = distanceToNext / (beatmap.Difficulty.SliderMultiplier * 100) / (timeToNext / nextTimingPoint.BeatLength);
            if (XDistToNext_NoSliderVelocityMultiplier <= 0 || XDistToNext_NoSliderVelocityMultiplier > 100)
            {
                XDistToNext_NoSliderVelocityMultiplier = 0;
                return;
            }
            if (nextDifficultyControlPoint.SliderVelocity <= 0) return;
            XDistToNext_SameWithEditor = XDistToNext_NoSliderVelocityMultiplier / nextDifficultyControlPoint.SliderVelocity;
            if (XDistToNext_SameWithEditor <= 0 || XDistToNext_SameWithEditor > 100)
            {
                XDistToNext_SameWithEditor = 0;
                return;
            }

        }

        public string GetDistanceString(DistanceType dt)
        {
            if (dt == DistanceType.None) return "";
            else if (dt == DistanceType.SameWithEditor)
            {
                if (XDistToNext_SameWithEditor < 0.01) return "";
                else return "x" + XDistToNext_SameWithEditor.ToString("F2");
            }
            else if (dt == DistanceType.NoSliderVelocityMultiplier)
            {
                if (XDistToNext_NoSliderVelocityMultiplier < 0.01) return "";
                else return "x" + XDistToNext_NoSliderVelocityMultiplier.ToString("F2");
            }
            else if (dt == DistanceType.CompareWithWalkSpeed)
            {
                if (XDistToNext_CompareWithWalkSpeed < 0.01) return "";
                else return "x" + XDistToNext_CompareWithWalkSpeed.ToString("F2");
            }
            else return "";
        }
    }
}
