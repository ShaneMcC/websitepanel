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
	public partial class DomainName_ServiceDetails : ecControlBase, IViewServiceDetails
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
				// load domain name svc
				DomainNameSvc domainSvc = StorehouseHelper.GetDomainNameService(serviceId);
				if (domainSvc == null)
					RedirectToBrowsePage();
				//
				ecUtils.ToggleControls(PanelSecurity.LoggedUser.Role != UserRole.User, pnlDomainOrder, pnlUsername);
				//
				ltrServiceName.Text = domainSvc.ServiceName;
				ltrUsername.Text = domainSvc.Username;
				ltrServiceTypeName.Text = ecPanelFormatter.GetSvcItemTypeName(domainSvc.TypeId);
				ltrSvcCycleName.Text = domainSvc.CycleName;
				ltrSvcCyclePeriod.Text = String.Concat(domainSvc.PeriodLength, " ", domainSvc.BillingPeriod, "(s)");
				ltrSvcSetupFee.Text = String.Concat(domainSvc.Currency, " ", domainSvc.SetupFee.ToString("C"));
				ltrSvcRecurringFee.Text = String.Concat(domainSvc.Currency, " ", domainSvc.RecurringFee.ToString("C"));
				ltrSvcCreated.Text = domainSvc.Created.ToString();
				ltrServiceStatus.Text = ecPanelFormatter.GetServiceStatusName(domainSvc.Status);
				//
				if (pnlDomainOrder.Visible)
				{
					//ltrSvcRegOrderId.Text = domainSvc.RegOrderId;
					ltrSvcProviderName.Text = domainSvc.ProviderName;
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
			gvServiceHistory.DataSource = StorehouseHelper.GetServiceHistory(serviceId);
			gvServiceHistory.DataBind();
		}

		#endregion
	}
}