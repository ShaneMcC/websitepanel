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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel;
using WebsitePanel.WebPortal;
using WebsitePanel.Portal;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
    public class ecControlBase : WebsitePanelControlBase
	{
		protected bool StorefrontRunning
		{
			get { return true; }/* !ecUtils.IsGuidEmpty(EcommerceSettings.OwnerSpaceId);*/
		}

		public string EditUrlSecure(string controlKey)
		{
			string url = EditUrl(controlKey);
			
			if (EcommerceSettings.UseSSL)
				url = url.Replace("http://", "https://");

			return url;
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
				Array.Copy(new string[] { String.Concat("mode=", controlKey) }, addList, 1);
				Array.Copy(parameters, 0, addList, 1, count);
			}
			else if (!String.IsNullOrEmpty(controlKey))
				addList = new string[] { String.Concat("mode=", controlKey) };

            return NavigateURL(
                DefaultPage.MODULE_ID_PARAM,
                Request[DefaultPage.MODULE_ID_PARAM],
                addList
            );
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!StorefrontRunning)
				DisableFormControls(this);
		}

		public new void LocalizeModuleControls(Control module)
		{
			base.LocalizeModuleControls(module);

			foreach (Control ctrl in module.Controls)
			{
				if (ctrl is DropDownList)
				{
					DropDownList ddl = ctrl as DropDownList;

					string key = ddl.Attributes["listlabel"];
					if (!String.IsNullOrEmpty(key))
					{
						LocalizeDropDownListLabel(ddl, key);
					}
				}
				else
				{
					// localize children
					foreach (Control childCtrl in ctrl.Controls)
						LocalizeModuleControls(childCtrl);
				}
			}
		}

		public void LocalizeDropDownListLabel(DropDownList list, string key)
		{
			if (IsPostBack)
				return;

			string labelStr = GetLocalizedString(String.Concat("ListLabel.", key));

			if (!String.IsNullOrEmpty(labelStr))
				list.Items.Insert(0, new ListItem(labelStr));

			list.Attributes.Remove("listlabel");
		}

		protected string GetLocalizedErrorMessage(string resourceKey)
		{
			return GetLocalizedString(String.Concat(Keys.ErrorMessage, ".", resourceKey));
		}

		protected override void OnPreRender(EventArgs e)
		{
			LocalizeModuleControls(this);

			// call base handler
			base.OnPreRender(e);
		}
	}
}