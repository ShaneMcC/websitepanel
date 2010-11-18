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
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    public partial class Domains : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gvDomains.PageSize = UsersHelper.GetDisplayItemsPerPage();

            // visibility
            chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
            gvDomains.Columns[2].Visible = gvDomains.Columns[3].Visible =
                (PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;
			gvDomains.Columns[4].Visible = (PanelSecurity.SelectedUser.Role == UserRole.Administrator);
			gvDomains.Columns[5].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);

            if (!IsPostBack)
            {
                // toggle controls
                btnAddDomain.Enabled = PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Os, Quotas.OS_DOMAINS)
                    || PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Os, Quotas.OS_SUBDOMAINS)
                    || PackagesHelper.CheckGroupQuotaEnabled(PanelSecurity.PackageId, ResourceGroups.Os, Quotas.OS_DOMAINPOINTERS);

                searchBox.AddCriteria("DomainName", GetLocalizedString("SearchField.DomainName"));
                searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
                searchBox.AddCriteria("FullName", GetLocalizedString("SearchField.FullName"));
                searchBox.AddCriteria("Email", GetLocalizedString("SearchField.Email"));
            }
        }

        public string GetItemEditUrl(object packageId, object itemId)
        {
            return EditUrl("DomainID", itemId.ToString(), "edit_item",
                PortalUtils.SPACE_ID_PARAM + "=" + packageId.ToString());
        }

        public string GetUserHomePageUrl(int userId)
        {
            return PortalUtils.GetUserHomePageUrl(userId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
        }

        public string GetItemsPageUrl(string parameterName, string parameterValue)
        {
            return NavigateURL(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                parameterName + "=" + parameterValue);
        }

        public string GetDomainTypeName(bool isSubDomain, bool isInstantAlias, bool isDomainPointer)
        {
            if(isDomainPointer)
                return GetLocalizedString("DomainType.DomainPointer");
            else if (isSubDomain)
                return GetLocalizedString("DomainType.SubDomain");
            else
                return GetLocalizedString("DomainType.Domain");
        }

        protected void odsDomainsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                //this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(), "add_domain"));
        }

		protected void gvDomains_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Detach")
			{
				
                // remove item from meta base
				int domainId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

				int result = ES.Services.Servers.DetachDomain(domainId);
				if (result < 0)
				{
					 
                    ShowResultMessage(result);
				//	return;
				}

				// refresh the list
				//gvDomains.DataBind();
			}
		}
    }
}