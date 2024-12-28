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
            Canvas.CatcherAreaHeight = 0F;
            Canvas.Location = new Point(13, 13);
            Canvas.Margin = new Padding(4, 4, 4, 4);
            Canvas.Name = "Canvas";
            Canvas.ShowHelp = 0;
            Canvas.Size = new Size(208, 685);
            Canvas.TabIndex = 33;
            Canvas.VSync = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(234, 711);
            Controls.Add(Canvas);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer reader_timer;
        private Canvas Canvas;

    }
}
