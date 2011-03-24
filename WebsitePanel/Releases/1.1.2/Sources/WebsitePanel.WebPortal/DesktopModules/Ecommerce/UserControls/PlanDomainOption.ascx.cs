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
	public partial class PlanDomainOption : ecControlBase
	{
		public const string VIEW_STATE_KEY = "__DomainOption";

		public int DomainOption
		{
			get { return (int)ViewState[VIEW_STATE_KEY]; }
			set
			{
				ViewState[VIEW_STATE_KEY] = value;
				//
				SyncDomainOption(value);
			}
		}

		public bool DomainChecked
		{
			get
			{
				if (DomainOption == HostingPlan.DOMAIN_HIDE)
					return false;
				//
				if (DomainOption == HostingPlan.DOMAIN_REQUIRED)
					return true;
				//
				EnsureChildControls();
				return chkSelected.Checked;
			}
		}

		public OrderItem DomainOrderItem
		{
			get
			{
				// return nothing
				if (DomainOption == HostingPlan.DOMAIN_HIDE || !DomainChecked)
					return null;
				// assemble a domain to be ordered
				OrderItem item = OrderItem.GetTopLevelDomainItem(SelectedDomain, SelectedCycle, DomainName);
				// add domain action flag
				if (rbTransferDomain.Checked)
					item["SPF_ACTION"] = DomainNameSvc.SPF_TRANSFER_ACTION;
				else if (rbRegisterNew.Checked)
					item["SPF_ACTION"] = DomainNameSvc.SPF_REGISTER_ACTION;
				else
					item["SPF_ACTION"] = DomainNameSvc.SPF_UPDATE_NS_ACTION;
				// sync extensions if any
				SyncDomainExtensionFields(item);
				// return assembled domain order item
				return item;
			}
		}

		public string DomainName
		{
			get
			{
				EnsureChildControls();
				//
				if (rbRegisterNew.Checked)
				{
					return EnsureDomainCorrectness(txtDomainReg.Text) + "." + ddlTopLevelDoms.SelectedItem.Text;
				}
				else if (rbTransferDomain.Checked)
				{
					return EnsureDomainCorrectness(txtDomainTrans.Text) + "." + ddlTopLevelDomsTrans.SelectedItem.Text;
				}
				// default
				return EnsureDomainCorrectness(txtDomainUpdate.Text);
			}
		}

		public int SelectedDomain
		{
			get
			{
				EnsureChildControls();
				if (rbRegisterNew.Checked)
					return Convert.ToInt32(ddlTopLevelDoms.SelectedValue);
				else if (rbTransferDomain.Checked)
					return Convert.ToInt32(ddlTopLevelDomsTrans.SelectedValue);

				return 0;
			}
		}

		public int SelectedCycle
		{
			get
			{
				EnsureChildControls();
				if (rbRegisterNew.Checked)
					return Convert.ToInt32(ddlDomCycles.SelectedValue);
				else if (rbTransferDomain.Checked)
					return Convert.ToInt32(ddlTransDomCycles.SelectedValue);

				return 0;
			}
		}

		protected int ResellerId
		{
			get
			{
				return (Page.User.Identity.IsAuthenticated) ? PanelSecurity.LoggedUser.OwnerId : 
					ecPanelRequest.ResellerId;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void btnCheckDomain_Click(object sender, EventArgs e)
		{
			ctxDomainValidator.Validate();
		}

		protected void ctxDomainValidator_EvaluatingContext(object sender, ManualValidationEventArgs e)
		{
			CheckDomain(e);
		}

		protected void chkSelected_OnCheckedChanged(object sender, EventArgs e)
		{
			ecUtils.ToggleControls(chkSelected.Checked, pnlTopLevelDomain);
		}

		protected void rbRegisterNew_OnCheckedChanged(object sender, EventArgs e)
		{
			// turn on
			ecUtils.ToggleControls(rbRegisterNew.Checked, pnlDomainReg);
			// turn off
			ecUtils.ToggleControls(false, pnlDomainTrans, pnlUpdateNs);
			//
			BindTopLevelDomains(ddlTopLevelDoms, false);
		}

		protected void rbTransferDomain_OnCheckedChanged(object sender, EventArgs e)
		{
			// turn on
			ecUtils.ToggleControls(rbTransferDomain.Checked, pnlDomainTrans);
			// turn off
			ecUtils.ToggleControls(false, pnlDomainReg, pnlUpdateNs);
			//
			BindTopLevelDomains(ddlTopLevelDomsTrans, true);
		}

		protected void rbUpdateNs_OnCheckedChanged(object sender, EventArgs e)
		{
			ecUtils.ToggleControls(true, pnlUpdateNs);
			ecUtils.ToggleControls(false, pnlDomainReg, pnlDomainTrans);
		}

		protected void ddlTopLevelDoms_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			BindDomainCyclesForRegistration();
			ShowExtensionFields();
		}

		protected void ddlTransDomCycles_OnSelectedIndexChanged(object sender, EventArgs e)
		{
			BindDomainCyclesForTransfer();
		}

		private void CheckDomain(ManualValidationEventArgs args)
		{
			// ensure domain name not empty
			reqDomainVal.Validate();
			//
			if (!reqDomainVal.IsValid)
				return;

			string domain = EnsureDomainCorrectness(txtDomainReg.Text);
			//
			CheckDomainResult result = StorefrontHelper.CheckDomain(ResellerId, domain, ddlTopLevelDoms.SelectedItem.Text);

			// domain is available for purchase
			if (result.Succeed && result.ResultCode == 0)
			{
				//
				lblDomainAvailable.Visible = true;
				//
				args.ContextIsValid = true;
				//
				return;
			}

			//
			if (result.Succeed && result.ResultCode != 0)
			{
				//
				args.ContextIsValid = false;
				// show error message
				ctxDomainValidator.ErrorMessage = GetSharedLocalizedString(Keys.ModuleName, "CHECK_DOMAIN." + (result.ResultCode * -1));
				//
				return;
			}

			//
			if (!result.Succeed)
			{
				//
				args.ContextIsValid = false;
				//
				ctxDomainValidator.ErrorMessage = result.ErrorMessage;
				//
				return;
			}
			
			//
			args.ContextIsValid = false;
		}

		private string EnsureDomainCorrectness(string domain)
		{
			string domainName = domain.Trim();

			if (domainName.StartsWith("www."))
				domainName = domainName.Substring(4);

			return domainName;
		}

		private void SyncDomainOption(int domainOption)
		{
			// check for tlds in stock
			if (!StorefrontHelper.HasTopLevelDomainsInStock(ResellerId))
			{
				ecUtils.ToggleControls(false, pnlTopLevelDomain, chkSelected);
				ecUtils.ToggleControls(true, lclNoTLDsInStock);
				return;
			}

			switch (domainOption)
			{
				case HostingPlan.DOMAIN_OPTIONAL:
					// turn on
					ecUtils.ToggleControls(true, chkSelected, this);
					// turn off
					ecUtils.ToggleControls(false, pnlTopLevelDomain);
					break;
				case HostingPlan.DOMAIN_REQUIRED:
					// turn on
					ecUtils.ToggleControls(true, this, pnlTopLevelDomain);
					// turn off
					ecUtils.ToggleControls(false, chkSelected);
					//
					chkSelected.Checked = false;
					break;
				case HostingPlan.DOMAIN_HIDE:
					// turn off
					ecUtils.ToggleControls(false, this);
					//
					chkSelected.Checked = false;
					break;
			}
		}

		private void BindTopLevelDomains(DropDownList ctl_list, bool transfer)
		{
			ctl_list.DataSource = StorefrontHelper.GetStorefrontProductsByType(ResellerId, 
				Product.TOP_LEVEL_DOMAIN);
			ctl_list.DataBind();
			// bind cycles
			if (transfer)
				BindDomainCyclesForTransfer();
			else
				BindDomainCyclesForRegistration();
			//
			ShowExtensionFields();
		}

		private void ShowExtensionFields()
		{
			if (ddlTopLevelDoms.SelectedItem.Text == "us")
			{
				ecUtils.ToggleControls(true, usTldFields);
				ecUtils.ToggleControls(false, euTldFields, ukTldFields);
			}
			else if (ddlTopLevelDoms.SelectedItem.Text == "eu")
			{
				ecUtils.ToggleControls(true, euTldFields);
				ecUtils.ToggleControls(false, usTldFields, ukTldFields);
			}
			else if (ddlTopLevelDoms.SelectedItem.Text.EndsWith(".uk"))
			{
				ecUtils.ToggleControls(true, ukTldFields);
				ecUtils.ToggleControls(false, usTldFields, euTldFields);
			}
		}

		private void SyncDomainExtensionFields(OrderItem domainItem)
		{
			// skip sync when user would like to update name servers only
			if (rbUpdateNs.Checked)
				return;
			//
			if (ddlTopLevelDoms.SelectedItem.Text == "us")
			{
				domainItem["NexusCategory"] = ddlNexusCategory.SelectedValue;
				domainItem["ApplicationPurpose"] = ddlAppPurpose.SelectedValue;
			}
			else if (ddlTopLevelDoms.SelectedItem.Text.EndsWith(".uk"))
			{
				domainItem["RegisteredFor"] = txtRegisteredFor.Text.Trim();
				domainItem["UK_LegalType"] = ddlUkLegalType.SelectedValue;
				domainItem["UK_CompanyIdNumber"] = txtCompanyIdNum.Text.Trim();
				domainItem["HideWhoisInfo"] = chkHideWhoisInfo.Checked ? "y" : "n";
			}
			else if (ddlTopLevelDoms.SelectedItem.Text.EndsWith("eu"))
			{
				domainItem["EU_WhoisPolicy"] = chkDtPolicyAgree.Checked ? "I Agree" : "";
				domainItem["EU_AgreeDelete"] = chkDelPolicyAgree.Checked ? "YES" : "";
				domainItem["EU_ADRLang"] = ddlEuAdrLang.SelectedValue;
			}
		}

		private void BindDomainCyclesForRegistration()
		{
			// try parse product id
			int productId = ecUtils.ParseInt(ddlTopLevelDoms.SelectedValue, 0);
			// bind
			if (productId > 0)
			{
				DomainNameCycle[] cycles = StorefrontHelper.GetTopLevelDomainCycles(
					ResellerId, productId);
				// cleanup all of items
				ddlDomCycles.Items.Clear();
				// re-create items from scratch
				foreach (DomainNameCycle cycle in cycles)
				{
					ddlDomCycles.Items.Add(CreateDomainCycleItem(cycle, false));
				}
			}
		}

		private void BindDomainCyclesForTransfer()
		{
			// try parse product id
			int productId = ecUtils.ParseInt(ddlTopLevelDomsTrans.SelectedValue, 0);
			// bind
			if (productId > 0)
			{
				DomainNameCycle[] cycles = StorefrontHelper.GetTopLevelDomainCycles(
					ResellerId, productId);
				// cleanup all of items
				ddlTransDomCycles.Items.Clear();
				// re-create items from scratch
				foreach (DomainNameCycle cycle in cycles)
				{
					if (cycle.TransferFee > 0)
						ddlTransDomCycles.Items.Add(CreateDomainCycleItem(cycle, true));
				}
			}
		}

		private ListItem CreateDomainCycleItem(DomainNameCycle cycleItem, bool transfer)
		{
			if (transfer)
				return new ListItem(
					String.Format("{0} - {1} {2:C} + {3} {4:C}",
						cycleItem.CycleName, EcommerceSettings.CurrencyCodeISO,
						cycleItem.TransferFee, EcommerceSettings.CurrencyCodeISO, cycleItem.SetupFee),
					cycleItem.CycleId.ToString()
				);
			else
				return new ListItem(
					String.Format("{0} - {1} {2:C} + {3} {4:C}",
						cycleItem.CycleName, EcommerceSettings.CurrencyCodeISO,
						cycleItem.RecurringFee, EcommerceSettings.CurrencyCodeISO, cycleItem.SetupFee),
					cycleItem.CycleId.ToString()
				);
		}
	}
}