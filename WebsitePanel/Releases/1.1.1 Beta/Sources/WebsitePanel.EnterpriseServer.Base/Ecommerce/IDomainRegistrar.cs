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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public interface IDomainRegistrar : ISystemPlugin
	{
		//bool SubAccountRequired { get; }

		//bool CheckSubAccountExists(string account, string emailAddress);

		//AccountResult GetSubAccount(string account, string emailAddress);

		//AccountResult CreateSubAccount(CommandParams args);

		//DomainStatus CheckDomain(string domain);
		
		void RegisterDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers);

		void RenewDomain(DomainNameSvc domainSvc, ContractAccount accountInfo, string[] nameServers);

		TransferDomainResult TransferDomain(CommandParams args, DomainContacts contacts);

		//DomainContacts GetAccountContacts(CommandParams args);
	}

	#region Registrar's results

	public abstract class RegistrarResultBase
	{
		private bool succeed;
		private NameValueCollection resultData;

		public const string REGISTRAR = "Registrar";

		public bool Succeed
		{
			get { return this.succeed; }
			set { this.succeed = value; }
		}

		public int Count
		{
			get
			{
				EnsureResultDataCreated();
				return resultData.Count;
			}
		}

		public string[] AllKeys
		{
			get
			{
				EnsureResultDataCreated();
				return resultData.AllKeys;
			}
		}

		public string this[string keyName]
		{
			get
			{
				EnsureResultDataCreated();
				return resultData[keyName];
			}
			set
			{
				EnsureResultDataCreated();
				resultData[keyName] = value;
			}
		}

		public string this[int index]
		{
			get
			{
				EnsureResultDataCreated();
				return resultData[index];
			}
		}

		protected void EnsureResultDataCreated()
		{
			if (resultData == null)
				resultData = new NameValueCollection();
		}
	}

	public enum DomainStatus
	{
		Registered = 0,
		NotFound = 1,
		UnableToCheck = 2
	};

	public class RegisterDomainResult : RegistrarResultBase
	{
		public const string ORDER_NUMBER = "OrderNumber";
		public const string STATUS_CODE = "StatusCode";
		public const string STATUS_REASON = "StatusReason";
	}

	public class RenewDomainResult : RegistrarResultBase
	{
		public const string RENEW_ORDER_NUMBER = "RenewOrderNumber";
		public const string STATUS_CODE = "StatusCode";
		public const string STATUS_REASON = "StatusReason";
	}

	public class TransferDomainResult : RegistrarResultBase
	{
		public const string TRANSFER_ORDER_NUMBER = "TransferOrderNumber";
		public const string ERROR_MESSAGE = "ErrorMessage";
		
	}

	public class AccountResult : RegistrarResultBase
	{
		public const string STATUS_CODE = "StatusCode";
		public const string STATUS_REASON = "StatusReason";
		public const string DEFAULT_CONTACT_ID = "DefaultContactID";
		public const string REGISTRAR_ACCOUNT_ID = "RegistrarAccountID";
		public const string ACCOUNT_LOGIN_ID = "AccountLoginID";
		public const string ACCOUNT_ID = "AccountID";
		public const string ACCOUNT_PARTY_ID = "AccountPartyID";
	}

	#endregion

	#region Registrar's core types

	public class CommandParams : NameValueCollection
	{
		public const string USERNAME = "Username";
		public const string NAME_SERVERS = "NameServers";
		public const string DOMAIN_NAME = "DomainName";
		public const string DOMAIN_TLD = "DomainTLD";
		public const string YEARS = "Years";
		public const string EMAIL = "Email";
		public const string PASSWORD = "Password";
		public const string FIRST_NAME = "FirstName";
		public const string LAST_NAME = "LastName";
		public const string ADDRESS = "Address";
		public const string CITY = "City";
		public const string STATE = "State";
		public const string COUNTRY = "Country";
		public const string ZIP = "Zip";
		public const string PHONE = "Phone";
		public const string FAX = "Fax";

		public bool HasKey(string keyName)
		{
			return !String.IsNullOrEmpty(
				this[keyName]
			);
		}
	}

	public class DomainContacts : NameObjectCollectionBase
	{
		public DomainContact this[string contactType]
		{
			get
			{
				DomainContact contact = (DomainContact)BaseGet(contactType);

				if (contact == null)
				{
					contact = new DomainContact();
					BaseSet(contactType, contact);
				}

				return contact;
			}
		}
	}

	public class DomainContact
	{
		private NameValueCollection contactInfo;

		public string this[string keyName]
		{
			get { return contactInfo[keyName]; }
			set { contactInfo[keyName] = value; }
		}

		public string[] Keys
		{
			get { return contactInfo.AllKeys; }
		}

		public bool HasKey(string keyName)
		{
			return !String.IsNullOrEmpty(
				contactInfo[keyName]
			);
		}

		public DomainContact()
		{
			contactInfo = new NameValueCollection();
		}
	}

	#endregion
}