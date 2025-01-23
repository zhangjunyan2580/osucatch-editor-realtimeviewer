using Microsoft.Win32;
using osu.Game.Beatmaps;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;

namespace osucatch_editor_realtimeviewer
{
    public partial class Form1 : Form
    {
        public static string Path_Settings = "settings.txt";

        public EditorReaderHelper editorReaderHelper = new();

        public static DrawingHelper drawingHelper = new();

        BeatmapInfoCollection? lastReader = null;
        List<string>? lastColourLines = null;
        Beatmap? lastBeatmap = null;
        IBeatmap? lastConvertedBeatmap = null;
        int lastMods = -1;
        HitObjectLabelType lastLabelType = HitObjectLabelType.None;

        bool Need_Backup = false;
        Int64 LastDrawingTimeStamp = DateTime.Now.Ticks;
        int dpi = 96;
        float fontscale = 1;

        public static string Path_Img_Hitcircle = @"img/fruit-apple.png";
        public static string Path_Img_Drop = @"img/fruit-drop.png";
        public static string Path_Img_Banana = @"img/fruit-bananas.png";

        public static bool NeedReapplySettings = false;

        private static System.Timers.Timer backup_timer = new System.Timers.Timer(app.Default.Backup_Interval);
        private static System.Timers.Timer reader_timer = new System.Timers.Timer(app.Default.Idle_Interval);
        private static System.Timers.Timer Memory_Monitor_Timer = new System.Timers.Timer(200);

