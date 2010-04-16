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
	public partial class CustomersInvoicesViewInvoice : ecModuleBase
	{
		protected Invoice CustomerInvoice
		{
			get { return GetViewStateObject<Invoice>("__INVOICE", new GetStateObjectDelegate(GetInvoice)); }
		}

		private object GetInvoice()
		{
			return StorehouseHelper.GetCustomerInvoice(ecPanelRequest.InvoiceId);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			ctlCustomerInvoice.LoadCustomerInvoiceTemplate(ecPanelRequest.InvoiceId);
			//
			if (!IsPostBack)
			{
                // customer not allowed to manage his invoice
				if (PanelSecurity.LoggedUserId == CustomerInvoice.CustomerId)
				{
					ecUtils.ToggleControls(false, pnlAddManPayment, btnAddPayment, btnActivateInvoice);
					ecUtils.ToggleControls(!CustomerInvoice.Paid, ctlSelPaymentMethod, btnPayForInvoice);
					//
					if (ctlSelPaymentMethod.Visible)
						ctlSelPaymentMethod.LoadPaymentMethods();
					//
					return;
				}
				// toggle off user's payment method ctl
				ecUtils.ToggleControls(false, ctlSelPaymentMethod, btnPayForInvoice);
				// admins and resellers are allowed to manage user's invoice
				if (PanelSecurity.LoggedUser.Role != UserRole.User)
					ecUtils.ToggleControls(!CustomerInvoice.Paid, pnlAddManPayment, btnAddPayment);
				// hide or show activate invoice button
				if (StorehouseHelper.IsInvoiceProcessed(ecPanelRequest.InvoiceId))
					ecUtils.ToggleControls(false, btnActivateInvoice);
				//
				if (pnlAddManPayment.Visible)
					FillPaymentFields();
			}
		}

		protected void btnAddPayment_Click(object sender, EventArgs e)
		{
			AddManualPayment();
		}

		protected void btnActivateInvoice_Click(object sender, EventArgs e)
		{
			ActivateCustomerInvoice();
		}		

		protected void btnReturn_Click(object sender, EventArgs e)
		{
			RedirectToBrowsePage();
		}

		protected void btnPayForInvoice_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid)
				return;

			// do corresponding redirect
			ecUtils.Navigate(PagesKeys.ORDER_CHECKOUT, true, "ContractId=" + CustomerInvoice.ContractId, "InvoiceId=" + CustomerInvoice.InvoiceId,
				"Method=" + ctlSelPaymentMethod.SelectedMethod);
		}

		private void FillPaymentFields()
		{
			ctlManualPayment.Currency = CustomerInvoice.Currency;
			ctlManualPayment.TotalAmount = CustomerInvoice.Total;
		}

		private void ActivateCustomerInvoice()
		{
			try
			{
				GenericSvcResult[] results = StorehouseHelper.ActivateInvoice(CustomerInvoice.InvoiceId);

				int succeedSvcs = 0, errorSvcs = 0;

				foreach (GenericSvcResult result in results)
				{
					if (!result.Succeed)
						errorSvcs++;
					else
						succeedSvcs++;
				}
				// invoice partially activated 
				if (errorSvcs > 0 && succeedSvcs > 0)
				{
					ShowErrorMessage("INVOICE_PARTIALLY_ACTIVATED");
					return;
				}

				if (errorSvcs > 0 && succeedSvcs == 0)
				{
					ShowErrorMessage("ACTIVATE_INVOICE_SERVICES");
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("ACTIVATE_INVOICE_SERVICES", ex);
			}
		}

		private void AddManualPayment()
		{
			if (!Page.IsValid)
				return;

			try
			{
				decimal totalAmount = ctlManualPayment.TotalAmount;
				if (totalAmount == 0)
					totalAmount = CustomerInvoice.Total;

				int result = StorehouseHelper.AddCustomerPayment(CustomerInvoice.ContractId, CustomerInvoice.InvoiceId, 
                    ctlManualPayment.TransactionId, totalAmount, ctlManualPayment.Currency, 
                    ctlManualPayment.SelectedMethod, ctlManualPayment.TranStatus);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("ADD_INVOICE_PAYMENT", ex);
			}
		}
	}
}