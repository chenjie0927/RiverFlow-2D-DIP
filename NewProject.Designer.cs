namespace Simulation_Options
{
	partial class NewProject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProject));
            this.groupNewProject = new System.Windows.Forms.GroupBox();
            this.radioFE = new System.Windows.Forms.RadioButton();
            this.radioPlus = new System.Windows.Forms.RadioButton();
            this.radioPlusGPU = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnProjectNameAndLocation = new System.Windows.Forms.Button();
            this.txtProjectNameAndLocation = new System.Windows.Forms.TextBox();
            this.groupNewProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupNewProject
            // 
            this.groupNewProject.Controls.Add(this.radioFE);
            this.groupNewProject.Controls.Add(this.radioPlus);
            this.groupNewProject.Controls.Add(this.radioPlusGPU);
            this.groupNewProject.Location = new System.Drawing.Point(79, 19);
            this.groupNewProject.Name = "groupNewProject";
            this.groupNewProject.Size = new System.Drawing.Size(168, 112);
            this.groupNewProject.TabIndex = 5;
            this.groupNewProject.TabStop = false;
            this.groupNewProject.Text = "RiverFlow2D Model";
            // 
            // radioFE
            // 
            this.radioFE.AutoSize = true;
            this.radioFE.Location = new System.Drawing.Point(16, 84);
            this.radioFE.Name = "radioFE";
            this.radioFE.Size = new System.Drawing.Size(102, 17);
            this.radioFE.TabIndex = 5;
            this.radioFE.TabStop = true;
            this.radioFE.Text = "RiverFlow2D FE";
            this.radioFE.UseVisualStyleBackColor = true;
            this.radioFE.Visible = false;
            // 
            // radioPlus
            // 
            this.radioPlus.AutoSize = true;
            this.radioPlus.Location = new System.Drawing.Point(16, 53);
            this.radioPlus.Name = "radioPlus";
            this.radioPlus.Size = new System.Drawing.Size(109, 17);
            this.radioPlus.TabIndex = 4;
            this.radioPlus.TabStop = true;
            this.radioPlus.Text = "RiverFlow2D Plus";
            this.radioPlus.UseVisualStyleBackColor = true;
            // 
            // radioPlusGPU
            // 
            this.radioPlusGPU.AutoSize = true;
            this.radioPlusGPU.Location = new System.Drawing.Point(16, 22);
            this.radioPlusGPU.Name = "radioPlusGPU";
            this.radioPlusGPU.Size = new System.Drawing.Size(135, 17);
            this.radioPlusGPU.TabIndex = 3;
            this.radioPlusGPU.TabStop = true;
            this.radioPlusGPU.Text = "RiverFlow2D Plus GPU";
            this.radioPlusGPU.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(81, 245);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(79, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(166, 244);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(79, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnProjectNameAndLocation
            // 
            this.btnProjectNameAndLocation.Location = new System.Drawing.Point(22, 150);
            this.btnProjectNameAndLocation.Name = "btnProjectNameAndLocation";
            this.btnProjectNameAndLocation.Size = new System.Drawing.Size(225, 29);
            this.btnProjectNameAndLocation.TabIndex = 6;
            this.btnProjectNameAndLocation.Text = "Project Name and Location (click to browse)";
            this.btnProjectNameAndLocation.UseVisualStyleBackColor = true;
            this.btnProjectNameAndLocation.Click += new System.EventHandler(this.btnProjectNameAndLocation_Click);
            // 
            // txtProjectNameAndLocation
            // 
            this.txtProjectNameAndLocation.Location = new System.Drawing.Point(22, 185);
            this.txtProjectNameAndLocation.Multiline = true;
            this.txtProjectNameAndLocation.Name = "txtProjectNameAndLocation";
            this.txtProjectNameAndLocation.ReadOnly = true;
            this.txtProjectNameAndLocation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProjectNameAndLocation.Size = new System.Drawing.Size(293, 53);
            this.txtProjectNameAndLocation.TabIndex = 7;
            // 
            // NewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 280);
            this.Controls.Add(this.txtProjectNameAndLocation);
            this.Controls.Add(this.btnProjectNameAndLocation);
            this.Controls.Add(this.groupNewProject);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Project";
            this.Load += new System.EventHandler(this.NewProject_Load);
            this.groupNewProject.ResumeLayout(false);
            this.groupNewProject.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupNewProject;
		private System.Windows.Forms.RadioButton radioFE;
		private System.Windows.Forms.RadioButton radioPlus;
		private System.Windows.Forms.RadioButton radioPlusGPU;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnProjectNameAndLocation;
		private System.Windows.Forms.TextBox txtProjectNameAndLocation;
	}
}