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
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for Organizations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Organizations : HostingServiceProviderWebService
    {

        private IOrganization Organization
        {
            get { return (IOrganization)Provider; }
        }

        [WebMethod, SoapHeader("settings")]
        public bool OrganizationExists(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' OrganizationExists", ProviderSettings.ProviderName);
                bool ret = Organization.OrganizationExists(organizationId);
                Log.WriteEnd("'{0}' OrganizationExists", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't CreateOrganization '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        
        
        [WebMethod, SoapHeader("settings")]
        public Organization CreateOrganization(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' CreateOrganization", ProviderSettings.ProviderName);                
                Organization ret = Organization.CreateOrganization(organizationId);
                Log.WriteEnd("'{0}' CreateOrganization", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't CreateOrganization '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        
        [WebMethod, SoapHeader("settings")]
        public void DeleteOrganization(string organizationId)
        {
             Organization.DeleteOrganization(organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled)
        {
            Log.WriteStart("'{0} CreateUser", ProviderSettings.ProviderName);

            Organization.CreateUser(organizationId, loginName, displayName, upn, password, enabled);                        
            
            Log.WriteEnd("'{0}' CreateUser", ProviderSettings.ProviderName);
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteUser(string loginName, string organizationId)
        {
            Organization.DeleteUser(loginName, organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public OrganizationUser GeUserGeneralSettings(string loginName, string organizationId)
        {
            return Organization.GetUserGeneralSettings(loginName, organizationId);
        }

        [WebMethod, SoapHeader("settings")]
        public void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password,
            bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName,
            string address, string city, string state, string zip, string country, string jobTitle,
            string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail)
        {
            Organization.SetUserGeneralSettings(organizationId, accountName, displayName, password, hideFromAddressBook,
                disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle,
                company, department, office, managerAccountName, businessPhone, fax, homePhone,
                mobilePhone, pager, webPage, notes, externalEmail);
        }


        [WebMethod, SoapHeader("settings")]
        public void DeleteOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            Organization.DeleteOrganizationDomain(organizationDistinguishedName, domain);    
        }
        
        [WebMethod, SoapHeader("settings")]
        public void CreateOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            Organization.CreateOrganizationDomain(organizationDistinguishedName, domain);
        }

        [WebMethod, SoapHeader("settings")]
        public PasswordPolicyResult GetPasswordPolicy()
        {
            return Organization.GetPasswordPolicy();
        }

    }
}
