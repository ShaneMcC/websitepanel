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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers;
using WebsitePanel.Providers.Web;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Portal
{
    public partial class WebSitesEditSite : WebsitePanelModuleBase
    {

        class Tab
        {
            int index;
            string id;
            string name;

            public Tab(int index, string id, string name)
            {
                this.index = index;
                this.id = id;
                this.name = name;
            }

            public int Index
            {
                get { return this.index; }
                set { this.index = value; }
            }

            public string Id
            {
                get { return this.id; }
                set { this.id = value; }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }
        }

        private int PackageId
        {
            get { return (int)ViewState["PackageId"]; }
            set { ViewState["PackageId"] = value; }
        }

        private bool IIs7
        {
            get { return (bool)ViewState["IIs7"]; }
            set { ViewState["IIs7"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindWebSite();
            }
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(new Tab(0, "home", GetLocalizedString("Tab.HomeFolder")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_VIRTUALDIRS))
                tabsList.Add(new Tab(1, "vdirs", GetLocalizedString("Tab.VirtualDirs")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_SECUREDFOLDERS))
                tabsList.Add(new Tab(2, "securedfolders", GetLocalizedString("Tab.SecuredFolders")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_FRONTPAGE))
                tabsList.Add(new Tab(3, "frontpage", GetLocalizedString("Tab.FrontPage")));
            tabsList.Add(new Tab(4, "extensions", GetLocalizedString("Tab.Extensions")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_ERRORS))
                tabsList.Add(new Tab(5, "errors", GetLocalizedString("Tab.CustomErrors")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_HEADERS))
                tabsList.Add(new Tab(6, "headers", GetLocalizedString("Tab.CustomHeaders")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_MIME))
                tabsList.Add(new Tab(7, "mime", GetLocalizedString("Tab.MIMETypes")));
            if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_COLDFUSION))
                tabsList.Add(new Tab(8, "coldfusion", GetLocalizedString("Tab.ColdFusion")));
			if (PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_REMOTEMANAGEMENT))
				tabsList.Add(new Tab(9, "webman", GetLocalizedString("Tab.WebManagement")));

            if (dlTabs.SelectedIndex == -1)
            {
                if (!IsPostBack && Request["MenuID"] != null)
                {
                    // find required menu item
                    int idx = 0;
                    foreach (Tab tab in tabsList)
                    {
                        if (String.Compare(tab.Id, Request["MenuID"], true) == 0)
                            break;
                        idx++;
                    }
                    dlTabs.SelectedIndex = idx;
                }
                else
                    dlTabs.SelectedIndex = 0;
            }

            dlTabs.DataSource = tabsList.ToArray();
            dlTabs.DataBind();

            tabs.ActiveViewIndex = tabsList[dlTabs.SelectedIndex].Index;
        }

        protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabs();
        }

        private void BindWebSite()
        {
            WebSite site = null;
            try
            {
                site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_GET_SITE", ex);
                return;
            }

            if (site == null)
                RedirectToBrowsePage();

            // IIS 7.0 mode
            IIs7 = site.IIs7;

            

            PackageId = site.PackageId;

            // bind site
            lnkSiteName.Text = site.Name;
            lnkSiteName.NavigateUrl = "http://" + site.Name;
            litIPAddress.Text = site.SiteIPAddress;
            
            litFrontPageUnavailable.Visible = false;
            tblSharePoint.Visible = site.SharePointInstalled;
            tblFrontPage.Visible = !site.SharePointInstalled;

            
            if (!site.ColdFusionAvailable)
            {
                litCFUnavailable.Text = GetLocalizedString("Text.COLDFUSION_UNAVAILABLE");
                litCFUnavailable.Visible = true;
                rowCF.Visible = false;
                rowVirtDir.Visible = false;
            }
            else
            {
                if (site.ColdFusionVersion.Equals("7"))
                {
                    litCFUnavailable.Text = "ColdFusion 7.x is installed";
                    litCFUnavailable.Visible = true;
                }
                else
                {
                    if (site.ColdFusionVersion.Equals("8"))
                        litCFUnavailable.Text = "ColdFusion 8.x is installed";
                    litCFUnavailable.Visible = true;
                }

                if (site.ColdFusionVersion.Equals("9"))
                    litCFUnavailable.Text = "ColdFusion 9.x is installed";
                litCFUnavailable.Visible = true;
                
            }

            if (!PackagesHelper.CheckGroupQuotaEnabled(PackageId, ResourceGroups.Web, Quotas.WEB_CFVIRTUALDIRS))
            {
                //virtual directories are not implemented for IIS 7
                rowVirtDir.Visible = false;
            }
            
            chkCfExt.Checked = site.ColdFusionInstalled;
            chkVirtDir.Checked = site.CreateCFVirtualDirectories;
            
            // bind FrontPage
            if (!site.FrontPageAvailable)
            {
                litFrontPageUnavailable.Text = GetLocalizedString("Text.FPSE_UNAVAILABLE");
                litFrontPageUnavailable.Visible = true;
                tblFrontPage.Visible = false;
            }
            else
            {
                // set control policies
                frontPageUsername.SetPackagePolicy(site.PackageId, UserSettings.WEB_POLICY, "FrontPageAccountPolicy");
                frontPagePassword.SetPackagePolicy(site.PackageId, UserSettings.WEB_POLICY, "FrontPagePasswordPolicy");

                // set default account name
                frontPageUsername.Text = site.FrontPageAccount;
                ToggleFrontPageControls(site.FrontPageInstalled);
            }

            // bind controls
            webSitesHomeFolderControl.BindWebItem(PackageId, site);
            webSitesSecuredFoldersControl.BindSecuredFolders(site);
            webSitesExtensionsControl.BindWebItem(PackageId, site);
            webSitesMimeTypesControl.BindWebItem(site);
            webSitesCustomHeadersControl.BindWebItem(site);
            webSitesCustomErrorsControl.BindWebItem(site);

            BindVirtualDirectories();

            // bind state
            BindSiteState(site.SiteState);

            // bind pointers
            BindPointers();

            // save packageid
            ViewState["PackageID"] = site.PackageId;

			//
			ToggleWmSvcControls(site);
			AutoSuggestWmSvcAccontName(site);
			ToggleWmSvcConnectionHint(site);

            // bind tabs
            BindTabs();
        }

		#region WmSvc Management

		private void AutoSuggestWmSvcAccontName(WebVirtualDirectory item)
		{
			bool wmSvcItemEnabled = item.GetValue<bool>(WebVirtualDirectory.WmSvcSiteEnabled);
			//
			if (!wmSvcItemEnabled)
			{
				string autoSuggestedPart = item.Name;
				//
				if (autoSuggestedPart.Length > 14)
				{
					autoSuggestedPart = autoSuggestedPart.Substring(0, 14);
					//
					while (!String.IsNullOrEmpty(autoSuggestedPart) &&  
						!Char.IsLetterOrDigit(autoSuggestedPart[autoSuggestedPart.Length - 1]))
					{
						autoSuggestedPart = autoSuggestedPart.Substring(0, autoSuggestedPart.Length - 1);
					}
				}
				//
				txtWmSvcAccountName.Text = autoSuggestedPart + "_admin";
			}
		}

		private void ToggleWmSvcControls(WebVirtualDirectory item)
		{
			if (!item.GetValue<bool>(WebVirtualDirectory.WmSvcAvailable))
			{
				pnlWmcSvcManagement.Visible = false;
				pnlNotInstalled.Visible = true;
				//
				return;
			}
			//
			pnlWmcSvcManagement.Visible = true;
			pnlNotInstalled.Visible = false;

			//
			string wmSvcAccountName = item.GetValue<string>(WebVirtualDirectory.WmSvcAccountName);
			bool wmcSvcSiteEnabled = item.GetValue<bool>(WebVirtualDirectory.WmSvcSiteEnabled);

			btnWmSvcSiteEnable.Visible = true;
			txtWmSvcAccountName.Visible = true;

			//
			txtWmSvcAccountPassword.Text = txtWmSvcAccountPassword.Attributes["value"] = String.Empty;
			//
			txtWmSvcAccountPasswordC.Text = txtWmSvcAccountPasswordC.Attributes["value"] = String.Empty;

			// Disable edit mode if WmSvc account name is set
			if (wmcSvcSiteEnabled)
			{
				btnWmSvcSiteEnable.Visible = false;
				txtWmSvcAccountName.Visible = false;
				
				//
				txtWmSvcAccountPassword.Text = PasswordControl.EMPTY_PASSWORD;
				txtWmSvcAccountPassword.Attributes["value"] = PasswordControl.EMPTY_PASSWORD;
				
				//
				txtWmSvcAccountPasswordC.Text = PasswordControl.EMPTY_PASSWORD;
				txtWmSvcAccountPasswordC.Attributes["value"] = PasswordControl.EMPTY_PASSWORD;
			}

			//
			litWmSvcAccountName.Visible = wmcSvcSiteEnabled;
			btnWmSvcSiteDisable.Visible = wmcSvcSiteEnabled;
			btnWmSvcChangePassw.Visible = wmcSvcSiteEnabled;
			pnlWmSvcSiteDisabled.Visible = !wmcSvcSiteEnabled;
			pnlWmSvcSiteEnabled.Visible = wmcSvcSiteEnabled;

			//
			txtWmSvcAccountName.Text = wmSvcAccountName;
			litWmSvcAccountName.Text = wmSvcAccountName;
		}

		private void ToggleWmSvcConnectionHint(WebVirtualDirectory item)
		{
			bool wmcSvcSiteEnabled = item.GetValue<bool>(WebSite.WmSvcSiteEnabled);
			//
			if (wmcSvcSiteEnabled)
			{
				//
				string wmSvcServicePort = item.GetValue<String>(WebSite.WmSvcServicePort);
				string wmSvcServiceUrl = item.GetValue<String>(WebSite.WmSvcServiceUrl);
				//
				if (!String.IsNullOrEmpty(wmSvcServiceUrl))
				{
					if (!String.IsNullOrEmpty(wmSvcServicePort) 
						&& !String.Equals(wmSvcServicePort, WebSite.WmSvcDefaultPort))
						lclWmSvcConnectionHint.Text = String.Format(
							lclWmSvcConnectionHint.Text, String.Format("{0}:{1}", wmSvcServiceUrl, wmSvcServicePort), item.Name);
					else
						lclWmSvcConnectionHint.Text = String.Format(
							lclWmSvcConnectionHint.Text, wmSvcServiceUrl, item.Name);
				}
				else
					lclWmSvcConnectionHint.Visible = false;
			}
		}

		protected void btnWmSvcSiteEnable_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;
			
			//
			string accountName = txtWmSvcAccountName.Text.Trim();
			string accountPassword = txtWmSvcAccountPassword.Text;
			
			//
			ResultObject result = ES.Services.WebServers.GrantWebManagementAccess(PanelRequest.ItemID, accountName, accountPassword);
			//
			if (!result.IsSuccess)
			{
				messageBox.ShowMessage(result, "IIS7_WMSVC", "IIS7");
				return;
			}
			//
			messageBox.ShowSuccessMessage("Iis7WmSvc_Enabled");
			//
			BindWebSite();
		}

		protected void btnWmSvcChangePassw_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			//
			string accountPassword = txtWmSvcAccountPassword.Text;

			//
			ResultObject result = ES.Services.WebServers.ChangeWebManagementAccessPassword(
				PanelRequest.ItemID, accountPassword);
			//
			if (!result.IsSuccess)
			{
				messageBox.ShowMessage(result, "IIS7_WMSVC", "IIS7");
				return;
			}
			//
			messageBox.ShowSuccessMessage("Iis7WmSvc_PasswordChanged");
			//
			BindWebSite();
		}

		protected void btnWmSvcSiteDisable_Click(object sender, EventArgs e)
		{
			//
			string accountName = txtWmSvcAccountName.Text.Trim();

			//
			ES.Services.WebServers.RevokeWebManagementAccess(PanelRequest.ItemID);
			//
			messageBox.ShowSuccessMessage("Iis7WmSvc_Disabled");
			//
			BindWebSite();
		}

		#endregion

        #region FrontPage
        private void ToggleFrontPageControls(bool installed)
        {
            // status
            litFrontPageStatus.Text = installed ? GetLocalizedString("Text.FPSE_INSTALLED") : GetLocalizedString("Text.FPSE_NOT_INSTALLED");

            if (!installed)
                frontPageUsername.Text = "";

            frontPageUsername.EditMode = installed;

            // toggle buttons
            btnInstallFrontPage.Visible = !installed;
            btnUninstallFrontPage.Visible = installed;
            btnChangeFrontPagePassword.Visible = installed;
            pnlFrontPage.DefaultButton = installed ? "btnChangeFrontPagePassword" : "btnInstallFrontPage";
        }

        protected void btnInstallFrontPage_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.WebServers.InstallFrontPage(PanelRequest.ItemID,
                    frontPageUsername.Text, frontPagePassword.Password);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("WEB_FP_INSTALL");
                frontPagePassword.Password = "";
                frontPageUsername.Text = frontPageUsername.Text;
                ToggleFrontPageControls(true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_FP_INSTALL", ex);
                return;
            }
        }
        protected void btnChangeFrontPagePassword_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.WebServers.ChangeFrontPagePassword(PanelRequest.ItemID,
                    frontPagePassword.Password);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("WEB_FP_CHANGE_PASSWORD");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_FP_CHANGE_PASSWORD", ex);
                return;
            }
        }
        protected void btnUninstallFrontPage_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.WebServers.UninstallFrontPage(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("WEB_FP_UNINSTALL");
                ToggleFrontPageControls(false);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_FP_UNINSTALL", ex);
                return;
            }
        }
        #endregion

        private void BindVirtualDirectories()
        {
            gvVirtualDirectories.DataSource = ES.Services.WebServers.GetVirtualDirectories(PanelRequest.ItemID);
            gvVirtualDirectories.DataBind();
        }

        private void BindPointers()
        {
            gvPointers.DataSource = ES.Services.WebServers.GetWebSitePointers(PanelRequest.ItemID);
            gvPointers.DataBind();
        }

        private void SaveWebSite()
        {
            if (!Page.IsValid)
                return;

            // load original web site item
            WebSite site = ES.Services.WebServers.GetWebSite(PanelRequest.ItemID);

            // collect form data
            site.FrontPageAccount = frontPageUsername.Text;

            site.ColdFusionInstalled = chkCfExt.Checked;
            site.CreateCFVirtualDirectories = chkVirtDir.Checked;

            // other controls
            webSitesExtensionsControl.SaveWebItem(site);
            webSitesHomeFolderControl.SaveWebItem(site);
            webSitesMimeTypesControl.SaveWebItem(site);
            webSitesCustomHeadersControl.SaveWebItem(site);
            webSitesCustomErrorsControl.SaveWebItem(site);

            // update web site
            try
            {
                int result = ES.Services.WebServers.UpdateWebSite(site);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_UPDATE_SITE", ex);
                return;
            }

            RedirectSpaceHomePage();
        }

        private void DeleteWebSite()
        {
            try
            {
                int result = ES.Services.WebServers.DeleteWebSite(PanelRequest.ItemID);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_DELETE_SITE", ex);
                return;
            }

            RedirectSpaceHomePage();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveWebSite();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteWebSite();
        }

        protected void btnAddVirtualDirectory_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_vdir",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }

        #region Site State
        private void BindSiteState(ServerState state)
        {
            if (state == ServerState.Continuing)
                state = ServerState.Started;

            litStatus.Text = GetLocalizedString("SiteState." + state.ToString());
            cmdStart.Visible = (state == ServerState.Stopped);
            cmdContinue.Visible = (state == ServerState.Paused);
            cmdPause.Visible = (state == ServerState.Started);
            cmdStop.Visible = (state == ServerState.Started || state == ServerState.Paused);
        }

        protected void cmdChangeState_Click(object sender, ImageClickEventArgs e)
        {
            string stateName = ((ImageButton)sender).CommandName;
            ServerState state = (ServerState)Enum.Parse(typeof(ServerState), stateName, true);

            try
            {
                int result = ES.Services.WebServers.ChangeSiteState(PanelRequest.ItemID, state);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                BindSiteState(state);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_CHANGE_SITE_STATE", ex);
                return;
            }
        }
        #endregion

        #region Pointers
        protected void gvPointers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int domainId = (int)gvPointers.DataKeys[e.RowIndex][0];

            try
            {
                int result = ES.Services.WebServers.DeleteWebSitePointer(PanelRequest.ItemID, domainId);

                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                ShowSuccessMessage("WEB_DELETE_SITE_POINTER");
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_DELETE_SITE_POINTER", ex);
                return;
            }

            // rebind pointers
            BindPointers();
        }

        protected void btnAddPointer_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "add_pointer",
                PortalUtils.SPACE_ID_PARAM + "=" + PanelSecurity.PackageId.ToString()));
        }
        #endregion
    }
}

