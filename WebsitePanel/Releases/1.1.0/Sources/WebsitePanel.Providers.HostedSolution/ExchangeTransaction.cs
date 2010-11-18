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

using System.Collections.Generic;

namespace WebsitePanel.Providers.HostedSolution
{
	internal class ExchangeTransaction
	{
		List<TransactionAction> actions = null;

		public ExchangeTransaction()
		{
			actions = new List<TransactionAction>();
		}

		internal List<TransactionAction> Actions
		{
			get { return actions; }
		}

		internal void RegisterNewOrganizationUnit(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateOrganizationUnit;
			action.Id = id;
			Actions.Add(action);
		}

		public void RegisterNewDistributionGroup(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateDistributionGroup;
			action.Id = id;
			Actions.Add(action);
		}


		public void RegisterMailEnabledDistributionGroup(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.EnableDistributionGroup;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewGlobalAddressList(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateGlobalAddressList;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewAddressList(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateAddressList;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewOfflineAddressBook(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateOfflineAddressBook;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewActiveSyncPolicy(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateActiveSyncPolicy;
			action.Id = id;
			Actions.Add(action);
		}


		internal void RegisterNewAcceptedDomain(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateAcceptedDomain;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewUPNSuffix(string id, string suffix)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.AddUPNSuffix;
			action.Id = id;
			action.Suffix = suffix;
			Actions.Add(action);
		}

		internal void RegisterNewMailbox(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateMailbox;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewContact(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreateContact;
			action.Id = id;
			Actions.Add(action);
		}

		internal void RegisterNewPublicFolder(string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.CreatePublicFolder;
			action.Id = id;
			Actions.Add(action);
		}

		internal void AddMailBoxFullAccessPermission(string accountName, string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.AddMailboxFullAccessPermission;
			action.Id = id;
			action.Account = accountName;
			Actions.Add(action);
		}

		internal void AddSendAsPermission(string accountName, string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.AddSendAsPermission;
			action.Id = id;
			action.Account = accountName;
			Actions.Add(action);
		}

		internal void RemoveMailboxFullAccessPermission(string accountName, string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.RemoveMailboxFullAccessPermission;
			action.Id = id;
			action.Account = accountName;
			Actions.Add(action);		
		}

		internal void RemoveSendAsPermission(string accountName, string id)
		{
			TransactionAction action = new TransactionAction();
			action.ActionType = TransactionAction.TransactionActionTypes.RemoveSendAsPermission;
			action.Id = id;
			action.Account = accountName;
			Actions.Add(action);
		}
	}

	internal class TransactionAction
	{
		private TransactionActionTypes actionType;

		public TransactionActionTypes ActionType
		{
			get { return actionType; }
			set { actionType = value; }
		}
	
		private string id;

		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		private string suffix;

		public string Suffix
		{
			get { return suffix; }
			set { suffix = value; }
		}

		private string account;

		public string Account
		{
			get { return account; }
			set { account = value; }

		}
		
		internal enum TransactionActionTypes
		{
			CreateOrganizationUnit,
			CreateGlobalAddressList,
			CreateAddressList,
			CreateOfflineAddressBook,
			CreateDistributionGroup,
			EnableDistributionGroup,
			CreateAcceptedDomain,
			AddUPNSuffix,
			CreateMailbox,
			CreateContact,
			CreatePublicFolder,
			CreateActiveSyncPolicy,
			AddMailboxFullAccessPermission,
			AddSendAsPermission,
			RemoveMailboxFullAccessPermission,
			RemoveSendAsPermission
		};
	}


}
