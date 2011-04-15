using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class AddOrganizationDomain : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void update_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // call service
                ResultObject result = ES.Services.ExchangeHostedEdition.AddExchangeOrganizationDomain(PanelRequest.ItemID, domain.Text.Trim());

                // check results
                if (result.IsSuccess)
                {
                    // navigate to details
                    RedirectBack();
                }
                else
                {
                    // display message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_ADD_DOMAIN", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_ADD_DOMAIN", ex);
            }
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            RedirectBack();
        }

        private void RedirectBack()
        {
            Response.Redirect(NavigateURL("SpaceID", PanelSecurity.PackageId.ToString(), "ItemID=" + PanelRequest.ItemID.ToString()));
        }
    }
}