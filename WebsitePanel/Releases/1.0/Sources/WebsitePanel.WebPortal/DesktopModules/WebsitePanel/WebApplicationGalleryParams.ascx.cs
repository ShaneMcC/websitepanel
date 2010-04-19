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

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.WebAppGallery;
using WebsitePanel.Providers.ResultObjects;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace WebsitePanel.Portal
{
    public partial class WebApplicationGalleryParams : WebsitePanelModuleBase
    {
		private GalleryApplicationResult webAppResult;

		#region Adaptive parameters naming
		protected string _DATABASE_USERNAME_PARAM
		{
			get { return (string)ViewState["_DATABASE_USERNAME_PARAM"]; }
			set { ViewState["_DATABASE_USERNAME_PARAM"] = value; }
		}

		protected string _DATABASE_USERNAMEPWD_PARAM
		{
			get { return (string)ViewState["_DATABASE_USERNAMEPWD_PARAM"]; }
			set { ViewState["_DATABASE_USERNAMEPWD_PARAM"] = value; }
		}

		protected string _DATABASE_NAME_PARAM
		{
			get { return (string)ViewState["_DATABASE_NAME_PARAM"]; }
			set { ViewState["_DATABASE_NAME_PARAM"] = value; }
		}
		#endregion

		protected void Page_Load(object sender, EventArgs e)
        {                        
            try
            {
				webAppResult = ES.Services.WebApplicationGallery.GetGalleryApplicationDetails(PanelSecurity.PackageId, PanelRequest.ApplicationID);
				//
				if (!webAppResult.IsSuccess)
				{
					messageBox.ShowMessage(webAppResult, "WAG_NOT_AVAILABLE", "ModuleWAG");
					return;
				}
				//
                if (!IsPostBack)
                {
                    DeploymentParametersResult appParamsResult = 
						ES.Services.WebApplicationGallery.GetGalleryApplicationParams(PanelSecurity.PackageId, PanelRequest.ApplicationID);
					//
					if (!appParamsResult.IsSuccess)
					{
						messageBox.ShowMessage(appParamsResult, "WAG_NOT_AVAILABLE", "ModuleWAG");
						return;
					}
					//
                    BindWebSites();
                    BindDatabaseVersions(appParamsResult.Value);
                    BindParams(appParamsResult.Value);
                    ToggleControls();                    
                }                                
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WEB_GALLERY_INSTALLER_INIT_FORM", ex);                
            }
        }

        private void BindParams(List<DeploymentParameter> parameters)
        {
            List<DeploymentParameter> resParameters = new List<DeploymentParameter>();
            foreach(DeploymentParameter parameter in parameters)
            {
				// Match database name parameter
				if(MatchParameterByNames(parameter, DeploymentParameter.DATABASE_NAME_PARAMS) 
					|| MatchParameterTag(parameter, DeploymentParameter.DB_NAME_PARAM_TAG))
				{
					_DATABASE_NAME_PARAM = parameter.Name;
					continue;
				}
				// Match database username parameter
				if(MatchParameterByNames(parameter, DeploymentParameter.DATABASE_USERNAME_PARAMS)
					|| MatchParameterTag(parameter, DeploymentParameter.DB_USERNAME_PARAM_TAG))
				{
					_DATABASE_USERNAME_PARAM = parameter.Name;
					continue;
				}
				// Match database user password parameter
				if (MatchParameterByNames(parameter, DeploymentParameter.DATABASE_USERPWD_PARAMS)
					|| MatchParameterTag(parameter, DeploymentParameter.DB_PASSWORD_PARAM_TAG))
				{
					_DATABASE_USERNAMEPWD_PARAM = parameter.Name;
					continue;
				}

                //
                resParameters.Add(parameter);
            }

            if (resParameters.Count > 0)
            {
                gvParams.DataSource = resParameters;
                gvParams.DataKeyNames = new[] {"Name"};
                gvParams.DataBind();
            }
            else
            {
                secAppSettings.Visible = false;        
            }

        }
        
        private void BindWebSites()
        {            
            ddlWebSite.DataSource = ES.Services.WebServers.GetWebSites(PanelSecurity.PackageId, false);
            ddlWebSite.DataBind();
            ddlWebSite.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectWebSite"), ""));

            // apply policy to virtual dirs
            directoryName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "VirtDirNamePolicy");
        }

        private void BindDatabaseVersions(List<DeploymentParameter> parameters)
        {
            if (parameters == null)
                return;

            List<string> versions = new List<string>();
                
            foreach (DeploymentParameter current in parameters)
            {
                if (string.IsNullOrEmpty(current.Tags))
                    continue;
                
                string[] tags = current.Tags.ToLowerInvariant().Split(',');
                foreach (string tag in tags)
                {
                    if (tag.Trim().ToLower() == DeploymentParameter.SQL_PARAM_TAG.ToLower())
                    {
                        versions.Add(ResourceGroups.MsSql2000);
                        versions.Add(ResourceGroups.MsSql2005);
                        versions.Add(ResourceGroups.MsSql2008);
                    }
                    else if (tag.Trim().ToLower() == DeploymentParameter.MYSQL_PARAM_TAG.ToLower())
                    {
                        versions.Add(ResourceGroups.MySql4);
                        versions.Add(ResourceGroups.MySql5);
                    }
                }
            }
            
            // fill databases box
            FillDatabaseVersions(PanelSecurity.PackageId, ddlDatabaseGroup.Items, versions);

            // hide module if required
            divDatabase.Visible = (ddlDatabaseGroup.Items.Count > 0);

            
            BindDatabases();
            BindDatabaseUsers();
            ApplyDatabasePolicy();
        }

        private void BindDatabases()
        {
            if (ddlDatabaseGroup.Items.Count == 0)
                return; // no database required

            ddlDatabase.DataSource = ES.Services.DatabaseServers.GetSqlDatabases(PanelSecurity.PackageId, ddlDatabaseGroup.SelectedValue, false);
            ddlDatabase.DataBind();
            ddlDatabase.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDatabase"), ""));
        }

        private void BindDatabaseUsers()
        {
            if (ddlDatabaseGroup.Items.Count == 0)
                return; // no database required

            ddlUser.DataSource = ES.Services.DatabaseServers.GetSqlUsers(PanelSecurity.PackageId, ddlDatabaseGroup.SelectedValue, false);
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectUser"), ""));
        }

        private void ApplyDatabasePolicy()
        {
            string groupName = ddlDatabaseGroup.SelectedValue;
            if (groupName == null)
                return;

            string settingsName = UserSettings.MYSQL_POLICY;
            if (groupName.ToLower().StartsWith("mssql"))
                settingsName = UserSettings.MSSQL_POLICY;

            databaseName.SetPackagePolicy(PanelSecurity.PackageId, settingsName, "DatabaseNamePolicy");
            databaseUser.SetPackagePolicy(PanelSecurity.PackageId, settingsName, "UserNamePolicy");
            databasePassword.SetPackagePolicy(PanelSecurity.PackageId, settingsName, "UserPasswordPolicy");
        }

        private void ToggleControls()
        {
            tblNewDatabase.Visible = (rblDatabase.SelectedIndex == 0);
            tblExistingDatabase.Visible = (rblDatabase.SelectedIndex != 0);
            rowNewUser.Visible = (rblUser.SelectedIndex == 0);
            rowExistingUser.Visible = (rblUser.SelectedIndex != 0);
			//
			if (!String.IsNullOrEmpty(webAppResult.Value.StartPage))
				pnlVirtualDir.Visible =  !Path.IsPathRooted(webAppResult.Value.StartPage);
        }

        private void InstallApplication()
        {                   
            List<DeploymentParameter> parameters = GetParameters();

            ResultObject res = ES.Services.WebApplicationGallery.Install(PanelSecurity.PackageId,
                                                                         PanelRequest.ApplicationID, 
                                                                         ddlWebSite.SelectedItem.Text, 
                                                                         directoryName.Text, 
                                                                         parameters.ToArray());

            messageBox.ShowMessage(res, "WEB_APPLICATION_GALLERY_INSTALLED", "INSTALL_WEB_APPLICATION");
            if (res.IsSuccess)
            {
                secAppSettings.Visible = false;
                secDatabase.Visible = false;
                secLocation.Visible = false;
                btnCancel.Visible = false;
                btnInstall.Visible = false;
                btnOK.Visible = true;
                applicationUrl.Visible = true;
                try
                {
                    GalleryApplicationResult appResult =
                        ES.Services.WebApplicationGallery.GetGalleryApplicationDetails(PanelSecurity.PackageId,
                                                                                       PanelRequest.ApplicationID);
					//
					GalleryApplication application = appResult.Value;
					//
                    if (application != null)
                    {
                        string url = "http://" + ddlWebSite.SelectedItem.Text;

                        url = Path.Combine(url, directoryName.Text);
						if (Path.IsPathRooted(application.StartPage))
						{
							if (application.StartPage != "/")
								url = Path.Combine(url, application.StartPage.Substring(1));
						}
						else
						{
							url = Path.Combine(url, application.StartPage);
						}
                        
                        hlApplication.NavigateUrl = url;
                    }
                }
                catch(Exception ex)
                {
                    ShowErrorMessage("GET_GALLERY_APPLIACTION_DETAILS", ex);
                }

            }            
        }

        private List<DeploymentParameter> GetParameters()
        {
            List<DeploymentParameter> parameters = new List<DeploymentParameter>();
            foreach (GridViewRow row in gvParams.Rows)
            {                
                string name = string.Empty;
                DataKey key = gvParams.DataKeys[row.DataItemIndex];                
                
                if (key != null)
                    name = key.Value as string;

                TextBox txt = row.FindControl("txtParamValue") as TextBox;

                if (!String.IsNullOrEmpty(name) && txt != null)
                {
                    DeploymentParameter param = new DeploymentParameter();
                    param.Name = name;
                    param.Value = txt.Text;
                    parameters.Add(param);
                }
            }

            DeploymentParameter userName = new DeploymentParameter();
            userName.Name = _DATABASE_USERNAME_PARAM;
            userName.Value = rblUser.SelectedValue == "New" ? databaseUser.Text : ddlUser.SelectedItem.Text;

            DeploymentParameter password = new DeploymentParameter();
            password.Name = _DATABASE_USERNAMEPWD_PARAM;
            password.Value = databasePassword.Password;


            DeploymentParameter database = new DeploymentParameter();
            database.Name = _DATABASE_NAME_PARAM;
            database.Value = rblDatabase.SelectedValue == "New" ? databaseName.Text : ddlDatabase.SelectedItem.Text;
            database.Tags = ddlDatabaseGroup.SelectedValue;

            
            if (!String.IsNullOrEmpty(userName.Name) && !String.IsNullOrEmpty(userName.Value))
                parameters.Add(userName);
            
            if (!String.IsNullOrEmpty(password.Name) && !String.IsNullOrEmpty(password.Value))
                parameters.Add(password);

            if (!String.IsNullOrEmpty(database.Name) && !String.IsNullOrEmpty(database.Value))
                parameters.Add(database);


            return parameters;
        }

        protected void ddlDatabaseGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDatabases();
            BindDatabaseUsers();
            ApplyDatabasePolicy();
        }

        protected void rblDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleControls();
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
            InstallApplication();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

		static Dictionary<Regex, TextBoxMode> ParamsTextModes = new Dictionary<Regex, TextBoxMode>
		{
			// Match for password word to display text field in the corresponding mode
			{ new Regex (@"\bpassword\b", RegexOptions.CultureInvariant|RegexOptions.IgnoreCase), TextBoxMode.Password }
		};

		private bool MatchParameterByNames<T>(T param, string[] nameMatches)
		{
			foreach (var nameMatch in nameMatches)
			{
				if (MatchParameterName<T>(param, nameMatch))
					return true;
			}
			//
			return false;
		}

		private bool MatchParameterName<T>(T param, string nameMatch)
		{
			if (param == null || String.IsNullOrEmpty(nameMatch))
				return false;
			//
			Type paramTypeOf = typeof(T);
			//
			PropertyInfo namePropInfo = paramTypeOf.GetProperty("Name");
			//
			String paramName = Convert.ToString(namePropInfo.GetValue(param, null));
			//
			if (String.IsNullOrEmpty(paramName))
				return false;
			// Compare for tag name match
			return (paramName.ToLowerInvariant() == nameMatch.ToLowerInvariant());
		}

		private bool MatchParameterTag<T>(T param, string tagMatch)
		{
			if (param == null || String.IsNullOrEmpty(tagMatch))
				return false;
			//
			Type paramTypeOf = typeof(T);
			//
			PropertyInfo tagsPropInfo = paramTypeOf.GetProperty("Tags");
			//
			String strTags = Convert.ToString(tagsPropInfo.GetValue(param, null));
			//
			if (String.IsNullOrEmpty(strTags))
				return false;
			// Lookup for specific tags within the parameter
			return Array.Exists<string>(strTags.ToLowerInvariant().Split(','), x => x.Trim() == tagMatch.ToLowerInvariant());
		}

		// Matches only for the first entry 
		public TextBoxMode GetFieldTextMode(string name)
		{
			foreach (KeyValuePair<Regex, TextBoxMode> kvp in ParamsTextModes)
			{
				if (kvp.Key.IsMatch(name))
				{
					return kvp.Value;
				}
			}
			// SingleLine by default
			return TextBoxMode.SingleLine;
		}
    }
}