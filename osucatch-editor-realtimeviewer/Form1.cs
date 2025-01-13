using Editor_Reader;
using Microsoft.Win32;
using osu.Game.Beatmaps;
using System.Diagnostics;
using System.Drawing;
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
        bool Is_Doing_SetProcess = false;
        bool Is_Doing_FetchEditor = false;
        bool Is_Osu_Running = false;
        bool Is_Editor_Running = false;
        string beatmap_path = "";
        string lastBeatmap = "";
        string newBeatmap = "";
        int lastMods = -1;
        DistanceType lastDistanceType = DistanceType.None;
        bool Need_Backup = false;
        Int64 LastDrawingTimeStamp = DateTime.Now.Ticks;
        int dpi = 96;
        float fontscale = 1;

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

        public static string GetOsuPath()
        {
            using (RegistryKey? osureg = Registry.ClassesRoot.OpenSubKey("osu\\DefaultIcon"))
            {
                if (osureg != null)
                {
                    string? osukey = osureg.GetValue(null)?.ToString();
                    if (osukey == null) return "";
                    string osupath = osukey.Remove(0, 1);
                    osupath = osupath.Remove(osupath.Length - 11);
                    return osupath;
                }
                else
                {
                    Log.ConsoleLog("Could not find osu path from registry.", Log.LogType.Program, Log.LogLevel.Warning);
                    return "";
                }
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

            Graphics graphics = this.CreateGraphics();
            dpi = (Int32)graphics.DpiX;
            Log.ConsoleLog("DPI: " + dpi, Log.LogType.Program, Log.LogLevel.Info);
            fontscale = 96f / dpi;
            Log.ConsoleLog("Text Scale x" + fontscale.ToString("F2"), Log.LogType.Program, Log.LogLevel.Info);
            this.Canvas.fontScale = fontscale;

            this.Canvas.screensContain = app.Default.ScreensContain;
            ToolStripMenuItem[] screensMenuItems = {
                Screens1ToolStripMenuItem,
                Screens2ToolStripMenuItem,
                Screens3ToolStripMenuItem,
                Screens4ToolStripMenuItem,
                Screens5ToolStripMenuItem,
                Screens6ToolStripMenuItem,
                Screens7ToolStripMenuItem,
                Screens8ToolStripMenuItem,
            };
            for (int i = 0; i < screensMenuItems.Length; i++)
            {
                if (i == app.Default.ScreensContain - 1) screensMenuItems[i].Checked = true;
                else screensMenuItems[i].Checked = false;
            }

            reader_timer.Interval = Idle_Interval;
            reader_timer.Start();

            this.Canvas.Init();

            if (Backup_Enabled == true)
            {
                backup_timer.Interval = Backup_Interval;
                backup_timer.Start();
            }

            System.Windows.Forms.Timer Memory_Monitor_Timer = new System.Windows.Forms.Timer();
            Memory_Monitor_Timer.Interval = 200;
            Memory_Monitor_Timer.Tick += Memory_Monitor;
            Memory_Monitor_Timer.Start();

        }

        private void Memory_Monitor(object? sender, EventArgs e)
        {
            long memorySize = System.GC.GetTotalMemory(false);
            long requiredMemory = 1024 * 1024 * 1000; // 1G

            if (memorySize > requiredMemory)
            {
                Log.ConsoleLog("Total Memory: " + (1.0 * memorySize / 1024 / 1024).ToString("F3") + "MB", Log.LogType.Program, Log.LogLevel.Warning);
                //ConsoleLog(newBeatmap, LogType.BeatmapBuilder, LogLevel.Warning);
            }
            else
            {
                Log.ConsoleLog("Total Memory: " + (1.0 * memorySize / 1024 / 1024).ToString("F3") + "MB", Log.LogType.Program, Log.LogLevel.Debug);
            }
        }

        private void reader_timer_Work(CancellationToken cancellationToken)
        {
            if (NeedReapplySettings)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    this.Width = Window_Width;
                    this.Height = Window_Height;

                }));
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
                        Log.ConsoleLog("Osu! process needs Refetch.", Log.LogType.EditorReader, Log.LogLevel.Info);
                        if (Is_Doing_SetProcess)
                        {
                            Log.ConsoleLog("Already fetching osu!.", Log.LogType.EditorReader, Log.LogLevel.Info);
                            reader_timer.Interval = Idle_Interval;
                            return;
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
                        Invoke(new MethodInvoker(delegate ()
                        {
                            this.Text = "Osu!.exe is not running";
                        }));
                        reader_timer.Interval = Idle_Interval;
                        Is_Osu_Running = false;
                        Is_Editor_Running = false;
                        beatmap_path = "";
                        return;
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before fetching editor.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                string title = reader.ProcessTitle();
                if (title == "")
                {
                    Log.ConsoleLog("Empty osu title.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Text = "Osu!.exe is not running";
                    }));
                    reader_timer.Interval = Idle_Interval;
                    Is_Osu_Running = false;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    return;
                }
                if (!title.EndsWith(".osu"))
                {
                    Log.ConsoleLog("Osu title is not editor: " + title, Log.LogType.EditorReader, Log.LogLevel.Info);
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Text = "Editor is not running";
                    }));
                    reader_timer.Interval = Idle_Interval;
                    Is_Editor_Running = false;
                    beatmap_path = "";
                    return;
                }
                if (reader.EditorNeedsReload())
                {
                    Log.ConsoleLog("Editor needs Reload.", Log.LogType.EditorReader, Log.LogLevel.Info);
                    try
                    {
                        if (Is_Doing_SetProcess || Is_Doing_FetchEditor)
                        {
                            Log.ConsoleLog("Already reloading editor.", Log.LogType.EditorReader, Log.LogLevel.Info);
                            reader_timer.Interval = Idle_Interval;
                            return;
                        }
                        if (reader.ProcessNeedsReload())
                        {
                            Log.ConsoleLog("Process needs Reload.", Log.LogType.EditorReader, Log.LogLevel.Info);
                            return;
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
                        Invoke(new MethodInvoker(delegate ()
                        {
                            this.Text = "Editor is not running";
                        }));
                        reader_timer.Interval = Idle_Interval;
                        Is_Editor_Running = false;
                        beatmap_path = "";
                        return;
                    }
                }
                else
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Text = title;
                    }));
                    Is_Editor_Running = true;
                    if (reader_timer.Interval != Drawing_Interval)
                    {
                        reader_timer.Interval = Drawing_Interval;
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before FetchAll().", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                Log.ConsoleLog("Start FetchAll().", Log.LogType.EditorReader, Log.LogLevel.Debug);

                try
                {
                    reader.FetchAll();
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("FetchAll failed.\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                    reader_timer.Interval = Idle_Interval;
                    return;
                }


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before checking Editor_Reader's data.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                // 读取editor reader的内容一定要进行检查
                // reader数据在程序运行种可能会变的

                var thisReader = new BeatmapInfoCollection(reader);

                string newpath = "";
                try
                {
                    newpath = Path.Combine(osu_path, "Songs", thisReader.ContainingFolder, thisReader.Filename);
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Path is invalid.\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                    Log.ConsoleLog("ContainingFolder: " + thisReader.ContainingFolder, Log.LogType.EditorReader, Log.LogLevel.Error);
                    Log.ConsoleLog("Filename: " + thisReader.Filename, Log.LogType.EditorReader, Log.LogLevel.Error);
                    return;
                }

                float readerTime = thisReader.EditorTime;

                if (thisReader.HitObjects == null || thisReader.HitObjects.Count <= 0)
                {
                    Log.ConsoleLog("HitObjects is empty.", Log.LogType.EditorReader, Log.LogLevel.Warning);
                    return;
                }

                Log.ConsoleLog("FetchAll complete.", Log.LogType.EditorReader, Log.LogLevel.Debug);

                // Fix Editor Reader
                // Modified from Mapping_Tools
                // https://github.com/OliBomby/Mapping_Tools/tree/master/Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs
                // Under MIT Licnece https://github.com/OliBomby/Mapping_Tools/blob/master/LICENCE
                if (!(thisReader.NumControlPoints > 0 &&
                thisReader.ControlPoints != null && thisReader.HitObjects != null &&
                thisReader.NumControlPoints == thisReader.ControlPoints.Count && thisReader.NumObjects == thisReader.HitObjects.Count))
                {
                    Log.ConsoleLog("Fetched data is invalid.", Log.LogType.EditorReader, Log.LogLevel.Error);
                    return;
                }

                int removeCount = thisReader.HitObjects.RemoveAll(readerHitObject => readerHitObject.X > 1000 || readerHitObject.X < -1000 || readerHitObject.Y > 1000 || readerHitObject.Y < -1000 ||
                readerHitObject.SegmentCount > 9000 || readerHitObject.Type == 0 || readerHitObject.SampleSet > 1000 ||
                readerHitObject.SampleSetAdditions > 1000 || readerHitObject.SampleVolume > 1000);
                // -----------------------

                if (removeCount > 0) Log.ConsoleLog("Removed " + removeCount + " invalid hitObject(s).", Log.LogType.BeatmapBuilder, Log.LogLevel.Warning);

                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before building new beatmap.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                Log.ConsoleLog("Start build new beatmap.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);

                // 新文件
                if (beatmap_path != newpath || lastBeatmap == "")
                {
                    beatmap_path = newpath;
                    try
                    {
                        newBeatmap = BuildNewBeatmapFromFilepath(beatmap_path, thisReader);
                    }
                    catch (Exception ex)
                    {
                        Log.ConsoleLog("Build new beatmap from file failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                        return;
                    }
                }
                else
                {
                    try
                    {
                        newBeatmap = BuildNewBeatmapFromString(newBeatmap, thisReader);
                    }
                    catch (Exception ex)
                    {
                        Log.ConsoleLog("Build new beatmap from string failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                        return;
                    }
                }

                Log.ConsoleLog("Build new beatmap successfully.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);

                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before backup or beatmap prasing.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                bool isSameBeatmap = false;
                bool isSameMods = false;
                bool isSameDistanceType = false;

                if (string.Compare(newBeatmap, lastBeatmap, StringComparison.Ordinal) == 0)
                {
                    isSameBeatmap = true;
                }
                else
                {
                    lastBeatmap = newBeatmap;
                }

                int mods = 0;
                if (hRToolStripMenuItem.Checked) mods = (1 << 4);
                else if (eZToolStripMenuItem.Checked) mods = (1 << 1);
                if (mods == lastMods)
                {
                    isSameMods = true;
                }
                else
                {
                    lastMods = mods;
                }

                if (this.Canvas.viewerManager != null)
                {
                    if (hideToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.None;
                    else if (sameWithEditorToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.SameWithEditor;
                    else if (noSliderVelocityMultiplierToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.NoSliderVelocityMultiplier;
                    else if (compareWithWalkSpeedToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.CompareWithWalkSpeed;
                    else this.Canvas.viewerManager.DistanceType = DistanceType.None;
                    if (this.Canvas.viewerManager.DistanceType == lastDistanceType)
                    {
                        isSameDistanceType = true;
                    }
                }

                // Backup
                if (!isSameBeatmap && Need_Backup)
                {
                    if (Is_Editor_Running && newBeatmap != "")
                    {
                        try
                        {
                            Log.ConsoleLog("Start backup.", Log.LogType.Backup, Log.LogLevel.Info);
                            string backupFilePath = Path.Combine(Backup_Folder, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ") + thisReader.Filename);
                            string? directoryPath = Path.GetDirectoryName(backupFilePath);
                            if (directoryPath == null)
                            {
                                Log.ConsoleLog("Backup failed. Path is invalid: " + backupFilePath, Log.LogType.Backup, Log.LogLevel.Error);
                            }
                            else
                            {
                                Directory.CreateDirectory(directoryPath);
                                File.WriteAllText(backupFilePath, newBeatmap);
                                Need_Backup = false;
                                Log.ConsoleLog("Backup successfully.", Log.LogType.Backup, Log.LogLevel.Info);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.ConsoleLog("Backup failed.\r\n" + ex.ToString(), Log.LogType.Backup, Log.LogLevel.Error);
                        }
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before beatmap prasing.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                // 丢弃旧帧
                if (DateTime.Now.Ticks <= LastDrawingTimeStamp)
                {
                    Log.ConsoleLog("Drop an outdated data.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }

                if (this.Canvas.viewerManager != null && isSameBeatmap && isSameMods && isSameDistanceType)
                {
                    Log.ConsoleLog("Beatmap no changes. Using last data.", Log.LogType.BeatmapParser, Log.LogLevel.Debug);
                }
                else
                {
                    Log.ConsoleLog("Try parse beatmap.", Log.LogType.BeatmapParser, Log.LogLevel.Debug);
                    if (this.Canvas.viewerManager == null) this.Canvas.viewerManager = new ViewerManager(newBeatmap, mods);
                    else this.Canvas.viewerManager.LoadBeatmap(newBeatmap, mods);
                    lastDistanceType = this.Canvas.viewerManager.DistanceType;

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Log.ConsoleLog("Task Timeout, cancelled before drawing.", Log.LogType.Program, Log.LogLevel.Warning);
                        return;
                    }

                    Log.ConsoleLog("Parse beatmap successfully.", Log.LogType.BeatmapParser, Log.LogLevel.Debug);
                }

                this.Canvas.viewerManager.currentTime = readerTime;
                Log.ConsoleLog("Start paint.", Log.LogType.Drawing, Log.LogLevel.Debug);

                Invoke(new MethodInvoker(delegate ()
                {
                    this.Canvas.Canvas_Paint(null, null);
                }));

                Log.ConsoleLog("Paint a frame successful.", Log.LogType.Drawing, Log.LogLevel.Debug);
            }
            catch (Exception ex)
            {
                Log.ConsoleLog(ex.ToString(), Log.LogType.Program, Log.LogLevel.Error);
                Is_Osu_Running = false;
                Is_Editor_Running = false;
            }

            if (DateTime.Now.Ticks > LastDrawingTimeStamp) LastDrawingTimeStamp = DateTime.Now.Ticks;
        }

        private async void reader_timer_Tick(object sender, EventArgs e)
        {
            reader_timer.Stop();
            var cancellationTokenSource = new CancellationTokenSource();
            var task = Task.Run(() => reader_timer_Work(cancellationTokenSource.Token), cancellationTokenSource.Token);

            var isCompleted = await Task.WhenAny(task, Task.Delay(1000)) == task;
            if (!isCompleted) cancellationTokenSource.Cancel();
            Log.ConsoleLog("Start Timer", Log.LogType.Timer, Log.LogLevel.Debug);
            Log.ConsoleLog("Timer Interval = " + reader_timer.Interval, Log.LogType.Timer, Log.LogLevel.Debug);
            reader_timer.Start();
        }


        private string BuildNewBeatmapFromString(string orgbeatmap, BeatmapInfoCollection thisReader)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(orgbeatmap);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader file = new StreamReader(stream);
            return BuildNewBeatmap(file, thisReader);
        }
        private string BuildNewBeatmapFromFilepath(string orgpath, BeatmapInfoCollection thisReader)
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
                // 保留当前滑条和spin
                if (editorTime - ho.StartTime >= 0 && ho.EndTime - editorTime >= 0) return true;
                // 只保留Endtime距当前时间小于10秒前的物件，或者StartTime距当前时间小于10秒后的物件
                if (editorTime - ho.EndTime >= 0 && editorTime - ho.EndTime <= halfDuring) return true;
                else if (ho.StartTime - editorTime >= 0 && ho.StartTime - editorTime <= halfDuring) return true;
                else return false;
            }).ToList();
        }
        */


        private string BuildNewBeatmap(StreamReader file, BeatmapInfoCollection thisReader)
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

                // 只替换必要的东西
                if (Regex.IsMatch(line, "^PreviewTime:")) newfile.AppendLine("PreviewTime: " + thisReader.PreviewTime);
                else if (Regex.IsMatch(line, "^StackLeniency:")) newfile.AppendLine("StackLeniency: " + thisReader.StackLeniency);

                // 强制CTB模式
                // if (Regex.IsMatch(line, "^Mode:")) newfile += "Mode: 2" + "\r\n";

                else if (Regex.IsMatch(line, "^HPDrainRate:")) newfile.AppendLine("HPDrainRate: " + thisReader.HPDrainRate);
                else if (Regex.IsMatch(line, "^CircleSize:")) newfile.AppendLine("CircleSize: " + thisReader.CircleSize);
                else if (Regex.IsMatch(line, "^OverallDifficulty:")) newfile.AppendLine("OverallDifficulty: " + thisReader.OverallDifficulty);
                else if (Regex.IsMatch(line, "^ApproachRate:")) newfile.AppendLine("ApproachRate: " + thisReader.ApproachRate);

                else if (Regex.IsMatch(line, "^SliderMultiplier:")) newfile.AppendLine("SliderMultiplier: " + thisReader.SliderMultiplier);
                else if (Regex.IsMatch(line, "^SliderTickRate:")) newfile.AppendLine("SliderTickRate: " + thisReader.SliderTickRate);

                else if (Regex.IsMatch(line, "^Bookmarks:"))
                {
                    newfile.AppendLine("Bookmarks: " + String.Join(",", thisReader.Bookmarks));
                }

                else if (Regex.IsMatch(line, @"^\[TimingPoints\]"))
                {
                    newfile.AppendLine("[TimingPoints]");
                    newfile.AppendLine(String.Join("\r\n", thisReader.ControlPoints));
                    newfile.AppendLine();
                    isMultiLine = true;
                }
                else if (Regex.IsMatch(line, @"^\[HitObjects\]"))
                {
                    newfile.AppendLine("[HitObjects]");
                    // newfile.AppendLine(String.Join("\r\n", FilterNearbyHitObjects(thisReader.HitObjects, editorTime)));
                    newfile.AppendLine(String.Join("\r\n", thisReader.HitObjects));
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

        private void Form1_SizeChanged(object? sender, EventArgs e)
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

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = true;
            sameWithEditorToolStripMenuItem.Checked = false;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = false;
            compareWithWalkSpeedToolStripMenuItem.Checked = false;
        }

        private void sameWithEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = true;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = false;
            compareWithWalkSpeedToolStripMenuItem.Checked = false;
        }

        private void noSliderVelocityMultiplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = false;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = true;
            compareWithWalkSpeedToolStripMenuItem.Checked = false;
        }

        private void compareWithWalkSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = false;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = false;
            compareWithWalkSpeedToolStripMenuItem.Checked = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Screens1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = true;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 1;
            app.Default.Save();
            this.Canvas.screensContain = 1;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = true;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 2;
            app.Default.Save();
            this.Canvas.screensContain = 2;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = true;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 3;
            app.Default.Save();
            this.Canvas.screensContain = 3;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = true;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 4;
            app.Default.Save();
            this.Canvas.screensContain = 4;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = true;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 5;
            app.Default.Save();
            this.Canvas.screensContain = 5;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens6ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = true;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 6;
            app.Default.Save();
            this.Canvas.screensContain = 6;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens7ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = true;
            Screens8ToolStripMenuItem.Checked = false;

            app.Default.ScreensContain = 7;
            app.Default.Save();
            this.Canvas.screensContain = 7;
            this.Canvas.ScreensContainChanged();
        }

        private void Screens8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Screens1ToolStripMenuItem.Checked = false;
            Screens2ToolStripMenuItem.Checked = false;
            Screens3ToolStripMenuItem.Checked = false;
            Screens4ToolStripMenuItem.Checked = false;
            Screens5ToolStripMenuItem.Checked = false;
            Screens6ToolStripMenuItem.Checked = false;
            Screens7ToolStripMenuItem.Checked = false;
            Screens8ToolStripMenuItem.Checked = true;

            app.Default.ScreensContain = 8;
            app.Default.Save();
            this.Canvas.screensContain = 8;
            this.Canvas.ScreensContainChanged();
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
