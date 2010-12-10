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
using WebsitePanel.Providers.Web;

namespace WebsitePanel.Portal
{
    public partial class WebSitesExtensionsControl : WebsitePanelControlBase
    {
        private bool isVirtualDirectory;
        public bool IsVirtualDirectory
        {
            get { return isVirtualDirectory; }
            set { isVirtualDirectory = value; }
        }

        private bool IIs7
        {
            get { return (bool)ViewState["IIs7"]; }
            set { ViewState["IIs7"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (isVirtualDirectory)
                rowCgiBin.Visible = false;
        }

        public void BindWebItem(int packageId, WebVirtualDirectory item)
        {
            // IIS 7.0 mode
            IIs7 = item.IIs7;

            chkAsp.Checked = item.AspInstalled;
            Utils.SelectListItem(ddlAspNet, item.AspNetInstalled);
            Utils.SelectListItem(ddlPhp, item.PhpInstalled);
            chkPerl.Checked = item.PerlInstalled;
            chkPython.Checked = item.PythonInstalled;
            chkCgiBin.Checked = item.CgiBinInstalled;
            
            // toggle controls by quotas
            rowAsp.Visible = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_ASP);

            // IIS 7 does not support web sites w/o ASP.NET, so do we
			if (IIs7)
				ddlAspNet.Items.Remove(ddlAspNet.Items.FindByValue(""));
            
            // asp.net
			if (!PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_ASPNET11))
                ddlAspNet.Items.Remove(ddlAspNet.Items.FindByValue("1"));

            if (!PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_ASPNET20))
                ddlAspNet.Items.Remove(ddlAspNet.Items.FindByValue("2"));

			if (!PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_ASPNET40))
				ddlAspNet.Items.Remove(ddlAspNet.Items.FindByValue("4"));

            if (!IIs7 || !PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_ASPNET20))
                ddlAspNet.Items.Remove(ddlAspNet.Items.FindByValue("2I"));

			if (!IIs7 || !PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_ASPNET40))
				ddlAspNet.Items.Remove(ddlAspNet.Items.FindByValue("4I"));

            rowAspNet.Visible = ddlAspNet.Items.Count > 1;

            // php
            if (!PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_PHP4))
                ddlPhp.Items.Remove(ddlPhp.Items.FindByValue("4"));
            if (!PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_PHP5))
                ddlPhp.Items.Remove(ddlPhp.Items.FindByValue("5"));
            rowPhp.Visible = ddlPhp.Items.Count > 1;

            rowPerl.Visible = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_PERL);
            rowCgiBin.Visible = PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_CGIBIN);

			// Left Python support along IIS 7
			rowPython.Visible = !IIs7 && PackagesHelper.CheckGroupQuotaEnabled(packageId, ResourceGroups.Web, Quotas.WEB_PYTHON);
        }

        public void SaveWebItem(WebVirtualDirectory item)
        {
            item.AspInstalled = chkAsp.Checked;
            item.AspNetInstalled = ddlAspNet.SelectedValue;
            item.PhpInstalled = ddlPhp.SelectedValue;
            item.PerlInstalled = chkPerl.Checked;
            item.PythonInstalled = chkPython.Checked;
            item.CgiBinInstalled = chkCgiBin.Checked;
        }
    }
}