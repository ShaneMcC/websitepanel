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
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebsitePanel.Providers;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Server.Utils;
using Microsoft.Web.Services3;

namespace WebsitePanel.Server
{
	/// <summary>
	/// OCS Web Service
	/// </summary>
	[WebService(Namespace = "http://smbsaas/websitepanel/server/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[Policy("ServerPolicy")]
	[ToolboxItem(false)]
	public class OCSEdgeServer : HostingServiceProviderWebService
	{
		private IOCSEdgeServer OCS
		{
			get { return (IOCSEdgeServer)Provider; }
		}

		#region Domains
		[WebMethod, SoapHeader("settings")]
		public void AddDomain(string domainName)
		{
			try
			{
				Log.WriteStart("{0}.AddDomain", ProviderSettings.ProviderName);
				OCS.AddDomain(domainName);
				Log.WriteEnd("{0}.AddDomain", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.AddDomain", ProviderSettings.ProviderName), ex);
				throw;
			}
		}
		[WebMethod, SoapHeader("settings")]
		public void DeleteDomain(string domainName)
		{
			try
			{
				Log.WriteStart("{0}.DeleteDomain", ProviderSettings.ProviderName);
				OCS.DeleteDomain(domainName);
				Log.WriteEnd("{0}.DeleteDomain", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.DeleteDomain", ProviderSettings.ProviderName), ex);
				throw;
			}
		}
	
		#endregion

	}
}
