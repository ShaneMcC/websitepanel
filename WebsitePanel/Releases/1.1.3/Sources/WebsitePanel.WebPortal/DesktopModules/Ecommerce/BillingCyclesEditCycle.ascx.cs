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

using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
    public partial class BillingCyclesEditCycle : ecModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
				LoadBillingCycle();
        }

		protected void btnSaveCycle_Click(object sender, EventArgs e)
		{
			SaveBillingCycle();
		}

		protected void btnDeleteCycle_Click(object sender, EventArgs e)
		{
			DeleteBillingCycle();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToBrowsePage();
		}

		private void LoadBillingCycle()
		{
			try
			{
				BillingCycle cycle = StorehouseHelper.GetBillingCycle(ecPanelRequest.BillingCycleId);

				txtCycleName.Text = cycle.CycleName;
				txtPeriodLength.Text = cycle.PeriodLength.ToString();
				ecUtils.SelectListItem(ddlBillingPeriod, cycle.BillingPeriod);
			}
			catch (Exception ex)
			{
				ShowErrorMessage("BILLING_CYCLE_LOAD", ex);
			}
		}

		private void SaveBillingCycle()
		{
			if (!Page.IsValid)
				return;

			try
			{
				string cycleName = txtCycleName.Text.Trim();
				string billingPeriod = ddlBillingPeriod.SelectedValue.Trim();
				int periodLength = Convert.ToInt32(txtPeriodLength.Text.Trim());

				int result = StorehouseHelper.UpdateBillingCycle(ecPanelRequest.BillingCycleId, cycleName, billingPeriod, periodLength);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				// show error
				ShowErrorMessage("BILLING_CYCLE_SAVE", ex);
				return;
			}

			RedirectToBrowsePage();
		}

		private void DeleteBillingCycle()
		{
			try
			{
				int result = StorehouseHelper.DeleteBillingCycle(ecPanelRequest.BillingCycleId);

				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				ShowErrorMessage("BILLING_CYCLE_DELETE", ex);
			}

			RedirectToBrowsePage();
		}
    }
}