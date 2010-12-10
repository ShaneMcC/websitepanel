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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal.SkinControls
{
    public partial class GlobalSearch : WebsitePanelControlBase
    {
        class Tab
        {
            int index;
            string name;

            public Tab(int index, string name)
            {
                this.index = index;
                this.name = name;
            }

            public int Index
            {
                get { return this.index; }
                set { this.index = value; }
            }

            public string Name
            {
                get { return this.name; }
                set { this.name = value; }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindTabs();
                BindItemTypes();
            }
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            if (PanelSecurity.EffectiveUser.Role != UserRole.User)
                tabsList.Add(new Tab(0, GetLocalizedString("Users.Text")));

			tabsList.Add(new Tab(1, GetLocalizedString("Spaces.Text")));

            if(dlTabs.SelectedIndex == -1)
                dlTabs.SelectedIndex = 0;   
            dlTabs.DataSource = tabsList.ToArray();
            dlTabs.DataBind();

            tabs.ActiveViewIndex = tabsList[dlTabs.SelectedIndex].Index;
        }

        private void BindItemTypes()
        {
            // bind item types
            DataTable dtItemTypes = ES.Services.Packages.GetSearchableServiceItemTypes().Tables[0];
            foreach (DataRow dr in dtItemTypes.Rows)
                ddlItemType.Items.Add(new ListItem(
                    PortalUtils.GetSharedLocalizedString(Utils.ModuleName, "ServiceItemType." + dr["DisplayName"].ToString()),
                    dr["ItemTypeID"].ToString()));
        }

        protected void dlTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTabs();
        }

        protected void btnSearchUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetUsersSearchPageId(),
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "Query=" + Server.UrlEncode(txtUsersQuery.Text),
                "Criteria=" + ddlUserFields.SelectedValue));
        }

        protected void btnSearchSpaces_Click(object sender, EventArgs e)
        {
            Response.Redirect(PortalUtils.NavigatePageURL(PortalUtils.GetSpacesSearchPageId(),
                PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "Query=" + Server.UrlEncode(txtSpacesQuery.Text),
                "ItemTypeID=" + ddlItemType.SelectedValue));
        }
    }
}