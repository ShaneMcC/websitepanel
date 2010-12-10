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
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;
using Microsoft.Web.Services3;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    
    public class esCRM : WebService
    {

        [WebMethod]
        public OrganizationResult CreateOrganization(int organizationId, string baseCurrencyCode, string baseCurrencyName, string baseCurrencySymbol, string regionName, int userId, string collation)
        {
            return CRMController.CreateOrganization(organizationId, baseCurrencyCode, baseCurrencyName, baseCurrencySymbol, regionName, userId, collation);
        }


        [WebMethod]
        public StringArrayResultObject GetCollation(int packageId)
        {            
            return CRMController.GetCollationNames(packageId);            
        }

        [WebMethod]
        public CurrencyArrayResultObject GetCurrency(int packageId)
        {
            
            return CRMController.GetCurrency(packageId);
        }
      
        [WebMethod]
        public ResultObject DeleteCRMOrganization(int organizationId)
        {
            return CRMController.DeleteOrganization(organizationId);
        }

        [WebMethod]
        public OrganizationUsersPagedResult GetCRMUsersPaged(int itemId, string sortColumn, string sortDirection, string name, string email,
            int startRow, int maximumRows)
        {
            return CRMController.GetCRMUsers(itemId, sortColumn, sortDirection, name, email, startRow, maximumRows);
        }

        [WebMethod]
        public IntResult GetCRMUserCount(int itemId, string name, string email)
        {
            return CRMController.GetCRMUsersCount(itemId, name, email);
        }

        [WebMethod]
        public UserResult CreateCRMUser(OrganizationUser user, int packageId, int itemId, Guid businessUnitOrgId)
        {
            return CRMController.CreateCRMUser(user, packageId, itemId, businessUnitOrgId);
        }


        [WebMethod]
        public CRMBusinessUnitsResult GetBusinessUnits(int itemId, int packageId)
        {
            return CRMController.GetCRMBusinessUnits(itemId, packageId);
        }


        [WebMethod]
        public CrmRolesResult GetCrmRoles(int itemId, int accountId, int packageId)
        {
            return CRMController.GetCRMRoles(itemId, accountId, packageId);
        }

        [WebMethod]
        public ResultObject SetUserRoles(int itemId, int accountId, int packageId, Guid[] roles)
        {
            return CRMController.SetUserRoles(itemId, accountId, packageId, roles);
        }

        [WebMethod]
        public ResultObject ChangeUserState(int itemId, int accountId, bool disable)
        {
            return CRMController.ChangeUserState(itemId, accountId, disable);
        }


        [WebMethod]
        public CrmUserResult GetCrmUser(int itemId, int accountId)
        {
            return CRMController.GetCrmUser(itemId, accountId);
        }

    }
}
