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
	public partial class StorefrontWelcomeEdit : ecModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadWelcomeMessage();
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			SaveWelcomeMessage();
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			RedirectToBrowsePage();
		}

		private void SaveWelcomeMessage()
		{
			try
			{
				StoreSettings settings = new StoreSettings();
				//
				settings["HtmlText"] = txtWelcomeMsg.Text;
				//
				int result = StorehouseHelper.SetStoreSettings(StoreSettings.WELCOME_MESSAGE, settings);
				//
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
				//
				RedirectToBrowsePage();
			}
			catch (Exception ex)
			{
				ShowErrorMessage("SAVE_WELCOME_MSG", ex);
			}
		}

		private void LoadWelcomeMessage()
		{
			try
			{
				StoreSettings settings = StorehouseHelper.GetStoreSettings(StoreSettings.WELCOME_MESSAGE);
				//
				txtWelcomeMsg.Text = settings["HtmlText"];
			}
			catch (Exception ex)
			{
				ShowErrorMessage("LOAD_WELCOME_MSG", ex);
			}
		}
	}
}