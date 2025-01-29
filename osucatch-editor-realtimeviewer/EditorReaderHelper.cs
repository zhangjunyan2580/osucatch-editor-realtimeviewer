using Editor_Reader;

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

        private int fetchAll_Failed_Count = 0;
        private const int FetchAll_MaxRetry_Count = 10;

        public EditorReaderHelper()
        {
            reader.autoDeStack = true;
        }

        /// <summary>
        /// Fetch osu! process if needed for Editor Reader.
        /// </summary>
        /// <returns>Is success or not.</returns>
        public bool FetchProcess()
        {
            bool isNeedReload = true;
            try
            {
                isNeedReload = reader.ProcessNeedsReload();
            }
            catch (Exception ex)
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

        /// <summary>
        /// Fetch osu! window's title from Editor Reader.
        /// </summary>
        /// <returns>osu! window's title.
        /// <para />"" if failed.</returns>
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

        /// <summary>
        /// Fetch editor if needed for Editor Reader.
        /// </summary>
        /// <returns>Is success or not.</returns>
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

        /// <summary>
        /// Call Editor Reader's FetchAll().
        /// </summary>
        /// <returns>An object with editor reader's primary data.
        /// <para />null if failed.</returns>
        public BeatmapInfoCollection? FetchAll()
        {
            try
            {
                if (fetchAll_Failed_Count > FetchAll_MaxRetry_Count)
                {
                    Log.ConsoleLog("Refetching editor...", Log.LogType.EditorReader, Log.LogLevel.Warning);
                    fetchAll_Failed_Count = 0;
                    FetchEditor();
                    return null;
                }

                Log.ConsoleLog("Start FetchAll().", Log.LogType.EditorReader, Log.LogLevel.Debug);

                reader.FetchAll();
                var thisReaderData = new BeatmapInfoCollection(reader);

                Log.ConsoleLog("FetchAll complete.", Log.LogType.EditorReader, Log.LogLevel.Debug);
                fetchAll_Failed_Count = 0;
                return thisReaderData;
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("FetchAll failed.(" + fetchAll_Failed_Count + ")\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                fetchAll_Failed_Count++;
                return null;
            }
        }

        /// <summary>
        /// Call Editor Reader's FetchAll() and filter nearby hitobjects. Should disable backup.
        /// </summary>
        /// <param name="partialLoadingHalfTimeSpan">The half time span at reader time for filter hitobjects.
        /// <para />Warning: Cause RANDOM ERROR when using it. Should disable backup.</param>
        /// <returns>An object with editor reader's primary data.
        /// <para />null if failed.</returns>
        public BeatmapInfoCollection? FetchAll(double partialLoadingHalfTimeSpan)
        {
            try
            {
                if (fetchAll_Failed_Count > FetchAll_MaxRetry_Count)
                {
                    Log.ConsoleLog("Refetching editor...", Log.LogType.EditorReader, Log.LogLevel.Warning);
                    fetchAll_Failed_Count = 0;
                    FetchEditor();
                    return null;
                }

                Log.ConsoleLog("Start FetchAll().", Log.LogType.EditorReader, Log.LogLevel.Debug);

                reader.FetchAll();
                var thisReaderData = new BeatmapInfoCollection(reader, partialLoadingHalfTimeSpan);

                Log.ConsoleLog("FetchAll complete.", Log.LogType.EditorReader, Log.LogLevel.Debug);
                fetchAll_Failed_Count = 0;
                return thisReaderData;
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("FetchAll failed.(" + fetchAll_Failed_Count + ")\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                fetchAll_Failed_Count++;
                return null;
            }
        }
    }

    public class BeatmapInfoCollection
    {
        public bool IsFull;

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
        public List<string> ControlPointLines;
        public List<ReaderHitObjectWithSelect> HitObjectLines;

        /// <summary>
        /// Check Editor Reader's data and make a copy of its current data.
        /// </summary>
        /// <param name="reader">EditorReader</param>
        /// <exception cref="Exception">Throw when Editor Reader's data is invalid.</exception>
        public BeatmapInfoCollection(EditorReader reader)
        {
            IsFull = true;

            // Check editor reader's data
            if (reader.hitObjects == null)
            {
                throw new Exception("HitObjects is null.");
            }
            // Fix Editor Reader
            // Modified from Mapping_Tools
            // https://github.com/OliBomby/Mapping_Tools/tree/master/Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs
            // Under MIT Licnece https://github.com/OliBomby/Mapping_Tools/blob/master/LICENCE
            if (!(reader.numControlPoints > 0 &&
                reader.controlPoints != null && reader.hitObjects != null &&
                reader.numControlPoints == reader.controlPoints.Count && reader.numObjects == reader.hitObjects.Count))
            {
                throw new Exception("Fetched data is invalid.");
            }
            bool FindInvalid = reader.hitObjects.Any(readerHitObject => readerHitObject.X > 1000 || readerHitObject.X < -1000 || readerHitObject.Y > 1000 || readerHitObject.Y < -1000 ||
            readerHitObject.SegmentCount > 9000 || readerHitObject.Type == 0 || readerHitObject.SampleSet > 1000 ||
            readerHitObject.SampleSetAdditions > 1000 || readerHitObject.SampleVolume > 1000);
            if (FindInvalid) throw new Exception("Find invalid hitObject.");
            // -----------------------

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
            ControlPointLines = reader.controlPoints.Select((cp) => cp.ToString()).ToList();
            HitObjectLines = reader.hitObjects.Select((ho) => new ReaderHitObjectWithSelect(ho.ToString(), ho.IsSelected)).ToList();

            // We don't need breaks because editor force a new combo after every break.
        }

        /// <summary>
        /// Check Editor Reader's data, filter the objects near the editor time and make a copy of its current data.
        /// <para /> Warning: Cause RANDOM ERROR. Should disable backup.
        /// </summary>
        /// <param name="reader">EditorReader</param>
        /// <param name="partialLoadingHalfTimeSpan">The half time span at reader time for filter hitobjects.
        /// <para />Warning: Cause RANDOM ERROR when using it.</param>
        /// <exception cref="Exception">Throw when Editor Reader's data is invalid.</exception>
        public BeatmapInfoCollection(EditorReader reader, double partialLoadingHalfTimeSpan)
        {
            IsFull = false;

            // Check editor reader's data
            if (reader.hitObjects == null)
            {
                throw new Exception("HitObjects is null.");
            }
            // Fix Editor Reader
            // Modified from Mapping_Tools
            // https://github.com/OliBomby/Mapping_Tools/tree/master/Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs
            // Under MIT Licnece https://github.com/OliBomby/Mapping_Tools/blob/master/LICENCE
            if (!(reader.numControlPoints > 0 &&
                reader.controlPoints != null && reader.hitObjects != null &&
                reader.numControlPoints == reader.controlPoints.Count && reader.numObjects == reader.hitObjects.Count))
            {
                throw new Exception("Fetched data is invalid.");
            }

            EditorTime = reader.EditorTime();

            var NearbyHitObjects = FilterNearbyHitObjects(reader.hitObjects, partialLoadingHalfTimeSpan);

            bool FindInvalid = NearbyHitObjects.Any(readerHitObject => readerHitObject.X > 1000 || readerHitObject.X < -1000 || readerHitObject.Y > 1000 || readerHitObject.Y < -1000 ||
            readerHitObject.SegmentCount > 9000 || readerHitObject.Type == 0 || readerHitObject.SampleSet > 1000 ||
            readerHitObject.SampleSetAdditions > 1000 || readerHitObject.SampleVolume > 1000);
            if (FindInvalid) throw new Exception("Find invalid hitObject.");
            // -----------------------

            NumControlPoints = reader.numControlPoints;
            NumObjects = reader.numObjects;
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
            ControlPointLines = reader.controlPoints.Select((cp) => cp.ToString()).ToList();
            HitObjectLines = NearbyHitObjects.Select((ho) => new ReaderHitObjectWithSelect(ho.ToString(), ho.IsSelected)).ToList();

            // We don't need breaks because editor force a new combo after every break.
        }

        /// <summary>
        /// MUCH BOOST BUT IT CAUSE RANDOM ERROR.
        /// </summary>
        private List<Editor_Reader.HitObject> FilterNearbyHitObjects(List<Editor_Reader.HitObject> hitObject, double halfTimeSpan)
        {
            if (EditorTime < 0) return hitObject;
            return hitObject.Where(ho =>
            {
                // keep sliders & spins
                if (EditorTime - ho.StartTime >= 0 && ho.EndTime - EditorTime >= 0) return true;
                // keep the objects which |endtime - nowtime| < 10s, or which starttime - nowtime < 10s
                if (EditorTime - ho.EndTime >= 0 && EditorTime - ho.EndTime <= halfTimeSpan) return true;
                else if (ho.StartTime - EditorTime >= 0 && ho.StartTime - EditorTime <= halfTimeSpan) return true;
                else return false;
            }).ToList();
        }


        /// <summary>
        /// Check difference between two copy of Editor Reader's data.
        /// <para />To determine whether the previous beatmap built can be used directly without the need to rebuild.
        /// </summary>
        /// <param name="other">another BeatmapInfoCollection</param>
        /// <param name="isCheckSelected">Changes in object selection in editor will be considered different if it is true.
        /// <para />Set it to true for reanalyzing when showing selected hitobjects.</param>
        /// <returns>the level of different.</returns>
        public DifferenceType CheckDifference(BeatmapInfoCollection? other, bool isCheckSelected = false)
        {
            if (other is null) return DifferenceType.DifferentFile;
            if (ReferenceEquals(other, this)) return DifferenceType.None;

            if (ContainingFolder != other.ContainingFolder) return DifferenceType.DifferentFile;
            if (Filename != other.Filename) return DifferenceType.DifferentFile;

            if (IsFull != other.IsFull) return DifferenceType.DifferentObjects;

            if (NumControlPoints != other.NumControlPoints) return DifferenceType.DifferentObjects;
            if (NumObjects != other.NumObjects) return DifferenceType.DifferentObjects;

            if (HPDrainRate != other.HPDrainRate) return DifferenceType.DifferentObjects;
            if (CircleSize != other.CircleSize) return DifferenceType.DifferentObjects;
            if (OverallDifficulty != other.OverallDifficulty) return DifferenceType.DifferentObjects;
            if (ApproachRate != other.ApproachRate) return DifferenceType.DifferentObjects;
            if (SliderMultiplier != other.SliderMultiplier) return DifferenceType.DifferentObjects;
            if (SliderTickRate != other.SliderTickRate) return DifferenceType.DifferentObjects;

            if (ControlPointLines.Count != other.ControlPointLines.Count) return DifferenceType.DifferentObjects;
            for (int i = 0; i < ControlPointLines.Count; i++)
            {
                if (ControlPointLines[i] != other.ControlPointLines[i]) return DifferenceType.DifferentObjects;
            }

            if (HitObjectLines.Count != other.HitObjectLines.Count) return DifferenceType.DifferentObjects;
            for (int i = 0; i < HitObjectLines.Count; i++)
            {
                if (!HitObjectLines[i].EqualTo(other.HitObjectLines[i], isCheckSelected)) return DifferenceType.DifferentObjects;
            }

            return DifferenceType.None;
        }
    }

    public class ReaderHitObjectWithSelect
    {
        public string HitObjectLine;
        public bool IsSelect;

        public ReaderHitObjectWithSelect(string hitObjectLine, bool IsSelect)
        {
            HitObjectLine = hitObjectLine;
            this.IsSelect = IsSelect;
        }

        public bool EqualTo(ReaderHitObjectWithSelect? other, bool isCheckSelected = false)
        {
            if (other is null) return false;
            if (ReferenceEquals(other, this)) return true;

            if (isCheckSelected)
            {
                if (HitObjectLine == other.HitObjectLine && IsSelect == other.IsSelect) return true;
            }
            else
            {
                if (HitObjectLine == other.HitObjectLine) return true;
            }
            return false;
        }
    }

    /// <summary>
    /// The level of difference between two BeatmapInfoCollection.
    /// </summary>
    public enum DifferenceType
    {
        /// <summary>
        /// No difference.
        /// </summary>
        None,

        /// <summary>
        /// Same .osu file in disk but changes in hitobjects or beatmap settings etc.
        /// </summary>
        DifferentObjects,

        /// <summary>
        /// Different .osu file in disk.
        /// </summary>
        DifferentFile
    }
}
