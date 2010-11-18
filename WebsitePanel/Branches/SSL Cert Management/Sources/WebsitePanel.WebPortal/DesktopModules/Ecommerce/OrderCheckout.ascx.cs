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
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce
{
    public partial class OrderCheckout : ecModuleBase
    {
		public const string CTL_PAYMENT_FORM = "ctlPaymentForm";

		protected void Page_Load(object sender, EventArgs e)
        {
			//
			LoadPaymentControl();
			LoadCustomerInvoice();
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
			CompleteCheckout();
        }

		private void LoadCustomerInvoice()
		{
			try
			{
				//
				string invoiceTemplate = StorefrontHelper.GetContractInvoiceTemplated(ecPanelRequest.ContractId, 
					ecPanelRequest.InvoiceId);
				//
				ctlCustomerInvoice.ShowCustomerInvoice(invoiceTemplate);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("LOAD_CUSTOMER_INVOICE", ex);
				//
				DisableActionCtls();
			}
		}

		private void LoadPaymentControl()
		{
			try
			{
				// load payment method control
				Control ctlMethod = LoadControl("PaymentMethods/" + ecPanelRequest.PaymentMethod + "_Payment.ascx");
				//
				ctlMethod.ID = CTL_PAYMENT_FORM;
				//
				IPaymentMethod methodObj = (IPaymentMethod)ctlMethod;
				//
				PaymentMethod method = StorefrontHelper.GetContractPaymentMethod(ecPanelRequest.ContractId, 
                    ecPanelRequest.PaymentMethod);
				//
                ContractAccount account = StorefrontHelper.GetContractAccount(ecPanelRequest.ContractId);
				//
				methodObj.LoadContractAccount(account);
				//
				methodObj.CheckSupportedItems(method.SupportedItems);
				//
				btnComplete.Visible = !method.Interactive;
				//
				btnProceed.Visible = method.Interactive;
				//
				phPaymentMethod.Controls.Add(ctlMethod);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("LOAD_PAYMENT_CTL", ex);
				//
				DisableActionCtls();
			}
		}

		private void DisableActionCtls()
		{
			// disable controls
			DisableFormControls(btnProceed);
			DisableFormControls(btnComplete);
			DisableFormControls(phPaymentMethod);
		}

		private void CompleteCheckout()
		{
			try
			{
				// lookup for payment form
				IPaymentMethod methodObj = (IPaymentMethod)FindControl(CTL_PAYMENT_FORM);
				//
				CheckoutDetails details = methodObj.GetCheckoutDetails();
				//
                CheckoutResult result = StorefrontHelper.CompleteCheckout(ecPanelRequest.ContractId, 
                    ecPanelRequest.InvoiceId, ecPanelRequest.PaymentMethod, details);
				//
				if (!result.Succeed)
				{
					ShowErrorMessage(result.StatusCode);
					return;
				}
				//
				ecUtils.Navigate("ecOrderComplete", true);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("COMPLETE_CHECKOUT", ex);
			}
		}
    }
}