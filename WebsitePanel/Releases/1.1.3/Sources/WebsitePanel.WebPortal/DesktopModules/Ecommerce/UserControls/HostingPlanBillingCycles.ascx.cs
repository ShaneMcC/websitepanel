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

namespace WebsitePanel.Ecommerce.Portal.UserControls
{
	public partial class HostingPlanBillingCycles : ecControlBase
	{
		public const string VIEWSTATE_KEY = "__PlanBillingCycles";

		protected List<HostingPlanCycle> PlanBillingCycles
		{
			get
			{
				List<HostingPlanCycle> cycles = ViewState[VIEWSTATE_KEY] as List<HostingPlanCycle>;

				if (cycles == null)
				{
					cycles = new List<HostingPlanCycle>();
					ViewState[VIEWSTATE_KEY] = cycles;
				}

				return cycles;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadBillingCyclesDDL();

			gvPlanCycles.RowCommand += new GridViewCommandEventHandler(gvPlanCycles_RowCommand);
		}

		protected void gvPlanCycles_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "CYCLE_DELETE":
					DeleteHostingPlanCycle(Convert.ToInt32(e.CommandArgument));
					break;
				case "CYCLE_MOVEDOWN":
					FlipHostingPlanCycles(Convert.ToInt32(e.CommandArgument), true);
					break;
				case "CYCLE_MOVEUP":
					FlipHostingPlanCycles(Convert.ToInt32(e.CommandArgument), false);
					break;
			}
		}

		protected void btnAddCycle_Click(object sender, EventArgs e)
		{
			AddBillingCycleToHostingPlan();
		}

		public void LoadHostingPlanCycles(HostingPlanCycle[] cycles)
		{
			// convert
			List<HostingPlanCycle> data = new List<HostingPlanCycle>();
			for (int i = 0; i < cycles.Length; i++)
				data.Add(cycles[i]);
			// save
			ViewState[VIEWSTATE_KEY] = data;
			// bind cycles
			BindHostingPlanCycles();
		}

		public HostingPlanCycle[] GetHostingPlanCycles()
		{
			// sync entered data prior
			SyncGridViewDataEntered();

			// return data syncronized
			return PlanBillingCycles.ToArray();
		}

		private void LoadBillingCyclesDDL()
		{
			int cyclesCount = PlanBillingCycles.Count;
			// load taken cycles
			int[] cyclesTaken = new int[cyclesCount];
			// build array
			for (int i = 0; i < cyclesCount; i++)
				cyclesTaken[i] = PlanBillingCycles[i].CycleId;

			// bind loaded cycles
			BillingCycle[] cyclesToBind = StorehouseHelper.GetBillingCyclesFree(cyclesTaken);
			// show button while at least one cycle available
			phBillingCycles.Visible = (cyclesToBind.Length > 0);
			// bind cycles
			if (cyclesToBind.Length > 0)
			{
				ddlBillingCycles.DataSource = cyclesToBind;
				ddlBillingCycles.DataBind();
			}
		}

		private void SyncGridViewDataEntered()
		{
			// preserve chose cycles details: setup fee, recurring fee and 2CO id
			if (PlanBillingCycles.Count > 0)
			{
				for (int i = 0; i < gvPlanCycles.Rows.Count; i++)
				{
					GridViewRow gv_row = gvPlanCycles.Rows[i];
					HostingPlanCycle gv_cycle = PlanBillingCycles[i];

					TextBox txtSetupFee = ecUtils.FindControl<TextBox>(gv_row, "txtSetupFee");
					TextBox txtRecurringFee = ecUtils.FindControl<TextBox>(gv_row, "txtRecurringFee");

					gv_cycle.SetupFee = ecUtils.ParseDecimal(txtSetupFee.Text, 0);
					gv_cycle.RecurringFee = ecUtils.ParseDecimal(txtRecurringFee.Text, 0);
				}
			}
		}

		private void BindHostingPlanCycles()
		{
			// bind cycles
			gvPlanCycles.DataSource = PlanBillingCycles;
			gvPlanCycles.DataBind();
		}

		private void AddBillingCycleToHostingPlan()
		{
			// sync entered data prior
			SyncGridViewDataEntered();

			// load selected billing cycle
			BillingCycle chosenCycle = StorehouseHelper.GetBillingCycle(Convert.ToInt32(ddlBillingCycles.SelectedValue));
			// convert billing to hosting plan
			HostingPlanCycle cycle = new HostingPlanCycle();
			// fill fields
			cycle.CycleId = chosenCycle.CycleId;
			cycle.CycleName = chosenCycle.CycleName;
			cycle.BillingPeriod = chosenCycle.BillingPeriod;
			cycle.PeriodLength = chosenCycle.PeriodLength;
			// put into viewstate
			PlanBillingCycles.Add(cycle);

			// bind new plan cycles
			BindHostingPlanCycles();

			// re-load billing cycles
			LoadBillingCyclesDDL();
		}

		private void FlipHostingPlanCycles(int indexFrom, bool moveDown)
		{
			// last item can't move down
			if (moveDown && indexFrom == PlanBillingCycles.Count - 1)
				return;
			// first item can't move up
			if (!moveDown && indexFrom == 0)
				return;
			// single item can't move in both directions
			if (PlanBillingCycles.Count == 1)
				return;

			// sync data prior
			SyncGridViewDataEntered();

			HostingPlanCycle cycleToMove = PlanBillingCycles[indexFrom];
			// remove
			PlanBillingCycles.RemoveAt(indexFrom);
			// insert
			if (moveDown)
				PlanBillingCycles.Insert(indexFrom + 1, cycleToMove);
			else
				PlanBillingCycles.Insert(indexFrom - 1, cycleToMove);

			// re-bind data
			BindHostingPlanCycles();
		}

		private void DeleteHostingPlanCycle(int dataItemIndex)
		{
			// remove cycle
			PlanBillingCycles.RemoveAt(dataItemIndex);

			// re-bind cycles
			BindHostingPlanCycles();

			// re-load billing cycles
			LoadBillingCyclesDDL();
		}
	}
}