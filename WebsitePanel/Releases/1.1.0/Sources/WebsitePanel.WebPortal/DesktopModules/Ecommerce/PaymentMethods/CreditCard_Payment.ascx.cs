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
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.PaymentMethods
{
	public partial class CreditCard_Payment : ecControlBase, IPaymentMethod
	{
		#region Public routines

		public bool SaveDetailsEnabled
		{
			get
			{
				EnsureChildControls();
				//
				return chkSaveDetails.Visible;
			}
			set
			{
				EnsureChildControls();
				//
				chkSaveDetails.Visible = value;
			}
		}

		public void CheckSupportedItems(string supportedItems)
		{
			// split
			string[] pairs = supportedItems.Split(',');
			// format and process
			foreach (string pair in pairs)
			{
				string[] inpair = pair.Split('=');
				if (inpair.Length == 2)
					ddlCardTypes.Items.Add(new ListItem(inpair[0].Trim(), inpair[1].Trim()));
				else
					ddlCardTypes.Items.Add(pair.Trim());
			}
		}

		public void LoadContractAccount(ContractAccount account)
		{
			txtAddress.Text = account[ContractAccount.ADDRESS];
			//
            txtCity.Text = account[ContractAccount.CITY];
			//
            txtCompany.Text = account[ContractAccount.COMPANY_NAME];
			//
			EnsureCountriesLoaded();
			//
            ecUtils.SelectListItem(ddlCountry, account[ContractAccount.COUNTRY]);
			//
			EnsureCountryStatesLoad();
			//
			if (ddlCountryStates.Items.Count > 0)
                ecUtils.SelectListItem(ddlCountryStates, account[ContractAccount.STATE]);
			else
                txtCountryState.Text = account[ContractAccount.STATE];
			//
            txtEmail.Text = account[ContractAccount.EMAIL];
			//
            txtPhoneNumber.Text = account[ContractAccount.PHONE_NUMBER];
			//
            txtFaxNumber.Text = account[ContractAccount.FAX_NUMBER];
			//
            txtFirstName.Text = account[ContractAccount.FIRST_NAME];
			//
            txtLastName.Text = account[ContractAccount.LAST_NAME];
			//
            txtPostalCode.Text = account[ContractAccount.ZIP];
		}

		public void SetCheckoutDetails(CheckoutDetails details)
		{
			// setup controls first
			EnsureYearsLoaded(ddlExpYear);
			//
			EnsureCountriesLoaded();
			//
			EnsureCountryStatesLoad();
			// skip display values
			if (details == null)
				details = new CheckoutDetails();
			//
			txtCreditCard.Text = details[CheckoutKeys.CardNumber];
			//
			Utils.SelectListItem(ddlCardTypes, details[CheckoutKeys.CardType]);
			//
			txtVerificationCode.Text = details[CheckoutKeys.VerificationCode];
			//
			Utils.SelectListItem(ddlExpYear, details[CheckoutKeys.ExpireYear]);
			//
			Utils.SelectListItem(ddlExpMonth, details[CheckoutKeys.ExpireMonth]);
			//
			txtFirstName.Text = details[CheckoutKeys.FirstName];
			//
			txtLastName.Text = details[CheckoutKeys.LastName];
			//
			txtEmail.Text = details[CheckoutKeys.CustomerEmail];
			//
			txtCompany.Text = details[CheckoutKeys.CompanyName];
			//
			txtAddress.Text = details[CheckoutKeys.Address];
			//
			txtPostalCode.Text = details[CheckoutKeys.Zip];
			//
			txtCity.Text = details[CheckoutKeys.City];
			//
			Utils.SelectListItem(ddlCountry, details[CheckoutKeys.Country]);
			//
			if (ddlCountryStates.Visible)
				Utils.SelectListItem(ddlCountryStates, details[CheckoutKeys.State]);
			else
				txtCountryState.Text = details[CheckoutKeys.State];

			//
			txtPhoneNumber.Text = details[CheckoutKeys.Phone];
			//
			txtFaxNumber.Text = details[CheckoutKeys.Fax];

			//
			if (phCardExt.Visible)
			{
				//
				EnsureYearsLoaded(ddlStartYear);
				//
				Utils.SelectListItem(ddlStartYear, details[CheckoutKeys.StartYear]);

				//
				Utils.SelectListItem(ddlStartMonth, details[CheckoutKeys.StartMonth]);
				
				//
				txtIssueNumber.Text = details[CheckoutKeys.IssueNumber];
			}
		}

		public CheckoutDetails GetCheckoutDetails()
		{
			CheckoutDetails info = new CheckoutDetails();
			//
			info.Persistent = chkSaveDetails.Checked;
			//
			info[CheckoutKeys.IPAddress] = Request.UserHostAddress;
			//
			info[CheckoutKeys.CardNumber] = txtCreditCard.Text.Trim();
			//
			info[CheckoutKeys.CardType] = ddlCardTypes.SelectedValue;
			//
			info[CheckoutKeys.VerificationCode] = txtVerificationCode.Text.Trim();
			//
			info[CheckoutKeys.ExpireMonth] = ddlExpMonth.SelectedValue.Trim();
			//
			info[CheckoutKeys.ExpireYear] = ddlExpYear.SelectedValue.Trim();
			//
			info[CheckoutKeys.FirstName] = txtFirstName.Text.Trim();
			//
			info[CheckoutKeys.LastName] = txtLastName.Text.Trim();
			//
			info[CheckoutKeys.CustomerEmail] = txtEmail.Text.Trim();
			//
			info[CheckoutKeys.CompanyName] = txtCompany.Text.Trim();
			//
			info[CheckoutKeys.Address] = txtAddress.Text.Trim();
			//
			info[CheckoutKeys.Zip] = txtPostalCode.Text.Trim();
			//
			info[CheckoutKeys.City] = txtCity.Text.Trim();
			//
			if (ddlCountryStates.Visible)
				info[CheckoutKeys.State] = ddlCountryStates.Text.Trim();
			else
				info[CheckoutKeys.State] = txtCountryState.Text.Trim();
			//
			info[CheckoutKeys.Country] = ddlCountry.SelectedValue.Trim();
			//
			info[CheckoutKeys.Phone] = txtPhoneNumber.Text.Trim();
			//
			info[CheckoutKeys.Fax] = txtFaxNumber.Text.Trim();
			//
			if (phCardExt.Visible)
			{
				//
				info[CheckoutKeys.StartMonth] = ddlStartMonth.SelectedValue;
				//
				info[CheckoutKeys.StartYear] = ddlStartYear.SelectedValue;
				//
				info[CheckoutKeys.IssueNumber] = txtIssueNumber.Text.Trim();
			}
			//
			return info;
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			// ensure cc payments use SSL
			((ecModuleBase)HostModule).EnsurePageIsSecured();
			//
			if (!IsPostBack)
				EnsureYearsLoaded(ddlExpYear);
		}

		protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
		{
			LoadCountryStates(ddlCountry.SelectedValue);
		}

		protected void ddlCardTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			ShowExtendedCardFields();
		}

		private void EnsureCountriesLoaded()
		{
			if (ddlCountry.Items.Count == 1)
				LoadCountries();
		}

		private void EnsureYearsLoaded(DropDownList list)
		{
			if (list.Items.Count == 0)
				LoadYears(list);
		}

		private void EnsureCountryStatesLoad()
		{
			LoadCountryStates(ddlCountry.SelectedValue);
		}

		private void ShowExtendedCardFields()
		{
			string cardType = ddlCardTypes.SelectedValue;

			if (cardType == "Solo" || cardType == "Switch")
			{
				// load years
				if (ddlStartYear.Items.Count == 0)
					LoadYears(ddlStartYear);
				//
				phCardExt.Visible = true;
			}
		}

		private void LoadYears(DropDownList ctl)
		{
			int year = DateTime.Now.Year;

			for (int i = year; i < year + 5; i++)
				ctl.Items.Add(new ListItem(i.ToString(), i.ToString()));
		}

		private void LoadCountryStates(string countryCode)
		{
			PortalUtils.LoadStatesDropDownList(ddlCountryStates, countryCode);

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
			PortalUtils.LoadCountriesDropDownList(ddlCountry, "");
		}
	}
}