// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Web;
using WebsitePanel.Providers.ExchangeHostedEdition;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers;
using System.Collections.Specialized;
using System.Collections;
using System.Net.Mail;

namespace WebsitePanel.EnterpriseServer
{
    public class ExchangeHostedEditionController
    {
        // messages
        public const string GeneralError = "GeneralError";
        public const string ExchangeServiceNotEnabledError = "ExchangeServiceNotEnabledError";
        public const string ProgramIdIsNotSetError = "ProgramIdIsNotSetError";
        public const string OfferIdIsNotSetError = "OfferIdIsNotSetError";
        public const string CreateOrganizationError = "CreateOrganizationError";
        public const string OrganizationNotFoundError = "OrganizationNotFoundError";
        public const string SendOrganizationSummaryError = "SendOrganizationSummaryError";
        public const string SendOrganizationTemplateNotSetError = "SendOrganizationTemplateNotSetError";
        public const string AddDomainError = "AddDomainError";
        public const string AddDomainQuotaExceededError = "AddDomainQuotaExceededError";
        public const string AddDomainExistsError = "AddDomainExistsError";
        public const string AddDomainAlreadyUsedError = "AddDomainAlreadyUsedError";
        public const string DeleteDomainError = "DeleteDomainError";
        public const string UpdateQuotasError = "UpdateQuotasError";
        public const string UpdateQuotasWrongQuotaError = "UpdateQuotasWrongQuotaError";
        public const string UpdateCatchAllError = "UpdateCatchAllError";
        public const string UpdateServicePlanError = "UpdateServicePlanError";
        public const string DeleteOrganizationError = "DeleteOrganizationError";

        public const string TempDomainSetting = "temporaryDomain";
        public const string ExchangeControlPanelUrlSetting = "ecpURL";

        // other constants
        public const string TaskManagerSource = "ExchangeHostedEdition";

        public static IntResult CreateOrganization(int packageId, string organizationId,
            string domain, string adminName, string adminEmail, string adminPassword)
        {
            IntResult result = new IntResult();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "CREATE_ORGANIZATION");
                TaskManager.WriteParameter("packageId", packageId);
                TaskManager.WriteParameter("organizationId", organizationId);
                TaskManager.WriteParameter("domain", domain);
                TaskManager.WriteParameter("adminName", adminName);
                TaskManager.WriteParameter("adminEmail", adminEmail);
                TaskManager.WriteParameter("adminPassword", adminPassword);

                // get Exchange service ID
                int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.ExchangeHostedEdition);
                if(serviceId < 1)
                    return Error<IntResult>(ExchangeServiceNotEnabledError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<IntResult>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<IntResult>((-packageCheck).ToString());
                #endregion

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(serviceId);

                // load service settings to know ProgramID, OfferID
                StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
                string programId = serviceSettings["programID"];
                string offerId = serviceSettings["offerID"];

                // check settings
                if(String.IsNullOrEmpty(programId))
                    result.ErrorCodes.Add(ProgramIdIsNotSetError);
                if (String.IsNullOrEmpty(offerId))
                    result.ErrorCodes.Add(OfferIdIsNotSetError);

                if (result.ErrorCodes.Count > 0)
                {
                    result.IsSuccess = false;
                    return result;
                }

                #region Create organization
                int itemId = -1;
                ExchangeOrganization org = null;
                try
                {
                    // create organization
                    exchange.CreateOrganization(organizationId, programId, offerId, domain, adminName, adminEmail, adminPassword);

                    // save item into meta-base
                    org = new ExchangeOrganization();
                    org.Name = organizationId;
                    org.PackageId = packageId;
                    org.ServiceId = serviceId;
                    itemId = PackageController.AddPackageItem(org);
                }
                catch (Exception ex)
                {
                    // log error
                    TaskManager.WriteError(ex);
                    return Error<IntResult>(CreateOrganizationError);
                }
                #endregion

                #region Update organization quotas
                // update max org quotas
                UpdateOrganizationQuotas(org);

                // override quotas
                ResultObject quotasResult = ExchangeHostedEditionController.UpdateOrganizationQuotas(itemId,
                    org.MaxMailboxCountQuota,
                    org.MaxContactCountQuota,
                    org.MaxDistributionListCountQuota);

                if (!quotasResult.IsSuccess)
                    return Error<IntResult>(quotasResult, CreateOrganizationError);
                #endregion

                #region Add temporary domain
                // load settings
                PackageSettings settings = GetExchangePackageSettings(org);
                string tempDomainTemplate = settings[TempDomainSetting];
                if (!String.IsNullOrEmpty(tempDomainTemplate))
                {
                    // add temp domain
                    string tempDomain = String.Format("{0}.{1}", domain, tempDomainTemplate);
                    AddOrganizationDomain(itemId, tempDomain);
                }

                #endregion

                result.Value = itemId;
                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<IntResult>(GeneralError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static List<ExchangeOrganization> GetOrganizations(int packageId)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(ExchangeOrganization), false);

            return items.ConvertAll<ExchangeOrganization>(i => { return (ExchangeOrganization)i;  });
        }

