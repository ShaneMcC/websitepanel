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
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.Portal.ExchangeServer
{
	public partial class ExchangeCreateMailbox : WebsitePanelModuleBase
	{
	    private bool IsNewUser
	    {
	        get
	        {
	            return NewUserTable.Visible;
	        }
	    }
        
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                password.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.EXCHANGE_POLICY, "MailboxPasswordPolicy");
                PasswordPolicyResult passwordPolicy = ES.Services.Organizations.GetPasswordPolicy(PanelRequest.ItemID);
                if (passwordPolicy.IsSuccess)
                {
                    password.MinimumLength = passwordPolicy.Value.MinLength;
                    if (passwordPolicy.Value.IsComplexityEnable)
                    {
                        password.MinimumNumbers = 1;
                        password.MinimumSymbols = 1;
                        password.MinimumUppercase = 1;
                    }
                }
                else
                {
                    messageBox.ShowMessage(passwordPolicy, "EXCHANGE_CREATE_MAILBOX", "HostedOrganization");
                    return;
                }
                
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package != null)
                {
                    UserInfo user = ES.Services.Users.GetUserById(package.UserId);
                    if (user != null)
                        sendInstructionEmail.Text = user.Email;
                }
            }                                 			
		}

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CreateMailbox();
        }

        private void CreateMailbox()
        {
            if (!Page.IsValid)
                return;

            try
            {
                string name = IsNewUser ? email.AccountName : userSelector.GetPrimaryEmailAddress().Split('@')[0];
                string displayName = IsNewUser ? txtDisplayName.Text.Trim() : userSelector.GetDisplayName();
                string accountName = IsNewUser ? string.Empty : userSelector.GetAccount();

                ExchangeAccountType type = IsNewUser
                                               ? (ExchangeAccountType) Utils.ParseInt(rbMailboxType.SelectedValue, 1)
                                               : ExchangeAccountType.Mailbox;
                
                string domain = IsNewUser ? email.DomainName : userSelector.GetPrimaryEmailAddress().Split('@')[1];

                int accountId = IsNewUser ? 0 : userSelector.GetAccountId();

                accountId = ES.Services.ExchangeServer.CreateMailbox(PanelRequest.ItemID, accountId, type,
                                    accountName,
                                    displayName,
                                    name,
                                    domain,
                                    password.Password,
                                    chkSendInstructions.Checked,
                                    sendInstructionEmail.Text); 
                                                   
                
                if (accountId < 0)
                {
                    messageBox.ShowResultMessage(accountId);
                    return;
                }

                Response.Redirect(EditUrl("AccountID", accountId.ToString(), "mailbox_settings",
                    "SpaceID=" + PanelSecurity.PackageId.ToString(),
                    "ItemID=" + PanelRequest.ItemID.ToString()));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_CREATE_MAILBOX", ex);
            }
        }

       

        protected void rbtnUserExistingUser_CheckedChanged(object sender, EventArgs e)
        {
            ExistingUserTable.Visible = true;
            NewUserTable.Visible = false;
        }

        protected void rbtnCreateNewMailbox_CheckedChanged(object sender, EventArgs e)
        {
            NewUserTable.Visible = true;
            ExistingUserTable.Visible = false;

        }       
        
	}
}