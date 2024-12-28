using Editor_Reader;
using Microsoft.Win32;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Primitives;
using System.IO;
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
            /*
            reader.FetchAll();
            Console.WriteLine(reader.ContainingFolder);
            Console.WriteLine(reader.Filename);
            Console.WriteLine("oR" + reader.objectRadius);
            Console.WriteLine("sO" + reader.stackOffset);
            Console.WriteLine("HP" + reader.HPDrainRate);
            Console.WriteLine("CS" + reader.CircleSize);
            Console.WriteLine("AR" + reader.ApproachRate);
            Console.WriteLine("OD" + reader.OverallDifficulty);
            Console.WriteLine("SV" + reader.SliderMultiplier);
            Console.WriteLine("TR" + reader.SliderTickRate);
            Console.WriteLine("CT" + reader.ComposeTool());
            Console.WriteLine("GS" + reader.GridSize());
            Console.WriteLine("BD" + reader.BeatDivisor());
            Console.WriteLine("TZ" + reader.TimelineZoom);
            Console.WriteLine("DS" + reader.DistanceSpacing());

            Console.WriteLine("Current Time:");
            Console.WriteLine(reader.EditorTime());

            Console.WriteLine("Timing Points:");
            for (int i = 0; i < reader.numControlPoints; i++)
                Console.WriteLine(reader.controlPoints[i].ToString());

            Console.WriteLine("Bookmarks:");
            for (int i = 0; i < reader.numBookmarks; i++)
                Console.WriteLine(reader.bookmarks[i]);

            Console.WriteLine("Hit Objects (selected):");
            for (int i = 0; i < reader.numObjects; i++)
                if (reader.hitObjects[i].IsSelected)
                    Console.WriteLine(reader.hitObjects[i].ToString());

            while (true)
            {
                Console.WriteLine(reader.SnapPosition());

                reader.FetchSelected();
                Console.WriteLine("Selected Hit Objects:");
                for (int i = 0; i < reader.numSelected; i++)
                    Console.WriteLine(reader.selectedObjects[i].ToString());

                reader.FetchClipboard();
                Console.WriteLine("Copied Hit Objects:");
                for (int i = 0; i < reader.numClipboard; i++)
                    Console.WriteLine(reader.clipboardObjects[i].ToString());

                Console.WriteLine("Hovered Hit Object:");
                if (reader.FetchHovered())
                    Console.WriteLine(reader.hoveredObject.ToString());

                Console.ReadLine();
            }
            */
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

                // 新文件
                if (beatmap_path != newpath || newBeatmap == "")
                {
                    beatmap_path = newpath;
                    newBeatmap = BuildNewBeatmapFromFilepath(beatmap_path);
                }
                else
                {
                    // 检查和上一tick的谱面文件有无区别
                    string _newBeatmap = BuildNewBeatmapFromString(newBeatmap);
                    if (!String.Equals(_newBeatmap, newBeatmap))
                    {
                        newBeatmap = _newBeatmap;
                    }
                }
                // 使用官方库分析新谱面
                int mods = 0;
                if (hRToolStripMenuItem.Checked) mods = (1 << 4);
                else if (eZToolStripMenuItem.Checked) mods = (1 << 1);
                if (this.Canvas.viewerManager == null) this.Canvas.viewerManager = new ViewerManager(newBeatmap, false, mods);
                else this.Canvas.viewerManager.LoadBeatmap(newBeatmap, mods);
                this.Canvas.viewerManager.currentTime = reader.EditorTime();
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
        private string BuildNewBeatmap(StreamReader file)
        {
            string newfile = "";
            string line;
            bool isMultiLine = false;
            while ((line = file.ReadLine()) != null)
            {
                if (isMultiLine)
                {
                    if (Regex.IsMatch(line, @"^\["))
                    {
                        isMultiLine = false;
                    }
                    else continue;
                }

                // 只替换必要的东西
                if (Regex.IsMatch(line, "^PreviewTime:")) newfile += "PreviewTime: " + reader.PreviewTime + "\r\n";
                else if (Regex.IsMatch(line, "^StackLeniency:")) newfile += "StackLeniency: " + reader.StackLeniency + "\r\n";

                // 强制CTB模式
                // if (Regex.IsMatch(line, "^Mode:")) newfile += "Mode: 2" + "\r\n";

                else if (Regex.IsMatch(line, "^HPDrainRate:")) newfile += "HPDrainRate: " + reader.HPDrainRate + "\r\n";
                else if (Regex.IsMatch(line, "^CircleSize:")) newfile += "CircleSize: " + reader.CircleSize + "\r\n";
                else if (Regex.IsMatch(line, "^OverallDifficulty:")) newfile += "OverallDifficulty: " + reader.OverallDifficulty + "\r\n";
                else if (Regex.IsMatch(line, "^ApproachRate:")) newfile += "ApproachRate: " + reader.ApproachRate + "\r\n";

                else if (Regex.IsMatch(line, "^SliderMultiplier:")) newfile += "SliderMultiplier: " + reader.SliderMultiplier + "\r\n";
                else if (Regex.IsMatch(line, "^SliderTickRate:")) newfile += "SliderTickRate: " + reader.SliderTickRate + "\r\n";

                else if (Regex.IsMatch(line, @"^\[TimingPoints\]"))
                {
                    newfile += "[TimingPoints]" + "\r\n";
                    newfile += String.Join("\r\n", reader.controlPoints) + "\r\n";
                    newfile += "\r\n";
                    isMultiLine = true;
                }
                else if (Regex.IsMatch(line, @"^\[HitObjects\]"))
                {
                    newfile += "[HitObjects]" + "\r\n";
                    newfile += String.Join("\r\n", reader.hitObjects) + "\r\n";
                    newfile += "\r\n";
                    isMultiLine = true;
                }
                else newfile += line + "\r\n";
            }
            return newfile;
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
