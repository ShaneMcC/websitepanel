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

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class CustomerPaymentProfile : ecModuleBase
	{
        private Contract myContract;

		protected void Page_Load(object sender, EventArgs e)
		{
			// Step #1: Disable "On Behalf Of" feature if that the case
			if (PanelSecurity.LoggedUserId != PanelSecurity.SelectedUserId)
			{
				DisablePageFunctionality();
				ShowWarningMessage("WORK_ON_BEHALF_DISABLED");
				return;
			}

			// Step #2: Create customer contract if necessary
			if (!StorehouseHelper.CheckCustomerContractExists())
			{
				ContractAccount accountSettings = ecUtils.GetContractAccountFromUserInfo(PanelSecurity.SelectedUser);
				GenericResult result = StorefrontHelper.AddContract(PanelSecurity.SelectedUser.OwnerId, accountSettings);
				// Show error message
				if (!result.Succeed)
				{
					DisablePageFunctionality();
					ShowResultMessage(result.GetProperty<int>("ResultCode"));
					return;
				}
			}

			// Step #3: Load customer contract
			myContract = StorehouseHelper.GetCustomerContract(PanelSecurity.SelectedUserId);

			// Step #4: Bind payment profile
			if (!IsPostBack)
				BindPaymentProfile();
		}

		protected void btnUpdateProfile_Click(object sender, EventArgs e)
		{
			SavePaymentProfile();
		}

		protected void btnCreateProfile_Click(object sender, EventArgs e)
		{
			SavePaymentProfile();
		}

		protected void btnDeleteProfile_Click(object sender, EventArgs e)
		{
			DeletePaymentProfile();
		}

		protected void DisablePageFunctionality()
		{
			// Hide all controls
			ecUtils.ToggleControls(false, ctlPaymentProfile, btnCreateProfile, btnUpdateProfile, btnDeleteProfile);
		}

		private void BindPaymentProfile()
		{
			// load cc payment method
            PaymentMethod ccMethod = StorefrontHelper.GetContractPaymentMethod(myContract.ContractId,
                PaymentMethod.CREDIT_CARD);
			//
			if (ccMethod == null)
			{
				ShowWarningMessage("PAYMENT_METHOD_NOT_SUPPORTED");
				// hide controls
				ecUtils.ToggleControls(false, ctlPaymentProfile, btnCreateProfile, 
					btnDeleteProfile, btnUpdateProfile);
				//
				return;
			}
			//
			ctlPaymentProfile.CheckSupportedItems(ccMethod.SupportedItems);
			//
			bool exists = StorehouseHelper.PaymentProfileExists(myContract.ContractId);

			// toggle buttons
			ecUtils.ToggleControls(!exists, btnCreateProfile);
			ecUtils.ToggleControls(exists, btnUpdateProfile, btnDeleteProfile);
			//
			if (exists)
				//
                ctlPaymentProfile.SetCheckoutDetails(StorehouseHelper.GetPaymentProfile(myContract.ContractId));
			else
			{
				ShowWarningMessage("EMPTY_PAYMENT_PROFILE");
				ctlPaymentProfile.SetCheckoutDetails(null);
			}
		}

		private void DeletePaymentProfile()
		{
			try
			{
                int result = StorehouseHelper.DeletePaymentProfile(myContract.ContractId);
				// show result
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
				//
				BindPaymentProfile();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("DELETE_PAYMENT_PROFILE", ex);
			}
		}

		private void SavePaymentProfile()
		{
			try
			{
				CheckoutDetails profile = ctlPaymentProfile.GetCheckoutDetails();

				StorehouseHelper.SetPaymentProfile(myContract.ContractId, profile);

				ShowSuccessMessage("SAVE_PAYMENT_PROFILE");
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SAVE_PAYMENT_PROFILE", ex);
			}
		}
	}
}