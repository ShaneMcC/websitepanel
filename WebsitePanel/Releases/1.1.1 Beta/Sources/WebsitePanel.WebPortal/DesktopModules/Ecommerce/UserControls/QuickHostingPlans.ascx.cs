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

namespace WebsitePanel.Ecommerce.Portal.UserControls
{

	public partial class QuickHostingPlans : ecControlBase
	{
		public event EventHandler PlanSelected;

		public int SelectedPlan
		{
			get { return SyncDataListSelection(); }
		}

		public string SelectedPlanName
		{
			get { return SyncDataListSelectionStr(); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				BindQuickHostingPlans();
		}

		protected void rbSelected_OnCheckedChanged(object sender, EventArgs e)
		{
			// raise event
			if (PlanSelected != null)
				PlanSelected(this, EventArgs.Empty);
		}

		private void BindQuickHostingPlans()
		{
			rptHostingPlans.DataSource = StorefrontHelper.GetStorefrontProductsByType(ecPanelRequest.ResellerId, Product.HOSTING_PLAN);
			rptHostingPlans.DataBind();
		}

		private int SyncDataListSelection()
		{
			int selectedId = -1;

			foreach (RepeaterItem item in rptHostingPlans.Items)
			{
				GroupRadioButton r_button = ecUtils.FindControl<GroupRadioButton>(item, "rbSelected");

				if (r_button.Checked)
				{
					selectedId = (int)r_button.ControlValue;
					break;
				}
			}

			return selectedId;
		}

		private string SyncDataListSelectionStr()
		{
			string planName = String.Empty;

			foreach (RepeaterItem item in rptHostingPlans.Items)
			{
				GroupRadioButton r_button = ecUtils.FindControl<GroupRadioButton>(item, "rbSelected");

				if (r_button.Checked)
				{
					planName = r_button.Text;
					break;
				}
			}

			return planName;
		}
	}
}