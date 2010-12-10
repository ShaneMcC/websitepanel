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

using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.Providers.HostedSolution
{
    public interface IOrganization
    {
        Organization CreateOrganization(string organizationId);
        
        void DeleteOrganization(string organizationId);

        void CreateUser(string organizationId, string loginName, string displayName, string upn, string password, bool enabled);

        void DeleteUser(string loginName, string organizationId);

        OrganizationUser GetUserGeneralSettings(string loginName, string organizationId);

        void SetUserGeneralSettings(string organizationId, string accountName, string displayName, string password,
                                    bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials,
                                    string lastName,
                                    string address, string city, string state, string zip, string country,
                                    string jobTitle,
                                    string company, string department, string office, string managerAccountName,
                                    string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
									string webPage, string notes, string externalEmail);

        bool OrganizationExists(string organizationId);

        void DeleteOrganizationDomain(string organizationDistinguishedName, string domain);

        void CreateOrganizationDomain(string organizationDistinguishedName, string domain);

        PasswordPolicyResult GetPasswordPolicy();
    }
}
