using osu.Game.Beatmaps;
using osu.Game.IO;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Catch;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Mods;
using System.Text;

namespace osucatch_editor_realtimeviewer
{

    public class CatchBeatmapAPI
    {
        public static Ruleset catchRulest => new CatchRuleset();
        private static Beatmap readFromFile(string file)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(file);
            MemoryStream stream = new MemoryStream(byteArray);
            using (var reader = new LineBufferedReader(stream))
                return osu.Game.Beatmaps.Formats.Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
        }

        static Mod[] GetMods(string[] Mods, Ruleset ruleset)
        {
            if (Mods == null)
                return Array.Empty<Mod>();

            List<Mod> availableMods = ruleset.CreateAllMods().ToList();
            List<Mod> mods = new List<Mod>();

            foreach (var modString in Mods)
            {
                if (modString == "ScoreV2") continue;
                Mod newMod = availableMods.FirstOrDefault(m => string.Equals(m.Acronym, modString, StringComparison.CurrentCultureIgnoreCase)) ?? throw new ArgumentException($"Invalid mod provided: {modString}");
                mods.Add(newMod);
            }

            return mods.ToArray();
        }

        public static string[] GetModsString(int raw_mods)
        {
            List<string> modsArr = new List<string>();
            if ((raw_mods & 1) > 0) modsArr.Add("NF");
            if ((raw_mods & 2) > 0) modsArr.Add("EZ");
            if ((raw_mods & 4) > 0) modsArr.Add("TD");
            if ((raw_mods & 8) > 0) modsArr.Add("HD");
            if ((raw_mods & 16) > 0) modsArr.Add("HR");
            if ((raw_mods & 32) > 0) modsArr.Add("SD");
            if ((raw_mods & 64) > 0) modsArr.Add("DT");
            if ((raw_mods & 128) > 0) modsArr.Add("Relax");
            if ((raw_mods & 256) > 0) modsArr.Add("HT");
            if ((raw_mods & 512) > 0) { modsArr.Add("NC"); modsArr.Remove("DT"); }
            if ((raw_mods & 1024) > 0) modsArr.Add("FL");
            if ((raw_mods & 2048) > 0) modsArr.Add("Auto");
            if ((raw_mods & 4096) > 0) modsArr.Add("SO");
            if ((raw_mods & 8192) > 0) modsArr.Add("AP");
            if ((raw_mods & 16384) > 0) { modsArr.Add("PF"); modsArr.Remove("SD"); };
            if ((raw_mods & 32768) > 0) modsArr.Add("4K");
            if ((raw_mods & 65536) > 0) modsArr.Add("5K");
            if ((raw_mods & 131072) > 0) modsArr.Add("6K");
            if ((raw_mods & 262144) > 0) modsArr.Add("7K");
            if ((raw_mods & 524288) > 0) modsArr.Add("8K");
            if ((raw_mods & 1048576) > 0) modsArr.Add("FI");
            if ((raw_mods & 2097152) > 0) modsArr.Add("RD");
            if ((raw_mods & 4194304) > 0) modsArr.Add("Cinema");
            if ((raw_mods & 8388608) > 0) modsArr.Add("Target");
            if ((raw_mods & 16777216) > 0) modsArr.Add("9K");
            if ((raw_mods & 33554432) > 0) modsArr.Add("KeyCoop");
            if ((raw_mods & 67108864) > 0) modsArr.Add("1K");
            if ((raw_mods & 134217728) > 0) modsArr.Add("3K");
            if ((raw_mods & 268435456) > 0) modsArr.Add("2K");
            if ((raw_mods & 536870912) > 0) modsArr.Add("ScoreV2");
            if ((raw_mods & 1073741824) > 0) modsArr.Add("MR");
            return modsArr.ToArray();
        }

        public static IBeatmap Execute(string file, Mod[] mods)
        {
            Ruleset ruleset = catchRulest;
            Beatmap beatmap = readFromFile(file);
            FlatWorkingBeatmap workingBeatmap = new FlatWorkingBeatmap(beatmap);
            IBeatmap playableBeatmap = workingBeatmap.GetPlayableBeatmap(ruleset.RulesetInfo, mods);
            if (playableBeatmap == null) throw new Exception("该谱面有错误或无法游玩接水果模式。");
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

        public static List<PalpableCatchHitObject> GetPalpableObjects(IBeatmap beatmap)
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

            return palpableObjects;
        }

    }
}
