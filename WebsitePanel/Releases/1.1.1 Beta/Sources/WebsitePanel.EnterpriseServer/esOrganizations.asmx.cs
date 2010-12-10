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

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esOrganizations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class esOrganizations : WebService
    {
        #region Organizations

        [WebMethod]
        public int CreateOrganization(int packageId,  string organizationID, string organizationName)
        {
            return OrganizationController.CreateOrganization(packageId, organizationID, organizationName);
            
        }

        [WebMethod]
        public DataSet GetRawOrganizationsPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return OrganizationController.GetRawOrganizationsPaged(packageId, recursive,
                filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

               
        [WebMethod]
        public List<Organization> GetOrganizations(int packageId, bool recursive)
        {
            return OrganizationController.GetOrganizations(packageId, recursive);
        }

        [WebMethod]
        public string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return OrganizationController.GetOrganizationUserSummuryLetter(itemId, accountId, pmm, emailMode, signup);
        }
        
        [WebMethod]
        public int SendOrganizationUserSummuryLetter(int itemId, int accountId, bool signup, string to, string cc)
        {
            return OrganizationController.SendSummaryLetter(itemId, accountId, signup, to, cc);
        }
        
        [WebMethod]
        public int DeleteOrganization(int itemId)
        {
            return OrganizationController.DeleteOrganization(itemId);
        }

        [WebMethod]
        public OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            return OrganizationController.GetOrganizationStatistics(itemId);
        }

        [WebMethod]
        public Organization GetOrganization(int itemId)
        {
            return OrganizationController.GetOrganization(itemId);
        }

        #endregion


        #region Domains

        [WebMethod]
        public int AddOrganizationDomain(int itemId, string domainName)
        {
            return OrganizationController.AddOrganizationDomain(itemId, domainName);
        }
        
        [WebMethod]
        public List<OrganizationDomainName> GetOrganizationDomains(int itemId)
        {
            return OrganizationController.GetOrganizationDomains(itemId);
        }
        
        [WebMethod]
        public int DeleteOrganizationDomain(int itemId, int domainId)
        {
            return OrganizationController.DeleteOrganizationDomain(itemId, domainId);
        }

        [WebMethod]
        public int SetOrganizationDefaultDomain(int itemId, int domainId)
        {
            return OrganizationController.SetOrganizationDefaultDomain(itemId, domainId);
        }

        #endregion


        #region Users

        [WebMethod]
        public int CreateUser(int itemId, string displayName, string name, string domain, string password, bool sendNotification, string to)
        {
            string accountName;
            return OrganizationController.CreateUser(itemId, displayName, name, domain, password, true, sendNotification, to, out accountName);
        }

        [WebMethod]
        public OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
			int startRow, int maximumRows)
        {
            return OrganizationController.GetOrganizationUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public OrganizationUser GetUserGeneralSettings(int itemId, int accountId)
        {
            return OrganizationController.GetUserGeneralSettings(itemId, accountId);
        }

        [WebMethod]
        public int SetUserGeneralSettings(int itemId, int accountId, string displayName,
            string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials,
            string lastName, string address, string city, string state, string zip, string country,
            string jobTitle, string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail)
        {
            return OrganizationController.SetUserGeneralSettings(itemId, accountId, displayName,
                password, hideAddressBook, disabled, locked, firstName, initials,
                lastName, address, city, state, zip, country,
                jobTitle, company, department, office, managerAccountName,
                businessPhone, fax, homePhone, mobilePhone, pager,
                webPage, notes, externalEmail);
        }


        [WebMethod]
        public List<OrganizationUser> SearchAccounts(int itemId,            
            string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return OrganizationController.SearchAccounts(itemId,
                filterColumn, filterValue, sortColumn, includeMailboxes);
        }
                

        [WebMethod]
        public int DeleteUser(int itemId, int accountId)
        {
            return OrganizationController.DeleteUser(itemId, accountId); 
        }


        [WebMethod]
        public PasswordPolicyResult GetPasswordPolicy(int itemId)
        {
            return OrganizationController.GetPasswordPolicy(itemId);
        }


        #endregion

    }
}
