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
using WebsitePanel.Providers.Mail;

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class SmarterMail60_EditDomain_Sharing : WebsitePanelControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SaveItem(MailDomain item)
        {
            item.IsGlobalAddressList = cbGlobalAddressList.Checked;
            item.SharedCalendars = cbSharedCalendars.Checked;
            item.SharedContacts = cbSharedContacts.Checked;
            item.SharedFolders = cbSharedFolders.Checked;
            item.SharedNotes = cbSharedNotes.Checked;
            item.SharedTasks = cbSharedTasks.Checked;

        }

        public void BindItem(MailDomain item)
        {
            cbGlobalAddressList.Checked = item.IsGlobalAddressList;
            cbSharedCalendars.Checked = item.SharedCalendars;
            cbSharedContacts.Checked = item.SharedContacts;
            cbSharedFolders.Checked = item.SharedFolders;
            cbSharedNotes.Checked = item.SharedNotes;
            cbSharedTasks.Checked = item.SharedTasks;
        }

    }
}