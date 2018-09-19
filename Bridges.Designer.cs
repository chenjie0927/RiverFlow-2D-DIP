namespace Simulation_Options
{
	partial class Bridges
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bridges));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.actionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPolylineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.lblCoordinatesI = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionsToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1015, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // actionsToolStripMenuItem
            // 
            this.actionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodesToolStripMenuItem,
            this.createPolylineToolStripMenuItem});
            this.actionsToolStripMenuItem.Name = "actionsToolStripMenuItem";
            this.actionsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.actionsToolStripMenuItem.Text = "Actions";
            this.actionsToolStripMenuItem.Click += new System.EventHandler(this.actionsToolStripMenuItem_Click);
            // 
            // addNodesToolStripMenuItem
            // 
            this.addNodesToolStripMenuItem.Name = "addNodesToolStripMenuItem";
            this.addNodesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.addNodesToolStripMenuItem.Text = "Add Nodes";
            this.addNodesToolStripMenuItem.Click += new System.EventHandler(this.addNodesToolStripMenuItem_Click);
            // 
            // createPolylineToolStripMenuItem
            // 
            this.createPolylineToolStripMenuItem.Name = "createPolylineToolStripMenuItem";
            this.createPolylineToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.createPolylineToolStripMenuItem.Text = "Create Polyline";
            this.createPolylineToolStripMenuItem.Click += new System.EventHandler(this.createPolylineToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCoordinatesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // showCoordinatesToolStripMenuItem
            // 
            this.showCoordinatesToolStripMenuItem.Name = "showCoordinatesToolStripMenuItem";
            this.showCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.showCoordinatesToolStripMenuItem.Text = "Show Coordinates";
            this.showCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.showCoordinatesToolStripMenuItem_Click);
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.Color.White;
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(0, 24);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(1015, 652);
            this.Canvas.TabIndex = 3;
            this.Canvas.TabStop = false;
            this.Canvas.Click += new System.EventHandler(this.Canvas_Click);
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseEnter += new System.EventHandler(this.Canvas_MouseEnter);
            this.Canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseMove);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.AutoSize = true;
            this.lblCoordinates.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCoordinates.Location = new System.Drawing.Point(121, 6);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(75, 15);
            this.lblCoordinates.TabIndex = 4;
            this.lblCoordinates.Text = "lblCoordinates";
            // 
            // lblCoordinatesI
            // 
            this.lblCoordinatesI.AutoSize = true;
            this.lblCoordinatesI.Location = new System.Drawing.Point(304, 7);
            this.lblCoordinatesI.Name = "lblCoordinatesI";
            this.lblCoordinatesI.Size = new System.Drawing.Size(35, 13);
            this.lblCoordinatesI.TabIndex = 5;
            this.lblCoordinatesI.Text = "label1";
            this.lblCoordinatesI.Visible = false;
            // 
            // Bridges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 676);
            this.Controls.Add(this.lblCoordinatesI);
            this.Controls.Add(this.lblCoordinates);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Bridges";
            this.Text = "Bridge Construction";
            this.Load += new System.EventHandler(this.Bridges_Load);
            this.ResizeEnd += new System.EventHandler(this.Bridges_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Bridges_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Bridges_KeyPress);
            this.Resize += new System.EventHandler(this.Bridges_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem actionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createPolylineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCoordinatesToolStripMenuItem;
		private System.Windows.Forms.Label lblCoordinates;
		private System.Windows.Forms.PictureBox Canvas;
		private System.Windows.Forms.Label lblCoordinatesI;
	}
}

