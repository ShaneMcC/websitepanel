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
using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.UserControls
{
	public partial class ChoosePaymentMethod : ecControlBase
	{
		public bool ShowHeader
		{
			get
			{
				EnsureChildControls();
				return pnlControlHdr.Visible;
			}
			set
			{
				EnsureChildControls();
				pnlControlHdr.Visible = value;
			}
		}

		public string SelectedMethod
		{
			get
			{
				return GetPaymentMethodSelected();
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Page.User.Identity.IsAuthenticated)
					BindPaymentMethods(PanelSecurity.SelectedUser.OwnerId);
				else if (ecPanelRequest.ResellerId > 0)
					BindPaymentMethods(ecPanelRequest.ResellerId);
			}
		}

		public void LoadPaymentMethods()
		{
			if (PanelSecurity.SelectedUser.Role == UserRole.Administrator)
				BindPaymentMethods(PanelSecurity.SelectedUserId);
			else
				BindPaymentMethods(PanelSecurity.SelectedUser.OwnerId);
		}

		private string GetPaymentMethodSelected()
		{
			foreach (RepeaterItem item in rptPaymentMethods.Items)
			{
				GroupRadioButton rbutton = ecUtils.FindControl<GroupRadioButton>(item, "rbChecked");
				//
				if (rbutton.Checked)
					return (string)rbutton.ControlValue;
			}
			//
			return String.Empty;
		}

		private void BindPaymentMethods(int resellerId)
		{
			rptPaymentMethods.DataSource = StorefrontHelper.GetResellerPaymentMethods(resellerId);
			rptPaymentMethods.DataBind();
		}
	}
}