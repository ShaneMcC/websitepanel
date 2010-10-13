using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ExchangeHostedEdition;

namespace WebsitePanel.Portal.ExchangeHostedEdition
{
    public partial class OrganizationDetails : WebsitePanelModuleBase
    {
        private int ItemID
        {
            get { return (ViewState["ItemID"] != null) ? (int)ViewState["ItemID"] : 0; }
            set { ViewState["ItemID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // get organization details
            ExchangeOrganization[] orgs = ES.Services.ExchangeHostedEdition.GetOrganizations(PanelSecurity.PackageId);

            if (orgs.Length == 0)
            {
                // create a new organization
                Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "create_org"));
                return;
            }
            else
            {
                // bind
                if(!IsPostBack)
                    BindOrganization(orgs[0].Id);
            }
        }

        private void BindOrganization(int itemId)
        {
            // load organization details
            ExchangeOrganization org = null;
            try
            {
                org = ES.Services.ExchangeHostedEdition.GetExchangeOrganizationDetails(itemId);
                if (org == null)
                    throw new ArgumentNullException("Organization not found");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_GET_ORGANIZATION", ex);
                return;
            }

            // basic details
            ItemID = org.Id;
            organizationName.Text = org.Name;
            administratorName.Text = org.AdministratorName;
            administratorEmail.Text = org.AdministratorEmail;
            ecpURL.Text = org.ExchangeControlPanelUrl;
            ecpURL.NavigateUrl = org.ExchangeControlPanelUrl;

            // service plan
            serviceName.Text = org.ServicePlan;
            programID.Text = org.ProgramId;
            offerID.Text = org.OfferId;
            servicePlanBlock.Visible = (PanelSecurity.LoggedUser.Role == EnterpriseServer.UserRole.Administrator);

            // quotas
            string quotaFormat = GetLocalizedString("quota.FormatText");
            mailboxes.Text = String.Format(quotaFormat, org.MailboxCount, FormatUnlimited(org.MailboxCountQuota), FormatUnlimited(org.MaxMailboxCountQuota));
            contacts.Text = String.Format(quotaFormat, org.ContactCount, FormatUnlimited(org.ContactCountQuota), FormatUnlimited(org.MaxContactCountQuota));
            distributionLists.Text = String.Format(quotaFormat, org.DistributionListCount, FormatUnlimited(org.DistributionListCountQuota), FormatUnlimited(org.MaxDistributionListCountQuota));


            // catch-all
            //catchAllAddress.Text = !String.IsNullOrEmpty(org.CatchAllAddress) ? org.CatchAllAddress : GetLocalizedString("catchAllNotSet.Text");

            // domains
            BindOrganizationDomains(org);

            // summary
            BindOrganizationSummary(org);
        }

        private string FormatUnlimited(int num)
        {
            return (num != -1) ? num.ToString() : GetLocalizedString("unlimited.Text");
        }

        private void BindOrganizationDomains(ExchangeOrganization org)
        {
            try
            {
                // bind grid
                gvDomains.DataSource = org.Domains;
                gvDomains.DataBind();

                // set gauge
                domainsQuota.QuotaValue = org.MaxDomainsCountQuota;
                domainsQuota.QuotaUsedValue = org.Domains.Length;
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_GET_DOMAINS", ex);
                return;
            }
        }

        private void BindOrganizationSummary(ExchangeOrganization org)
        {
            try
            {
                string summaryText = ES.Services.ExchangeHostedEdition.GetExchangeOrganizationSummary(org.Id);
                setupInstructions.Text = !String.IsNullOrEmpty(summaryText) ? summaryText : GetLocalizedString("summaryTemplateNotSet.Text");

                // hide block if template is not set
                organizationSummary.Visible = !String.IsNullOrEmpty(summaryText) || (PanelSecurity.LoggedUser.Role != EnterpriseServer.UserRole.User);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_GET_DOMAINS", ex);
                return;
            }
        }

        protected void gvDomains_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // delete domain
            DeleteDomain(gvDomains.DataKeys[e.RowIndex][0].ToString());

            // cancel event
            e.Cancel = true;
        }

        private void DeleteDomain(string domainName)
        {
            try
            {
                // call service
                ResultObject result = ES.Services.ExchangeHostedEdition.DeleteExchangeOrganizationDomain(ItemID, domainName);

                // check results
                if (result.IsSuccess)
                {
                    // refresh details
                    BindOrganization(ItemID);
                }
                else
                {
                    // display message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_DELETE_DOMAIN", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_DELETE_DOMAIN", ex);
            }
        }

        protected void deleteOrganization_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", ItemID.ToString(), "delete_org", "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void changeServicePlan_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", ItemID.ToString(), "update_org_plan", "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void updateQuotas_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", ItemID.ToString(), "update_org_quotas", "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", ItemID.ToString(), "add_org_domain", "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void setCatchAll_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ItemID", ItemID.ToString(), "update_org_catchall", "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }

        protected void sendSetupInstructions_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                ResultObject result = ES.Services.ExchangeHostedEdition.SendExchangeOrganizationSummary(ItemID, sendTo.Text.Trim());

                if (result.IsSuccess)
                {
                    // display success message
                    messageBox.ShowSuccessMessage("EXCHANGE_HOSTED_SEND_SUMMARY");
                }
                else
                {
                    // display error message
                    messageBox.ShowMessage(result, "EXCHANGE_HOSTED_SEND_SUMMARY", "ExchangeHostedEdition");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_HOSTED_SEND_SUMMARY", ex);
            }
        }
    }
}