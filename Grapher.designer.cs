namespace Simulation_Options
{
    partial class Grapher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Grapher));
            this.statBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Posx = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Posy = new System.Windows.Forms.ToolStripStatusLabel();
            this.Legend = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmboDataShown = new System.Windows.Forms.ToolStripComboBox();
            this.statBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statBar
            // 
            this.statBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.Posx,
            this.toolStripStatusLabel2,
            this.Posy});
            this.statBar.Location = new System.Drawing.Point(0, 377);
            this.statBar.Name = "statBar";
            this.statBar.Size = new System.Drawing.Size(741, 22);
            this.statBar.TabIndex = 0;
            this.statBar.Text = "Mouse Position";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(20, 17);
            this.toolStripStatusLabel1.Text = "X: ";
            // 
            // Posx
            // 
            this.Posx.Name = "Posx";
            this.Posx.Size = new System.Drawing.Size(50, 17);
            this.Posx.Text = "MouseX";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(20, 17);
            this.toolStripStatusLabel2.Text = "Y: ";
            // 
            // Posy
            // 
            this.Posy.Name = "Posy";
            this.Posy.Size = new System.Drawing.Size(50, 17);
            this.Posy.Text = "MouseY";
            // 
            // Legend
            // 
            this.Legend.AutoSize = true;
            this.Legend.BackColor = System.Drawing.Color.LightGray;
            this.Legend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Legend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Legend.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Legend.ForeColor = System.Drawing.Color.Black;
            this.Legend.Location = new System.Drawing.Point(570, 133);
            this.Legend.Name = "Legend";
            this.Legend.Size = new System.Drawing.Size(51, 16);
            this.Legend.TabIndex = 1;
            this.Legend.Text = "Legend";
            this.Legend.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Legend_MouseDown);
            this.Legend.MouseEnter += new System.EventHandler(this.Legend_MouseEnter);
            this.Legend.MouseLeave += new System.EventHandler(this.Legend_MouseLeave);
            this.Legend.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Legend_MouseMove);
            this.Legend.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Legend_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmboDataShown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(741, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(73, 22);
            this.toolStripLabel1.Text = "Data Shown:";
            // 
            // cmboDataShown
            // 
            this.cmboDataShown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboDataShown.Name = "cmboDataShown";
            this.cmboDataShown.Size = new System.Drawing.Size(175, 25);
            this.cmboDataShown.SelectedIndexChanged += new System.EventHandler(this.cmboDataShown_SelectedIndexChanged);
            // 
            // Grapher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(741, 399);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.Legend);
            this.Controls.Add(this.statBar);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Grapher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Grapher_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Grapher_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Grapher_MouseMove);
            this.Resize += new System.EventHandler(this.Grapher_Resize);
            this.statBar.ResumeLayout(false);
            this.statBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statBar;
        private System.Windows.Forms.ToolStripStatusLabel Posx;
        private System.Windows.Forms.ToolStripStatusLabel Posy;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label Legend;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
				private System.Windows.Forms.ToolStripComboBox cmboDataShown;
    }
}