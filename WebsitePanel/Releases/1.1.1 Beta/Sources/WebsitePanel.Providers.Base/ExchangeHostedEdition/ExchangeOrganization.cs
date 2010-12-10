using System;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Providers.ExchangeHostedEdition
{
    public class ExchangeOrganization : ServiceProviderItem
    {
        // basic props
        public string ExchangeControlPanelUrl { get; set; }
        public string DistinguishedName { get; set; }
        public string ServicePlan { get; set; }
        public string ProgramId { get; set; }
        public string OfferId { get; set; }
        public string AdministratorName { get; set; }
        public string AdministratorEmail { get; set; }

        // domains
        public ExchangeOrganizationDomain[] Domains { get; set; }

        // this is real usage
        public int MailboxCount { get; set; }
        public int ContactCount { get; set; }
        public int DistributionListCount { get; set; }

        // these quotas are set in Exchange for the organization
        public int MailboxCountQuota { get; set; }
        public int ContactCountQuota { get; set; }
        public int DistributionListCountQuota { get; set; }

        // these quotas are set in the hosting plan + add-ons
        public int MaxDomainsCountQuota { get; set; }
        public int MaxMailboxCountQuota { get; set; }
        public int MaxContactCountQuota { get; set; }
        public int MaxDistributionListCountQuota { get; set; }

        // summary information
        public string SummaryInformation { get; set; }

        [Persistent]
        public string CatchAllAddress { get; set; }
    }
}
