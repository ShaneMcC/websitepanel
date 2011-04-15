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
using Microsoft.Web.Services3;
using System.Collections.Generic;
using System.Text;
using System.Web;

using WebsitePanel.Portal;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public class ProxyConfigurator
	{
		public static void Configure(WebServicesClientProtocol proxy)
		{
			Configure(proxy, true);
		}

		public static void RunServiceAsSpaceOwner(WebServicesClientProtocol proxy)
		{
			// impersonate as space owner
			string username = EcommerceSettings.GetSetting("OwnerUsername");
			string password = EcommerceSettings.GetSetting("OwnerPassword");

			if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
			{
				EnterpriseServerProxyConfigurator config = new EnterpriseServerProxyConfigurator();

				config.EnterpriseServerUrl = EcommerceSettings.GetSetting("EnterpriseServer");
				config.Username = username;
				config.Password = password;

				config.Configure(proxy);
			}
			else
				throw new Exception("Ecommerce doesn't configured correctly, please review SitesSettings/Ecommerce section");
		}

		public static void Configure(WebServicesClientProtocol proxy, bool applyPolicy)
		{
			if (applyPolicy && !HttpContext.Current.Request.IsAuthenticated)
			{
				// impersonate as space owner
				string username = EcommerceSettings.GetSetting("OwnerUsername");
				string password = EcommerceSettings.GetSetting("OwnerPassword");

				if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
				{
					EnterpriseServerProxyConfigurator config = new EnterpriseServerProxyConfigurator();

					config.EnterpriseServerUrl = EcommerceSettings.GetSetting("EnterpriseServer");
					config.Username = username;
					config.Password = password;

					config.Configure(proxy);

					return;
				}
			}
			
			PortalUtils.ConfigureEnterpriseServerProxy(proxy, applyPolicy);
        }
	}
}