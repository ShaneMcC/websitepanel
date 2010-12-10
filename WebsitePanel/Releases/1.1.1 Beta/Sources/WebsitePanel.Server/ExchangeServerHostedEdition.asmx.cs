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
using System.Web;
using System.Web.Services;
using WebsitePanel.Providers;
using WebsitePanel.Providers.ExchangeHostedEdition;
using Microsoft.Web.Services3;
using System.Web.Services.Protocols;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for ExchangeHostedEdition
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    public class ExchangeServerHostedEdition : HostingServiceProviderWebService, IExchangeHostedEdition
    {
        private IExchangeHostedEdition ExchangeServer
        {
            get { return (IExchangeHostedEdition)Provider; }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateOrganization(string organizationId, string programId, string offerId, string domain,
            string adminName, string adminEmail, string adminPassword)
        {
            try
            {
                Log.WriteStart("'{0}' CreateOrganization", ProviderSettings.ProviderName);
                ExchangeServer.CreateOrganization(organizationId, programId, offerId, domain, adminName, adminEmail, adminPassword);
                Log.WriteEnd("'{0}' CreateOrganization", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<ExchangeOrganizationDomain> GetOrganizationDomains(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' GetOrganizationDomains", ProviderSettings.ProviderName);
                List<ExchangeOrganizationDomain> result = ExchangeServer.GetOrganizationDomains(organizationId);
                Log.WriteEnd("'{0}' GetOrganizationDomains", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetOrganizationDomains", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddOrganizationDomain(string organizationId, string domain)
        {
            try
            {
                Log.WriteStart("'{0}' AddOrganizationDomain", ProviderSettings.ProviderName);
                ExchangeServer.AddOrganizationDomain(organizationId, domain);
                Log.WriteEnd("'{0}' AddOrganizationDomain", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddOrganizationDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteOrganizationDomain(string organizationId, string domain)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteOrganizationDomain", ProviderSettings.ProviderName);
                ExchangeServer.DeleteOrganizationDomain(organizationId, domain);
                Log.WriteEnd("'{0}' DeleteOrganizationDomain", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteOrganizationDomain", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ExchangeOrganization GetOrganizationDetails(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' GetOrganizationDetails", ProviderSettings.ProviderName);
                ExchangeOrganization result = ExchangeServer.GetOrganizationDetails(organizationId);
                Log.WriteEnd("'{0}' GetOrganizationDetails", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetOrganizationDetails", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateOrganizationQuotas(string organizationId, int mailboxesNumber, int contactsNumber, int distributionListsNumber)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateOrganizationQuotas", ProviderSettings.ProviderName);
                ExchangeServer.UpdateOrganizationQuotas(organizationId, mailboxesNumber, contactsNumber, distributionListsNumber);
                Log.WriteEnd("'{0}' UpdateOrganizationQuotas", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateOrganizationQuotas", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateOrganizationCatchAllAddress(string organizationId, string catchAllEmail)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateOrganizationCatchAllAddress", ProviderSettings.ProviderName);
                ExchangeServer.UpdateOrganizationCatchAllAddress(organizationId, catchAllEmail);
                Log.WriteEnd("'{0}' UpdateOrganizationCatchAllAddress", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateOrganizationCatchAllAddress", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateOrganizationServicePlan(string organizationId, string programId, string offerId)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateOeganizationServicePlan", ProviderSettings.ProviderName);
                ExchangeServer.UpdateOrganizationServicePlan(organizationId, programId, offerId);
                Log.WriteEnd("'{0}' UpdateOeganizationServicePlan", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateOeganizationServicePlan", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteOrganization(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteOrganization", ProviderSettings.ProviderName);
                ExchangeServer.DeleteOrganization(organizationId);
                Log.WriteEnd("'{0}' DeleteOrganization", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteOrganization", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
    }
}
