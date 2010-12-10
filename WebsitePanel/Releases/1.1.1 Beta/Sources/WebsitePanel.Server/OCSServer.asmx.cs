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
	public class OCSServer : HostingServiceProviderWebService
	{
		private IOCSServer OCS
		{
			get { return (IOCSServer)Provider; }
		}



		#region Users

		[WebMethod, SoapHeader("settings")]
		public string CreateUser(string userUpn, string userDistinguishedName)
		{
			try
			{
				Log.WriteStart("{0}.CreateUser", ProviderSettings.ProviderName);
				string ret = OCS.CreateUser(userUpn, userDistinguishedName);
				Log.WriteEnd("{0}.CreateUser", ProviderSettings.ProviderName);
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.CreateUser", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public OCSUser GetUserGeneralSettings(string instanceId)
		{
			try
			{
				Log.WriteStart("{0}.GetUserGeneralSettings", ProviderSettings.ProviderName);
				OCSUser ret = OCS.GetUserGeneralSettings(instanceId);
				Log.WriteEnd("{0}.GetUserGeneralSettings", ProviderSettings.ProviderName);
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.GetUserGeneralSettings", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetUserGeneralSettings(string instanceId, bool enabledForFederation, bool enabledForPublicIMConectivity, bool archiveInternalCommunications, bool archiveFederatedCommunications, bool enabledForEnhancedPresence)
		{
			try
			{
				Log.WriteStart("{0}.SetUserGeneralSettings", ProviderSettings.ProviderName);
				OCS.SetUserGeneralSettings(instanceId, enabledForFederation, enabledForPublicIMConectivity, archiveInternalCommunications, archiveFederatedCommunications, enabledForEnhancedPresence);
				Log.WriteEnd("{0}.SetUserGeneralSettings", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.SetUserGeneralSettings", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void DeleteUser(string instanceId)
		{
			try
			{
				Log.WriteStart("{0}.DeleteUser", ProviderSettings.ProviderName);
				OCS.DeleteUser(instanceId);
				Log.WriteEnd("{0}.DeleteUser", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.DeleteUser", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetUserPrimaryUri(string instanceId, string userUpn)
		{
			try
			{
				Log.WriteStart("{0}.SetUserPrimaryUri", ProviderSettings.ProviderName);
				OCS.SetUserPrimaryUri(instanceId, userUpn);
				Log.WriteEnd("{0}.SetUserPrimaryUri", ProviderSettings.ProviderName);

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error: {0}.SetUserPrimaryUri", ProviderSettings.ProviderName), ex);
				throw;
			}
		}

		#endregion

	}
}
