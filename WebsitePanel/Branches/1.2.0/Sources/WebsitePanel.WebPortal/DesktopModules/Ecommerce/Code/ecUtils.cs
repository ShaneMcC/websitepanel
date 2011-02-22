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
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

using WebsitePanel.WebPortal;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public class ecUtils
	{
		public static ContractAccount GetContractAccountFromUserInfo(UserInfo user)
		{
			string mailFormat = user.HtmlMail ? "HTML" : "Text";
			//
			ContractAccount account = GetContractAccountFromInput(user.Username, null, user.FirstName, user.LastName,
				user.Email, user.CompanyName, user.Address, user.City, user.Country, user.State, user.Zip, 
				user.PrimaryPhone, user.Fax, user.InstantMessenger, mailFormat);
			//
			account[ContractAccount.CUSTOMER_ID] = Convert.ToString(user.UserId);
			//
			return account;
		}

		public static ContractAccount GetContractAccountFromInput(string username, string password, string firstName,
			string lastName, string email, string companyName, string address, string city, string country, string state,
			string zip, string phoneNumber, string faxNumber, string instantMessenger, string mailFormat)
		{
			ContractAccount account = new ContractAccount();
			account[ContractAccount.USERNAME] = username;
			account[ContractAccount.PASSWORD] = password;
			account[ContractAccount.FIRST_NAME] = firstName;
			account[ContractAccount.LAST_NAME] = lastName;
			account[ContractAccount.EMAIL] = email;
			account[ContractAccount.COMPANY_NAME] = companyName;
			account[ContractAccount.ADDRESS] = address;
			account[ContractAccount.CITY] = city;
			account[ContractAccount.COUNTRY] = country;
			account[ContractAccount.STATE] = state;
			account[ContractAccount.ZIP] = zip;
			account[ContractAccount.PHONE_NUMBER] = phoneNumber;
			account[ContractAccount.FAX_NUMBER] = faxNumber;
			account[ContractAccount.INSTANT_MESSENGER] = instantMessenger;
			account[ContractAccount.MAIL_FORMAT] = mailFormat;
			//
			return account;
		}

		public static void ToggleControls(bool turnOn, params Control[] ctrls)
		{
			foreach (Control ctrl in ctrls)
				ctrl.Visible = turnOn;
		}

		public static void SelectListItem(DropDownList ctrl, object value)
		{
			// unselect currently selected item
			ctrl.SelectedIndex = -1;

			string val = (value != null) ? value.ToString() : "";
			ListItem item = ctrl.Items.FindByValue(val);
			if (item != null)
				ctrl.SelectedIndex = ctrl.Items.IndexOf(item);
		}

		public static void SelectListItemByText(DropDownList ctrl, object value)
		{
			// unselect currently selected item
			ctrl.SelectedIndex = -1;

			string val = (value != null) ? value.ToString() : "";
			ListItem item = ctrl.Items.FindByText(val);
			if (item != null)
				ctrl.SelectedIndex = ctrl.Items.IndexOf(item);
		}

		public static T FindControl<T>(Control parent, string id)
		{
			return (T)Convert.ChangeType(parent.FindControl(id), typeof(T));
		}

		public static int ParseInt(object value, int defaultValue)
		{
			try { return Int32.Parse(value.ToString()); }
			catch { return defaultValue; }
		}

		public static bool ParseBoolean(object value, bool defaultValue)
		{
			bool result = defaultValue;
			try { result = Convert.ToBoolean(value); }
			catch { /* do nothing*/ }
			return result;
		}

		public static Guid ParseGuid(object value)
		{
			Guid result = Guid.Empty;

			try
			{
				result = new Guid(Convert.ToString(value));
			}
			catch { }

			return result;
		}

		public static decimal ParseDecimal(string val, decimal defaultValue)
		{
			decimal result = defaultValue;
			try { result = Decimal.Parse(val); }
			catch { /* do nothing */ }
			return result;
		}

		public static void InsertLocalizedListMessage(ListControl list, string key)
		{
			key = string.Concat("ListMessage.", key);
			//string localizedMessage = DotNetNuke.Services.Localization.Localization.GetString(key, ecPanelGlobals.ecSharedResourceFile);
            
            string localizedMessage = "";

			if (!String.IsNullOrEmpty(localizedMessage))
			{
				bool firstSelected = list.SelectedIndex == 0;

				list.Items.Insert(0, new ListItem(localizedMessage, string.Empty));

				// restore default selection
				if (firstSelected)
					list.SelectedIndex = 0;
			}
		}

		public static bool IsGuidEmpty(Guid guid)
		{
			return guid.Equals(Guid.Empty);
		}

		public static string EnsurePathWithQuestionMark(string path)
		{
			if (!path.EndsWith("?"))
				return string.Concat(path, "?");

			return path;
		}

        #region Navigate url wrappers

		public static string GetOrderCompleteUrl()
		{
			return GetNavigateUrl("ecOrderComplete", false);
		}

		public static string GetStorefrontUrl()
		{
			return GetNavigateUrl("ecOnlineStore", false);
		}

        public static string GetShoppingCartUrl()
        {
            return GetNavigateUrl("ecMyShoppingCart", false);
        }

        public static string GetRegistrationFormUrl()
        {
            return GetNavigateUrl("ecUserSignup", true);
        }

        #endregion

        #region Jump wrappers

        public static void JumpToShoppingCart()
        {
            Navigate("ecMyShoppingCart", false);
        }

        public static void JumpToOrderCheckout(bool secure)
        {
            Navigate("ecOrderCheckout", secure);
        }

        public static void JumpToCustomerSignup(bool secure)
        {
            Navigate("ecCustomerSignup", secure);
        }

        #endregion

        #region Navigation helper routines

		public static void NavigateToPageModule(string pageKey, string controlKey, params string[] additionalParams)
		{
			PortalPage page = PortalConfiguration.Site.Pages[pageKey];
			ModuleDefinition moduleDef = PortalConfiguration.ModuleDefinitions[controlKey];

			UrlBuilder url = UrlBuilder.FromScratch();
			url.QueryString[DefaultPage.PAGE_ID_PARAM] = pageKey;

			url.QueryString[DefaultPage.MODULE_ID_PARAM] = moduleDef.Id;
		}

        public static string GetNavigateUrl(string pageId, bool secure, params string[] extraParams)
        {
			UrlBuilder url = UrlBuilder.FromScratch();
            url.QueryString[DefaultPage.PAGE_ID_PARAM] = pageId;

			if (secure && EcommerceSettings.UseSSL)
				url.Scheme = "https";
			//
			if (extraParams != null)
			{
				foreach (string extraParam in extraParams)
				{
					string[] data = extraParam.Split('=');
					if (data.Length == 2)
						url.QueryString.Add(data[0].Trim(), data[1].Trim());
				}
			}

            return url.ToString();
        }

        public static void Navigate(string pageId, bool secure)
		{
			UrlBuilder url = UrlBuilder.FromScratch();
			url.QueryString[DefaultPage.PAGE_ID_PARAM] = pageId;

			if (secure && EcommerceSettings.UseSSL)
				url.Scheme = "https";

			url.Navigate();
		}

		public static void Navigate(string pageId, bool secure, params string[] parameters)
		{
			//
			bool useSSL = StorefrontHelper.GetSecurePayments(ecPanelRequest.ResellerId);
			//
			UrlBuilder url = UrlBuilder.FromScratch();
			url.QueryString[DefaultPage.PAGE_ID_PARAM] = pageId;

			if (secure && useSSL)
				url.Scheme = "https";

			// copy query params
			foreach (string parameter in parameters)
			{
				string[] parts = parameter.Split('=');
				url.QueryString[parts[0]] = parts[1];
			}

			url.Navigate();
		}

        #endregion
    }
}