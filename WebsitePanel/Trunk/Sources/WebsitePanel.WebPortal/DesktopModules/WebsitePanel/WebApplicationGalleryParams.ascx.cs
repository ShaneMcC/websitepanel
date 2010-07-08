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
using WebsitePanel.Providers.Database;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace WebsitePanel.Portal
{
    public partial class WebApplicationGalleryParams : WebsitePanelModuleBase
    {
        private const DeploymentParameterWellKnownTag databaseEngineTags =
                    DeploymentParameterWellKnownTag.Sql |
                    DeploymentParameterWellKnownTag.MySql |
                    DeploymentParameterWellKnownTag.SqLite |
                    DeploymentParameterWellKnownTag.VistaDB |
                    DeploymentParameterWellKnownTag.FlatFile;

        private GalleryApplicationResult appResult;

		protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            try
            {
                // load application details
                appResult = ES.Services.WebApplicationGallery.GetGalleryApplicationDetails(PanelSecurity.PackageId, PanelRequest.ApplicationID);
                
                if (!appResult.IsSuccess)
                {
                    messageBox.ShowMessage(appResult, "WAG_NOT_AVAILABLE", "WebAppGallery");
                    return;
                }

                // bind app details
                appHeader.BindApplicationDetails(appResult.Value);

                // check for warnings
                if (appResult.ErrorCodes.Count > 0)
                {
                    // app does not meet requirements
                    messageBox.ShowMessage(appResult, "WAG_CANNOT_INSTALL_APPLICATION", "WebAppGallery");
                    btnInstall.Enabled = false;
                }

                // bind app parameters
                List<DeploymentParameter> parameters = GetApplicationParameters();
                if (parameters == null)
                    return;

				//
                BindWebSites();
                BindDatabaseEngines(parameters);                
            }
            catch (Exception ex)
            {
                ShowErrorMessage("WAG_NOT_AVAILABLE", ex);
                DisableForm();
            }
        }

        private List<DeploymentParameter> GetApplicationParameters()
        {
            DeploymentParametersResult result =
                        ES.Services.WebApplicationGallery.GetGalleryApplicationParams(PanelSecurity.PackageId, PanelRequest.ApplicationID);
            
            // check results
            if (!result.IsSuccess)
            {
                messageBox.ShowMessage(result, "WAG_NOT_AVAILABLE", "WebAppGallery");
                DisableForm();
                return null;
            }

            // return params
            return result.Value;
        }

        private void DisableForm()
        {
            btnInstall.Enabled = false;
            secAppSettings.Visible = false;
            urlPanel.Visible = false;
            tempUrlPanel.Visible = false;
        }
        
        private void BindWebSites()
        {            
            ddlWebSite.DataSource = ES.Services.WebServers.GetWebSites(PanelSecurity.PackageId, false);
            ddlWebSite.DataBind();
            ddlWebSite.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectWebSite"), ""));

            // apply policy to virtual dirs
            directoryName.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.WEB_POLICY, "VirtDirNamePolicy");
        }

        private void BindDatabaseEngines(List<DeploymentParameter> parameters)
        {
            // SQL Server
            if (FindParameterByTag(parameters, DeploymentParameterWellKnownTag.Sql) != null)
            {
                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                // add SQL Server engines
                if (cntx.Groups.ContainsKey(ResourceGroups.MsSql2008))
                    AddDatabaseEngine(DeploymentParameterWellKnownTag.Sql, ResourceGroups.MsSql2008, GetSharedLocalizedString("ResourceGroup." + ResourceGroups.MsSql2008));
                if (cntx.Groups.ContainsKey(ResourceGroups.MsSql2005))
                    AddDatabaseEngine(DeploymentParameterWellKnownTag.Sql, ResourceGroups.MsSql2005, GetSharedLocalizedString("ResourceGroup." + ResourceGroups.MsSql2005));
                if (cntx.Groups.ContainsKey(ResourceGroups.MsSql2000))
                    AddDatabaseEngine(DeploymentParameterWellKnownTag.Sql, ResourceGroups.MsSql2000, GetSharedLocalizedString("ResourceGroup." + ResourceGroups.MsSql2000));
            }

            // MySQL Server
            if (FindParameterByTag(parameters, DeploymentParameterWellKnownTag.MySql) != null)
            {
                // load package context
                PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

                // add SQL Server engines
                if (cntx.Groups.ContainsKey(ResourceGroups.MySql5))
                    AddDatabaseEngine(DeploymentParameterWellKnownTag.MySql, ResourceGroups.MySql5, GetSharedLocalizedString("ResourceGroup." + ResourceGroups.MySql5));
                if (cntx.Groups.ContainsKey(ResourceGroups.MySql4))
                    AddDatabaseEngine(DeploymentParameterWellKnownTag.MySql, ResourceGroups.MySql4, GetSharedLocalizedString("ResourceGroup." + ResourceGroups.MySql4));
            }

            // SQLite
            if (FindParameterByTag(parameters, DeploymentParameterWellKnownTag.SqLite) != null)
                AddDatabaseEngine(DeploymentParameterWellKnownTag.SqLite, "", GetLocalizedString("DatabaseEngine.SQLite"));

            // VistaFB
            if (FindParameterByTag(parameters, DeploymentParameterWellKnownTag.VistaDB) != null)
                AddDatabaseEngine(DeploymentParameterWellKnownTag.VistaDB, "", GetLocalizedString("DatabaseEngine.VistaDB"));

            // Flat File
            if (FindParameterByTag(parameters, DeploymentParameterWellKnownTag.FlatFile) != null)
                AddDatabaseEngine(DeploymentParameterWellKnownTag.FlatFile, "", GetLocalizedString("DatabaseEngine.FlatFile"));

            // hide module if no database required
            divDatabase.Visible = (databaseEngines.Items.Count > 0);

            // bind parameters
            BindParameters(parameters);
        }

        private void AddDatabaseEngine(DeploymentParameterWellKnownTag engine, string group, string text)
        {
            databaseEngines.Items.Add(new ListItem(text, String.Format("{0},{1}", engine, group)));
        }

        protected void databaseEngines_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindParameters();
        }

        protected void databaseMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindParameters();
        }

        private void BindParameters()
        {
            // load parameters
            List<DeploymentParameter> parameters = GetApplicationParameters();
            if (parameters == null)
                return;

            // bind parameters
            BindParameters(parameters);
        }

        private void BindParameters(List<DeploymentParameter> parameters)
        {
            if (databaseEngines.Items.Count > 0)
            {
                // filter parameters by database engine
                DeploymentParameterWellKnownTag engine = GetSelectedDatabaseEngine();
                string resourceGroup = GetSelectedDatabaseResourceGroup();

                // remove parameters for other engines
                parameters.RemoveAll(delegate(DeploymentParameter p)
                {
                    return (p.WellKnownTags & databaseEngineTags) > 0
                        && (p.WellKnownTags & engine) != engine;
                });

                // hide database mode for file-based engines
                databaseModeBlock.Visible = !String.IsNullOrEmpty(resourceGroup);
            }

            // bind
            repParams.DataSource = parameters;
            repParams.DataBind();

            // bind existing databases and user
            BindExistingDatabases();
        }

        private void BindExistingDatabases()
        {
            string resourceGroup = GetSelectedDatabaseResourceGroup();
            if (String.IsNullOrEmpty(resourceGroup))
                return; // application does not require database

            // databases
            WebApplicationGalleryParamControl databaseControl = FindParameterControlByTag(DeploymentParameterWellKnownTag.DBName);
            if (databaseControl != null && databaseMode.SelectedValue == "existing")
            {
                // bind databases
                SqlDatabase[] databases = ES.Services.DatabaseServers.GetSqlDatabases(PanelSecurity.PackageId, resourceGroup, false);

                // disable regexp validator
                databaseControl.ValidationKind &= ~DeploymentParameterValidationKind.RegularExpression;

                // enable enumeration
                List<string> databaseNames = new List<string>();
                databaseNames.Add(""); // add empty database
                foreach (SqlDatabase database in databases)
                    databaseNames.Add(database.Name);

                databaseControl.ValidationKind |= DeploymentParameterValidationKind.Enumeration;
                databaseControl.ValidationString = String.Join(",", databaseNames.ToArray());

                // fill users list
                WebApplicationGalleryParamControl userControl = FindParameterControlByTag(DeploymentParameterWellKnownTag.DBUserName);
                if (userControl != null)
                {
                    SqlUser[] users = ES.Services.DatabaseServers.GetSqlUsers(PanelSecurity.PackageId, resourceGroup, false);

                    // disable regexp validator
                    userControl.ValidationKind &= ~DeploymentParameterValidationKind.RegularExpression;

                    // enable enumeration
                    List<string> userNames = new List<string>();
                    userNames.Add(""); // add empty username
                    foreach (SqlUser user in users)
                        userNames.Add(user.Name);

                    userControl.ValidationKind |= DeploymentParameterValidationKind.Enumeration;
                    userControl.ValidationString = String.Join(",", userNames.ToArray());

                    // username password
                    WebApplicationGalleryParamControl userPasswordControl = FindParameterControlByTag(DeploymentParameterWellKnownTag.DBUserPassword);
                    if (userPasswordControl != null)
                    {
                        userPasswordControl.WellKnownTags &= ~DeploymentParameterWellKnownTag.New;
                    }
                }
            }
        }

        protected void repParams_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            WebApplicationGalleryParamControl paramControl = e.Item.FindControl("param") as WebApplicationGalleryParamControl;
            if (paramControl != null)
                paramControl.BindParameter(e.Item.DataItem as DeploymentParameter);
        }

        private DeploymentParameter FindParameterByTag(List<DeploymentParameter> parameters, DeploymentParameterWellKnownTag tag)
        {
            return parameters.Find(delegate(DeploymentParameter p)
            {
                return ((p.WellKnownTags & tag) == tag);
            });
        }

        private WebApplicationGalleryParamControl FindParameterControlByTag(DeploymentParameterWellKnownTag tag)
        {
            foreach (RepeaterItem item in repParams.Items)
            {
                WebApplicationGalleryParamControl paramControl = item.FindControl("param") as WebApplicationGalleryParamControl;
                if (paramControl != null && (paramControl.WellKnownTags & tag) == tag)
                    return paramControl;
            }
            return null;
        }

        private DeploymentParameterWellKnownTag GetSelectedDatabaseEngine()
        {
            string val = databaseEngines.SelectedValue;
            return String.IsNullOrEmpty(val)
                ? DeploymentParameterWellKnownTag.None
                : (DeploymentParameterWellKnownTag)Enum.Parse(typeof(DeploymentParameterWellKnownTag), val.Split(',')[0]);
        }

        private string GetSelectedDatabaseResourceGroup()
        {
            string val = databaseEngines.SelectedValue.Trim();
            return val != "" ? val.Split(',')[1] : val;
        }

        private List<DeploymentParameter> GetParameters()
        {
            // get the information about selected database engine
            string resourceGroup = GetSelectedDatabaseResourceGroup();
            DeploymentParameterWellKnownTag databaseEngine = GetSelectedDatabaseEngine();

            // collect parameters
            List<DeploymentParameter> parameters = new List<DeploymentParameter>();
            foreach (RepeaterItem item in repParams.Items)
            {
                WebApplicationGalleryParamControl paramControl = item.FindControl("param") as WebApplicationGalleryParamControl;
                if (paramControl != null)
                {
                    // store parameter in the collection
                    DeploymentParameter param = paramControl.GetParameter();
                    parameters.Add(param);

                    // set database engine flag
                    param.WellKnownTags &= ~databaseEngineTags; // reset all database flags
                    param.WellKnownTags |= databaseEngine; // set seleced
                }
            }

            // add the information about selected resource group
            if (!String.IsNullOrEmpty(resourceGroup))
            {
                parameters.Add(new DeploymentParameter()
                {
                    Name = DeploymentParameter.ResourceGroupParameterName,
                    Value = resourceGroup,
                    WellKnownTags = databaseEngine
                });
            }

            return parameters;
        }

        private void InstallApplication()
        {
            if (!Page.IsValid)
                return; // server-side validation

            // collect parameters       
            List<DeploymentParameter> parameters = GetParameters();

            // install application
            ResultObject res = ES.Services.WebApplicationGallery.Install(PanelSecurity.PackageId,
                                                                         PanelRequest.ApplicationID,
                                                                         ddlWebSite.SelectedItem.Text,
                                                                         directoryName.Text.Trim(),
                                                                         parameters.ToArray());

            // show message box with results
            messageBox.ShowMessage(res, "WEB_APPLICATION_GALLERY_INSTALLED", "WebAppGallery");

            // toggle controls if there are no errors
            if (res.ErrorCodes.Count == 0)
            {
                secAppSettings.Visible = false;
                btnCancel.Visible = false;
                btnInstall.Visible = false;
                btnOK.Visible = true;
                urlPanel.Visible = true;
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
                        // change "Launch" link
                        hlApplication.Text = String.Format(GetLocalizedString("LaunchApplication.Text"), application.Title);

                        // set "main" application URL
                        hlApplication.NavigateUrl = GetAppLaunchUrl(application, ddlWebSite.SelectedItem.Text);

                        // set "temp" application URL if required
                        DomainInfo[] domains = ES.Services.WebServers.GetWebSitePointers(Int32.Parse(ddlWebSite.SelectedValue));
                        foreach (DomainInfo domain in domains)
                        {
                            if (domain.IsInstantAlias)
                            {
                                // show temp URL
                                tempUrlPanel.Visible = true;

                                // set URL text
                                tempUrl.Text = String.Format(GetLocalizedString("LaunchApplicationTemp.Text"), GetAppLaunchUrl(application, domain.DomainName));
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("GET_GALLERY_APPLIACTION_DETAILS", ex);
                }
            }
        }

        private string GetAppLaunchUrl(GalleryApplication app, string siteUrl)
        {
            // fix app start page
            string startPage = app.StartPage != null ? app.StartPage.Replace('\\', '/').TrimStart('/') : "";

            // build URL
            string url = "http://" + siteUrl;

            // append virtual dir
            if (directoryName.Text != "")
                url += "/" + directoryName.Text;

            // append start page and return
            return url + "/" + startPage;
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
    }
}