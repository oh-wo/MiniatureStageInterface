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
            this.components = new System.ComponentModel.Container();
            this.tabFsOnChip = new MetroFramework.Controls.MetroTabPage();
            this.tabDielectricMach = new MetroFramework.Controls.MetroTabPage();
            this.labelDrawingScale = new System.Windows.Forms.Label();
            this.checkStageBounds = new System.Windows.Forms.CheckBox();
            this.checkDisplayOrigin = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelUnits = new System.Windows.Forms.Label();
            this.textScale = new System.Windows.Forms.TextBox();
            this.textLineSpacing = new System.Windows.Forms.TextBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.labelLineSpacing = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.tabSerialConnect = new MetroFramework.Controls.MetroTabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.comboSerialPorts = new System.Windows.Forms.ComboBox();
            this.toggleSerialConnect = new MetroFramework.Controls.MetroToggle();
            this.tabErrythang = new MetroFramework.Controls.MetroTabControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabDielectricMach.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabSerialConnect.SuspendLayout();
            this.tabErrythang.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabFsOnChip
            // 
            this.tabFsOnChip.CustomBackground = false;
            this.tabFsOnChip.HorizontalScrollbar = false;
            this.tabFsOnChip.HorizontalScrollbarBarColor = true;
            this.tabFsOnChip.HorizontalScrollbarHighlightOnWheel = false;
            this.tabFsOnChip.HorizontalScrollbarSize = 10;
            this.tabFsOnChip.Location = new System.Drawing.Point(4, 35);
            this.tabFsOnChip.Name = "tabFsOnChip";
            this.tabFsOnChip.Size = new System.Drawing.Size(828, 611);
            this.tabFsOnChip.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabFsOnChip.StyleManager = null;
            this.tabFsOnChip.TabIndex = 2;
            this.tabFsOnChip.Text = "Fs On Chip";
            this.tabFsOnChip.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabFsOnChip.VerticalScrollbar = false;
            this.tabFsOnChip.VerticalScrollbarBarColor = true;
            this.tabFsOnChip.VerticalScrollbarHighlightOnWheel = false;
            this.tabFsOnChip.VerticalScrollbarSize = 10;
            // 
            // tabDielectricMach
            // 
            this.tabDielectricMach.Controls.Add(this.textBox1);
            this.tabDielectricMach.Controls.Add(this.labelDrawingScale);
            this.tabDielectricMach.Controls.Add(this.checkStageBounds);
            this.tabDielectricMach.Controls.Add(this.checkDisplayOrigin);
            this.tabDielectricMach.Controls.Add(this.pictureBox1);
            this.tabDielectricMach.Controls.Add(this.labelUnits);
            this.tabDielectricMach.Controls.Add(this.textScale);
            this.tabDielectricMach.Controls.Add(this.textLineSpacing);
            this.tabDielectricMach.Controls.Add(this.vScrollBar1);
            this.tabDielectricMach.Controls.Add(this.labelLineSpacing);
            this.tabDielectricMach.Controls.Add(this.hScrollBar1);
            this.tabDielectricMach.CustomBackground = false;
            this.tabDielectricMach.HorizontalScrollbar = false;
            this.tabDielectricMach.HorizontalScrollbarBarColor = true;
            this.tabDielectricMach.HorizontalScrollbarHighlightOnWheel = false;
            this.tabDielectricMach.HorizontalScrollbarSize = 10;
            this.tabDielectricMach.Location = new System.Drawing.Point(4, 35);
            this.tabDielectricMach.Name = "tabDielectricMach";
            this.tabDielectricMach.Padding = new System.Windows.Forms.Padding(3);
            this.tabDielectricMach.Size = new System.Drawing.Size(997, 611);
            this.tabDielectricMach.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabDielectricMach.StyleManager = null;
            this.tabDielectricMach.TabIndex = 1;
            this.tabDielectricMach.Text = "Dielectric Machining";
            this.tabDielectricMach.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabDielectricMach.UseVisualStyleBackColor = true;
            this.tabDielectricMach.VerticalScrollbar = false;
            this.tabDielectricMach.VerticalScrollbarBarColor = true;
            this.tabDielectricMach.VerticalScrollbarHighlightOnWheel = false;
            this.tabDielectricMach.VerticalScrollbarSize = 10;
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
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(11, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(705, 520);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
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
            // textScale
            // 
            this.textScale.Location = new System.Drawing.Point(457, 14);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(44, 20);
            this.textScale.TabIndex = 2;
            this.textScale.Text = "1";
            // 
            // textLineSpacing
            // 
            this.textLineSpacing.Location = new System.Drawing.Point(576, 14);
            this.textLineSpacing.Name = "textLineSpacing";
            this.textLineSpacing.Size = new System.Drawing.Size(61, 20);
            this.textLineSpacing.TabIndex = 7;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(719, 44);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(15, 520);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.Value = 50;
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
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(11, 567);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(707, 15);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Value = 50;
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
            this.tabSerialConnect.Size = new System.Drawing.Size(828, 611);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Choose a serial port from the list below and then change the toggle to \"On\"";
            // 
            // comboSerialPorts
            // 
            this.comboSerialPorts.FormattingEnabled = true;
            this.comboSerialPorts.Location = new System.Drawing.Point(3, 42);
            this.comboSerialPorts.Name = "comboSerialPorts";
            this.comboSerialPorts.Size = new System.Drawing.Size(361, 21);
            this.comboSerialPorts.TabIndex = 6;
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
            // 
            // tabErrythang
            // 
            this.tabErrythang.Controls.Add(this.tabSerialConnect);
            this.tabErrythang.Controls.Add(this.tabDielectricMach);
            this.tabErrythang.Controls.Add(this.tabFsOnChip);
            this.tabErrythang.CustomBackground = false;
            this.tabErrythang.FontSize = MetroFramework.MetroTabControlSize.Medium;
            this.tabErrythang.FontWeight = MetroFramework.MetroTabControlWeight.Light;
            this.tabErrythang.Location = new System.Drawing.Point(9, 63);
            this.tabErrythang.Name = "tabErrythang";
            this.tabErrythang.SelectedIndex = 1;
            this.tabErrythang.Size = new System.Drawing.Size(1005, 650);
            this.tabErrythang.Style = MetroFramework.MetroColorStyle.Blue;
            this.tabErrythang.StyleManager = null;
            this.tabErrythang.TabIndex = 10;
            this.tabErrythang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tabErrythang.Theme = MetroFramework.MetroThemeStyle.Light;
            this.tabErrythang.UseStyleColors = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(746, 44);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(210, 538);
            this.textBox1.TabIndex = 10;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 713);
            this.Controls.Add(this.tabErrythang);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form1";
            this.Text = "Laser Micromachining Control";
            this.tabDielectricMach.ResumeLayout(false);
            this.tabDielectricMach.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabSerialConnect.ResumeLayout(false);
            this.tabSerialConnect.PerformLayout();
            this.tabErrythang.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        
        #endregion

        private MetroFramework.Controls.MetroTabPage tabFsOnChip;
        private MetroFramework.Controls.MetroTabPage tabDielectricMach;
        private System.Windows.Forms.Label labelDrawingScale;
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;


    }
}

