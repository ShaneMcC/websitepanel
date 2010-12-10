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

namespace WebsitePanel.Portal.ExchangeServer.UserControls
{
    class Tab
    {
        string id;
        string name;
        string url;

        public Tab(string id, string name, string url)
        {
            this.id = id;
            this.name = name;
            this.url = url;
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }
    }

    public partial class MailboxTabs : WebsitePanelControlBase
    {
        public const string ADUserTabs = "ADUserTabs";
        private string selectedTab;
        public string SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; }
        }

        public bool IsADUserTabs
        {
            get { return ViewState[ADUserTabs] != null ? Utils.ParseBool(ViewState[ADUserTabs].ToString(), false) : false; }
            set { ViewState[ADUserTabs] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsADUserTabs)
                BindAdUserTabs();
            else
                BindTabs();
        }

        private void BindTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("mailbox_settings", "Tab.Settings"));
            tabsList.Add(CreateTab("mailbox_addresses", "Tab.Addresses"));
            tabsList.Add(CreateTab("mailbox_mailflow", "Tab.Mailflow"));
            tabsList.Add(CreateTab("mailbox_permissions", "Tab.Permissions"));
            tabsList.Add(CreateTab("mailbox_advanced", "Tab.Advanced"));
            tabsList.Add(CreateTab("mailbox_setup", "Tab.Setup"));
            tabsList.Add(CreateTab("mailbox_mobile", "Tab.Mobile"));
            //tabsList.Add(CreateTab("mailbddox_spam", "Tab.Spam"));
            

            // find selected menu item
            int idx = 0;
            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                    break;
                idx++;
            }
            dlTabs.SelectedIndex = idx;

            dlTabs.DataSource = tabsList;
            dlTabs.DataBind();
        }

        private void BindAdUserTabs()
        {
            List<Tab> tabsList = new List<Tab>();
            tabsList.Add(CreateTab("edit_user", "Tab.Settings"));
            tabsList.Add(CreateTab("organization_user_setup", "Tab.Setup"));

            // find selected menu item
            int idx = 0;
            foreach (Tab tab in tabsList)
            {
                if (String.Compare(tab.Id, SelectedTab, true) == 0)
                    break;
                idx++;
            }
            dlTabs.SelectedIndex = idx;

            dlTabs.DataSource = tabsList;
            dlTabs.DataBind();
        }
        
        private Tab CreateTab(string id, string text)
        {
            return new Tab(id, GetLocalizedString(text),
                HostModule.EditUrl("AccountID", PanelRequest.AccountID.ToString(), id,
                "SpaceID=" + PanelSecurity.PackageId.ToString(),
                "ItemID=" + PanelRequest.ItemID.ToString()));
        }
    }
}