        public static ExchangeOrganization GetOrganizationDetails(int itemId)
        {
            // load organization item
            ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
            if (item == null)
                return null; // organization item not found

            // get Exchange service
            ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

            // get organization details
            ExchangeOrganization org = exchange.GetOrganizationDetails(item.Name);

            //ExchangeOrganization org = new ExchangeOrganization
            //{
            //    Id = item.Id,
            //    PackageId = item.PackageId,
            //    ServiceId = item.ServiceId,
            //    Name = item.Name,
            //    AdministratorEmail = "admin@email.com",
            //    AdministratorName = "Administrator Mailbox",
            //    CatchAllAddress = "",
            //    ContactCount = 1,
            //    ContactCountQuota = 2,
            //    MaxContactCountQuota = 3,
            //    DistinguishedName = "DN=....",
            //    DistributionListCount = 2,
            //    DistributionListCountQuota = 3,
            //    MaxDistributionListCountQuota = 3,
            //    MaxDomainsCountQuota = 4,
            //    ExchangeControlPanelUrl = "http://ecp.domain.com",
            //    MailboxCount = 3,
            //    MailboxCountQuota = 4,
            //    MaxMailboxCountQuota = 4,
            //    ProgramId = "HostedExchange",
            //    OfferId = "2",
            //    ServicePlan = "HostedExchange_Basic",
            //    Domains = GetOrganizationDomains(item.Id).ToArray()
            //};

            // update item props
            org.Id = item.Id;
            org.PackageId = item.PackageId;
            org.ServiceId = item.ServiceId;
            org.Name = item.Name;
            org.CatchAllAddress = item.CatchAllAddress;

            // update max quotas
            UpdateOrganizationQuotas(org);

            // process summary information
            org.SummaryInformation = GetExchangeOrganizationSummary(org);

            // process domains
            PackageSettings settings = GetExchangePackageSettings(org);
            if(settings != null)
            {
                // get settings
                string tempDomain = settings[TempDomainSetting];
                string ecpUrl = settings[ExchangeControlPanelUrlSetting];

                // iterate through domains
                foreach (ExchangeOrganizationDomain domain in org.Domains)
                {
                    if (tempDomain != null && domain.Name.EndsWith("." + tempDomain, StringComparison.InvariantCultureIgnoreCase))
                        domain.IsTemp = true;
                    if (domain.IsDefault && ecpUrl != null)
                        org.ExchangeControlPanelUrl = Utils.ReplaceStringVariable(ecpUrl, "domain_name", domain.Name);
                }
            }

            // return org
            return org;
        }

        public static void UpdateOrganizationQuotas(ExchangeOrganization org)
        {
            // load default package quotas
            PackageContext cntx = PackageController.GetPackageContext(org.PackageId);
            if (!cntx.Groups.ContainsKey(ResourceGroups.ExchangeHostedEdition))
                return;

            org.MaxMailboxCountQuota = cntx.Quotas[Quotas.EXCHANGEHOSTEDEDITION_MAILBOXES].QuotaAllocatedValue;
            org.MaxContactCountQuota = cntx.Quotas[Quotas.EXCHANGEHOSTEDEDITION_CONTACTS].QuotaAllocatedValue;
            org.MaxDistributionListCountQuota = cntx.Quotas[Quotas.EXCHANGEHOSTEDEDITION_DISTRIBUTIONLISTS].QuotaAllocatedValue;
            org.MaxDomainsCountQuota = cntx.Quotas[Quotas.EXCHANGEHOSTEDEDITION_DOMAINS].QuotaAllocatedValue;
        }

