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
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using Microsoft.Security.Application;

namespace WebsitePanel.Portal
{
    public partial class UserCreateSpace : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ftpAccountName.SetUserPolicy(PanelSecurity.SelectedUserId, UserSettings.FTP_POLICY, "UserNamePolicy");
                    BindHostingPlans(PanelSecurity.SelectedUserId);
                    BindHostingPlan();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
                this.DisableControls = true;
                //ShowErrorMessage("USERWIZARD_INIT_FORM", ex);
                return;
            }
        }

        private void BindHostingPlans(int userId)
        {
            ddlPlans.DataSource = ES.Services.Packages.GetUserAvailableHostingPlans(userId);
            ddlPlans.DataBind();

            ddlPlans.Items.Insert(0, new ListItem(GetLocalizedString("SelectHostingPlan.Text"), ""));
        }

        private void BindHostingPlan()
        {
            // plan resources
            int planId = Utils.ParseInt(ddlPlans.SelectedValue, 0);

            chkCreateResources.Visible = (planId > 0);
            bool createResources = chkCreateResources.Checked;
            ResourcesPanel.Visible = createResources & chkCreateResources.Visible;
            if (!createResources)
                return;


            bool systemEnabled = false;
            bool webEnabled = false;
            bool ftpEnabled = false;
            bool mailEnabled = false;

            // load hosting context
            if (planId > 0)
            {
                HostingPlanContext cntx = PackagesHelper.GetCachedHostingPlanContext(planId);
                if (cntx != null)
                {
                    systemEnabled = cntx.Groups.ContainsKey(ResourceGroups.Os);
                    webEnabled = cntx.Groups.ContainsKey(ResourceGroups.Web);
                    ftpEnabled = cntx.Groups.ContainsKey(ResourceGroups.Ftp);
                    mailEnabled = cntx.Groups.ContainsKey(ResourceGroups.Mail);
                }
            }

            // toggle group controls
            fsSystem.Visible = systemEnabled;

            fsWeb.Visible = webEnabled;
            chkCreateWebSite.Checked &= webEnabled;

            fsFtp.Visible = ftpEnabled;
            chkCreateFtpAccount.Checked &= ftpEnabled;

            fsMail.Visible = mailEnabled;
            chkCreateMailAccount.Checked &= mailEnabled;

            ftpAccountName.Visible = (rbFtpAccountName.SelectedIndex == 1);
        }

        private void CreateHostingSpace()
        {
            if (!Page.IsValid)
                return;

            string spaceName = ddlPlans.SelectedItem.Text;

            string ftpAccount = (rbFtpAccountName.SelectedIndex == 0) ? null : ftpAccountName.Text;

            string domainName = txtDomainName.Text.Trim();
            
            PackageResult result = null;
            try
            {
                result = ES.Services.Packages.AddPackageWithResources(PanelSecurity.SelectedUserId,
                    Utils.ParseInt(ddlPlans.SelectedValue, 0),
                    spaceName,
                    Utils.ParseInt(ddlStatus.SelectedValue, 0),
                    chkPackageLetter.Checked,
                    chkCreateResources.Checked, domainName, true, chkCreateWebSite.Checked,
                    chkCreateFtpAccount.Checked, ftpAccount, chkCreateMailAccount.Checked);

                if (result.Result < 0)
                {
                    ShowResultMessage(result.Result);
                    lblMessage.Text = AntiXss.HtmlEncode(GetExceedingQuotasMessage(result.ExceedingQuotas));
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("USERWIZARD_CREATE_ACCOUNT", ex);
                return;
            }

            // go to space home
            Response.Redirect(PortalUtils.GetSpaceHomePageUrl(result.Result));
        }

        protected void ddlPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHostingPlan();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateHostingSpace();
        }

        protected void rbFtpAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindHostingPlan();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString()));
        }

        protected void chkCreateResources_CheckedChanged(object sender, EventArgs e)
        {
            BindHostingPlan();
        }
    }
}