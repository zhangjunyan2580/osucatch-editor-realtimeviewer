namespace osucatch_editor_realtimeviewer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            reader_timer = new System.Windows.Forms.Timer(components);
            Canvas = new Canvas();
            menuStrip1 = new MenuStrip();
            viewerToolStripMenuItem = new ToolStripMenuItem();
            modToolStripMenuItem = new ToolStripMenuItem();
            hRToolStripMenuItem = new ToolStripMenuItem();
            eZToolStripMenuItem = new ToolStripMenuItem();
            noneToolStripMenuItem = new ToolStripMenuItem();
            distanceToolStripMenuItem = new ToolStripMenuItem();
            hideToolStripMenuItem = new ToolStripMenuItem();
            sameWithEditorToolStripMenuItem = new ToolStripMenuItem();
            noSliderVelocityMultiplierToolStripMenuItem = new ToolStripMenuItem();
            compareWithWalkSpeedToolStripMenuItem = new ToolStripMenuItem();
            screenToolStripMenuItem = new ToolStripMenuItem();
            Screens1ToolStripMenuItem = new ToolStripMenuItem();
            Screens2ToolStripMenuItem = new ToolStripMenuItem();
            Screens3ToolStripMenuItem = new ToolStripMenuItem();
            Screens4ToolStripMenuItem = new ToolStripMenuItem();
            Screens5ToolStripMenuItem = new ToolStripMenuItem();
            Screens6ToolStripMenuItem = new ToolStripMenuItem();
            Screens7ToolStripMenuItem = new ToolStripMenuItem();
            Screens8ToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            openSettingsFileToolStripMenuItem = new ToolStripMenuItem();
            githubToolStripMenuItem = new ToolStripMenuItem();
            githubToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // reader_timer
            // 
            reader_timer.Interval = 1000;
            reader_timer.Tick += reader_timer_Tick;
            // 
            // Canvas
            // 
            Canvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Canvas.BackColor = Color.Black;
            Canvas.Location = new Point(0, 30);
            Canvas.Margin = new Padding(4, 5, 4, 5);
            Canvas.Name = "Canvas";
            Canvas.Size = new Size(256, 576);
            Canvas.TabIndex = 33;
            Canvas.VSync = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { viewerToolStripMenuItem, settingsToolStripMenuItem, githubToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(256, 25);
            menuStrip1.TabIndex = 34;
            menuStrip1.Text = "menuStrip1";
            // 
            // viewerToolStripMenuItem
            // 
            viewerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { modToolStripMenuItem, distanceToolStripMenuItem, screenToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            viewerToolStripMenuItem.Name = "viewerToolStripMenuItem";
            viewerToolStripMenuItem.Size = new Size(59, 21);
            viewerToolStripMenuItem.Text = "&Viewer";
            // 
            // modToolStripMenuItem
            // 
            modToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hRToolStripMenuItem, eZToolStripMenuItem, noneToolStripMenuItem });
            modToolStripMenuItem.Name = "modToolStripMenuItem";
            modToolStripMenuItem.Size = new Size(175, 22);
            modToolStripMenuItem.Text = "&Mod";
            // 
            // hRToolStripMenuItem
            // 
            hRToolStripMenuItem.Name = "hRToolStripMenuItem";
            hRToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
            hRToolStripMenuItem.Size = new Size(157, 22);
            hRToolStripMenuItem.Text = "&HR";
            hRToolStripMenuItem.Click += hRToolStripMenuItem_Click;
            // 
            // eZToolStripMenuItem
            // 
            eZToolStripMenuItem.Name = "eZToolStripMenuItem";
            eZToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            eZToolStripMenuItem.Size = new Size(157, 22);
            eZToolStripMenuItem.Text = "&EZ";
            eZToolStripMenuItem.Click += eZToolStripMenuItem_Click;
            // 
            // noneToolStripMenuItem
            // 
            noneToolStripMenuItem.Checked = true;
            noneToolStripMenuItem.CheckState = CheckState.Checked;
            noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            noneToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.W;
            noneToolStripMenuItem.Size = new Size(157, 22);
            noneToolStripMenuItem.Text = "&None";
            noneToolStripMenuItem.Click += noneToolStripMenuItem_Click;
            // 
            // distanceToolStripMenuItem
            // 
            distanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hideToolStripMenuItem, sameWithEditorToolStripMenuItem, noSliderVelocityMultiplierToolStripMenuItem, compareWithWalkSpeedToolStripMenuItem });
            distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
            distanceToolStripMenuItem.Size = new Size(175, 22);
            distanceToolStripMenuItem.Text = "&Distance To Next";
            // 
            // hideToolStripMenuItem
            // 
            hideToolStripMenuItem.Checked = true;
            hideToolStripMenuItem.CheckState = CheckState.Checked;
            hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            hideToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Space;
            hideToolStripMenuItem.Size = new Size(283, 22);
            hideToolStripMenuItem.Text = "&Hide Distance";
            hideToolStripMenuItem.Click += hideToolStripMenuItem_Click;
            // 
            // sameWithEditorToolStripMenuItem
            // 
            sameWithEditorToolStripMenuItem.Name = "sameWithEditorToolStripMenuItem";
            sameWithEditorToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D1;
            sameWithEditorToolStripMenuItem.Size = new Size(283, 22);
            sameWithEditorToolStripMenuItem.Text = "&Same With Editor";
            sameWithEditorToolStripMenuItem.Click += sameWithEditorToolStripMenuItem_Click;
            // 
            // noSliderVelocityMultiplierToolStripMenuItem
            // 
            noSliderVelocityMultiplierToolStripMenuItem.Name = "noSliderVelocityMultiplierToolStripMenuItem";
            noSliderVelocityMultiplierToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D2;
            noSliderVelocityMultiplierToolStripMenuItem.Size = new Size(283, 22);
            noSliderVelocityMultiplierToolStripMenuItem.Text = "&No Slider Velocity Multiplier";
            noSliderVelocityMultiplierToolStripMenuItem.Click += noSliderVelocityMultiplierToolStripMenuItem_Click;
            // 
            // compareWithWalkSpeedToolStripMenuItem
            // 
            compareWithWalkSpeedToolStripMenuItem.Name = "compareWithWalkSpeedToolStripMenuItem";
            compareWithWalkSpeedToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D3;
            compareWithWalkSpeedToolStripMenuItem.Size = new Size(283, 22);
            compareWithWalkSpeedToolStripMenuItem.Text = "&Compare With Walk Speed";
            compareWithWalkSpeedToolStripMenuItem.Click += compareWithWalkSpeedToolStripMenuItem_Click;
            // 
            // screenToolStripMenuItem
            // 
            screenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { Screens1ToolStripMenuItem, Screens2ToolStripMenuItem, Screens3ToolStripMenuItem, Screens4ToolStripMenuItem, Screens5ToolStripMenuItem, Screens6ToolStripMenuItem, Screens7ToolStripMenuItem, Screens8ToolStripMenuItem });
            screenToolStripMenuItem.Name = "screenToolStripMenuItem";
            screenToolStripMenuItem.Size = new Size(175, 22);
            screenToolStripMenuItem.Text = "&Accommodate";
            // 
            // Screens1ToolStripMenuItem
            // 
            Screens1ToolStripMenuItem.Name = "Screens1ToolStripMenuItem";
            Screens1ToolStripMenuItem.Size = new Size(139, 22);
            Screens1ToolStripMenuItem.Text = "1 (Preview)";
            Screens1ToolStripMenuItem.Click += Screens1ToolStripMenuItem_Click;
            // 
            // Screens2ToolStripMenuItem
            // 
            Screens2ToolStripMenuItem.Name = "Screens2ToolStripMenuItem";
            Screens2ToolStripMenuItem.Size = new Size(139, 22);
            Screens2ToolStripMenuItem.Text = "2";
            Screens2ToolStripMenuItem.Click += Screens2ToolStripMenuItem_Click;
            // 
            // Screens3ToolStripMenuItem
            // 
            Screens3ToolStripMenuItem.Name = "Screens3ToolStripMenuItem";
            Screens3ToolStripMenuItem.Size = new Size(139, 22);
            Screens3ToolStripMenuItem.Text = "3";
            Screens3ToolStripMenuItem.Click += Screens3ToolStripMenuItem_Click;
            // 
            // Screens4ToolStripMenuItem
            // 
            Screens4ToolStripMenuItem.Checked = true;
            Screens4ToolStripMenuItem.CheckState = CheckState.Checked;
            Screens4ToolStripMenuItem.Name = "Screens4ToolStripMenuItem";
            Screens4ToolStripMenuItem.Size = new Size(139, 22);
            Screens4ToolStripMenuItem.Text = "4";
            Screens4ToolStripMenuItem.Click += Screens4ToolStripMenuItem_Click;
            // 
            // Screens5ToolStripMenuItem
            // 
            Screens5ToolStripMenuItem.Name = "Screens5ToolStripMenuItem";
            Screens5ToolStripMenuItem.Size = new Size(139, 22);
            Screens5ToolStripMenuItem.Text = "5";
            Screens5ToolStripMenuItem.Click += Screens5ToolStripMenuItem_Click;
            // 
            // Screens6ToolStripMenuItem
            // 
            Screens6ToolStripMenuItem.Name = "Screens6ToolStripMenuItem";
            Screens6ToolStripMenuItem.Size = new Size(139, 22);
            Screens6ToolStripMenuItem.Text = "6";
            Screens6ToolStripMenuItem.Click += Screens6ToolStripMenuItem_Click;
            // 
            // Screens7ToolStripMenuItem
            // 
            Screens7ToolStripMenuItem.Name = "Screens7ToolStripMenuItem";
            Screens7ToolStripMenuItem.Size = new Size(139, 22);
            Screens7ToolStripMenuItem.Text = "7";
            Screens7ToolStripMenuItem.Click += Screens7ToolStripMenuItem_Click;
            // 
            // Screens8ToolStripMenuItem
            // 
            Screens8ToolStripMenuItem.Name = "Screens8ToolStripMenuItem";
            Screens8ToolStripMenuItem.Size = new Size(139, 22);
            Screens8ToolStripMenuItem.Text = "8";
            Screens8ToolStripMenuItem.Click += Screens8ToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(172, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(175, 22);
            exitToolStripMenuItem.Text = "&Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openSettingsFileToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(66, 21);
            settingsToolStripMenuItem.Text = "&Settings";
            // 
            // openSettingsFileToolStripMenuItem
            // 
            openSettingsFileToolStripMenuItem.Name = "openSettingsFileToolStripMenuItem";
            openSettingsFileToolStripMenuItem.Size = new Size(158, 22);
            openSettingsFileToolStripMenuItem.Text = "&Open Settings";
            openSettingsFileToolStripMenuItem.Click += openSettingsFileToolStripMenuItem_Click;
            // 
            // githubToolStripMenuItem
            // 
            githubToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { githubToolStripMenuItem1 });
            githubToolStripMenuItem.Name = "githubToolStripMenuItem";
            githubToolStripMenuItem.Size = new Size(55, 21);
            githubToolStripMenuItem.Text = "&About";
            // 
            // githubToolStripMenuItem1
            // 
            githubToolStripMenuItem1.Name = "githubToolStripMenuItem1";
            githubToolStripMenuItem1.Size = new Size(114, 22);
            githubToolStripMenuItem1.Text = "&Github";
            githubToolStripMenuItem1.Click += githubToolStripMenuItem1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(256, 606);
            Controls.Add(Canvas);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "OsuCatch Editor Realtime Viewer";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer reader_timer;
        private Canvas Canvas;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem viewerToolStripMenuItem;
        private ToolStripMenuItem modToolStripMenuItem;
        private ToolStripMenuItem noneToolStripMenuItem;
        private ToolStripMenuItem hRToolStripMenuItem;
        private ToolStripMenuItem eZToolStripMenuItem;
        private ToolStripMenuItem githubToolStripMenuItem;
        private ToolStripMenuItem githubToolStripMenuItem1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem openSettingsFileToolStripMenuItem;
        private ToolStripMenuItem distanceToolStripMenuItem;
        private ToolStripMenuItem hideToolStripMenuItem;
        private ToolStripMenuItem sameWithEditorToolStripMenuItem;
        private ToolStripMenuItem noSliderVelocityMultiplierToolStripMenuItem;
        private ToolStripMenuItem compareWithWalkSpeedToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem screenToolStripMenuItem;
        private ToolStripMenuItem Screens1ToolStripMenuItem;
        private ToolStripMenuItem Screens2ToolStripMenuItem;
        private ToolStripMenuItem Screens3ToolStripMenuItem;
        private ToolStripMenuItem Screens4ToolStripMenuItem;
        private ToolStripMenuItem Screens5ToolStripMenuItem;
        private ToolStripMenuItem Screens6ToolStripMenuItem;
        private ToolStripMenuItem Screens7ToolStripMenuItem;
        private ToolStripMenuItem Screens8ToolStripMenuItem;
    }
}
