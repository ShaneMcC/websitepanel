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

namespace WebsitePanel.Portal
{
    public partial class SpaceQuotas : WebsitePanelModuleBase
    {
		//
		private PackageContext cntx;
		
		// Brief quotas mapping hash
		// In case if you add new brief quota: just update this collection with quota & table row control ids
		// The code will do the rest...
		private readonly Dictionary<string, string> briefQuotaHash = new Dictionary<string, string>
		{
			// QUOTA CONTROL ID <=> TABLE ROW CONTROL ID
			{ "quotaDiskspace", "pnlDiskspace" },
			{ "quotaBandwidth", "pnlBandwidth" },
			{ "quotaFtpAccounts", "pnlFtpAccounts" },
			{ "quotaDomains", "pnlDomains" },
			{ "quotaSubDomains", "pnlSubDomains" },
			{ "quotaDomainPointers", "pnlDomainPointers" },
			{ "quotaMailAccounts", "pnlMailAccounts" },
			{ "quotaWebSites", "pnlWebSites" }
		};

        protected void Page_Load(object sender, EventArgs e)
        {
            // bind quotas
            BindQuotas();
        }

        private void BindQuotas()
        {
            // load package context
            cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
            
            int packageId = ES.Services.Packages.GetPackage(PanelSecurity.PackageId).PackageId;
            lnkViewBandwidthDetails.NavigateUrl = GetNavigateBandwidthDetails(packageId);
			lnkViewDiskspaceDetails.NavigateUrl = GetNavigateDiskspaceDetails(packageId);
        }

        protected string GetNavigateBandwidthDetails(int packageId)
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            return
                PortalUtils.NavigatePageURL("BandwidthReport", PortalUtils.SPACE_ID_PARAM, packageId.ToString(),
                                            "StartDate=" + startDate.Ticks.ToString(),
                                           "EndDate=" + endDate.Ticks.ToString(), "ctl=edit", "moduleDefId=BandwidthReport");
            
        }

		protected string GetNavigateDiskspaceDetails(int packageId)
		{
			return PortalUtils.NavigatePageURL("DiskspaceReport", PortalUtils.SPACE_ID_PARAM, packageId.ToString(),
				"ctl=edit", "moduleDefId=DiskspaceReport");

		}

        protected void btnViewQuotas_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.SPACE_ID_PARAM, PanelSecurity.PackageId.ToString(),
                "view_quotas"));
        }

		protected override void OnPreRender(EventArgs e)
		{
			//
			SetVisibilityStatus4BriefQuotasBlock();
			//
			base.OnPreRender(e);
		}

		private void SetVisibilityStatus4BriefQuotasBlock()
		{
			foreach (KeyValuePair<string, string> kvp in briefQuotaHash)
			{
				// Lookup for quota control...
				Quota quotaCtl = FindControl(kvp.Key) as Quota;
				Control containerCtl = FindControl(kvp.Value);
				
				// Skip processing if quota or its container ctrl not found
				if (quotaCtl == null || containerCtl == null)
					continue;
				
				// Find out a quita value info within the package context
				QuotaValueInfo qvi = Array.Find<QuotaValueInfo>(
						cntx.QuotasArray, x => x.QuotaName == quotaCtl.QuotaName);
				
				// Skip processing if quota not defined in the package context
				if (qvi == null)
					continue;
				
				// Show or hide corresponding quotas' containers
				switch (qvi.QuotaTypeId)
				{
					case QuotaInfo.BooleanQuota:
						// 1: Quota is enabled;
						// 0: Quota is disabled;
						containerCtl.Visible = (qvi.QuotaAllocatedValue > 0);
						break;
					case QuotaInfo.NumericQuota:
					case QuotaInfo.MaximumValueQuota:
						// -1: Quota is unlimited
						//  0: Quota is disabled
						// xx: Quota is enabled
						containerCtl.Visible = (qvi.QuotaAllocatedValue != 0);
						break;
				}
			}
		}
    }
}