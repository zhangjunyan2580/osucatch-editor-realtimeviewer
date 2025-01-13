using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace osucatch_editor_realtimeviewer
{
    public static class BeatmapBuilder
    {
        public static string BuildNewBeatmapFromString(string orgbeatmap, BeatmapInfoCollection thisReader)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(orgbeatmap);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader file = new StreamReader(stream);
            return BuildNewBeatmap(file, thisReader);
        }
        public static string BuildNewBeatmapFromFilepath(string orgpath, BeatmapInfoCollection thisReader)
        {
            StreamReader file = File.OpenText(orgpath);
            return BuildNewBeatmap(file, thisReader);
        }

        /*
         * MUCH BOOST BUT IT CAUSE RANDOM ERROR >_<
         * 
        private List<HitObject> FilterNearbyHitObjects(List<HitObject> hitObject, float? editorTime)
        {
            if (editorTime == null) return hitObject;
            double halfDuring = 10 * 1000;
            if (hRToolStripMenuItem.Checked) halfDuring /= 1.5;
            else if (eZToolStripMenuItem.Checked) halfDuring *= 2;
            return hitObject.Where(ho =>
            {
                // keep sliders & spins
                if (editorTime - ho.StartTime >= 0 && ho.EndTime - editorTime >= 0) return true;
                // keep the objects which |endtime - nowtime| < 10s, or which starttime - nowtime < 10s
                if (editorTime - ho.EndTime >= 0 && editorTime - ho.EndTime <= halfDuring) return true;
                else if (ho.StartTime - editorTime >= 0 && ho.StartTime - editorTime <= halfDuring) return true;
                else return false;
            }).ToList();
        }
        */


        private static string BuildNewBeatmap(StreamReader file, BeatmapInfoCollection thisReader)
        {
            StringBuilder newfile = new StringBuilder();
            string? line;
            bool isMultiLine = false;
            while ((line = file.ReadLine()) != null)
            {
                if (!line.StartsWith("Tags") && line.Length > 1000)
                {
                    // Known bug: ":0|0" repeat
                    if (line.Length > 10000 && line.IndexOf(":0|0:0|0:0|0:0|0:0|0:0|0:0|0:0|0") > 0)
                    {
                        throw new Exception("Found an incorrect \":0|0 repeat\" line.");
                    }
                    Log.ConsoleLog("Maybe an incorrect line: " + line, Log.LogType.BeatmapParser, Log.LogLevel.Debug);
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
                if (Regex.IsMatch(line, "^PreviewTime:")) newfile.AppendLine("PreviewTime: " + thisReader.PreviewTime);
                else if (Regex.IsMatch(line, "^StackLeniency:")) newfile.AppendLine("StackLeniency: " + thisReader.StackLeniency);

                // force ctb mode
                // if (Regex.IsMatch(line, "^Mode:")) newfile += "Mode: 2" + "\r\n";

                else if (Regex.IsMatch(line, "^HPDrainRate:")) newfile.AppendLine("HPDrainRate: " + thisReader.HPDrainRate);
                else if (Regex.IsMatch(line, "^CircleSize:")) newfile.AppendLine("CircleSize: " + thisReader.CircleSize);
                else if (Regex.IsMatch(line, "^OverallDifficulty:")) newfile.AppendLine("OverallDifficulty: " + thisReader.OverallDifficulty);
                else if (Regex.IsMatch(line, "^ApproachRate:")) newfile.AppendLine("ApproachRate: " + thisReader.ApproachRate);

                else if (Regex.IsMatch(line, "^SliderMultiplier:")) newfile.AppendLine("SliderMultiplier: " + thisReader.SliderMultiplier);
                else if (Regex.IsMatch(line, "^SliderTickRate:")) newfile.AppendLine("SliderTickRate: " + thisReader.SliderTickRate);

                else if (Regex.IsMatch(line, "^Bookmarks:"))
                {
                    newfile.Append("Bookmarks: ");
                    for (int i = 0; i < thisReader.Bookmarks.Length; i++)
                    {
                        if (i > 0) newfile.Append(',');
                        newfile.Append(thisReader.Bookmarks[i]);
                    }
                    newfile.Append("\r\n");
                }

                else if (Regex.IsMatch(line, @"^\[TimingPoints\]"))
                {
                    newfile.AppendLine("[TimingPoints]");
                    for (int i = 0; i < thisReader.ControlPoints.Count; i++)
                    {
                        newfile.AppendLine(thisReader.ControlPoints[i].ToString());
                    }
                    isMultiLine = true;
                }
                else if (Regex.IsMatch(line, @"^\[HitObjects\]"))
                {
                    newfile.AppendLine("[HitObjects]");
                    // newfile.AppendLine(String.Join("\r\n", FilterNearbyHitObjects(thisReader.HitObjects, editorTime)));
                    for (int i = 0; i < thisReader.HitObjects.Count; i++)
                    {
                        newfile.AppendLine(thisReader.HitObjects[i].ToString());
                    }
                    isMultiLine = true;
                }
                else newfile.AppendLine(line);
            }
            return newfile.ToString();
        }
    }
}
