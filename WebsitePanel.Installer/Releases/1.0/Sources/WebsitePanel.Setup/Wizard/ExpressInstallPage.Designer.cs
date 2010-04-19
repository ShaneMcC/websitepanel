namespace WebsitePanel.Setup
{
	partial class ExpressInstallPage
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpFiles = new System.Windows.Forms.GroupBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblProcess = new System.Windows.Forms.Label();
			this.lblIntro = new System.Windows.Forms.Label();
			this.grpFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpFiles
			// 
			this.grpFiles.Controls.Add(this.progressBar);
			this.grpFiles.Controls.Add(this.lblProcess);
			this.grpFiles.Location = new System.Drawing.Point(0, 43);
			this.grpFiles.Name = "grpFiles";
			this.grpFiles.Size = new System.Drawing.Size(457, 88);
			this.grpFiles.TabIndex = 4;
			this.grpFiles.TabStop = false;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(16, 40);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(427, 23);
			this.progressBar.Step = 1;
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 1;
			// 
			// lblProcess
			// 
			this.lblProcess.Location = new System.Drawing.Point(16, 24);
			this.lblProcess.Name = "lblProcess";
			this.lblProcess.Size = new System.Drawing.Size(427, 16);
			this.lblProcess.TabIndex = 0;
			// 
			// lblIntro
			// 
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(3, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(451, 40);
			this.lblIntro.TabIndex = 5;
			// 
			// ExpressInstallPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpFiles);
			this.Controls.Add(this.lblIntro);
			this.Name = "ExpressInstallPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpFiles.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpFiles;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Label lblProcess;
		private System.Windows.Forms.Label lblIntro;
	}
}
