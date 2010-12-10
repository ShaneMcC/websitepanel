namespace WebsitePanel.Import.Enterprise
{
	partial class ApplicationForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationForm));
			this.lblSpace = new System.Windows.Forms.Label();
			this.txtSpace = new System.Windows.Forms.TextBox();
			this.btnBrowseSpace = new System.Windows.Forms.Button();
			this.btnBrowseOU = new System.Windows.Forms.Button();
			this.txtOU = new System.Windows.Forms.TextBox();
			this.lblOU = new System.Windows.Forms.Label();
			this.grpOrganization = new System.Windows.Forms.GroupBox();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.btnDeselectAll = new System.Windows.Forms.Button();
			this.rbCreateAndImport = new System.Windows.Forms.RadioButton();
			this.rbImport = new System.Windows.Forms.RadioButton();
			this.lvUsers = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.txtOrgName = new System.Windows.Forms.TextBox();
			this.lblOrgName = new System.Windows.Forms.Label();
			this.txtOrgId = new System.Windows.Forms.TextBox();
			this.lblOrgId = new System.Windows.Forms.Label();
			this.btnStart = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.lblMessage = new System.Windows.Forms.Label();
			this.grpOrganization.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblSpace
			// 
			this.lblSpace.Location = new System.Drawing.Point(15, 15);
			this.lblSpace.Name = "lblSpace";
			this.lblSpace.Size = new System.Drawing.Size(125, 23);
			this.lblSpace.TabIndex = 0;
			this.lblSpace.Text = "Target Hosting Space:";
			// 
			// txtSpace
			// 
			this.txtSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSpace.Location = new System.Drawing.Point(146, 12);
			this.txtSpace.Name = "txtSpace";
			this.txtSpace.ReadOnly = true;
			this.txtSpace.Size = new System.Drawing.Size(429, 20);
			this.txtSpace.TabIndex = 1;
			this.txtSpace.TextChanged += new System.EventHandler(this.OnDataChanged);
			// 
			// btnBrowseSpace
			// 
			this.btnBrowseSpace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseSpace.Location = new System.Drawing.Point(581, 10);
			this.btnBrowseSpace.Name = "btnBrowseSpace";
			this.btnBrowseSpace.Size = new System.Drawing.Size(24, 22);
			this.btnBrowseSpace.TabIndex = 2;
			this.btnBrowseSpace.Text = "...";
			this.btnBrowseSpace.UseVisualStyleBackColor = true;
			this.btnBrowseSpace.Click += new System.EventHandler(this.OnBrowseSpace);
			// 
			// btnBrowseOU
			// 
			this.btnBrowseOU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseOU.Location = new System.Drawing.Point(581, 36);
			this.btnBrowseOU.Name = "btnBrowseOU";
			this.btnBrowseOU.Size = new System.Drawing.Size(24, 22);
			this.btnBrowseOU.TabIndex = 5;
			this.btnBrowseOU.Text = "...";
			this.btnBrowseOU.UseVisualStyleBackColor = true;
			this.btnBrowseOU.Click += new System.EventHandler(this.OnBrowseOU);
			// 
			// txtOU
			// 
			this.txtOU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOU.Location = new System.Drawing.Point(146, 38);
			this.txtOU.Name = "txtOU";
			this.txtOU.ReadOnly = true;
			this.txtOU.Size = new System.Drawing.Size(429, 20);
			this.txtOU.TabIndex = 4;
			this.txtOU.TextChanged += new System.EventHandler(this.OnDataChanged);
			// 
			// lblOU
			// 
			this.lblOU.Location = new System.Drawing.Point(15, 41);
			this.lblOU.Name = "lblOU";
			this.lblOU.Size = new System.Drawing.Size(125, 23);
			this.lblOU.TabIndex = 3;
			this.lblOU.Text = "Organizational Unit:";
			// 
			// grpOrganization
			// 
			this.grpOrganization.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpOrganization.Controls.Add(this.btnSelectAll);
			this.grpOrganization.Controls.Add(this.btnDeselectAll);
			this.grpOrganization.Controls.Add(this.rbCreateAndImport);
			this.grpOrganization.Controls.Add(this.rbImport);
			this.grpOrganization.Controls.Add(this.lvUsers);
			this.grpOrganization.Controls.Add(this.txtOrgName);
			this.grpOrganization.Controls.Add(this.lblOrgName);
			this.grpOrganization.Controls.Add(this.txtOrgId);
			this.grpOrganization.Controls.Add(this.lblOrgId);
			this.grpOrganization.Location = new System.Drawing.Point(15, 67);
			this.grpOrganization.Name = "grpOrganization";
			this.grpOrganization.Size = new System.Drawing.Size(590, 328);
			this.grpOrganization.TabIndex = 6;
			this.grpOrganization.TabStop = false;
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectAll.Location = new System.Drawing.Point(417, 282);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
			this.btnSelectAll.TabIndex = 7;
			this.btnSelectAll.Text = "Select All";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			this.btnSelectAll.Click += new System.EventHandler(this.OnSelectAllClick);
			// 
			// btnDeselectAll
			// 
			this.btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDeselectAll.Location = new System.Drawing.Point(498, 282);
			this.btnDeselectAll.Name = "btnDeselectAll";
			this.btnDeselectAll.Size = new System.Drawing.Size(75, 23);
			this.btnDeselectAll.TabIndex = 8;
			this.btnDeselectAll.Text = "Unselect All";
			this.btnDeselectAll.UseVisualStyleBackColor = true;
			this.btnDeselectAll.Click += new System.EventHandler(this.OnDeselectAllClick);
			// 
			// rbCreateAndImport
			// 
			this.rbCreateAndImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbCreateAndImport.AutoSize = true;
			this.rbCreateAndImport.Checked = true;
			this.rbCreateAndImport.Enabled = false;
			this.rbCreateAndImport.Location = new System.Drawing.Point(19, 282);
			this.rbCreateAndImport.Name = "rbCreateAndImport";
			this.rbCreateAndImport.Size = new System.Drawing.Size(261, 17);
			this.rbCreateAndImport.TabIndex = 5;
			this.rbCreateAndImport.TabStop = true;
			this.rbCreateAndImport.Text = "Create new organization and import selected items";
			this.rbCreateAndImport.UseVisualStyleBackColor = true;
			this.rbCreateAndImport.CheckedChanged += new System.EventHandler(this.OnCheckedChanged);
			// 
			// rbImport
			// 
			this.rbImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbImport.AutoSize = true;
			this.rbImport.Enabled = false;
			this.rbImport.Location = new System.Drawing.Point(19, 305);
			this.rbImport.Name = "rbImport";
			this.rbImport.Size = new System.Drawing.Size(237, 17);
			this.rbImport.TabIndex = 6;
			this.rbImport.Text = "Import selected items for existing organization";
			this.rbImport.UseVisualStyleBackColor = true;
			// 
			// lvUsers
			// 
			this.lvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvUsers.CheckBoxes = true;
			this.lvUsers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.lvUsers.FullRowSelect = true;
			this.lvUsers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvUsers.Location = new System.Drawing.Point(19, 74);
			this.lvUsers.MultiSelect = false;
			this.lvUsers.Name = "lvUsers";
			this.lvUsers.Size = new System.Drawing.Size(554, 202);
			this.lvUsers.SmallImageList = this.images;
			this.lvUsers.TabIndex = 4;
			this.lvUsers.UseCompatibleStateImageBehavior = false;
			this.lvUsers.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			this.columnHeader1.Width = 229;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Email";
			this.columnHeader2.Width = 163;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Type";
			this.columnHeader3.Width = 152;
			// 
			// images
			// 
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			this.images.Images.SetKeyName(0, "UserSmallIcon.ico");
			this.images.Images.SetKeyName(1, "contact.ico");
			this.images.Images.SetKeyName(2, "DL.ico");
			// 
			// txtOrgName
			// 
			this.txtOrgName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOrgName.Location = new System.Drawing.Point(155, 45);
			this.txtOrgName.Name = "txtOrgName";
			this.txtOrgName.Size = new System.Drawing.Size(418, 20);
			this.txtOrgName.TabIndex = 3;
			// 
			// lblOrgName
			// 
			this.lblOrgName.Location = new System.Drawing.Point(19, 48);
			this.lblOrgName.Name = "lblOrgName";
			this.lblOrgName.Size = new System.Drawing.Size(130, 23);
			this.lblOrgName.TabIndex = 2;
			this.lblOrgName.Text = "Organization Name:";
			// 
			// txtOrgId
			// 
			this.txtOrgId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtOrgId.Location = new System.Drawing.Point(155, 19);
			this.txtOrgId.Name = "txtOrgId";
			this.txtOrgId.ReadOnly = true;
			this.txtOrgId.Size = new System.Drawing.Size(418, 20);
			this.txtOrgId.TabIndex = 1;
			// 
			// lblOrgId
			// 
			this.lblOrgId.Location = new System.Drawing.Point(19, 22);
			this.lblOrgId.Name = "lblOrgId";
			this.lblOrgId.Size = new System.Drawing.Size(130, 23);
			this.lblOrgId.TabIndex = 0;
			this.lblOrgId.Text = "Organization Id:";
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.Location = new System.Drawing.Point(527, 461);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 9;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.OnImportClick);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(15, 427);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(587, 23);
			this.progressBar.TabIndex = 8;
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.Location = new System.Drawing.Point(12, 401);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(593, 23);
			this.lblMessage.TabIndex = 7;
			// 
			// ApplicationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(617, 496);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.grpOrganization);
			this.Controls.Add(this.btnBrowseOU);
			this.Controls.Add(this.txtOU);
			this.Controls.Add(this.lblOU);
			this.Controls.Add(this.btnBrowseSpace);
			this.Controls.Add(this.txtSpace);
			this.Controls.Add(this.lblSpace);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(630, 500);
			this.Name = "ApplicationForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "WebsitePanel Enterprise Import Tool";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.grpOrganization.ResumeLayout(false);
			this.grpOrganization.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSpace;
		private System.Windows.Forms.TextBox txtSpace;
		private System.Windows.Forms.Button btnBrowseSpace;
		private System.Windows.Forms.Button btnBrowseOU;
		private System.Windows.Forms.TextBox txtOU;
		private System.Windows.Forms.Label lblOU;
		private System.Windows.Forms.GroupBox grpOrganization;
		private System.Windows.Forms.TextBox txtOrgId;
		private System.Windows.Forms.Label lblOrgId;
		private System.Windows.Forms.TextBox txtOrgName;
		private System.Windows.Forms.Label lblOrgName;
		private System.Windows.Forms.ListView lvUsers;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		internal System.Windows.Forms.Button btnStart;
		internal System.Windows.Forms.ProgressBar progressBar;
		internal System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.RadioButton rbCreateAndImport;
		private System.Windows.Forms.RadioButton rbImport;
		internal System.Windows.Forms.Button btnSelectAll;
		internal System.Windows.Forms.Button btnDeselectAll;
	}
}

