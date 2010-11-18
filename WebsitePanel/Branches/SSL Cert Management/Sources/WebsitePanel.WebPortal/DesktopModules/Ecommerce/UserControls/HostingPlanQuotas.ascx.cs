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

using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.UserControls
{
	public partial class HostingPlanQuotas : ecControlBase
	{
		public KeyValueBunch PlanQuotas
		{
			set
			{
				//
				EnsureChildControls();
				//
				BindPlanQuotas(value);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		private void BindPlanQuotas(KeyValueBunch quotas)
		{
			string[] groupKeys = quotas.GetAllKeys();
			//
			foreach (string groupKey in groupKeys)
			{
				//
				PlaceHolder container = (PlaceHolder)FindControl(groupKey);
				//
				if (container != null)
					container.Visible = true;

				//
				Repeater repeater = (Repeater)FindControl(groupKey + "_Quotas");
				//
				if (repeater != null)
				{
					//
					repeater.DataSource = quotas[groupKey].Split(',');
					//
					repeater.DataBind();
				}
			}
		}

		public string GetQuotaItemName(string record)
		{
			return record.Split('=')[0];
		}

		public string GetQuotaItemAllocatedValue(string record)
		{
			string[] pair = record.Split('=');
			//
			if (pair[1] == "Enabled")
				return GetSharedLocalizedString(Keys.ModuleName, "Quota.Enabled");
			else if (pair[1] == "Disabled")
				return GetSharedLocalizedString(Keys.ModuleName, "Quota.Disabled");
			//
			return pair[1];
		}
	}
}