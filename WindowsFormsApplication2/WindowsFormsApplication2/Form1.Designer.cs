namespace WindowsFormsApplication2
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.partyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.labelCurrentXPos = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.listBoxCommands = new System.Windows.Forms.ListBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.groupBoxMoveProperties = new System.Windows.Forms.GroupBox();
            this.textBoxMoveProp = new System.Windows.Forms.TextBox();
            this.groupBoxLoopProperties = new System.Windows.Forms.GroupBox();
            this.textBoxLoopN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.delayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxDelayProperties = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.groupBoxMoveProperties.SuspendLayout();
            this.groupBoxLoopProperties.SuspendLayout();
            this.groupBoxDelayProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addToolStripMenuItem,
            this.partyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(782, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loopToolStripMenuItem,
            this.moveToolStripMenuItem,
            this.delayToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // loopToolStripMenuItem
            // 
            this.loopToolStripMenuItem.Name = "loopToolStripMenuItem";
            this.loopToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loopToolStripMenuItem.Text = "Loop";
            this.loopToolStripMenuItem.Click += new System.EventHandler(this.loopToolStripMenuItem_Click);
            // 
            // moveToolStripMenuItem
            // 
            this.moveToolStripMenuItem.Name = "moveToolStripMenuItem";
            this.moveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.moveToolStripMenuItem.Text = "Move";
            this.moveToolStripMenuItem.Click += new System.EventHandler(this.moveToolStripMenuItem_Click);
            // 
            // partyToolStripMenuItem
            // 
            this.partyToolStripMenuItem.Name = "partyToolStripMenuItem";
            this.partyToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.partyToolStripMenuItem.Text = "Party";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(345, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "^";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelCurrentXPos
            // 
            this.labelCurrentXPos.AutoSize = true;
            this.labelCurrentXPos.Location = new System.Drawing.Point(533, 78);
            this.labelCurrentXPos.Name = "labelCurrentXPos";
            this.labelCurrentXPos.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentXPos.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(345, 107);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = ",";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBoxCommands
            // 
            this.listBoxCommands.FormattingEnabled = true;
            this.listBoxCommands.Location = new System.Drawing.Point(59, 62);
            this.listBoxCommands.Name = "listBoxCommands";
            this.listBoxCommands.Size = new System.Drawing.Size(192, 160);
            this.listBoxCommands.TabIndex = 4;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(176, 228);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 5;
            this.buttonExecute.Text = "Execute";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.button3_Click);
            // 
            // groupBoxMoveProperties
            // 
            this.groupBoxMoveProperties.Controls.Add(this.label1);
            this.groupBoxMoveProperties.Controls.Add(this.textBoxMoveProp);
            this.groupBoxMoveProperties.Location = new System.Drawing.Point(306, 161);
            this.groupBoxMoveProperties.Name = "groupBoxMoveProperties";
            this.groupBoxMoveProperties.Size = new System.Drawing.Size(227, 136);
            this.groupBoxMoveProperties.TabIndex = 6;
            this.groupBoxMoveProperties.TabStop = false;
            this.groupBoxMoveProperties.Text = "Move properties";
            this.groupBoxMoveProperties.Visible = false;
            // 
            // textBoxMoveProp
            // 
            this.textBoxMoveProp.Location = new System.Drawing.Point(121, 19);
            this.textBoxMoveProp.Name = "textBoxMoveProp";
            this.textBoxMoveProp.Size = new System.Drawing.Size(100, 20);
            this.textBoxMoveProp.TabIndex = 0;
            // 
            // groupBoxLoopProperties
            // 
            this.groupBoxLoopProperties.Controls.Add(this.label2);
            this.groupBoxLoopProperties.Controls.Add(this.textBoxLoopN);
            this.groupBoxLoopProperties.Location = new System.Drawing.Point(399, 136);
            this.groupBoxLoopProperties.Name = "groupBoxLoopProperties";
            this.groupBoxLoopProperties.Size = new System.Drawing.Size(227, 136);
            this.groupBoxLoopProperties.TabIndex = 7;
            this.groupBoxLoopProperties.TabStop = false;
            this.groupBoxLoopProperties.Text = "Loop Properties";
            this.groupBoxLoopProperties.Visible = false;
            // 
            // textBoxLoopN
            // 
            this.textBoxLoopN.Location = new System.Drawing.Point(100, 19);
            this.textBoxLoopN.Name = "textBoxLoopN";
            this.textBoxLoopN.Size = new System.Drawing.Size(54, 20);
            this.textBoxLoopN.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "X Increment by:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Loops:";
            // 
            // delayToolStripMenuItem
            // 
            this.delayToolStripMenuItem.Name = "delayToolStripMenuItem";
            this.delayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.delayToolStripMenuItem.Text = "Delay";
            this.delayToolStripMenuItem.Click += new System.EventHandler(this.delayToolStripMenuItem_Click);
            // 
            // groupBoxDelayProperties
            // 
            this.groupBoxDelayProperties.Controls.Add(this.textBoxDelay);
            this.groupBoxDelayProperties.Controls.Add(this.label3);
            this.groupBoxDelayProperties.Location = new System.Drawing.Point(547, 95);
            this.groupBoxDelayProperties.Name = "groupBoxDelayProperties";
            this.groupBoxDelayProperties.Size = new System.Drawing.Size(200, 100);
            this.groupBoxDelayProperties.TabIndex = 7;
            this.groupBoxDelayProperties.TabStop = false;
            this.groupBoxDelayProperties.Text = "Delay Properties";
            this.groupBoxDelayProperties.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Delay (ms):";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(73, 38);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(100, 20);
            this.textBoxDelay.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 348);
            this.Controls.Add(this.groupBoxLoopProperties);
            this.Controls.Add(this.groupBoxDelayProperties);
            this.Controls.Add(this.groupBoxMoveProperties);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.listBoxCommands);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelCurrentXPos);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBoxMoveProperties.ResumeLayout(false);
            this.groupBoxMoveProperties.PerformLayout();
            this.groupBoxLoopProperties.ResumeLayout(false);
            this.groupBoxLoopProperties.PerformLayout();
            this.groupBoxDelayProperties.ResumeLayout(false);
            this.groupBoxDelayProperties.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelCurrentXPos;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBoxCommands;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem partyToolStripMenuItem;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.GroupBox groupBoxMoveProperties;
        private System.Windows.Forms.TextBox textBoxMoveProp;
        private System.Windows.Forms.GroupBox groupBoxLoopProperties;
        private System.Windows.Forms.TextBox textBoxLoopN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem delayToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxDelayProperties;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDelay;
    }
}

