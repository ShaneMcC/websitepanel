using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Microsoft.Web.Services3;
using System.ComponentModel;
using WebsitePanel.Providers.ExchangeHostedEdition;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esExchangeHostedEdition
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esExchangeHostedEdition : WebService
    {
        [WebMethod]
        public List<ExchangeOrganization> GetOrganizations(int packageId)
        {
            return ExchangeHostedEditionController.GetOrganizations(packageId);
        }

        [WebMethod]
        public IntResult CreateExchangeOrganization(int packageId, string organizationId,
            string domain, string adminName, string adminEmail, string adminPassword)
        {
            return ExchangeHostedEditionController.CreateOrganization(packageId, organizationId, domain, adminName, adminEmail, adminPassword);
        }

        [WebMethod]
        public ExchangeOrganization GetExchangeOrganizationDetails(int itemId)
        {
            return ExchangeHostedEditionController.GetOrganizationDetails(itemId);
        }

        [WebMethod]
        public List<ExchangeOrganizationDomain> GetExchangeOrganizationDomains(int itemId)
        {
            return ExchangeHostedEditionController.GetOrganizationDomains(itemId);
        }

        [WebMethod]
        public string GetExchangeOrganizationSummary(int itemId)
        {
            return ExchangeHostedEditionController.GetExchangeOrganizationSummary(itemId);
        }

        [WebMethod]
        public ResultObject SendExchangeOrganizationSummary(int itemId, string toEmail)
        {
            return ExchangeHostedEditionController.SendExchangeOrganizationSummary(itemId, toEmail);
        }

        [WebMethod]
        public ResultObject AddExchangeOrganizationDomain(int itemId, string domain)
        {
            return ExchangeHostedEditionController.AddOrganizationDomain(itemId, domain);
        }

        [WebMethod]
        public ResultObject DeleteExchangeOrganizationDomain(int itemId, string domain)
        {
            return ExchangeHostedEditionController.DeleteOrganizationDomain(itemId, domain);
        }

        [WebMethod]
        public ResultObject UpdateExchangeOrganizationQuotas(int itemId, int mailboxesNumber, int contactsNumber, int distributionListsNumber)
        {
            return ExchangeHostedEditionController.UpdateOrganizationQuotas(itemId, mailboxesNumber, contactsNumber, distributionListsNumber);
        }

        [WebMethod]
        public ResultObject UpdateExchangeOrganizationCatchAllAddress(int itemId, string catchAllEmail)
        {
            return ExchangeHostedEditionController.UpdateOrganizationCatchAllAddress(itemId, catchAllEmail);
        }

        [WebMethod]
        public ResultObject UpdateExchangeOrganizationServicePlan(int itemId, int newServiceId)
        {
            return ExchangeHostedEditionController.UpdateOrganizationServicePlan(itemId, newServiceId);
        }

        [WebMethod]
        public ResultObject DeleteExchangeOrganization(int itemId)
        {
            return ExchangeHostedEditionController.DeleteOrganization(itemId);
        }
    }
}
