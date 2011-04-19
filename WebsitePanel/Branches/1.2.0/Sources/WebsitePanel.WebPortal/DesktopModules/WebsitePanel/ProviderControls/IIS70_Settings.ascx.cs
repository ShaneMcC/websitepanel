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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Portal.ProviderControls
{
	public partial class IIS70_Settings : WebsitePanelControlBase, IHostingServiceProviderSettings
	{
		private string FilteredAppIds;

		public const string WDeployEnabled = "WDeployEnabled";
		public const string WDeployRepair = "WDeployRepair";

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		public void WebAppGalleryList_DataBound(object sender, EventArgs e)
		{
			SetAppsCalatolgFilter(FilteredAppIds);
		}

		public void ResetButton_Click(object sender, EventArgs e)
		{
			WebAppGalleryList.ClearSelection();
			//
			FilterDialogButton.Text = GetLocalizedString("FilterDialogButton.Text");
		}

		protected void SetAppsCalatolgFilter(string appsIds)
		{
			if (String.IsNullOrEmpty(appsIds))
				return;
			//
			string[] filteredApps = appsIds.Split(new string[] { "," }, 
				StringSplitOptions.RemoveEmptyEntries);
			//
			foreach (ListItem li in WebAppGalleryList.Items)
			{
				li.Selected = Array.Exists<string>(filteredApps, 
					x => x.Equals(li.Value.Trim(), 
						StringComparison.InvariantCultureIgnoreCase));
			}
			//
			FilterDialogButton.Text = GetLocalizedString("FilterDialogButton.AlternateText");
		}

		protected string GetAppsCatalogFilter()
		{
			var builder = new StringBuilder();
			var formatStr = "{0}";
			//
			foreach (ListItem li in WebAppGalleryList.Items)
			{
				if (li.Selected)
				{
					builder.AppendFormat(formatStr, li.Value.Trim());
					//
					formatStr = ",{0}";
				}
			}
			//
			return builder.ToString();
		}

		public void BindSettings(StringDictionary settings)
		{
			//
			ipAddress.AddressId = (settings["SharedIP"] != null) ? Utils.ParseInt(settings["SharedIP"], 0) : 0;
            ipAddress.SelectValueText = GetLocalizedString("ipAddress.SelectValueText");

			txtWebGroupName.Text = settings["WebGroupName"];
			chkAssignIPAutomatically.Checked = Utils.ParseBool(settings["AutoAssignDedicatedIP"], true);

			txtAspNet11Path.Text = settings["AspNet11Path"];
			txtAspNet20Path.Text = settings["AspNet20Path"];
			txtAspNet20x64Path.Text = settings["AspNet20x64Path"];
            txtAspNet40Path.Text = settings["AspNet40Path"];
			txtAspNet40x64Path.Text = settings["AspNet40x64Path"];

			txtAspNet11Pool.Text = settings["AspNet11Pool"];
			// ASP.NET 2.0
			txtAspNet20Pool.Text = settings["ClassicAspNet20Pool"];
			txtAspNet20IntegratedPool.Text = settings["IntegratedAspNet20Pool"];
			// ASP.NET 4.0
			ClassicAspNet40Pool.Text = settings[ClassicAspNet40Pool.ID];
			IntegratedAspNet40Pool.Text = settings[IntegratedAspNet40Pool.ID];
			// ASP.NET 2.0 & 4.0 Bitness Mode
			Utils.SelectListItem(AspNetBitnessMode, settings[AspNetBitnessMode.ID]);

			txtGalleryFeedUrl.Text = settings["GalleryXmlFeedUrl"];

			txtAspPath.Text = settings["AspPath"];
			php4Path.Text = settings["Php4Path"];
			txtPhpPath.Text = settings["PhpPath"];
			Utils.SelectListItem(ddlPhpMode, settings["PhpMode"]);

			perlPath.Text = settings["PerlPath"];

			txtColdFusionPath.Text = settings["ColdFusionPath"];
            txtScriptsDirectory.Text = settings["CFScriptsDirectory"];
            txtFlashRemotingDir.Text = settings["CFFlashRemotingDirectory"];


			txtProtectedUsersFile.Text = settings["ProtectedUsersFile"];
			txtProtectedGroupsFile.Text = settings["ProtectedGroupsFile"];
			txtSecureFoldersModuleAsm.Text = settings["SecureFoldersModuleAssembly"];

            //Helicon Ape   
            WebsitePanel.Providers.ResultObjects.HeliconApeStatus sts = ES.Services.WebServers.GetHeliconApeStatus(int.Parse(Request.QueryString["ServiceID"]));

            if (sts.IsInstalled)
            {
                downloadApePanel.Visible = false;
                txtHeliconApeVersion.Text = sts.Version;
                lblHeliconRegistrationText.Text = sts.RegistrationInfo;
            }
            else
            {
                configureApePanel.Visible = false;
            }

            //
			sharedSslSites.Value = settings["SharedSslSites"];
			ActiveDirectoryIntegration.BindSettings(settings);

			//
			txtWmSvcServicePort.Text = settings["WmSvc.Port"];
			//
			string wmsvcServiceUrl = settings["WmSvc.ServiceUrl"];
			//
			if (wmsvcServiceUrl == "*")
				wmsvcServiceUrl = String.Empty;
			// Set service url as is
			txtWmSvcServiceUrl.Text = wmsvcServiceUrl;

			//
            //if (settings["WmSvc.RequiresWindowsCredentials"] == "1")
            //    ddlWmSvcCredentialsMode.Items.RemoveAt(1);
			//
			Utils.SelectListItem(ddlWmSvcCredentialsMode, settings["WmSvc.CredentialsMode"]);
			//
			FilteredAppIds = settings["GalleryAppsFilter"];
			//
			if (String.IsNullOrEmpty(settings[WDeployEnabled]) == false)
			{
				if (Convert.ToBoolean(settings[WDeployEnabled]) == true)
				{
					WDeployEnabledCheckBox.Checked = true;
				}
				else
				{
					WDeployDisabledCheckBox.Checked = true;
				}
			}
		}

		public void SaveSettings(StringDictionary settings)
		{
			//
			settings["SharedIP"] = ipAddress.AddressId.ToString();
			settings["WebGroupName"] = txtWebGroupName.Text.Trim();
			settings["AutoAssignDedicatedIP"] = chkAssignIPAutomatically.Checked.ToString();

            // paths
            settings["AspNet11Path"] = txtAspNet11Path.Text.Trim();
            settings["AspNet20Path"] = txtAspNet20Path.Text.Trim();
            settings["AspNet20x64Path"] = txtAspNet20x64Path.Text.Trim();
            settings["AspNet40Path"] = txtAspNet40Path.Text.Trim();
            settings["AspNet40x64Path"] = txtAspNet40x64Path.Text.Trim();

			settings["AspNet11Pool"] = txtAspNet11Pool.Text.Trim();
			// ASP.NET 2.0
			settings["ClassicAspNet20Pool"] = txtAspNet20Pool.Text.Trim();
			settings["IntegratedAspNet20Pool"] = txtAspNet20IntegratedPool.Text.Trim();
			// ASP.NET 4.0
			settings[ClassicAspNet40Pool.ID] = ClassicAspNet40Pool.Text.Trim();
			settings[IntegratedAspNet40Pool.ID] = IntegratedAspNet40Pool.Text.Trim();
			// ASP.NET 2.0 & 4.0 Bitness Mode
			settings[AspNetBitnessMode.ID] = AspNetBitnessMode.SelectedValue;

			settings["GalleryXmlFeedUrl"] = txtGalleryFeedUrl.Text.Trim();

            settings["AspPath"] = txtAspPath.Text.Trim();
			settings["Php4Path"] = php4Path.Text.Trim();
			settings["PhpPath"] = txtPhpPath.Text.Trim();
			settings["PhpMode"] = ddlPhpMode.SelectedValue;
			settings["PerlPath"] = perlPath.Text.Trim();
			settings["ColdFusionPath"] = txtColdFusionPath.Text.Trim();
            settings["CFScriptsDirectory"] = txtScriptsDirectory.Text.Trim();
            settings["CFFlashRemotingDirectory"] = txtFlashRemotingDir.Text.Trim();

			settings["SharedSslSites"] = sharedSslSites.Value;

			settings["ProtectedUsersFile"] = txtProtectedUsersFile.Text.Trim();
			settings["ProtectedGroupsFile"] = txtProtectedGroupsFile.Text.Trim();
			settings["SecureFoldersModuleAssembly"] = txtSecureFoldersModuleAsm.Text.Trim();

			settings["WmSvc.ServiceUrl"] = txtWmSvcServiceUrl.Text.Trim();
			settings["WmSvc.Port"] = Utils.ParseInt(txtWmSvcServicePort.Text.Trim(), 0).ToString();
			settings["WmSvc.CredentialsMode"] = ddlWmSvcCredentialsMode.SelectedValue;

			ActiveDirectoryIntegration.SaveSettings(settings);

			settings["GalleryAppsFilter"] = GetAppsCatalogFilter();

			if (WDeployEnabledCheckBox.Checked)
			{
				settings[WDeployEnabled] = Boolean.TrueString;
				//
				if (WDeployRepairSettingCheckBox.Checked)
				{
					settings[WDeployRepair] = Boolean.TrueString;
				}
			}
			else if (WDeployDisabledCheckBox.Checked)
			{
				settings[WDeployEnabled] = Boolean.FalseString;
			}
		}

        protected void DownladAndIstallApeLinkButton_Click(object sender, EventArgs e)
        {
            ES.Services.WebServers.InstallHeliconApe(PanelRequest.ServiceId);

            //Redirect to avoid 2-nd call
            Response.Redirect(this.Context.Request.Url.AbsoluteUri);
        }
        
        public string GetHttpdEditControlUrl(string ctrlKey, string name)
        {
            return HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), ctrlKey,
                "Name=" + name,
                PortalUtils.SPACE_ID_PARAM + "=" + int.Parse(Request.QueryString["ServiceID"]),
                "ReturnUrlBase64="+ EncodeTo64(Server.UrlEncode(Request.Url.PathAndQuery))
                );
        }

        static public string EncodeTo64(string toEncode)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));
        }

        protected void EditHeliconApeConfButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetHttpdEditControlUrl("edit_htaccessfolder", "httpd.conf"));
        }

	}
}