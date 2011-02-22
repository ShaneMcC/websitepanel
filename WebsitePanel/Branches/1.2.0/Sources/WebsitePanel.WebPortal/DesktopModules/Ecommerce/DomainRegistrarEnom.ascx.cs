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

using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class DomainRegistrarEnom : ecModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadRegistrarSettings();
		}

		protected void btnSaveSettings_Click(object sender, EventArgs e)
		{
			SaveRegistrarSettings();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToBrowsePage();
		}

		protected void btnDisable_Click(object sender, EventArgs e)
		{
			DisablePlugin();
		}

		private void LoadRegistrarSettings()
		{
			try
			{
				KeyValueBunch props = StorehouseHelper.GetPluginProperties(SupportedPlugin.ENOM);
				//
				if (!props.IsEmpty)
				{
					txtServiceUsername.Text = props[EnomSettings.USERNAME];
					txtServicePassword.EnableDefaultPassword();
					chkLiveMode.Checked = ecUtils.ParseBoolean(props[EnomSettings.LIVE_MODE], false);
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("LOAD_PLUGIN_SETTINGS", ex);
			}
		}

		private void DisablePlugin()
		{
			try
			{
				int result = StorehouseHelper.SetPluginProperties(SupportedPlugin.ENOM, null);
				// ERROR
				if (result < 0)
				{
					HostModule.ShowResultMessage(result);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				// ERROR
				ShowErrorMessage("DISABLE_PLUGIN", ex);
			}
		}

		private void SaveRegistrarSettings()
		{
			try
			{
				KeyValueBunch props = new KeyValueBunch();
				//
				props[EnomSettings.USERNAME] = txtServiceUsername.Text.Trim();
				props[EnomSettings.LIVE_MODE] = chkLiveMode.Checked.ToString();
				if (txtServicePassword.PasswordChanged)
					props[EnomSettings.PASSWORD] = txtServicePassword.Text;
				// set plugins props
				int result = StorehouseHelper.SetPluginProperties(SupportedPlugin.ENOM, props);
				if (result < 0)
				{
					// ERROR
					ShowResultMessage(result);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SAVE_PLUGIN_SETTINGS", ex);
			}
		}
	}
}