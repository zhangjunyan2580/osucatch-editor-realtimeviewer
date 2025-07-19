namespace osucatch_editor_realtimeviewer
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            groupBox1 = new GroupBox();
            button_height_reset = new Button();
            button_width_reset = new Button();
            numericUpDown_height = new NumericUpDown();
            numericUpDown_width = new NumericUpDown();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            button_osuFolder_reset = new Button();
            button_osuFolder_select = new Button();
            textBox_osuFolder = new TextBox();
            label3 = new Label();
            groupBox3 = new GroupBox();
            button_drawingInterval_reset = new Button();
            numericUpDown_idleInterval = new NumericUpDown();
            button_idleInterval_reset = new Button();
            label5 = new Label();
            numericUpDown_drawingInterval = new NumericUpDown();
            label4 = new Label();
            groupBox4 = new GroupBox();
            checkBox_enableBackup = new CheckBox();
            numericUpDown_backupInterval = new NumericUpDown();
            button_backupFolder_reset = new Button();
            button_backupInterval_reset = new Button();
            textBox_backupFolder = new TextBox();
            label7 = new Label();
            label6 = new Label();
            button_backupFolder_select = new Button();
            button_apply = new Button();
            button_cancel = new Button();
            groupBox5 = new GroupBox();
            label12 = new Label();
            button_Label_Color = new Button();
            checkBox_ShowSelected = new CheckBox();
            checkBox_withColor = new CheckBox();
            groupBox6 = new GroupBox();
            checkBox_Log_BeatmapBuilder = new CheckBox();
            comboBox_Log_Level = new ComboBox();
            label8 = new Label();
            checkBox_Log_Timer = new CheckBox();
            checkBox_Log_Backup = new CheckBox();
            checkBox_Log_Drawing = new CheckBox();
            checkBox_Log_BeatmapConverter = new CheckBox();
            checkBox_Log_EditorReader = new CheckBox();
            checkBox_Log_Program = new CheckBox();
            checkBox_showConsole = new CheckBox();
            groupBox7 = new GroupBox();
            checkBox_BarLine_Show = new CheckBox();
            checkBox_TimingLine_ShowGreen = new CheckBox();
            checkBox_TimingLine_ShowRed = new CheckBox();
            groupBox8 = new GroupBox();
            button_timeOut_reset = new Button();
            label11 = new Label();
            label10 = new Label();
            numericUpDown_timeOut = new NumericUpDown();
            label9 = new Label();
            checkBox_FilterNearbyHitObjects = new CheckBox();
            checkBox_Log_BookmarkPlus = new CheckBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_width).BeginInit();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_idleInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_drawingInterval).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backupInterval).BeginInit();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_timeOut).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button_height_reset);
            groupBox1.Controls.Add(button_width_reset);
            groupBox1.Controls.Add(numericUpDown_height);
            groupBox1.Controls.Add(numericUpDown_width);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // button_height_reset
            // 
            resources.ApplyResources(button_height_reset, "button_height_reset");
            button_height_reset.Name = "button_height_reset";
            button_height_reset.UseVisualStyleBackColor = true;
            button_height_reset.Click += button_height_reset_Click;
            // 
            // button_width_reset
            // 
            resources.ApplyResources(button_width_reset, "button_width_reset");
            button_width_reset.Name = "button_width_reset";
            button_width_reset.UseVisualStyleBackColor = true;
            button_width_reset.Click += button_width_reset_Click;
            // 
            // numericUpDown_height
            // 
            resources.ApplyResources(numericUpDown_height, "numericUpDown_height");
            numericUpDown_height.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown_height.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_height.Name = "numericUpDown_height";
            numericUpDown_height.Value = new decimal(new int[] { 750, 0, 0, 0 });
            // 
            // numericUpDown_width
            // 
            resources.ApplyResources(numericUpDown_width, "numericUpDown_width");
            numericUpDown_width.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDown_width.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_width.Name = "numericUpDown_width";
            numericUpDown_width.Value = new decimal(new int[] { 250, 0, 0, 0 });
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button_osuFolder_reset);
            groupBox2.Controls.Add(button_osuFolder_select);
            groupBox2.Controls.Add(textBox_osuFolder);
            groupBox2.Controls.Add(label3);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // button_osuFolder_reset
            // 
            resources.ApplyResources(button_osuFolder_reset, "button_osuFolder_reset");
            button_osuFolder_reset.Name = "button_osuFolder_reset";
            button_osuFolder_reset.UseVisualStyleBackColor = true;
            button_osuFolder_reset.Click += button_osuFolder_reset_Click;
            // 
            // button_osuFolder_select
            // 
            resources.ApplyResources(button_osuFolder_select, "button_osuFolder_select");
            button_osuFolder_select.Name = "button_osuFolder_select";
            button_osuFolder_select.UseVisualStyleBackColor = true;
            button_osuFolder_select.Click += button_osuFolder_select_Click;
            // 
            // textBox_osuFolder
            // 
            resources.ApplyResources(textBox_osuFolder, "textBox_osuFolder");
            textBox_osuFolder.Name = "textBox_osuFolder";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button_drawingInterval_reset);
            groupBox3.Controls.Add(numericUpDown_idleInterval);
            groupBox3.Controls.Add(button_idleInterval_reset);
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(numericUpDown_drawingInterval);
            groupBox3.Controls.Add(label4);
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // button_drawingInterval_reset
            // 
            resources.ApplyResources(button_drawingInterval_reset, "button_drawingInterval_reset");
            button_drawingInterval_reset.Name = "button_drawingInterval_reset";
            button_drawingInterval_reset.UseVisualStyleBackColor = true;
            button_drawingInterval_reset.Click += button_drawingInterval_reset_Click;
            // 
            // numericUpDown_idleInterval
            // 
            resources.ApplyResources(numericUpDown_idleInterval, "numericUpDown_idleInterval");
            numericUpDown_idleInterval.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown_idleInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_idleInterval.Name = "numericUpDown_idleInterval";
            numericUpDown_idleInterval.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // button_idleInterval_reset
            // 
            resources.ApplyResources(button_idleInterval_reset, "button_idleInterval_reset");
            button_idleInterval_reset.Name = "button_idleInterval_reset";
            button_idleInterval_reset.UseVisualStyleBackColor = true;
            button_idleInterval_reset.Click += button_idleInterval_reset_Click;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // numericUpDown_drawingInterval
            // 
            resources.ApplyResources(numericUpDown_drawingInterval, "numericUpDown_drawingInterval");
            numericUpDown_drawingInterval.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown_drawingInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_drawingInterval.Name = "numericUpDown_drawingInterval";
            numericUpDown_drawingInterval.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(checkBox_enableBackup);
            groupBox4.Controls.Add(numericUpDown_backupInterval);
            groupBox4.Controls.Add(button_backupFolder_reset);
            groupBox4.Controls.Add(button_backupInterval_reset);
            groupBox4.Controls.Add(textBox_backupFolder);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(button_backupFolder_select);
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // checkBox_enableBackup
            // 
            resources.ApplyResources(checkBox_enableBackup, "checkBox_enableBackup");
            checkBox_enableBackup.Name = "checkBox_enableBackup";
            checkBox_enableBackup.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_backupInterval
            // 
            resources.ApplyResources(numericUpDown_backupInterval, "numericUpDown_backupInterval");
            numericUpDown_backupInterval.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown_backupInterval.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_backupInterval.Name = "numericUpDown_backupInterval";
            numericUpDown_backupInterval.Value = new decimal(new int[] { 60, 0, 0, 0 });
            // 
            // button_backupFolder_reset
            // 
            resources.ApplyResources(button_backupFolder_reset, "button_backupFolder_reset");
            button_backupFolder_reset.Name = "button_backupFolder_reset";
            button_backupFolder_reset.UseVisualStyleBackColor = true;
            button_backupFolder_reset.Click += button_backupFolder_reset_Click;
            // 
            // button_backupInterval_reset
            // 
            resources.ApplyResources(button_backupInterval_reset, "button_backupInterval_reset");
            button_backupInterval_reset.Name = "button_backupInterval_reset";
            button_backupInterval_reset.UseVisualStyleBackColor = true;
            button_backupInterval_reset.Click += button_backupInterval_reset_Click;
            // 
            // textBox_backupFolder
            // 
            resources.ApplyResources(textBox_backupFolder, "textBox_backupFolder");
            textBox_backupFolder.Name = "textBox_backupFolder";
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // button_backupFolder_select
            // 
            resources.ApplyResources(button_backupFolder_select, "button_backupFolder_select");
            button_backupFolder_select.Name = "button_backupFolder_select";
            button_backupFolder_select.UseVisualStyleBackColor = true;
            button_backupFolder_select.Click += button_backupFolder_select_Click;
            // 
            // button_apply
            // 
            resources.ApplyResources(button_apply, "button_apply");
            button_apply.Name = "button_apply";
            button_apply.UseVisualStyleBackColor = true;
            button_apply.Click += button_apply_Click;
            // 
            // button_cancel
            // 
            resources.ApplyResources(button_cancel, "button_cancel");
            button_cancel.Name = "button_cancel";
            button_cancel.UseVisualStyleBackColor = true;
            button_cancel.Click += button_cancel_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(label12);
            groupBox5.Controls.Add(button_Label_Color);
            groupBox5.Controls.Add(checkBox_ShowSelected);
            groupBox5.Controls.Add(checkBox_withColor);
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // label12
            // 
            resources.ApplyResources(label12, "label12");
            label12.Name = "label12";
            // 
            // button_Label_Color
            // 
            button_Label_Color.BackColor = Color.LightBlue;
            resources.ApplyResources(button_Label_Color, "button_Label_Color");
            button_Label_Color.Name = "button_Label_Color";
            button_Label_Color.UseVisualStyleBackColor = false;
            button_Label_Color.Click += button_Label_Color_Click;
            // 
            // checkBox_ShowSelected
            // 
            resources.ApplyResources(checkBox_ShowSelected, "checkBox_ShowSelected");
            checkBox_ShowSelected.Name = "checkBox_ShowSelected";
            checkBox_ShowSelected.UseVisualStyleBackColor = true;
            // 
            // checkBox_withColor
            // 
            resources.ApplyResources(checkBox_withColor, "checkBox_withColor");
            checkBox_withColor.Name = "checkBox_withColor";
            checkBox_withColor.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(checkBox_Log_BookmarkPlus);
            groupBox6.Controls.Add(checkBox_Log_BeatmapBuilder);
            groupBox6.Controls.Add(comboBox_Log_Level);
            groupBox6.Controls.Add(label8);
            groupBox6.Controls.Add(checkBox_Log_Timer);
            groupBox6.Controls.Add(checkBox_Log_Backup);
            groupBox6.Controls.Add(checkBox_Log_Drawing);
            groupBox6.Controls.Add(checkBox_Log_BeatmapConverter);
            groupBox6.Controls.Add(checkBox_Log_EditorReader);
            groupBox6.Controls.Add(checkBox_Log_Program);
            groupBox6.Controls.Add(checkBox_showConsole);
            resources.ApplyResources(groupBox6, "groupBox6");
            groupBox6.Name = "groupBox6";
            groupBox6.TabStop = false;
            // 
            // checkBox_Log_BeatmapBuilder
            // 
            resources.ApplyResources(checkBox_Log_BeatmapBuilder, "checkBox_Log_BeatmapBuilder");
            checkBox_Log_BeatmapBuilder.Name = "checkBox_Log_BeatmapBuilder";
            checkBox_Log_BeatmapBuilder.UseVisualStyleBackColor = true;
            // 
            // comboBox_Log_Level
            // 
            comboBox_Log_Level.FormattingEnabled = true;
            comboBox_Log_Level.Items.AddRange(new object[] { resources.GetString("comboBox_Log_Level.Items"), resources.GetString("comboBox_Log_Level.Items1"), resources.GetString("comboBox_Log_Level.Items2"), resources.GetString("comboBox_Log_Level.Items3") });
            resources.ApplyResources(comboBox_Log_Level, "comboBox_Log_Level");
            comboBox_Log_Level.Name = "comboBox_Log_Level";
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // checkBox_Log_Timer
            // 
            resources.ApplyResources(checkBox_Log_Timer, "checkBox_Log_Timer");
            checkBox_Log_Timer.Name = "checkBox_Log_Timer";
            checkBox_Log_Timer.UseVisualStyleBackColor = true;
            // 
            // checkBox_Log_Backup
            // 
            resources.ApplyResources(checkBox_Log_Backup, "checkBox_Log_Backup");
            checkBox_Log_Backup.Name = "checkBox_Log_Backup";
            checkBox_Log_Backup.UseVisualStyleBackColor = true;
            // 
            // checkBox_Log_Drawing
            // 
            resources.ApplyResources(checkBox_Log_Drawing, "checkBox_Log_Drawing");
            checkBox_Log_Drawing.Name = "checkBox_Log_Drawing";
            checkBox_Log_Drawing.UseVisualStyleBackColor = true;
            // 
            // checkBox_Log_BeatmapConverter
            // 
            resources.ApplyResources(checkBox_Log_BeatmapConverter, "checkBox_Log_BeatmapConverter");
            checkBox_Log_BeatmapConverter.Name = "checkBox_Log_BeatmapConverter";
            checkBox_Log_BeatmapConverter.UseVisualStyleBackColor = true;
            // 
            // checkBox_Log_EditorReader
            // 
            resources.ApplyResources(checkBox_Log_EditorReader, "checkBox_Log_EditorReader");
            checkBox_Log_EditorReader.Name = "checkBox_Log_EditorReader";
            checkBox_Log_EditorReader.UseVisualStyleBackColor = true;
            // 
            // checkBox_Log_Program
            // 
            resources.ApplyResources(checkBox_Log_Program, "checkBox_Log_Program");
            checkBox_Log_Program.Name = "checkBox_Log_Program";
            checkBox_Log_Program.UseVisualStyleBackColor = true;
            // 
            // checkBox_showConsole
            // 
            resources.ApplyResources(checkBox_showConsole, "checkBox_showConsole");
            checkBox_showConsole.Name = "checkBox_showConsole";
            checkBox_showConsole.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(checkBox_BarLine_Show);
            groupBox7.Controls.Add(checkBox_TimingLine_ShowGreen);
            groupBox7.Controls.Add(checkBox_TimingLine_ShowRed);
            resources.ApplyResources(groupBox7, "groupBox7");
            groupBox7.Name = "groupBox7";
            groupBox7.TabStop = false;
            // 
            // checkBox_BarLine_Show
            // 
            resources.ApplyResources(checkBox_BarLine_Show, "checkBox_BarLine_Show");
            checkBox_BarLine_Show.Name = "checkBox_BarLine_Show";
            checkBox_BarLine_Show.UseVisualStyleBackColor = true;
            // 
            // checkBox_TimingLine_ShowGreen
            // 
            resources.ApplyResources(checkBox_TimingLine_ShowGreen, "checkBox_TimingLine_ShowGreen");
            checkBox_TimingLine_ShowGreen.Name = "checkBox_TimingLine_ShowGreen";
            checkBox_TimingLine_ShowGreen.UseVisualStyleBackColor = true;
            // 
            // checkBox_TimingLine_ShowRed
            // 
            resources.ApplyResources(checkBox_TimingLine_ShowRed, "checkBox_TimingLine_ShowRed");
            checkBox_TimingLine_ShowRed.Name = "checkBox_TimingLine_ShowRed";
            checkBox_TimingLine_ShowRed.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(button_timeOut_reset);
            groupBox8.Controls.Add(label11);
            groupBox8.Controls.Add(label10);
            groupBox8.Controls.Add(numericUpDown_timeOut);
            groupBox8.Controls.Add(label9);
            groupBox8.Controls.Add(checkBox_FilterNearbyHitObjects);
            resources.ApplyResources(groupBox8, "groupBox8");
            groupBox8.Name = "groupBox8";
            groupBox8.TabStop = false;
            // 
            // button_timeOut_reset
            // 
            resources.ApplyResources(button_timeOut_reset, "button_timeOut_reset");
            button_timeOut_reset.Name = "button_timeOut_reset";
            button_timeOut_reset.UseVisualStyleBackColor = true;
            button_timeOut_reset.Click += button_timeOut_reset_Click;
            // 
            // label11
            // 
            resources.ApplyResources(label11, "label11");
            label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.Name = "label10";
            // 
            // numericUpDown_timeOut
            // 
            resources.ApplyResources(numericUpDown_timeOut, "numericUpDown_timeOut");
            numericUpDown_timeOut.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numericUpDown_timeOut.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDown_timeOut.Name = "numericUpDown_timeOut";
            numericUpDown_timeOut.Value = new decimal(new int[] { 3000, 0, 0, 0 });
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // checkBox_FilterNearbyHitObjects
            // 
            resources.ApplyResources(checkBox_FilterNearbyHitObjects, "checkBox_FilterNearbyHitObjects");
            checkBox_FilterNearbyHitObjects.Name = "checkBox_FilterNearbyHitObjects";
            checkBox_FilterNearbyHitObjects.UseVisualStyleBackColor = true;
            checkBox_FilterNearbyHitObjects.CheckedChanged += checkBox_FilterNearbyHitObjects_CheckedChanged;
            // 
            // checkBox_Log_BookmarkPlus
            // 
            resources.ApplyResources(checkBox_Log_BookmarkPlus, "checkBox_Log_BookmarkPlus");
            checkBox_Log_BookmarkPlus.Name = "checkBox_Log_BookmarkPlus";
            checkBox_Log_BookmarkPlus.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox8);
            Controls.Add(groupBox7);
            Controls.Add(groupBox6);
            Controls.Add(button_cancel);
            Controls.Add(button_apply);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "SettingsForm";
            Load += SettingsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_height).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_width).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_idleInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_drawingInterval).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_backupInterval).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_timeOut).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label2;
        private Label label1;
        private Button button_width_reset;
        private NumericUpDown numericUpDown_height;
        private NumericUpDown numericUpDown_width;
        private Button button_height_reset;
        private GroupBox groupBox2;
        private Button button_osuFolder_select;
        private TextBox textBox_osuFolder;
        private Label label3;
        private Button button_osuFolder_reset;
        private GroupBox groupBox3;
        private Button button_drawingInterval_reset;
        private NumericUpDown numericUpDown_idleInterval;
        private Button button_idleInterval_reset;
        private Label label5;
        private NumericUpDown numericUpDown_drawingInterval;
        private Label label4;
        private GroupBox groupBox4;
        private Button button_backupFolder_reset;
        private TextBox textBox_backupFolder;
        private Label label6;
        private Button button_backupFolder_select;
        private CheckBox checkBox_enableBackup;
        private NumericUpDown numericUpDown_backupInterval;
        private Button button_backupInterval_reset;
        private Label label7;
        private Button button_apply;
        private Button button_cancel;
        private GroupBox groupBox5;
        private CheckBox checkBox_withColor;
        private GroupBox groupBox6;
        private CheckBox checkBox_showConsole;
        private CheckBox checkBox_Log_Program;
        private CheckBox checkBox_Log_Backup;
        private CheckBox checkBox_Log_Drawing;
        private CheckBox checkBox_Log_BeatmapConverter;
        private CheckBox checkBox_Log_EditorReader;
        private CheckBox checkBox_Log_Timer;
        private ComboBox comboBox_Log_Level;
        private Label label8;
        private CheckBox checkBox_Log_BeatmapBuilder;
        private GroupBox groupBox7;
        private CheckBox checkBox_TimingLine_ShowGreen;
        private CheckBox checkBox_TimingLine_ShowRed;
        private CheckBox checkBox_BarLine_Show;
        private CheckBox checkBox_ShowSelected;
        private GroupBox groupBox8;
        private CheckBox checkBox_FilterNearbyHitObjects;
        private Label label10;
        private NumericUpDown numericUpDown_timeOut;
        private Label label9;
        private Label label11;
        private Button button_timeOut_reset;
        private Label label12;
        private Button button_Label_Color;
        private CheckBox checkBox_Log_BookmarkPlus;
    }
}