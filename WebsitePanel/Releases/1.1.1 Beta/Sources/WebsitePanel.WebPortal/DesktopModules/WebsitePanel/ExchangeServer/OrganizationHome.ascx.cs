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
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.HostedSolution;

namespace WebsitePanel.Portal.ExchangeServer
{
	public partial class OrganizationHome : WebsitePanelModuleBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindOrgStats();
			}
		}

        private void BindExchangeStats()
        {
            OrganizationStatistics exchangeOrgStats = ES.Services.ExchangeServer.GetOrganizationStatistics(PanelRequest.ItemID);
            lnkMailboxes.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "mailboxes",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkContacts.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "contacts",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkLists.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "dlists",
            "SpaceID=" + PanelSecurity.PackageId.ToString());

            lnkFolders.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "public_folders",
            "SpaceID=" + PanelSecurity.PackageId.ToString());


            
            
            

            mailboxesStats.QuotaUsedValue = exchangeOrgStats.CreatedMailboxes;
            mailboxesStats.QuotaValue = exchangeOrgStats.AllocatedMailboxes;

            contactsStats.QuotaUsedValue = exchangeOrgStats.CreatedContacts;
            contactsStats.QuotaValue = exchangeOrgStats.AllocatedContacts;

            listsStats.QuotaUsedValue = exchangeOrgStats.CreatedDistributionLists;
            listsStats.QuotaValue = exchangeOrgStats.AllocatedDistributionLists;


            foldersStats.QuotaUsedValue = exchangeOrgStats.CreatedPublicFolders;
            foldersStats.QuotaValue = exchangeOrgStats.AllocatedPublicFolders;

            
            lnkUsedExchangeDiskSpace.Text = exchangeOrgStats.UsedDiskSpace.ToString();
            lnkUsedExchangeDiskSpace.NavigateUrl
                = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "storage_usage",
                "SpaceID=" + PanelSecurity.PackageId.ToString());
            
            
        
        }
        
        private void BindOrgStats()
		{
            Organization org =  ES.Services.Organizations.GetOrganization(PanelRequest.ItemID);


            lblOrganizationNameValue.Text = org.Name;
            lblOrganizationIDValue.Text = org.OrganizationId;
            lblCreatedValue.Text = org.CreatedDate.Date.ToShortDateString();

            
            OrganizationStatistics orgStats = ES.Services.Organizations.GetOrganizationStatistics(PanelRequest.ItemID);
			if (orgStats == null)
				return;


            litTotalDiskSpaceValue.Text = orgStats.UsedDiskSpace.ToString();

            domainStats.QuotaUsedValue = orgStats.CreatedDomains;
            domainStats.QuotaValue = orgStats.AllocatedDomains;

            userStats.QuotaUsedValue = orgStats.CreatedUsers;
            userStats.QuotaValue = orgStats.AllocatedUsers;
            
            
            
            lnkDomains.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "domains",
                "SpaceID=" + PanelSecurity.PackageId);

            lnkUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "users",
                "SpaceID=" + PanelSecurity.PackageId);

            
            
            //If Organization is ExchangeOrganization show exchage specific statistics
            if (!string.IsNullOrEmpty(org.GlobalAddressList))
            {
                exchangeStatsPanel.Visible = true;
                BindExchangeStats();

            }
            else
                exchangeStatsPanel.Visible = false;
                                    

            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);            
            //Show SharePoint statistics
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedSharePoint))
            {
                sharePointStatsPanel.Visible = true;
				
                lnkSiteCollections.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "sharepoint_sitecollections",
                "SpaceID=" + PanelSecurity.PackageId);
                siteCollectionsStats.QuotaUsedValue = orgStats.CreatedSharePointSiteCollections;
                siteCollectionsStats.QuotaValue = orgStats.AllocatedSharePointSiteCollections;
                
            }
            else
                sharePointStatsPanel.Visible = false;


            if (org.CrmOrganizationId != Guid.Empty)
            {
                crmStatsPanel.Visible = true;
                BindCRMStats(orgStats);
            }
            else
                crmStatsPanel.Visible = false;

            
		}

		private void BindCRMStats(OrganizationStatistics orgStats)
		{
            lnkCRMUsers.NavigateUrl = EditUrl("ItemID", PanelRequest.ItemID.ToString(), "crmusers",
                "SpaceID=" + PanelSecurity.PackageId);

		    crmUsersStats.QuotaUsedValue = orgStats.CreatedCRMUsers;
		    crmUsersStats.QuotaValue = orgStats.AllocatedCRMUsers;
		}
        
        
	}
}