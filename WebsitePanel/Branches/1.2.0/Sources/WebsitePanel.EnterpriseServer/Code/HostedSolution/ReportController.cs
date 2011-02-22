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

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WebsitePanel.EnterpriseServer.Code.SharePoint;
using WebsitePanel.Providers.CRM;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.SharePoint;

namespace WebsitePanel.EnterpriseServer.Code.HostedSolution
{
    public class ReportController
    {
        private static void PopulateOrganizationStatisticsReport(Organization org, EnterpriseSolutionStatisticsReport report, string topReseller)
        {
            OrganizationStatisticsRepotItem item = new OrganizationStatisticsRepotItem();
            PopulateBaseItem(item, org, topReseller);                        
            
            if (report.ExchangeReport != null)
            {
                try
                {
                    List<ExchangeMailboxStatistics> mailboxStats =
                        report.ExchangeReport.Items.FindAll(
                            delegate(ExchangeMailboxStatistics stats)
                                { return stats.OrganizationID == org.OrganizationId; });

                    item.TotalMailboxes = mailboxStats.Count;
                    foreach (ExchangeMailboxStatistics current in mailboxStats)
                    {
                        item.TotalMailboxesSize += current.TotalSize;
                    }

                    Providers.Exchange.ExchangeServer exchange;
                    if (!string.IsNullOrEmpty(org.GlobalAddressList))
                    {
                        try
                        {

                            int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                            exchange =
                                ExchangeServerController.GetExchangeServer(exchangeServiceId, org.ServiceId);
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(
                                string.Format("Could not get exchange server. PackageId: {0}", org.PackageId), ex);
                        }

                        try
                        {
                            item.TotalPublicFoldersSize = exchange.GetPublicFolderSize("\\" + org.OrganizationId);
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(
                                string.Format("Could not get public folder size. OrgId: {0}", org.OrganizationId), ex);
                        }
                    }
                    
                    try
                    {
                        org.DiskSpace = (int)(item.TotalPublicFoldersSize + item.TotalMailboxesSize);
                        PackageController.UpdatePackageItem(org);
                    }
                    catch(Exception ex)
                    {
                        throw new ApplicationException(string.Format("Could not calcualate diskspace. Org Id: {0}", org.Id), ex);
                    }
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
            
            if (report.SharePointReport != null)
            {
                List<SharePointStatistics> sharePoints =
                        report.SharePointReport.Items.FindAll(
                            delegate(SharePointStatistics stats) { return stats.OrganizationID == org.OrganizationId; });

                    item.TotalSharePointSiteCollections = sharePoints.Count;
                    foreach (SharePointStatistics current in sharePoints)
                    {
                        item.TotalSharePointSiteCollectionsSize += current.SiteCollectionSize;
                    }                
            }

            if (report.CRMReport != null)
            {
                List<CRMOrganizationStatistics> crmOrganizationStatistics =
                    report.CRMReport.Items.FindAll(
                        delegate(CRMOrganizationStatistics stats) { return stats.OrganizationID == org.OrganizationId; });

                item.TotalCRMUsers = crmOrganizationStatistics.Count;                
            }            
            
            report.OrganizationReport.Items.Add(item);
        }


        private static HostedSharePointServer GetHostedSharePointServer(int serviceId)
        {
            HostedSharePointServer sps = new HostedSharePointServer();
            ServiceProviderProxy.Init(sps, serviceId);
            return sps;
        }

        private static int GetHostedSharePointServiceId(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedSharePoint);
        }

        
        private static void PopulateBaseItem(BaseStatistics stats, Organization org, string topReseller)
        {
            PackageInfo package;
            UserInfo user;

            try
            {
                package = PackageController.GetPackage(org.PackageId);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(string.Format("Could not get package {0}", org.PackageId), ex);
            }
            
            
            try
            {
                user = UserController.GetUser(package.UserId);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(string.Format("Could not get user {0}", package.UserId), ex);
            }
            
            stats.HostingSpace = package.PackageName;                        
            stats.OrganizationID = org.OrganizationId;
            stats.OrganizationName = org.Name;

            stats.CustomerName = UserController.GetUser(package.UserId).Username;
            stats.CustomerCreated = user.Created;
            stats.ResellerName = UserController.GetUser(user.OwnerId).Username;
            stats.TopResellerName = topReseller;

            stats.OrganizationCreated = org.CreatedDate;
            stats.HostingSpaceCreated = package.PurchaseDate;
            
        }


        private static int GetCRMServiceId(int packageId)
        {
            int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.HostedCRM);
            return serviceId;
        }

