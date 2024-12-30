using Editor_Reader;
using Microsoft.Win32;
using SharpCompress.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace osucatch_editor_realtimeviewer
{
    public partial class Form1 : Form
    {
        public static string Path_Settings = "settings.txt";

        public static int Window_Width = app.Default.Window_Width;
        public static int Window_Height = app.Default.Window_Height;
        public static string osu_path = app.Default.osu_path;
        public static bool Backup_Enabled = app.Default.Backup_Enabled;
        public static string Backup_Folder = app.Default.Backup_Folder;
        public static int Backup_Interval = app.Default.Backup_Interval;
        public static int Idle_Interval = app.Default.Idle_Interval;
        public static int Drawing_Interval = app.Default.Drawing_Interval;
        public static bool Combo_Colour = app.Default.Combo_Colour;

        EditorReader reader = new EditorReader();
        bool Is_Osu_Running = false;
        bool Is_Editor_Running = false;
        bool Is_Editor_CTB = false;
        string beatmap_path = "";
        string newBeatmap = "";
        bool Need_Backup = false;
        Int64 LastDrawingTimeStamp = DateTime.Now.Ticks;

        public static string Path_Img_Hitcircle = @"img/fruit-apple.png";
        public static string Path_Img_Drop = @"img/fruit-drop.png";
        public static string Path_Img_Banana = @"img/fruit-bananas.png";

        public static bool NeedReapplySettings = false;

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

        public static void ErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error");
        }

        public static string GetOsuPath()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            if (app.Default.Show_Console) Program.ShowConsole();

            this.Width = Window_Width;
            this.Height = Window_Height;
            SizeChanged += Form1_SizeChanged;

            if (osu_path == "")
            {
                osu_path = GetOsuPath();
                if (osu_path == "")
                {
                    osu_path = Select_Osu_Path();
                }
                app.Default.osu_path = osu_path;
                app.Default.Save();
            }

            reader_timer.Interval = Idle_Interval;
            reader_timer.Start();

            this.Canvas.Init();

            if (Backup_Enabled == true)
            {
                backup_timer.Interval = Backup_Interval;
                backup_timer.Start();
            }
        }

        private async void reader_timer_Tick(object sender, EventArgs e)
        {
            if (NeedReapplySettings)
            {
                this.Width = Window_Width;
                this.Height = Window_Height;

                if (Backup_Enabled && !backup_timer.Enabled)
                {
                    backup_timer.Interval = Backup_Interval;
                    backup_timer.Start();
                }
                else if (!Backup_Enabled && backup_timer.Enabled)
                {
                    backup_timer.Stop();
                }

                NeedReapplySettings = false;
            }

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
                        reader_timer.Stop();
                        reader_timer.Interval = Idle_Interval;
                        Is_Osu_Running = false;
                        Is_Editor_Running = false;
                        beatmap_path = "";
                        reader_timer.Start();
                        return;
                    }
                }
                string title = reader.ProcessTitle();
                if (title == "")
                {
                    this.Text = "Osu!.exe is not running";
                    reader_timer.Stop();
                    reader_timer.Interval = Idle_Interval;
                    Is_Osu_Running = false;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    reader_timer.Start();
                    return;
                }
                if (!title.EndsWith(".osu"))
                {
                    this.Text = "Editor is not running";
                    reader_timer.Stop();
                    reader_timer.Interval = Idle_Interval;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    reader_timer.Start();
                    return;
                }
                if (reader.EditorNeedsReload())
                {
                    try
                    {
                        reader.FetchEditor();
                        Is_Osu_Running = true;
                        Is_Editor_Running = true;
                    }
                    catch
                    {
                        this.Text = "Editor is not running";
                        reader_timer.Stop();
                        reader_timer.Interval = Idle_Interval;
                        Is_Editor_Running = false;
                        beatmap_path = "";
                        reader_timer.Start();
                        return;
                    }
                }
                else
                {
                    this.Text = title;
                    Is_Editor_Running = true;
                    reader_timer.Stop();
                    reader_timer.Interval = Drawing_Interval;
                    reader_timer.Start();
                }

                try
                {
                    await TimeoutCheck.DoOperationWithTimeout(() =>
                    {
                        try
                        {
                            reader.FetchAll();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                            return;
                        }
                    }, TimeSpan.FromSeconds(4));
                }
                catch (TimeoutException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return;
                }

                // Fix Editor Reader
                // Modified from Mapping_Tools
                // https://github.com/OliBomby/Mapping_Tools/tree/master/Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs
                // Under MIT Licnece https://github.com/OliBomby/Mapping_Tools/blob/master/LICENCE
                reader.hitObjects.RemoveAll(readerHitObject => readerHitObject.X > 1000 || readerHitObject.X < -1000 || readerHitObject.Y > 1000 || readerHitObject.Y < -1000 ||
                readerHitObject.SegmentCount > 9000 || readerHitObject.Type == 0 || readerHitObject.SampleSet > 1000 ||
                readerHitObject.SampleSetAdditions > 1000 || readerHitObject.SampleVolume > 1000);
                // -----------------------

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
                // Backup
                if (Need_Backup)
                {
                    if (Is_Editor_Running && newBeatmap != "")
                    {
                        string backupFilePath = Path.Combine(Backup_Folder, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ") + reader.Filename);
                        string directoryPath = Path.GetDirectoryName(backupFilePath);
                        Directory.CreateDirectory(directoryPath);
                        File.WriteAllText(backupFilePath, newBeatmap);
                        Need_Backup = false;
                    }
                }

                // 丢弃旧帧
                if (DateTime.Now.Ticks <= LastDrawingTimeStamp)
                {
                    Console.WriteLine("Drop a frame.");
                    return;
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

            if (DateTime.Now.Ticks > LastDrawingTimeStamp) LastDrawingTimeStamp = DateTime.Now.Ticks;
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

                else if (Regex.IsMatch(line, "^Bookmarks:"))
                {
                    var bookmarks_copy = reader.bookmarks.ToList();
                    newfile.AppendLine("Bookmarks: " + String.Join(",", bookmarks_copy));
                }

                else if (Regex.IsMatch(line, @"^\[TimingPoints\]"))
                {
                    newfile.AppendLine("[TimingPoints]");
                    var controlPoints_copy = reader.controlPoints.ToList();
                    newfile.AppendLine(String.Join("\r\n", controlPoints_copy));
                    newfile.AppendLine();
                    isMultiLine = true;
                }
                else if (Regex.IsMatch(line, @"^\[HitObjects\]"))
                {
                    newfile.AppendLine("[HitObjects]");
                    // newfile.AppendLine(String.Join("\r\n", FilterNearbyHitObjects(reader.hitObjects, editorTime)));
                    var hitObjects_copy = reader.hitObjects.ToList();
                    newfile.AppendLine(String.Join("\r\n", hitObjects_copy));
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
            backup_timer.Stop();
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

        private void githubToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo(@"https://github.com/Exsper/osucatch-editor-realtimeviewer") { UseShellExecute = true });
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Window_Width = this.Width;
            Window_Height = this.Height;
            app.Default.Window_Width = Window_Width;
            app.Default.Window_Height = Window_Height;
            app.Default.Save();
        }

        private void openSettingsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
        }

        private void backup_timer_Tick(object sender, EventArgs e)
        {
            Need_Backup = true;
        }
    }

}
