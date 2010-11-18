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

namespace WebsitePanel.Ecommerce.Portal.UserControls
{
	public partial class QuickHostingAddon : ecControlBase
	{
		public const string VIEW_STATE_KEY = "__QHostingAddon";

		public HostingAddon AddonInfo
		{
			get { return (HostingAddon)ViewState[VIEW_STATE_KEY]; }
			set { ViewState[VIEW_STATE_KEY] = value; }
		}

		public bool Selected
		{
			get
			{
				EnsureChildControls();
				return chkSelected.Checked;
			}
		}

		public int Quantity
		{
			get
			{
				EnsureChildControls();
				//
				if (AddonInfo.Countable)
					return ecUtils.ParseInt(txtQuantity.Text, 1);
				//
				return 1;
			}
		}

		public int SelectedCycleId
		{
			get
			{
				EnsureChildControls();
				return SyncSelectedCycle();
			}
		}

		public OrderItem AddonOrderItem
		{
			get
			{
				if (!chkSelected.Checked)
					return null;
				//
				return OrderItem.GetHostingAddonItem(AddonInfo.ProductId, SelectedCycleId, Quantity,
					AddonInfo.ProductName);
			}
		}

		protected string GroupName
		{
			get { return String.Concat("RadioGroup_", AddonInfo.ProductId); }
		}

		protected void Page_Load(object sender, EventArgs e)
		{
				
		}

		protected void chkSelected_CheckedChanged(object sender, EventArgs e)
		{
			ShowAddonInfo();
		}

		protected override void OnDataBinding(EventArgs e)
		{
			base.OnDataBinding(e);
			//
			EnsureChildControls();
			ShowAddonInfo();
		}

		private void ShowAddonInfo()
		{
			// ignore empty addons
			if (AddonInfo == null)
				return;
			//
			ltrAddonName.Text = AddonInfo.ProductName;
			//
			if (chkSelected.Checked)
			{
				if (AddonInfo.Recurring)
					ShowRecurringAddon();
				else
					ShowOneTimeAddon();
			}
			else
				ecUtils.ToggleControls(false, pnlOneTimeFee, pnlRecurring, lclAddonPrice, lclSetupFee, pnlQuantity);
		}

		private void ShowRecurringAddon()
		{
			// turn off
			ecUtils.ToggleControls(false, pnlOneTimeFee);
			// turn on
			if (AddonInfo.Countable)
				ecUtils.ToggleControls(true, pnlRecurring, lclAddonPrice, lclSetupFee, pnlQuantity);
			else
				ecUtils.ToggleControls(true, pnlRecurring, lclAddonPrice, lclSetupFee);
			
			int resellerId = (Page.User.Identity.IsAuthenticated) ? PanelSecurity.SelectedUser.OwnerId : ecPanelRequest.ResellerId;
			rptAddonCycles.DataSource = StorefrontHelper.GetHostingAddonCycles(resellerId, AddonInfo.ProductId);
			rptAddonCycles.DataBind();
		}

		private void ShowOneTimeAddon()
		{
			// turn off
			ecUtils.ToggleControls(false, pnlRecurring);
			// turn on
			if (AddonInfo.Countable)
				ecUtils.ToggleControls(true, pnlOneTimeFee, pnlQuantity, lclAddonPrice, lclSetupFee);
			else
				ecUtils.ToggleControls(true, pnlOneTimeFee, lclAddonPrice, lclSetupFee);

			ltrAddonPrice.Text = AddonInfo.OneTimeFee.ToString("C");
			ltrSetupFee.Text = AddonInfo.SetupFee.ToString("C");
		}

		private int SyncSelectedCycle()
		{
			if (!AddonInfo.Recurring)
				return -1;

			foreach (RepeaterItem item in rptAddonCycles.Items)
			{
				GroupRadioButton r_button = ecUtils.FindControl<GroupRadioButton>(item, "rbSelected");
				//
				if (r_button != null && r_button.Checked)
					return (int)r_button.ControlValue;
			}

			return -1;
		}
	}
}