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
using System.Text;

namespace WebsitePanel.Providers.HostedSolution
{
	public class ExchangeAccount
	{
		int accountId;
		int itemId;
        int packageId;
		ExchangeAccountType accountType;
		string accountName;
		string displayName;
		string primaryEmailAddress;
		bool mailEnabledPublicFolder;
        MailboxManagerActions mailboxManagerActions;
        string accountPassword;
        string samAccountName;

		public int AccountId
		{
			get { return this.accountId; }
			set { this.accountId = value; }
		}

		public int ItemId
		{
            get { return this.itemId; }
            set { this.itemId = value; }
		}

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

		public ExchangeAccountType AccountType
		{
			get { return this.accountType; }
			set { this.accountType = value; }
		}

		public string AccountName
		{
			get { return this.accountName; }
			set { this.accountName = value; }
		}

        public string SamAccountName
        {
            get { return this.samAccountName; }
            set { this.samAccountName = value; }
        }

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public string PrimaryEmailAddress
		{
			get { return this.primaryEmailAddress; }
			set { this.primaryEmailAddress = value; }
		}

		public bool MailEnabledPublicFolder
		{
			get { return this.mailEnabledPublicFolder; }
			set { this.mailEnabledPublicFolder = value; }
		}

        public string AccountPassword
        {
            get { return this.accountPassword; }
            set { this.accountPassword = value; }
        }

        public MailboxManagerActions MailboxManagerActions
        {
            get { return this.mailboxManagerActions; }
            set { this.mailboxManagerActions = value; }
        }
	}
}
