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

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class CustomersInvoices : ecModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//
			if (!IsPostBack)
			{
				odsCustomersInvoices.SelectParameters.Add("isReseller",
					TypeCode.Boolean, Settings["IsReseller"].ToString());
			}

			CheckColumnsVisibility();
		}

		public bool GetVoidVisibility(object value)
		{
			return true;
		}

		protected void CheckColumnsVisibility()
		{
			bool isReseller = Convert.ToBoolean(Settings["IsReseller"]);;
			gvCustomersInvoices.Columns[1].Visible = isReseller;
			gvCustomersInvoices.Columns[7].Visible = isReseller;
		}

		protected void btnVoidInvoice_Click(object sender, EventArgs e)
		{
			VoidInvoice(Convert.ToInt32(((Button)sender).CommandArgument));
		}

		private void VoidInvoice(int invoiceId)
		{
			try
			{
				StorehouseHelper.VoidCustomerInvoice(invoiceId);
				//
				gvCustomersInvoices.DataBind();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("VOID_CUSTOMER_INVOICE", ex);
			}
		}
	}
}