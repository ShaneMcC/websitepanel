using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class CreateOrganization : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // apply password policy
                administratorPassword.SetPackagePolicy(PanelSecurity.PackageId, UserSettings.EXCHANGE_HOSTED_EDITION_POLICY, "MailboxPasswordPolicy");
            }
        }

        protected void createOrganization_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                string orgDomain = domain.Text.Trim();
                string adminEmail = administratorEmail.Text.Trim() + "@" + orgDomain;

                IntResult result = ES.Services.ExchangeHostedEdition.CreateExchangeOrganization(PanelSecurity.PackageId,
                    organizationName.Text.Trim(),
                    orgDomain,
                    administratorName.Text.Trim(),
                    adminEmail,
                    administratorPassword.Password);

                if (result.IsSuccess)
                {
                    // navigate to details
                    Response.Redirect(NavigateURL("SpaceID", PanelSecurity.PackageId.ToString(), "ItemID=" + result.Value.ToString()));
                }
                else
                {
                    // display message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_CREATE_ORGANIZATION", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_CREATE_ORGANIZATION", ex);
            }
        }
    }
}