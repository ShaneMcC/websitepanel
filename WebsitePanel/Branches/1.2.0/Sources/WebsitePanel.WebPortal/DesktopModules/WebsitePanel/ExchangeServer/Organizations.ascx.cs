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
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal.ExchangeServer
{
	public partial class Organizations : WebsitePanelModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// set display preferences
			gvOrgs.PageSize = UsersHelper.GetDisplayItemsPerPage();

			// visibility
			chkRecursive.Visible = (PanelSecurity.SelectedUser.Role != UserRole.User);
			gvOrgs.Columns[2].Visible = gvOrgs.Columns[3].Visible =
				(PanelSecurity.SelectedUser.Role != UserRole.User) && chkRecursive.Checked;
		    btnCreate.Enabled = (PanelSecurity.SelectedUser.Role != UserRole.Administrator);
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "create_organization"));
		}

		protected void odsOrgsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.Exception != null)
			{
				messageBox.ShowErrorMessage("GET_ORGS", e.Exception);
				e.ExceptionHandled = true;
			}
		}

		public string GetOrganizationEditUrl(string itemId)
		{
			return EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "organization_home",
                    "ItemID=" + itemId);
		}

		public string GetUserHomePageUrl(int userId)
		{
			return PortalUtils.GetUserHomePageUrl(userId);
		}

		public string GetSpaceHomePageUrl(int spaceId)
		{
			return NavigateURL(PortalUtils.SPACE_ID_PARAM, spaceId.ToString());
		}

        protected void gvOrgs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
			if (e.CommandName == "DeleteItem")
            {
                // delete organization
                int itemId = Utils.ParseInt(e.CommandArgument.ToString(), 0);

                try
                {
                    int result = ES.Services.Organizations.DeleteOrganization(itemId);
                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    // rebind grid
                    gvOrgs.DataBind();

					orgsQuota.BindQuota();
                }
                catch (Exception ex)
                {
					messageBox.ShowErrorMessage("DELETE_ORG", ex);
                }
            }
        }
	}
}