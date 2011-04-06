namespace WebsitePanel.Installer
{
	partial class TopLogoControl
    {
		private System.Windows.Forms.Panel pnlLogo;
		private System.Windows.Forms.PictureBox imgLogo;
		private System.Windows.Forms.Label lblVersion;
		private LineBox line;
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TopLogoControl));
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.progressIcon = new WebsitePanel.Installer.Controls.ProgressIcon();
            this.lblVersion = new System.Windows.Forms.Label();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.line = new WebsitePanel.Installer.LineBox();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLogo
            // 
            this.pnlLogo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLogo.BackColor = System.Drawing.Color.White;
            this.pnlLogo.Controls.Add(this.progressIcon);
            this.pnlLogo.Controls.Add(this.lblVersion);
            this.pnlLogo.Controls.Add(this.imgLogo);
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(496, 63);
            this.pnlLogo.TabIndex = 2;
            // 
            // progressIcon
            // 
            this.progressIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressIcon.Location = new System.Drawing.Point(452, 15);
            this.progressIcon.Name = "progressIcon";
            this.progressIcon.Size = new System.Drawing.Size(30, 30);
            this.progressIcon.TabIndex = 4;
            this.progressIcon.Visible = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblVersion.Location = new System.Drawing.Point(317, 31);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(42, 13);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "v1.0";
            // 
            // imgLogo
            // 
            this.imgLogo.Image = ((System.Drawing.Image)(resources.GetObject("imgLogo.Image")));
            this.imgLogo.Location = new System.Drawing.Point(13, 7);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(303, 48);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgLogo.TabIndex = 0;
            this.imgLogo.TabStop = false;
            // 
            // line
            // 
            this.line.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.line.Location = new System.Drawing.Point(0, 61);
            this.line.Name = "line";
            this.line.Size = new System.Drawing.Size(496, 2);
            this.line.TabIndex = 3;
            this.line.TabStop = false;
            // 
            // TopLogoControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.line);
            this.Controls.Add(this.pnlLogo);
            this.Name = "TopLogoControl";
            this.Size = new System.Drawing.Size(496, 64);
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private WebsitePanel.Installer.Controls.ProgressIcon progressIcon;


	}
}

