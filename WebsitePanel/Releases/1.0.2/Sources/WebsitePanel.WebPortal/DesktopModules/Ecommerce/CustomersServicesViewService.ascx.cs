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

namespace WebsitePanel.Ecommerce.Portal
{
	public partial class CustomersServicesViewService : ecModuleBase
	{
		private Hashtable SvcViews;

		public CustomersServicesViewService()
		{
			SvcViews = new Hashtable();

			SvcViews.Add(Product.HOSTING_PLAN, "HostingPlan_ServiceDetails.ascx");
			SvcViews.Add(Product.HOSTING_ADDON, "HostingAddon_ServiceDetails.ascx");
			SvcViews.Add(Product.TOP_LEVEL_DOMAIN, "DomainName_ServiceDetails.ascx");
		}

		protected Service CustomerService
		{
			get { return GetViewStateObject<Service>("__SERVICE", new GetStateObjectDelegate(GetRawService)); }
		}
		
		private object GetRawService()
		{
			return StorehouseHelper.GetRawCustomerService(ecPanelRequest.ServiceId);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (CustomerService == null)
				RedirectToBrowsePage();
			//
			LoadServiceViewModule();
			CheckButtonsVisibility();
		}

		protected void btnReturn_Click(object sender, EventArgs e)
		{
			RedirectToBrowsePage();
		}

		protected void btnSvcDelete_Click(object sender, EventArgs e)
		{
			DeleteService();
		}

		protected void btnSvcActivate_Click(object sender, EventArgs e)
		{
			ActivateService();
		}

		protected void btnSvcSuspend_Click(object sender, EventArgs e)
		{
			SuspendService();
		}

		protected void btnSvcCancel_Click(object sender, EventArgs e)
		{
			CancelService();
		}

		private void CheckButtonsVisibility()
		{
			// disable buttons for user
			if (!Convert.ToBoolean(Settings["IsReseller"]))
			{
				ecUtils.ToggleControls(false, chkSendMail, btnSvcDelete, btnSvcActivate, btnSvcSuspend, btnSvcCancel);
				return;
			}
			//
			ecUtils.ToggleControls(CustomerService.Status != ServiceStatus.Active, btnSvcActivate);
			ecUtils.ToggleControls(CustomerService.Status == ServiceStatus.Active, btnSvcSuspend);
			ecUtils.ToggleControls(CustomerService.Status == ServiceStatus.Active || CustomerService.Status == ServiceStatus.Suspended,
				btnSvcCancel);
		}

		private void DeleteService()
		{
			try
			{
				int result = StorehouseHelper.DeleteCustomerService(CustomerService.ServiceId);
				//
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
				ShowErrorMessage("DELETE_SERVICE", ex);
			}
		}

		private void ActivateService()
		{
			try
			{
				GenericSvcResult result = StorehouseHelper.ActivateService(CustomerService.ServiceId,
					chkSendMail.Checked);
				//
				if (!result.Succeed)
				{
					ShowResultMessage(Keys.ModuleName, result.ResultCode);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch(Exception ex)
			{
				ShowErrorMessage("ACTIVATE_SERVICE", ex);
			}
		}

		private void SuspendService()
		{
			try
			{
				GenericSvcResult result = StorehouseHelper.SuspendService(CustomerService.ServiceId,
					chkSendMail.Checked);
				//
				if (!result.Succeed)
				{
					ShowResultMessage(Keys.ModuleName, result.ResultCode);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SUSPEND_SERVICE", ex);
			}
		}

		private void CancelService()
		{
			try
			{
				GenericSvcResult result = StorehouseHelper.CancelService(CustomerService.ServiceId,
					chkSendMail.Checked);
				//
				if (!result.Succeed)
				{
					ShowResultMessage(Keys.ModuleName, result.ResultCode);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("CANCEL_SERVICE", ex);
			}
		}

		private void LoadServiceViewModule()
		{
			if (!SvcViews.ContainsKey(CustomerService.TypeId))
				RedirectToBrowsePage();
			//
			try
			{
				Control ctl = LoadControl("ProductControls/" + SvcViews[CustomerService.TypeId]);
				pnlViewSvcDetails.Controls.Add(ctl);
				//
				IViewServiceDetails svc_view = (IViewServiceDetails)ctl;
				if (svc_view.LoadServiceInfo(CustomerService.ServiceId))
					svc_view.BindServiceHistory(CustomerService.ServiceId);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("", ex);
			}
		}
	}
}