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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            languageToolStripMenuItem = new ToolStripMenuItem();
            defaultLanguageToolStripMenuItem = new ToolStripMenuItem();
            englishLanguageToolStripMenuItem = new ToolStripMenuItem();
            zhHansLanguageToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            backupToolStripMenuItem = new ToolStripMenuItem();
            githubToolStripMenuItem = new ToolStripMenuItem();
            githubToolStripMenuItem1 = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // Canvas
            // 
            resources.ApplyResources(Canvas, "Canvas");
            Canvas.BackColor = Color.Black;
            Canvas.Name = "Canvas";
            Canvas.VSync = false;
            // 
            // menuStrip1
            // 
            resources.ApplyResources(menuStrip1, "menuStrip1");
            menuStrip1.Items.AddRange(new ToolStripItem[] { viewerToolStripMenuItem, settingsToolStripMenuItem, githubToolStripMenuItem });
            menuStrip1.Name = "menuStrip1";
            // 
            // viewerToolStripMenuItem
            // 
            resources.ApplyResources(viewerToolStripMenuItem, "viewerToolStripMenuItem");
            viewerToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { modToolStripMenuItem, distanceToolStripMenuItem, screenToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            viewerToolStripMenuItem.Name = "viewerToolStripMenuItem";
            // 
            // modToolStripMenuItem
            // 
            resources.ApplyResources(modToolStripMenuItem, "modToolStripMenuItem");
            modToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hRToolStripMenuItem, eZToolStripMenuItem, noneToolStripMenuItem });
            modToolStripMenuItem.Name = "modToolStripMenuItem";
            // 
            // hRToolStripMenuItem
            // 
            resources.ApplyResources(hRToolStripMenuItem, "hRToolStripMenuItem");
            hRToolStripMenuItem.Name = "hRToolStripMenuItem";
            hRToolStripMenuItem.Click += hRToolStripMenuItem_Click;
            // 
            // eZToolStripMenuItem
            // 
            resources.ApplyResources(eZToolStripMenuItem, "eZToolStripMenuItem");
            eZToolStripMenuItem.Name = "eZToolStripMenuItem";
            eZToolStripMenuItem.Click += eZToolStripMenuItem_Click;
            // 
            // noneToolStripMenuItem
            // 
            resources.ApplyResources(noneToolStripMenuItem, "noneToolStripMenuItem");
            noneToolStripMenuItem.Checked = true;
            noneToolStripMenuItem.CheckState = CheckState.Checked;
            noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            noneToolStripMenuItem.Click += noneToolStripMenuItem_Click;
            // 
            // distanceToolStripMenuItem
            // 
            resources.ApplyResources(distanceToolStripMenuItem, "distanceToolStripMenuItem");
            distanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { hideToolStripMenuItem, sameWithEditorToolStripMenuItem, noSliderVelocityMultiplierToolStripMenuItem, compareWithWalkSpeedToolStripMenuItem });
            distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
            // 
            // hideToolStripMenuItem
            // 
            resources.ApplyResources(hideToolStripMenuItem, "hideToolStripMenuItem");
            hideToolStripMenuItem.Checked = true;
            hideToolStripMenuItem.CheckState = CheckState.Checked;
            hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            hideToolStripMenuItem.Click += hideToolStripMenuItem_Click;
            // 
            // sameWithEditorToolStripMenuItem
            // 
            resources.ApplyResources(sameWithEditorToolStripMenuItem, "sameWithEditorToolStripMenuItem");
            sameWithEditorToolStripMenuItem.Name = "sameWithEditorToolStripMenuItem";
            sameWithEditorToolStripMenuItem.Click += sameWithEditorToolStripMenuItem_Click;
            // 
            // noSliderVelocityMultiplierToolStripMenuItem
            // 
            resources.ApplyResources(noSliderVelocityMultiplierToolStripMenuItem, "noSliderVelocityMultiplierToolStripMenuItem");
            noSliderVelocityMultiplierToolStripMenuItem.Name = "noSliderVelocityMultiplierToolStripMenuItem";
            noSliderVelocityMultiplierToolStripMenuItem.Click += noSliderVelocityMultiplierToolStripMenuItem_Click;
            // 
            // compareWithWalkSpeedToolStripMenuItem
            // 
            resources.ApplyResources(compareWithWalkSpeedToolStripMenuItem, "compareWithWalkSpeedToolStripMenuItem");
            compareWithWalkSpeedToolStripMenuItem.Name = "compareWithWalkSpeedToolStripMenuItem";
            compareWithWalkSpeedToolStripMenuItem.Click += compareWithWalkSpeedToolStripMenuItem_Click;
            // 
            // screenToolStripMenuItem
            // 
            resources.ApplyResources(screenToolStripMenuItem, "screenToolStripMenuItem");
            screenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { Screens1ToolStripMenuItem, Screens2ToolStripMenuItem, Screens3ToolStripMenuItem, Screens4ToolStripMenuItem, Screens5ToolStripMenuItem, Screens6ToolStripMenuItem, Screens7ToolStripMenuItem, Screens8ToolStripMenuItem });
            screenToolStripMenuItem.Name = "screenToolStripMenuItem";
            // 
            // Screens1ToolStripMenuItem
            // 
            resources.ApplyResources(Screens1ToolStripMenuItem, "Screens1ToolStripMenuItem");
            Screens1ToolStripMenuItem.Name = "Screens1ToolStripMenuItem";
            Screens1ToolStripMenuItem.Click += Screens1ToolStripMenuItem_Click;
            // 
            // Screens2ToolStripMenuItem
            // 
            resources.ApplyResources(Screens2ToolStripMenuItem, "Screens2ToolStripMenuItem");
            Screens2ToolStripMenuItem.Name = "Screens2ToolStripMenuItem";
            Screens2ToolStripMenuItem.Click += Screens2ToolStripMenuItem_Click;
            // 
            // Screens3ToolStripMenuItem
            // 
            resources.ApplyResources(Screens3ToolStripMenuItem, "Screens3ToolStripMenuItem");
            Screens3ToolStripMenuItem.Name = "Screens3ToolStripMenuItem";
            Screens3ToolStripMenuItem.Click += Screens3ToolStripMenuItem_Click;
            // 
            // Screens4ToolStripMenuItem
            // 
            resources.ApplyResources(Screens4ToolStripMenuItem, "Screens4ToolStripMenuItem");
            Screens4ToolStripMenuItem.Checked = true;
            Screens4ToolStripMenuItem.CheckState = CheckState.Checked;
            Screens4ToolStripMenuItem.Name = "Screens4ToolStripMenuItem";
            Screens4ToolStripMenuItem.Click += Screens4ToolStripMenuItem_Click;
            // 
            // Screens5ToolStripMenuItem
            // 
            resources.ApplyResources(Screens5ToolStripMenuItem, "Screens5ToolStripMenuItem");
            Screens5ToolStripMenuItem.Name = "Screens5ToolStripMenuItem";
            Screens5ToolStripMenuItem.Click += Screens5ToolStripMenuItem_Click;
            // 
            // Screens6ToolStripMenuItem
            // 
            resources.ApplyResources(Screens6ToolStripMenuItem, "Screens6ToolStripMenuItem");
            Screens6ToolStripMenuItem.Name = "Screens6ToolStripMenuItem";
            Screens6ToolStripMenuItem.Click += Screens6ToolStripMenuItem_Click;
            // 
            // Screens7ToolStripMenuItem
            // 
            resources.ApplyResources(Screens7ToolStripMenuItem, "Screens7ToolStripMenuItem");
            Screens7ToolStripMenuItem.Name = "Screens7ToolStripMenuItem";
            Screens7ToolStripMenuItem.Click += Screens7ToolStripMenuItem_Click;
            // 
            // Screens8ToolStripMenuItem
            // 
            resources.ApplyResources(Screens8ToolStripMenuItem, "Screens8ToolStripMenuItem");
            Screens8ToolStripMenuItem.Name = "Screens8ToolStripMenuItem";
            Screens8ToolStripMenuItem.Click += Screens8ToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(toolStripSeparator1, "toolStripSeparator1");
            toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // exitToolStripMenuItem
            // 
            resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            resources.ApplyResources(settingsToolStripMenuItem, "settingsToolStripMenuItem");
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openSettingsFileToolStripMenuItem, languageToolStripMenuItem, toolStripSeparator2, backupToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            // 
            // openSettingsFileToolStripMenuItem
            // 
            resources.ApplyResources(openSettingsFileToolStripMenuItem, "openSettingsFileToolStripMenuItem");
            openSettingsFileToolStripMenuItem.Name = "openSettingsFileToolStripMenuItem";
            openSettingsFileToolStripMenuItem.Click += openSettingsFileToolStripMenuItem_Click;
            // 
            // languageToolStripMenuItem
            // 
            resources.ApplyResources(languageToolStripMenuItem, "languageToolStripMenuItem");
            languageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { defaultLanguageToolStripMenuItem, englishLanguageToolStripMenuItem, zhHansLanguageToolStripMenuItem });
            languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            // 
            // defaultLanguageToolStripMenuItem
            // 
            resources.ApplyResources(defaultLanguageToolStripMenuItem, "defaultLanguageToolStripMenuItem");
            defaultLanguageToolStripMenuItem.Name = "defaultLanguageToolStripMenuItem";
            defaultLanguageToolStripMenuItem.Click += defaultLanguageToolStripMenuItem_Click;
            // 
            // englishLanguageToolStripMenuItem
            // 
            resources.ApplyResources(englishLanguageToolStripMenuItem, "englishLanguageToolStripMenuItem");
            englishLanguageToolStripMenuItem.Name = "englishLanguageToolStripMenuItem";
            englishLanguageToolStripMenuItem.Click += englishLanguageToolStripMenuItem_Click;
            // 
            // zhHansLanguageToolStripMenuItem
            // 
            resources.ApplyResources(zhHansLanguageToolStripMenuItem, "zhHansLanguageToolStripMenuItem");
            zhHansLanguageToolStripMenuItem.Name = "zhHansLanguageToolStripMenuItem";
            zhHansLanguageToolStripMenuItem.Click += zhHansLanguageToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(toolStripSeparator2, "toolStripSeparator2");
            toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // backupToolStripMenuItem
            // 
            resources.ApplyResources(backupToolStripMenuItem, "backupToolStripMenuItem");
            backupToolStripMenuItem.Name = "backupToolStripMenuItem";
            backupToolStripMenuItem.Click += backupToolStripMenuItem_Click;
            // 
            // githubToolStripMenuItem
            // 
            resources.ApplyResources(githubToolStripMenuItem, "githubToolStripMenuItem");
            githubToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { githubToolStripMenuItem1 });
            githubToolStripMenuItem.Name = "githubToolStripMenuItem";
            // 
            // githubToolStripMenuItem1
            // 
            resources.ApplyResources(githubToolStripMenuItem1, "githubToolStripMenuItem1");
            githubToolStripMenuItem1.Name = "githubToolStripMenuItem1";
            githubToolStripMenuItem1.Click += githubToolStripMenuItem1_Click;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Canvas);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
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
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem backupToolStripMenuItem;
        private ToolStripMenuItem languageToolStripMenuItem;
        private ToolStripMenuItem englishLanguageToolStripMenuItem;
        private ToolStripMenuItem zhHansLanguageToolStripMenuItem;
        private ToolStripMenuItem defaultLanguageToolStripMenuItem;
    }
}
