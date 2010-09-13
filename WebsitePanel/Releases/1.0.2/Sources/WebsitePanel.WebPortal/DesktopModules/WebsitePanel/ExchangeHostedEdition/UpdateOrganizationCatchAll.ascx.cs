using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ExchangeHostedEdition;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class UpdateOrganizationCatchAll : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrganizationDomains();
                BindCatchAllMode();
            }
        }

        private void BindOrganizationDomains()
        {
            try
            {
                // read domains
                ExchangeOrganizationDomain[] orgDomains = ES.Services.ExchangeHostedEdition.GetExchangeOrganizationDomains(PanelRequest.ItemID);

                // bind domains
                domains.DataSource = orgDomains;
                domains.DataBind();

                // select default domain
                foreach (ExchangeOrganizationDomain domain in orgDomains)
                {
                    if (domain.IsDefault)
                    {
                        domains.SelectedValue = domain.Name;
                        break;
                    }
                }

                // insert empty item in the beginning
                domains.Items.Insert(0, new ListItem(GetLocalizedString("SelectDomain.Text"), ""));
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_GET_DOMAINS", ex);
            }
        }

        private void BindCatchAllMode()
        {
            rawCatchAllAddress.Visible = (catchAllMode.SelectedIndex > 0);
        }

        protected void update_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // collect form data
                string catchAllEmail = catchAllAddress.Text.Trim() + "@" + domains.SelectedValue;
                if (catchAllMode.SelectedIndex == 0)
                {
                    catchAllEmail = ""; // disabled
                }

                // call service
                ResultObject result = ES.Services.ExchangeHostedEdition.UpdateExchangeOrganizationCatchAllAddress(
                    PanelRequest.ItemID, catchAllEmail);

                // check results
                if (result.IsSuccess)
                {
                    // navigate to details
                    RedirectBack();
                }
                else
                {
                    // display message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_CHANGE_QUOTAS", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_CHANGE_QUOTAS", ex);
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

        protected void catchAllMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCatchAllMode();
        }
    }
}