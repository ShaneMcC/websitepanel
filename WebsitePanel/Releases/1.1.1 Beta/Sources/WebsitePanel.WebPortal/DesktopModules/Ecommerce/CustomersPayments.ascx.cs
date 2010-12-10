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

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class CustomersPayments : ecModuleBase
	{
		protected bool IsReseller
		{
			get { return Convert.ToBoolean(Settings["IsReseller"]); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			//
			if (!IsPostBack)
			{
				odsCustomersPayments.SelectParameters.Add("isReseller", TypeCode.Boolean,
					Settings["IsReseller"].ToString());
			}

			CheckColumnVisibility();
		}

		protected string GetReferringInvoiceURL(int invoiceId)
		{
			if (IsReseller)
                return NavigatePageURL("ecCustomersInvoices", "InvoiceId", Eval("InvoiceId").ToString(), "moduleDefId=ecCustomersInvoices", "ctl=view_invoice", "UserId=" + PanelSecurity.SelectedUserId);
			else
                return NavigatePageURL("ecMyInvoices", "InvoiceId", Eval("InvoiceId").ToString(), "moduleDefId=ecCustomersInvoices", "ctl=view_invoice", "UserId=" + PanelSecurity.SelectedUserId);
		}

		protected void CheckColumnVisibility()
		{
			// hide payment provider column for users
			gvCustomersPayments.Columns[6].Visible = IsReseller;
			// hide actions column for users
			gvCustomersPayments.Columns[8].Visible = IsReseller;
		}

		protected bool GetApproveButtonEnabled(object value)
		{
			return ((TransactionStatus)value) == TransactionStatus.Pending;
		}

		protected bool GetDeclineButtonEnabled(object value)
		{
			return ((TransactionStatus)value) == TransactionStatus.Pending ||
				((TransactionStatus)value) == TransactionStatus.Approved;
		}

		protected void gvCustomersPayments_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			RunGridViewRowCommand(e.CommandName, e.CommandArgument);
		}

		private void RunGridViewRowCommand(string commandName, object commandArgument)
		{
			int result = 0;
			// get data key
			DataKey dk = gvCustomersPayments.DataKeys[Convert.ToInt32(commandArgument)];
			// check data key
			if (dk == null)
				return;
			// get payment
			int paymentId = (int)dk.Value;
			// run command
			switch (commandName)
			{
				case "Approve":
					result = StorehouseHelper.ChangeCustomerPaymentStatus(paymentId, TransactionStatus.Approved);
					break;
				case "Decline":
					result = StorehouseHelper.ChangeCustomerPaymentStatus(paymentId, TransactionStatus.Declined);
					break;
				case "Remove":
					result = StorehouseHelper.DeleteCustomerPayment(paymentId);
					break;
			}
			// show error
			if (result < 0)
			{
				ShowResultMessage(result);
				return;
			}
			// re-bind data
			gvCustomersPayments.DataBind();
		}
	}
}