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

namespace WebsitePanel.Portal.UserControls
{
    public partial class EditDomainsList : WebsitePanelControlBase
    {
        public bool DisplayNames
        {
            get { return gvDomains.Columns[0].Visible; }
            set { gvDomains.Columns[0].Visible = value; }
        }

        public string Value
        {
            get { return GetDomainsValue(); }
            set { SetDomainsValue(value); }
        }

        private string GetDomainsValue()
        {
            List<string> items = CollectFormData(false);
            return String.Join(";", items.ToArray());
        }

        private void SetDomainsValue(string s)
        {
            List<string> items = new List<string>();
            if (!String.IsNullOrEmpty(s))
            {
                string[] parts = s.Split(';');
                foreach (string part in parts)
                {
                    if (part.Trim() != "")
                        items.Add(part.Trim());
                }
            }

            // save items
            loadItems = items.ToArray();

            if (IsPostBack)
            {
                BindItems(loadItems);
            }
        }

        private string[] loadItems = new string[] { };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindItems(loadItems); // empty list
            }
        }

        private void BindItems(string[] items)
        {
            gvDomains.DataSource = items;
            gvDomains.DataBind();
        }

        public List<string> CollectFormData(bool includeEmpty)
        {
            List<string> items = new List<string>();
            foreach (GridViewRow row in gvDomains.Rows)
            {
                TextBox txtDomainName = (TextBox)row.FindControl("txtDomainName");
                string val = txtDomainName.Text.Trim();

                if (includeEmpty || val != "")
                    items.Add(val);
            }

            return items;
        }

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            List<string> items = CollectFormData(true);

            // add empty string
            items.Add("");

            // bind items
            BindItems(items.ToArray());
        }

        protected void gvDomains_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_item")
            {
                List<string> items = CollectFormData(true);

                // remove error
                items.RemoveAt(Utils.ParseInt((string)e.CommandArgument, 0));

                // bind items
                BindItems(items.ToArray());
            }
        }

        protected void gvDomains_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblDomainName = (Label)e.Row.FindControl("lblDomainName");
            TextBox txtDomainName = (TextBox)e.Row.FindControl("txtDomainName");
            ImageButton cmdDeleteDomain = (ImageButton)e.Row.FindControl("cmdDeleteDomain");

            if (lblDomainName != null)
            {
                string val = (string)e.Row.DataItem;
                txtDomainName.Text = val;

                string pos = (e.Row.RowIndex < 2) ? e.Row.RowIndex.ToString() : "";
                lblDomainName.Text = GetLocalizedString("Item" + pos + ".Text");

                cmdDeleteDomain.CommandArgument = e.Row.RowIndex.ToString();
            }
        }
    }
}