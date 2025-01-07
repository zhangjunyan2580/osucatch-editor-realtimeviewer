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
        string newBeatmap = "";
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

        public static void ErrorMessage(string msg)
        {
            MessageBox.Show(msg, "Error");
        }

        public enum LogType
        {
            Default,
            Program,
            EditorReader,
            BeatmapBuilder,
            BeatmapParser,
            Drawing,
            Backup,
            Timer,
        }

        public enum LogLevel { Debug, Info, Warning, Error }

        public static void ConsoleLog(string msg, LogType logType = LogType.Default, LogLevel logLevel = LogLevel.Info)
        {
            if (!app.Default.Show_Console) return;

            if (logType == LogType.Program && !app.Default.Log_Program) return;
            if (logType == LogType.EditorReader && !app.Default.Log_EditorReader) return;
            if (logType == LogType.BeatmapBuilder && !app.Default.Log_BeatmapBuilder) return;
            if (logType == LogType.BeatmapParser && !app.Default.Log_BeatmapParser) return;
            if (logType == LogType.Drawing && !app.Default.Log_Drawing) return;
            if (logType == LogType.Backup && !app.Default.Log_Backup) return;
            if (logType == LogType.Timer && !app.Default.Log_Timer) return;

            if (app.Default.Log_Level > (int)logLevel) return;

            Console.WriteLine("[" + logLevel + "] [" + DateTime.Now.ToString("HH:mm:ss.fff") + "] [" + logType + "] " + msg);
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
                    ConsoleLog("Could not find osu path from registry.", LogType.Program, LogLevel.Warning);
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
            ConsoleLog("DPI: " + dpi, LogType.Program, LogLevel.Info);
            fontscale = 96f / dpi;
            ConsoleLog("Text Scale x" + fontscale.ToString("F2"), LogType.Program, LogLevel.Info);
            this.Canvas.fontScale = fontscale;

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
                ConsoleLog("Total Memory: " + (1.0 * memorySize / 1024 / 1024).ToString("F3") + "MB", LogType.Program, LogLevel.Warning);
                //ConsoleLog(newBeatmap, LogType.BeatmapBuilder, LogLevel.Warning);
            }
            else
            {
                ConsoleLog("Total Memory: " + (1.0 * memorySize / 1024 / 1024).ToString("F3") + "MB", LogType.Program, LogLevel.Debug);
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
                        ConsoleLog("Osu! process needs Refetch.", LogType.EditorReader, LogLevel.Info);
                        if (Is_Doing_SetProcess)
                        {
                            ConsoleLog("Already fetching osu!.", LogType.EditorReader, LogLevel.Info);
                            reader_timer.Interval = Idle_Interval;
                            return;
                        }
                        ConsoleLog("Try to fetch osu! process.", LogType.EditorReader, LogLevel.Info);
                        Is_Doing_SetProcess = true;
                        reader.SetProcess();
                        Is_Doing_SetProcess = false;
                        ConsoleLog("Fetch osu! process successfully.", LogType.EditorReader, LogLevel.Info);
                        Is_Osu_Running = true;
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog("No Osu!.exe found.", LogType.EditorReader, LogLevel.Info);
                        ConsoleLog(ex.ToString(), LogType.EditorReader, LogLevel.Debug);
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
                    ConsoleLog("Task Timeout, cancelled before fetching editor.", LogType.Program, LogLevel.Warning);
                    return;
                }

                string title = reader.ProcessTitle();
                if (title == "")
                {
                    ConsoleLog("Empty osu title.", LogType.EditorReader, LogLevel.Info);
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
                    ConsoleLog("Osu title is not editor: " + title, LogType.EditorReader, LogLevel.Info);
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
                    ConsoleLog("Editor needs Reload.", LogType.EditorReader, LogLevel.Info);
                    try
                    {
                        if (Is_Doing_SetProcess || Is_Doing_FetchEditor)
                        {
                            ConsoleLog("Already reloading editor.", LogType.EditorReader, LogLevel.Info);
                            reader_timer.Interval = Idle_Interval;
                            return;
                        }
                        if (reader.ProcessNeedsReload())
                        {
                            ConsoleLog("Process needs Reload.", LogType.EditorReader, LogLevel.Info);
                            return;
                        }
                        ConsoleLog("Try fetch editor.", LogType.EditorReader, LogLevel.Info);
                        Is_Doing_FetchEditor = true;
                        reader.FetchEditor();
                        Is_Doing_FetchEditor = false;
                        ConsoleLog("Fetch editor successfully.", LogType.EditorReader, LogLevel.Info);
                        Is_Osu_Running = true;
                        Is_Editor_Running = true;
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog("Fetch editor failed.\r\n" + ex, LogType.EditorReader, LogLevel.Error);
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
                    ConsoleLog("Task Timeout, cancelled before FetchAll().", LogType.Program, LogLevel.Warning);
                    return;
                }

                ConsoleLog("Start FetchAll().", LogType.EditorReader, LogLevel.Debug);

                reader.FetchAll();

                if (cancellationToken.IsCancellationRequested)
                {
                    ConsoleLog("Task Timeout, cancelled before checking Editor_Reader's data.", LogType.Program, LogLevel.Warning);
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
                    ConsoleLog("Path is invalid.\r\n" + ex.ToString(), LogType.EditorReader, LogLevel.Error);
                    ConsoleLog("ContainingFolder: " + thisReader.ContainingFolder, LogType.EditorReader, LogLevel.Error);
                    ConsoleLog("Filename: " + thisReader.Filename, LogType.EditorReader, LogLevel.Error);
                    return;
                }

                float readerTime = thisReader.EditorTime;

                if (thisReader.HitObjects == null || thisReader.HitObjects.Count <= 0)
                {
                    ConsoleLog("HitObjects is empty.", LogType.EditorReader, LogLevel.Warning);
                    return;
                }

                ConsoleLog("FetchAll complete.", LogType.EditorReader, LogLevel.Debug);

                // Fix Editor Reader
                // Modified from Mapping_Tools
                // https://github.com/OliBomby/Mapping_Tools/tree/master/Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs
                // Under MIT Licnece https://github.com/OliBomby/Mapping_Tools/blob/master/LICENCE
                if (!(thisReader.NumControlPoints > 0 &&
                thisReader.ControlPoints != null && thisReader.HitObjects != null &&
                thisReader.NumControlPoints == thisReader.ControlPoints.Count && thisReader.NumObjects == thisReader.HitObjects.Count))
                {
                    ConsoleLog("Fetched data is invalid.", LogType.EditorReader, LogLevel.Error);
                    return;
                }

                int removeCount = thisReader.HitObjects.RemoveAll(readerHitObject => readerHitObject.X > 1000 || readerHitObject.X < -1000 || readerHitObject.Y > 1000 || readerHitObject.Y < -1000 ||
                readerHitObject.SegmentCount > 9000 || readerHitObject.Type == 0 || readerHitObject.SampleSet > 1000 ||
                readerHitObject.SampleSetAdditions > 1000 || readerHitObject.SampleVolume > 1000);
                // -----------------------

                if (removeCount > 0) ConsoleLog("Removed " + removeCount + " invalid hitObject(s).", LogType.BeatmapBuilder, LogLevel.Warning);

                if (cancellationToken.IsCancellationRequested)
                {
                    ConsoleLog("Task Timeout, cancelled before building new beatmap.", LogType.Program, LogLevel.Warning);
                    return;
                }

                ConsoleLog("Start build new beatmap.", LogType.BeatmapBuilder, LogLevel.Debug);

                // 新文件
                if (beatmap_path != newpath || newBeatmap == "")
                {
                    beatmap_path = newpath;
                    try
                    {
                        newBeatmap = BuildNewBeatmapFromFilepath(beatmap_path, thisReader);
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog("Build new beatmap from file failed.\r\n" + ex, LogType.BeatmapBuilder, LogLevel.Error);
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
                        ConsoleLog("Build new beatmap from string failed.\r\n" + ex, LogType.BeatmapBuilder, LogLevel.Error);
                        return;
                    }
                }

                ConsoleLog("Build new beatmap successfully.", LogType.BeatmapBuilder, LogLevel.Debug);

                if (cancellationToken.IsCancellationRequested)
                {
                    ConsoleLog("Task Timeout, cancelled before backup or beatmap prasing.", LogType.Program, LogLevel.Warning);
                    return;
                }

                // Backup
                if (Need_Backup)
                {
                    if (Is_Editor_Running && newBeatmap != "")
                    {
                        try
                        {
                            ConsoleLog("Start backup.", LogType.Backup, LogLevel.Info);
                            string backupFilePath = Path.Combine(Backup_Folder, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ") + thisReader.Filename);
                            string? directoryPath = Path.GetDirectoryName(backupFilePath);
                            if (directoryPath == null)
                            {
                                ConsoleLog("Backup failed. Path is invalid: " + backupFilePath, LogType.Backup, LogLevel.Error);
                            }
                            else
                            {
                                Directory.CreateDirectory(directoryPath);
                                File.WriteAllText(backupFilePath, newBeatmap);
                                Need_Backup = false;
                                ConsoleLog("Backup successfully.", LogType.Backup, LogLevel.Info);
                            }
                        }
                        catch (Exception ex)
                        {
                            ConsoleLog("Backup failed.\r\n" + ex.ToString(), LogType.Backup, LogLevel.Error);
                        }
                    }
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    ConsoleLog("Task Timeout, cancelled before beatmap prasing.", LogType.Program, LogLevel.Warning);
                    return;
                }

                // 丢弃旧帧
                if (DateTime.Now.Ticks <= LastDrawingTimeStamp)
                {
                    ConsoleLog("Drop an outdated data.", LogType.Program, LogLevel.Warning);
                    return;
                }

                // 使用官方库分析新谱面
                int mods = 0;
                if (hRToolStripMenuItem.Checked) mods = (1 << 4);
                else if (eZToolStripMenuItem.Checked) mods = (1 << 1);
                ConsoleLog("Try parse beatmap.", LogType.BeatmapParser, LogLevel.Debug);
                if (this.Canvas.viewerManager == null) this.Canvas.viewerManager = new ViewerManager(newBeatmap, mods);
                else this.Canvas.viewerManager.LoadBeatmap(newBeatmap, mods);

                if (cancellationToken.IsCancellationRequested)
                {
                    ConsoleLog("Task Timeout, cancelled before drawing.", LogType.Program, LogLevel.Warning);
                    return;
                }

                ConsoleLog("Parse beatmap successfully.", LogType.BeatmapParser, LogLevel.Debug);
                this.Canvas.viewerManager.currentTime = readerTime;
                if (hideToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.None;
                else if (sameWithEditorToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.SameWithEditor;
                else if (noSliderVelocityMultiplierToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.NoSliderVelocityMultiplier;
                else if (compareWithWalkSpeedToolStripMenuItem.Checked) this.Canvas.viewerManager.DistanceType = DistanceType.CompareWithWalkSpeed;
                else this.Canvas.viewerManager.DistanceType = DistanceType.None;
                ConsoleLog("Start paint.", LogType.Drawing, LogLevel.Debug);

                Invoke(new MethodInvoker(delegate ()
                {
                    this.Canvas.Canvas_Paint(null, null);
                }));

                ConsoleLog("Paint a frame successful.", LogType.Drawing, LogLevel.Debug);
            }
            catch (Exception ex)
            {
                ConsoleLog(ex.ToString(), LogType.Program, LogLevel.Error);
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
            ConsoleLog("Start Timer", LogType.Timer, LogLevel.Debug);
            ConsoleLog("Timer Interval = " + reader_timer.Interval, LogType.Timer, LogLevel.Debug);
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
                    ConsoleLog("Maybe an incorrect line: " + line, LogType.BeatmapParser, LogLevel.Debug);
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
