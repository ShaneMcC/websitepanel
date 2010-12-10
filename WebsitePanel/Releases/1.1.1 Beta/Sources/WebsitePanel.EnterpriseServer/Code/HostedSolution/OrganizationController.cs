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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Net.Mail;
using System.Text;
using WebsitePanel.EnterpriseServer.Code.HostedSolution;
using WebsitePanel.EnterpriseServer.Code.SharePoint;
using WebsitePanel.Providers;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.SharePoint;
using WebsitePanel.Providers.Common;
namespace WebsitePanel.EnterpriseServer
{
    public class OrganizationController
    {
        public const string TemporyDomainName = "TempDomain";

        private static bool CheckUserQuota(int orgId, out int errorCode)
        {
            errorCode = 0;
            OrganizationStatistics stats = GetOrganizationStatistics(orgId);
            
            
            if (stats.AllocatedUsers != -1 && (stats.CreatedUsers >= stats.AllocatedUsers) )
            {
                errorCode = BusinessErrorCodes.ERROR_USERS_RESOURCE_QUOTA_LIMIT;
                return false;
            }
            
            return true;
        }

        private static string EvaluateMailboxTemplate(int itemId, int accountId,
            bool pmm, bool emailMode, bool signup, string template)
        {
            Hashtable items = new Hashtable();

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;
            // add organization
            items["Organization"] = org;
            OrganizationUser user = GetAccount(itemId, accountId);
            
            items["account"] = user;

            
            // evaluate template
            return PackageController.EvaluateTemplate(template, items);
        }
        
        public static string GetOrganizationUserSummuryLetter(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;

            // load user info
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);

            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.ORGANIZATION_USER_SUMMARY_LETTER);

            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";
            string body = settings[settingName];
            if (String.IsNullOrEmpty(body))
                return null;

