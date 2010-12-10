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
using Microsoft.Security.Application;

namespace WebsitePanel.Portal.HostedSolution
{
	public partial class UserGeneralSettings : WebsitePanelModuleBase
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
                OrganizationUser user = ES.Services.Organizations.GetUserGeneralSettings(PanelRequest.ItemID,
                    PanelRequest.AccountID);

                litDisplayName.Text =  AntiXss.HtmlEncode(user.DisplayName);

                lblUserDomainName.Text = user.DomainUserName;
                
                // bind form
                txtDisplayName.Text = user.DisplayName;
                
                chkDisable.Checked = user.Disabled;

                txtFirstName.Text = user.FirstName;
                txtInitials.Text = user.Initials;
                txtLastName.Text = user.LastName;

                txtJobTitle.Text = user.JobTitle;
                txtCompany.Text = user.Company;
                txtDepartment.Text = user.Department;
                txtOffice.Text = user.Office;
                manager.SetAccount(user.Manager);

                txtBusinessPhone.Text = user.BusinessPhone;
                txtFax.Text = user.Fax;
                txtHomePhone.Text = user.HomePhone;
                txtMobilePhone.Text = user.MobilePhone;
                txtPager.Text = user.Pager;
                txtWebPage.Text = user.WebPage;

                txtAddress.Text = user.Address;
                txtCity.Text = user.City;
                txtState.Text = user.State;
                txtZip.Text = user.Zip;
                country.Country = user.Country;

                txtNotes.Text = user.Notes;
                txtExternalEmailAddress.Text = user.ExternalEmail;

                txtExternalEmailAddress.Enabled = user.AccountType == ExchangeAccountType.User;
                lblUserDomainName.Text = user.DomainUserName;

                if (user.Locked)
                    chkLocked.Enabled = true;
                else
                    chkLocked.Enabled = false;
                    
                chkLocked.Checked = user.Locked;
             
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("ORGANIZATION_GET_USER_SETTINGS", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                int result = ES.Services.Organizations.SetUserGeneralSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID,
                    txtDisplayName.Text,
                    password.Password,
                    false,
                    chkDisable.Checked,
                    chkLocked.Checked,
                    
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
                    txtNotes.Text, 
                    txtExternalEmailAddress.Text);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

				// update title
				litDisplayName.Text = txtDisplayName.Text;
                if (!chkLocked.Checked)
                    chkLocked.Enabled = false;

				messageBox.ShowSuccessMessage("ORGANIZATION_UPDATE_USER_SETTINGS");
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("ORGANIZATION_UPDATE_USER_SETTINGS", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
        
	}
}