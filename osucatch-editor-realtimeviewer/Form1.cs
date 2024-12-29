using Editor_Reader;
using Microsoft.Win32;
using System.Text;
using System.Text.RegularExpressions;

namespace osucatch_editor_realtimeviewer
{
    public partial class Form1 : Form
    {
        EditorReader reader = new EditorReader();
        bool Is_Osu_Running = false;
        bool Is_Editor_Running = false;
        bool Is_Editor_CTB = false;
        string osu_path = "";
        string beatmap_path = "";
        string newBeatmap = "";

        public static string Path_Img_Hitcircle = @"img/fruit-apple.png";
        public static string Path_Img_Drop = @"img/fruit-drop.png";
        public static string Path_Img_Banana = @"img/fruit-bananas.png";

        public Form1()
        {
            InitializeComponent();
        }

        private string Select_Osu_Path()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = false;
            folder.RootFolder = Environment.SpecialFolder.MyComputer;
            folder.Description = "Select osu! Folder";
            DialogResult path = folder.ShowDialog();
            if (path == DialogResult.OK)
            {
                //check if osu!.exe is present
                if (!File.Exists(System.IO.Path.Combine(folder.SelectedPath, "osu!.exe")))
                {
                    MessageBox.Show("No osu!.exe in this folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return Select_Osu_Path();
                }
            }
            return folder.SelectedPath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            osu_path = GetOsuPath();
            if (osu_path == "") osu_path = Select_Osu_Path();

            reader_timer.Interval = 1000;
            reader_timer.Start();

            this.Canvas.Init();
        }

        private void reader_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Is_Osu_Running || reader.ProcessNeedsReload())
                {
                    try
                    {
                        reader.SetProcess();
                        Is_Osu_Running = true;
                    }
                    catch
                    {
                        this.Text = "Osu!.exe is not running";
                        reader_timer.Interval = 1000;
                        Is_Osu_Running = false;
                        Is_Editor_Running = false;
                        beatmap_path = "";
                        return;
                    }
                }
                string title = reader.ProcessTitle();
                if (title == "")
                {
                    this.Text = "Osu!.exe is not running";
                    reader_timer.Interval = 1000;
                    Is_Osu_Running = false;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    return;
                }
                if (!title.EndsWith(".osu"))
                {
                    this.Text = "Editor is not running";
                    reader_timer.Interval = 1000;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    return;
                }
                if (reader.EditorNeedsReload())
                {
                    try
                    {
                        reader.SetEditor();
                        Is_Osu_Running = true;
                        Is_Editor_Running = true;
                    }
                    catch
                    {
                        this.Text = "Editor is not running";
                        reader_timer.Interval = 1000;
                        Is_Editor_Running = false;
                        beatmap_path = "";
                        return;
                    }
                }
                else
                {
                    this.Text = title;
                    Is_Editor_Running = true;
                    reader_timer.Interval = 20;
                }
                reader.FetchAll();

                string newpath = System.IO.Path.Combine(osu_path, "Songs", reader.ContainingFolder, reader.Filename);
                float readerTime = reader.EditorTime();

