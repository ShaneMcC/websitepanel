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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WebsitePanel;
using WebsitePanel.Portal;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public delegate object GetStateObjectDelegate();

	public class ecModuleBase : WebsitePanelModuleBase
	{
		private UrlBuilder currentUrl;
		private bool showStorefrontWarning = true;
		private bool enabledForInactiveStore;

		protected bool EnabledForInactiveStore
		{
			get { return enabledForInactiveStore; }
			set { enabledForInactiveStore = value; }
		}

		protected bool ShowStorefrontWarning
		{
			get { return showStorefrontWarning; }
			set { showStorefrontWarning = value; }
		}

		protected bool StorefrontRunning
		{
			get { return true;/*!ecUtils.IsGuidEmpty(EcommerceSettings.OwnerSpaceId)*/ }
		}

		public UrlBuilder CurrentUrl
		{
			get
			{
				if (currentUrl == null)
					currentUrl = UrlBuilder.FromCurrentRequest();

				return currentUrl;
			}
		}

		#region ViewState helper routines

		protected T GetViewStateObject<T>(string keyName, GetStateObjectDelegate callback)
		{
			//
			object obj = ViewState[keyName];
			//
			if (obj == null)
			{
				// obtain object via callback
				obj = callback();
				// store object
				ViewState[keyName] = obj;
			}
			//
			return (T)Convert.ChangeType(obj, typeof(T));
		}

		#endregion

		/*public string NavigateURL(string keyName, string keyValue)
        {
            return DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID,
                PortalSettings.ActiveTab.IsSuperTab,
                PortalSettings,
                string.Empty,
                new string[] { keyName, keyValue });
        }*/

		public override void RedirectToBrowsePage()
		{
			Response.Redirect(NavigateURL("UserID", PanelSecurity.SelectedUserId.ToString()), true);
		}

        public string EditUrlSecure(string controlKey)
		{
			string url = EditUrl(controlKey);

			if (EcommerceSettings.UseSSL)
				url = url.Replace("http://", "https://");

			return url;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!StorefrontRunning && ShowStorefrontWarning)
			{
				ShowWarningMessage("STOREFRONT_DISABLED");

				if (!EnabledForInactiveStore)
					DisableFormControls(this);
			}
		}

		public void EnsurePageIsSecured()
		{
			// read SSL
			bool useSSL = StorefrontHelper.GetSecurePayments(ecPanelRequest.ResellerId);
			//
			if (useSSL && !Request.IsSecureConnection)
			{
				//
				UrlBuilder url = UrlBuilder.FromCurrentRequest();
				// change scheme to secure
				url.Scheme = "https";
				//
				url.Navigate();
			}
		}

		protected void EnsureResellerIsCorrect(params Control[] ctlsToHide)
		{
			// redirect authenticated user to reseller's store
			if (Page.User.Identity.IsAuthenticated)
			{
				if (ecPanelRequest.ResellerId != PanelSecurity.LoggedUser.OwnerId)
				{
					// build url
					UrlBuilder url = UrlBuilder.FromCurrentRequest();
					// replace reseller id
					url.QueryString["ResellerId"] = PanelSecurity.LoggedUser.OwnerId.ToString();
					// navigate back
					url.Navigate();
				}
			}
			// user not logged
			if (ecPanelRequest.ResellerId == 0)
			{
				ShowErrorMessage("RESELLER_NOT_SPECIFIED");
				ecUtils.ToggleControls(false, ctlsToHide);
			}
		}

		/// <summary>
		/// Returns the switched mode module URL. Also additional parameters can be injected to the URL.
		/// </summary>
		/// <param name="controlKey">Control key from module definition without the "Mode" suffix. "Mode" suffix used only to visual distinct in controls defintions.</param>
		/// <param name="parameters">Additional mode switch parameters</param>
		/// <returns></returns>
		public string SwitchModuleMode(string controlKey, params string[] parameters)
		{
			// ensure whether the controlKey is in the correct format
			if (controlKey != null && controlKey.EndsWith("Mode"))
				controlKey = controlKey.Replace("Mode", string.Empty);

			string[] addList = null;
			if (parameters.Length > 0)
			{
				int count = parameters.Length;
				addList = new string[count + 2];
				Array.Copy(new string[] { "mode", controlKey }, addList, 2);
				Array.Copy(parameters, 0, addList, 2, count);
			}
			else if (!string.IsNullOrEmpty(controlKey))
				addList = new string[] { "mode", controlKey };

            return String.Empty;
		}

		public override void ShowWarningMessage(string messageKey)
		{
			base.ShowWarningMessage(Keys.ModuleName, messageKey);
		}

		public override void ShowSuccessMessage(string messageKey)
		{
			base.ShowSuccessMessage(Keys.ModuleName, messageKey);
		}

		public override void ShowErrorMessage(string messageKey, Exception ex, params string[] additionalParameters)
		{
			base.ShowErrorMessage(Keys.ModuleName, messageKey, ex, additionalParameters);
		}

		public void LocalizeDropDownListLabel(DropDownList list, string key)
		{
			string labelStr = GetLocalizedString(string.Concat("ListLabel.", key));
			list.Attributes.Remove("listlabel");

			if (!String.IsNullOrEmpty(labelStr))
			{
				// check whether an item is already exists
				if (list.Items.Count > 0)
				{
					ListItem li = list.Items[0];
					if (String.IsNullOrEmpty(li.Value) && String.Compare(li.Text, labelStr) == 0)
						return;
				}

				list.Items.Insert(0, new ListItem(labelStr, String.Empty));
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			LocalizeModuleControls(this);

			// call base handler
			base.OnPreRender(e);
		}
	}
}