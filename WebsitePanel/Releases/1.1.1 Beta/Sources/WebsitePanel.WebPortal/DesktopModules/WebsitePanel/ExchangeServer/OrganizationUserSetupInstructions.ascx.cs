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

namespace WebsitePanel.Portal.ExchangeServer
{
    public partial class OrganizationUserSetupInstructions : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInstructions();
            }
        }

        private void BindInstructions()
        {
            // load content
            litContent.Text = ES.Services.Organizations.GetOrganizationUserSummuryLetter(
                PanelRequest.ItemID, PanelRequest.AccountID,
                false, false, false);

            // bind user details
            PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
            if (package == null)
                RedirectSpaceHomePage();
            
            OrganizationUser account = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                                                                                        PanelRequest.AccountID);
            if (account != null)
                txtTo.Text = account.ExternalEmail;

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.Organizations.SendOrganizationUserSummuryLetter(
                    PanelRequest.ItemID, PanelRequest.AccountID, false,
                    txtTo.Text.Trim(), txtCC.Text.Trim());

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("ORGANIZATION_LETTER_SEND");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("ORGANIZATION_LETTER_SEND", ex);
                return;
            }
        }
    }
}