            string result = EvaluateMailboxTemplate(itemId, accountId, pmm, false, false, body);
            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
        }
        
        public static int SendSummaryLetter(int itemId, int accountId, bool signup, string to, string cc)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
            if (accountCheck < 0) return accountCheck;

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return -1;

            // load user info
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);

            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.ORGANIZATION_USER_SUMMARY_LETTER);

            string from = settings["From"];
            if (cc == null)
                cc = settings["CC"];
            string subject = settings["Subject"];
            string body = user.HtmlMail ? settings["HtmlBody"] : settings["TextBody"];
            bool isHtml = user.HtmlMail;

            MailPriority priority = MailPriority.Normal;
            if (!String.IsNullOrEmpty(settings["Priority"]))
                priority = (MailPriority)Enum.Parse(typeof(MailPriority), settings["Priority"], true);

            if (String.IsNullOrEmpty(body))
                return 0;// BusinessErrorCodes.ERROR_SETTINGS_ACCOUNT_LETTER_EMPTY_BODY;

            // load user info
            if (to == null)
                to = user.Email;

            subject = EvaluateMailboxTemplate(itemId, accountId, false, true, signup, subject);
            body = EvaluateMailboxTemplate(itemId, accountId, false, true, signup, body);

            // send message
            return MailHelper.SendMessage(from, to, cc, subject, body, priority, isHtml);
        }
        
        private static bool CheckQuotas(int packageId, out int errorCode)
        {
            
            // check account
            errorCode = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (errorCode < 0) return false;

            // check package
            errorCode = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
            if (errorCode < 0) return false;

            // check organizations quota
            QuotaValueInfo quota = PackageController.GetPackageQuota(packageId, Quotas.ORGANIZATIONS );
            if (quota.QuotaExhausted)
            {
                errorCode = BusinessErrorCodes.ERROR_ORGS_RESOURCE_QUOTA_LIMIT;
                return false;
            }


            // check sub-domains quota (for temporary domain)
            quota = PackageController.GetPackageQuota(packageId, Quotas.OS_SUBDOMAINS);
            if (quota.QuotaExhausted)
            {
                errorCode = BusinessErrorCodes.ERROR_SUBDOMAIN_QUOTA_LIMIT;
                return false;
            }
                                        
            
            return true;            
        }

        private static string CreateTemporyDomainName(int serviceId, string organizationId)
        {
            // load service settings
            StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
            
            string tempDomain = serviceSettings[TemporyDomainName];
            return String.IsNullOrEmpty(tempDomain) ? null : organizationId + "."+ tempDomain;                                                                               
        }
                       
        private static DomainInfo CreateNewDomain(int packageId, string domainName)
        {          
            // new domain
            DomainInfo domain = new DomainInfo();
            domain.PackageId = packageId;
            domain.DomainName = domainName;
            domain.IsInstantAlias = true;
            domain.IsSubDomain = true;
            
            return domain;
        }
              
        private static int CreateDomain(string domainName, int packageId, out bool domainCreated)
        {
            // trying to locate (register) temp domain
            DomainInfo domain = null;
            int domainId = 0;
            domainCreated = false;

            // check if the domain already exists
            int checkResult = ServerController.CheckDomain(domainName);
            if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
            {
                // domain exists
                // check if it belongs to the same space
                domain = ServerController.GetDomain(domainName);
                if (domain == null)
                    return checkResult;

                if (domain.PackageId != packageId)
                    return checkResult;

                if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
                    return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;

                domainId = domain.DomainId;
            }
            else if (checkResult < 0)
            {
                return checkResult;
            }

            // create domain if required
            if (domain == null)
            {
                domain = CreateNewDomain(packageId, domainName);
                // add WebsitePanel domain
                domainId = ServerController.AddDomain(domain);

                if (domainId < 0)
                    return domainId;

                domainCreated = true;
            }

            return domainId;
        }
        
        private static int AddOrganizationToPackageItems(Organization org, int serviceId, int packageId, string organizationName, string organizationId, string domainName)
        {            
            org.ServiceId = serviceId;
            org.PackageId = packageId;
            org.Name = organizationName;
            org.OrganizationId = organizationId;
            org.DefaultDomain = domainName;
            
            int itemId = PackageController.AddPackageItem(org);
            
            
            return itemId;
        }

        private static bool OrganizationIdentifierExists(string organizationId)
        {
            return DataProvider.ExchangeOrganizationExists(organizationId);
        }

		private static void RollbackOrganization(int packageId, string organizationId)
		{
			try
			{
				int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedOrganizations);
				Organizations orgProxy = GetOrganizationProxy(serviceId);
				orgProxy.DeleteOrganization(organizationId);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
		}
        public static int CreateOrganization(int packageId,  string organizationId, string organizationName)
        {
            int itemId;
            int errorCode;
            if (!CheckQuotas(packageId, out errorCode))
                return errorCode;
            
            // place log record
            TaskManager.StartTask("ORGANIZATION", "CREATE_ORG", organizationName);
            TaskManager.TaskParameters["Organization ID"] = organizationId;

			try
			{
				// Check if organization exitsts.                
				if (OrganizationIdentifierExists(organizationId))
					return BusinessErrorCodes.ERROR_ORG_ID_EXISTS;

				// Create Organization Unit
				int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedOrganizations);

				Organizations orgProxy = GetOrganizationProxy(serviceId);
				Organization org = null;
				if (!orgProxy.OrganizationExists(organizationId))
				{
					org = orgProxy.CreateOrganization(organizationId);
				}
				else
					return BusinessErrorCodes.ERROR_ORG_ID_EXISTS;

				//create temporary domain name;
				string domainName = CreateTemporyDomainName(serviceId, organizationId);
				if (string.IsNullOrEmpty(domainName))
				{
					RollbackOrganization(packageId, organizationId);
					return BusinessErrorCodes.ERROR_ORGANIZATION_TEMP_DOMAIN_IS_NOT_SPECIFIED;
				}

				bool domainCreated;
				int domainId = CreateDomain(domainName, packageId, out domainCreated);
				//create domain 
				if (domainId < 0)
				{
					RollbackOrganization(packageId, organizationId);
					return domainId;
				}

				PackageContext cntx = PackageController.GetPackageContext(packageId);

				if (cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE] != null)
					org.MaxSharePointStorage = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE].QuotaAllocatedValue;
				

				if (cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE] != null)
					org.WarningSharePointStorage = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_STORAGE_SIZE].QuotaAllocatedValue;
				

				//add organization to package items                
				itemId = AddOrganizationToPackageItems(org, serviceId, packageId, organizationName, organizationId, domainName);

				// register org ID

				DataProvider.AddExchangeOrganization(itemId, organizationId);

				// register domain                
				DataProvider.AddExchangeOrganizationDomain(itemId, domainId, true);

				// register organization domain service item
				OrganizationDomain orgDomain = new OrganizationDomain
				                                   {
				                                       Name = domainName,
				                                       PackageId = packageId,
				                                       ServiceId = serviceId
				                                   };

			    PackageController.AddPackageItem(orgDomain);

			}
			catch (Exception ex)
			{
				//rollback organization
				try
				{
					RollbackOrganization(packageId, organizationId);
				}
				catch (Exception rollbackException)
				{
					TaskManager.WriteError(rollbackException);
				}

				throw TaskManager.WriteError(ex);
			}
            finally
            {
                TaskManager.CompleteTask();
            }
            
            return itemId;
        }

        public static int DeleteOrganizationDomain(int itemId, int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_DOMAIN");
            TaskManager.TaskParameters["Domain ID"] = domainId;
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return -1;

                // load domain
                DomainInfo domain = ServerController.GetDomain(domainId);
                if (domain == null)
                    return -1;
                
                // unregister domain
                DataProvider.DeleteExchangeOrganizationDomain(itemId, domainId);

                // remove service item
                ServiceProviderItem itemDomain = PackageController.GetPackageItemByName(
                    org.PackageId, domain.DomainName, typeof(OrganizationDomain));
                if (itemDomain != null)
                    PackageController.DeletePackageItem(itemDomain.Id);


                /*Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                orgProxy.CreateOrganizationDomain(org.DistinguishedName, domain.DomainName);*/
                if (!string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExchangeServerController.DeleteAuthoritativeDomain(itemId, domainId);
                }

                if (org.IsOCSOrganization)
                {
                    OCSController.DeleteDomain(itemId, domain.DomainName);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static void DeleteOCSUsers(int itemId, ref bool successful)
        {
            try
            {                  
                    OCSUsersPagedResult res = OCSController.GetOCSUsers(itemId, string.Empty, string.Empty, string.Empty,
                                                            string.Empty, 0, int.MaxValue);
                    if (res.IsSuccess)
                    {
                        foreach (OCSUser user in res.Value.PageUsers)
                        {
                            try
                            {
                                ResultObject delUserResult = OCSController.DeleteOCSUser(itemId, user.InstanceId);          
                                if (!delUserResult.IsSuccess)
                                {
                                    StringBuilder sb = new StringBuilder();                                   
                                    foreach(string str in delUserResult.ErrorCodes)
                                    {
                                        sb.Append(str);
                                        sb.Append('\n');
                                    }

                                    throw new ApplicationException(sb.ToString());
                                }
                            }
                            catch(Exception ex)
                            {
                                successful = false;
                                TaskManager.WriteError(ex);
                            }
                        }
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();                                   
                        foreach(string str in res.ErrorCodes)
                        {
                            sb.Append(str);
                            sb.Append('\n');
                        }

                        throw new ApplicationException(sb.ToString());
                    }
                }
                catch(Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }
        }

        public static int DeleteOrganization(int itemId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_ORG");
            TaskManager.ItemId = itemId;

            try
            {
                bool successful = true;
                Organization org = (Organization)PackageController.GetPackageItem(itemId);

                try
                {
                    HostedSharePointServerController.DeleteSiteCollections(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                if (org.IsOCSOrganization)
                {
                    DeleteOCSUsers(itemId, ref successful);                                        
                }

                try
                {
                    if (org.CrmOrganizationId != Guid.Empty)
                        CRMController.DeleteOrganization(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }                

                try
                {                  
                    OrganizationUsersPagedResult res = BlackBerryController.GetBlackBerryUsers(itemId, string.Empty, string.Empty, string.Empty,
                                                            string.Empty, 0, int.MaxValue);
                    if (res.IsSuccess)
                    {
                        foreach (OrganizationUser user in res.Value.PageUsers)
                        {
                            try
                            {
                                ResultObject delUserResult = BlackBerryController.DeleteBlackBerryUser(itemId, user.AccountId);          
                                if (!delUserResult.IsSuccess)
                                {
                                    StringBuilder sb = new StringBuilder();                                   
                                    foreach(string str in delUserResult.ErrorCodes)
                                    {
                                        sb.Append(str);
                                        sb.Append('\n');
                                    }

                                    throw new ApplicationException(sb.ToString());
                                }
                            }
                            catch(Exception ex)
                            {
                                successful = false;
                                TaskManager.WriteError(ex);
                            }
                        }
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();                                   
                        foreach(string str in res.ErrorCodes)
                        {
                            sb.Append(str);
                            sb.Append('\n');
                        }

                        throw new ApplicationException(sb.ToString());
                    }
                }
                catch(Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }
                
                try
                {
                    if (!string.IsNullOrEmpty(org.GlobalAddressList))
                        ExchangeServerController.DeleteOrganization(itemId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }

                
                                                
                Organizations orgProxy =  GetOrganizationProxy(org.ServiceId);
              
                try
                {
                    orgProxy.DeleteOrganization(org.OrganizationId);
                }
                catch (Exception ex)
                {
                    successful = false;
                    TaskManager.WriteError(ex);
                }
                
                    

                // delete organization domains
                List<OrganizationDomainName> domains = GetOrganizationDomains(itemId);
                foreach (OrganizationDomainName domain in domains)
                {
                    try
                    {
                        DeleteOrganizationDomain(itemId, domain.DomainId);
                    }
                    catch (Exception ex)
                    {
                        successful = false;
                        TaskManager.WriteError(ex);
                    }
                
                }

                DataProvider.DeleteOrganizationUser(itemId);
                
                // delete meta-item
                PackageController.DeletePackageItem(itemId);
                
                
                return successful ? 0 : BusinessErrorCodes.ERROR_ORGANIZATION_DELETE_SOME_PROBLEMS;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

       
        private static Organizations GetOrganizationProxy(int serviceId)
        {            
            Organizations ws = new Organizations();
            ServiceProviderProxy.Init(ws, serviceId);            
            return ws;
        }

        public static List<Organization> GetOrganizations(int packageId, bool recursive)
        {
            List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
                packageId, typeof(Organization), recursive);

            return items.ConvertAll<Organization>(
                delegate(ServiceProviderItem item) { return (Organization)item; });
        }

        public static DataSet GetRawOrganizationsPaged(int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {               
            #region Demo Mode
            if (IsDemoMode)
            {
                DataSet ds = new DataSet();

                // total records
                DataTable dtTotal = ds.Tables.Add();
                dtTotal.Columns.Add("Records", typeof(int));
                dtTotal.Rows.Add(2);

                // organizations
                DataTable dtItems = ds.Tables.Add();
                dtItems.Columns.Add("ItemID", typeof(int));
                dtItems.Columns.Add("OrganizationID", typeof(string));
                dtItems.Columns.Add("ItemName", typeof(string));
                dtItems.Columns.Add("PackageName", typeof(string));
                dtItems.Columns.Add("PackageID", typeof(int));
                dtItems.Columns.Add("Username", typeof(string));
                dtItems.Columns.Add("UserID", typeof(int));
                dtItems.Rows.Add(1, "fabrikam", "Fabrikam Inc", "Hosted Exchange", 1, "Customer", 1);

                
                dtItems.Rows.Add(2, "Contoso", "Contoso Ltd", "Hosted Exchange", 2, "Customer", 2);
                

                return ds;
            }
            #endregion
            
            
            return PackageController.GetRawPackageItemsPaged(
                   packageId, ResourceGroups.HostedOrganizations, typeof(Organization),
                   recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
        }

        public static Organization GetOrganizationById(string organizationId)
        {
            if (string.IsNullOrEmpty(organizationId))
                throw new ArgumentNullException("organizationId");

            int itemId = DataProvider.GetItemIdByOrganizationId(organizationId);

            Organization org = GetOrganization(itemId);

            return org; 
        }
        
        public static Organization GetOrganization(int itemId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                // load package by user
                Organization org = new Organization();
                org.PackageId = 0;
                org.Id = 1;
                org.OrganizationId = "fabrikam";
                org.Name = "Fabrikam Inc";
                org.IssueWarningKB = 150000;
                org.ProhibitSendKB = 170000;
                org.ProhibitSendReceiveKB = 190000;
                org.KeepDeletedItemsDays = 14;
                org.GlobalAddressList = "FabrikamGAL";
                return org;
            }
            #endregion

            return (Organization)PackageController.GetPackageItem(itemId);
        }

        public static OrganizationStatistics GetOrganizationStatistics(int itemId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                OrganizationStatistics stats = new OrganizationStatistics();
                stats.AllocatedMailboxes = 4;
                stats.CreatedMailboxes = 3;
                stats.AllocatedContacts = 4;
                stats.CreatedContacts = 2;
                stats.AllocatedDistributionLists = 5;
                stats.CreatedDistributionLists = 1;
                stats.AllocatedPublicFolders = 40;
                stats.CreatedPublicFolders = 4;
                stats.AllocatedDomains = 5;
                stats.CreatedDomains = 2;
                stats.AllocatedDiskSpace = 200;
                stats.UsedDiskSpace = 70;
                stats.CreatedUsers = 5;
                stats.AllocatedUsers = 10;
                stats.CreatedSharePointSiteCollections = 1;
                stats.AllocatedSharePointSiteCollections = 5;
                return stats;
            }
            #endregion
            
            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_ORG_STATS");
            TaskManager.ItemId = itemId;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return null;

                OrganizationStatistics stats = ObjectUtils.FillObjectFromDataReader<OrganizationStatistics>(
                    DataProvider.GetOrganizationStatistics(itemId));


                // disk space               
                // allocated quotas                               
                PackageContext cntx = PackageController.GetPackageContext(org.PackageId);                
                stats.AllocatedUsers = cntx.Quotas[Quotas.ORGANIZATION_USERS].QuotaAllocatedValue;
                stats.AllocatedDomains = cntx.Quotas[Quotas.ORGANIZATION_DOMAINS].QuotaAllocatedValue;
				
				if (cntx.Groups.ContainsKey(ResourceGroups.HostedSharePoint))
				{
					SharePointSiteCollectionListPaged sharePointStats = HostedSharePointServerController.GetSiteCollectionsPaged(org.PackageId, org.Id, string.Empty, string.Empty, string.Empty, 0, 0);
					stats.CreatedSharePointSiteCollections = sharePointStats.TotalRowCount;
					stats.AllocatedSharePointSiteCollections = cntx.Quotas[Quotas.HOSTED_SHAREPOINT_SITES].QuotaAllocatedValue;
				}


                if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                {                                        
                    stats.CreatedCRMUsers = CRMController.GetCRMUsersCount(org.Id, string.Empty, string.Empty).Value;
                    stats.AllocatedCRMUsers = cntx.Quotas[Quotas.CRM_USERS].QuotaAllocatedValue;
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.BlackBerry))
                {
                    stats.CreatedBlackBerryUsers = BlackBerryController.GetBlackBerryUsersCount(org.Id, string.Empty, string.Empty).Value;
                    stats.AllocatedBlackBerryUsers = cntx.Quotas[Quotas.BLACKBERRY_USERS].QuotaAllocatedValue;
                }

                if (cntx.Groups.ContainsKey(ResourceGroups.OCS))
                {
                    stats.CreatedOCSUsers = OCSController.GetOCSUsersCount(org.Id, string.Empty, string.Empty).Value;
                    stats.AllocatedOCSUsers = cntx.Quotas[Quotas.OCS_USERS].QuotaAllocatedValue;
                }


                return stats;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
        
        public static int AddOrganizationDomain(int itemId, string domainName)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // check domains quota
            OrganizationStatistics orgStats = GetOrganizationStatistics(itemId);
            if (orgStats.AllocatedDomains > -1
                && orgStats.CreatedDomains >= orgStats.AllocatedDomains)
                return BusinessErrorCodes.ERROR_EXCHANGE_DOMAINS_QUOTA_LIMIT;
            
            // place log record
            TaskManager.StartTask("ORGANIZATION", "ADD_DOMAIN", domainName);
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                DomainInfo domain = null;

                // check if the domain already exists
                int checkResult = ServerController.CheckDomain(domainName);
                if (checkResult == BusinessErrorCodes.ERROR_DOMAIN_ALREADY_EXISTS)
                {
                    // domain exists
                    // check if it belongs to the same space
                    domain = ServerController.GetDomain(domainName);
                    if (domain == null)
                        return checkResult;

                    if (domain.PackageId != org.PackageId)
                        return checkResult;

                    if (DataProvider.ExchangeOrganizationDomainExists(domain.DomainId))
                        return BusinessErrorCodes.ERROR_ORGANIZATION_DOMAIN_IS_IN_USE;
                }
                else if (checkResult == BusinessErrorCodes.ERROR_RESTRICTED_DOMAIN)
                {
                    return checkResult;
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
                        return domainId;
                    
                    // add domain
                    domain.DomainId = domainId;
                }

               
                
                // register domain
                DataProvider.AddExchangeOrganizationDomain(itemId, domain.DomainId, false);

                // register service item
                OrganizationDomain exchDomain = new OrganizationDomain();
                exchDomain.Name = domainName;
                exchDomain.PackageId = org.PackageId;
                exchDomain.ServiceId = org.ServiceId;
                PackageController.AddPackageItem(exchDomain);

                
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                orgProxy.CreateOrganizationDomain(org.DistinguishedName, domainName);
                if (!string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExchangeServerController.AddAuthoritativeDomain(itemId, domain.DomainId);
                }

                if (org.IsOCSOrganization)
                {
                    OCSController.AddDomain(domain.DomainName, itemId);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int SetOrganizationDefaultDomain(int itemId, int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // load organization
            Organization org = (Organization)PackageController.GetPackageItem(itemId);
            if (org == null)
                return -1;

            // update default domain
            DomainInfo domain = ServerController.GetDomain(domainId);
            if (domain == null)
                return 0;

            org.DefaultDomain = domain.DomainName;

            // save organization
            PackageController.UpdatePackageItem(org);

            return 0;
        }

        #region Users


        
        
        public static OrganizationUsersPaged GetOrganizationUsersPaged(int itemId, string filterColumn, string filterValue, string sortColumn,
			int startRow, int maximumRows)
        {

            #region Demo Mode
            if (IsDemoMode)
            {                                
                OrganizationUsersPaged res = new OrganizationUsersPaged();
                List<OrganizationUser> demoAccounts = SearchAccounts(1, "", "", "", true);
                
                OrganizationUser r1 = new OrganizationUser();
                r1.AccountId = 20;
                r1.AccountName = "room1_fabrikam";
                r1.AccountType = ExchangeAccountType.Room;
                r1.DisplayName = "Meeting Room 1";
                r1.PrimaryEmailAddress = "room1@fabrikam.net";
                demoAccounts.Add(r1);
                
                OrganizationUser e1 = new OrganizationUser();
                e1.AccountId = 21;
                e1.AccountName = "projector_fabrikam";
                e1.AccountType = ExchangeAccountType.Equipment;
                e1.DisplayName = "Projector 1";
                e1.PrimaryEmailAddress = "projector@fabrikam.net";
                demoAccounts.Add(e1);
                res.PageUsers = demoAccounts.ToArray();
                res.RecordsCount = res.PageUsers.Length;
                return res;
            }
            #endregion
            
            string accountTypes = string.Format("{0}, {1}, {2}, {3}", ((int)ExchangeAccountType.User), ((int)ExchangeAccountType.Mailbox), ((int)ExchangeAccountType.Room), ((int)ExchangeAccountType.Equipment));
            
            
            DataSet ds =
                DataProvider.GetExchangeAccountsPaged(SecurityContext.User.UserId, itemId, accountTypes, filterColumn,
                                                      filterValue, sortColumn, startRow, maximumRows);
           
            OrganizationUsersPaged result = new OrganizationUsersPaged();
            result.RecordsCount = (int)ds.Tables[0].Rows[0][0];

            List<OrganizationUser> accounts = new List<OrganizationUser>();
            ObjectUtils.FillCollectionFromDataView(accounts, ds.Tables[1].DefaultView);
            result.PageUsers = accounts.ToArray();
            return result;
        }


        
        private static bool EmailAddressExists(string emailAddress)
        {
            return DataProvider.ExchangeAccountEmailAddressExists(emailAddress);		
        }

        
        private static int AddOrganizationUser(int itemId, string accountName, string displayName,  string email, string accountPassword)
		{
            return DataProvider.AddExchangeAccount(itemId, (int)ExchangeAccountType.User, accountName, displayName, email, false, string.Empty,
                                            string.Empty, CryptoUtils.Encrypt(accountPassword));
                       
		}

        public static string GetAccountName(string loginName)
        {
            string []parts = loginName.Split('@');
            return parts != null && parts.Length > 1 ? parts[0] : loginName;
                
        }

        public static int CreateUser(int itemId, string displayName, string name, string domain, string password, bool enabled, bool sendNotification, string to, out string accountName)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentNullException("displayName");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            if (string.IsNullOrEmpty(domain))
                throw new ArgumentNullException("domain");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            accountName = string.Empty;
            
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            
            // place log record
            TaskManager.StartTask("ORGANIZATION", "CREATE_USER");
            TaskManager.ItemId = itemId;

            // e-mail
            string email = name + "@" + domain;

            if (EmailAddressExists(email))
                return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return -1;

            // check package
            int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
            if (packageCheck < 0) return packageCheck;

            int errorCode;
            if (!CheckUserQuota(org.Id, out errorCode))
                return errorCode;

         
            Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

            string upn = string.Format("{0}@{1}", name, domain);
            accountName = BuildAccountName(org.OrganizationId, name);            
            orgProxy.CreateUser(org.OrganizationId, accountName, displayName, upn, password, enabled);

            int userId = AddOrganizationUser(itemId, accountName, displayName, email, password);

            // register email address
            AddAccountEmailAddress(userId, email);

            if (sendNotification)
            {
                SendSummaryLetter(org.Id, userId, true, to, "");
            }
            return userId;
        }

       

        private static void AddAccountEmailAddress(int accountId, string emailAddress)
        {
            DataProvider.AddExchangeAccountEmailAddress(accountId, emailAddress);
        }
        
        private static string BuildAccountName(string orgId, string name)
        {
            int maxLen = 19 - orgId.Length;

            // try to choose name
            int i = 0;
            while (true)
            {
                string num = i > 0 ? i.ToString() : "";
                int len = maxLen - num.Length;

                if (name.Length > len)
                    name = name.Substring(0, len);

                string accountName = name + num + "_" + orgId;

                // check if already exists
                if (!AccountExists(accountName))
                    return accountName;

                i++;
            }
        }

        private static bool AccountExists(string accountName)
        {
            return DataProvider.ExchangeAccountExists(accountName);
        }

        public static int DeleteUser(int itemId, int accountId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("ORGANIZATION", "DELETE_USER");
            TaskManager.ItemId = itemId;

            try
            {
                Guid crmUserId = CRMController.GetCrmUserId( accountId);
                if (crmUserId != Guid.Empty)
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_CRM_USER;
                }


                if (DataProvider.CheckOCSUserExists(accountId))
                {
                    return BusinessErrorCodes.CURRENT_USER_IS_OCS_USER; ;
                }
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load account
                OrganizationUser user = GetAccount(itemId, accountId);
                
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);

                string account = GetAccountName(user.AccountName);
                
                if (user.AccountType == ExchangeAccountType.User)
                {
                    //Delete user from AD
                    orgProxy.DeleteUser(account, org.OrganizationId);
                    // unregister account
                    DeleteUserFromMetabase(itemId, accountId);
                }
                else 
                {                    
                    //Delete mailbox with AD user
                    ExchangeServerController.DeleteMailbox(itemId, accountId);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static OrganizationUser GetAccount(int itemId, int userId)
        {            
            OrganizationUser account = ObjectUtils.FillObjectFromDataReader<OrganizationUser>(
                DataProvider.GetExchangeAccount(itemId, userId));

            if (account == null)
                return null;

            // decrypt password
            account.AccountPassword = CryptoUtils.Decrypt(account.AccountPassword);

            return account;
        }

        private static void DeleteUserFromMetabase(int itemId, int accountId)
        {
            // try to get organization
            if (GetOrganization(itemId) == null)
                return;

            DataProvider.DeleteExchangeAccount(itemId, accountId);            
        }

        public static OrganizationUser GetUserGeneralSettings(int itemId, int accountId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                return GetDemoUserGeneralSettings();
            }
            #endregion

            // place log record
            TaskManager.StartTask("ORGANIZATION", "GET_USER_GENERAL");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                // load account
                OrganizationUser account = GetAccount(itemId, accountId);

                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId);
                string accountName = GetAccountName(account.AccountName);
                
                
                OrganizationUser retUser = orgProxy.GeUserGeneralSettings(accountName, org.OrganizationId);
                retUser.AccountId = accountId;
                retUser.AccountName = account.AccountName;
                retUser.PrimaryEmailAddress = account.PrimaryEmailAddress;
                retUser.AccountType = account.AccountType;
                
                return retUser;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }
       
        public static int SetUserGeneralSettings(int itemId, int accountId, string displayName,
            string password, bool hideAddressBook, bool disabled, bool locked, string firstName, string initials,
            string lastName, string address, string city, string state, string zip, string country,
            string jobTitle, string company, string department, string office, string managerAccountName,
            string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, string externalEmail)
        {

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("EXCHANGE", "UPDATE_MAILBOX_GENERAL");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // check package
                int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
                if (packageCheck < 0) return packageCheck;

                // load account
                ExchangeAccount account = ExchangeServerController.GetAccount(itemId, accountId);

                string accountName = GetAccountName(account.AccountName);
                // get mailbox settings
                Organizations orgProxy = GetOrganizationProxy(org.ServiceId); 
				// external email
				string externalEmailAddress = (account.AccountType == ExchangeAccountType.User ) ? externalEmail : account.PrimaryEmailAddress;
				 
                orgProxy.SetUserGeneralSettings(
                    org.OrganizationId,
                    accountName,
                    displayName,
                    password,
                    hideAddressBook,
                    disabled,
                    locked,
                    firstName,
                    initials,
                    lastName,
                    address,
                    city,
                    state,
                    zip,
                    country,
                    jobTitle,
                    company,
                    department,
                    office,
                    managerAccountName,
                    businessPhone,
                    fax,
                    homePhone,
                    mobilePhone,
                    pager,
                    webPage,
                    notes,
					externalEmailAddress);

                // update account
                account.DisplayName = displayName;
                
                //account.
                if (!String.IsNullOrEmpty(password))
                    account.AccountPassword = CryptoUtils.Encrypt(password);
                else 
                    account.AccountPassword = string.Empty;

                UpdateAccount(account);

                
                return 0;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        private static void UpdateAccount(ExchangeAccount account)
        {                        
            DataProvider.UpdateExchangeAccount(account.AccountId, account.AccountName, account.AccountType, account.DisplayName,
                account.PrimaryEmailAddress, account.MailEnabledPublicFolder,
                account.MailboxManagerActions.ToString(), account.SamAccountName, account.AccountPassword);
        }



        public static List<OrganizationUser> SearchAccounts(int itemId,
            
            string filterColumn, string filterValue, string sortColumn, bool includeMailboxes )
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                List<OrganizationUser> demoAccounts = new List<OrganizationUser>();


                OrganizationUser m1 = new OrganizationUser();
                    m1.AccountId = 1;
                    m1.AccountName = "john_fabrikam";
                    m1.AccountType = ExchangeAccountType.Mailbox;
                    m1.DisplayName = "John Smith";
                    m1.PrimaryEmailAddress = "john@fabrikam.net";
                    
                    if (includeMailboxes)                
                        demoAccounts.Add(m1);

                    OrganizationUser m2 = new OrganizationUser();
                    m2.AccountId = 2;
                    m2.AccountName = "jack_fabrikam";
                    m2.AccountType = ExchangeAccountType.User;
                    m2.DisplayName = "Jack Brown";
                    m2.PrimaryEmailAddress = "jack@fabrikam.net";
                    demoAccounts.Add(m2);

                    OrganizationUser m3 = new OrganizationUser();
                    m3.AccountId = 3;
                    m3.AccountName = "marry_fabrikam";
                    m3.AccountType = ExchangeAccountType.Mailbox;
                    m3.DisplayName = "Marry Smith";
                    m3.PrimaryEmailAddress = "marry@fabrikam.net";
                    
                    if (includeMailboxes)
                        demoAccounts.Add(m3);                  
                  

                return demoAccounts;
            }
            #endregion
            
            return ObjectUtils.CreateListFromDataReader<OrganizationUser>(
                DataProvider.SearchOrganizationAccounts(SecurityContext.User.UserId, itemId, 
                filterColumn, filterValue, sortColumn, includeMailboxes));
        }
        
        #endregion

        public static List<OrganizationDomainName> GetOrganizationDomains(int itemId)
        {

            #region Demo Mode
            if (IsDemoMode)
            {
                List<OrganizationDomainName> demoDomains = new List<OrganizationDomainName>();
                OrganizationDomainName d1 = new OrganizationDomainName();
                d1.DomainId = 1;
                d1.DomainName = "fabrikam.hosted-exchange.com";
                d1.IsDefault = false;
                d1.IsHost = true;
                demoDomains.Add(d1);

                OrganizationDomainName d2 = new OrganizationDomainName();
                d2.DomainId = 2;
                d2.DomainName = "fabrikam.net";
                d2.IsDefault = true;
                d2.IsHost = false;
                demoDomains.Add(d2);

                return demoDomains;
            }
            #endregion
            
            
            // load organization
            Organization org = (Organization)PackageController.GetPackageItem(itemId);
            if (org == null)
                return null;

            // load all domains
            List<OrganizationDomainName> domains = ObjectUtils.CreateListFromDataReader<OrganizationDomainName>(
                DataProvider.GetExchangeOrganizationDomains(itemId));

            // set default domain
            foreach (OrganizationDomainName domain in domains)
            {
                if (String.Compare(domain.DomainName, org.DefaultDomain, true) == 0)
                {
                    domain.IsDefault = true;
                    break;
                }
            }

            return domains;
        }

        private static OrganizationUser GetDemoUserGeneralSettings()
        {
            OrganizationUser user = new OrganizationUser();
            user.DisplayName = "John Smith";            
            user.AccountName = "john_fabrikam";
            user.FirstName = "John";
            user.LastName = "Smith";
            user.AccountType = ExchangeAccountType.Mailbox;            
            return user;
        }
        
        private static bool IsDemoMode
        {
            get
            {
                return (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);
            }
        }


        public static PasswordPolicyResult GetPasswordPolicy(int itemId)
        {
            PasswordPolicyResult res = new PasswordPolicyResult {IsSuccess = true};
            try
            {
                Organization org = GetOrganization(itemId);
                if (org == null)
                {
                    res.IsSuccess = false;
                    res.ErrorCodes.Add(ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID);
                    return res;
                }

                Organizations orgProxy;
                try
                {
                    orgProxy = GetOrganizationProxy(org.ServiceId);
                }
                catch(Exception ex)
                {
                    res.IsSuccess = false;
                    res.ErrorCodes.Add(ErrorCodes.CANNOT_GET_ORGANIZATION_PROXY);
                    TaskManager.WriteError(ex);
                    return res;
                }

                 PasswordPolicyResult policyRes = orgProxy.GetPasswordPolicy();                
                 res.ErrorCodes.AddRange(policyRes.ErrorCodes);
                 if (!policyRes.IsSuccess)
                 {
                     res.IsSuccess = false;
                     return res;
                 }
                
                res.Value = policyRes.Value;
            }
            catch(Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            return res;
        }
    }
    
}
