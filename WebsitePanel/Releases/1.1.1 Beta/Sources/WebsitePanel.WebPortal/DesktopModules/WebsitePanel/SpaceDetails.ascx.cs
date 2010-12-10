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
    public partial class SpaceDetails : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindSpace();
        }

        private void BindSpace()
        {
            // load space
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package != null)
            {
                litSpaceName.Text = package.PackageName;

                // bind space status
                PackageStatus status = (PackageStatus)package.StatusId;
                litStatus.Text = PanelFormatter.GetPackageStatusName(package.StatusId);

                cmdActive.Visible = (status != PackageStatus.Active);
                cmdSuspend.Visible = (status == PackageStatus.Active);
                cmdCancel.Visible = (status != PackageStatus.Cancelled);

                StatusBlock.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);

                // bind account details
                litCreated.Text = package.PurchaseDate.ToString();
                serverDetails.ServerId = package.ServerId;

                // load plan
                HostingPlanInfo plan = ES.Services.Packages.GetHostingPlan(package.PlanId);
                if (plan != null)
                    litHostingPlan.Text = plan.PlanName;

                // links
                lnkSummaryLetter.NavigateUrl = EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "summary_letter");
                lnkSummaryLetter.Visible = (PanelSecurity.PackageId > 1);

				lnkOverusageReport.NavigateUrl = NavigatePageURL("OverusageReport", PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString());
				OverusageReport.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);

                lnkEditSpaceDetails.NavigateUrl = EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "edit_details");

				bool ownSpace = (package.UserId == PanelSecurity.EffectiveUserId);
                lnkEditSpaceDetails.Visible = (PanelSecurity.PackageId > 1 && !ownSpace);

                lnkDelete.NavigateUrl = EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "delete");
                lnkDelete.Visible = ((PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId)
                    && (PanelSecurity.PackageId > 1));
            }
        }

        protected void statusButton_Click(object sender, ImageClickEventArgs e)
        {
            string sStatus = ((ImageButton)sender).CommandName;
            PackageStatus status = (PackageStatus)Enum.Parse(typeof(PackageStatus), sStatus, true);

            // chanhe user status
            try
            {
                int result = ES.Services.Packages.ChangePackageStatus(PanelSecurity.PackageId, status);
                if (result < 0)
                {
                    ShowResultMessage(result);
                    return;
                }

                BindSpace();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("PACKAGE_CHANGE_STATUS", ex);
                return;
            }
        }
    }
}