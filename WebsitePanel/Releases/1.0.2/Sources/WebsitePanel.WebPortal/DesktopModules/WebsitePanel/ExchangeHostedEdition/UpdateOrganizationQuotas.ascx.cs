using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ExchangeHostedEdition;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class UpdateOrganizationQuotas : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrganizationQuotas();
            }
        }

        private void BindOrganizationQuotas()
        {
            // load organization details
            ExchangeOrganization org = null;
            try
            {
                org = ES.Services.ExchangeHostedEdition.GetExchangeOrganizationDetails(PanelRequest.ItemID);
                if (org == null)
                    throw new ArgumentNullException("Organization not found");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_GET_ORGANIZATION", ex);
                return;
            }

            // current quotas
            mailboxes.Text = org.MailboxCountQuota.ToString();
            contacts.Text = org.ContactCountQuota.ToString();
            distributionLists.Text = org.DistributionListCountQuota.ToString();

            // max quotas
            string maxQuotaFormat = GetLocalizedString("maxQuota.Text");
            maxMailboxes.Text = String.Format(maxQuotaFormat, org.MaxMailboxCountQuota);
            maxContacts.Text = String.Format(maxQuotaFormat, org.MaxContactCountQuota);
            maxDistributionLists.Text = String.Format(maxQuotaFormat, org.MaxDistributionListCountQuota);

            rangeMailboxes.MaximumValue = org.MaxMailboxCountQuota.ToString();
            rangeMailboxes.Enabled = (org.MaxMailboxCountQuota != -1);

            rangeContacts.MaximumValue = org.MaxContactCountQuota.ToString();
            rangeContacts.Enabled = (org.MaxContactCountQuota != -1);

            rangeDistributionLists.MaximumValue = org.MaxDistributionListCountQuota.ToString();
            rangeDistributionLists.Enabled = (org.MaxDistributionListCountQuota != -1);
          }

        protected void update_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // collect form data
                int mailboxesNumber = Utils.ParseInt(mailboxes.Text, 0);
                int contactsNumber = Utils.ParseInt(contacts.Text, 0);
                int distributionListsNumber = Utils.ParseInt(distributionLists.Text, 0);

                // call service
                ResultObject result = ES.Services.ExchangeHostedEdition.UpdateExchangeOrganizationQuotas(
                    PanelRequest.ItemID, mailboxesNumber, contactsNumber, distributionListsNumber);

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
    }
}