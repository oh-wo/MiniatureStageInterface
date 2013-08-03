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
            this.textOutput = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textScale = new System.Windows.Forms.TextBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.labelDrawingScale = new System.Windows.Forms.Label();
            this.labelLineSpacing = new System.Windows.Forms.Label();
            this.textLineSpacing = new System.Windows.Forms.TextBox();
            this.labelUnits = new System.Windows.Forms.Label();
            this.checkDisplayOrigin = new System.Windows.Forms.CheckBox();
            this.checkStageBounds = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textOutput
            // 
            this.textOutput.Location = new System.Drawing.Point(72, 59);
            this.textOutput.Multiline = true;
            this.textOutput.Name = "textOutput";
            this.textOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textOutput.Size = new System.Drawing.Size(425, 520);
            this.textOutput.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(518, 59);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(517, 520);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseWheel);
            // 
            // textScale
            // 
            this.textScale.Location = new System.Drawing.Point(593, 29);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(44, 20);
            this.textScale.TabIndex = 2;
            this.textScale.Text = "200";
            this.textScale.LostFocus += new System.EventHandler(this.textScale_LostFocus);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(1038, 59);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(15, 520);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_valueChanged);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(518, 582);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(517, 11);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_valueChanged);
            // 
            // labelDrawingScale
            // 
            this.labelDrawingScale.AutoSize = true;
            this.labelDrawingScale.Location = new System.Drawing.Point(515, 32);
            this.labelDrawingScale.Name = "labelDrawingScale";
            this.labelDrawingScale.Size = new System.Drawing.Size(72, 13);
            this.labelDrawingScale.TabIndex = 5;
            this.labelDrawingScale.Text = "drawing scale";
            // 
            // labelLineSpacing
            // 
            this.labelLineSpacing.AutoSize = true;
            this.labelLineSpacing.Location = new System.Drawing.Point(643, 32);
            this.labelLineSpacing.Name = "labelLineSpacing";
            this.labelLineSpacing.Size = new System.Drawing.Size(63, 13);
            this.labelLineSpacing.TabIndex = 6;
            this.labelLineSpacing.Text = "line spacing";
            // 
            // textLineSpacing
            // 
            this.textLineSpacing.Location = new System.Drawing.Point(712, 29);
            this.textLineSpacing.Name = "textLineSpacing";
            this.textLineSpacing.Size = new System.Drawing.Size(61, 20);
            this.textLineSpacing.TabIndex = 7;
            this.textLineSpacing.LostFocus += new System.EventHandler(this.textLineSpacing_LostFocus);
            // 
            // labelUnits
            // 
            this.labelUnits.AutoSize = true;
            this.labelUnits.Location = new System.Drawing.Point(793, 32);
            this.labelUnits.Name = "labelUnits";
            this.labelUnits.Size = new System.Drawing.Size(74, 13);
            this.labelUnits.TabIndex = 8;
            this.labelUnits.Text = "units detected";
            // 
            // checkDisplayOrigin
            // 
            this.checkDisplayOrigin.AutoSize = true;
            this.checkDisplayOrigin.Checked = true;
            this.checkDisplayOrigin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDisplayOrigin.Location = new System.Drawing.Point(518, 597);
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
            this.checkStageBounds.Location = new System.Drawing.Point(575, 597);
            this.checkStageBounds.Name = "checkStageBounds";
            this.checkStageBounds.Size = new System.Drawing.Size(90, 17);
            this.checkStageBounds.TabIndex = 9;
            this.checkStageBounds.Text = "stage bounds";
            this.checkStageBounds.UseVisualStyleBackColor = true;
            this.checkStageBounds.CheckedChanged += new System.EventHandler(this.checkStageBounds_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 650);
            this.Controls.Add(this.checkStageBounds);
            this.Controls.Add(this.checkDisplayOrigin);
            this.Controls.Add(this.labelUnits);
            this.Controls.Add(this.textLineSpacing);
            this.Controls.Add(this.labelLineSpacing);
            this.Controls.Add(this.labelDrawingScale);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.textScale);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textOutput);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion

        private System.Windows.Forms.TextBox textOutput;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textScale;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label labelDrawingScale;
        private System.Windows.Forms.Label labelLineSpacing;
        private System.Windows.Forms.TextBox textLineSpacing;
        private System.Windows.Forms.Label labelUnits;
        private System.Windows.Forms.CheckBox checkDisplayOrigin;
        private System.Windows.Forms.CheckBox checkStageBounds;

    }
}

