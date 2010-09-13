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

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    public partial class SettingsWebPolicy : WebsitePanelControlBase, IUserSettingsEditorControl
    {
        public void BindSettings(UserSettings settings)
        {
            // parking page
            chkAddParkingPage.Checked = Utils.ParseBool(settings["AddParkingPage"], false);
            txtPageName.Text = settings["ParkingPageName"];
            txtPageContent.Text = settings["ParkingPageContent"];

            // default documents
            if (!String.IsNullOrEmpty(settings["DefaultDocuments"]))
                txtDefaultDocs.Text = String.Join("\n", settings["DefaultDocuments"].Split(',', ';'));

            // general settings
            chkWrite.Checked = Utils.ParseBool(settings["EnableWritePermissions"], false);
            chkDirectoryBrowsing.Checked = Utils.ParseBool(settings["EnableDirectoryBrowsing"], false);
            chkParentPaths.Checked = Utils.ParseBool(settings["EnableParentPaths"], false);
            chkDedicatedPool.Checked = Utils.ParseBool(settings["EnableDedicatedPool"], false);

            chkAuthAnonymous.Checked = Utils.ParseBool(settings["EnableAnonymousAccess"], false);
            chkAuthWindows.Checked = Utils.ParseBool(settings["EnableWindowsAuthentication"], false);
            chkAuthBasic.Checked = Utils.ParseBool(settings["EnableBasicAuthentication"], false);

            // extensions
            chkAsp.Checked = Utils.ParseBool(settings["AspInstalled"], false);
            Utils.SelectListItem(ddlAspNet, settings["AspNetInstalled"]);
            Utils.SelectListItem(ddlPhp, settings["PhpInstalled"]);
            chkPerl.Checked = Utils.ParseBool(settings["PerlInstalled"], false);
            chkPython.Checked = Utils.ParseBool(settings["PythonInstalled"], false);
            chkCgiBin.Checked = Utils.ParseBool(settings["CgiBinInstalled"], false);

            // anonymous account policy
            anonymousUsername.Value = settings["AnonymousAccountPolicy"];

            // virtual directories
            virtDirName.Value = settings["VirtDirNamePolicy"];

            // FrontPage
            frontPageUsername.Value = settings["FrontPageAccountPolicy"];
            frontPagePassword.Value = settings["FrontPagePasswordPolicy"];

			// secured folders
			securedUserNamePolicy.Value = settings["SecuredUserNamePolicy"];
			securedUserPasswordPolicy.Value = settings["SecuredUserPasswordPolicy"];
			securedGroupNamePolicy.Value = settings["SecuredGroupNamePolicy"];

            // folders
            txtSiteRootFolder.Text = settings["WebRootFolder"];
            txtSiteLogsFolder.Text = settings["WebLogsFolder"];
            txtSiteDataFolder.Text = settings["WebDataFolder"];
            chkAddRandomDomainString.Checked = Utils.ParseBool(settings["AddRandomDomainString"], false);
        }

        public void SaveSettings(UserSettings settings)
        {
            // parking page
            settings["AddParkingPage"] = chkAddParkingPage.Checked.ToString();
            settings["ParkingPageName"] = txtPageName.Text;
            settings["ParkingPageContent"] = txtPageContent.Text;

            // default documents
            settings["DefaultDocuments"] = String.Join(",", Utils.ParseDelimitedString(txtDefaultDocs.Text, '\n', '\r', ';', ',')); ;

            // general settings
            settings["EnableWritePermissions"] = chkWrite.Checked.ToString();
            settings["EnableDirectoryBrowsing"] = chkDirectoryBrowsing.Checked.ToString();
            settings["EnableParentPaths"] = chkParentPaths.Checked.ToString();
            settings["EnableDedicatedPool"] = chkDedicatedPool.Checked.ToString();

            settings["EnableAnonymousAccess"] = chkAuthAnonymous.Checked.ToString();
            settings["EnableWindowsAuthentication"] = chkAuthWindows.Checked.ToString();
            settings["EnableBasicAuthentication"] = chkAuthBasic.Checked.ToString();

            // extensions
            settings["AspInstalled"] = chkAsp.Checked.ToString();
            settings["AspNetInstalled"] = ddlAspNet.SelectedValue;
            settings["PhpInstalled"] = ddlPhp.SelectedValue;
            settings["PerlInstalled"] = chkPerl.Checked.ToString();
            settings["PythonInstalled"] = chkPython.Checked.ToString();
            settings["CgiBinInstalled"] = chkCgiBin.Checked.ToString();

            // anonymous account policy
            settings["AnonymousAccountPolicy"] = anonymousUsername.Value;

            // virtual directories
            settings["VirtDirNamePolicy"] = virtDirName.Value;

            // FrontPage
            settings["FrontPageAccountPolicy"] = frontPageUsername.Value;
            settings["FrontPagePasswordPolicy"] = frontPagePassword.Value;

			// secured folders
			settings["SecuredUserNamePolicy"] = securedUserNamePolicy.Value;
			settings["SecuredUserPasswordPolicy"] = securedUserPasswordPolicy.Value;
			settings["SecuredGroupNamePolicy"] = securedGroupNamePolicy.Value;

            // folders
            settings["WebRootFolder"] = txtSiteRootFolder.Text;
            settings["WebLogsFolder"] = txtSiteLogsFolder.Text;
            settings["WebDataFolder"] = txtSiteDataFolder.Text;
            settings["AddRandomDomainString"] = chkAddRandomDomainString.Checked.ToString();
        }
    }
}