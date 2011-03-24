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
using System.Data;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal
{
    public class OrganizationsHelper
    {
        #region Organizations
        DataSet orgs;

        public int GetOrganizationsPagedCount(int packageId,
            bool recursive, string filterColumn, string filterValue)
        {
            return (int)orgs.Tables[0].Rows[0][0];
        }

        public DataTable GetOrganizationsPaged(int packageId,
            bool recursive, string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";

            orgs = ES.Services.Organizations.GetRawOrganizationsPaged(packageId,
                recursive, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);
            
            return orgs.Tables[1];
        }
        #endregion

        #region Accounts
        OrganizationUsersPaged users;

        public int GetOrganizationUsersPagedCount(int itemId, 
            string filterColumn, string filterValue)
        {
            return users.RecordsCount;            
        }

        public OrganizationUser[] GetOrganizationUsersPaged(int itemId, 
            string filterColumn, string filterValue,
            int maximumRows, int startRowIndex, string sortColumn)
        {
            if (!String.IsNullOrEmpty(filterValue))
                filterValue = filterValue + "%";
			if (maximumRows == 0)
			{
				maximumRows = Int32.MaxValue;
			}

            users = ES.Services.Organizations.GetOrganizationUsersPaged(itemId, filterColumn, filterValue, sortColumn, startRowIndex, maximumRows);

            return users.PageUsers;            
        }

        #endregion
    }
}
