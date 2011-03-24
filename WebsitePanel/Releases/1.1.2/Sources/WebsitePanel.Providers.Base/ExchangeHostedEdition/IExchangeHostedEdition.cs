using System;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Providers.ExchangeHostedEdition
{
    public interface IExchangeHostedEdition
    {
        void CreateOrganization(string organizationId, string programId, string offerId,
            string domain, string adminName, string adminEmail, string adminPassword);
        ExchangeOrganization GetOrganizationDetails(string organizationId);

        List<ExchangeOrganizationDomain> GetOrganizationDomains(string organizationId);
        void AddOrganizationDomain(string organizationId, string domain);
        void DeleteOrganizationDomain(string organizationId, string domain);
        
        void UpdateOrganizationQuotas(string organizationId, int mailboxesNumber, int contactsNumber, int distributionListsNumber);
        void UpdateOrganizationCatchAllAddress(string organizationId, string catchAllEmail);
        void UpdateOrganizationServicePlan(string organizationId, string programId, string offerId);
        void DeleteOrganization(string organizationId);
    }
}