        public static List<ExchangeOrganizationDomain> GetOrganizationDomains(int itemId)
        {
            // load organization item
            ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
            if (item == null)
                return null; // organization item not found

            // get Exchange service
            ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

            // get organization domains
            List<ExchangeOrganizationDomain> domains = new List<ExchangeOrganizationDomain>();
            domains.AddRange(exchange.GetOrganizationDomains(item.Name));
            return domains;

            //return new List<ExchangeOrganizationDomain>
            //{
            //    new ExchangeOrganizationDomain { Identity = "org101\\domain1.com", Name = "domain1.com", IsDefault = true, IsTemp = false },
            //    new ExchangeOrganizationDomain { Identity = "org101\\org101.tempdomain.com", Name = "org101.tempdomain.com", IsDefault = false, IsTemp = true },
            //    new ExchangeOrganizationDomain { Identity = "org101\\myseconddomain.com", Name = "myseconddomain.com", IsDefault = false, IsTemp = false }
            //};
        }

        public static string GetExchangeOrganizationSummary(int itemId)
        {
            // load organization details
            ExchangeOrganization org = GetOrganizationDetails(itemId);
            if (org == null)
                return null; // organization not found

            return GetExchangeOrganizationSummary(org);
        }

        private static string GetExchangeOrganizationSummary(ExchangeOrganization org)
        {
            // evaluate template
            MailTemplate template = EvaluateOrganizationSummaryTemplate(org);
            if (template == null || template.Body == null)
                return null;

            return template.IsHtml ? template.Body : template.Body.Replace("\n", "<br/>");
        }

        private static MailTemplate EvaluateOrganizationSummaryTemplate(ExchangeOrganization org)
        {
            #region create template context
            Hashtable items = new Hashtable();

            // add organization
            items["org"] = org;

            // add package information
            PackageInfo space = PackageController.GetPackage(org.PackageId);
            items["space"] = space;

            // add user information
            UserInfo user = UserController.GetUser(space.UserId);
            items["user"] = user;
            #endregion

            #region load template
            // load template settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.EXCHANGE_HOSTED_EDITION_ORGANIZATION_SUMMARY);
            if(settings == null)
                return null;

            // create template
            MailTemplate template = new MailTemplate();

            // from
            template.From = settings["From"];

            // BCC
            template.Bcc = settings["CC"];

            // subject
            template.Subject = settings["Subject"];

            // body
            template.IsHtml = user.HtmlMail;
            string bodySetting = template.IsHtml ? "HtmlBody" : "TextBody";
            template.Body = settings[bodySetting];

            // priority
            string priority = settings["Priority"];
            template.Priority = String.IsNullOrEmpty(priority)
                ? MailPriority.Normal
                : (MailPriority)Enum.Parse(typeof(MailPriority), priority, true);
            #endregion

            #region evaluate template
            if(template.Subject != null)
                template.Subject = PackageController.EvaluateTemplate(template.Subject, items);

            if(template.Body != null)
                template.Body = PackageController.EvaluateTemplate(template.Body, items);
            #endregion

            return template;
        }

        private static PackageSettings GetExchangePackageSettings(ExchangeOrganization org)
        {
            // load package settings
            return PackageController.GetPackageSettings(org.PackageId, PackageSettings.EXCHANGE_HOSTED_EDITION);
        }

