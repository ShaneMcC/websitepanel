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
using System.Web;
using System.Collections;

using WebsitePanel.Portal;
using WebsitePanel.WebPortal;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public class EcommerceSettings
	{
		public static string AbsoluteAppPath
		{
			get
			{
				Uri baseUri = HttpContext.Current.Request.Url;
				string appPath = (HttpContext.Current.Request.ApplicationPath == "/") ? "" : HttpContext.Current.Request.ApplicationPath;
				string appPort = (baseUri.Port == 80) ? "" : ":" + baseUri.Port.ToString();

				return String.Format("{0}://{1}{2}{3}", baseUri.Scheme, baseUri.Host, appPort, appPath);
			}
		}

		public static string EcommerceRootPath
		{
			get
			{
				return PortalUtils.ApplicationPath + "/DesktopModules/Ecommerce/";
			}
		}

		public static string CurrencyCodeISO
		{
			get
			{
				string CodeISO = PortalCache.GetString(Keys.CurrencyCodeISO);

				if (String.IsNullOrEmpty(CodeISO))
				{
					int resellerId = 0;

					if (ecPanelRequest.ResellerId > 0)
						resellerId = ecPanelRequest.ResellerId;
					else if (PanelSecurity.SelectedUserId > 0)
						resellerId = PanelSecurity.SelectedUser.OwnerId;

					CodeISO = StorefrontHelper.GetBaseCurrency(resellerId);
					// fallback to usd if currency is empty
					if (String.IsNullOrEmpty(CodeISO))
						CodeISO = "USD";
					PortalCache.SetObject(Keys.CurrencyCodeISO, CodeISO);
				}

				return CodeISO;
			}
		}

		public static bool UseSSL
		{
			get { return GetBoolean("UseSSL"); }
		}

		public static string StorefrontUrl
		{
			get { return ecUtils.GetStorefrontUrl(); }
		}

		public static string OrderCompleteUrl
		{
			get { return ecUtils.GetOrderCompleteUrl(); }
		}

        public static string ShoppingCartUrl
        {
            get { return ecUtils.GetShoppingCartUrl(); }
        }

        public static string RegistrationFormUrl
        {
            get { return ecUtils.GetRegistrationFormUrl(); }
        }

		/// <summary>
		/// Ctor.
		/// </summary>
		private EcommerceSettings()
		{
		}

		public static int GetInt(string keyName, int defaultValue)
		{
			string strValue = GetSetting(keyName);
			return ecUtils.ParseInt(strValue, defaultValue);
		}

		public static int GetInt(string keyName)
		{
			return GetInt(keyName, Keys.DefaultInt);
		}

		public static Guid GetGuid(string keyName)
		{
			string strValue = GetSetting(keyName);
			return ecUtils.ParseGuid(strValue);
		}

		public static bool GetBoolean(string keyName)
		{
			string strValue = GetSetting(keyName);
			return ecUtils.ParseBoolean(strValue, false);
		}

		/// <summary>
		/// Gets value for a particular setting.
		/// </summary>
		/// <param name="keyName">Setting name</param>
		/// <returns></returns>
		public static string GetSetting(string keyName)
		{
            return PortalConfiguration.SiteSettings[keyName];
		}
	}
}