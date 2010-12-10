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

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class CustomersServicesUpgradeService : ecModuleBase
	{
		protected Contract CustomerContract
		{
			get { return GetViewStateObject<Contract>("__CONTRACT", 
				new GetStateObjectDelegate(GetCustomerContract)); }
		}

		protected HostingPackageSvc CustomerPackage
		{
			get { return GetViewStateObject<HostingPackageSvc>("__PACKAGESVC", 
				new GetStateObjectDelegate(GetHostingPackageSvc)); }
		}

		private object GetHostingPackageSvc()
		{
			return StorehouseHelper.GetHostingPackageService(ecPanelRequest.ServiceId);
		}

		private object GetCustomerContract()
		{
			return StorehouseHelper.GetCustomerContract(PanelSecurity.SelectedUserId);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ltrProductName.Text = CustomerPackage.ServiceName;
				ctlPlanDomain.DomainOption = HostingPlan.DOMAIN_OPTIONAL;
				ctlPlanAddons.LoadHostingAddons(CustomerContract.ResellerId, CustomerPackage.ProductId);
				ctlPaymentMethod.LoadPaymentMethods();
			}
		}

		protected void btnContinue_Click(object sender, EventArgs e)
		{
			ContinuePackageUpgrade();
		}

		private void ContinuePackageUpgrade()
		{
			List<OrderItem> itemsOrdered = new List<OrderItem>();
			//
			if (ctlPlanDomain.DomainChecked && ctlPlanDomain.DomainOrderItem != null)
			{
				OrderItem domainItem = ctlPlanDomain.DomainOrderItem;
				domainItem.ParentSvcId = CustomerPackage.ServiceId;
				itemsOrdered.Add(domainItem);
			}
			//
			if (ctlPlanAddons.OrderedAddons != null)
			{
				OrderItem[] addonItems = ctlPlanAddons.OrderedAddons;
				// calculate parent item index
				foreach (OrderItem item in addonItems)
					item.ParentSvcId = CustomerPackage.ServiceId;
				//
				itemsOrdered.AddRange(addonItems);
			}
			//
			if (itemsOrdered.Count == 0)
			{
				RedirectToBrowsePage();
				return;
			}
			//
			try
			{
				OrderResult result = StorefrontHelper.SubmitCustomerOrder(CustomerContract.ContractId, 
					itemsOrdered.ToArray(), null);
				//
				if (!result.Succeed)
				{
					ShowResultMessage(result.ResultCode);
					return;
				}
				//
				NavigateToCheckoutPage(CustomerContract.ContractId, result.OrderInvoice, ctlPaymentMethod.SelectedMethod);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("", ex);
			}
		}

		private void NavigateToCheckoutPage(string contractId, int invoiceId, string method)
		{
			// do corresponding redirect
			ecUtils.Navigate(PagesKeys.ORDER_CHECKOUT, true, "ContractId=" + contractId, "InvoiceId=" + invoiceId,
				"Method=" + method);
		}
	}
}