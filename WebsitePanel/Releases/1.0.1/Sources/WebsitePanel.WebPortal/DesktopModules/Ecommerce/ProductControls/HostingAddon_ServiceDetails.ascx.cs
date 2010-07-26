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

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.ProductControls
{
	public partial class HostingAddon_ServiceDetails : ecControlBase, IViewServiceDetails
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		#region IViewServiceDetails Members

		public bool LoadServiceInfo(int serviceId)
		{
			EnsureChildControls();

			try
			{
				// load hosting addon svc
				HostingAddonSvc addonSvc = StorehouseHelper.GetHostingAddonService(serviceId);
				if (addonSvc == null)
					RedirectToBrowsePage();
				//
				ecUtils.ToggleControls(addonSvc.Recurring, pnlRecurringAddon);
				ecUtils.ToggleControls(!addonSvc.Recurring, pnlOneTimeAddon);
				ecUtils.ToggleControls(PanelSecurity.LoggedUser.Role != UserRole.User, pnlUsername);

				ltrServiceName.Text = addonSvc.ServiceName;
				ltrUsername.Text = addonSvc.Username;
				ltrServiceTypeName.Text = ecPanelFormatter.GetSvcItemTypeName(addonSvc.TypeId);
				ltrSvcSetupFee.Text = String.Concat(addonSvc.Currency, " ", addonSvc.SetupFee.ToString("C"));
				ltrSvcCreated.Text = addonSvc.Created.ToString();
				ltrServiceStatus.Text = ecPanelFormatter.GetServiceStatusName(addonSvc.Status);

				if (addonSvc.Recurring)
				{
					ltrSvcCycleName.Text = addonSvc.CycleName;
					ltrSvcCyclePeriod.Text = String.Concat(addonSvc.PeriodLength, " ", addonSvc.BillingPeriod, "(s)");
					ltrSvcRecurringFee.Text = String.Concat(addonSvc.Currency, " ", addonSvc.CyclePrice.ToString("C"));
				}
				else
				{
					ltrAddonSvcPrice.Text = String.Concat(addonSvc.Currency, " ", addonSvc.CyclePrice.ToString("C"));
				}
			}
			catch (Exception ex)
			{
				HostModule.ShowErrorMessage("LOAD_DOMAIN_NAME_SVC", ex);
				return false;
			}

			return true;
		}

		public void BindServiceHistory(int serviceId)
		{
			// load hosting addon svc
			HostingAddonSvc addonSvc = StorehouseHelper.GetHostingAddonService(serviceId);
			//
			ecUtils.ToggleControls(addonSvc.Recurring, pnlAddonSvcHistory);
			//
			if (addonSvc.Recurring)
			{
				gvServiceHistory.DataSource = StorehouseHelper.GetServiceHistory(serviceId);
				gvServiceHistory.DataBind();
			}
		}

		#endregion
	}
}