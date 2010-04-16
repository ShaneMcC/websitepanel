namespace WebsitePanel.Setup
{
	partial class WebPage
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
			this.grpWebSiteSettings = new System.Windows.Forms.GroupBox();
			this.lblHint = new System.Windows.Forms.Label();
			this.cbWebSiteIP = new System.Windows.Forms.ComboBox();
			this.lblWebSiteTcpPort = new System.Windows.Forms.Label();
			this.txtWebSiteTcpPort = new System.Windows.Forms.TextBox();
			this.lblWebSiteIP = new System.Windows.Forms.Label();
			this.lblWebSiteDomain = new System.Windows.Forms.Label();
			this.txtWebSiteDomain = new System.Windows.Forms.TextBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.lblWarning = new System.Windows.Forms.Label();
			this.grpWebSiteSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpWebSiteSettings
			// 
			this.grpWebSiteSettings.Controls.Add(this.lblHint);
			this.grpWebSiteSettings.Controls.Add(this.cbWebSiteIP);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteTcpPort);
			this.grpWebSiteSettings.Controls.Add(this.txtWebSiteTcpPort);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteIP);
			this.grpWebSiteSettings.Controls.Add(this.lblWebSiteDomain);
			this.grpWebSiteSettings.Controls.Add(this.txtWebSiteDomain);
			this.grpWebSiteSettings.Location = new System.Drawing.Point(30, 27);
			this.grpWebSiteSettings.Name = "grpWebSiteSettings";
			this.grpWebSiteSettings.Size = new System.Drawing.Size(396, 141);
			this.grpWebSiteSettings.TabIndex = 0;
			this.grpWebSiteSettings.TabStop = false;
			this.grpWebSiteSettings.Text = "Web Site Settings";
			// 
			// lblHint
			// 
			this.lblHint.Location = new System.Drawing.Point(30, 116);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(330, 16);
			this.lblHint.TabIndex = 6;
			this.lblHint.Text = "Example: www.contoso.com or panel.contoso.com";
			// 
			// cbWebSiteIP
			// 
			this.cbWebSiteIP.Location = new System.Drawing.Point(33, 41);
			this.cbWebSiteIP.Name = "cbWebSiteIP";
			this.cbWebSiteIP.Size = new System.Drawing.Size(220, 21);
			this.cbWebSiteIP.TabIndex = 1;
			this.cbWebSiteIP.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblWebSiteTcpPort
			// 
			this.lblWebSiteTcpPort.Location = new System.Drawing.Point(264, 22);
			this.lblWebSiteTcpPort.Name = "lblWebSiteTcpPort";
			this.lblWebSiteTcpPort.Size = new System.Drawing.Size(96, 16);
			this.lblWebSiteTcpPort.TabIndex = 2;
			this.lblWebSiteTcpPort.Text = "Port:";
			// 
			// txtWebSiteTcpPort
			// 
			this.txtWebSiteTcpPort.Location = new System.Drawing.Point(267, 41);
			this.txtWebSiteTcpPort.Name = "txtWebSiteTcpPort";
			this.txtWebSiteTcpPort.Size = new System.Drawing.Size(48, 20);
			this.txtWebSiteTcpPort.TabIndex = 3;
			this.txtWebSiteTcpPort.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblWebSiteIP
			// 
			this.lblWebSiteIP.Location = new System.Drawing.Point(30, 22);
			this.lblWebSiteIP.Name = "lblWebSiteIP";
			this.lblWebSiteIP.Size = new System.Drawing.Size(104, 16);
			this.lblWebSiteIP.TabIndex = 0;
			this.lblWebSiteIP.Text = "IP address:";
			// 
			// lblWebSiteDomain
			// 
			this.lblWebSiteDomain.Location = new System.Drawing.Point(30, 74);
			this.lblWebSiteDomain.Name = "lblWebSiteDomain";
			this.lblWebSiteDomain.Size = new System.Drawing.Size(104, 16);
			this.lblWebSiteDomain.TabIndex = 4;
			this.lblWebSiteDomain.Text = "Host name:";
			// 
			// txtWebSiteDomain
			// 
			this.txtWebSiteDomain.Location = new System.Drawing.Point(33, 93);
			this.txtWebSiteDomain.Name = "txtWebSiteDomain";
			this.txtWebSiteDomain.Size = new System.Drawing.Size(327, 20);
			this.txtWebSiteDomain.TabIndex = 5;
			this.txtWebSiteDomain.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// txtAddress
			// 
			this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtAddress.Location = new System.Drawing.Point(30, 8);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.ReadOnly = true;
			this.txtAddress.Size = new System.Drawing.Size(396, 13);
			this.txtAddress.TabIndex = 2;
			// 
			// lblWarning
			// 
			this.lblWarning.Location = new System.Drawing.Point(30, 171);
			this.lblWarning.Name = "lblWarning";
			this.lblWarning.Size = new System.Drawing.Size(396, 36);
			this.lblWarning.TabIndex = 1;
			this.lblWarning.Text = "Make sure the specified host name is pointed to this web site; otherwise you migh" +
				"t not be able to access the application.\r\n";
			// 
			// WebPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblWarning);
			this.Controls.Add(this.grpWebSiteSettings);
			this.Controls.Add(this.txtAddress);
			this.Name = "WebPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpWebSiteSettings.ResumeLayout(false);
			this.grpWebSiteSettings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpWebSiteSettings;
		private System.Windows.Forms.Label lblWebSiteTcpPort;
		private System.Windows.Forms.TextBox txtWebSiteTcpPort;
		private System.Windows.Forms.Label lblWebSiteIP;
		private System.Windows.Forms.Label lblWebSiteDomain;
		private System.Windows.Forms.TextBox txtWebSiteDomain;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblWarning;
		private System.Windows.Forms.ComboBox cbWebSiteIP;
		private System.Windows.Forms.Label lblHint;



	}
}
