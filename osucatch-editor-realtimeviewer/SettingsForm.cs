using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osucatch_editor_realtimeviewer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            numericUpDown_width.Value = Form1.Window_Width;
            numericUpDown_height.Value = Form1.Window_Height;
            numericUpDown_idleInterval.Value = Form1.Idle_Interval;
            numericUpDown_drawingInterval.Value = Form1.Drawing_Interval;
            numericUpDown_backupInterval.Value = Form1.Backup_Interval / 1000;
            textBox_osuFolder.Text = Form1.osu_path;
            textBox_backupFolder.Text = Form1.Backup_Folder;
            if (textBox_backupFolder.Text == "" && textBox_osuFolder.Text != "")
            {
                textBox_backupFolder.Text = Path.Combine(textBox_osuFolder.Text, "EditorBackups");
            }
            checkBox_enableBackup.Checked = Form1.Backup_Enabled;
        }

        private void button_width_reset_Click(object sender, EventArgs e)
        {
            numericUpDown_width.Value = 250;
        }

        private void button_height_reset_Click(object sender, EventArgs e)
        {
            numericUpDown_height.Value = 750;
        }

        private void button_idleInterval_reset_Click(object sender, EventArgs e)
        {
            numericUpDown_idleInterval.Value = 1000;
        }

        private void button_drawingInterval_reset_Click(object sender, EventArgs e)
        {
            numericUpDown_drawingInterval.Value = 20;
        }

        private void button_osuFolder_reset_Click(object sender, EventArgs e)
        {
            textBox_osuFolder.Text = Form1.GetOsuPath();
        }

        private void button_backupFolder_reset_Click(object sender, EventArgs e)
        {
            textBox_backupFolder.Text = (textBox_osuFolder.Text == "") ? "" : Path.Combine(textBox_osuFolder.Text, "EditorBackups");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            numericUpDown_backupInterval.Value = 60;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Path.Combine(textBox_osuFolder.Text, "osu!.exe")))
            {
                MessageBox.Show("No osu!.exe in the osu folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Form1.Window_Width = (int)numericUpDown_width.Value;
            app.Default.Window_Width = Form1.Window_Width;
            Form1.Window_Height = (int)numericUpDown_height.Value;
            app.Default.Window_Height = Form1.Window_Height;
            Form1.Idle_Interval = (int)numericUpDown_idleInterval.Value;
            app.Default.Idle_Interval = Form1.Idle_Interval;
            Form1.Drawing_Interval = (int)numericUpDown_drawingInterval.Value;
            app.Default.Drawing_Interval = Form1.Drawing_Interval;
            Form1.Backup_Interval = (int)numericUpDown_backupInterval.Value * 1000;
            app.Default.Backup_Interval = Form1.Backup_Interval;
            Form1.osu_path = textBox_osuFolder.Text;
            app.Default.osu_path = Form1.osu_path;
            Form1.Backup_Folder = textBox_backupFolder.Text;
            app.Default.Backup_Folder = Form1.Backup_Folder;
            Form1.Backup_Enabled = checkBox_enableBackup.Checked;
            app.Default.Backup_Enabled = Form1.Backup_Enabled;

            Form1.NeedReapplySettings = true;
            this.Close();
        }

        private void button_osuFolder_select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = false;
            folder.RootFolder = Environment.SpecialFolder.MyComputer;
            folder.Description = "Select osu! Folder";
            DialogResult path = folder.ShowDialog();
            if (path == DialogResult.OK)
            {
                //check if osu!.exe is present
                if (!File.Exists(Path.Combine(folder.SelectedPath, "osu!.exe")))
                {
                    MessageBox.Show("No osu!.exe in this folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else textBox_osuFolder.Text = folder.SelectedPath;
            }
        }

        private void button_backupFolder_select_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.ShowNewFolderButton = false;
            folder.RootFolder = Environment.SpecialFolder.MyComputer;
            folder.Description = "Select .osu Files Backup Folder";
            DialogResult path = folder.ShowDialog();
            if (path == DialogResult.OK)
            {
                textBox_backupFolder.Text = folder.SelectedPath;
            }
        }
    }
}
