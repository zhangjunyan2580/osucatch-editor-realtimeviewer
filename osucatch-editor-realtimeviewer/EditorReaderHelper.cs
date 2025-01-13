using Editor_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osucatch_editor_realtimeviewer
{
    public class EditorReaderHelper
    {
        private static readonly EditorReader reader = new();

        private bool Is_Doing_SetProcess = false;
        private bool Is_Doing_FetchEditor = false;
        private bool Is_Osu_Running = false;
        public bool Is_Editor_Running = false;

        public string beatmap_path = "";
        public string beatmap_title = "";

        public EditorReaderHelper() {
            reader.autoDeStack = true;
        }

        public bool FetchProcess()
        {
            bool isNeedReload = true;
            try
            {
                isNeedReload = reader.ProcessNeedsReload();
            }
            catch(Exception ex)
            {
                Log.ConsoleLog("Get osu! process failed.\r\n" + ex, Log.LogType.EditorReader, Log.LogLevel.Error);
            }

            if (!Is_Osu_Running || isNeedReload)
            {
                try
                {
                    Log.ConsoleLog("Osu! process needs refetch.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    if (Is_Doing_SetProcess)
                    {
                        Log.ConsoleLog("Still fetching osu!.", Log.LogType.EditorReader, Log.LogLevel.Info);
                        return false;
                    }
                    Log.ConsoleLog("Try to fetch osu! process.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    Is_Doing_SetProcess = true;
                    reader.SetProcess();
                    Is_Doing_SetProcess = false;
                    Log.ConsoleLog("Fetch osu! process successfully.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    Is_Osu_Running = true;
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("No Osu!.exe found.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    Log.ConsoleLog(ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Debug);
                    Is_Doing_SetProcess = false;
                    Is_Osu_Running = false;
                    Is_Editor_Running = false;
                    return false;
                }
            }
            return true;
        }

        public string FetchTitle()
        {
            try
            {
                string title = reader.ProcessTitle();
                return title;
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("Get osu! title failed.\r\n" + ex, Log.LogType.EditorReader, Log.LogLevel.Error);
                Is_Osu_Running = false;
                return "";
            }
        }

        public bool FetchEditor()
        {
            beatmap_title = "";
            string title = FetchTitle();
            if (title == "")
            {
                Log.ConsoleLog("Empty osu title.", Log.LogType.EditorReader, Log.LogLevel.Info);
                Is_Editor_Running = false;
                beatmap_path = "";
                return false;
            }
            if (!title.EndsWith(".osu"))
            {
                Log.ConsoleLog("Osu title is not editor: " + title, Log.LogType.EditorReader, Log.LogLevel.Info);
                Is_Editor_Running = false;
                beatmap_path = "";
                return false;
            }
            if (reader.EditorNeedsReload())
            {
                Log.ConsoleLog("Editor needs Reload.", Log.LogType.EditorReader, Log.LogLevel.Info);
                try
                {
                    if (Is_Doing_SetProcess || Is_Doing_FetchEditor)
                    {
                        Log.ConsoleLog("Still fetching editor.", Log.LogType.EditorReader, Log.LogLevel.Info);
                        return false;
                    }
                    if (reader.ProcessNeedsReload())
                    {
                        Log.ConsoleLog("Process needs reload.", Log.LogType.EditorReader, Log.LogLevel.Info);
                        return false;
                    }
                    Log.ConsoleLog("Try fetch editor.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    Is_Doing_FetchEditor = true;
                    reader.FetchEditor();
                    Is_Doing_FetchEditor = false;
                    Log.ConsoleLog("Fetch editor successfully.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    Is_Osu_Running = true;
                    Is_Editor_Running = true;
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Fetch editor failed.\r\n" + ex, Log.LogType.EditorReader, Log.LogLevel.Error);
                    Is_Doing_FetchEditor = false;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    return false;
                }
            }
            Is_Editor_Running = true;
            beatmap_title = title;
            return true;
        }

        public BeatmapInfoCollection? FetchAll()
        {
            try
            {
                reader.FetchAll();
                var thisReader = new BeatmapInfoCollection(reader);
                return thisReader;
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("FetchAll failed.\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                return null;
            }
        }
    }

    public class BeatmapInfoCollection
    {
        public int NumControlPoints;
        public int NumObjects;
        public int EditorTime;
        public string ContainingFolder;
        public string Filename;
        public int PreviewTime;
        public float StackLeniency;
        public float HPDrainRate;
        public float CircleSize;
        public float OverallDifficulty;
        public float ApproachRate;
        public double SliderMultiplier;
        public double SliderTickRate;
        public int[] Bookmarks;
        public List<ControlPoint> ControlPoints;
        public List<Editor_Reader.HitObject> HitObjects;
        public BeatmapInfoCollection(EditorReader reader)
        {
            NumControlPoints = reader.numControlPoints;
            NumObjects = reader.numObjects;
            EditorTime = reader.EditorTime();
            ContainingFolder = reader.ContainingFolder;
            Filename = reader.Filename;
            PreviewTime = reader.PreviewTime;
            StackLeniency = reader.StackLeniency;
            HPDrainRate = reader.HPDrainRate;
            CircleSize = reader.CircleSize;
            OverallDifficulty = reader.OverallDifficulty;
            ApproachRate = reader.ApproachRate;
            SliderMultiplier = reader.SliderMultiplier;
            SliderTickRate = reader.SliderTickRate;
            Bookmarks = reader.bookmarks;
            ControlPoints = reader.controlPoints;
            HitObjects = reader.hitObjects;
        }
    }
}
