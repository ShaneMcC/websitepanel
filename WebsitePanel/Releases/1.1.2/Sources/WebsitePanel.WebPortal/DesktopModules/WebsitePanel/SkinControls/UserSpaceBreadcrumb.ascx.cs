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

namespace WebsitePanel.Portal.SkinControls
{
    public partial class UserSpaceBreadcrumb : System.Web.UI.UserControl
    {
        public bool CurrentNodeVisible
        {
            get { return CurrentNode.Visible; }
            set { CurrentNode.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindUsersPath();
            BindUserSpace();
        }

        private void BindUsersPath()
        {
            repUsersPath.DataSource = ES.Services.Users.GetUserParents(PanelSecurity.SelectedUserId);
            repUsersPath.DataBind();
        }

        private void BindUserSpace()
        {
            spanSpace.Visible = false;
            pnlViewSpace.Visible = false;
            pnlEditSpace.Visible = false;

            lnkCurrentPage.Text = PortalUtils.GetLocalizedPageName(PortalUtils.GetCurrentPageId());

            if (PanelSecurity.PackageId > 0)
            {
                // space
                PackageInfo package = ES.Services.Packages.GetPackage(PanelSecurity.PackageId);
                if (package != null)
                {
                    spanSpace.Visible = true;
                    pnlViewSpace.Visible = true;

                    lnkSpace.Text = package.PackageName;
                    lnkSpace.NavigateUrl = PortalUtils.GetSpaceHomePageUrl(package.PackageId);

					cmdSpaceName.Text = package.PackageName;
					lblSpaceDescription.Text = package.PackageComments;

                    lnkCurrentPage.NavigateUrl = PortalUtils.NavigatePageURL(
                        PortalUtils.GetCurrentPageId(), "SpaceID", PanelSecurity.PackageId.ToString());
                }
            }
            else
            {
                pnlViewUser.Visible = true;

                // user
                UserInfo user = UsersHelper.GetUser(PanelSecurity.SelectedUserId);
                if (user != null)
                {
                    lblUsername.Text = user.Username;

                    lnkCurrentPage.NavigateUrl = PortalUtils.NavigatePageURL(
                        PortalUtils.GetCurrentPageId(), "UserID", PanelSecurity.SelectedUserId.ToString());
                }
            }
        }

        protected void repUsersPath_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            UserInfo user = (UserInfo)e.Item.DataItem;

            HyperLink lnkUser = (HyperLink)e.Item.FindControl("lnkUser");
            if (lnkUser != null)
            {
                lnkUser.Text = user.Username;
                lnkUser.NavigateUrl = PortalUtils.GetUserHomePageUrl(user.UserId);
            }
        }

        protected void cmdChangeName_Click(object sender, EventArgs e)
        {
            pnlEditSpace.Visible = true;
            pnlViewSpace.Visible = false;

			txtName.Text = Server.HtmlDecode(cmdSpaceName.Text);
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            pnlEditSpace.Visible = false;
            pnlViewSpace.Visible = true;
        }

        protected void cmdSave_Click(object sender, EventArgs e)
        {
            // update space
            int result = ES.Services.Packages.UpdatePackageName(PanelSecurity.PackageId,
                Server.HtmlEncode(txtName.Text), lblSpaceDescription.Text);

            if (result < 0)
            {
                return;
            }

            // refresh page
            Response.Redirect(Request.Url.ToString());
        }
    }
}