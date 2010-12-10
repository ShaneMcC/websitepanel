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
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Ecommerce.Portal
{
	public class PortalCache
	{
		protected PortalCache()
		{
		}

		public static string GetString(string keyName)
		{
			object keyValue = GetObject(keyName);

			if (keyValue == null)
				return string.Empty;

			return Convert.ToString(keyValue);
		}

		public static int GetInt(string keyName, int defaultValue)
		{
			return ecUtils.ParseInt(GetObject(keyName), defaultValue);
		}

		public static int GetInt(string keyName)
		{
			return GetInt(keyName, Keys.DefaultInt);
		}

		public static Guid GetGuid(string keyName)
		{
			object keyValue = GetObject(keyName);

			if (keyValue != null)
				return (Guid)keyValue;

			return Guid.Empty;
		}

		public static void SetObject(string keyName, object keyValue)
		{
			if (!HttpContext.Current.Items.Contains(keyName))
				HttpContext.Current.Items.Add(keyName, keyValue);
			else
				HttpContext.Current.Items[keyName] = keyValue;
		}

		public static object GetObject(string keyName)
		{
			return HttpContext.Current.Items[keyName];
		}
	}

    public class CryptoCache
    {
        private static readonly string CacheCryptoKey;

        static CryptoCache()
        {
            CacheCryptoKey = EcommerceSettings.GetSetting(Keys.CacheCryptoKey);
        }

        private CryptoCache() { }

        public static int GetInt(string keyName)
        {
            string keyValue = GetObject(keyName);
            return ecUtils.ParseInt(keyValue, Keys.DefaultInt);
        }

        protected static string GetObject(string keyName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[keyName];

            if (cookie != null)
                return cookie.Value;

            return null;
        }

        public static void RemoveObject(string keyName)
        {
            HttpContext.Current.Request.Cookies.Remove(keyName);
        }

        public static void SetObject(string keyName, object keyValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[keyName];

            if (cookie == null)
                cookie = new HttpCookie(keyName);
            cookie.Value = keyValue.ToString();
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}