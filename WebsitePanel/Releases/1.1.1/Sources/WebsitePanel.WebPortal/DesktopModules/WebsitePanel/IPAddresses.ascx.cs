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
using System.Collections.Generic;
using WebsitePanel.Providers.Common;
using System.Text;

namespace WebsitePanel.Portal
{
    public partial class IPAddresses : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // set display preferences
            if (!IsPostBack)
            {
                // page size
                gvIPAddresses.PageSize = UsersHelper.GetDisplayItemsPerPage();
                ddlItemsPerPage.SelectedValue = gvIPAddresses.PageSize.ToString();

                // pool
                if (!String.IsNullOrEmpty(PanelRequest.PoolId))
                    ddlPools.SelectedValue = PanelRequest.PoolId;
            }
            else
            {
                gvIPAddresses.PageSize = Utils.ParseInt(ddlItemsPerPage.SelectedValue, 10);
            }


            if (!IsPostBack)
            {
                searchBox.AddCriteria("ExternalIP", GetLocalizedString("SearchField.ExternalIP"));
				searchBox.AddCriteria("InternalIP", GetLocalizedString("SearchField.InternalIP"));
                searchBox.AddCriteria("DefaultGateway", GetLocalizedString("SearchField.DefaultGateway"));
				searchBox.AddCriteria("ServerName", GetLocalizedString("SearchField.Server"));
                searchBox.AddCriteria("ItemName", GetLocalizedString("SearchField.ItemName"));
				searchBox.AddCriteria("Username", GetLocalizedString("SearchField.Username"));
            }

            // toggle columns
            bool vps = ddlPools.SelectedIndex > 1;
            gvIPAddresses.Columns[3].Visible = vps;
        }
        protected void odsIPAddresses_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        public string GetSpaceHomeUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }


        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("PoolID", ddlPools.SelectedValue, "add_ip"), true);
        }

        protected void ddlItemsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvIPAddresses.PageSize = Utils.ParseInt(ddlItemsPerPage.SelectedValue, 10);
            gvIPAddresses.DataBind();
        }

        protected void btnEditSelected_Click(object sender, EventArgs e)
        {
            int[] addresses = GetSelectedItems(gvIPAddresses);
            if (addresses.Length == 0)
            {
                ShowWarningMessage("IP_EDIT_LIST_EMPTY_ERROR");
                return;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < addresses.Length; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append(addresses[i]);
            }

            // go to edit screen
            Response.Redirect(EditUrl("Addresses", sb.ToString(), "edit_ip"), true);
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            int[] addresses = GetSelectedItems(gvIPAddresses);
            if (addresses.Length == 0)
            {
                ShowWarningMessage("IP_DELETE_LIST_EMPTY_ERROR");
                return;
            }

            try
            {
                // delete selected IP addresses
                ResultObject res = ES.Services.Servers.DeleteIPAddresses(addresses);

                if (!res.IsSuccess)
                {
                    messageBox.ShowMessage(res, "IP_DELETE_RANGE_IP", "IP");
                    return;
                }

                // refresh grid
                gvIPAddresses.DataBind();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("IP_DELETE_RANGE_IP", ex);
                return;
            }
        }

        private int[] GetSelectedItems(GridView gv)
        {
            List<int> items = new List<int>();

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect.Checked)
                    items.Add((int)gv.DataKeys[i].Value);
            }

            return items.ToArray();
        }
    }
}