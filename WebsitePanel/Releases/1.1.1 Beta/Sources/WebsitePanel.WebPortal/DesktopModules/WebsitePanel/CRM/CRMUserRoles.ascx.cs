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
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.Portal.CRM
{
    public partial class CRMUserRoles : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    OrganizationUser user =
                        ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID, PanelRequest.AccountID);

                    CrmUserResult userResult = ES.Services.CRM.GetCrmUser(PanelRequest.ItemID, PanelRequest.AccountID);

                    if (userResult.IsSuccess)
                    {
                        btnActive.Visible = userResult.Value.IsDisabled;
                        locEnabled.Visible = !userResult.Value.IsDisabled;

                        btnDeactivate.Visible = !userResult.Value.IsDisabled;
                        locDisabled.Visible = userResult.Value.IsDisabled;
                        lblDisplayName.Text = user.DisplayName;
                        lblEmailAddress.Text = user.PrimaryEmailAddress;
                        lblDomainName.Text = user.DomainUserName;
                    }
                    else
                    {
                        messageBox.ShowMessage(userResult, "GET_CRM_USER", "HostedCRM");
                        return;
                    }
                    
                    CrmRolesResult res =
                        ES.Services.CRM.GetCrmRoles(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId);

                    if (res.IsSuccess)
                    {
                        gvRoles.DataSource = res.Value;
                        gvRoles.DataBind();
                    }
                    else
                    {
                        messageBox.ShowMessage(res, "GET_CRM_USER_ROLES", "HostedCRM");
                    }
                }
                catch(Exception ex)
                {
                    messageBox.ShowErrorMessage("GET_CRM_USER_ROLES", ex);
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                List<Guid> roles = new List<Guid>();
                foreach (GridViewRow row in gvRoles.Rows)
                {
                    CheckBox cbSelected = row.FindControl("cbSelected") as CheckBox;
                    string str = gvRoles.DataKeys[row.DataItemIndex].Value.ToString();
                    if (cbSelected != null && cbSelected.Checked)
                        roles.Add(new Guid(str));
                }


                ResultObject res =
                    ES.Services.CRM.SetUserRoles(PanelRequest.ItemID, PanelRequest.AccountID, PanelSecurity.PackageId,
                                                 roles.ToArray());

                messageBox.ShowMessage(res, "UPDATE_CRM_USER_ROLES", "HostedCRM");
            }
            catch(Exception ex)
            {
                messageBox.ShowErrorMessage("UPDATE_CRM_USER_ROLES", ex);
            }
        }

        private void ActivateUser()
        {
            ResultObject res = ES.Services.CRM.ChangeUserState(PanelRequest.ItemID, PanelRequest.AccountID, false);
            messageBox.ShowMessage(res, "CHANGE_USER_STATE", "HostedCRM");
            locDisabled.Visible = false;
            btnDeactivate.Visible = true;
            btnActive.Visible = false;
            locEnabled.Visible = true;
        }

        private void DeactivateUser()
        {
            ResultObject res = ES.Services.CRM.ChangeUserState(PanelRequest.ItemID, PanelRequest.AccountID, true);
            messageBox.ShowMessage(res, "CHANGE_USER_STATE", "HostedCRM");
            locDisabled.Visible = true;
            btnDeactivate.Visible = false;
            btnActive.Visible = true;
            locEnabled.Visible = false;
        }
        
        
        protected void btnActive_Click(object sender, EventArgs e)
        {
            ActivateUser();
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            DeactivateUser();
        }


        
    }
}