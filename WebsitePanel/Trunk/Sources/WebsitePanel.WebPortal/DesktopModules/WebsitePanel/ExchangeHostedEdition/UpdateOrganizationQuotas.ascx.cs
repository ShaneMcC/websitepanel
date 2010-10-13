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
            mailboxes.Text = IsUnlimited(org.MailboxCountQuota) ? "" : org.MailboxCountQuota.ToString();
            contacts.Text = IsUnlimited(org.ContactCountQuota) ? "" : org.ContactCountQuota.ToString();
            distributionLists.Text = IsUnlimited(org.DistributionListCountQuota) ? "" : org.DistributionListCountQuota.ToString();

            // max quotas
            string maxQuotaFormat = GetLocalizedString("maxQuota.Text");
            maxMailboxes.Text = String.Format(maxQuotaFormat, FormatUnlimited(org.MaxMailboxCountQuota));
            maxContacts.Text = String.Format(maxQuotaFormat, FormatUnlimited(org.MaxContactCountQuota));
            maxDistributionLists.Text = String.Format(maxQuotaFormat, FormatUnlimited(org.MaxDistributionListCountQuota));

            if (!IsUnlimited(org.MaxMailboxCountQuota))
            {
                requireMailboxes.Enabled = true;
                rangeMailboxes.MaximumValue = org.MaxMailboxCountQuota.ToString();
            }

            if (!IsUnlimited(org.MaxContactCountQuota))
            {
                requireContacts.Enabled = true;
                rangeContacts.MaximumValue = org.MaxContactCountQuota.ToString();
            }

            if (!IsUnlimited(org.MaxDistributionListCountQuota))
            {
                requireDistributionLists.Enabled = true;
                rangeDistributionLists.MaximumValue = org.MaxDistributionListCountQuota.ToString();
            }
        }

        private bool IsUnlimited(int num)
        {
            return (num == -1);
        }

        private string FormatUnlimited(int num)
        {
            return IsUnlimited(num) ? GetLocalizedString("unlimited.Text") : num.ToString();
        }

        protected void update_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                // collect form data
                int mailboxesNumber = (mailboxes.Text.Trim() == "") ? -1 : Utils.ParseInt(mailboxes.Text, 0);
                int contactsNumber = (contacts.Text.Trim() == "") ? -1 : Utils.ParseInt(contacts.Text, 0);
                int distributionListsNumber = (distributionLists.Text.Trim() == "") ? -1 : Utils.ParseInt(distributionLists.Text, 0);

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