namespace WebsitePanel.Setup
{
	partial class UrlPage
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
			this.lblURL = new System.Windows.Forms.Label();
			this.txtURL = new System.Windows.Forms.TextBox();
			this.lblIntro = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblURL
			// 
			this.lblURL.Location = new System.Drawing.Point(3, 92);
			this.lblURL.Name = "lblURL";
			this.lblURL.Size = new System.Drawing.Size(139, 23);
			this.lblURL.TabIndex = 13;
			this.lblURL.Text = "Enterprise Server URL:";
			// 
			// txtURL
			// 
			this.txtURL.Location = new System.Drawing.Point(148, 92);
			this.txtURL.Name = "txtURL";
			this.txtURL.Size = new System.Drawing.Size(306, 20);
			this.txtURL.TabIndex = 14;
			// 
			// lblIntro
			// 
			this.lblIntro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblIntro.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIntro.Location = new System.Drawing.Point(0, 0);
			this.lblIntro.Name = "lblIntro";
			this.lblIntro.Size = new System.Drawing.Size(457, 58);
			this.lblIntro.TabIndex = 12;
			this.lblIntro.Text = "Please, specify URL which will be used to access the Enterprise Server from the P" +
				"ortal. Click Next to continue.";
			// 
			// UrlPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblURL);
			this.Controls.Add(this.txtURL);
			this.Controls.Add(this.lblIntro);
			this.Name = "UrlPage";
			this.Size = new System.Drawing.Size(457, 228);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblURL;
		private System.Windows.Forms.TextBox txtURL;
		private System.Windows.Forms.Label lblIntro;

	}
}
