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
using Microsoft.Security.Application;
using WSP = WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.UserControls
{
	public partial class CreateUserAccount : ecControlBase
	{
        public const string VIEWSTATE_KEY = "__ContractAccount";

        protected ContractAccount Account
        {
            get { return (ContractAccount)ViewState[VIEWSTATE_KEY]; }
        }

		private string contractId;

		public string ContractId
		{
            get { return contractId; }
            set { contractId = value; }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// 
			txtPassword.Attributes["value"] = txtPassword.Text;
			txtConfirmPassword.Attributes["value"] = txtConfirmPassword.Text;

			if (!IsPostBack)
			{
				EnsureCountriesLoaded();
				EnsureCountryStatesLoad();
			}
			//
		}

		protected void ctxFormShieldValidator_EvaluatingContext(object sender, ManualValidationEventArgs args)
		{
			args.ContextIsValid = ctlNoBot.IsValid();
		}

		public bool CreateNewAccount(int resellerId)
		{
			if (!Page.IsValid)
				return false;

			try
			{
                GenericResult result = StorefrontHelper.AddContract(Account);
				// check result
				if (!result.Succeed)
				{
					HostModule.ShowResultMessage(result.GetProperty<int>("ResultCode"));
					return false;
				}
				//
                contractId = result["ContractId"];
				//
				return true;
			}
			catch (Exception ex)
			{
				HostModule.ShowErrorMessage("CREATE_NEW_ACCOUNT", ex);
				return false;
			}
		}

        public bool SaveContractAccount()
        {
			ContractAccount account = null;
            //
			if (!Page.User.Identity.IsAuthenticated)
			{
				//
				string username = txtUsername.Text.Trim();
				//
				if (StorefrontHelper.UsernameExists(username))
				{
					HostModule.ShowResultMessage(BusinessErrorCodes.ERROR_USER_ALREADY_EXISTS);
					return false;
				}
				//
				string state = (txtCountryState.Visible) ? txtCountryState.Text.Trim() : ddlCountryStates.SelectedValue;
				//
				account = ecUtils.GetContractAccountFromInput(username, txtPassword.Text, txtFirstName.Text.Trim(),
					txtLastName.Text.Trim(), txtEmail.Text.Trim(), txtCompany.Text.Trim(), txtAddress.Text.Trim(),
					txtCity.Text.Trim(), ddlCountry.SelectedValue, state, txtPostalCode.Text.Trim(), txtPhoneNumber.Text.Trim(),
					txtFaxNumber.Text.Trim(), txtInstantMsngr.Text.Trim(), ddlMailFormat.SelectedValue);
			}
			else
			{
				account = ecUtils.GetContractAccountFromUserInfo(WSP.PanelSecurity.EffectiveUser);
			}
			//
			ViewState[VIEWSTATE_KEY] = account;
			//
			return true;
        }

		protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadCountryStates(ddlCountry.SelectedValue);
		}

		private void EnsureCountriesLoaded()
		{
			if (ddlCountry.Items.Count == 1)
				LoadCountries();
		}

		private void EnsureCountryStatesLoad()
		{
			LoadCountryStates(ddlCountry.SelectedValue);
		}

		private void LoadCountryStates(string countryCode)
		{
			WSP.PortalUtils.LoadStatesDropDownList(ddlCountryStates, countryCode);

			//
			if (ddlCountryStates.Items.Count > 0)
			{
				txtCountryState.Visible = false;
				ddlCountryStates.Visible = true;
				reqCountryState.ControlToValidate = ddlCountryStates.ID;
			}
			else
			{
				txtCountryState.Visible = true;
				ddlCountryStates.Visible = false;
				reqCountryState.ControlToValidate = txtCountryState.ID;
			}
		}

		private void LoadCountries()
		{
			WSP.PortalUtils.LoadCountriesDropDownList(ddlCountry, "");
		}

		protected void ctlNoBot_GenerateChallengeAndResponse(object sender, AjaxControlToolkit.NoBotEventArgs e)
		{
			Random rand = new Random();
			//
			int firstNum = rand.Next(1000);
			int secondNum = rand.Next(1000);
			//
			e.ChallengeScript = String.Format("eval(\"{0}+{1}\")", firstNum, secondNum);
			//
			e.RequiredResponse = Convert.ToString(firstNum + secondNum);
		}
	}
}