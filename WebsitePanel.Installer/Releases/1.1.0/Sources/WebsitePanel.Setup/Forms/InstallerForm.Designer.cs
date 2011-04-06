namespace WebsitePanel.Setup
{
	partial class InstallerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		private Wizard wizard;

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
			this.wizard = new WebsitePanel.Setup.Wizard();
			this.SuspendLayout();
			// 
			// wizard
			// 
			this.wizard.BannerImage = global::WebsitePanel.Setup.Properties.Resources.BannerImage;
			this.wizard.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizard.Location = new System.Drawing.Point(0, 0);
			this.wizard.MarginImage = global::WebsitePanel.Setup.Properties.Resources.MarginImage;
			this.wizard.Name = "wizard";
			this.wizard.SelectedPage = null;
			this.wizard.Size = new System.Drawing.Size(495, 358);
			this.wizard.TabIndex = 0;
			this.wizard.Finish += new System.EventHandler(this.OnWizardFinish);
			this.wizard.Cancel += new System.EventHandler(this.OnWizardCancel);
			// 
			// InstallerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(495, 358);
			this.Controls.Add(this.wizard);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InstallerForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Setup Wizard";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
			this.ResumeLayout(false);

		}

		#endregion

		
	}
}

