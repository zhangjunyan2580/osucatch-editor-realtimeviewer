namespace osucatch_editor_realtimeviewer
{
    partial class BookmarkSettingsForm
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
            groupBox1 = new GroupBox();
            label1 = new Label();
            loadWithStyleCheckBox = new CheckBox();
            button_saveFolder_reset = new Button();
            button_saveFolder_select = new Button();
            autoLoadSaveCheckBox = new CheckBox();
            textBox_saveFolder = new TextBox();
            label3 = new Label();
            groupBox2 = new GroupBox();
            button_loadStyle = new Button();
            button_saveStyle = new Button();
            commentTextBox_8 = new TextBox();
            commentTextBox_7 = new TextBox();
            commentTextBox_6 = new TextBox();
            commentTextBox_5 = new TextBox();
            commentTextBox_4 = new TextBox();
            commentTextBox_3 = new TextBox();
            commentTextBox_2 = new TextBox();
            commentTextBox_1 = new TextBox();
            label42 = new Label();
            label41 = new Label();
            label40 = new Label();
            label39 = new Label();
            label38 = new Label();
            label37 = new Label();
            label36 = new Label();
            label35 = new Label();
            colorbutton_8 = new Button();
            label31 = new Label();
            lineWidthComboBox_8 = new ComboBox();
            label32 = new Label();
            dashStyleComboBox_8 = new ComboBox();
            label33 = new Label();
            label34 = new Label();
            colorbutton_7 = new Button();
            label27 = new Label();
            lineWidthComboBox_7 = new ComboBox();
            label28 = new Label();
            dashStyleComboBox_7 = new ComboBox();
            label29 = new Label();
            label30 = new Label();
            colorbutton_6 = new Button();
            label23 = new Label();
            lineWidthComboBox_6 = new ComboBox();
            label24 = new Label();
            dashStyleComboBox_6 = new ComboBox();
            label25 = new Label();
            label26 = new Label();
            colorbutton_5 = new Button();
            label19 = new Label();
            lineWidthComboBox_5 = new ComboBox();
            label20 = new Label();
            dashStyleComboBox_5 = new ComboBox();
            label21 = new Label();
            label22 = new Label();
            colorbutton_4 = new Button();
            label15 = new Label();
            lineWidthComboBox_4 = new ComboBox();
            label16 = new Label();
            dashStyleComboBox_4 = new ComboBox();
            label17 = new Label();
            label18 = new Label();
            colorbutton_3 = new Button();
            label11 = new Label();
            lineWidthComboBox_3 = new ComboBox();
            label12 = new Label();
            dashStyleComboBox_3 = new ComboBox();
            label13 = new Label();
            label14 = new Label();
            colorbutton_2 = new Button();
            label7 = new Label();
            lineWidthComboBox_2 = new ComboBox();
            label8 = new Label();
            dashStyleComboBox_2 = new ComboBox();
            label9 = new Label();
            label10 = new Label();
            colorbutton_1 = new Button();
            label6 = new Label();
            lineWidthComboBox_1 = new ComboBox();
            label5 = new Label();
            dashStyleComboBox_1 = new ComboBox();
            label4 = new Label();
            label2 = new Label();
            button_cancel = new Button();
            button_apply = new Button();
            button_default = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(loadWithStyleCheckBox);
            groupBox1.Controls.Add(button_saveFolder_reset);
            groupBox1.Controls.Add(button_saveFolder_select);
            groupBox1.Controls.Add(autoLoadSaveCheckBox);
            groupBox1.Controls.Add(textBox_saveFolder);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(772, 135);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Auto";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 73);
            label1.Name = "label1";
            label1.Size = new Size(620, 17);
            label1.TabIndex = 11;
            label1.Text = "Warning: The original style settings will be overwritten when auto-loading a bookmark with style settings.";
            // 
            // loadWithStyleCheckBox
            // 
            loadWithStyleCheckBox.AutoSize = true;
            loadWithStyleCheckBox.Location = new Point(6, 49);
            loadWithStyleCheckBox.Name = "loadWithStyleCheckBox";
            loadWithStyleCheckBox.Size = new Size(232, 21);
            loadWithStyleCheckBox.TabIndex = 10;
            loadWithStyleCheckBox.Text = "Load Beatmap's Bookmark+ Styles";
            loadWithStyleCheckBox.UseVisualStyleBackColor = true;
            // 
            // button_saveFolder_reset
            // 
            button_saveFolder_reset.ImeMode = ImeMode.NoControl;
            button_saveFolder_reset.Location = new Point(546, 98);
            button_saveFolder_reset.Name = "button_saveFolder_reset";
            button_saveFolder_reset.Size = new Size(25, 23);
            button_saveFolder_reset.TabIndex = 9;
            button_saveFolder_reset.Text = "↺";
            button_saveFolder_reset.UseVisualStyleBackColor = true;
            button_saveFolder_reset.Click += button_saveFolder_reset_Click;
            // 
            // button_saveFolder_select
            // 
            button_saveFolder_select.ImeMode = ImeMode.NoControl;
            button_saveFolder_select.Location = new Point(494, 97);
            button_saveFolder_select.Name = "button_saveFolder_select";
            button_saveFolder_select.Size = new Size(46, 24);
            button_saveFolder_select.TabIndex = 8;
            button_saveFolder_select.Text = "...";
            button_saveFolder_select.UseVisualStyleBackColor = true;
            button_saveFolder_select.Click += button_saveFolder_select_Click;
            // 
            // autoLoadSaveCheckBox
            // 
            autoLoadSaveCheckBox.AutoSize = true;
            autoLoadSaveCheckBox.Location = new Point(6, 22);
            autoLoadSaveCheckBox.Name = "autoLoadSaveCheckBox";
            autoLoadSaveCheckBox.Size = new Size(193, 21);
            autoLoadSaveCheckBox.TabIndex = 0;
            autoLoadSaveCheckBox.Text = "Auto Load/Save Bookmark+";
            autoLoadSaveCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBox_saveFolder
            // 
            textBox_saveFolder.Location = new Point(141, 97);
            textBox_saveFolder.Name = "textBox_saveFolder";
            textBox_saveFolder.Size = new Size(347, 23);
            textBox_saveFolder.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ImeMode = ImeMode.NoControl;
            label3.Location = new Point(6, 100);
            label3.Name = "label3";
            label3.Size = new Size(110, 17);
            label3.TabIndex = 6;
            label3.Text = "Auto Save Folder:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button_loadStyle);
            groupBox2.Controls.Add(button_saveStyle);
            groupBox2.Controls.Add(commentTextBox_8);
            groupBox2.Controls.Add(commentTextBox_7);
            groupBox2.Controls.Add(commentTextBox_6);
            groupBox2.Controls.Add(commentTextBox_5);
            groupBox2.Controls.Add(commentTextBox_4);
            groupBox2.Controls.Add(commentTextBox_3);
            groupBox2.Controls.Add(commentTextBox_2);
            groupBox2.Controls.Add(commentTextBox_1);
            groupBox2.Controls.Add(label42);
            groupBox2.Controls.Add(label41);
            groupBox2.Controls.Add(label40);
            groupBox2.Controls.Add(label39);
            groupBox2.Controls.Add(label38);
            groupBox2.Controls.Add(label37);
            groupBox2.Controls.Add(label36);
            groupBox2.Controls.Add(label35);
            groupBox2.Controls.Add(colorbutton_8);
            groupBox2.Controls.Add(label31);
            groupBox2.Controls.Add(lineWidthComboBox_8);
            groupBox2.Controls.Add(label32);
            groupBox2.Controls.Add(dashStyleComboBox_8);
            groupBox2.Controls.Add(label33);
            groupBox2.Controls.Add(label34);
            groupBox2.Controls.Add(colorbutton_7);
            groupBox2.Controls.Add(label27);
            groupBox2.Controls.Add(lineWidthComboBox_7);
            groupBox2.Controls.Add(label28);
            groupBox2.Controls.Add(dashStyleComboBox_7);
            groupBox2.Controls.Add(label29);
            groupBox2.Controls.Add(label30);
            groupBox2.Controls.Add(colorbutton_6);
            groupBox2.Controls.Add(label23);
            groupBox2.Controls.Add(lineWidthComboBox_6);
            groupBox2.Controls.Add(label24);
            groupBox2.Controls.Add(dashStyleComboBox_6);
            groupBox2.Controls.Add(label25);
            groupBox2.Controls.Add(label26);
            groupBox2.Controls.Add(colorbutton_5);
            groupBox2.Controls.Add(label19);
            groupBox2.Controls.Add(lineWidthComboBox_5);
            groupBox2.Controls.Add(label20);
            groupBox2.Controls.Add(dashStyleComboBox_5);
            groupBox2.Controls.Add(label21);
            groupBox2.Controls.Add(label22);
            groupBox2.Controls.Add(colorbutton_4);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(lineWidthComboBox_4);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(dashStyleComboBox_4);
            groupBox2.Controls.Add(label17);
            groupBox2.Controls.Add(label18);
            groupBox2.Controls.Add(colorbutton_3);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(lineWidthComboBox_3);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(dashStyleComboBox_3);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(colorbutton_2);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(lineWidthComboBox_2);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(dashStyleComboBox_2);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(colorbutton_1);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(lineWidthComboBox_1);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(dashStyleComboBox_1);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(12, 153);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(772, 329);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Bookmark+ Style";
            // 
            // button_loadStyle
            // 
            button_loadStyle.ImeMode = ImeMode.NoControl;
            button_loadStyle.Location = new Point(534, 280);
            button_loadStyle.Name = "button_loadStyle";
            button_loadStyle.Size = new Size(102, 33);
            button_loadStyle.TabIndex = 73;
            button_loadStyle.Text = "Load from File";
            button_loadStyle.UseVisualStyleBackColor = true;
            button_loadStyle.Click += button_loadStyle_Click;
            // 
            // button_saveStyle
            // 
            button_saveStyle.ImeMode = ImeMode.NoControl;
            button_saveStyle.Location = new Point(656, 280);
            button_saveStyle.Name = "button_saveStyle";
            button_saveStyle.Size = new Size(102, 33);
            button_saveStyle.TabIndex = 72;
            button_saveStyle.Text = "Save to File";
            button_saveStyle.UseVisualStyleBackColor = true;
            button_saveStyle.Click += button_saveStyle_Click;
            // 
            // commentTextBox_8
            // 
            commentTextBox_8.Location = new Point(598, 242);
            commentTextBox_8.Name = "commentTextBox_8";
            commentTextBox_8.Size = new Size(160, 23);
            commentTextBox_8.TabIndex = 71;
            // 
            // commentTextBox_7
            // 
            commentTextBox_7.Location = new Point(598, 211);
            commentTextBox_7.Name = "commentTextBox_7";
            commentTextBox_7.Size = new Size(160, 23);
            commentTextBox_7.TabIndex = 70;
            // 
            // commentTextBox_6
            // 
            commentTextBox_6.Location = new Point(598, 180);
            commentTextBox_6.Name = "commentTextBox_6";
            commentTextBox_6.Size = new Size(160, 23);
            commentTextBox_6.TabIndex = 69;
            // 
            // commentTextBox_5
            // 
            commentTextBox_5.Location = new Point(598, 149);
            commentTextBox_5.Name = "commentTextBox_5";
            commentTextBox_5.Size = new Size(160, 23);
            commentTextBox_5.TabIndex = 68;
            // 
            // commentTextBox_4
            // 
            commentTextBox_4.Location = new Point(598, 118);
            commentTextBox_4.Name = "commentTextBox_4";
            commentTextBox_4.Size = new Size(160, 23);
            commentTextBox_4.TabIndex = 67;
            // 
            // commentTextBox_3
            // 
            commentTextBox_3.Location = new Point(598, 87);
            commentTextBox_3.Name = "commentTextBox_3";
            commentTextBox_3.Size = new Size(160, 23);
            commentTextBox_3.TabIndex = 66;
            // 
            // commentTextBox_2
            // 
            commentTextBox_2.Location = new Point(598, 56);
            commentTextBox_2.Name = "commentTextBox_2";
            commentTextBox_2.Size = new Size(160, 23);
            commentTextBox_2.TabIndex = 65;
            // 
            // commentTextBox_1
            // 
            commentTextBox_1.Location = new Point(598, 25);
            commentTextBox_1.Name = "commentTextBox_1";
            commentTextBox_1.Size = new Size(160, 23);
            commentTextBox_1.TabIndex = 64;
            // 
            // label42
            // 
            label42.AutoSize = true;
            label42.Location = new Point(546, 245);
            label42.Name = "label42";
            label42.Size = new Size(46, 17);
            label42.TabIndex = 63;
            label42.Text = "Label: ";
            // 
            // label41
            // 
            label41.AutoSize = true;
            label41.Location = new Point(546, 214);
            label41.Name = "label41";
            label41.Size = new Size(46, 17);
            label41.TabIndex = 62;
            label41.Text = "Label: ";
            // 
            // label40
            // 
            label40.AutoSize = true;
            label40.Location = new Point(546, 183);
            label40.Name = "label40";
            label40.Size = new Size(46, 17);
            label40.TabIndex = 61;
            label40.Text = "Label: ";
            // 
            // label39
            // 
            label39.AutoSize = true;
            label39.Location = new Point(546, 152);
            label39.Name = "label39";
            label39.Size = new Size(46, 17);
            label39.TabIndex = 60;
            label39.Text = "Label: ";
            // 
            // label38
            // 
            label38.AutoSize = true;
            label38.Location = new Point(546, 121);
            label38.Name = "label38";
            label38.Size = new Size(46, 17);
            label38.TabIndex = 59;
            label38.Text = "Label: ";
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Location = new Point(546, 90);
            label37.Name = "label37";
            label37.Size = new Size(46, 17);
            label37.TabIndex = 58;
            label37.Text = "Label: ";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new Point(546, 59);
            label36.Name = "label36";
            label36.Size = new Size(46, 17);
            label36.TabIndex = 57;
            label36.Text = "Label: ";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(546, 28);
            label35.Name = "label35";
            label35.Size = new Size(46, 17);
            label35.TabIndex = 56;
            label35.Text = "Label: ";
            // 
            // colorbutton_8
            // 
            colorbutton_8.BackColor = Color.Blue;
            colorbutton_8.ImeMode = ImeMode.NoControl;
            colorbutton_8.Location = new Point(494, 242);
            colorbutton_8.Name = "colorbutton_8";
            colorbutton_8.Size = new Size(25, 25);
            colorbutton_8.TabIndex = 55;
            colorbutton_8.UseVisualStyleBackColor = false;
            colorbutton_8.Click += colorbutton_8_Click;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(441, 245);
            label31.Name = "label31";
            label31.Size = new Size(47, 17);
            label31.TabIndex = 54;
            label31.Text = "Color: ";
            // 
            // lineWidthComboBox_8
            // 
            lineWidthComboBox_8.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_8.FormattingEnabled = true;
            lineWidthComboBox_8.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_8.Location = new Point(326, 242);
            lineWidthComboBox_8.Name = "lineWidthComboBox_8";
            lineWidthComboBox_8.Size = new Size(86, 25);
            lineWidthComboBox_8.TabIndex = 53;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(271, 245);
            label32.Name = "label32";
            label32.Size = new Size(49, 17);
            label32.TabIndex = 52;
            label32.Text = "Width: ";
            // 
            // dashStyleComboBox_8
            // 
            dashStyleComboBox_8.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_8.FormattingEnabled = true;
            dashStyleComboBox_8.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_8.Location = new Point(120, 242);
            dashStyleComboBox_8.Name = "dashStyleComboBox_8";
            dashStyleComboBox_8.Size = new Size(128, 25);
            dashStyleComboBox_8.TabIndex = 51;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(39, 245);
            label33.Name = "label33";
            label33.Size = new Size(69, 17);
            label33.TabIndex = 50;
            label33.Text = "Line Style: ";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(6, 245);
            label34.Name = "label34";
            label34.Size = new Size(18, 17);
            label34.TabIndex = 49;
            label34.Text = "8.";
            // 
            // colorbutton_7
            // 
            colorbutton_7.BackColor = Color.Blue;
            colorbutton_7.ImeMode = ImeMode.NoControl;
            colorbutton_7.Location = new Point(494, 211);
            colorbutton_7.Name = "colorbutton_7";
            colorbutton_7.Size = new Size(25, 25);
            colorbutton_7.TabIndex = 48;
            colorbutton_7.UseVisualStyleBackColor = false;
            colorbutton_7.Click += colorbutton_7_Click;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(441, 214);
            label27.Name = "label27";
            label27.Size = new Size(47, 17);
            label27.TabIndex = 47;
            label27.Text = "Color: ";
            // 
            // lineWidthComboBox_7
            // 
            lineWidthComboBox_7.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_7.FormattingEnabled = true;
            lineWidthComboBox_7.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_7.Location = new Point(326, 211);
            lineWidthComboBox_7.Name = "lineWidthComboBox_7";
            lineWidthComboBox_7.Size = new Size(86, 25);
            lineWidthComboBox_7.TabIndex = 46;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(271, 214);
            label28.Name = "label28";
            label28.Size = new Size(49, 17);
            label28.TabIndex = 45;
            label28.Text = "Width: ";
            // 
            // dashStyleComboBox_7
            // 
            dashStyleComboBox_7.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_7.FormattingEnabled = true;
            dashStyleComboBox_7.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_7.Location = new Point(120, 211);
            dashStyleComboBox_7.Name = "dashStyleComboBox_7";
            dashStyleComboBox_7.Size = new Size(128, 25);
            dashStyleComboBox_7.TabIndex = 44;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(39, 214);
            label29.Name = "label29";
            label29.Size = new Size(69, 17);
            label29.TabIndex = 43;
            label29.Text = "Line Style: ";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(6, 214);
            label30.Name = "label30";
            label30.Size = new Size(18, 17);
            label30.TabIndex = 42;
            label30.Text = "7.";
            // 
            // colorbutton_6
            // 
            colorbutton_6.BackColor = Color.Blue;
            colorbutton_6.ImeMode = ImeMode.NoControl;
            colorbutton_6.Location = new Point(494, 180);
            colorbutton_6.Name = "colorbutton_6";
            colorbutton_6.Size = new Size(25, 25);
            colorbutton_6.TabIndex = 41;
            colorbutton_6.UseVisualStyleBackColor = false;
            colorbutton_6.Click += colorbutton_6_Click;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(441, 183);
            label23.Name = "label23";
            label23.Size = new Size(47, 17);
            label23.TabIndex = 40;
            label23.Text = "Color: ";
            // 
            // lineWidthComboBox_6
            // 
            lineWidthComboBox_6.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_6.FormattingEnabled = true;
            lineWidthComboBox_6.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_6.Location = new Point(326, 180);
            lineWidthComboBox_6.Name = "lineWidthComboBox_6";
            lineWidthComboBox_6.Size = new Size(86, 25);
            lineWidthComboBox_6.TabIndex = 39;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(271, 183);
            label24.Name = "label24";
            label24.Size = new Size(49, 17);
            label24.TabIndex = 38;
            label24.Text = "Width: ";
            // 
            // dashStyleComboBox_6
            // 
            dashStyleComboBox_6.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_6.FormattingEnabled = true;
            dashStyleComboBox_6.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_6.Location = new Point(120, 180);
            dashStyleComboBox_6.Name = "dashStyleComboBox_6";
            dashStyleComboBox_6.Size = new Size(128, 25);
            dashStyleComboBox_6.TabIndex = 37;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(39, 183);
            label25.Name = "label25";
            label25.Size = new Size(69, 17);
            label25.TabIndex = 36;
            label25.Text = "Line Style: ";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(6, 183);
            label26.Name = "label26";
            label26.Size = new Size(18, 17);
            label26.TabIndex = 35;
            label26.Text = "6.";
            // 
            // colorbutton_5
            // 
            colorbutton_5.BackColor = Color.Blue;
            colorbutton_5.ImeMode = ImeMode.NoControl;
            colorbutton_5.Location = new Point(494, 149);
            colorbutton_5.Name = "colorbutton_5";
            colorbutton_5.Size = new Size(25, 25);
            colorbutton_5.TabIndex = 34;
            colorbutton_5.UseVisualStyleBackColor = false;
            colorbutton_5.Click += colorbutton_5_Click;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(441, 152);
            label19.Name = "label19";
            label19.Size = new Size(47, 17);
            label19.TabIndex = 33;
            label19.Text = "Color: ";
            // 
            // lineWidthComboBox_5
            // 
            lineWidthComboBox_5.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_5.FormattingEnabled = true;
            lineWidthComboBox_5.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_5.Location = new Point(326, 149);
            lineWidthComboBox_5.Name = "lineWidthComboBox_5";
            lineWidthComboBox_5.Size = new Size(86, 25);
            lineWidthComboBox_5.TabIndex = 32;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(271, 152);
            label20.Name = "label20";
            label20.Size = new Size(49, 17);
            label20.TabIndex = 31;
            label20.Text = "Width: ";
            // 
            // dashStyleComboBox_5
            // 
            dashStyleComboBox_5.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_5.FormattingEnabled = true;
            dashStyleComboBox_5.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_5.Location = new Point(120, 149);
            dashStyleComboBox_5.Name = "dashStyleComboBox_5";
            dashStyleComboBox_5.Size = new Size(128, 25);
            dashStyleComboBox_5.TabIndex = 30;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(39, 152);
            label21.Name = "label21";
            label21.Size = new Size(69, 17);
            label21.TabIndex = 29;
            label21.Text = "Line Style: ";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(6, 152);
            label22.Name = "label22";
            label22.Size = new Size(18, 17);
            label22.TabIndex = 28;
            label22.Text = "5.";
            // 
            // colorbutton_4
            // 
            colorbutton_4.BackColor = Color.Blue;
            colorbutton_4.ImeMode = ImeMode.NoControl;
            colorbutton_4.Location = new Point(494, 118);
            colorbutton_4.Name = "colorbutton_4";
            colorbutton_4.Size = new Size(25, 25);
            colorbutton_4.TabIndex = 27;
            colorbutton_4.UseVisualStyleBackColor = false;
            colorbutton_4.Click += colorbutton_4_Click;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(441, 121);
            label15.Name = "label15";
            label15.Size = new Size(47, 17);
            label15.TabIndex = 26;
            label15.Text = "Color: ";
            // 
            // lineWidthComboBox_4
            // 
            lineWidthComboBox_4.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_4.FormattingEnabled = true;
            lineWidthComboBox_4.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_4.Location = new Point(326, 118);
            lineWidthComboBox_4.Name = "lineWidthComboBox_4";
            lineWidthComboBox_4.Size = new Size(86, 25);
            lineWidthComboBox_4.TabIndex = 25;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(271, 121);
            label16.Name = "label16";
            label16.Size = new Size(49, 17);
            label16.TabIndex = 24;
            label16.Text = "Width: ";
            // 
            // dashStyleComboBox_4
            // 
            dashStyleComboBox_4.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_4.FormattingEnabled = true;
            dashStyleComboBox_4.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_4.Location = new Point(120, 118);
            dashStyleComboBox_4.Name = "dashStyleComboBox_4";
            dashStyleComboBox_4.Size = new Size(128, 25);
            dashStyleComboBox_4.TabIndex = 23;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(39, 121);
            label17.Name = "label17";
            label17.Size = new Size(69, 17);
            label17.TabIndex = 22;
            label17.Text = "Line Style: ";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(6, 121);
            label18.Name = "label18";
            label18.Size = new Size(18, 17);
            label18.TabIndex = 21;
            label18.Text = "4.";
            // 
            // colorbutton_3
            // 
            colorbutton_3.BackColor = Color.Blue;
            colorbutton_3.ImeMode = ImeMode.NoControl;
            colorbutton_3.Location = new Point(494, 87);
            colorbutton_3.Name = "colorbutton_3";
            colorbutton_3.Size = new Size(25, 25);
            colorbutton_3.TabIndex = 20;
            colorbutton_3.UseVisualStyleBackColor = false;
            colorbutton_3.Click += colorbutton_3_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(441, 90);
            label11.Name = "label11";
            label11.Size = new Size(47, 17);
            label11.TabIndex = 19;
            label11.Text = "Color: ";
            // 
            // lineWidthComboBox_3
            // 
            lineWidthComboBox_3.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_3.FormattingEnabled = true;
            lineWidthComboBox_3.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_3.Location = new Point(326, 87);
            lineWidthComboBox_3.Name = "lineWidthComboBox_3";
            lineWidthComboBox_3.Size = new Size(86, 25);
            lineWidthComboBox_3.TabIndex = 18;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(271, 90);
            label12.Name = "label12";
            label12.Size = new Size(49, 17);
            label12.TabIndex = 17;
            label12.Text = "Width: ";
            // 
            // dashStyleComboBox_3
            // 
            dashStyleComboBox_3.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_3.FormattingEnabled = true;
            dashStyleComboBox_3.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_3.Location = new Point(120, 87);
            dashStyleComboBox_3.Name = "dashStyleComboBox_3";
            dashStyleComboBox_3.Size = new Size(128, 25);
            dashStyleComboBox_3.TabIndex = 16;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(39, 90);
            label13.Name = "label13";
            label13.Size = new Size(69, 17);
            label13.TabIndex = 15;
            label13.Text = "Line Style: ";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 90);
            label14.Name = "label14";
            label14.Size = new Size(18, 17);
            label14.TabIndex = 14;
            label14.Text = "3.";
            // 
            // colorbutton_2
            // 
            colorbutton_2.BackColor = Color.Blue;
            colorbutton_2.ImeMode = ImeMode.NoControl;
            colorbutton_2.Location = new Point(494, 56);
            colorbutton_2.Name = "colorbutton_2";
            colorbutton_2.Size = new Size(25, 25);
            colorbutton_2.TabIndex = 13;
            colorbutton_2.UseVisualStyleBackColor = false;
            colorbutton_2.Click += colorbutton_2_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(441, 59);
            label7.Name = "label7";
            label7.Size = new Size(47, 17);
            label7.TabIndex = 12;
            label7.Text = "Color: ";
            // 
            // lineWidthComboBox_2
            // 
            lineWidthComboBox_2.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_2.FormattingEnabled = true;
            lineWidthComboBox_2.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_2.Location = new Point(326, 56);
            lineWidthComboBox_2.Name = "lineWidthComboBox_2";
            lineWidthComboBox_2.Size = new Size(86, 25);
            lineWidthComboBox_2.TabIndex = 11;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(271, 59);
            label8.Name = "label8";
            label8.Size = new Size(49, 17);
            label8.TabIndex = 10;
            label8.Text = "Width: ";
            // 
            // dashStyleComboBox_2
            // 
            dashStyleComboBox_2.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_2.FormattingEnabled = true;
            dashStyleComboBox_2.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_2.Location = new Point(120, 56);
            dashStyleComboBox_2.Name = "dashStyleComboBox_2";
            dashStyleComboBox_2.Size = new Size(128, 25);
            dashStyleComboBox_2.TabIndex = 9;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(39, 59);
            label9.Name = "label9";
            label9.Size = new Size(69, 17);
            label9.TabIndex = 8;
            label9.Text = "Line Style: ";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 59);
            label10.Name = "label10";
            label10.Size = new Size(18, 17);
            label10.TabIndex = 7;
            label10.Text = "2.";
            // 
            // colorbutton_1
            // 
            colorbutton_1.BackColor = Color.Blue;
            colorbutton_1.ImeMode = ImeMode.NoControl;
            colorbutton_1.Location = new Point(494, 25);
            colorbutton_1.Name = "colorbutton_1";
            colorbutton_1.Size = new Size(25, 25);
            colorbutton_1.TabIndex = 6;
            colorbutton_1.UseVisualStyleBackColor = false;
            colorbutton_1.Click += colorbutton_1_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(441, 28);
            label6.Name = "label6";
            label6.Size = new Size(47, 17);
            label6.TabIndex = 5;
            label6.Text = "Color: ";
            // 
            // lineWidthComboBox_1
            // 
            lineWidthComboBox_1.DropDownStyle = ComboBoxStyle.DropDownList;
            lineWidthComboBox_1.FormattingEnabled = true;
            lineWidthComboBox_1.Items.AddRange(new object[] { "1px", "2px", "3px", "4px", "5px" });
            lineWidthComboBox_1.Location = new Point(326, 25);
            lineWidthComboBox_1.Name = "lineWidthComboBox_1";
            lineWidthComboBox_1.Size = new Size(86, 25);
            lineWidthComboBox_1.TabIndex = 4;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(271, 28);
            label5.Name = "label5";
            label5.Size = new Size(49, 17);
            label5.TabIndex = 3;
            label5.Text = "Width: ";
            // 
            // dashStyleComboBox_1
            // 
            dashStyleComboBox_1.DropDownStyle = ComboBoxStyle.DropDownList;
            dashStyleComboBox_1.FormattingEnabled = true;
            dashStyleComboBox_1.Items.AddRange(new object[] { "Solid", "Dash", "Dot", "DashDot", "DashDotDot" });
            dashStyleComboBox_1.Location = new Point(120, 25);
            dashStyleComboBox_1.Name = "dashStyleComboBox_1";
            dashStyleComboBox_1.Size = new Size(128, 25);
            dashStyleComboBox_1.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(39, 28);
            label4.Name = "label4";
            label4.Size = new Size(69, 17);
            label4.TabIndex = 1;
            label4.Text = "Line Style: ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 28);
            label2.Name = "label2";
            label2.Size = new Size(18, 17);
            label2.TabIndex = 0;
            label2.Text = "1.";
            // 
            // button_cancel
            // 
            button_cancel.ImeMode = ImeMode.NoControl;
            button_cancel.Location = new Point(682, 506);
            button_cancel.Name = "button_cancel";
            button_cancel.Size = new Size(102, 33);
            button_cancel.TabIndex = 7;
            button_cancel.Text = "Cancel";
            button_cancel.UseVisualStyleBackColor = true;
            button_cancel.Click += button_cancel_Click;
            // 
            // button_apply
            // 
            button_apply.ImeMode = ImeMode.NoControl;
            button_apply.Location = new Point(558, 506);
            button_apply.Name = "button_apply";
            button_apply.Size = new Size(102, 33);
            button_apply.TabIndex = 6;
            button_apply.Text = "Apply";
            button_apply.UseVisualStyleBackColor = true;
            button_apply.Click += button_apply_Click;
            // 
            // button_default
            // 
            button_default.ImeMode = ImeMode.NoControl;
            button_default.Location = new Point(12, 506);
            button_default.Name = "button_default";
            button_default.Size = new Size(102, 33);
            button_default.TabIndex = 8;
            button_default.Text = "Default";
            button_default.UseVisualStyleBackColor = true;
            button_default.Click += button_default_Click;
            // 
            // BookmarkSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(796, 551);
            Controls.Add(button_default);
            Controls.Add(button_cancel);
            Controls.Add(button_apply);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "BookmarkSettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "BookmarkPlus Settings";
            Load += BookmarkSettingsForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private CheckBox autoLoadSaveCheckBox;
        private Button button_saveFolder_reset;
        private Button button_saveFolder_select;
        private TextBox textBox_saveFolder;
        private Label label3;
        private CheckBox loadWithStyleCheckBox;
        private Label label1;
        private GroupBox groupBox2;
        private Label label2;
        private Label label5;
        private ComboBox dashStyleComboBox_1;
        private Label label4;
        private Label label6;
        private ComboBox lineWidthComboBox_1;
        private Button colorbutton_1;
        private Button colorbutton_8;
        private Label label31;
        private ComboBox lineWidthComboBox_8;
        private Label label32;
        private ComboBox dashStyleComboBox_8;
        private Label label33;
        private Label label34;
        private Button colorbutton_7;
        private Label label27;
        private ComboBox lineWidthComboBox_7;
        private Label label28;
        private ComboBox dashStyleComboBox_7;
        private Label label29;
        private Label label30;
        private Button colorbutton_6;
        private Label label23;
        private ComboBox lineWidthComboBox_6;
        private Label label24;
        private ComboBox dashStyleComboBox_6;
        private Label label25;
        private Label label26;
        private Button colorbutton_5;
        private Label label19;
        private ComboBox lineWidthComboBox_5;
        private Label label20;
        private ComboBox dashStyleComboBox_5;
        private Label label21;
        private Label label22;
        private Button colorbutton_4;
        private Label label15;
        private ComboBox lineWidthComboBox_4;
        private Label label16;
        private ComboBox dashStyleComboBox_4;
        private Label label17;
        private Label label18;
        private Button colorbutton_3;
        private Label label11;
        private ComboBox lineWidthComboBox_3;
        private Label label12;
        private ComboBox dashStyleComboBox_3;
        private Label label13;
        private Label label14;
        private Button colorbutton_2;
        private Label label7;
        private ComboBox lineWidthComboBox_2;
        private Label label8;
        private ComboBox dashStyleComboBox_2;
        private Label label9;
        private Label label10;
        private TextBox commentTextBox_8;
        private TextBox commentTextBox_7;
        private TextBox commentTextBox_6;
        private TextBox commentTextBox_5;
        private TextBox commentTextBox_4;
        private TextBox commentTextBox_3;
        private TextBox commentTextBox_2;
        private TextBox commentTextBox_1;
        private Label label42;
        private Label label41;
        private Label label40;
        private Label label39;
        private Label label38;
        private Label label37;
        private Label label36;
        private Label label35;
        private Button button_cancel;
        private Button button_apply;
        private Button button_default;
        private Button button_loadStyle;
        private Button button_saveStyle;
    }
}