using Editor_Reader;
using Microsoft.Win32;
using osu.Game.Beatmaps;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;

namespace osucatch_editor_realtimeviewer
{
    public partial class Form1 : Form
    {
        public static string Path_Settings = "settings.txt";

        public EditorReaderHelper editorReaderHelper = new();
        
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

        private static System.Timers.Timer? backup_timer;

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
            // -----------------------reading settings-----------------------
            // show log console
            if (app.Default.Show_Console) Program.ShowConsole();

            // window size
            this.Width = app.Default.Window_Width;
            this.Height = app.Default.Window_Height;
            SizeChanged += Form1_SizeChanged;

            // osu path
            if (app.Default.osu_path == "")
            {
                string osu_path = "";
                osu_path = GetOsuPath();
                if (osu_path == "")
                {
                    osu_path = Select_Osu_Path();
                }
                app.Default.osu_path = osu_path;
                app.Default.Save();
            }

            // contain screens count
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
            // --------------------------------------------------------------

            // ----------------------------get dpi---------------------------
            Graphics graphics = this.CreateGraphics();
            dpi = (Int32)graphics.DpiX;
            Log.ConsoleLog("DPI: " + dpi, Log.LogType.Program, Log.LogLevel.Info);
            fontscale = 96f / dpi;
            Log.ConsoleLog("Text Scale x" + fontscale.ToString("F2"), Log.LogType.Program, Log.LogLevel.Info);
            this.Canvas.fontScale = fontscale;
            graphics.Dispose();
            // --------------------------------------------------------------

            // canvas init
            this.Canvas.Init();

            // reader timer
            reader_timer.Interval = app.Default.Idle_Interval;
            reader_timer.Start();

            // backup timer
            if (app.Default.Backup_Enabled == true)
            {
                backup_timer = new System.Timers.Timer(app.Default.Backup_Interval);
                backup_timer.Elapsed += backup_timer_Tick;
                backup_timer.Start();
            }

