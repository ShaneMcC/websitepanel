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
using Microsoft.Web.Services3;
using System.Collections.Generic;
using System.Text;

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public class EC : ES
	{
        public new static EC Services
        {
            get
            {
                EC services = (EC)PortalCache.GetObject("ECWebServices");

                if (services == null)
                {
                    services = new EC();
                    PortalCache.SetObject("ESWebServices", services);
                }

                return services;
            }
        }

        public ecStorefront Storefront
        {
            get { return GetCachedProxy<ecStorefront>(false); }
        }

		public ecStorehouse Storehouse
		{
			get { return GetCachedProxy<ecStorehouse>(true); }
		}

		public ecServiceHandler ServiceHandler
		{
			get { return GetCachedProxy<ecServiceHandler>(false); }
		}

        protected EC()
        {
        }

		protected T GetCachedProxy2<T>(bool secureCalls)
		{
			Type t = typeof(T);
			string key = t.FullName + ".ServiceProxy";
			T proxy = (T)HttpContext.Current.Items[key];
			if (proxy == null)
			{
				proxy = (T)Activator.CreateInstance(t);
				HttpContext.Current.Items[key] = proxy;
			}

			object p = proxy;
			// configure proxy
			ProxyConfigurator.RunServiceAsSpaceOwner((WebServicesClientProtocol)p);

			return proxy;
		}
	}
}