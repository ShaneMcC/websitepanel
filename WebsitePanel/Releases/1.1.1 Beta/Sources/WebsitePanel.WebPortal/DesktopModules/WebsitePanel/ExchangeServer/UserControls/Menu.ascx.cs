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
using WebsitePanel.WebPortal;
using WebsitePanel.EnterpriseServer;
namespace WebsitePanel.Portal.ExchangeServer.UserControls
{
	public partial class Menu : WebsitePanelControlBase
	{
		
        public class MenuGroup
        {
            private string text;
            private List<MenuItem> menuItems;
            private string imageUrl;

            public string Text
            {
                get { return text; }
                set { text = value; }
            }

            public List<MenuItem> MenuItems
            {
                get { return menuItems; }
                set { menuItems = value; }
            }

            public string ImageUrl
            {
                get { return imageUrl; }
                set { imageUrl = value;}
            }

            public MenuGroup(string text, string imageUrl)
            {
                this.text = text;
                this.imageUrl = imageUrl;
                menuItems = new List<MenuItem>();
            }

        }
        
        public class MenuItem
		{			
            private string url;
			private string text;
			private string key;

            public string Url
			{
				get { return url; }
				set { url = value; }
			}

			public string Text
			{
				get { return text; }
				set { text = value; }
			}

			public string Key
			{
				get { return key; }
				set { key = value; }
			}


		}

		private string selectedItem;
		public string SelectedItem
		{
			get { return selectedItem; }
			set { selectedItem = value; }
		}

		private bool CheckQouta(string key, PackageContext cntx)
		{
            return cntx.Quotas.ContainsKey(key) &&
                ((cntx.Quotas[key].QuotaAllocatedValue == 1 && cntx.Quotas[key].QuotaTypeId == 1) ||
                (cntx.Quotas[key].QuotaTypeId != 1 && (cntx.Quotas[key].QuotaAllocatedValue > 0 || cntx.Quotas[key].QuotaAllocatedValue == -1)));
		}
        
        private void PrepareExchangeMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
                        
            MenuGroup exchangeGroup = new MenuGroup(GetLocalizedString("Text.ExchangeGroup"), imagePath + "exchange24.png");

            if (CheckQouta(Quotas.EXCHANGE2007_MAILBOXES, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("Mailboxes", "mailboxes"));

            if (CheckQouta(Quotas.EXCHANGE2007_CONTACTS, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("Contacts", "contacts"));

            if (CheckQouta(Quotas.EXCHANGE2007_DISTRIBUTIONLISTS, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("DistributionLists", "dlists"));

            if (CheckQouta(Quotas.EXCHANGE2007_PUBLICFOLDERS, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("PublicFolders", "public_folders"));


            
            exchangeGroup.MenuItems.Add(CreateMenuItem("StorageUsage", "storage_usage"));
            exchangeGroup.MenuItems.Add(CreateMenuItem("StorageLimits", "storage_limits"));

            if (CheckQouta(Quotas.EXCHANGE2007_ACTIVESYNCENABLED, cntx))
                exchangeGroup.MenuItems.Add(CreateMenuItem("ActiveSyncPolicy", "activesync_policy"));

            if (exchangeGroup.MenuItems.Count > 0)
                groups.Add(exchangeGroup);

        }
        
        private void PrepareOrganizationMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup organizationGroup = new MenuGroup(GetLocalizedString("Text.OrganizationGroup"), imagePath + "company24.png");
            if (CheckQouta(Quotas.ORGANIZATION_DOMAINS, cntx))
                organizationGroup.MenuItems.Add(CreateMenuItem("DomainNames", "domains"));
            if (CheckQouta(Quotas.ORGANIZATION_USERS, cntx))
                organizationGroup.MenuItems.Add(CreateMenuItem("Users", "users"));
            
            if (organizationGroup.MenuItems.Count >0)
                groups.Add(organizationGroup);
   
        }

        private void PrepareCRMMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup crmGroup = new MenuGroup(GetLocalizedString("Text.CRMGroup"), imagePath + "crm_16.png");

            crmGroup.MenuItems.Add(CreateMenuItem("CRMOrganization", "CRMOrganizationDetails"));
            crmGroup.MenuItems.Add(CreateMenuItem("CRMUsers", "CRMUsers"));
            
            if (crmGroup.MenuItems.Count > 0)
                groups.Add(crmGroup);
       
        }

        private void PrepareBlackBerryMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup bbGroup = new MenuGroup(GetLocalizedString("Text.BlackBerryGroup"), imagePath + "blackberry16.png");

            bbGroup.MenuItems.Add(CreateMenuItem("BlackBerryUsers", "blackberry_users"));
            

            if (bbGroup.MenuItems.Count > 0)
                groups.Add(bbGroup);

        }