            // memory monitor timer
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
                    this.Width = app.Default.Window_Width;
                    this.Height = app.Default.Window_Height;

                }));
                if (app.Default.Backup_Enabled)
                {
                    if (backup_timer == null)
                    {
                        backup_timer = new System.Timers.Timer(app.Default.Backup_Interval);
                        backup_timer.Elapsed += backup_timer_Tick;
                        backup_timer.Start();
                    }
                    else
                    {
                        backup_timer.Interval = app.Default.Backup_Interval;
                    }
                }
                else
                {
                    if (backup_timer != null)
                    {
                        backup_timer.Stop();
                        backup_timer = null;
                    }
                }

                NeedReapplySettings = false;
            }



            try
            {
                // fetch osu! process
                if (!editorReaderHelper.FetchProcess())
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Text = "Osu!.exe is not running";
                    }));
                    reader_timer.Interval = app.Default.Idle_Interval;
                    beatmap_path = "";
                    return;
                }


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after fetching osu! process.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // fetch editor
                if (!editorReaderHelper.FetchEditor())
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Text = "Editor is not running";
                    }));
                    reader_timer.Interval = app.Default.Idle_Interval;
                    beatmap_path = "";
                    return;
                }
                else
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Text = editorReaderHelper.beatmap_title;
                    }));
                }


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after fetching editor.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // fetch all
                Log.ConsoleLog("Start FetchAll().", Log.LogType.EditorReader, Log.LogLevel.Debug);
                var thisReader = editorReaderHelper.FetchAll();
                if (thisReader == null)
                {
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }
                Log.ConsoleLog("FetchAll complete.", Log.LogType.EditorReader, Log.LogLevel.Debug);


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after FetchAll().", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Build osu file Path
                string newpath = "";
                try
                {
                    newpath = Path.Combine(app.Default.osu_path, "Songs", thisReader.ContainingFolder, thisReader.Filename);
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Path is invalid.\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                    Log.ConsoleLog("ContainingFolder: " + thisReader.ContainingFolder, Log.LogType.EditorReader, Log.LogLevel.Error);
                    Log.ConsoleLog("Filename: " + thisReader.Filename, Log.LogType.EditorReader, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Check editor reader's data
                if (thisReader.HitObjects == null || thisReader.HitObjects.Count <= 0)
                {
                    Log.ConsoleLog("HitObjects is empty.", Log.LogType.EditorReader, Log.LogLevel.Warning);
                    return;
                }
                // Fix Editor Reader
                // Modified from Mapping_Tools
                // https://github.com/OliBomby/Mapping_Tools/tree/master/Mapping_Tools/Classes/ToolHelpers/EditorReaderStuff.cs
                // Under MIT Licnece https://github.com/OliBomby/Mapping_Tools/blob/master/LICENCE
                if (!(thisReader.NumControlPoints > 0 &&
                    thisReader.ControlPoints != null && thisReader.HitObjects != null &&
                    thisReader.NumControlPoints == thisReader.ControlPoints.Count && thisReader.NumObjects == thisReader.HitObjects.Count))
                {
                    Log.ConsoleLog("Fetched data is invalid.", Log.LogType.EditorReader, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }
                int removeCount = thisReader.HitObjects.RemoveAll(readerHitObject => readerHitObject.X > 1000 || readerHitObject.X < -1000 || readerHitObject.Y > 1000 || readerHitObject.Y < -1000 ||
                readerHitObject.SegmentCount > 9000 || readerHitObject.Type == 0 || readerHitObject.SampleSet > 1000 ||
                readerHitObject.SampleSetAdditions > 1000 || readerHitObject.SampleVolume > 1000);
                if (removeCount > 0) Log.ConsoleLog("Removed " + removeCount + " invalid hitObject(s).", Log.LogType.BeatmapBuilder, Log.LogLevel.Warning);
                // -----------------------


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after checking editor reader's data.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // build new beatmap
                Log.ConsoleLog("Start build new beatmap.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);
                if (beatmap_path != newpath || lastBeatmap == "")
                {
                    // new beatmap
                    beatmap_path = newpath;
                    try
                    {
                        newBeatmap = BeatmapBuilder.BuildNewBeatmapFromFilepath(beatmap_path, thisReader);
                    }
                    catch (Exception ex)
                    {
                        Log.ConsoleLog("Build new beatmap from file failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                        reader_timer.Interval = app.Default.Idle_Interval;
                        return;
                    }
                }
                else
                {
                    try
                    {
                        newBeatmap = BeatmapBuilder.BuildNewBeatmapFromString(newBeatmap, thisReader);
                    }
                    catch (Exception ex)
                    {
                        Log.ConsoleLog("Build new beatmap from string failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                        reader_timer.Interval = app.Default.Idle_Interval;
                        return;
                    }
                }
                Log.ConsoleLog("Build new beatmap successfully.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after building new beatmap.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // cache beatmap & mods & distanceType
                bool isSameBeatmap = false;
                bool isSameMods = false;
                bool isSameDistanceType = false;
                // isSameBeatmap
                if (string.Compare(newBeatmap, lastBeatmap, StringComparison.Ordinal) == 0)
                {
                    isSameBeatmap = true;
                }
                else
                {
                    lastBeatmap = newBeatmap;
                }
                // isSameMods
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
                // isSameDistanceType
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
                if (Need_Backup)
                {
                    if (editorReaderHelper.Is_Editor_Running && newBeatmap != "")
                    {
                        try
                        {
                            Log.ConsoleLog("Start backup.", Log.LogType.Backup, Log.LogLevel.Info);
                            string backupFilePath = Path.Combine(app.Default.Backup_Folder, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss ") + thisReader.Filename);
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
                            Need_Backup = false;
                        }
                    }
                }


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after backup.", Log.LogType.Program, Log.LogLevel.Error);
                    return;
                }


                // drop outdated data
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


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled before beatmap prasing.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }


                // draw
                try
                {
                    this.Canvas.viewerManager.currentTime = thisReader.EditorTime;
                    Log.ConsoleLog("Start drawing.", Log.LogType.Drawing, Log.LogLevel.Debug);
                    Invoke(new MethodInvoker(delegate ()
                    {
                        this.Canvas.Canvas_Paint(null, null);
                    }));
                    Log.ConsoleLog("Draw a frame successful.", Log.LogType.Drawing, Log.LogLevel.Debug);
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Draw a frame failed.\r\n" + ex, Log.LogType.Drawing, Log.LogLevel.Debug);
                }


                if (DateTime.Now.Ticks > LastDrawingTimeStamp) LastDrawingTimeStamp = DateTime.Now.Ticks;
                reader_timer.Interval = app.Default.Drawing_Interval;
            }
            catch (Exception ex)
            {
                Log.ConsoleLog(ex.ToString(), Log.LogType.Program, Log.LogLevel.Error);
            }

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




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            reader_timer.Stop();
            backup_timer?.Stop();
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
            app.Default.Window_Width = this.Width;
            app.Default.Window_Height = this.Height;
            app.Default.Save();
        }

        private void openSettingsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm();
            sf.ShowDialog();
        }

        private void backup_timer_Tick(object? source, ElapsedEventArgs? e)
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



}
