// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WSP = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
	public partial class SystemSettings : WebsitePanelModuleBase
	{
		public const string SMTP_SERVER = "SmtpServer";
		public const string SMTP_PORT = "SmtpPort";
		public const string SMTP_USERNAME = "SmtpUsername";
		public const string SMTP_PASSWORD = "SmtpPassword";
		public const string SMTP_ENABLE_SSL = "SmtpEnableSsl";

		public const string BACKUPS_PATH = "BackupsPath";

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					LoadSettings();
				}
				catch (Exception ex)
				{
					ShowErrorMessage("SYSTEM_SETTINGS_LOAD", ex);
				}
			}
		}

		private void LoadSettings()
		{
			// SMTP
			WSP.SystemSettings settings = ES.Services.System.GetSystemSettings(
				WSP.SystemSettings.SMTP_SETTINGS);

			if (settings != null)
			{
				txtSmtpServer.Text = settings[SMTP_SERVER];
				txtSmtpPort.Text = settings[SMTP_PORT];
				txtSmtpUser.Text = settings[SMTP_USERNAME];
				chkEnableSsl.Checked = Utils.ParseBool(settings[SMTP_ENABLE_SSL], false);
			}

			// BACKUP
			settings = ES.Services.System.GetSystemSettings(
				WSP.SystemSettings.BACKUP_SETTINGS);

			if (settings != null)
			{
				txtBackupsPath.Text = settings["BackupsPath"];
			}
		}

		private void SaveSettings()
		{
			try
			{
				WSP.SystemSettings settings = new WSP.SystemSettings();

				// SMTP
				settings[SMTP_SERVER] = txtSmtpServer.Text.Trim();
				settings[SMTP_PORT] = txtSmtpPort.Text.Trim();
				settings[SMTP_USERNAME] = txtSmtpUser.Text.Trim();
				settings[SMTP_PASSWORD] = txtSmtpPassword.Text;
				settings[SMTP_ENABLE_SSL] = chkEnableSsl.Checked.ToString();

				// SMTP
				int result = ES.Services.System.SetSystemSettings(
					WSP.SystemSettings.SMTP_SETTINGS, settings);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}

				// BACKUP
				settings = new WSP.SystemSettings();
				settings[BACKUPS_PATH] = txtBackupsPath.Text.Trim();

				// BACKUP
				result = ES.Services.System.SetSystemSettings(
					WSP.SystemSettings.BACKUP_SETTINGS, settings);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SYSTEM_SETTINGS_SAVE", ex);
				return;
			}

			ShowSuccessMessage("SYSTEM_SETTINGS_SAVE");
		}

		protected void btnSaveSettings_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}
	}
}