        private void PrepareSharePointMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup sharepointGroup =
                    new MenuGroup(GetLocalizedString("Text.SharePointGroup"), imagePath + "sharepoint24.png");
            sharepointGroup.MenuItems.Add(CreateMenuItem("SiteCollections", "sharepoint_sitecollections"));
            sharepointGroup.MenuItems.Add(CreateMenuItem("StorageUsage", "sharepoint_storage_usage"));
            sharepointGroup.MenuItems.Add(CreateMenuItem("StorageLimits", "sharepoint_storage_settings"));
            
            groups.Add(sharepointGroup);


        }
        
        private void PrepareOCSMenu(PackageContext cntx, List<MenuGroup> groups, string imagePath)
        {
            MenuGroup ocsGroup =
                   new MenuGroup(GetLocalizedString("Text.OCSGroup"), imagePath + "ocs16.png");
            ocsGroup.MenuItems.Add(CreateMenuItem("OCSUsers", "ocs_users"));
            
            
            groups.Add(ocsGroup);
        }

        private List<MenuGroup> PrepareMenu()
		{
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);
                        
            List<MenuGroup> groups = new List<MenuGroup>();

            string imagePath = String.Concat("~/", DefaultPage.THEMES_FOLDER, "/", Page.Theme, "/", "Images/Exchange", "/");
            
            //Organization menu group;
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedOrganizations))
                PrepareOrganizationMenu(cntx, groups, imagePath);
            
            
            //Exchange menu group;
            if (cntx.Groups.ContainsKey(ResourceGroups.Exchange))
                PrepareExchangeMenu(cntx, groups, imagePath);

            //BlackBerry Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.BlackBerry))
                PrepareBlackBerryMenu(cntx, groups, imagePath);
            
            //SharePoint menu group;
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedSharePoint))
            {             
                PrepareSharePointMenu(cntx, groups, imagePath);   
            }

            //CRM Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.HostedCRM))
                PrepareCRMMenu(cntx, groups, imagePath);


            //OCS Menu
            if (cntx.Groups.ContainsKey(ResourceGroups.OCS))
                PrepareOCSMenu(cntx, groups, imagePath);

		    
            return groups;
		}
        
        protected void Page_Load(object sender, EventArgs e)
		{
            List<MenuGroup> groups = PrepareMenu();
			
            /*repMenu.SelectedIndex = -1;
			
			for(int i = 0; i < items.Count; i++)
			{
				if (String.Compare(SelectedItem, items[i].Key, true) == 0)
				{
					repMenu.SelectedIndex = i;
					break;
				}
			}*/

			// bind
            repMenu.DataSource = groups;
			repMenu.DataBind();
		}

		private MenuItem CreateMenuItem(string text, string key)
		{
			MenuItem item = new MenuItem();
			item.Key = key;
			item.Text = GetLocalizedString("Text." + text);
			item.Url = HostModule.EditUrl("ItemID", PanelRequest.ItemID.ToString(), key,
				"SpaceID=" + PanelSecurity.PackageId);
			return item;
		}
	}
}
