using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.ExchangeHostedEdition;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class UpdateOrganizationServicePlan : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindServices();
                BindOrganizationDetails();
            }
        }

        private void BindServices()
        {
            // bind
            services.DataSource = ES.Services.Servers.GetRawServicesByGroupName(ResourceGroups.ExchangeHostedEdition).Tables[0].DefaultView;
            services.DataBind();

            // insert empty item
            services.Items.Insert(0, new ListItem(GetLocalizedString("SelectService.Text"), ""));
        }

        private void BindOrganizationDetails()
        {
            ExchangeOrganization org = ES.Services.ExchangeHostedEdition.GetExchangeOrganizationDetails(PanelRequest.ItemID);
            if (org == null)
                return;

            // selected service
            ListItem sourceItem = null;
            foreach (ListItem item in services.Items)
            {
                if (item.Value == org.ServiceId.ToString())
                {
                    sourceItem = item;
                    currentServiceName.Text = item.Text;
                    break;
                }
            }

            if (sourceItem != null)
                services.Items.Remove(sourceItem);

            currentProgramID.Text = org.ProgramId;
            currentOfferID.Text = org.OfferId;
        }

        private void BindSelectedService()
        {
            int newServiceId = Utils.ParseInt(services.SelectedValue, 0);

            // get service settings
            string[] settings = ES.Services.Servers.GetServiceSettings(newServiceId);

            foreach (string setting in settings)
            {
                string[] pair = setting.Split('=');
                if(String.Equals(pair[0], "ProgramID", StringComparison.InvariantCultureIgnoreCase))
                    newProgramID.Text = pair[1];
                else if (String.Equals(pair[0], "OfferID", StringComparison.InvariantCultureIgnoreCase))
                    newOfferID.Text = pair[1];
            }
        }

        protected void services_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSelectedService();
        }

        protected void apply_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // collect form data
                int serviceId = Utils.ParseInt(services.SelectedValue, 0);

                // call service
                ResultObject result = ES.Services.ExchangeHostedEdition.UpdateExchangeOrganizationServicePlan(PanelRequest.ItemID, serviceId);

                // check results
                if (result.IsSuccess)
                {
                    // navigate to details
                    RedirectBack();
                }
                else
                {
                    // display message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_CHANGE_SERVICE_PLAN", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_CHANGE_SERVICE_PLAN", ex);
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