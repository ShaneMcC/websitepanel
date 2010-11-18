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
    public partial class DomainsSelectDomainControl : WebsitePanelControlBase
    {
        public bool HideWebSites
        {
            get { return (ViewState["HideWebSites"] != null) ? (bool)ViewState["HideWebSites"] : false; }
            set { ViewState["HideWebSites"] = value; }
        }

        public bool HideMailDomains
        {
            get { return (ViewState["HideMailDomains"] != null) ? (bool)ViewState["HideMailDomains"] : false; }
            set { ViewState["HideMailDomains"] = value; }
        }

        public bool HideDomainPointers
        {
            get { return (ViewState["HideDomainPointers"] != null) ? (bool)ViewState["HideDomainPointers"] : false; }
            set { ViewState["HideDomainPointers"] = value; }
        }

        public bool HideDomainsSubDomains
        {
            get { return (ViewState["HideDomainsSubDomains"] != null) ? (bool)ViewState["HideDomainsSubDomains"] : false; }
            set { ViewState["HideDomainsSubDomains"] = value; }
        }

        public int PackageId
        {
            get { return (ViewState["PackageId"] != null) ? (int)ViewState["PackageId"] : 0; }
            set { ViewState["PackageId"] = value; }
        }

        public int DomainId
        {
            get
            {
                return Utils.ParseInt(ddlDomains.SelectedValue, 0);
            }
        }

        public string DomainName
        {
            get
            {
                return ddlDomains.SelectedItem.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDomains();
            }
        }

        private void BindDomains()
        {
            DomainInfo[] domains = ES.Services.Servers.GetMyDomains(PackageId);

            ddlDomains.Items.Clear();

            // add "select" item
            ddlDomains.Items.Insert(0, new ListItem(GetLocalizedString("Text.SelectDomain"), ""));

            foreach (DomainInfo domain in domains)
            {
                if (HideWebSites && domain.WebSiteId > 0)
                    continue;
                else if (domain.IsInstantAlias)
                    continue; // remove instant aliases at all
                else if (HideMailDomains && domain.MailDomainId > 0)
                    continue;
                else if (HideDomainPointers && (domain.IsDomainPointer || domain.IsInstantAlias))
                    continue;
                else if (HideDomainsSubDomains && !(domain.IsDomainPointer || domain.IsInstantAlias))
                    continue;

                ddlDomains.Items.Add(new ListItem(domain.DomainName, domain.DomainId.ToString()));
            }

            if (Request.Cookies["CreatedDomainId"] != null)
                Utils.SelectListItem(ddlDomains, Request.Cookies["CreatedDomainId"].Value);
        }
    }
}