        public static ResultObject SendExchangeOrganizationSummary(int itemId, string toEmail)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "SEND_SUMMARY");
                TaskManager.WriteParameter("Item ID", itemId);
                TaskManager.WriteParameter("To e-mail", toEmail);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // load organization details
                ExchangeOrganization org = GetOrganizationDetails(item.Id);
                if(org == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                // get evaluated summary information
                MailTemplate msg = EvaluateOrganizationSummaryTemplate(org);
                if (msg == null)
                    return Error<ResultObject>(SendOrganizationTemplateNotSetError);

                // send message
                int sendResult = MailHelper.SendMessage(msg.From, toEmail, msg.Bcc, msg.Subject, msg.Body, msg.Priority, msg.IsHtml);
                if (sendResult < 0)
                    return Error<ResultObject>((-sendResult).ToString());

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(SendOrganizationSummaryError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ResultObject AddOrganizationDomain(int itemId, string domainName)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "ADD_DOMAIN");
                TaskManager.WriteParameter("itemId", itemId);
                TaskManager.WriteParameter("domain", domainName);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // get organization details
                ExchangeOrganization org = GetOrganizationDetails(item.Id);
                if (org == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                // check domains quota
                if(org.MaxDomainsCountQuota > -1 && org.Domains.Length >= org.MaxDomainsCountQuota)
                    return Error<IntResult>(AddDomainQuotaExceededError);

                // check if the domain already exists
                DomainInfo domain = null;
                int checkResult = ServerController.CheckDomain(domainName);
                if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
                {
                    // domain exists
                    // check if it belongs to the same space
                    domain = ServerController.GetDomain(domainName);
                    if (domain == null)
                        return Error<ResultObject>((-checkResult).ToString());

                    if (domain.PackageId != org.PackageId)
                        return Error<ResultObject>((-checkResult).ToString());

                    // check if domain is already used in this organization
                    foreach (ExchangeOrganizationDomain orgDomain in org.Domains)
                    {
                        if(String.Equals(orgDomain.Name, domainName, StringComparison.InvariantCultureIgnoreCase))
                            return Error<ResultObject>(AddDomainAlreadyUsedError);
                    }
                }
                else if (checkResult == BusinessErrorCodes.ERROR_RESTRICTED_DOMAIN)
                {
                    return Error<ResultObject>((-checkResult).ToString());
                }

                // create domain if required
                if (domain == null)
                {
                    domain = new DomainInfo();
                    domain.PackageId = org.PackageId;
                    domain.DomainName = domainName;
                    domain.IsInstantAlias = false;
                    domain.IsSubDomain = false;

                    int domainId = ServerController.AddDomain(domain);
                    if (domainId < 0)
                        return Error<ResultObject>((-domainId).ToString());

                    // add domain
                    domain.DomainId = domainId;
                }

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

                // add domain
                exchange.AddOrganizationDomain(item.Name, domainName);

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(AddDomainError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ResultObject DeleteOrganizationDomain(int itemId, string domainName)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "DELETE_DOMAIN");
                TaskManager.WriteParameter("itemId", itemId);
                TaskManager.WriteParameter("domain", domainName);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

                // delete domain
                exchange.DeleteOrganizationDomain(item.Name, domainName);

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(DeleteDomainError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ResultObject UpdateOrganizationQuotas(int itemId, int mailboxesNumber, int contactsNumber, int distributionListsNumber)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "UPDATE_QUOTAS");
                TaskManager.WriteParameter("itemId", itemId);
                TaskManager.WriteParameter("mailboxesNumber", mailboxesNumber);
                TaskManager.WriteParameter("contactsNumber", contactsNumber);
                TaskManager.WriteParameter("distributionListsNumber", distributionListsNumber);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // check quotas
                UpdateOrganizationQuotas(item);

                if(item.MaxMailboxCountQuota > -1 && mailboxesNumber > item.MaxMailboxCountQuota)
                    return Error<ResultObject>(UpdateQuotasWrongQuotaError);
                if (item.MaxContactCountQuota > -1 && contactsNumber > item.MaxContactCountQuota)
                    return Error<ResultObject>(UpdateQuotasWrongQuotaError);
                if (item.MaxDistributionListCountQuota > -1 && distributionListsNumber > item.MaxDistributionListCountQuota)
                    return Error<ResultObject>(UpdateQuotasWrongQuotaError);

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

                // update quotas
                exchange.UpdateOrganizationQuotas(item.Name, mailboxesNumber, contactsNumber, distributionListsNumber);

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(UpdateQuotasError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ResultObject UpdateOrganizationCatchAllAddress(int itemId, string catchAllEmail)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "UPDATE_CATCHALL");
                TaskManager.WriteParameter("itemId", itemId);
                TaskManager.WriteParameter("catchAllEmail", catchAllEmail);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

                // update catch-all
                exchange.UpdateOrganizationCatchAllAddress(item.Name, catchAllEmail);

                // save new catch-all in the item
                item.CatchAllAddress = catchAllEmail;
                PackageController.UpdatePackageItem(item);

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(UpdateCatchAllError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ResultObject UpdateOrganizationServicePlan(int itemId, int newServiceId)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "UPDATE_SERVICE");
                TaskManager.WriteParameter("itemId", itemId);
                TaskManager.WriteParameter("newServiceId", newServiceId);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

                // load service settings to know ProgramID, OfferID
                StringDictionary serviceSettings = ServerController.GetServiceSettings(newServiceId);
                string programId = serviceSettings["programID"];
                string offerId = serviceSettings["offerID"];

                // check settings
                if(String.IsNullOrEmpty(programId))
                    result.ErrorCodes.Add(ProgramIdIsNotSetError);
                if (String.IsNullOrEmpty(offerId))
                    result.ErrorCodes.Add(OfferIdIsNotSetError);

                // update service plan
                exchange.UpdateOrganizationServicePlan(item.Name, programId, offerId);

                // move item between services
                int moveResult = PackageController.MovePackageItem(itemId, newServiceId);
                if (moveResult < 0)
                    return Error<ResultObject>((-moveResult).ToString());

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(UpdateServicePlanError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ResultObject DeleteOrganization(int itemId)
        {
            ResultObject result = new ResultObject();
            result.IsSuccess = true;

            try
            {
                // initialize task manager
                TaskManager.StartTask(TaskManagerSource, "DELETE_ORGANIZATION");
                TaskManager.WriteParameter("itemId", itemId);

                // load organization item
                ExchangeOrganization item = PackageController.GetPackageItem(itemId) as ExchangeOrganization;
                if (item == null)
                    return Error<ResultObject>(OrganizationNotFoundError);

                #region Check Space and Account
                // Check account
                int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
                if (accountCheck < 0)
                    return Warning<ResultObject>((-accountCheck).ToString());

                // Check space
                int packageCheck = SecurityContext.CheckPackage(item.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0)
                    return Warning<ResultObject>((-packageCheck).ToString());
                #endregion

                // get Exchange service
                ExchangeServerHostedEdition exchange = GetExchangeService(item.ServiceId);

                // delete organization
                exchange.DeleteOrganization(item.Name);

                // delete meta-item
                PackageController.DeletePackageItem(itemId);

                return result;
            }
            catch (Exception ex)
            {
                // log error
                TaskManager.WriteError(ex);

                // exit with error code
                return Error<ResultObject>(DeleteOrganizationError, ex.Message);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        #region Private helpers
        public static ExchangeServerHostedEdition GetExchangeService(int serviceId)
        {
            ExchangeServerHostedEdition ws = new ExchangeServerHostedEdition();
            ServiceProviderProxy.Init(ws, serviceId);
            return ws;
        }
        #endregion

        #region Result object routines
        private static T Warning<T>(params string[] messageParts)
        {
            return Warning<T>(null, messageParts);
        }

        private static T Warning<T>(ResultObject innerResult, params string[] messageParts)
        {
            return Result<T>(innerResult, false, messageParts);
        }

        private static T Error<T>(params string[] messageParts)
        {
            return Error<T>(null, messageParts);
        }

        private static T Error<T>(ResultObject innerResult, params string[] messageParts)
        {
            return Result<T>(innerResult, true, messageParts);
        }

        private static T Result<T>(ResultObject innerResult, bool isError, params string[] messageParts)
        {
            object obj = Activator.CreateInstance<T>();
            ResultObject result = (ResultObject)obj;

            // set error
            result.IsSuccess = !isError;

            // add message
            if (messageParts != null)
                result.ErrorCodes.Add(String.Join(":", messageParts));

            // copy errors from inner result
            if (innerResult != null)
                result.ErrorCodes.AddRange(innerResult.ErrorCodes);

            return (T)obj;
        }
        #endregion
    }
}