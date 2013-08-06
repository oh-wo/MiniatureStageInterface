namespace dxfTest
{
    partial class Form1
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
            this.tabManualControl = new MetroFramework.Controls.MetroTabPage();
            this.textIncrement = new MetroFramework.Controls.MetroTextBox();
            this.but0YAxis = new MetroFramework.Controls.MetroButton();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.but0Xaxis = new MetroFramework.Controls.MetroButton();
            this.butFrf = new MetroFramework.Controls.MetroButton();
            this.tabImportDxf = new MetroFramework.Controls.MetroTabPage();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.labelLineSpacing = new System.Windows.Forms.Label();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.textLineSpacing = new System.Windows.Forms.TextBox();
            this.textScale = new System.Windows.Forms.TextBox();
            this.labelUnits = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkDisplayOrigin = new System.Windows.Forms.CheckBox();
            this.checkStageBounds = new System.Windows.Forms.CheckBox();
            this.butXMinus = new MetroFramework.Controls.MetroButton();
            this.labelDrawingScale = new System.Windows.Forms.Label();
            this.butXPlus = new MetroFramework.Controls.MetroButton();
            this.butYMinus = new MetroFramework.Controls.MetroButton();
            this.butYPlus = new MetroFramework.Controls.MetroButton();
            this.labelLaserPos = new System.Windows.Forms.Label();
            this.tabSerialConnect = new MetroFramework.Controls.MetroTabPage();
            this.toggleSerialConnect = new MetroFramework.Controls.MetroToggle();
            this.comboSerialPorts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabErrythang = new MetroFramework.Controls.MetroTabControl();
            this.tabManualControl.SuspendLayout();
            this.tabImportDxf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabSerialConnect.SuspendLayout();
            this.tabErrythang.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabManualControl
            // 
            this.tabManualControl.Controls.Add(this.butFrf);
            this.tabManualControl.Controls.Add(this.but0Xaxis);
            this.tabManualControl.Controls.Add(this.metroLabel1);
            this.tabManualControl.Controls.Add(this.but0YAxis);
            this.tabManualControl.Controls.Add(this.textIncrement);
            this.tabManualControl.CustomBackground = false;
            this.tabManualControl.HorizontalScrollbar = false;
            this.tabManualControl.HorizontalScrollbarBarColor = true;
            this.tabManualControl.HorizontalScrollbarHighlightOnWheel = false;
            this.tabManualControl.HorizontalScrollbarSize = 10;
            this.tabManualControl.Location = new System.Drawing.Point(4, 35);
            this.tabManualControl.Name = "tabManualControl";
            this.tabManualControl.Size = new System.Drawing.Size(838, 615);
            this.tabManualControl.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabManualControl.StyleManager = null;
            this.tabManualControl.TabIndex = 2;
            this.tabManualControl.Text = "Manual Control";
            this.tabManualControl.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabManualControl.VerticalScrollbar = false;
            this.tabManualControl.VerticalScrollbarBarColor = true;
            this.tabManualControl.VerticalScrollbarHighlightOnWheel = false;
            this.tabManualControl.VerticalScrollbarSize = 10;
            // 
            // textIncrement
            // 
            this.textIncrement.CustomBackground = false;
            this.textIncrement.CustomForeColor = false;
            this.textIncrement.FontSize = MetroFramework.MetroTextBoxSize.Small;
            this.textIncrement.FontWeight = MetroFramework.MetroTextBoxWeight.Regular;
            this.textIncrement.Location = new System.Drawing.Point(144, 104);
            this.textIncrement.Multiline = false;
            this.textIncrement.Name = "textIncrement";
            this.textIncrement.SelectedText = "";
            this.textIncrement.Size = new System.Drawing.Size(75, 20);
            this.textIncrement.Style = MetroFramework.MetroColorStyle.Blue;
            this.textIncrement.StyleManager = null;
            this.textIncrement.TabIndex = 41;
            this.textIncrement.Theme = MetroFramework.MetroThemeStyle.Light;
            this.textIncrement.UseStyleColors = false;
            // 
            // but0YAxis
            // 
            this.but0YAxis.Highlight = false;
            this.but0YAxis.Location = new System.Drawing.Point(144, 75);
            this.but0YAxis.Name = "but0YAxis";
            this.but0YAxis.Size = new System.Drawing.Size(75, 23);
            this.but0YAxis.Style = MetroFramework.MetroColorStyle.Blue;
            this.but0YAxis.StyleManager = null;
            this.but0YAxis.TabIndex = 40;
            this.but0YAxis.Text = "Zero Y Axis";
            this.but0YAxis.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.CustomBackground = false;
            this.metroLabel1.CustomForeColor = false;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Medium;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Light;
            this.metroLabel1.LabelMode = MetroFramework.Controls.MetroLabelMode.Default;
            this.metroLabel1.Location = new System.Drawing.Point(8, 104);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(134, 19);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel1.StyleManager = null;
            this.metroLabel1.TabIndex = 42;
            this.metroLabel1.Text = "movement increment";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroLabel1.UseStyleColors = false;
            // 
            // but0Xaxis
            // 
            this.but0Xaxis.Highlight = false;
            this.but0Xaxis.Location = new System.Drawing.Point(144, 46);
            this.but0Xaxis.Name = "but0Xaxis";
            this.but0Xaxis.Size = new System.Drawing.Size(75, 23);
            this.but0Xaxis.Style = MetroFramework.MetroColorStyle.Blue;
            this.but0Xaxis.StyleManager = null;
            this.but0Xaxis.TabIndex = 35;
            this.but0Xaxis.Text = "Zero X Axis";
            this.but0Xaxis.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // butFrf
            // 
            this.butFrf.Highlight = false;
            this.butFrf.Location = new System.Drawing.Point(144, 17);
            this.butFrf.Name = "butFrf";
            this.butFrf.Size = new System.Drawing.Size(75, 23);
            this.butFrf.Style = MetroFramework.MetroColorStyle.Blue;
            this.butFrf.StyleManager = null;
            this.butFrf.TabIndex = 34;
            this.butFrf.Text = "frf";
            this.butFrf.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // tabImportDxf
            // 
            this.tabImportDxf.Controls.Add(this.labelLaserPos);
            this.tabImportDxf.Controls.Add(this.butYPlus);
            this.tabImportDxf.Controls.Add(this.butYMinus);
            this.tabImportDxf.Controls.Add(this.butXPlus);
            this.tabImportDxf.Controls.Add(this.labelDrawingScale);
            this.tabImportDxf.Controls.Add(this.butXMinus);
            this.tabImportDxf.Controls.Add(this.checkStageBounds);
            this.tabImportDxf.Controls.Add(this.checkDisplayOrigin);
            this.tabImportDxf.Controls.Add(this.pictureBox1);
            this.tabImportDxf.Controls.Add(this.labelUnits);
            this.tabImportDxf.Controls.Add(this.textScale);
            this.tabImportDxf.Controls.Add(this.textLineSpacing);
            this.tabImportDxf.Controls.Add(this.vScrollBar1);
            this.tabImportDxf.Controls.Add(this.labelLineSpacing);
            this.tabImportDxf.Controls.Add(this.hScrollBar1);
            this.tabImportDxf.CustomBackground = false;
            this.tabImportDxf.HorizontalScrollbar = false;
            this.tabImportDxf.HorizontalScrollbarBarColor = true;
            this.tabImportDxf.HorizontalScrollbarHighlightOnWheel = false;
            this.tabImportDxf.HorizontalScrollbarSize = 10;
            this.tabImportDxf.Location = new System.Drawing.Point(4, 35);
            this.tabImportDxf.Name = "tabImportDxf";
            this.tabImportDxf.Padding = new System.Windows.Forms.Padding(3);
            this.tabImportDxf.Size = new System.Drawing.Size(838, 615);
            this.tabImportDxf.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabImportDxf.StyleManager = null;
            this.tabImportDxf.TabIndex = 1;
            this.tabImportDxf.Text = "Import Dxf";
            this.tabImportDxf.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabImportDxf.UseVisualStyleBackColor = true;
            this.tabImportDxf.VerticalScrollbar = false;
            this.tabImportDxf.VerticalScrollbarBarColor = true;
            this.tabImportDxf.VerticalScrollbarHighlightOnWheel = false;
            this.tabImportDxf.VerticalScrollbarSize = 10;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(11, 567);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(707, 15);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_valueChanged);
            // 
            // labelLineSpacing
            // 
            this.labelLineSpacing.AutoSize = true;
            this.labelLineSpacing.Location = new System.Drawing.Point(507, 17);
            this.labelLineSpacing.Name = "labelLineSpacing";
            this.labelLineSpacing.Size = new System.Drawing.Size(63, 13);
            this.labelLineSpacing.TabIndex = 6;
            this.labelLineSpacing.Text = "line spacing";
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(719, 44);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(15, 520);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_valueChanged);
            // 
            // textLineSpacing
            // 
            this.textLineSpacing.Location = new System.Drawing.Point(576, 14);
            this.textLineSpacing.Name = "textLineSpacing";
            this.textLineSpacing.Size = new System.Drawing.Size(61, 20);
            this.textLineSpacing.TabIndex = 7;
            this.textLineSpacing.LostFocus += new System.EventHandler(this.textLineSpacing_LostFocus);
            // 
            // textScale
            // 
            this.textScale.Location = new System.Drawing.Point(457, 14);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(44, 20);
            this.textScale.TabIndex = 2;
            this.textScale.Text = "0.188075436651707";
            this.textScale.LostFocus += new System.EventHandler(this.textScale_LostFocus);
            // 
            // labelUnits
            // 
            this.labelUnits.AutoSize = true;
            this.labelUnits.Location = new System.Drawing.Point(643, 17);
            this.labelUnits.Name = "labelUnits";
            this.labelUnits.Size = new System.Drawing.Size(74, 13);
            this.labelUnits.TabIndex = 8;
            this.labelUnits.Text = "units detected";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(11, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(705, 520);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // checkDisplayOrigin
            // 
            this.checkDisplayOrigin.AutoSize = true;
            this.checkDisplayOrigin.Checked = true;
            this.checkDisplayOrigin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDisplayOrigin.Location = new System.Drawing.Point(14, 588);
            this.checkDisplayOrigin.Name = "checkDisplayOrigin";
            this.checkDisplayOrigin.Size = new System.Drawing.Size(51, 17);
            this.checkDisplayOrigin.TabIndex = 9;
            this.checkDisplayOrigin.Text = "origin";
            this.checkDisplayOrigin.UseVisualStyleBackColor = true;
            this.checkDisplayOrigin.CheckedChanged += new System.EventHandler(this.checkDisplayOrigin_CheckedChanged);
            // 
            // checkStageBounds
            // 
            this.checkStageBounds.AutoSize = true;
            this.checkStageBounds.Checked = true;
            this.checkStageBounds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkStageBounds.Location = new System.Drawing.Point(71, 588);
            this.checkStageBounds.Name = "checkStageBounds";
            this.checkStageBounds.Size = new System.Drawing.Size(90, 17);
            this.checkStageBounds.TabIndex = 9;
            this.checkStageBounds.Text = "stage bounds";
            this.checkStageBounds.UseVisualStyleBackColor = true;
            this.checkStageBounds.CheckedChanged += new System.EventHandler(this.checkStageBounds_CheckedChanged);
            // 
            // metroButton3
            // 
            this.butXMinus.Highlight = false;
            this.butXMinus.Location = new System.Drawing.Point(754, 66);
            this.butXMinus.Name = "metroButton3";
            this.butXMinus.Size = new System.Drawing.Size(23, 23);
            this.butXMinus.Style = MetroFramework.MetroColorStyle.Blue;
            this.butXMinus.StyleManager = null;
            this.butXMinus.TabIndex = 41;
            this.butXMinus.Text = "←";
            this.butXMinus.Theme = MetroFramework.MetroThemeStyle.Light;
            this.butXMinus.Click += new System.EventHandler(this.butXMinus_Click);
            // 
            // labelDrawingScale
            // 
            this.labelDrawingScale.AutoSize = true;
            this.labelDrawingScale.Location = new System.Drawing.Point(379, 17);
            this.labelDrawingScale.Name = "labelDrawingScale";
            this.labelDrawingScale.Size = new System.Drawing.Size(72, 13);
            this.labelDrawingScale.TabIndex = 5;
            this.labelDrawingScale.Text = "drawing scale";
            // 
            // metroButton4
            // 
            this.butXPlus.Highlight = false;
            this.butXPlus.Location = new System.Drawing.Point(804, 68);
            this.butXPlus.Name = "metroButton4";
            this.butXPlus.Size = new System.Drawing.Size(25, 21);
            this.butXPlus.Style = MetroFramework.MetroColorStyle.Blue;
            this.butXPlus.StyleManager = null;
            this.butXPlus.TabIndex = 40;
            this.butXPlus.Text = "→";
            this.butXPlus.Theme = MetroFramework.MetroThemeStyle.Light;
            this.butXPlus.Click += new System.EventHandler(this.butXPlus_Click);
            // 
            // metroButton1
            // 
            this.butYMinus.Highlight = false;
            this.butYMinus.Location = new System.Drawing.Point(779, 89);
            this.butYMinus.Name = "metroButton1";
            this.butYMinus.Size = new System.Drawing.Size(23, 22);
            this.butYMinus.Style = MetroFramework.MetroColorStyle.Blue;
            this.butYMinus.StyleManager = null;
            this.butYMinus.TabIndex = 42;
            this.butYMinus.Text = "↓";
            this.butYMinus.Theme = MetroFramework.MetroThemeStyle.Light;
            this.butYMinus.Click += new System.EventHandler(this.butYMinus_Click);
            // 
            // metroButton2
            // 
            this.butYPlus.Highlight = false;
            this.butYPlus.Location = new System.Drawing.Point(779, 44);
            this.butYPlus.Name = "metroButton2";
            this.butYPlus.Size = new System.Drawing.Size(23, 22);
            this.butYPlus.Style = MetroFramework.MetroColorStyle.Blue;
            this.butYPlus.StyleManager = null;
            this.butYPlus.TabIndex = 43;
            this.butYPlus.Text = "↑";
            this.butYPlus.Theme = MetroFramework.MetroThemeStyle.Light;
            this.butYPlus.Click += new System.EventHandler(this.butYPlus_Click);
            // 
            // labelLaserPos
            // 
            this.labelLaserPos.AutoSize = true;
            this.labelLaserPos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            this.labelLaserPos.ForeColor = System.Drawing.Color.White;
            this.labelLaserPos.Location = new System.Drawing.Point(585, 541);
            this.labelLaserPos.Name = "labelLaserPos";
            this.labelLaserPos.Size = new System.Drawing.Size(36, 13);
            this.labelLaserPos.TabIndex = 44;
            this.labelLaserPos.Text = "pos: ()";
            // 
            // tabSerialConnect
            // 
            this.tabSerialConnect.Controls.Add(this.label1);
            this.tabSerialConnect.Controls.Add(this.comboSerialPorts);
            this.tabSerialConnect.Controls.Add(this.toggleSerialConnect);
            this.tabSerialConnect.CustomBackground = false;
            this.tabSerialConnect.HorizontalScrollbar = false;
            this.tabSerialConnect.HorizontalScrollbarBarColor = true;
            this.tabSerialConnect.HorizontalScrollbarHighlightOnWheel = false;
            this.tabSerialConnect.HorizontalScrollbarSize = 10;
            this.tabSerialConnect.Location = new System.Drawing.Point(4, 35);
            this.tabSerialConnect.Name = "tabSerialConnect";
            this.tabSerialConnect.Padding = new System.Windows.Forms.Padding(3);
            this.tabSerialConnect.Size = new System.Drawing.Size(838, 615);
            this.tabSerialConnect.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabSerialConnect.StyleManager = null;
            this.tabSerialConnect.TabIndex = 0;
            this.tabSerialConnect.Text = "Serial";
            this.tabSerialConnect.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabSerialConnect.UseVisualStyleBackColor = true;
            this.tabSerialConnect.VerticalScrollbar = false;
            this.tabSerialConnect.VerticalScrollbarBarColor = true;
            this.tabSerialConnect.VerticalScrollbarHighlightOnWheel = false;
            this.tabSerialConnect.VerticalScrollbarSize = 10;
            // 
            // toggleSerialConnect
            // 
            this.toggleSerialConnect.CustomBackground = false;
            this.toggleSerialConnect.DisplayStatus = true;
            this.toggleSerialConnect.FontSize = MetroFramework.MetroLinkSize.Small;
            this.toggleSerialConnect.FontWeight = MetroFramework.MetroLinkWeight.Regular;
            this.toggleSerialConnect.Location = new System.Drawing.Point(3, 79);
            this.toggleSerialConnect.Name = "toggleSerialConnect";
            this.toggleSerialConnect.Size = new System.Drawing.Size(54, 22);
            this.toggleSerialConnect.Style = MetroFramework.MetroColorStyle.Blue;
            this.toggleSerialConnect.StyleManager = null;
            this.toggleSerialConnect.TabIndex = 7;
            this.toggleSerialConnect.Text = "Off";
            this.toggleSerialConnect.Theme = MetroFramework.MetroThemeStyle.Light;
            this.toggleSerialConnect.UseStyleColors = false;
            this.toggleSerialConnect.CheckStateChanged += new System.EventHandler(this.toggleSerialConnect_Changed);
            // 
            // comboSerialPorts
            // 
            this.comboSerialPorts.FormattingEnabled = true;
            this.comboSerialPorts.Location = new System.Drawing.Point(3, 42);
            this.comboSerialPorts.Name = "comboSerialPorts";
            this.comboSerialPorts.Size = new System.Drawing.Size(361, 21);
            this.comboSerialPorts.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose a serial port from the list below and then change the toggle to \"On\"";
            // 
            // tabErrythang
            // 
            this.tabErrythang.Controls.Add(this.tabSerialConnect);
            this.tabErrythang.Controls.Add(this.tabImportDxf);
            this.tabErrythang.Controls.Add(this.tabManualControl);
            this.tabErrythang.CustomBackground = false;
            this.tabErrythang.FontSize = MetroFramework.MetroTabControlSize.Medium;
            this.tabErrythang.FontWeight = MetroFramework.MetroTabControlWeight.Light;
            this.tabErrythang.Location = new System.Drawing.Point(8, 49);
            this.tabErrythang.Name = "tabErrythang";
            this.tabErrythang.SelectedIndex = 2;
            this.tabErrythang.Size = new System.Drawing.Size(846, 654);
            this.tabErrythang.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabErrythang.StyleManager = null;
            this.tabErrythang.TabIndex = 10;
            this.tabErrythang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tabErrythang.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabErrythang.UseStyleColors = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 713);
            this.Controls.Add(this.tabErrythang);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabManualControl.ResumeLayout(false);
            this.tabManualControl.PerformLayout();
            this.tabImportDxf.ResumeLayout(false);
            this.tabImportDxf.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabSerialConnect.ResumeLayout(false);
            this.tabSerialConnect.PerformLayout();
            this.tabErrythang.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        private MetroFramework.Controls.MetroTabPage tabManualControl;
        private MetroFramework.Controls.MetroButton butFrf;
        private MetroFramework.Controls.MetroButton but0Xaxis;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton but0YAxis;
        private MetroFramework.Controls.MetroTextBox textIncrement;
        private MetroFramework.Controls.MetroTabPage tabImportDxf;
        private System.Windows.Forms.Label labelLaserPos;
        private MetroFramework.Controls.MetroButton butYPlus;
        private MetroFramework.Controls.MetroButton butYMinus;
        private MetroFramework.Controls.MetroButton butXPlus;
        private System.Windows.Forms.Label labelDrawingScale;
        private MetroFramework.Controls.MetroButton butXMinus;
        private System.Windows.Forms.CheckBox checkStageBounds;
        private System.Windows.Forms.CheckBox checkDisplayOrigin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelUnits;
        private System.Windows.Forms.TextBox textScale;
        private System.Windows.Forms.TextBox textLineSpacing;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Label labelLineSpacing;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private MetroFramework.Controls.MetroTabPage tabSerialConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboSerialPorts;
        private MetroFramework.Controls.MetroToggle toggleSerialConnect;
        private MetroFramework.Controls.MetroTabControl tabErrythang;


    }
}

