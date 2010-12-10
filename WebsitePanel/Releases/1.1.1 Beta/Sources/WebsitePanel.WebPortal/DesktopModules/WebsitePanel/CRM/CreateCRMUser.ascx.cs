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
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.Portal.CRM
{
    public partial class CreateCRMUser : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CRMBusinessUnitsResult res =
                        ES.Services.CRM.GetBusinessUnits(PanelRequest.ItemID, PanelSecurity.PackageId);
                    if (res.IsSuccess)
                    {
                        ddlBusinessUnits.DataSource = res.Value;
                        ddlBusinessUnits.DataValueField = "BusinessUnitId";
                        ddlBusinessUnits.DataTextField = "BusinessUnitName";
                        ddlBusinessUnits.DataBind();
                    }
                    else
                    {
                        messageBox.ShowMessage(res, "CREATE_CRM_USER", "HostedCRM");
                    }
                }
                catch(Exception ex)
                {
                    messageBox.ShowErrorMessage("CREATE_CRM_USER", ex);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int accountId = userSelector.GetAccountId();
            UserResult res;
            try
            {
                OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, accountId);
                user.AccountId = accountId;                

                res = ES.Services.CRM.CreateCRMUser(user,  PanelSecurity.PackageId, PanelRequest.ItemID, new Guid(ddlBusinessUnits.SelectedValue));
                if (res.IsSuccess)
                {

                    Response.Redirect(EditUrl("AccountID", user.AccountId.ToString(), "CRMUserRoles",
                    "SpaceID=" + PanelSecurity.PackageId,
                    "ItemID=" + PanelRequest.ItemID));                    
                }
                else
                {
                    messageBox.ShowMessage(res, "CREATE_CRM_USER", "HostedCRM");
                }
                    
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("CREATE_CRM_USER", ex);
            }
        }
    }
}