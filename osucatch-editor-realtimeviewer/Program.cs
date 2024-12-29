using Microsoft.Win32;
using NuGet.Configuration;
using System.Diagnostics;

namespace osucatch_editor_realtimeviewer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            string[] settings = new string[8];
            if (File.Exists(Form1.Path_Settings))
            {
                settings = Program.LoadSettings();
            }
            else
            {
                string osupath = GetOsuPath();
                string[] settingsFile = new string[]
                {
                    @"# Lines starting with a # are ignored",
                    @"# Do not change the order of the settings",
                    @"",
                    @"# window width",
                    @"250",
                    @"",
                    @"# window height",
                    @"750",
                    @"",
                    @"# osu path",
                    osupath,
                    @"",
                    @"# backup enabled",
                    @"0",
                    @"",
                    @"# backup folder",
                    osupath + @"EditorBackups\",
                    @"",
                    @"# backup interval",
                    @"60000",
                    @"",
                    @"# idle interval",
                    @"1000",
                    @"",
                    @"# drawing interval",
                    @"20",
                    @"",
                };
                File.WriteAllLines(Form1.Path_Settings, settingsFile);
                settings = new string[8];
                settings[0] = @"250";
                settings[1] = @"750";
                settings[2] = osupath;
                settings[3] = @"0";
                settings[4] = osupath + @"EditorBackups\";
                settings[5] = @"60000";
                settings[6] = @"1000";
                settings[7] = @"20";
            }

            Application.Run(new Form1(settings));
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

        public static string[] LoadSettings()
        {
            string[] settings = new string[8];
            string[] lines = File.ReadAllLines(Form1.Path_Settings);
            int n = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (n >= settings.Length)
                {
                    break;
                }
                if (lines[i].Length > 0 && lines[i][0] != '#')
                {
                    settings[n] = lines[i];
                    n++;
                }
            }
            return settings;
        }

        public static void OpenSettings()
        {
            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo(Form1.Path_Settings) { UseShellExecute = true });
            }
            catch (Exception e)
            {
                Form1.ErrorMessage(e.Message);
            }
        }
    }
}