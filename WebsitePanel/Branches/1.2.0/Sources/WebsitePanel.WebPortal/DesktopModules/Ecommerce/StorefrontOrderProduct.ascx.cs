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

using WebsitePanel.EnterpriseServer;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.Portal;

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class StorefrontOrderProduct : ecModuleBase
	{
		const string VIEW_STATE_KEY = "__SelectedPlan";

		protected HostingPlan SelectedPlan
		{
			get { return GetViewStateObject<HostingPlan>(VIEW_STATE_KEY, 
					new GetStateObjectDelegate(GetHostingPlan)); }
		}
		// ViewState delegate
		private object GetHostingPlan()
		{
			return StorefrontHelper.GetHostingPlan(ecPanelRequest.ResellerId, ecPanelRequest.ProductId);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				EnsureResellerIsCorrect(wzrdOrderHosting);
				LoadProductInfo();
			}
		}

		protected void wzrdOrderHosting_NextButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (!Page.IsValid)
				return;
            //
			if (wzrdOrderHosting.ActiveStep == Step1 && Page.User.Identity.IsAuthenticated)
			{
				// Unable to save contract account and then cancel switch to the next step
				if (!ctlCustomerCreate.SaveContractAccount())
					e.Cancel = true;
				//
				wzrdOrderHosting.MoveTo(Step4);
				//
				return;
			}
			//
			if (wzrdOrderHosting.ActiveStep == Step2)
			{
				DoCustomerLoginStep(e);
				wzrdOrderHosting.MoveTo(Step4);
				return;
			}

			// 3. Save contract account
			if (wzrdOrderHosting.ActiveStep == Step3)
			{
				// Unable to save contract account and then cancel switch to the next step
				if (!ctlCustomerCreate.SaveContractAccount())
					e.Cancel = true;
				//
				return;
			}

			// 1. User already has an account and wish to signup next
			if (rbExistingUser.Checked)
				wzrdOrderHosting.MoveTo(Step2);
			// 2. User would like to create a new account
			if (rbNewUser.Checked)
			{
				wzrdOrderHosting.MoveTo(Step3);
			}
		}

        protected void wzrdOrderHosting_ActiveStepChanged(object sender, EventArgs e)
        {
			// 4. Display terms and conditions
            if (wzrdOrderHosting.ActiveStep == Step4)
                LoadTermsAndConditions();
        }

		protected void wzrdOrderHosting_FinishButtonClick(object sender, WizardNavigationEventArgs e)
		{
			if (!Page.IsValid)
				return;

			// 3. User accepted Terms and Conditions and wish to proceed to checkout
            if (wzrdOrderHosting.ActiveStep == Step4)
            {
				// Ensure the contract already exists
				if (Page.User.Identity.IsAuthenticated && StorehouseHelper.CheckCustomerContractExists())
					DoContractAccountGet(e);
				else
					DoContractAccountCreate(e);   
                //
                DoOrderSubmit(e);
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

        private void DoOrderSubmit(WizardNavigationEventArgs e)
        {
            // No errors were identified
            if (!e.Cancel)
            {
                //
                try
                {
                    //
                    OrderResult result = SubmitHostingOrder(ctlCustomerCreate.ContractId);
                    // check result
                    if (!result.Succeed)
                    {
                        ShowResultMessage(result.ResultCode);
                        e.Cancel = true;
                        return;
                    }
                    //
                    NavigateToCheckoutPage(ctlCustomerCreate.ContractId, result.OrderInvoice, ctlPaymentMethod.SelectedMethod);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage("SUBMIT_ORDER", ex);
                    e.Cancel = true;
                }
            }
        }

        private bool DoContractAccountGet(WizardNavigationEventArgs e)
        {
            try
            {
				Contract contract = StorehouseHelper.GetCustomerContract(PanelSecurity.SelectedUserId);
				if (contract != null)
				{
					ctlCustomerCreate.ContractId = contract.ContractId;
					return true;
				}
            }
            catch (Exception ex)
            {
                ShowErrorMessage("GET_CUSTOMER_CONTRACT", ex);
            }
			// No contract has been found
			return false;
        }

		private void DoContractAccountCreate(WizardNavigationEventArgs e)
		{
            // create an account
			if (!ctlCustomerCreate.CreateNewAccount(ecPanelRequest.ResellerId))
			{
				e.Cancel = true;
				return;
			}
		}

		private void DoCustomerLoginStep(WizardNavigationEventArgs e)
		{
			// authentication
			if (!ctlCustomerLogin.Authenticate())
			{
				e.Cancel = true;
				return;
			}
		}

        private void NavigateToCheckoutPage(string contractId, int invoiceId, string method)
        {
            // do corresponding redirect
            ecUtils.Navigate(PagesKeys.ORDER_CHECKOUT, true, "ContractId=" + contractId, "InvoiceId=" + invoiceId, 
                "Method=" + method);
        }

		private void NavigateToCheckoutPage(int resellerId, int userId, int invoiceId, string method, string sessionId)
		{
			// do corresponding redirect
			ecUtils.Navigate(PagesKeys.ORDER_CHECKOUT, true, "ResellerId=" + resellerId, "UserId=" + userId,
				"InvoiceId=" + invoiceId, "Method=" + method, "SessionId=" + sessionId);
		}

		private void LoadProductInfo()
		{
			if (SelectedPlan == null)
			{
				ShowErrorMessage("PRODUCT_NOT_FOUND");
				ecUtils.ToggleControls(false, wzrdOrderHosting);
				return;
			}
			// assign product name & desc
			ltrProductName.Text = SelectedPlan.ProductName;
			ltrProductDesc.Text = SelectedPlan.Description;
			// bind highlights
			ctlPlanHighlights.BindHighlights(StorefrontHelper.GetProductHighlights(
				ecPanelRequest.ResellerId, SelectedPlan.ProductId));
			// load plan cycles
			ctlPlanCycles.LoadHostingPlanCycles(SelectedPlan.ProductId);
			// load add-ons
			ctlPlanAddons.LoadHostingAddons(ecPanelRequest.ResellerId, SelectedPlan.ProductId);
			if (ctlPlanAddons.IsEmpty)
				pnlPlanAddons.Visible = false;
			// assign domain option
			ctlPlanDomain.DomainOption = SelectedPlan.DomainOption;
			//
			ecUtils.ToggleControls(!Page.User.Identity.IsAuthenticated, pnlUAccountOptions);
		}

		private OrderResult SubmitHostingOrder(string contractId)
		{
			List<OrderItem> itemsOrdered = new List<OrderItem>();
			// assemble hosting plan to be ordered
			OrderItem hostingPlan = OrderItem.GetHostingPlanItem(SelectedPlan.ProductId,
				SelectedPlan.ProductName, ctlPlanCycles.SelectedCycle);
			// put hosting plan into order
			itemsOrdered.Add(hostingPlan);
			// assemble domain option if selected
			if (ctlPlanDomain.DomainChecked && ctlPlanDomain.DomainOrderItem != null)
			{
				OrderItem domainItem = ctlPlanDomain.DomainOrderItem;
				// calculate parent item index
				domainItem.ParentIndex = itemsOrdered.IndexOf(hostingPlan);
				itemsOrdered.Add(domainItem);
			}
			// assemble ordered hosting addons
			if (ctlPlanAddons.OrderedAddons != null)
			{
				OrderItem[] addonItems = ctlPlanAddons.OrderedAddons;
				// calculate parent item index
				foreach (OrderItem item in addonItems)
					item.ParentIndex = itemsOrdered.IndexOf(hostingPlan);
				//
				itemsOrdered.AddRange(addonItems);
			}
			//
			KeyValueBunch args = new KeyValueBunch();
		    args[Keys.INVOICE_DIRECT_URL] = ecUtils.GetNavigateUrl(PagesKeys.ORDER_CHECKOUT, true, 
				String.Format("{0}={1}", RequestKeys.CONTRACT_ID, contractId),
				String.Format("{0}={1}", RequestKeys.PAYMENT_METHOD, ctlPaymentMethod.SelectedMethod));
			//
			return StorefrontHelper.SubmitCustomerOrder(contractId, itemsOrdered.ToArray(), args);
		}
	}
}