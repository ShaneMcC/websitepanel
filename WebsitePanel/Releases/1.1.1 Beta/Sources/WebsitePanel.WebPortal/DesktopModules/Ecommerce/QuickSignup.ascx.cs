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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WSP = WebsitePanel.Portal;

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class QuickSignup : ecModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			// ensure reseller is not 0
			EnsureResellerIsCorrect(wzrdOrderHosting);
		}

		protected void ctlQuickPlans_OnPlanSelected(object sender, EventArgs e)
		{
			if (ctlQuickPlans.SelectedPlan > -1)
			{
				//
				ctlPlanCycles.Visible = true;
				//
				ctlPlanCycles.LoadHostingPlanCycles(ctlQuickPlans.SelectedPlan);
				// load plan
				HostingPlan s_plan = StorefrontHelper.GetHostingPlan(ecPanelRequest.ResellerId, ctlQuickPlans.SelectedPlan);
				// domain visibility
				ctlPlanDomain.DomainOption = s_plan.DomainOption;
			}
		}

		protected void wzrdOrderHosting_NextButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (!Page.IsValid)
				return;
			// 1. Save contract account
			if (wzrdOrderHosting.ActiveStep == Step1)
			{
				// Unable to save contract account and then cancel switch to the next step
				if (!ctlUserAccount.SaveContractAccount())
					e.Cancel = true;
				//
				return;
			}
		}

		protected void wzrdOrderHosting_ActiveStepChanged(object sender, EventArgs e)
		{
			// 4. Display terms and conditions
			if (wzrdOrderHosting.ActiveStep == Step2)
				LoadTermsAndConditions();
		}

		protected void wzrdOrderHosting_FinishButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (!Page.IsValid)
				return;

			// 3. User accepted Terms and Conditions and wish to proceed to checkout
			if (wzrdOrderHosting.ActiveStep == Step2)
			{
				// Create contract
				if (!Page.User.Identity.IsAuthenticated)
					DoContractAccountCreate(e);
				else
					DoContractAccountGet(e);
				//
				DoOrderSubmit(e);
			}
		}

		private void DoContractAccountGet(WizardNavigationEventArgs e)
		{
			try
			{
				Contract contract = StorehouseHelper.GetCustomerContract(WSP.PanelSecurity.SelectedUserId);
				ctlUserAccount.ContractId = contract.ContractId;
			}
			catch (Exception ex)
			{
				ShowErrorMessage("GET_CUSTOMER_CONTRACT", ex);
			}
		}

		private void DoContractAccountCreate(WizardNavigationEventArgs e)
		{
			// create an account
			if (!ctlUserAccount.CreateNewAccount(ecPanelRequest.ResellerId))
			{
				e.Cancel = true;
				return;
			}
		}

		private void DoOrderSubmit(WizardNavigationEventArgs e)
		{
			// No errors were identified
			if (!e.Cancel)
			{
				//
				try
				{
					//
					OrderResult result = SubmitHostingOrder(ctlUserAccount.ContractId);
					// check result
					if (!result.Succeed)
					{
						ShowResultMessage(result.ResultCode);
						e.Cancel = true;
						return;
					}
					//
					NavigateToCheckoutPage(ctlUserAccount.ContractId, result.OrderInvoice, ctlPaymentMethod.SelectedMethod);
				}
				catch (Exception ex)
				{
					ShowErrorMessage("SUBMIT_ORDER", ex);
					e.Cancel = true;
				}
			}
		}

		private void LoadTermsAndConditions()
		{
			try
			{
				pnlTermsAndConds.InnerHtml = StorefrontHelper.GetTermsAndConditions(ecPanelRequest.ResellerId);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("LOAD_TERMS_CONDS", ex);
			}
		}

		private OrderResult SubmitHostingOrder(string contractId)
		{
			//
			List<OrderItem> itemsOrdered = new List<OrderItem>();
			// assemble hosting plan
			OrderItem hostingPlan = OrderItem.GetHostingPlanItem(ctlQuickPlans.SelectedPlan,
				ctlQuickPlans.SelectedPlanName, ctlPlanCycles.SelectedCycle);
			// put hosting plan into order
			itemsOrdered.Add(hostingPlan);
			//
			if (ctlPlanDomain.DomainChecked)
			{
				OrderItem domainItem = ctlPlanDomain.DomainOrderItem;
				domainItem.ParentIndex = itemsOrdered.IndexOf(hostingPlan);
				itemsOrdered.Add(domainItem);
			}
			//
			//
			KeyValueBunch args = new KeyValueBunch();
			args[Keys.INVOICE_DIRECT_URL] = ecUtils.GetNavigateUrl(PagesKeys.ORDER_CHECKOUT, true,
				String.Format("{0}={1}", RequestKeys.CONTRACT_ID, ctlUserAccount.ContractId),
				String.Format("{0}={1}", RequestKeys.PAYMENT_METHOD, ctlPaymentMethod.SelectedMethod));
			//
			return StorefrontHelper.SubmitCustomerOrder(ctlUserAccount.ContractId, itemsOrdered.ToArray(), args);
		}

		private void NavigateToCheckoutPage(string contractId, int invoiceId, string method)
		{
			// do corresponding redirect
			ecUtils.Navigate(PagesKeys.ORDER_CHECKOUT, true, "ContractId=" + contractId, "InvoiceId=" + invoiceId,
				"Method=" + method);
		}
	}
}