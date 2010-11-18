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
	public partial class DomainNameBillingCycles : ecControlBase
	{
		public const string VIEWSTATE_KEY = "__DomainBillingCycles";

		protected List<DomainNameCycle> DomainBillingCycles
		{
			get
			{
				List<DomainNameCycle> cycles = ViewState[VIEWSTATE_KEY] as List<DomainNameCycle>;

				if (cycles == null)
				{
					cycles = new List<DomainNameCycle>();
					ViewState[VIEWSTATE_KEY] = cycles;
				}

				return cycles;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadBillingCyclesDDL();

			gvDomainNameCycles.RowCommand += new GridViewCommandEventHandler(gvDomainNameCycles_RowCommand);
		}

		protected void gvDomainNameCycles_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "CYCLE_DELETE":
					DeleteDomainNameCycle(Convert.ToInt32(e.CommandArgument));
					break;
				case "CYCLE_MOVEDOWN":
					FlipDomainNameCycles(Convert.ToInt32(e.CommandArgument), true);
					break;
				case "CYCLE_MOVEUP":
					FlipDomainNameCycles(Convert.ToInt32(e.CommandArgument), false);
					break;
			}
		}

		protected void btnAddCycle_Click(object sender, EventArgs e)
		{
			AddBillingCycleToDomainName();
		}

		public void LoadDomainNameCycles(DomainNameCycle[] cycles)
		{
			// convert
			List<DomainNameCycle> data = new List<DomainNameCycle>();
			for (int i = 0; i < cycles.Length; i++)
				data.Add(cycles[i]);
			// save
			ViewState[VIEWSTATE_KEY] = data;
			// bind cycles
			BindDomainNameCycles();
		}

		public DomainNameCycle[] GetDomainNameCycles()
		{
			// sync entered data prior
			SyncGridViewDataEntered();

			// return data syncronized
			return DomainBillingCycles.ToArray();
		}

		private void LoadBillingCyclesDDL()
		{
			int cyclesCount = DomainBillingCycles.Count;
			// load taken cycles
			int[] cyclesTaken = new int[cyclesCount];
			// build array
			for (int i = 0; i < cyclesCount; i++)
				cyclesTaken[i] = DomainBillingCycles[i].CycleId;

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
			if (DomainBillingCycles.Count > 0)
			{
				for (int i = 0; i < gvDomainNameCycles.Rows.Count; i++)
				{
					GridViewRow gv_row = gvDomainNameCycles.Rows[i];
					DomainNameCycle gv_cycle = DomainBillingCycles[i];

					TextBox txtSetupFee = ecUtils.FindControl<TextBox>(gv_row, "txtSetupFee");
					TextBox txtRecurringFee = ecUtils.FindControl<TextBox>(gv_row, "txtRecurringFee");
					TextBox txtTransferFee = ecUtils.FindControl<TextBox>(gv_row, "txtTransferFee");

					gv_cycle.SetupFee = ecUtils.ParseDecimal(txtSetupFee.Text, 0);
					gv_cycle.RecurringFee = ecUtils.ParseDecimal(txtRecurringFee.Text, 0);
					gv_cycle.TransferFee = ecUtils.ParseDecimal(txtTransferFee.Text, 0);
				}
			}
		}

		private void BindDomainNameCycles()
		{
			// bind cycles
			gvDomainNameCycles.DataSource = DomainBillingCycles;
			gvDomainNameCycles.DataBind();
		}

		private void AddBillingCycleToDomainName()
		{
			// sync entered data prior
			SyncGridViewDataEntered();

			// load selected billing cycle
			BillingCycle chosenCycle = StorehouseHelper.GetBillingCycle(Convert.ToInt32(ddlBillingCycles.SelectedValue));
			// convert billing to domain name cycle
			DomainNameCycle cycle = new DomainNameCycle();
			// fill fields
			cycle.CycleId = chosenCycle.CycleId;
			cycle.CycleName = chosenCycle.CycleName;
			cycle.BillingPeriod = chosenCycle.BillingPeriod;
			cycle.PeriodLength = chosenCycle.PeriodLength;
			// put into viewstate
			DomainBillingCycles.Add(cycle);

			// bind new domain cycles
			BindDomainNameCycles();

			// re-load billing cycles
			LoadBillingCyclesDDL();
		}

		private void FlipDomainNameCycles(int indexFrom, bool moveDown)
		{
			// last item can't move down
			if (moveDown && indexFrom == DomainBillingCycles.Count - 1)
				return;
			// first item can't move up
			if (!moveDown && indexFrom == 0)
				return;
			// single item can't move in both directions
			if (DomainBillingCycles.Count == 1)
				return;

			// sync data prior
			SyncGridViewDataEntered();

			DomainNameCycle cycleToMove = DomainBillingCycles[indexFrom];
			// remove
			DomainBillingCycles.RemoveAt(indexFrom);
			// insert
			if (moveDown)
				DomainBillingCycles.Insert(indexFrom + 1, cycleToMove);
			else
				DomainBillingCycles.Insert(indexFrom - 1, cycleToMove);

			// re-bind data
			BindDomainNameCycles();
		}

		private void DeleteDomainNameCycle(int dataItemIndex)
		{
			// remove cycle
			DomainBillingCycles.RemoveAt(dataItemIndex);

			// re-bind cycles
			BindDomainNameCycles();

			// re-load billing cycles
			LoadBillingCyclesDDL();
		}
	}
}