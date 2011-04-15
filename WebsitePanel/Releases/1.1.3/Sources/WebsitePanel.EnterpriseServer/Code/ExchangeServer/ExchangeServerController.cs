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
using System.Threading;
using WebsitePanel.EnterpriseServer.Code.HostedSolution;
using WebsitePanel.Providers;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.Exchange;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.OCS;
using WebsitePanel.Providers.ResultObjects;


namespace WebsitePanel.EnterpriseServer
{
	public class ExchangeServerController
	{
		#region Organizations
		public static DataSet GetRawExchangeOrganizationsPaged(int packageId, bool recursive,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				DataSet ds = new DataSet();

				// total records
				DataTable dtTotal = ds.Tables.Add();
				dtTotal.Columns.Add("Records", typeof(int));
				dtTotal.Rows.Add(3);

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
				dtItems.Rows.Add(1, "contoso", "Contoso", "Hosted Exchange", 1, "Customer", 1);
				dtItems.Rows.Add(1, "gencons", "General Consultants", "Hosted Exchange", 1, "Customer", 1);

				return ds;
			}
			#endregion

			return PackageController.GetRawPackageItemsPaged(
				packageId, ResourceGroups.Exchange, typeof(Organization),
				recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		public static OrganizationsPaged GetExchangeOrganizationsPaged(int packageId, bool recursive,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			ServiceItemsPaged items = PackageController.GetPackageItemsPaged(
				packageId, ResourceGroups.Exchange, typeof(Organization),
				recursive, filterColumn, filterValue, sortColumn, startRow, maximumRows);

			OrganizationsPaged orgs = new OrganizationsPaged();
			orgs.RecordsCount = items.RecordsCount;
			orgs.PageItems = new Organization[items.PageItems.Length];

			for (int i = 0; i < orgs.PageItems.Length; i++)
				orgs.PageItems[i] = (Organization)items.PageItems[i];

			return orgs;
		}

		public static List<Organization> GetExchangeOrganizations(int packageId, bool recursive)
		{
			List<ServiceProviderItem> items = PackageController.GetPackageItemsByType(
				packageId, typeof(Organization), recursive);

			return items.ConvertAll<Organization>(
				new Converter<ServiceProviderItem, Organization>(
				delegate(ServiceProviderItem item) { return (Organization)item; }));
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
				stats.AllocatedMailboxes = 10;
				stats.CreatedMailboxes = 4;
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
				return stats;
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_ORG_STATS");
			TaskManager.ItemId = itemId;

			try
			{
				Organization org = (Organization)PackageController.GetPackageItem(itemId);
				if (org == null)
					return null;

				OrganizationStatistics stats = ObjectUtils.FillObjectFromDataReader<OrganizationStatistics>(
					DataProvider.GetExchangeOrganizationStatistics(itemId));
				
				// disk space
				stats.UsedDiskSpace = org.DiskSpace;

				// allocated quotas
				PackageContext cntx = PackageController.GetPackageContext(org.PackageId);
				stats.AllocatedMailboxes = cntx.Quotas[Quotas.EXCHANGE2007_MAILBOXES].QuotaAllocatedValue;
				stats.AllocatedContacts = cntx.Quotas[Quotas.EXCHANGE2007_CONTACTS].QuotaAllocatedValue;
				stats.AllocatedDistributionLists = cntx.Quotas[Quotas.EXCHANGE2007_DISTRIBUTIONLISTS].QuotaAllocatedValue;
				stats.AllocatedPublicFolders = cntx.Quotas[Quotas.EXCHANGE2007_PUBLICFOLDERS].QuotaAllocatedValue;				
				stats.AllocatedDiskSpace = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;

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

		public static int CalculateOrganizationDiskspace(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "CALCULATE_DISKSPACE");
			TaskManager.ItemId = itemId;

			try
			{
				// create thread parameters
				ThreadStartParameters prms = new ThreadStartParameters();
				prms.UserId = SecurityContext.User.UserId;
				prms.Parameters = new object[] { itemId };

				Thread t = new Thread(CalculateOrganizationDiskspaceAsync);
				t.Start(prms);
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

		private static void CalculateOrganizationDiskspaceAsync(object objPrms)
		{
			ThreadStartParameters prms = (ThreadStartParameters)objPrms;

			// impersonate thread
			SecurityContext.SetThreadPrincipal(prms.UserId);

			int itemId = (int)prms.Parameters[0];
			
			// calculate disk space
			CalculateOrganizationDiskspaceInternal(itemId);
		}

		internal static void CalculateOrganizationDiskspaceInternal(int itemId)
		{
			try
			{
				// calculate disk space
				Organization org = (Organization)PackageController.GetPackageItem(itemId);
				if (org == null)
					return;

                SoapServiceProviderItem soapOrg = SoapServiceProviderItem.Wrap(org);


                int exchangeServiceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.Exchange);

                ServiceProvider exchange = GetServiceProvider(exchangeServiceId, org.ServiceId);
                
				ServiceProviderItemDiskSpace[] itemsDiskspace = exchange.GetServiceItemsDiskSpace(new SoapServiceProviderItem[] { soapOrg });

				
                if (itemsDiskspace != null && itemsDiskspace.Length > 0)
				{
					// set disk space
					org.DiskSpace = (int)Math.Round(((float)itemsDiskspace[0].DiskSpace / 1024 / 1024));

					// save organization
					UpdateOrganization(org);
				}
			}
			catch (Exception ex)
			{
				// write to audit log
				TaskManager.WriteError(ex);
			}
		}

		private static bool OrganizationIdentifierExists(string organizationId)
		{
			return DataProvider.ExchangeOrganizationExists(organizationId);
		}

	
        
       

        private static int ExtendToExchangeOrganization(ref Organization org)
        {                        
            // place log record
            TaskManager.StartTask("EXCHANGE", "CREATE_ORG", org.Name);
            TaskManager.TaskParameters["Organization ID"] = org.OrganizationId;

            try
            {            
                // provision organization in Exchange
                int serviceId = GetExchangeServiceID(org.PackageId);
                int[] hubTransportServiceIds;
                int[] clientAccessServiceIds;
                
                GetExchangeServices(serviceId, out hubTransportServiceIds, out clientAccessServiceIds);

                
                ExchangeServer mailboxRole = GetExchangeServer(serviceId, org.ServiceId);                                
                
                
               
                
               
         
                bool authDomainCreated = false;               
                int itemId = 0;               
                bool organizationExtended = false;

                List<OrganizationDomainName> domains = null;
                try
                {                                        
                    // 1) Create Organization (Mailbox)
                    // ================================
                    Organization exchangeOrganization = mailboxRole.ExtendToExchangeOrganization(org.OrganizationId, org.SecurityGroup);
                    organizationExtended = true;

                    exchangeOrganization.OrganizationId = org.OrganizationId;
                    exchangeOrganization.PackageId = org.PackageId;
                    exchangeOrganization.ServiceId = org.ServiceId;

                    exchangeOrganization.DefaultDomain = org.DefaultDomain;
                    exchangeOrganization.Name = org.Name;
                    exchangeOrganization.Id = org.Id;
                    exchangeOrganization.SecurityGroup = org.SecurityGroup;
                    exchangeOrganization.DistinguishedName = org.DistinguishedName;
                    exchangeOrganization.CrmAdministratorId = org.CrmAdministratorId;
                    exchangeOrganization.CrmCollation = org.CrmCollation;
                    exchangeOrganization.CrmCurrency = org.CrmCurrency;
                    exchangeOrganization.CrmLanguadgeCode = org.CrmLanguadgeCode;
                    exchangeOrganization.CrmOrganizationId = org.CrmOrganizationId;
                    exchangeOrganization.CrmOrgState = org.CrmOrgState;
                    exchangeOrganization.CrmUrl = org.CrmUrl;

                    org = exchangeOrganization;

                    // 2) Get OAB virtual directories from Client Access servers and
                    //    create Create Organization OAB (Mailbox)
                    // ==========================================
                    List<string> oabVirtualDirs = new List<string>();
                    foreach (int id in clientAccessServiceIds)
                    {
                        ExchangeServer clientAccessRole = null;
                        try
                        {
                            clientAccessRole = GetExchangeServer(id, org.ServiceId);
                        }
                        catch(Exception ex)
                        {
                            TaskManager.WriteError(ex);
                            continue;
                        }
                        oabVirtualDirs.Add(clientAccessRole.GetOABVirtualDirectory());
                    }
                    
                    Organization orgOAB = mailboxRole.CreateOrganizationOfflineAddressBook(org.OrganizationId, org.SecurityGroup, string.Join(",", oabVirtualDirs.ToArray()));
                    org.OfflineAddressBook = orgOAB.OfflineAddressBook;
                    

                    // 3) Add organization domains (Hub Transport)
                    domains = OrganizationController.GetOrganizationDomains(org.Id);

                    foreach (int id in hubTransportServiceIds)
                    {
                        ExchangeServer hubTransportRole = null;
                        try
                        {
                            hubTransportRole = GetExchangeServer(id, org.ServiceId);
                        }
                        catch(Exception ex)
                        {
                            TaskManager.WriteError(ex);
                            continue;                            
                        }
                        
                        string[] existingDomains = hubTransportRole.GetAuthoritativeDomains();
                        if (existingDomains != null)
                            Array.Sort(existingDomains);

                        foreach (OrganizationDomainName domain in domains)
                        {
                            if (existingDomains == null || Array.BinarySearch(existingDomains, domain.DomainName) < 0)
                            {
                                hubTransportRole.AddAuthoritativeDomain(domain.DomainName);
                            }
                        }
                        authDomainCreated = true;
                        break;
                    }

                    PackageContext cntx = PackageController.GetPackageContext(org.PackageId);
                    // organization limits
                    org.IssueWarningKB = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;
                    if (org.IssueWarningKB > 0)
						org.IssueWarningKB *= Convert.ToInt32(1024*0.9); //90%
                    org.ProhibitSendKB = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;
                    if (org.ProhibitSendKB > 0)
						org.ProhibitSendKB *= 1024; //100%
                    org.ProhibitSendReceiveKB = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue;
                    if (org.ProhibitSendReceiveKB > 0)
						org.ProhibitSendReceiveKB *= 1024; //100%

                    StringDictionary settings = ServerController.GetServiceSettings(serviceId);                    
                    org.KeepDeletedItemsDays = Utils.ParseInt(settings["KeepDeletedItemsDays"], 14);                              
                    
                }
                catch (Exception ex)
                {
                    
                    // rollback organization creation
                    if (organizationExtended)                    
                        mailboxRole.DeleteOrganization(org.OrganizationId, org.DistinguishedName,
                            org.GlobalAddressList, org.AddressList, org.OfflineAddressBook, org.SecurityGroup);

                        // rollback domain
                        if (authDomainCreated)
                            foreach (int id in hubTransportServiceIds)
                            {
                                ExchangeServer hubTransportRole = null;
                                try
                                {
                                    hubTransportRole = GetExchangeServer(id, org.ServiceId);
                                }
                                catch (Exception exe)
                                {
                                    TaskManager.WriteError(exe);
                                    continue;
                                }
                                
                                foreach (OrganizationDomainName domain in domains)
                                {
                                    hubTransportRole.DeleteAuthoritativeDomain(domain.DomainName);    
                                                                        
                                }

                                break;                                
                            }

                    throw TaskManager.WriteError(ex);
                }

                return itemId;
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

		private static int[] ParseMultiSetting(int mailboxServiceId, string settingName)
		{
            List<int> retIds = new List<int>();
            StringDictionary settings = ServerController.GetServiceSettings(mailboxServiceId);
            if (!String.IsNullOrEmpty(settings[settingName]))
            {
                string[] ids = settings[settingName].Split(',');
                
                int res;
                foreach (string id in ids)
                {
                    if (int.TryParse(id, out res))
                        retIds.Add(res);
                }                
            }

            if (retIds.Count == 0)
                retIds.Add(mailboxServiceId);
		    
            return retIds.ToArray();
    
		}
        
        private static void GetExchangeServices(int mailboxServiceId,
			out int[] hubTransportServiceIds, out int[] clientAccessServiceIds)
		{		    
            hubTransportServiceIds = ParseMultiSetting(mailboxServiceId, "HubTransportServiceID");

            clientAccessServiceIds = ParseMultiSetting(mailboxServiceId, "ClientAccessServiceID");				
		}

		public static int DeleteOrganization(int itemId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_ORG");
			TaskManager.ItemId = itemId;

			try
			{
				// delete organization in Exchange
				//System.Threading.Thread.Sleep(5000);
				Organization org = (Organization)PackageController.GetPackageItem(itemId);

			    int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
				
                bool successful = exchange.DeleteOrganization(
					org.OrganizationId,
					org.DistinguishedName,
					org.GlobalAddressList,
					org.AddressList,
					org.OfflineAddressBook,
					org.SecurityGroup);

                
				
				return successful ? 0 : BusinessErrorCodes.ERROR_EXCHANGE_DELETE_SOME_PROBLEMS;
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

		public static Organization GetOrganizationStorageLimits(int itemId)
		{
			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_ORG_LIMITS");
			TaskManager.ItemId = itemId;

			try
			{
				return GetOrganization(itemId);
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

		public static int SetOrganizationStorageLimits(int itemId, int issueWarningKB, int prohibitSendKB,
			int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "SET_ORG_LIMITS");
			TaskManager.ItemId = itemId;

			try
			{
				Organization org = (Organization)PackageController.GetPackageItem(itemId);
				if (org == null)
					return 0;

				// load package context
				PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

				int maxDiskSpace = 0;
				if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_DISKSPACE)
					&& cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue > 0)
					maxDiskSpace = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue * 1024;

                if (maxDiskSpace > 0 && (issueWarningKB > maxDiskSpace || prohibitSendKB > maxDiskSpace || prohibitSendReceiveKB > maxDiskSpace || issueWarningKB == -1 || prohibitSendKB == -1 || prohibitSendReceiveKB == -1))
                    return BusinessErrorCodes.ERROR_EXCHANGE_STORAGE_QUOTAS_EXCEED_HOST_VALUES;

				// set limits
				org.IssueWarningKB = issueWarningKB;
				org.ProhibitSendKB = prohibitSendKB;
				org.ProhibitSendReceiveKB = prohibitSendReceiveKB;
				org.KeepDeletedItemsDays = keepDeletedItemsDays;

				// save organization
				UpdateOrganization(org);

				if (applyToMailboxes)
				{

                    int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                    ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

					exchange.SetOrganizationStorageLimits(org.DistinguishedName,
						issueWarningKB,
						prohibitSendKB,
						prohibitSendReceiveKB,
						keepDeletedItemsDays);
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

		public static ExchangeItemStatistics[] GetMailboxesStatistics(int itemId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				List<ExchangeItemStatistics> items = new List<ExchangeItemStatistics>();
				ExchangeItemStatistics item1 = new ExchangeItemStatistics();
				item1.ItemName = "John Smith";
				item1.TotalItems = 105;
				item1.TotalSizeMB = 14;
				item1.LastLogon = DateTime.Now;
				item1.LastLogoff = DateTime.Now;
				items.Add(item1);

				ExchangeItemStatistics item2 = new ExchangeItemStatistics();
				item2.ItemName = "Jack Brown";
				item2.TotalItems = 5;
				item2.TotalSizeMB = 2;
				item2.LastLogon = DateTime.Now;
				item2.LastLogoff = DateTime.Now;
				items.Add(item2);

				ExchangeItemStatistics item3 = new ExchangeItemStatistics();
				item3.ItemName = "Marry Smith";
				item3.TotalItems = 1302;
				item3.TotalSizeMB = 45;
				item3.LastLogon = DateTime.Now;
				item3.LastLogoff = DateTime.Now;
				items.Add(item3);

				return items.ToArray();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_MAILBOXES_STATS");
			TaskManager.ItemId = itemId;

			try
			{
				Organization org = (Organization)PackageController.GetPackageItem(itemId);
				if (org == null)
					return null;

				
                // get stats
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

				return exchange.GetMailboxesStatistics(org.DistinguishedName);
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

		public static ExchangeItemStatistics[] GetPublicFoldersStatistics(int itemId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				List<ExchangeItemStatistics> items = new List<ExchangeItemStatistics>();
				ExchangeItemStatistics item1 = new ExchangeItemStatistics();
				item1.ItemName = "\\fabrikam\\Documents";
				item1.TotalItems = 6;
				item1.TotalSizeMB = 56;
				item1.LastModificationTime = DateTime.Now;
				item1.LastAccessTime = DateTime.Now;
				items.Add(item1);

				ExchangeItemStatistics item2 = new ExchangeItemStatistics();
				item2.ItemName = "\\fabrikam\\Documents\\Legal";
				item2.TotalItems = 5;
				item2.TotalSizeMB = 4;
				item2.LastModificationTime = DateTime.Now;
				item2.LastAccessTime = DateTime.Now;
				items.Add(item2);

				ExchangeItemStatistics item3 = new ExchangeItemStatistics();
				item3.ItemName = "\\fabrikam\\Documents\\Contracts";
				item3.TotalItems = 8;
				item3.TotalSizeMB = 2;
				item3.LastModificationTime = DateTime.Now;
				item3.LastAccessTime = DateTime.Now;
				items.Add(item3);

				return items.ToArray();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_FOLDERS_STATS");
			TaskManager.ItemId = itemId;

			try
			{
				Organization org = (Organization)PackageController.GetPackageItem(itemId);
				if (org == null)
					return null;

				// get the list of all public folders
				List<string> folderNames = new List<string>();
				List<ExchangeAccount> folders = GetAccounts(itemId, ExchangeAccountType.PublicFolder);
				foreach (ExchangeAccount folder in folders)
					folderNames.Add(folder.DisplayName);

				// get stats
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
				return exchange.GetPublicFoldersStatistics(folderNames.ToArray());
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

        public static ExchangeActiveSyncPolicy GetActiveSyncPolicy(int itemId)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                ExchangeActiveSyncPolicy p = new ExchangeActiveSyncPolicy();
                p.MaxAttachmentSizeKB = -1;
                p.MaxPasswordFailedAttempts = -1;
                p.MinPasswordLength = 0;
                p.InactivityLockMin = -1;
                p.PasswordExpirationDays = -1;
                p.PasswordHistory = 0;
                return p;
            }
            #endregion

            // place log record
            TaskManager.StartTask("EXCHANGE", "GET_ACTIVESYNC_POLICY");
            TaskManager.ItemId = itemId;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return null;

                // get policy
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                //Create Exchange Organization
                if (string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExtendToExchangeOrganization(ref org);

                    PackageController.UpdatePackageItem(org);
                }
                ExchangeActiveSyncPolicy policy = exchange.GetActiveSyncPolicy(org.OrganizationId);

                // create policy if required
                if (policy == null)
                {
                    exchange.CreateOrganizationActiveSyncPolicy(org.OrganizationId);
                    return exchange.GetActiveSyncPolicy(org.OrganizationId);
                }

                return policy;
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

        public static int SetActiveSyncPolicy(int itemId, bool allowNonProvisionableDevices,
                bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled,
                bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled,
                bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength,
                int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInterval)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("EXCHANGE", "SET_ACTIVESYNC_POLICY");
            TaskManager.ItemId = itemId;

            try
            {
                Organization org = (Organization)PackageController.GetPackageItem(itemId);
                if (org == null)
                    return 0;

                // get policy
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                exchange.SetActiveSyncPolicy(org.OrganizationId, allowNonProvisionableDevices, attachmentsEnabled,
                    maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired,
                    passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts, 
                    minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);

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

		private static void UpdateOrganization(Organization organization)
		{
			PackageController.UpdatePackageItem(organization);
		}
		#endregion

		#region Accounts

		private static bool AccountExists(string accountName)
		{
			return DataProvider.ExchangeAccountExists(accountName);
		}

		public static ExchangeAccountsPaged GetAccountsPaged(int itemId, string accountTypes,
			string filterColumn, string filterValue, string sortColumn,
			int startRow, int maximumRows)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
                string []parseedAccountTypes = Utils.ParseDelimitedString(accountTypes, ',');
                                                
				ExchangeAccountsPaged res = new ExchangeAccountsPaged();
                res.PageItems = GetAccounts(itemId, (ExchangeAccountType)Utils.ParseInt(parseedAccountTypes[0], 1)).ToArray();
				res.RecordsCount = res.PageItems.Length;
				return res;
			}
			#endregion

			DataSet ds = DataProvider.GetExchangeAccountsPaged(SecurityContext.User.UserId, itemId,
                accountTypes, filterColumn, filterValue, sortColumn, startRow, maximumRows);

			ExchangeAccountsPaged result = new ExchangeAccountsPaged();
			result.RecordsCount = (int)ds.Tables[0].Rows[0][0];
			
			List<ExchangeAccount> accounts = new List<ExchangeAccount>();
			ObjectUtils.FillCollectionFromDataView(accounts, ds.Tables[1].DefaultView);
			result.PageItems = accounts.ToArray();
			return result;
		}

		public static List<ExchangeAccount> GetAccounts(int itemId, ExchangeAccountType accountType)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				if (accountType == ExchangeAccountType.Mailbox)
					return SearchAccounts(0, true, false, false, true, true, "", "", "");
				else if (accountType == ExchangeAccountType.Contact)
					return SearchAccounts(0, false, true, false, false, false, "", "", "");
				else if (accountType == ExchangeAccountType.DistributionList)
                    return SearchAccounts(0, false, false, true, false, false, "", "", "");
				else
				{
					List<ExchangeAccount> demoAccounts = new List<ExchangeAccount>();
					ExchangeAccount f1 = new ExchangeAccount();
					f1.AccountId = 7;
					f1.AccountName = "documents_fabrikam";
					f1.AccountType = ExchangeAccountType.PublicFolder;
					f1.DisplayName = "\\fabrikam\\Documents";
					f1.PrimaryEmailAddress = "documents@fabrikam.net";
					f1.MailEnabledPublicFolder = true;
					demoAccounts.Add(f1);

					ExchangeAccount f2 = new ExchangeAccount();
					f2.AccountId = 8;
					f2.AccountName = "documents_fabrikam";
					f2.AccountType = ExchangeAccountType.PublicFolder;
					f2.DisplayName = "\\fabrikam\\Documents\\Legal";
					f2.PrimaryEmailAddress = "";
					demoAccounts.Add(f2);

					ExchangeAccount f3 = new ExchangeAccount();
					f3.AccountId = 9;
					f3.AccountName = "documents_fabrikam";
					f3.AccountType = ExchangeAccountType.PublicFolder;
					f3.DisplayName = "\\fabrikam\\Documents\\Contracts";
					f3.PrimaryEmailAddress = "";
					demoAccounts.Add(f3);
					return demoAccounts;
				}
			}
			#endregion

			return ObjectUtils.CreateListFromDataReader<ExchangeAccount>(
				DataProvider.GetExchangeAccounts(itemId, (int)accountType));
		}

        public static List<ExchangeAccount> GetExchangeMailboxes(int itemId)
        {
            return ObjectUtils.CreateListFromDataReader<ExchangeAccount>(DataProvider.GetExchangeMailboxes(itemId));
        }

		public static List<ExchangeAccount> SearchAccounts(int itemId,
			bool includeMailboxes, bool includeContacts, bool includeDistributionLists,
            bool includeRooms, bool includeEquipment,
			string filterColumn, string filterValue, string sortColumn)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				List<ExchangeAccount> demoAccounts = new List<ExchangeAccount>();

				if (includeMailboxes)
				{
					ExchangeAccount m1 = new ExchangeAccount();
					m1.AccountId = 1;
					m1.AccountName = "john_fabrikam";
					m1.AccountType = ExchangeAccountType.Mailbox;
					m1.DisplayName = "John Smith";
					m1.PrimaryEmailAddress = "john@fabrikam.net";
					demoAccounts.Add(m1);

				

					ExchangeAccount m3 = new ExchangeAccount();
					m3.AccountId = 3;
					m3.AccountName = "marry_fabrikam";
					m3.AccountType = ExchangeAccountType.Mailbox;
					m3.DisplayName = "Marry Smith";
					m3.PrimaryEmailAddress = "marry@fabrikam.net";
					demoAccounts.Add(m3);
				}

                if (includeRooms)
                {
                    ExchangeAccount r1 = new ExchangeAccount();
                    r1.AccountId = 20;
                    r1.AccountName = "room1_fabrikam";
                    r1.AccountType = ExchangeAccountType.Room;
                    r1.DisplayName = "Meeting Room 1";
                    r1.PrimaryEmailAddress = "room1@fabrikam.net";
                    demoAccounts.Add(r1);
                }

                if (includeEquipment)
                {
                    ExchangeAccount e1 = new ExchangeAccount();
                    e1.AccountId = 21;
                    e1.AccountName = "projector_fabrikam";
                    e1.AccountType = ExchangeAccountType.Equipment;
                    e1.DisplayName = "Projector 1";
                    e1.PrimaryEmailAddress = "projector@fabrikam.net";
                    demoAccounts.Add(e1);
                }

				if (includeContacts)
				{
					ExchangeAccount c1 = new ExchangeAccount();
					c1.AccountId = 4;
					c1.AccountName = "pntr1_fabrikam";
					c1.AccountType = ExchangeAccountType.Contact;
					c1.DisplayName = "WebsitePanel Support";
					c1.PrimaryEmailAddress = "support@websitepanel.net";
					demoAccounts.Add(c1);

					ExchangeAccount c2 = new ExchangeAccount();
					c2.AccountId = 5;
					c2.AccountName = "acc1_fabrikam";
					c2.AccountType = ExchangeAccountType.Contact;
					c2.DisplayName = "John Home Account";
					c2.PrimaryEmailAddress = "john@yahoo.com";
					demoAccounts.Add(c2);
				}

				if (includeDistributionLists)
				{
					ExchangeAccount d1 = new ExchangeAccount();
					d1.AccountId = 6;
					d1.AccountName = "sales_fabrikam";
					d1.AccountType = ExchangeAccountType.DistributionList;
					d1.DisplayName = "Fabrikam Sales Dept";
					d1.PrimaryEmailAddress = "sales@fabrikam.net";
					demoAccounts.Add(d1);
				}

				return demoAccounts;
			}
			#endregion

			return ObjectUtils.CreateListFromDataReader<ExchangeAccount>(
				DataProvider.SearchExchangeAccounts(SecurityContext.User.UserId, itemId, includeMailboxes, includeContacts,
				includeDistributionLists, includeRooms, includeEquipment,
                filterColumn, filterValue, sortColumn));
		}



		public static ExchangeAccount GetAccount(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				ExchangeAccount m1 = new ExchangeAccount();
				m1.AccountId = 1;
				m1.AccountName = "john_fabrikam";
				m1.AccountType = ExchangeAccountType.Mailbox;
				m1.DisplayName = "John Smith";
				m1.PrimaryEmailAddress = "john@fabrikam.net";
				return m1;
			}
			#endregion

			ExchangeAccount account = ObjectUtils.FillObjectFromDataReader<ExchangeAccount>(
				DataProvider.GetExchangeAccount(itemId, accountId));

            if (account == null)
                return null;

            // decrypt password
            account.AccountPassword = CryptoUtils.Decrypt(account.AccountPassword);

            return account;
		}

        public static bool CheckAccountCredentials(int itemId, string email, string password)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "AUTHENTICATE", email);
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return false;

                // check credentials
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                return exchange.CheckAccountCredentials(email, password);
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
                return false;
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static ExchangeAccount SearchAccount(ExchangeAccountType accountType, string primaryEmailAddress)
        {
            ExchangeAccount account = ObjectUtils.FillObjectFromDataReader<ExchangeAccount>(
                DataProvider.SearchExchangeAccount(SecurityContext.User.UserId,
                (int)accountType, primaryEmailAddress));

            if (account == null)
                return null;

            // decrypt password
            account.AccountPassword = CryptoUtils.Decrypt(account.AccountPassword);

            return account;
        }

		private static int AddAccount(int itemId, ExchangeAccountType accountType,
			string accountName, string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            MailboxManagerActions mailboxManagerActions, string samAccountName, string accountPassword)
		{
			return DataProvider.AddExchangeAccount(itemId, (int)accountType,
				accountName, displayName, primaryEmailAddress, mailEnabledPublicFolder,
                mailboxManagerActions.ToString(), samAccountName, CryptoUtils.Encrypt(accountPassword));
		}

		private static void UpdateAccount(ExchangeAccount account)
		{
			DataProvider.UpdateExchangeAccount(account.AccountId, account.AccountName, account.AccountType, account.DisplayName,
				account.PrimaryEmailAddress,account.MailEnabledPublicFolder,
                account.MailboxManagerActions.ToString(), account.SamAccountName, account.AccountPassword);
		}

		private static void DeleteAccount(int itemId, int accountId)
		{
			// try to get organization
			if (GetOrganization(itemId) == null)
				return;

			DataProvider.DeleteExchangeAccount(itemId, accountId);
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

		#endregion

		#region Account Email Addresses
		private static bool EmailAddressExists(string emailAddress)
		{
			return DataProvider.ExchangeAccountEmailAddressExists(emailAddress);
		}

        
		private static ExchangeEmailAddress[] GetAccountEmailAddresses(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				List<ExchangeEmailAddress> demoEmails = new List<ExchangeEmailAddress>();
				ExchangeEmailAddress e1 = new ExchangeEmailAddress();
				e1.EmailAddress = "john@fabrikam.net";
				e1.IsPrimary = true;
				demoEmails.Add(e1);

				ExchangeEmailAddress e2 = new ExchangeEmailAddress();
				e2.EmailAddress = "john.smith@fabrikam.net";
				demoEmails.Add(e2);

				ExchangeEmailAddress e3 = new ExchangeEmailAddress();
				e3.EmailAddress = "john@fabrikam.hosted-exchange.com";
				demoEmails.Add(e3);
				return demoEmails.ToArray();
			}
			#endregion

			List<ExchangeEmailAddress> emails = ObjectUtils.CreateListFromDataReader<ExchangeEmailAddress>(
				DataProvider.GetExchangeAccountEmailAddresses(accountId));

			// load account
			ExchangeAccount account = GetAccount(itemId, accountId);

			foreach (ExchangeEmailAddress email in emails)
			{
				if (String.Compare(account.PrimaryEmailAddress, email.EmailAddress, true) == 0)
				{
					email.IsPrimary = true;
					break;
				}
			}

			return emails.ToArray();
		}

		private static void AddAccountEmailAddress(int accountId, string emailAddress)
		{
			DataProvider.AddExchangeAccountEmailAddress(accountId, emailAddress);
		}

		private static void DeleteAccountEmailAddresses(int accountId, string[] emailAddresses)
		{
			foreach (string emailAddress in emailAddresses)
				DataProvider.DeleteExchangeAccountEmailAddress(accountId, emailAddress);
		}

		#endregion

		#region Domains
		public static List<ExchangeDomainName> GetOrganizationDomains(int itemId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				List<ExchangeDomainName> demoDomains = new List<ExchangeDomainName>();
				ExchangeDomainName d1 = new ExchangeDomainName();
				d1.DomainId = 1;
				d1.DomainName = "fabrikam.hosted-exchange.com";
				d1.IsDefault = false;
				d1.IsHost = true;
				demoDomains.Add(d1);

				ExchangeDomainName d2 = new ExchangeDomainName();
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
			List<ExchangeDomainName> domains = ObjectUtils.CreateListFromDataReader<ExchangeDomainName>(
				DataProvider.GetExchangeOrganizationDomains(itemId));

			// set default domain
			foreach (ExchangeDomainName domain in domains)
			{
				if (String.Compare(domain.DomainName, org.DefaultDomain, true) == 0)
				{
					domain.IsDefault = true;
					break;
				}
			}

			return domains;
		}

        public static int AddAuthoritativeDomain(int itemId, int domainId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("EXCHANGE", "ADD_DOMAIN");
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


                // delete domain on Exchange
                int[] hubTransportServiceIds;
                int[] clientAccessServiceIds;
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                GetExchangeServices(exchangeServiceId, out hubTransportServiceIds, out clientAccessServiceIds);

                foreach (int id in hubTransportServiceIds)
                {
                    ExchangeServer hubTransportRole = null;
                    try
                    {
                        hubTransportRole = GetExchangeServer(id, org.ServiceId);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex);
                        continue;
                    }

                    string[] domains = hubTransportRole.GetAuthoritativeDomains();
                    if (domains != null)
                        Array.Sort(domains);

                    if (domains == null || Array.BinarySearch(domains, domain.DomainName) < 0)
                        hubTransportRole.AddAuthoritativeDomain(domain.DomainName);
                    break;
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
		

		
		public static int DeleteAuthoritativeDomain(int itemId, int domainId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_DOMAIN");
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
				if(domain == null)
					return -1;

			
				// delete domain on Exchange
				int[] hubTransportServiceIds;
				int[] clientAccessServiceIds;
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
				GetExchangeServices(exchangeServiceId, out hubTransportServiceIds, out clientAccessServiceIds);

                foreach (int id in hubTransportServiceIds)
                {
                    ExchangeServer hubTransportRole = null;
                    try
                    {
                        hubTransportRole = GetExchangeServer(id, org.ServiceId);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex);
                        continue;
                    }

                    hubTransportRole.DeleteAuthoritativeDomain(domain.DomainName);
                    break;

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


		#endregion

		#region Mailboxes
               
        private static void UpdateExchangeAccount(int  accountId, string accountName, ExchangeAccountType accountType,
            string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            string mailboxManagerActions, string samAccountName, string accountPassword)
		{
            DataProvider.UpdateExchangeAccount(accountId, 
                accountName, 
                accountType, 
                displayName, 
                primaryEmailAddress, 
                mailEnabledPublicFolder, 
                mailboxManagerActions,
                samAccountName,
                CryptoUtils.Encrypt(accountPassword));
        }


		public static int CreateMailbox(int itemId, int accountId, ExchangeAccountType accountType, string accountName,
			string displayName, string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check mailbox quota
			OrganizationStatistics orgStats = GetOrganizationStatistics(itemId);
			if ((orgStats.AllocatedMailboxes > -1 ) && ( orgStats.CreatedMailboxes >= orgStats.AllocatedMailboxes))
				return BusinessErrorCodes.ERROR_EXCHANGE_MAILBOXES_QUOTA_LIMIT;

			// place log record
			TaskManager.StartTask("EXCHANGE", "CREATE_MAILBOX");
			TaskManager.ItemId = itemId;
			bool userCreated = false;
			Organization org = null;
			try
			{
				// load organization
				org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// e-mail
				string email = name + "@" + domain;
				bool enabled = (accountType == ExchangeAccountType.Mailbox);
				

				//  string accountName = string.Empty;
				//Create AD user if needed
				if (accountId == 0)
				{
					accountId = OrganizationController.CreateUser(org.Id, displayName, name, domain, password, enabled, false, string.Empty, out accountName);
					if (accountId > 0)
						userCreated = true;
				}
				if (accountId < 0)
					return accountId;

				int exchangeServiceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.Exchange);
				
				ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);


				//Create Exchange Organization
				if (string.IsNullOrEmpty(org.GlobalAddressList))
				{
					ExtendToExchangeOrganization(ref org);

					PackageController.UpdatePackageItem(org);
				}

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;


				// load package context
				PackageContext cntx = PackageController.GetPackageContext(org.PackageId);


				string samAccount = exchange.CreateMailEnableUser(email, org.OrganizationId, org.DistinguishedName, accountType, org.Database,
											  org.OfflineAddressBook,
											  accountName,
											  QuotaEnabled(cntx, Quotas.EXCHANGE2007_POP3ENABLED),
											  QuotaEnabled(cntx, Quotas.EXCHANGE2007_IMAPENABLED),
											  QuotaEnabled(cntx, Quotas.EXCHANGE2007_OWAENABLED),
											  QuotaEnabled(cntx, Quotas.EXCHANGE2007_MAPIENABLED),
											  QuotaEnabled(cntx, Quotas.EXCHANGE2007_ACTIVESYNCENABLED),
											  org.IssueWarningKB,
											  org.ProhibitSendKB,
											  org.ProhibitSendReceiveKB,
											  org.KeepDeletedItemsDays);

				MailboxManagerActions pmmActions = MailboxManagerActions.GeneralSettings
					| MailboxManagerActions.MailFlowSettings
					| MailboxManagerActions.AdvancedSettings
					| MailboxManagerActions.EmailAddresses;


				UpdateExchangeAccount(accountId, accountName, accountType, displayName, email, false, pmmActions.ToString(), samAccount, password);



				// send setup instructions
				if (sendSetupInstructions)
				{
					try
					{
						// send setup instructions
						int sendResult = SendMailboxSetupInstructions(itemId, accountId, true, setupInstructionMailAddress, null);
						if (sendResult < 0)
							TaskManager.WriteWarning("Setup instructions were not sent. Error code: " + sendResult);
					}
					catch (Exception ex)
					{
						TaskManager.WriteError(ex);
					}
				}

				try
				{
					// update OAB
					// check if this is the first mailbox within the organization
					if (GetAccounts(itemId, ExchangeAccountType.Mailbox).Count == 1)
						exchange.UpdateOrganizationOfflineAddressBook(org.OfflineAddressBook);
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex);
				}

				return accountId;
			}
			catch (Exception ex)
			{
				//rollback AD user
				if (userCreated)
				{
					try
					{
						OrganizationController.DeleteUser(org.Id, accountId);
					}
					catch (Exception rollbackException)
					{
						TaskManager.WriteError(rollbackException);
					}
				}
				throw TaskManager.WriteError(ex);

			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

        public static int DisableMailbox(int itemId, int accountId)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("EXCHANGE", "DISABLE_MAILBOX");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return -1;

                // load account
                ExchangeAccount account = GetAccount(itemId, accountId);

                if (BlackBerryController.CheckBlackBerryUserExists(accountId))
                {
                    BlackBerryController.DeleteBlackBerryUser(itemId, accountId);
                }
                
                // delete mailbox
                int serviceExchangeId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(serviceExchangeId, org.ServiceId);
                exchange.DisableMailbox(account.AccountName);

                account.AccountType = ExchangeAccountType.User;                
                account.MailEnabledPublicFolder = false;                                
                UpdateAccount(account);
                DataProvider.DeleteUserEmailAddresses(account.AccountId, account.PrimaryEmailAddress);
                
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

        
        public static int DeleteMailbox(int itemId, int accountId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_MAILBOX");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

                if (BlackBerryController.CheckBlackBerryUserExists(accountId))
                {
                    BlackBerryController.DeleteBlackBerryUser(itemId, accountId);
                }
                

				// delete mailbox
			    int serviceExchangeId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(serviceExchangeId, org.ServiceId);
				exchange.DeleteMailbox(account.AccountName);

				
                
                // unregister account
				DeleteAccount(itemId, accountId);

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

		private static ExchangeMailbox GetDemoMailboxSettings()
		{
			ExchangeMailbox mb = new ExchangeMailbox();
			mb.DisplayName = "John Smith";
			mb.Domain = "HSTDEXCH1";
			mb.AccountName = "john_fabrikam";
			mb.EnableForwarding = true;
			mb.EnableIMAP = true;
			mb.EnableMAPI = true;
			mb.EnablePOP = true;
			mb.FirstName = "John";
			mb.LastName = "Smith";
			mb.ForwardingAccount = GetAccounts(0, ExchangeAccountType.Mailbox)[1];
			mb.EnableForwarding = true;
			mb.IssueWarningKB = 150000;
			mb.KeepDeletedItemsDays = 14;
			mb.LastLogoff = DateTime.Now;
			mb.LastLogon = DateTime.Now;
			mb.ManagerAccount = GetAccounts(0, ExchangeAccountType.Mailbox)[1];
			mb.MaxReceiveMessageSizeKB = 20000;
			mb.MaxRecipients = 30;
			mb.MaxSendMessageSizeKB = 10000;
			mb.ProhibitSendKB = 160000;
			mb.ProhibitSendReceiveKB = 170000;
			mb.TotalItems = 5;
			mb.TotalSizeMB = 4;
			return mb;
		}

		public static ExchangeMailbox GetMailboxGeneralSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoMailboxSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_MAILBOX_GENERAL");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings

			    int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
				return exchange.GetMailboxGeneralSettings(account.AccountName);
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

		public static int SetMailboxGeneralSettings(int itemId, int accountId, string displayName,
			string password, bool hideAddressBook, bool disabled, string firstName, string initials,
			string lastName, string address, string city, string state, string zip, string country,
			string jobTitle, string company, string department, string office, string managerAccountName,
			string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
			string webPage, string notes)
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetMailboxGeneralSettings(
					account.AccountName,
					displayName,
					password,
					hideAddressBook,
					disabled,
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
					notes);

				// update account
				account.DisplayName = displayName;

                if (!String.IsNullOrEmpty(password))
                    account.AccountPassword = CryptoUtils.Encrypt(password);

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

		public static ExchangeEmailAddress[] GetMailboxEmailAddresses(int itemId, int accountId)
		{
			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_MAILBOX_ADDRESSES");
			TaskManager.ItemId = itemId;

			try
			{
				return GetAccountEmailAddresses(itemId, accountId);
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

		public static int AddMailboxEmailAddress(int itemId, int accountId, string emailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "ADD_MAILBOX_ADDRESS");
			TaskManager.ItemId = itemId;

			try
			{
				// check
				if (EmailAddressExists(emailAddress))
					return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// add e-mail
				AddAccountEmailAddress(accountId, emailAddress);

				// update e-mail addresses
				int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                exchange.SetMailboxEmailAddresses(
					account.AccountName,
					GetAccountSimpleEmailAddresses(itemId, accountId));

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

        private static OCSServer GetOCSProxy(int itemId)
        {
            Organization org = OrganizationController.GetOrganization(itemId);
            int serviceId = PackageController.GetPackageServiceId(org.PackageId, ResourceGroups.OCS);

            OCSServer ocs = new OCSServer();
            ServiceProviderProxy.Init(ocs, serviceId);


            return ocs;
        }
        
		public static int SetMailboxPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "SET_PRIMARY_MAILBOX_ADDRESS");
			TaskManager.ItemId = itemId;

			try
			{
				// get account
				ExchangeAccount account = GetAccount(itemId, accountId);
				account.PrimaryEmailAddress = emailAddress;

				// update exchange
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetMailboxPrimaryEmailAddress(
					account.AccountName,
					emailAddress);

                if (DataProvider.CheckOCSUserExists(account.AccountId))
                {
                    OCSServer ocs = GetOCSProxy(itemId);
                    string instanceId = DataProvider.GetOCSUserInstanceID(account.AccountId);
                    ocs.SetUserPrimaryUri(instanceId, emailAddress);
                }

				// save account
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

		public static int DeleteMailboxEmailAddresses(int itemId, int accountId, string[] emailAddresses)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_MAILBOX_ADDRESSES");
			TaskManager.ItemId = itemId;

			try
			{
				// get account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// delete e-mail addresses
				List<string> toDelete = new List<string>();
				foreach (string emailAddress in emailAddresses)
				{
					if (String.Compare(account.PrimaryEmailAddress, emailAddress, true) != 0)
						toDelete.Add(emailAddress);
				}

				// delete from meta-base
				DeleteAccountEmailAddresses(accountId, toDelete.ToArray());

				// delete from Exchange
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// update e-mail addresses
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetMailboxEmailAddresses(
					account.AccountName,
					GetAccountSimpleEmailAddresses(itemId, accountId));

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

		public static ExchangeMailbox GetMailboxMailFlowSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoMailboxSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_MAILBOX_MAILFLOW");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
				ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
				ExchangeMailbox mailbox = exchange.GetMailboxMailFlowSettings(account.AccountName);
				mailbox.DisplayName = account.DisplayName;
				return mailbox;
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

		public static int SetMailboxMailFlowSettings(int itemId, int accountId,
			bool enableForwarding, string forwardingAccountName, bool forwardToBoth,
			string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts,
			int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB,
            bool requireSenderAuthentication)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_MAILBOX_MAILFLOW");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetMailboxMailFlowSettings(account.AccountName,
					enableForwarding,
					forwardingAccountName,
					forwardToBoth,
					sendOnBehalfAccounts,
					acceptAccounts,
					rejectAccounts,
					maxRecipients,
					maxSendMessageSizeKB,
					maxReceiveMessageSizeKB,
					requireSenderAuthentication);

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

		public static ExchangeMailbox GetMailboxAdvancedSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoMailboxSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_MAILBOX_ADVANCED");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
				ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
				ExchangeMailbox mailbox = exchange.GetMailboxAdvancedSettings(account.AccountName);
				mailbox.DisplayName = account.DisplayName;
				return mailbox;
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

		public static int SetMailboxAdvancedSettings(int itemId, int accountId, bool enablePOP,
			bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync,
            int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_MAILBOX_ADVANCED");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// load package context
				PackageContext cntx = PackageController.GetPackageContext(org.PackageId);

				int maxDiskSpace = 0;
				if (cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_DISKSPACE)
					&& cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue > 0)
					maxDiskSpace = cntx.Quotas[Quotas.EXCHANGE2007_DISKSPACE].QuotaAllocatedValue * 1024;

				if ((maxDiskSpace > 0 &&
					(issueWarningKB > maxDiskSpace
					|| prohibitSendKB > maxDiskSpace
					|| prohibitSendReceiveKB > maxDiskSpace || issueWarningKB == -1 || prohibitSendKB == -1 || prohibitSendReceiveKB == -1)))
					return BusinessErrorCodes.ERROR_EXCHANGE_STORAGE_QUOTAS_EXCEED_HOST_VALUES;

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetMailboxAdvancedSettings(
					org.OrganizationId,
					account.AccountName,
					QuotaEnabled(cntx, Quotas.EXCHANGE2007_POP3ALLOWED) && enablePOP,
					QuotaEnabled(cntx, Quotas.EXCHANGE2007_IMAPALLOWED) && enableIMAP,
					QuotaEnabled(cntx, Quotas.EXCHANGE2007_OWAALLOWED) && enableOWA,
					QuotaEnabled(cntx, Quotas.EXCHANGE2007_MAPIALLOWED) && enableMAPI,
					QuotaEnabled(cntx, Quotas.EXCHANGE2007_ACTIVESYNCALLOWED) && enableActiveSync,
					issueWarningKB,
					prohibitSendKB,
					prohibitSendReceiveKB,
					keepDeletedItemsDays);

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

        public static int SetMailboxManagerSettings(int itemId, int accountId, bool pmmAllowed, MailboxManagerActions action)
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
                ExchangeAccount account = GetAccount(itemId, accountId);

                // PMM settings
                if (pmmAllowed) account.MailboxManagerActions |= action;
                else account.MailboxManagerActions &= ~action;

                // update account
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

        public static string GetMailboxSetupInstructions(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            #region Demo Mode
            if (IsDemoMode)
            {
                return string.Empty;
            }
            #endregion
 
            // load organization
            Organization org = GetOrganization(itemId);
            if (org == null)
                return null;

            // load user info
            UserInfo user = PackageController.GetPackageOwner(org.PackageId);

            // get letter settings
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.EXCHANGE_MAILBOX_SETUP_LETTER);

            string settingName = user.HtmlMail ? "HtmlBody" : "TextBody";
            string body = settings[settingName];
            if (String.IsNullOrEmpty(body))
                return null;

            string result = EvaluateMailboxTemplate(itemId, accountId, pmm, false, false, body);
            return user.HtmlMail ? result : result.Replace("\n", "<br/>");
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

            // load account
            ExchangeAccount account = GetAccount(itemId, accountId);
            if (account == null)
                return null;

            // add account
            items["Account"] = account;
            items["AccountDomain"] = account.PrimaryEmailAddress.Substring(account.PrimaryEmailAddress.IndexOf("@") + 1);
            items["DefaultDomain"] = org.DefaultDomain;

            if (!String.IsNullOrEmpty(account.SamAccountName))
            {
                int idx = account.SamAccountName.IndexOf("\\");
                items["SamDomain"] = account.SamAccountName.Substring(0, idx);
                items["SamUsername"] = account.SamAccountName.Substring(idx + 1);
            }

            // name servers
            PackageSettings packageSettings = PackageController.GetPackageSettings(org.PackageId, PackageSettings.NAME_SERVERS);
            string[] nameServers = new string[] { };
            if (!String.IsNullOrEmpty(packageSettings["NameServers"]))
                nameServers = packageSettings["NameServers"].Split(';');

            items["NameServers"] = nameServers;

            // service settings
            int exchangeServiceId = GetExchangeServiceID(org.PackageId);
            StringDictionary exchangeSettings = ServerController.GetServiceSettings(exchangeServiceId);
            if (exchangeSettings != null)
            {
                items["TempDomain"] = exchangeSettings["TempDomain"];
                items["AutodiscoverIP"] = exchangeSettings["AutodiscoverIP"];
                items["AutodiscoverDomain"] = exchangeSettings["AutodiscoverDomain"];
                items["OwaUrl"] = exchangeSettings["OwaUrl"];
                items["ActiveSyncServer"] = exchangeSettings["ActiveSyncServer"];
                items["SmtpServers"] = Utils.ParseDelimitedString(exchangeSettings["SmtpServers"], '\n');
            }

            items["Email"] = emailMode;
            items["Signup"] = signup;
            items["PMM"] = pmm;

            // evaluate template
            return PackageController.EvaluateTemplate(template, items);
        }

        public static int SendMailboxSetupInstructions(int itemId, int accountId, bool signup, string to, string cc)
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
            UserSettings settings = UserController.GetUserSettings(user.UserId, UserSettings.EXCHANGE_MAILBOX_SETUP_LETTER);

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


        public static ExchangeMailbox GetMailboxPermissions(int itemId, int accountId)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "GET_MAILBOX_PERMISSIONS");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                // load account
                ExchangeAccount account = GetAccount(itemId, accountId);

                // get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
				ExchangeMailbox mailbox = exchange.GetMailboxPermissions(org.OrganizationId, account.AccountName);
                mailbox.DisplayName = account.DisplayName;
                return mailbox;
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

        public static int SetMailboxPermissions(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts)
        {
            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            if (accountCheck < 0) return accountCheck;

            // place log record
            TaskManager.StartTask("EXCHANGE", "SET_MAILBOX_PERMISSIONS");
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
                ExchangeAccount account = GetAccount(itemId, accountId);

                // get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
                exchange.SetMailboxPermissions(org.OrganizationId, account.AccountName, sendAsaccounts, fullAccessAcounts);
                  

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


	    #endregion

		#region Contacts
		public static int CreateContact(int itemId, string displayName, string email)
		{
            //if (EmailAddressExists(email))
              //  return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;
            

            // check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

            // check mailbox quota
			OrganizationStatistics orgStats = GetOrganizationStatistics(itemId);
			if (orgStats.AllocatedContacts > -1
				&& orgStats.CreatedContacts >= orgStats.AllocatedContacts)
				return BusinessErrorCodes.ERROR_EXCHANGE_CONTACTS_QUOTA_LIMIT;

			// place log record
			TaskManager.StartTask("EXCHANGE", "CREATE_CONTACT");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

				string name = email;
				int idx = email.IndexOf("@");
				if (idx > -1)
					name = email.Substring(0, idx);

				string accountName = BuildAccountName(org.OrganizationId, name);

				// add contact
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                //Create Exchange Organization
                if (string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExtendToExchangeOrganization(ref org);

                    PackageController.UpdatePackageItem(org);
                }
				
                exchange.CreateContact(
					org.OrganizationId,
					org.DistinguishedName,
					displayName,
					accountName,
                    email, org.DefaultDomain);

				// add meta-item
				int accountId = AddAccount(itemId, ExchangeAccountType.Contact, accountName,
					displayName, email, false,
                    0, "", null);

				return accountId;
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

		public static int DeleteContact(int itemId, int accountId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_CONTACT");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// delete contact
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.DeleteContact(account.AccountName);

				// remove meta-item
				DeleteAccount(itemId, accountId);

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

		private static ExchangeContact GetDemoContactSettings()
		{
			ExchangeContact c = new ExchangeContact();
			c.DisplayName = "WebsitePanel Support";
			c.AccountName = "wsp_fabrikam";
			c.FirstName = "WebsitePanel";
			c.LastName = "Support";
			c.EmailAddress = "support@websitepanel.net";
			c.AcceptAccounts = GetAccounts(0, ExchangeAccountType.Mailbox).ToArray();
			return c;
		}

		public static ExchangeContact GetContactGeneralSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoContactSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_CONTACT_GENERAL");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				return exchange.GetContactGeneralSettings(account.AccountName);
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

		public static int SetContactGeneralSettings(int itemId, int accountId, string displayName, string emailAddress,
			bool hideAddressBook, string firstName, string initials,
			string lastName, string address, string city, string state, string zip, string country,
			string jobTitle, string company, string department, string office, string managerAccountName,
			string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, int useMapiRichTextFormat)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_CONTACT_GENERAL");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetContactGeneralSettings(
					account.AccountName,
					displayName,
					emailAddress,
					hideAddressBook,
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
                    useMapiRichTextFormat, org.DefaultDomain);

				// update account
				account.DisplayName = displayName;
				account.PrimaryEmailAddress = emailAddress;
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

		public static ExchangeContact GetContactMailFlowSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoContactSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_CONTACT_MAILFLOW");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				ExchangeContact contact = exchange.GetContactMailFlowSettings(account.AccountName);
				contact.DisplayName = account.DisplayName;
				return contact;
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

		public static int SetContactMailFlowSettings(int itemId, int accountId,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_CONTACT_MAILFLOW");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetContactMailFlowSettings(account.AccountName,
					acceptAccounts,
					rejectAccounts,
					requireSenderAuthentication);

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
		#endregion

		#region Distribution Lists
		public static int CreateDistributionList(int itemId, string displayName, string name, string domain, int managerId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check mailbox quota
			OrganizationStatistics orgStats = GetOrganizationStatistics(itemId);
			if (orgStats.AllocatedDistributionLists > -1
				&& orgStats.CreatedDistributionLists >= orgStats.AllocatedDistributionLists)
				return BusinessErrorCodes.ERROR_EXCHANGE_DLISTS_QUOTA_LIMIT;

			// place log record
			TaskManager.StartTask("EXCHANGE", "CREATE_DISTR_LIST");
			TaskManager.ItemId = itemId;

			try
			{
				// e-mail
				string email = name + "@" + domain;

				// check e-mail
				if (EmailAddressExists(email))
					return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

				// load organization
				Organization org = GetOrganization(itemId);

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

				string accountName = BuildAccountName(org.OrganizationId, name);

				// add account
				// add contact
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                //Create Exchange Organization
                if (string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExtendToExchangeOrganization(ref org);

                    PackageController.UpdatePackageItem(org);
                }

			    OrganizationUser manager = OrganizationController.GetAccount(itemId, managerId);               
				exchange.CreateDistributionList(
					org.OrganizationId,
					org.DistinguishedName,
					displayName,
					accountName,
					name,
					domain, manager.AccountName);

				// add meta-item
				int accountId = AddAccount(itemId, ExchangeAccountType.DistributionList, accountName,
					displayName, email, false,
                    0, "", null);

				// register email address
				AddAccountEmailAddress(accountId, email);

				return accountId;
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

		public static int DeleteDistributionList(int itemId, int accountId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_DISTR_LIST");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// delete mailbox
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.DeleteDistributionList(account.AccountName);

				// unregister account
				DeleteAccount(itemId, accountId);

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

		private static ExchangeDistributionList GetDemoDistributionListSettings()
		{
			ExchangeDistributionList c = new ExchangeDistributionList();
			c.DisplayName = "Fabrikam Sales";
			c.AccountName = "sales_fabrikam";
			c.ManagerAccount = GetAccounts(0, ExchangeAccountType.Mailbox)[0];
			c.MembersAccounts = GetAccounts(0, ExchangeAccountType.Mailbox).ToArray();
			c.AcceptAccounts = GetAccounts(0, ExchangeAccountType.Mailbox).ToArray();
			return c;
		}

		public static ExchangeDistributionList GetDistributionListGeneralSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoDistributionListSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_DISTR_LIST_GENERAL");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				return exchange.GetDistributionListGeneralSettings(account.AccountName);
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

		public static int SetDistributionListGeneralSettings(int itemId, int accountId, string displayName,
			bool hideAddressBook, string managerAccount, string[] memberAccounts,
			string notes)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_DISTR_LIST_GENERAL");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetDistributionListGeneralSettings(
					account.AccountName,
					displayName,
					hideAddressBook,
					managerAccount,
					memberAccounts,
					notes);

				// update account
				account.DisplayName = displayName;
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

		public static ExchangeDistributionList GetDistributionListMailFlowSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoDistributionListSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_DISTR_LIST_MAILFLOW");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				ExchangeDistributionList list = exchange.GetDistributionListMailFlowSettings(account.AccountName);
				list.DisplayName = account.DisplayName;
				return list;
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

		public static int SetDistributionListMailFlowSettings(int itemId, int accountId,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_DISTR_LIST_MAILFLOW");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetDistributionListMailFlowSettings(account.AccountName,
					acceptAccounts,
					rejectAccounts,
					requireSenderAuthentication);

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

		public static ExchangeEmailAddress[] GetDistributionListEmailAddresses(int itemId, int accountId)
		{
			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_DISTR_LIST_ADDRESSES");
			TaskManager.ItemId = itemId;

			try
			{
				return GetAccountEmailAddresses(itemId, accountId);
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

		public static int AddDistributionListEmailAddress(int itemId, int accountId, string emailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "ADD_DISTR_LIST_ADDRESS");
			TaskManager.ItemId = itemId;

			try
			{
				// check
				if (EmailAddressExists(emailAddress))
					return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// add e-mail
				AddAccountEmailAddress(accountId, emailAddress);

				// update e-mail addresses
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetDistributionListEmailAddresses(
					account.AccountName,
					GetAccountSimpleEmailAddresses(itemId, accountId));

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

		public static int SetDistributionListPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "SET_PRIMARY_DISTR_LIST_ADDRESS");
			TaskManager.ItemId = itemId;

			try
			{
				// get account
				ExchangeAccount account = GetAccount(itemId, accountId);
				account.PrimaryEmailAddress = emailAddress;

				// update exchange
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetDistributionListPrimaryEmailAddress(
					account.AccountName,
					emailAddress);

				// save account
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

		public static int DeleteDistributionListEmailAddresses(int itemId, int accountId, string[] emailAddresses)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_DISTR_LIST_ADDRESSES");
			TaskManager.ItemId = itemId;

			try
			{
				// get account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// delete e-mail addresses
				List<string> toDelete = new List<string>();
				foreach (string emailAddress in emailAddresses)
				{
					if (String.Compare(account.PrimaryEmailAddress, emailAddress, true) != 0)
						toDelete.Add(emailAddress);
				}

				// delete from meta-base
				DeleteAccountEmailAddresses(accountId, toDelete.ToArray());

				// delete from Exchange
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// update e-mail addresses
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetDistributionListEmailAddresses(
					account.AccountName,
					GetAccountSimpleEmailAddresses(itemId, accountId));

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


        public static ResultObject SetDistributionListPermissions(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            ResultObject res = TaskManager.StartResultTask<ResultObject>("EXCHANGE", "SET_DISTRIBUTION_LIST_PERMISSINS");
            Organization org;
            try
            {
                org = GetOrganization(itemId);
                if (org == null)
                    throw new ApplicationException("Organization is null");
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);
                return res;
            }

            ExchangeServer exchange;
            try
            {

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_PROXY, ex);
                return res;
            }

            ExchangeAccount account;

            try
            {
                account = GetAccount(itemId, accountId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                exchange.SetDistributionListPermissions(org.OrganizationId, account.AccountName, sendAsAccounts,
                                                        sendOnBehalfAccounts);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_SET_DISTRIBUTION_LIST_PERMISSIONS, ex);
                return res;
            }

            TaskManager.CompleteTask();
            return res;
        }

        public static ExchangeDistributionListResult  GetDistributionListPermissions(int itemId, int accountId)
        {
            Organization org;
            ExchangeDistributionListResult res = TaskManager.StartResultTask<ExchangeDistributionListResult>("EXCHANGE", "GET_DISTRIBUTION_LIST_RESULT");
            
            try
            {
                org = GetOrganization(itemId);
                if (org == null)
                    throw new ApplicationException("Organization is null");
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_BY_ITEM_ID, ex);                
                return res;
            }

            ExchangeServer exchange;
            try
            {
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ORGANIZATION_PROXY, ex);
                return res;
            }
            
            ExchangeAccount account;
            try
            {
                account = GetAccount(itemId, accountId);
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_ACCOUNT, ex);
                return res;
            }

            try
            {
                res.Value = exchange.GetDistributionListPermissions(org.OrganizationId, account.AccountName);
                res.Value.DisplayName = account.DisplayName;
            }
            catch(Exception ex)
            {
                TaskManager.CompleteResultTask(res, ErrorCodes.CANNOT_GET_DISTRIBUTION_LIST_PERMISSIONS, ex);
                return res;
            }
            
            TaskManager.CompleteTask();
            return res;
        }

		#endregion

		#region Public Folders
		public static int CreatePublicFolder(int itemId, string parentFolder, string folderName,
			bool mailEnabled, string name, string domain)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// check mailbox quota
			OrganizationStatistics orgStats = GetOrganizationStatistics(itemId);
			if (orgStats.AllocatedPublicFolders > -1
				&& orgStats.CreatedPublicFolders >= orgStats.AllocatedPublicFolders)
				return BusinessErrorCodes.ERROR_EXCHANGE_PFOLDERS_QUOTA_LIMIT;

			// place log record
			TaskManager.StartTask("EXCHANGE", "CREATE_PUBLIC_FOLDER");
			TaskManager.ItemId = itemId;

			try
			{
				// e-mail
				string email = "";
				if (mailEnabled && !String.IsNullOrEmpty(name))
				{
					email = name + "@" + domain;

					// check e-mail
					if (EmailAddressExists(email))
						return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;
				}

				// full folder name
				string normParent = parentFolder;
				if (!normParent.StartsWith("\\"))
					normParent = "\\" + normParent;
				if (!normParent.EndsWith("\\"))
					normParent = normParent + "\\";

				string folderPath = normParent + folderName;

				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

				if (String.IsNullOrEmpty(name))
					name = Utils.CleanIdentifier(folderName);

				string accountName = BuildAccountName(org.OrganizationId, name);

				// add mailbox
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                //Create Exchange Organization
                if (string.IsNullOrEmpty(org.GlobalAddressList))
                {
                    ExtendToExchangeOrganization(ref org);

                    PackageController.UpdatePackageItem(org);
                }

				exchange.CreatePublicFolder(
					org.OrganizationId,
					org.SecurityGroup,
					parentFolder,
					folderName,
					mailEnabled,
					accountName,
					name,
					domain);

				// add meta-item
				int accountId = AddAccount(itemId, ExchangeAccountType.PublicFolder, accountName,
					folderPath, email, mailEnabled,
                    0, "", null);

				// register email address
				if(mailEnabled)
					AddAccountEmailAddress(accountId, email);

				return accountId;
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

		public static int DeletePublicFolders(int itemId, int[] accountIds)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			if (accountIds != null)
				foreach (int accountId in accountIds)
				{
					int result = DeletePublicFolder(itemId, accountId);
					if (result < 0)
						return result;
				}
			return 0;
		}

		public static int DeletePublicFolder(int itemId, int accountId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_PUBLIC_FOLDER");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// delete folder
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.DeletePublicFolder(account.DisplayName);

				// unregister account
				DeleteAccount(itemId, accountId);

				// delete all nested folder meta-items
				List<ExchangeAccount> folders = GetAccounts(itemId, ExchangeAccountType.PublicFolder);
				foreach (ExchangeAccount folder in folders)
				{
					if (folder.DisplayName.ToLower().StartsWith(account.DisplayName.ToLower() + "\\"))
						DeleteAccount(itemId, folder.AccountId);
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

		public static int EnableMailPublicFolder(int itemId, int accountId,
			string name, string domain)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "ENABLE_MAIL_PUBLIC_FOLDER");
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
				ExchangeAccount account = GetAccount(itemId, accountId);
				if (account.MailEnabledPublicFolder)
					return 0;

				// check email
				string email = name + "@" + domain;

				// check e-mail
				if (EmailAddressExists(email))
					return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

				string accountName = BuildAccountName(org.OrganizationId, name);

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.EnableMailPublicFolder(
					org.OrganizationId,
					account.DisplayName,
					account.AccountName,
					name,
					domain);

				// update and save account
				account.AccountName = accountName;
				account.MailEnabledPublicFolder = true;
				account.PrimaryEmailAddress = email;
				UpdateAccount(account);

				// register e-mail
				AddAccountEmailAddress(accountId, email);

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

		public static int DisableMailPublicFolder(int itemId, int accountId)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DISABLE_MAIL_PUBLIC_FOLDER");
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
				ExchangeAccount account = GetAccount(itemId, accountId);
				if (!account.MailEnabledPublicFolder)
					return 0;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.DisableMailPublicFolder(account.DisplayName);


				// update and save account
				account.MailEnabledPublicFolder = false;
				account.PrimaryEmailAddress = "";
				UpdateAccount(account);


				// delete all mail accounts
				List<string> addrs = new List<string>();
				ExchangeEmailAddress[] emails = GetAccountEmailAddresses(itemId, accountId);
				foreach (ExchangeEmailAddress email in emails)
					addrs.Add(email.EmailAddress);

				DeleteAccountEmailAddresses(accountId, addrs.ToArray());

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

		private static ExchangePublicFolder GetDemoPublicFolderSettings()
		{
			ExchangePublicFolder c = new ExchangePublicFolder();
			c.DisplayName = "\\fabrikam\\Documents";
			c.MailEnabled = true;
			c.Name = "Documents";
			c.AuthorsAccounts = GetAccounts(0, ExchangeAccountType.Mailbox).ToArray();
			c.AcceptAccounts = GetAccounts(0, ExchangeAccountType.Mailbox).ToArray();
			return c;
		}

		public static ExchangePublicFolder GetPublicFolderGeneralSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoPublicFolderSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_PUBLIC_FOLDER_GENERAL");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				ExchangePublicFolder folder = exchange.GetPublicFolderGeneralSettings(account.DisplayName);
				folder.MailEnabled = account.MailEnabledPublicFolder;
				folder.DisplayName = account.DisplayName;
				return folder;
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

		public static int SetPublicFolderGeneralSettings(int itemId, int accountId, string newName,
			bool hideAddressBook, string[] authorAccounts)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_PUBLIC_FOLDER_GENERAL");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetPublicFolderGeneralSettings(
					account.DisplayName,
					newName,
					authorAccounts,
					hideAddressBook);

				// update folder name
				string origName = account.DisplayName;
				string newFullName = origName.Substring(0, origName.LastIndexOf("\\") + 1) + newName;

				if (String.Compare(origName, newFullName, true) != 0)
				{
					// rename original folder
					account.DisplayName = newFullName;
					UpdateAccount(account);

					// rename nested folders
					List<ExchangeAccount> folders = GetAccounts(itemId, ExchangeAccountType.PublicFolder);
					foreach (ExchangeAccount folder in folders)
					{
						if (folder.DisplayName.ToLower().StartsWith(origName.ToLower() + "\\"))
						{
							folder.DisplayName = newFullName + folder.DisplayName.Substring(origName.Length);
							UpdateAccount(folder);
						}
					}
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

		public static ExchangePublicFolder GetPublicFolderMailFlowSettings(int itemId, int accountId)
		{
			#region Demo Mode
			if (IsDemoMode)
			{
				return GetDemoPublicFolderSettings();
			}
			#endregion

			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_PUBLIC_FOLDER_MAILFLOW");
			TaskManager.ItemId = itemId;

			try
			{
				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return null;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				ExchangePublicFolder folder = exchange.GetPublicFolderMailFlowSettings(account.DisplayName);
				folder.DisplayName = account.DisplayName;
				return folder;
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

		public static int SetPublicFolderMailFlowSettings(int itemId, int accountId,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "UPDATE_PUBLIC_FOLDER_MAILFLOW");
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
				ExchangeAccount account = GetAccount(itemId, accountId);

				// get mailbox settings
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetPublicFolderMailFlowSettings(account.DisplayName,
					acceptAccounts,
					rejectAccounts,
					requireSenderAuthentication);

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

		public static ExchangeEmailAddress[] GetPublicFolderEmailAddresses(int itemId, int accountId)
		{
			// place log record
			TaskManager.StartTask("EXCHANGE", "GET_PUBLIC_FOLDER_ADDRESSES");
			TaskManager.ItemId = itemId;

			try
			{
				return GetAccountEmailAddresses(itemId, accountId);
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

		public static int AddPublicFolderEmailAddress(int itemId, int accountId, string emailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "ADD_PUBLIC_FOLDER_ADDRESS");
			TaskManager.ItemId = itemId;

			try
			{
				// check
				if (EmailAddressExists(emailAddress))
					return BusinessErrorCodes.ERROR_EXCHANGE_EMAIL_EXISTS;

				// load organization
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

				// load account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// add e-mail
				AddAccountEmailAddress(accountId, emailAddress);

				// update e-mail addresses
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetPublicFolderEmailAddresses(
					account.DisplayName,
					GetAccountSimpleEmailAddresses(itemId, accountId));

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

		public static int SetPublicFolderPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "SET_PRIMARY_PUBLIC_FOLDER_ADDRESS");
			TaskManager.ItemId = itemId;

			try
			{
				// get account
				ExchangeAccount account = GetAccount(itemId, accountId);
				account.PrimaryEmailAddress = emailAddress;

				// update exchange
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// check package
				int packageCheck = SecurityContext.CheckPackage(org.PackageId, DemandPackage.IsActive);
				if (packageCheck < 0) return packageCheck;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetPublicFolderPrimaryEmailAddress(
					account.DisplayName,
					emailAddress);

				// save account
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

		public static int DeletePublicFolderEmailAddresses(int itemId, int accountId, string[] emailAddresses)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			// place log record
			TaskManager.StartTask("EXCHANGE", "DELETE_PUBLIC_FOLDER_ADDRESSES");
			TaskManager.ItemId = itemId;

			try
			{
				// get account
				ExchangeAccount account = GetAccount(itemId, accountId);

				// delete e-mail addresses
				List<string> toDelete = new List<string>();
				foreach (string emailAddress in emailAddresses)
				{
					if (String.Compare(account.PrimaryEmailAddress, emailAddress, true) != 0)
						toDelete.Add(emailAddress);
				}

				// delete from meta-base
				DeleteAccountEmailAddresses(accountId, toDelete.ToArray());

				// delete from Exchange
				Organization org = GetOrganization(itemId);
				if (org == null)
					return -1;

				// update e-mail addresses
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);
                
				exchange.SetPublicFolderEmailAddresses(
					account.DisplayName,
					GetAccountSimpleEmailAddresses(itemId, accountId));

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
		#endregion

		#region Private Helpers


        private static string GetPrimaryDomainController(int organizationServiceId)
        {
            
            Organizations orgProxy = new Organizations();

            ServiceProviderProxy.Init(orgProxy, organizationServiceId);

            string[] organizationSettings = orgProxy.ServiceProviderSettingsSoapHeaderValue.Settings;



            string orgPrimaryDomainController = string.Empty;
            foreach (string str in organizationSettings)
            {
                string[] props = str.Split('=');
                if (props[0].ToLower() == "primarydomaincontroller")
                {
                    orgPrimaryDomainController = str;
                    break;
                }
            }

            return orgPrimaryDomainController;
        }

        private static void ExtendExchangeSettings(List<string> exchangeSettings, string primaryDomainController)
        {
            bool isAdded = false;
            for (int i = 0; i < exchangeSettings.Count; i++)
            {
                string[] props = exchangeSettings[i].Split('=');
                if (props[0].ToLower() == "primarydomaincontroller")
                {
                    exchangeSettings[i] = primaryDomainController;
                    isAdded = true;
                    break;
                }
            }

            if (!isAdded)
            {
                exchangeSettings.Add(primaryDomainController);
            }
        }

        internal static ServiceProvider GetServiceProvider(int exchangeServiceId, int organizationServiceId)
        {
            ServiceProvider ws = new ServiceProvider();

            ServiceProviderProxy.Init(ws, exchangeServiceId);           

            string[] exchangeSettings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(exchangeSettings);

            string orgPrimaryDomainController = GetPrimaryDomainController(organizationServiceId);
            
            ExtendExchangeSettings(resSettings, orgPrimaryDomainController);
            ws.ServiceProviderSettingsSoapHeaderValue.Settings = resSettings.ToArray();
            return ws;
        }
        
        internal static ExchangeServer GetExchangeServer(int exchangeServiceId, int organizationServiceId)
		{
			ExchangeServer ws = new ExchangeServer();
			
            ServiceProviderProxy.Init(ws, exchangeServiceId);                       

            string []exchangeSettings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(exchangeSettings);

            string orgPrimaryDomainController = GetPrimaryDomainController(organizationServiceId);
           
            ExtendExchangeSettings(resSettings, orgPrimaryDomainController);
            ws.ServiceProviderSettingsSoapHeaderValue.Settings = resSettings.ToArray();
            return ws;
		}

        internal static ServiceProvider GetExchangeServiceProvider(int exchangeServiceId, int organizationServiceId)
        {
            ServiceProvider ws = new ServiceProvider();

            ServiceProviderProxy.Init(ws, exchangeServiceId);

            string[] exchangeSettings = ws.ServiceProviderSettingsSoapHeaderValue.Settings;

            List<string> resSettings = new List<string>(exchangeSettings);

            string orgPrimaryDomainController = GetPrimaryDomainController(organizationServiceId);

            ExtendExchangeSettings(resSettings, orgPrimaryDomainController);
            ws.ServiceProviderSettingsSoapHeaderValue.Settings = resSettings.ToArray();
            return ws;
        }   


		private static int GetExchangeServiceID(int packageId)
		{
			return PackageController.GetPackageServiceId(packageId, ResourceGroups.Exchange);
		}

		private static string[] GetAccountSimpleEmailAddresses(int itemId, int accountId)
		{
			ExchangeEmailAddress[] emails = GetAccountEmailAddresses(itemId, accountId);
			List<string> result = new List<string>();
			foreach (ExchangeEmailAddress email in emails)
			{
				string prefix = email.IsPrimary ? "SMTP:" : "smtp:";
				result.Add(prefix + email.EmailAddress);
			}

			return result.ToArray();
		}

		private static bool QuotaEnabled(PackageContext cntx, string quotaName)
		{
			return cntx.Quotas.ContainsKey(quotaName) && !cntx.Quotas[quotaName].QuotaExhausted;
		}

		private static bool IsDemoMode
		{
			get
			{
				return (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);
			}
		}
		#endregion

        public static ExchangeMobileDevice[] GetMobileDevices(int itemId, int accountId)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "GET_MOBILE_DEVICES");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                // load account
                ExchangeAccount account = GetAccount(itemId, accountId);

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                return exchange.GetMobileDevices(account.AccountName);
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

        public static ExchangeMobileDevice GetMobileDevice(int itemId, string deviceId)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "GET_MOBILE_DEVICE");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return null;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                return exchange.GetMobileDevice(deviceId);
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

        public static void WipeDataFromDevice(int itemId, string deviceId)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "WIPE_DATA_FROM_DEVICE");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                exchange.WipeDataFromDevice(deviceId);
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

        public static void CancelRemoteWipeRequest(int itemId, string deviceId)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "CANCEL_REMOTE_WIPE_REQUEST");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                exchange.CancelRemoteWipeRequest(deviceId);
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

        public static void RemoveDevice(int itemId, string deviceId)
        {
            // place log record
            TaskManager.StartTask("EXCHANGE", "REMOVE_DEVICE");
            TaskManager.ItemId = itemId;

            try
            {
                // load organization
                Organization org = GetOrganization(itemId);
                if (org == null)
                    return;

                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                ExchangeServer exchange = GetExchangeServer(exchangeServiceId, org.ServiceId);

                exchange.RemoveDevice(deviceId);
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
	}
}