        private static CRM GetCRMProxy(int packageId)
        {
            int crmServiceId = GetCRMServiceId(packageId);
            CRM ws = new CRM();
            ServiceProviderProxy.Init(ws, crmServiceId);
            return ws;
        }

        private static void PopulateCRMReportItems(Organization org, EnterpriseSolutionStatisticsReport report, string topReseller)
        {
            if (org.CrmOrganizationId == Guid.Empty)
                return;

            List<OrganizationUser> users;

            try
            {
                users = CRMController.GetCRMOrganizationUsers(org.Id);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(
                    string.Format("Could not get CRM Organization users. OrgId : {0}", org.Id), ex);
            }
            
            CRM crm;
            try
            {
                crm = GetCRMProxy(org.PackageId);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(string.Format("Could not get CRM Proxy. PackageId: {0}", org.PackageId),
                                               ex);
            }
            
            foreach (OrganizationUser user in users)
            {
                try
                {
                    CRMOrganizationStatistics stats = new CRMOrganizationStatistics();

                    PopulateBaseItem(stats, org, topReseller);

                    stats.CRMOrganizationId = org.CrmOrganizationId;
                    stats.CRMUserName = user.DisplayName;

                    Guid crmUserId = CRMController.GetCrmUserId(user.AccountId);
                    CrmUserResult res = crm.GetCrmUserById(crmUserId, org.OrganizationId);
					if (res.IsSuccess && res.Value != null)
					{
						stats.ClientAccessMode = res.Value.ClientAccessMode;
						stats.CRMDisabled = res.Value.IsDisabled;
					}
					else
					{
						StringBuilder sb = new StringBuilder("Could not get CRM user by id.");
						foreach (string str in res.ErrorCodes)
						{
							sb.AppendFormat("\n{0};", str);
						}
						throw new ApplicationException(sb.ToString());
					}
                                         
                    report.CRMReport.Items.Add(stats);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }

        private static void PopulateOrganizationData(Organization org, EnterpriseSolutionStatisticsReport report, string topReseller)
        {            
            if (report.ExchangeReport != null)
            {
                try
                {
                    PopulateExchangeReportItems(org, report, topReseller);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }

            if (report.CRMReport != null)
            {
                try
                {
                    PopulateCRMReportItems(org, report, topReseller);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }

            if (report.SharePointReport != null)
            {
                try
                {
                    PopulateSharePointItem(org, report, topReseller);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }            
            
            if (report.OrganizationReport != null)
            {
                try
                {
                    PopulateOrganizationStatisticsReport(org, report, topReseller);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }                                    
        }

        private static int GetExchangeServiceID(int packageId)
        {
            return PackageController.GetPackageServiceId(packageId, ResourceGroups.Exchange);
        }



        private static void PopulateSharePointItem(Organization org, EnterpriseSolutionStatisticsReport report, string topReseller)
        {
            List<SharePointSiteCollection> siteCollections;
            
            try
            {
                siteCollections = HostedSharePointServerController.GetSiteCollections(org.Id);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(string.Format("Could not get site collections. OrgId: {0}", org.Id), ex);
            }
            
            if (siteCollections == null || siteCollections.Count == 0)
                return;

            HostedSharePointServer srv;
            try
            {
                int serviceId = GetHostedSharePointServiceId(org.PackageId);
                srv = GetHostedSharePointServer(serviceId);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(
                    string.Format("Could not get sharepoint server. PackageId: {0}", org.PackageId), ex);
            }

            foreach (SharePointSiteCollection siteCollection in siteCollections)
            {
                try
                {
                    SharePointStatistics stats = new SharePointStatistics();
                    PopulateBaseItem(stats, org, topReseller);

                    stats.SiteCollectionUrl = siteCollection.PhysicalAddress;
                    stats.SiteCollectionOwner = siteCollection.OwnerName;
                    stats.SiteCollectionQuota = siteCollection.MaxSiteStorage;

                    stats.SiteCollectionCreated = siteCollection.CreatedDate;

                    stats.SiteCollectionSize = srv.GetSiteCollectionSize(siteCollection.PhysicalAddress);

                    report.SharePointReport.Items.Add(stats);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }


        private static void PopulateExchangeReportItems(Organization org, EnterpriseSolutionStatisticsReport report, string topReseller)
        {
            //Check if exchange organization
            if (string.IsNullOrEmpty(org.GlobalAddressList))
                return;

            List<ExchangeAccount> mailboxes;
            Providers.Exchange.ExchangeServer exchange;
            try
            {
                mailboxes = ExchangeServerController.GetExchangeMailboxes(org.Id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("Could not get mailboxes for current organization {0}", org.Id), ex);
            }

            try
            {
                int exchangeServiceId = GetExchangeServiceID(org.PackageId);
                exchange = ExchangeServerController.GetExchangeServer(exchangeServiceId, org.ServiceId);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(
                    string.Format("Could not get exchange server. PackageId: {0}", org.PackageId), ex);
            }

            ExchangeMailboxStatistics stats;
            foreach (ExchangeAccount mailbox in mailboxes)
            {
                try
                {
                    stats = null;
                    try
                    {

                        stats = exchange.GetMailboxStatistics(mailbox.AccountName);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteError(ex, "Could not get mailbox statistics. AccountName: {0}",
                                               mailbox.AccountName);
                    }

                    
                    if (stats != null)
                    {
                        PopulateBaseItem(stats, org, topReseller);
                        stats.MailboxType = mailbox.AccountType;

                        stats.BlackberryEnabled = BlackBerryController.CheckBlackBerryUserExists(mailbox.AccountId);
                        report.ExchangeReport.Items.Add(stats);
                    }
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
            
        }


        private static void PopulateSpaceData(int packageId, EnterpriseSolutionStatisticsReport report, string topReseller)
        {
            List<Organization> organizations;
            
            try
            {
                organizations = OrganizationController.GetOrganizations(packageId, false);

            }
            catch(Exception ex)
            {
                throw new ApplicationException(string.Format("Cannot get organizations in current package {0}", packageId), ex);
            }
            
            foreach(Organization org in organizations)
            {
                try
                {
                    PopulateOrganizationData(org, report, topReseller);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
        }
        
        private static void PopulateUserData(UserInfo user, EnterpriseSolutionStatisticsReport report, string topReseller)
        {
            DataSet ds;
            try
            {
                ds = PackageController.GetRawMyPackages(user.UserId);
            }
            catch(Exception ex)
            {
                throw new ApplicationException(string.Format("Cannot get user's spaces {0}", user.UserId), ex);
            }
                        
            foreach(DataRow row in  ds.Tables[0].Rows)
            {
                int packageId = (int) row["PackageID"];
                try
                {
                    PopulateSpaceData(packageId, report, topReseller);
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }
            
        }
        
        private static void GetUsersData(EnterpriseSolutionStatisticsReport report, int  userId, bool generateExchangeReport, bool generateSharePointReport, bool generateCRMReport, bool generateOrganizationReport, string topReseller)
        {
            List<UserInfo> users;
            try
            {
                users = UserController.GetUsers(userId, false);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Cannot get users for report", ex);
            }

            
            foreach (UserInfo user in users)
            {
                try
                {
                    PopulateUserData(user, report, topReseller);

                    if (user.Role == UserRole.Reseller || user.Role == UserRole.Administrator)
                    {
                        GetUsersData(report, user.UserId, generateExchangeReport, generateSharePointReport,
                                     generateCRMReport,
                                     generateOrganizationReport,
                                     string.IsNullOrEmpty(topReseller) ? user.Username : topReseller);
                    }
                }
                catch(Exception ex)
                {
                    TaskManager.WriteError(ex);
                }
            }            
        }
        
        public static EnterpriseSolutionStatisticsReport GetEnterpriseSolutionStatisticsReport(int userId, bool generateExchangeReport, bool generateSharePointReport, bool generateCRMReport, bool generateOrganizationReport)
        {
            EnterpriseSolutionStatisticsReport report = new EnterpriseSolutionStatisticsReport();
            
            if (generateExchangeReport || generateOrganizationReport)
                report.ExchangeReport = new ExchangeStatisticsReport();

            if (generateSharePointReport || generateOrganizationReport)
                report.SharePointReport = new SharePointStatisticsReport();

            if (generateCRMReport || generateOrganizationReport)
                report.CRMReport = new CRMStatisticsReport();

            if (generateOrganizationReport)
                report.OrganizationReport = new OrganizationStatisticsReport();


            try
            {
                GetUsersData(report, userId, generateExchangeReport, generateSharePointReport, generateCRMReport,
                             generateOrganizationReport, null);
            }
            catch(Exception ex)
            {
                TaskManager.WriteError(ex, "Cannot get enterprise solution statistics report");
            }

            return report;
        }
        
    }

}
