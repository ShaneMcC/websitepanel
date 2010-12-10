using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class DeleteOrganization : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void delete_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (!confirmDelete.Checked)
            {
                messageBox.ShowWarningMessage("EXCHANGE_HOSTED_CONFIRM_DELETE_ORGANIZATION");
                return;
            }

            try
            {
                ResultObject result = ES.Services.ExchangeHostedEdition.DeleteExchangeOrganization(PanelRequest.ItemID);

                if (result.IsSuccess)
                {
                    // navigate to details
                    Response.Redirect(NavigateURL("SpaceID", PanelSecurity.PackageId.ToString()));
                }
                else
                {
                    // display message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_DELETE_ORGANIZATION", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_DELETE_ORGANIZATION", ex);
            }
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(NavigateURL("SpaceID", PanelSecurity.PackageId.ToString(), "ItemID=" + PanelRequest.ItemID.ToString()));
        }
    }
}