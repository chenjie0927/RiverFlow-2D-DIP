namespace Simulation_Options
{
    partial class RiverFLO2DOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RiverFLO2DOptions));
            this.btnTabsColor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTabsColor
            // 
            this.btnTabsColor.Location = new System.Drawing.Point(66, 28);
            this.btnTabsColor.Margin = new System.Windows.Forms.Padding(2);
            this.btnTabsColor.Name = "btnTabsColor";
            this.btnTabsColor.Size = new System.Drawing.Size(73, 24);
            this.btnTabsColor.TabIndex = 0;
            this.btnTabsColor.Text = "Tabs Color";
            this.btnTabsColor.UseVisualStyleBackColor = true;
            this.btnTabsColor.Click += new System.EventHandler(this.btnTabsColor_Click);
            // 
            // RiverFLO2DOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 218);
            this.Controls.Add(this.btnTabsColor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RiverFLO2DOptions";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.RiverFLO2DOptions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTabsColor;
    }
}