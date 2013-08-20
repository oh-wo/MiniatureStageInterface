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
            this.tabImportDxf = new MetroFramework.Controls.MetroTabPage();
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
            this.tabImportDxf.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabSerialConnect.SuspendLayout();
            this.tabErrythang.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabManualControl
            // 
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
            // tabImportDxf
            // 
            this.tabImportDxf.Controls.Add(this.labelDrawingScale);
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
            this.textScale.Text = "0.188075436651707";
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
            this.tabSerialConnect.Size = new System.Drawing.Size(838, 561);
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
            this.tabErrythang.Controls.Add(this.tabImportDxf);
            this.tabErrythang.Controls.Add(this.tabManualControl);
            this.tabErrythang.CustomBackground = false;
            this.tabErrythang.FontSize = MetroFramework.MetroTabControlSize.Medium;
            this.tabErrythang.FontWeight = MetroFramework.MetroTabControlWeight.Light;
            this.tabErrythang.Location = new System.Drawing.Point(9, 63);
            this.tabErrythang.Name = "tabErrythang";
            this.tabErrythang.SelectedIndex = 0;
            this.tabErrythang.Size = new System.Drawing.Size(846, 600);
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
            this.Text = "Laser Micromachining Control";
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
        private MetroFramework.Controls.MetroTabPage tabImportDxf;
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


    }
}

