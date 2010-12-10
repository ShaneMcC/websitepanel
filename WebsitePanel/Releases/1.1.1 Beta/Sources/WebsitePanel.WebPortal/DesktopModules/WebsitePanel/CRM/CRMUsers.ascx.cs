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
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal.CRM
{
    public partial class CRMUsers : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Organization org = ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);
            if (org.CrmOrganizationId == Guid.Empty)
            {
                messageBox.ShowErrorMessage("NOT_CRM_ORGANIZATION");
                btnCreateUser.Enabled = false;
            }
            else
            {
                OrganizationStatistics stats = ES.Services.Organizations.GetOrganizationStatistics(PanelRequest.ItemID);
                int allocatedCrmUsers = stats.AllocatedCRMUsers;
                int usedUsers = stats.CreatedCRMUsers;
                usersQuota.QuotaUsedValue = usedUsers;
                usersQuota.QuotaValue = allocatedCrmUsers;
                
            }
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "create_crm_user",
                "SpaceID=" + PanelSecurity.PackageId));
        }

        public string GetAccountImage(int accountTypeId)
        {
            
            return GetThemedImage("Exchange/admin_16.png");
        }

        public string GetUserEditUrl(string accountId)
        {
            return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "CRMUserRoles",
                    "AccountID=" + accountId,
                    "ItemID=" + PanelRequest.ItemID);
        }

      

        protected void odsAccountsPaged_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {

        }

    }
}