namespace WebsitePanel.Setup
{
	partial class ServiceAddressPage
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
			this.grpSettings = new System.Windows.Forms.GroupBox();
			this.cbIP = new System.Windows.Forms.ComboBox();
			this.lblTcpPort = new System.Windows.Forms.Label();
			this.txtTcpPort = new System.Windows.Forms.TextBox();
			this.lblIP = new System.Windows.Forms.Label();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.grpSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.cbIP);
			this.grpSettings.Controls.Add(this.lblTcpPort);
			this.grpSettings.Controls.Add(this.txtTcpPort);
			this.grpSettings.Controls.Add(this.lblIP);
			this.grpSettings.Location = new System.Drawing.Point(30, 37);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Size = new System.Drawing.Size(396, 76);
			this.grpSettings.TabIndex = 1;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Address Settings";
			// 
			// cbIP
			// 
			this.cbIP.Location = new System.Drawing.Point(140, 19);
			this.cbIP.Name = "cbIP";
			this.cbIP.Size = new System.Drawing.Size(220, 21);
			this.cbIP.TabIndex = 3;
			this.cbIP.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblTcpPort
			// 
			this.lblTcpPort.Location = new System.Drawing.Point(30, 50);
			this.lblTcpPort.Name = "lblTcpPort";
			this.lblTcpPort.Size = new System.Drawing.Size(96, 16);
			this.lblTcpPort.TabIndex = 4;
			this.lblTcpPort.Text = "TCP Port:";
			// 
			// txtTcpPort
			// 
			this.txtTcpPort.Location = new System.Drawing.Point(140, 46);
			this.txtTcpPort.Name = "txtTcpPort";
			this.txtTcpPort.Size = new System.Drawing.Size(48, 20);
			this.txtTcpPort.TabIndex = 5;
			this.txtTcpPort.TextChanged += new System.EventHandler(this.OnAddressChanged);
			// 
			// lblIP
			// 
			this.lblIP.Location = new System.Drawing.Point(30, 22);
			this.lblIP.Name = "lblIP";
			this.lblIP.Size = new System.Drawing.Size(104, 16);
			this.lblIP.TabIndex = 0;
			this.lblIP.Text = "IP Address:";
			// 
			// txtAddress
			// 
			this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtAddress.Location = new System.Drawing.Point(28, 11);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.ReadOnly = true;
			this.txtAddress.Size = new System.Drawing.Size(398, 13);
			this.txtAddress.TabIndex = 0;
			// 
			// ServiceAddressPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.grpSettings);
			this.Controls.Add(this.txtAddress);
			this.Name = "ServiceAddressPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.grpSettings.ResumeLayout(false);
			this.grpSettings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.Label lblTcpPort;
		private System.Windows.Forms.TextBox txtTcpPort;
		private System.Windows.Forms.Label lblIP;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.ComboBox cbIP;



	}
}