        public Form1()
        {
            InitializeComponent();

            if (app.Default.Language_String != "")
            {
                defaultLanguageToolStripMenuItem.Checked = false;

                englishLanguageToolStripMenuItem.Checked = (app.Default.Language_String == "en-US");
                zhHansLanguageToolStripMenuItem.Checked = (app.Default.Language_String == "zh-Hans");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(app.Default.Language_String);
                Form1.ApplyResources(this);
            }
            else
            {
                defaultLanguageToolStripMenuItem.Checked = true;
                englishLanguageToolStripMenuItem.Checked = false;
                zhHansLanguageToolStripMenuItem.Checked = false;
            }

            if (app.Default.Window_X >= 0 && app.Default.Window_Y >= 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(app.Default.Window_X, app.Default.Window_Y);
            }
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

            if (app.Default.Window_Maximized) this.WindowState = FormWindowState.Maximized;

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
            drawingHelper.ScreensContain = app.Default.ScreensContain;
            Canvas.screensContain = app.Default.ScreensContain;
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
            Canvas.fontScale = fontscale;
            graphics.Dispose();
            // --------------------------------------------------------------

            // canvas init
            this.Canvas.Init();

            // reader timer
            reader_timer = new System.Timers.Timer(app.Default.Idle_Interval);
            reader_timer.AutoReset = false;
            reader_timer.Elapsed += reader_timer_Tick;
            reader_timer.Start();

            // backup timer
            if (app.Default.Backup_Enabled == true)
            {
                backup_timer = new System.Timers.Timer(app.Default.Backup_Interval);
                backup_timer.Elapsed += backup_timer_Tick;
                backup_timer.Start();
            }

            // memory monitor timer
            Memory_Monitor_Timer.Elapsed += Memory_Monitor;
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

        private void ReapplySettings()
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
                }
            }
            if (app.Default.FilterNearbyHitObjects)
            {
                backupToolStripMenuItem.Enabled = false;
            }
            else
            {
                backupToolStripMenuItem.Enabled = true;
            }
        }

        private bool FetchOsuProcess()
        {

            if (!editorReaderHelper.FetchProcess())
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    this.Text = "Osu!.exe is not running";
                }));
                reader_timer.Interval = app.Default.Idle_Interval;
                lastReader = null;
                return false;
            }
            return true;
        }

        private bool FetchEditor()
        {
            if (!editorReaderHelper.FetchEditor())
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    this.Text = "Editor is not running";
                }));
                reader_timer.Interval = app.Default.Idle_Interval;
                lastReader = null;
                return false;
            }
            else
            {
                return true;
            }
        }

        private Beatmap? BuildNewBeatmap(BeatmapInfoCollection thisReader, DifferenceType differenceType, string filepath)
        {
            Beatmap? beatmap = null;
            if (differenceType == DifferenceType.DifferentFile)
            {
                // fetch colors because editor reader doesn't fetch it.
                try
                {
                    beatmap = BeatmapBuilder.BuildNewBeatmapWithFilePath(thisReader, filepath, out lastColourLines);
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Build new beatmap from beatmap file failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return null;
                }
                lastBeatmap = beatmap;
            }
            else if (differenceType == DifferenceType.DifferentObjects)
            {
                try
                {
                    beatmap = BeatmapBuilder.BuildNewBeatmapWithColorString(thisReader, lastColourLines);
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Build new beatmap from reader failed.\r\n" + ex, Log.LogType.BeatmapBuilder, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return null;
                }
                lastBeatmap = beatmap;
            }
            else if (differenceType == DifferenceType.None)
            {
                beatmap = lastBeatmap;
            }
            return beatmap;
        }

        private int GetMods(out bool isSameMods)
        {
            int mods = 0;
            if (hRToolStripMenuItem.Checked) mods = (1 << 4);
            else if (eZToolStripMenuItem.Checked) mods = (1 << 1);
            if (mods == lastMods)
            {
                isSameMods = true;
            }
            else
            {
                isSameMods = false;
                lastMods = mods;
            }
            return mods;
        }

        private HitObjectLabelType GetHitObjectLabelType(out bool isSameLabelType)
        {
            HitObjectLabelType labelType = HitObjectLabelType.None;
            if (hideToolStripMenuItem.Checked) labelType = HitObjectLabelType.None;
            else if (sameWithEditorToolStripMenuItem.Checked) labelType = HitObjectLabelType.Distance_SameWithEditor;
            else if (noSliderVelocityMultiplierToolStripMenuItem.Checked) labelType = HitObjectLabelType.Distance_NoSliderVelocityMultiplier;
            else if (compareWithWalkSpeedToolStripMenuItem.Checked) labelType = HitObjectLabelType.Distance_CompareWithWalkSpeed;
            else if (difficultyStarsToolStripMenuItem.Checked) labelType = HitObjectLabelType.Difficulty_Stars;
            else labelType = HitObjectLabelType.None;
            if (labelType == lastLabelType)
            {
                isSameLabelType = true;
            }
            else
            {
                isSameLabelType = false;
                lastLabelType = labelType;
            }
            return labelType;
        }

        private void BackupBeatmap(BeatmapInfoCollection thisReader, string filepath)
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
                    Log.ConsoleLog("Create new beatmap.", Log.LogType.Backup, Log.LogLevel.Info);
                    string newBeatmap = BeatmapBuilder.BuildNewBeatmapFileFromFilepath(filepath, thisReader);
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

        private void reader_timer_Work(CancellationToken cancellationToken)
        {
            // Step0. check settings change
            if (NeedReapplySettings)
            {
                ReapplySettings();
                NeedReapplySettings = false;
            }

            try
            {
                // Step1. fetch osu! process
                if (!FetchOsuProcess()) return;


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after fetching osu! process.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Step2. fetch editor
                if (!FetchEditor()) return;


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after fetching editor.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Step3. fetch all
                BeatmapInfoCollection? thisReader;
                bool blockBackup = false;
                if (app.Default.FilterNearbyHitObjects)
                {
                    blockBackup = true;

                    double partialLoadingHalfTimeSpan = 10 * 1000;
                    if (eZToolStripMenuItem.Checked) partialLoadingHalfTimeSpan *= 1.5;
                    if (hRToolStripMenuItem.Checked) partialLoadingHalfTimeSpan /= 1.5;
                    thisReader = editorReaderHelper.FetchAll(partialLoadingHalfTimeSpan);
                }
                else thisReader = editorReaderHelper.FetchAll();
                if (thisReader == null)
                {
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }
                // save last reader
                DifferenceType differenceType = thisReader.CheckDifference(lastReader, app.Default.Selected_Show);
                lastReader = thisReader;


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after FetchAll().", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Step4. Build osu file Path
                string filepath = "";
                try
                {
                    filepath = Path.Combine(app.Default.osu_path, "Songs", thisReader.ContainingFolder, thisReader.Filename);
                }
                catch (Exception ex)
                {
                    Log.ConsoleLog("Path is invalid.\r\n" + ex.ToString(), Log.LogType.EditorReader, Log.LogLevel.Error);
                    Log.ConsoleLog("ContainingFolder: " + thisReader.ContainingFolder, Log.LogType.EditorReader, Log.LogLevel.Error);
                    Log.ConsoleLog("Filename: " + thisReader.Filename, Log.LogType.EditorReader, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after checking editor reader's data.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Step5. build new beatmap
                Log.ConsoleLog("Start build new beatmap.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);
                Beatmap? beatmap = BuildNewBeatmap(thisReader, differenceType, filepath);
                if (beatmap == null)
                {
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }
                Log.ConsoleLog("Build new beatmap successfully.", Log.LogType.BeatmapBuilder, Log.LogLevel.Debug);


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after building new beatmap.", Log.LogType.Program, Log.LogLevel.Error);
                    reader_timer.Interval = app.Default.Idle_Interval;
                    return;
                }


                // Step6. cache mods & distanceType
                bool isSameMods = false;
                bool isSameLabelType = false;
                // isSameMods
                int mods = GetMods(out isSameMods);
                // isSameDistanceType
                drawingHelper.LabelType = GetHitObjectLabelType(out isSameLabelType);


                // Step7. Backup
                if (!blockBackup & Need_Backup)
                {
                    if (editorReaderHelper.Is_Editor_Running && beatmap != null)
                    {
                        BackupBeatmap(thisReader, filepath);
                    }
                }


                if (cancellationToken.IsCancellationRequested)
                {
                    Log.ConsoleLog("Task Timeout, cancelled after backup.", Log.LogType.Program, Log.LogLevel.Error);
                    return;
                }


                // Step8. drop outdated data (really need it?)
                if (DateTime.Now.Ticks <= LastDrawingTimeStamp)
                {
                    Log.ConsoleLog("Drop an outdated data.", Log.LogType.Program, Log.LogLevel.Warning);
                    return;
                }


                // Step9. convert beatmap to catch
                IBeatmap? convertedBeatmap = null;
                if (differenceType != DifferenceType.None || lastConvertedBeatmap == null || !isSameMods)
                {
                    convertedBeatmap = BeatmapConverter.GetConvertedBeatmap(beatmap, mods);
                    lastConvertedBeatmap = convertedBeatmap;

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Log.ConsoleLog("Task Timeout, cancelled after converting beatmap.", Log.LogType.Program, Log.LogLevel.Warning);
                        return;
                    }
                }
                else
                {
                    convertedBeatmap = lastConvertedBeatmap;
                }


                // Step10. prepare drawing objects
                if (drawingHelper.CatchHitObjects != null && drawingHelper.CatchHitObjects.Count > 0 && differenceType == DifferenceType.None && isSameMods && isSameLabelType)
                {
                    Log.ConsoleLog("Beatmap no changes. Using last data.", Log.LogType.BeatmapConverter, Log.LogLevel.Debug);
                }
                else
                {
                    Log.ConsoleLog("Try building drawing objects.", Log.LogType.BeatmapConverter, Log.LogLevel.Debug);
                    drawingHelper.LoadBeatmap(convertedBeatmap);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Log.ConsoleLog("Task Timeout, cancelled after preparing drawing.", Log.LogType.Program, Log.LogLevel.Warning);
                        return;
                    }

                    Log.ConsoleLog("Build drawing objects successfully.", Log.LogType.BeatmapConverter, Log.LogLevel.Debug);
                }

                // change form's title
                Invoke(new MethodInvoker(delegate ()
                {
                    if (drawingHelper.LabelType == HitObjectLabelType.Difficulty_Stars)
                        this.Text = "Stars: " + convertedBeatmap.BeatmapInfo.StarRating.ToString("0.00") + "*";
                    else this.Text = editorReaderHelper.beatmap_title;
                }));


                // Step11. drawing
                try
                {
                    drawingHelper.CurrentTime = thisReader.EditorTime;
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

        private async void reader_timer_Tick(object? sender, EventArgs? e)
        {
            reader_timer.Stop();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(app.Default.WorkCancelAfter);
            var task = Task.Run(() => reader_timer_Work(cancellationTokenSource.Token), cancellationTokenSource.Token);

            await Task.WhenAll(task);

            Log.ConsoleLog("Start Timer", Log.LogType.Timer, Log.LogLevel.Debug);
            Log.ConsoleLog("Timer Interval = " + reader_timer.Interval, Log.LogType.Timer, Log.LogLevel.Debug);
            reader_timer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            reader_timer.Stop();
            backup_timer.Stop();
            Memory_Monitor_Timer.Stop();

            app.Default.Window_X = this.Location.X;
            app.Default.Window_Y = this.Location.Y;
            app.Default.Window_Maximized = (this.WindowState == FormWindowState.Maximized);
            app.Default.Save();
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
            difficultyStarsToolStripMenuItem.Checked = false;
        }

        private void sameWithEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = true;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = false;
            compareWithWalkSpeedToolStripMenuItem.Checked = false;
            difficultyStarsToolStripMenuItem.Checked = false;
        }

        private void noSliderVelocityMultiplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = false;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = true;
            compareWithWalkSpeedToolStripMenuItem.Checked = false;
            difficultyStarsToolStripMenuItem.Checked = false;
        }

        private void compareWithWalkSpeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = false;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = false;
            compareWithWalkSpeedToolStripMenuItem.Checked = true;
            difficultyStarsToolStripMenuItem.Checked = false;
        }

        private void difficultyStarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hideToolStripMenuItem.Checked = false;
            sameWithEditorToolStripMenuItem.Checked = false;
            noSliderVelocityMultiplierToolStripMenuItem.Checked = false;
            compareWithWalkSpeedToolStripMenuItem.Checked = false;
            difficultyStarsToolStripMenuItem.Checked = true;
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
            Canvas.screensContain = 1;
            drawingHelper.ScreensContain = 1;
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
            Canvas.screensContain = 2;
            drawingHelper.ScreensContain = 2;
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
            Canvas.screensContain = 3;
            drawingHelper.ScreensContain = 3;
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
            Canvas.screensContain = 4;
            drawingHelper.ScreensContain = 4;
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
            Canvas.screensContain = 5;
            drawingHelper.ScreensContain = 5;
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
            Canvas.screensContain = 6;
            drawingHelper.ScreensContain = 6;
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
            Canvas.screensContain = 7;
            drawingHelper.ScreensContain = 7;
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
            Canvas.screensContain = 8;
            drawingHelper.ScreensContain = 8;
            this.Canvas.ScreensContainChanged();
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Need_Backup = true;
        }

        public static void ApplyResources(Form form)
        {
            ComponentResourceManager rm = new System.ComponentModel.ComponentResourceManager(form.GetType());
            rm.ApplyResources(form, "$this");
            AppLang(form, rm);
        }

        private static void AppLang(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (var c in tsmi.DropDownItems)
                    {
                        if (c is ToolStripMenuItem) AppLang((ToolStripMenuItem)c, resources);
                    }
                }
            }
        }

        private static void AppLang(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        AppLang(c, resources);
                    }
                }
            }

            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                AppLang(c, resources);
            }
        }

        private void englishLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            defaultLanguageToolStripMenuItem.Checked = false;
            englishLanguageToolStripMenuItem.Checked = true;
            zhHansLanguageToolStripMenuItem.Checked = false;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Form1.ApplyResources(this);

            app.Default.Language_String = "en-US";
            app.Default.Save();
        }

        private void zhHansLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            defaultLanguageToolStripMenuItem.Checked = false;
            englishLanguageToolStripMenuItem.Checked = false;
            zhHansLanguageToolStripMenuItem.Checked = true;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-Hans");
            Form1.ApplyResources(this);

            app.Default.Language_String = "zh-Hans";
            app.Default.Save();
        }

        private void defaultLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            defaultLanguageToolStripMenuItem.Checked = true;
            englishLanguageToolStripMenuItem.Checked = false;
            zhHansLanguageToolStripMenuItem.Checked = false;
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CurrentCulture;
            Form1.ApplyResources(this);

            app.Default.Language_String = "";
            app.Default.Save();
        }
    }



}
