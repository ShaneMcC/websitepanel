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
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.EnterpriseServer;
using Microsoft.Security.Application;

namespace WebsitePanel.Portal.ExchangeServer
{
	public partial class ExchangeMailboxGeneralSettings : WebsitePanelModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                BindSettings();
            }
		}

        private void BindSettings()
        {
            try
            {
                password.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.EXCHANGE_POLICY, "MailboxPasswordPolicy");
                password.EditMode = true;

                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

				// title
				litDisplayName.Text = mailbox.DisplayName;

                // bind form
                txtDisplayName.Text = mailbox.DisplayName;
                chkHideAddressBook.Checked = mailbox.HideFromAddressBook;
                chkDisable.Checked = mailbox.Disabled;

                txtFirstName.Text = mailbox.FirstName;
                txtInitials.Text = mailbox.Initials;
                txtLastName.Text = mailbox.LastName;

                txtJobTitle.Text = mailbox.JobTitle;
                txtCompany.Text = mailbox.Company;
                txtDepartment.Text = mailbox.Department;
                txtOffice.Text = mailbox.Office;
                manager.SetAccount(mailbox.ManagerAccount);

                txtBusinessPhone.Text = mailbox.BusinessPhone;
                txtFax.Text = mailbox.Fax;
                txtHomePhone.Text = mailbox.HomePhone;
                txtMobilePhone.Text = mailbox.MobilePhone;
                txtPager.Text = mailbox.Pager;
                txtWebPage.Text = mailbox.WebPage;

                txtAddress.Text = mailbox.Address;
                txtCity.Text = mailbox.City;
                txtState.Text = mailbox.State;
                txtZip.Text = mailbox.Zip;
                country.Country = mailbox.Country;

                txtNotes.Text = mailbox.Notes;

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                chkPmmAllowed.Checked = (account.MailboxManagerActions & MailboxManagerActions.GeneralSettings) > 0;
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.ExchangeServer.SetMailboxGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    txtDisplayName.Text,
                    password.Password,
                    chkHideAddressBook.Checked,
                    chkDisable.Checked,
                    
                    txtFirstName.Text,
                    txtInitials.Text,
                    txtLastName.Text,
                    
                    txtAddress.Text,
                    txtCity.Text,
                    txtState.Text,
                    txtZip.Text,
                    country.Country,
                    
                    txtJobTitle.Text,
                    txtCompany.Text,
                    txtDepartment.Text,
                    txtOffice.Text,
                    manager.GetAccount(),
                    
                    txtBusinessPhone.Text,
                    txtFax.Text,
                    txtHomePhone.Text,
                    txtMobilePhone.Text,
                    txtPager.Text,
                    txtWebPage.Text,
                    txtNotes.Text);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

				// update title
				litDisplayName.Text = AntiXss.HtmlEncode(txtDisplayName.Text);

				messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS");
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void chkPmmAllowed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.ExchangeServer.SetMailboxManagerSettings(PanelRequest.ItemID, PanelRequest.AccountID,
                chkPmmAllowed.Checked, MailboxManagerActions.GeneralSettings);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILMANAGER");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILMANAGER", ex);
            }
        }
	}
}