                // 新文件
                if (beatmap_path != newpath || newBeatmap == "")
                {
                    beatmap_path = newpath;
                    newBeatmap = BuildNewBeatmapFromFilepath(beatmap_path);
                }
                else
                {
                    newBeatmap = BuildNewBeatmapFromString(newBeatmap);
                }
                // 使用官方库分析新谱面
                int mods = 0;
                if (hRToolStripMenuItem.Checked) mods = (1 << 4);
                else if (eZToolStripMenuItem.Checked) mods = (1 << 1);
                if (this.Canvas.viewerManager == null) this.Canvas.viewerManager = new ViewerManager(newBeatmap, mods);
                else this.Canvas.viewerManager.LoadBeatmap(newBeatmap, mods);
                this.Canvas.viewerManager.currentTime = readerTime;
                this.Canvas.Canvas_Paint(sender, null);
            }
            catch (Exception ex)
            {
                this.Text = ex.ToString();
                Is_Osu_Running = false;
                Is_Editor_Running = false;
            }
        }

        private static string GetOsuPath()
        {
            using (RegistryKey osureg = Registry.ClassesRoot.OpenSubKey("osu\\DefaultIcon"))
            {
                if (osureg != null)
                {
                    string osukey = osureg.GetValue(null).ToString();
                    string osupath = osukey.Remove(0, 1);
                    osupath = osupath.Remove(osupath.Length - 11);
                    return osupath;
                }
                else return "";
            }
        }

        private string BuildNewBeatmapFromString(string orgbeatmap)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(orgbeatmap);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader file = new StreamReader(stream);
            return BuildNewBeatmap(file);
        }
        private string BuildNewBeatmapFromFilepath(string orgpath)
        {
            StreamReader file = File.OpenText(orgpath);
            return BuildNewBeatmap(file);
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
                // 保留当前滑条和spin
                if (editorTime - ho.StartTime >= 0 && ho.EndTime - editorTime >= 0) return true;
                // 只保留Endtime距当前时间小于10秒前的物件，或者StartTime距当前时间小于10秒后的物件
                if (editorTime - ho.EndTime >= 0 && editorTime - ho.EndTime <= halfDuring) return true;
                else if (ho.StartTime - editorTime >= 0 && ho.StartTime - editorTime <= halfDuring) return true;
                else return false;
            }).ToList();
        }
        */


        private string BuildNewBeatmap(StreamReader file)
        {
            StringBuilder newfile = new StringBuilder();
            string line;
            bool isMultiLine = false;
            while ((line = file.ReadLine()) != null)
            {
                if (isMultiLine)
                {
                    if (line.StartsWith("["))
                    {
                        isMultiLine = false;
                    }
                    else continue;
                }

                // 只替换必要的东西
                if (Regex.IsMatch(line, "^PreviewTime:")) newfile.AppendLine("PreviewTime: " + reader.PreviewTime);
                else if (Regex.IsMatch(line, "^StackLeniency:")) newfile.AppendLine("StackLeniency: " + reader.StackLeniency);

                // 强制CTB模式
                // if (Regex.IsMatch(line, "^Mode:")) newfile += "Mode: 2" + "\r\n";

                else if (Regex.IsMatch(line, "^HPDrainRate:")) newfile.AppendLine("HPDrainRate: " + reader.HPDrainRate);
                else if (Regex.IsMatch(line, "^CircleSize:")) newfile.AppendLine("CircleSize: " + reader.CircleSize);
                else if (Regex.IsMatch(line, "^OverallDifficulty:")) newfile.AppendLine("OverallDifficulty: " + reader.OverallDifficulty);
                else if (Regex.IsMatch(line, "^ApproachRate:")) newfile.AppendLine("ApproachRate: " + reader.ApproachRate);

                else if (Regex.IsMatch(line, "^SliderMultiplier:")) newfile.AppendLine("SliderMultiplier: " + reader.SliderMultiplier);
                else if (Regex.IsMatch(line, "^SliderTickRate:")) newfile.AppendLine("SliderTickRate: " + reader.SliderTickRate);

                else if (Regex.IsMatch(line, @"^\[TimingPoints\]"))
                {
                    newfile.AppendLine("[TimingPoints]");
                    newfile.AppendLine(String.Join("\r\n", reader.controlPoints));
                    newfile.AppendLine();
                    isMultiLine = true;
                }
                else if (Regex.IsMatch(line, @"^\[HitObjects\]"))
                {
                    newfile.AppendLine("[HitObjects]");
                    // newfile.AppendLine(String.Join("\r\n", FilterNearbyHitObjects(reader.hitObjects, editorTime)));
                    newfile.AppendLine(String.Join("\r\n", reader.hitObjects));
                    newfile.AppendLine();
                    isMultiLine = true;
                }
                else newfile.AppendLine(line);
            }
            return newfile.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            reader_timer.Stop();
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = true;
            hRToolStripMenuItem.Checked = false;
            eZToolStripMenuItem.Checked = false;
        }

        private void hRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            hRToolStripMenuItem.Checked = true;
            eZToolStripMenuItem.Checked = false;
        }

        private void eZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            hRToolStripMenuItem.Checked = false;
            eZToolStripMenuItem.Checked = true;
        }
    }

}
