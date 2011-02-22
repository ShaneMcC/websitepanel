using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebsitePanel.Portal
{
	public partial class SetupControlPanelAccounts : WebsitePanelModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				EnsureSCPAEnabled();
			}
		}

		protected void CompleteSetupButton_Click(object sender, EventArgs e)
		{
			if (Page.IsValid == false)
			{
				return;
			}
			//
			CompleteSetupControlPanelAccounts();
		}

		private void EnsureSCPAEnabled()
		{
			var enabledScpa = ES.Services.Authentication.GetSystemSetupMode();
			//
			if (enabledScpa == false)
			{
				Response.Redirect(EditUrl(""));
			}
		}

		private void CompleteSetupControlPanelAccounts()
		{
			var resultCode = ES.Services.Authentication.SetupControlPanelAccounts(PasswordControlA.Password, PasswordControlB.Password, Request.UserHostAddress);
			//
			if (resultCode < 0)
			{
				ShowResultMessage(resultCode);
				//
				return;
			}
			//
			Response.Redirect(EditUrl("u", "admin", String.Empty, String.Format("p={0}", PasswordControlB.Password)));
		}
	}
}
