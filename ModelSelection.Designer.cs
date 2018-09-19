namespace Simulation_Options
{
	partial class ModelSelection
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelSelection));
			this.groupRiverFLOmodel = new System.Windows.Forms.GroupBox();
			this.radioPlusGPU = new System.Windows.Forms.RadioButton();
			this.radioV4 = new System.Windows.Forms.RadioButton();
			this.radioPlus = new System.Windows.Forms.RadioButton();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupRiverFLOmodel.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupRiverFLOmodel
			// 
			resources.ApplyResources(this.groupRiverFLOmodel, "groupRiverFLOmodel");
			this.groupRiverFLOmodel.Controls.Add(this.radioPlusGPU);
			this.groupRiverFLOmodel.Controls.Add(this.radioV4);
			this.groupRiverFLOmodel.Controls.Add(this.radioPlus);
			this.groupRiverFLOmodel.Name = "groupRiverFLOmodel";
			this.groupRiverFLOmodel.TabStop = false;
			this.groupRiverFLOmodel.Enter += new System.EventHandler(this.groupRiverFLOmodel_Enter);
			// 
			// radioPlusGPU
			// 
			resources.ApplyResources(this.radioPlusGPU, "radioPlusGPU");
			this.radioPlusGPU.Name = "radioPlusGPU";
			this.radioPlusGPU.TabStop = true;
			this.radioPlusGPU.UseVisualStyleBackColor = true;
			// 
			// radioV4
			// 
			resources.ApplyResources(this.radioV4, "radioV4");
			this.radioV4.Name = "radioV4";
			this.radioV4.TabStop = true;
			this.radioV4.UseVisualStyleBackColor = true;
			// 
			// radioPlus
			// 
			resources.ApplyResources(this.radioPlus, "radioPlus");
			this.radioPlus.Name = "radioPlus";
			this.radioPlus.TabStop = true;
			this.radioPlus.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// ModelSelection
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupRiverFLOmodel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ModelSelection";
			this.Load += new System.EventHandler(this.ModelSelection_Load);
			this.groupRiverFLOmodel.ResumeLayout(false);
			this.groupRiverFLOmodel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupRiverFLOmodel;
		private System.Windows.Forms.RadioButton radioV4;
		private System.Windows.Forms.RadioButton radioPlus;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RadioButton radioPlusGPU;
	}
}