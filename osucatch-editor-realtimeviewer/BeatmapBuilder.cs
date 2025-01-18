using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using System.Text;
using System.Text.RegularExpressions;

namespace osucatch_editor_realtimeviewer
{
    public static class BeatmapBuilder
    {
        private static Decoder<Beatmap> beatmapDecoder => new LegacyBeatmapDecoder();

        #region Build beatmap file (for backup)

        public static string BuildNewBeatmapFileFromFilepath(string orgpath, BeatmapInfoCollection thisReaderData)
        {
            StreamReader file = File.OpenText(orgpath);
            return BuildNewBeatmapFile(file, thisReaderData);
        }

        private static string BuildNewBeatmapFile(StreamReader file, BeatmapInfoCollection thisReaderData)
        {
            StringBuilder newfile = new StringBuilder();
            string? line;
            bool isMultiLine = false;
            bool hasTiming = false;
            while ((line = file.ReadLine()) != null)
            {
                if (!line.StartsWith("Tags") && line.Length > 1000)
                {
                    // Known bug: ":0|0" repeat
                    if (line.Length > 10000 && line.IndexOf(":0|0:0|0:0|0:0|0:0|0:0|0:0|0:0|0") > 0)
                    {
                        throw new Exception("Found an incorrect \":0|0 repeat\" line.");
                    }
                    Log.ConsoleLog("Maybe an incorrect line: " + line, Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);
                }

                if (isMultiLine)
                {
                    if (line.StartsWith("["))
                    {
                        isMultiLine = false;
                    }
                    else continue;
                }

                // replace necessary things
                if (Regex.IsMatch(line, "^PreviewTime:")) newfile.AppendLine("PreviewTime: " + thisReaderData.PreviewTime);
                else if (Regex.IsMatch(line, "^StackLeniency:")) newfile.AppendLine("StackLeniency: " + thisReaderData.StackLeniency);

                // force ctb mode
                // if (Regex.IsMatch(line, "^Mode:")) newfile += "Mode: 2" + "\r\n";

                else if (Regex.IsMatch(line, "^HPDrainRate:")) newfile.AppendLine("HPDrainRate:" + thisReaderData.HPDrainRate);
                else if (Regex.IsMatch(line, "^CircleSize:")) newfile.AppendLine("CircleSize:" + thisReaderData.CircleSize);
                else if (Regex.IsMatch(line, "^OverallDifficulty:")) newfile.AppendLine("OverallDifficulty:" + thisReaderData.OverallDifficulty);
                else if (Regex.IsMatch(line, "^ApproachRate:")) newfile.AppendLine("ApproachRate:" + thisReaderData.ApproachRate);

                else if (Regex.IsMatch(line, "^SliderMultiplier:")) newfile.AppendLine("SliderMultiplier:" + thisReaderData.SliderMultiplier);
                else if (Regex.IsMatch(line, "^SliderTickRate:")) newfile.AppendLine("SliderTickRate:" + thisReaderData.SliderTickRate);

                else if (Regex.IsMatch(line, "^Bookmarks:"))
                {
                    newfile.Append("Bookmarks: ");
                    for (int i = 0; i < thisReaderData.Bookmarks.Length; i++)
                    {
                        if (i > 0) newfile.Append(',');
                        newfile.Append(thisReaderData.Bookmarks[i]);
                    }
                    newfile.Append("\r\n");
                }

                else if (Regex.IsMatch(line, @"^\[TimingPoints\]"))
                {
                    hasTiming = true;
                    newfile.AppendLine("[TimingPoints]");
                    for (int i = 0; i < thisReaderData.ControlPointLines.Count; i++)
                    {
                        newfile.AppendLine(thisReaderData.ControlPointLines[i]);
                    }
                    isMultiLine = true;
                }
                else if (Regex.IsMatch(line, @"^\[HitObjects\]"))
                {
                    // fix when no timing
                    if (!hasTiming)
                    {
                        newfile.AppendLine("[TimingPoints]");
                        for (int i = 0; i < thisReaderData.ControlPointLines.Count; i++)
                        {
                            newfile.AppendLine(thisReaderData.ControlPointLines[i]);
                        }
                        newfile.AppendLine("\r\n");
                    }
                    newfile.AppendLine("[HitObjects]");
                    for (int i = 0; i < thisReaderData.HitObjectLines.Count; i++)
                    {
                        newfile.AppendLine(thisReaderData.HitObjectLines[i].HitObjectLine);
                    }
                    isMultiLine = true;
                }
                else newfile.AppendLine(line);
            }
            return newfile.ToString();
        }

        #endregion

        #region Build beatmap

        private static List<string> GetColourLinesFromBeatmapFilepath(string orgpath)
        {
            StreamReader file = File.OpenText(orgpath);
            StringBuilder newfile = new StringBuilder();
            List<string> colourLines = new List<string>();
            string? line;
            while ((line = file.ReadLine()) != null)
            {
                if (Regex.IsMatch(line, @"^\[Colours\]"))
                {
                    string? innerLine;
                    while ((innerLine = file.ReadLine()) != null)
                    {
                        if (innerLine.StartsWith("[")) break;
                        if (innerLine.Trim() == "") continue;
                        colourLines.Add(innerLine);
                    }
                    return colourLines;
                }
            }
            return colourLines;
        }

        public static Beatmap? BuildNewBeatmapWithFilePath(BeatmapInfoCollection thisReaderData, string beatmappath, out List<string>? colourLines)
        {
            try
            {
                colourLines = GetColourLinesFromBeatmapFilepath(beatmappath);
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("Can not read colors from path: " + beatmappath + "\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Warning);
                colourLines = null;
            }
            return BuildNewBeatmapWithColorString(thisReaderData, colourLines);
        }

        public static Beatmap? BuildNewBeatmapWithColorString(BeatmapInfoCollection thisReaderData, List<string>? colourLines)
        {
            return BuildNewBeatmap(thisReaderData, colourLines);
        }

        private static Beatmap? BuildNewBeatmap(BeatmapInfoCollection thisReaderData, List<string>? colourLines)
        {
            try
            {
                Log.ConsoleLog("Building beatmap.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);
                var beatmap = beatmapDecoder.Decode(thisReaderData, colourLines);
                return beatmap;
            }
            catch (Exception ex)
            {
                Log.ConsoleLog("Building beatmap failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                return null;
            }
        }

        #endregion
    }
}
