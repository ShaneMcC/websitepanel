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
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.ResultObjects;
using Microsoft.Web.Services3;

namespace WebsitePanel.EnterpriseServer
{
	/// <summary>
	/// Summary description for esApplicationsInstaller
	/// </summary>
	[WebService(Namespace = "http://smbsaas/websitepanel/enterpriseserver")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[Policy("ServerPolicy")]
	[ToolboxItem(false)]
	public class esExchangeServer : WebService
	{
		#region Organizations
		[WebMethod]
		public DataSet GetRawExchangeOrganizationsPaged(int packageId, bool recursive,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return ExchangeServerController.GetRawExchangeOrganizationsPaged(packageId, recursive,
				filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		[WebMethod]
		public OrganizationsPaged GetExchangeOrganizationsPaged(int packageId, bool recursive,
			string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
			return ExchangeServerController.GetExchangeOrganizationsPaged(packageId, recursive,
				filterColumn, filterValue, sortColumn, startRow, maximumRows);
		}

		[WebMethod]
		public List<Organization> GetExchangeOrganizations(int packageId, bool recursive)
		{
			return ExchangeServerController.GetExchangeOrganizations(packageId, recursive);
		}

		[WebMethod]
		public Organization GetOrganization(int itemId)
		{
			return ExchangeServerController.GetOrganization(itemId);
		}

		[WebMethod]
		public OrganizationStatistics GetOrganizationStatistics(int itemId)
		{
			return ExchangeServerController.GetOrganizationStatistics(itemId);
		}

		

		[WebMethod]
		public int DeleteOrganization(int itemId)
		{
			return ExchangeServerController.DeleteOrganization(itemId);
		}

		[WebMethod]
		public Organization GetOrganizationStorageLimits(int itemId)
		{
			return ExchangeServerController.GetOrganizationStorageLimits(itemId);
		}

		[WebMethod]
		public int SetOrganizationStorageLimits(int itemId, int issueWarningKB, int prohibitSendKB,
			int prohibitSendReceiveKB, int keepDeletedItemsDays, bool applyToMailboxes)
		{
			return ExchangeServerController.SetOrganizationStorageLimits(itemId, issueWarningKB, prohibitSendKB,
				prohibitSendReceiveKB, keepDeletedItemsDays, applyToMailboxes);
		}

		[WebMethod]
		public ExchangeItemStatistics[] GetMailboxesStatistics(int itemId)
		{
			return ExchangeServerController.GetMailboxesStatistics(itemId);
		}

		[WebMethod]
		public ExchangeItemStatistics[] GetPublicFoldersStatistics(int itemId)
		{
			return ExchangeServerController.GetPublicFoldersStatistics(itemId);
		}

		[WebMethod]
		public int CalculateOrganizationDiskspace(int itemId)
		{
			return ExchangeServerController.CalculateOrganizationDiskspace(itemId);
		}

        [WebMethod]
        public ExchangeActiveSyncPolicy GetActiveSyncPolicy(int itemId)
        {
            return ExchangeServerController.GetActiveSyncPolicy(itemId);
        }

        [WebMethod]
        public int SetActiveSyncPolicy(int itemId, bool allowNonProvisionableDevices,
                bool attachmentsEnabled, int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled,
                bool devicePasswordEnabled, bool alphanumericPasswordRequired, bool passwordRecoveryEnabled,
                bool deviceEncryptionEnabled, bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength,
                int inactivityLockMin, int passwordExpirationDays, int passwordHistory, int refreshInteval)
        {
            return ExchangeServerController.SetActiveSyncPolicy(itemId, allowNonProvisionableDevices, attachmentsEnabled,
                    maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired,
                    passwordRecoveryEnabled, deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts,
                    minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInteval);
        }
		#endregion

		#region Domains
		[WebMethod]
        public int AddAuthoritativeDomain(int itemId, int domainId)
		{
            return ExchangeServerController.AddAuthoritativeDomain(itemId, domainId);
		}

        [WebMethod]
        public int DeleteAuthoritativeDomain(int itemId, int domainId)
        {
            return ExchangeServerController.DeleteAuthoritativeDomain(itemId, domainId);
        }


		#endregion

		#region Accounts
		[WebMethod]
		public ExchangeAccountsPaged GetAccountsPaged(int itemId, string accountTypes,
			string filterColumn, string filterValue, string sortColumn,
			int startRow, int maximumRows)
		{
            return ExchangeServerController.GetAccountsPaged(itemId, accountTypes,
				filterColumn, filterValue, sortColumn,
				startRow, maximumRows);
		}

		[WebMethod]
		public List<ExchangeAccount> GetAccounts(int itemId, ExchangeAccountType accountType)
		{
			return ExchangeServerController.GetAccounts(itemId, accountType);
		}

		[WebMethod]
		public List<ExchangeAccount> SearchAccounts(int itemId,
			bool includeMailboxes, bool includeContacts, bool includeDistributionLists,
            bool includeRooms, bool includeEquipment,
			string filterColumn, string filterValue, string sortColumn)
		{
			return ExchangeServerController.SearchAccounts(itemId,
				includeMailboxes, includeContacts, includeDistributionLists,
                includeRooms, includeEquipment,
				filterColumn, filterValue, sortColumn);
		}

		[WebMethod]
		public ExchangeAccount GetAccount(int itemId, int accountId)
		{
			return ExchangeServerController.GetAccount(itemId, accountId);
		}

        [WebMethod]
        public ExchangeAccount SearchAccount(ExchangeAccountType accountType, string primaryEmailAddress)
        {
            return ExchangeServerController.SearchAccount(accountType, primaryEmailAddress);
        }

        [WebMethod]
        public bool CheckAccountCredentials(int itemId, string email, string password)
        {
            return ExchangeServerController.CheckAccountCredentials(itemId, email, password);
        }

		#endregion

		#region Mailboxes
		[WebMethod]
		public int CreateMailbox(int itemId, int accountId,  ExchangeAccountType accountType, string accountName, string displayName,
            string name, string domain, string password, bool sendSetupInstructions, string setupInstructionMailAddress)
		{
            return ExchangeServerController.CreateMailbox(itemId, accountId, accountType, accountName, displayName, name, domain, password, sendSetupInstructions, setupInstructionMailAddress);
		}

		[WebMethod]
		public int DeleteMailbox(int itemId, int accountId)
		{
			return ExchangeServerController.DeleteMailbox(itemId, accountId);
		}

        [WebMethod]
        public int DisableMailbox(int itemId, int accountId)
        {
            return ExchangeServerController.DisableMailbox(itemId, accountId);
        }


		[WebMethod]
		public ExchangeMailbox GetMailboxGeneralSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetMailboxGeneralSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetMailboxGeneralSettings(int itemId, int accountId, string displayName,
			string password, bool hideAddressBook, bool disabled, string firstName, string initials,
			string lastName, string address, string city, string state, string zip, string country,
			string jobTitle, string company, string department, string office, string managerAccountName,
			string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes)
		{
			return ExchangeServerController.SetMailboxGeneralSettings(itemId, accountId, displayName,
				password, hideAddressBook, disabled, firstName, initials,
				lastName, address, city, state, zip, country,
				jobTitle, company, department, office, managerAccountName,
				businessPhone, fax, homePhone, mobilePhone, pager,
				webPage, notes);
		}

		[WebMethod]
		public ExchangeEmailAddress[] GetMailboxEmailAddresses(int itemId, int accountId)
		{
			return ExchangeServerController.GetMailboxEmailAddresses(itemId, accountId);
		}

		[WebMethod]
		public int AddMailboxEmailAddress(int itemId, int accountId, string emailAddress)
		{
			return ExchangeServerController.AddMailboxEmailAddress(itemId, accountId, emailAddress);
		}

		[WebMethod]
		public int SetMailboxPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
		{
			return ExchangeServerController.SetMailboxPrimaryEmailAddress(itemId, accountId, emailAddress);
		}

		[WebMethod]
		public int DeleteMailboxEmailAddresses(int itemId, int accountId, string[] emailAddresses)
		{
			return ExchangeServerController.DeleteMailboxEmailAddresses(itemId, accountId, emailAddresses);
		}

		[WebMethod]
		public ExchangeMailbox GetMailboxMailFlowSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetMailboxMailFlowSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetMailboxMailFlowSettings(int itemId, int accountId,
			bool enableForwarding, string forwardingAccountName, bool forwardToBoth,
			string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts,
			int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB,
            bool requireSenderAuthentication)
		{
			return ExchangeServerController.SetMailboxMailFlowSettings(itemId, accountId,
				enableForwarding, forwardingAccountName, forwardToBoth,
				sendOnBehalfAccounts, acceptAccounts, rejectAccounts,
				maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB,
				requireSenderAuthentication);
		}

		[WebMethod]
		public ExchangeMailbox GetMailboxAdvancedSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetMailboxAdvancedSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetMailboxAdvancedSettings(int itemId, int accountId, bool enablePOP,
			bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync,
            int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays)
		{
			return ExchangeServerController.SetMailboxAdvancedSettings(itemId, accountId, enablePOP,
				enableIMAP, enableOWA, enableMAPI, enableActiveSync,
				issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
		}

        [WebMethod]
        public string GetMailboxSetupInstructions(int itemId, int accountId, bool pmm, bool emailMode, bool signup)
        {
            return ExchangeServerController.GetMailboxSetupInstructions(itemId, accountId, pmm, emailMode, signup);
        }

        [WebMethod]
        public int SendMailboxSetupInstructions(int itemId, int accountId, bool signup, string to, string cc)
        {
            return ExchangeServerController.SendMailboxSetupInstructions(itemId, accountId, signup, to, cc);
        }

        [WebMethod]
        public int SetMailboxManagerSettings(int itemId, int accountId, bool pmmAllowed, MailboxManagerActions action)
        {
            return ExchangeServerController.SetMailboxManagerSettings(itemId, accountId, pmmAllowed, action);
        }

		[WebMethod]
        public ExchangeMailbox GetMailboxPermissions(int itemId, int accountId)
        {
		    return ExchangeServerController.GetMailboxPermissions(itemId, accountId);
        }

        [WebMethod]
        public int SetMailboxPermissions(int itemId, int accountId, string[] sendAsaccounts, string[] fullAccessAcounts)
        {
            return ExchangeServerController.SetMailboxPermissions(itemId, accountId, sendAsaccounts, fullAccessAcounts);       
        }
        
        
	    #endregion

		#region Contacts
		[WebMethod]
		public int CreateContact(int itemId, string displayName, string email)
		{
			return ExchangeServerController.CreateContact(itemId, displayName, email);
		}

		[WebMethod]
		public int DeleteContact(int itemId, int accountId)
		{
			return ExchangeServerController.DeleteContact(itemId, accountId);
		}

		[WebMethod]
		public ExchangeContact GetContactGeneralSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetContactGeneralSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetContactGeneralSettings(int itemId, int accountId, string displayName, string emailAddress,
			bool hideAddressBook, string firstName, string initials,
			string lastName, string address, string city, string state, string zip, string country,
			string jobTitle, string company, string department, string office, string managerAccountName,
			string businessPhone, string fax, string homePhone, string mobilePhone, string pager,
            string webPage, string notes, int useMapiRichTextFormat)
		{
			return ExchangeServerController.SetContactGeneralSettings(itemId, accountId, displayName, emailAddress,
				hideAddressBook, firstName, initials,
				lastName, address, city, state, zip, country,
				jobTitle, company, department, office, managerAccountName,
				businessPhone, fax, homePhone, mobilePhone, pager,
				webPage, notes, useMapiRichTextFormat);
		}

		[WebMethod]
		public ExchangeContact GetContactMailFlowSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetContactMailFlowSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetContactMailFlowSettings(int itemId, int accountId,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			return ExchangeServerController.SetContactMailFlowSettings(itemId, accountId,
				acceptAccounts, rejectAccounts, requireSenderAuthentication);
		}
		#endregion

		#region Distribution Lists
		[WebMethod]
		public int CreateDistributionList(int itemId, string displayName, string name, string domain, int managerId)
		{
			return ExchangeServerController.CreateDistributionList(itemId, displayName, name, domain, managerId);
		}

		[WebMethod]
		public int DeleteDistributionList(int itemId, int accountId)
		{
			return ExchangeServerController.DeleteDistributionList(itemId, accountId);
		}

		[WebMethod]
		public ExchangeDistributionList GetDistributionListGeneralSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetDistributionListGeneralSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetDistributionListGeneralSettings(int itemId, int accountId, string displayName,
			bool hideAddressBook, string managerAccount, string[] memberAccounts,
			string notes)
		{
			return ExchangeServerController.SetDistributionListGeneralSettings(itemId, accountId, displayName,
				hideAddressBook, managerAccount, memberAccounts,
				notes);
		}

		[WebMethod]
		public ExchangeDistributionList GetDistributionListMailFlowSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetDistributionListMailFlowSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetDistributionListMailFlowSettings(int itemId, int accountId,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			return ExchangeServerController.SetDistributionListMailFlowSettings(itemId, accountId,
				acceptAccounts, rejectAccounts, requireSenderAuthentication);
		}

		[WebMethod]
		public ExchangeEmailAddress[] GetDistributionListEmailAddresses(int itemId, int accountId)
		{
			return ExchangeServerController.GetDistributionListEmailAddresses(itemId, accountId);
		}

		[WebMethod]
		public int AddDistributionListEmailAddress(int itemId, int accountId, string emailAddress)
		{
			return ExchangeServerController.AddDistributionListEmailAddress(itemId, accountId, emailAddress);
		}

		[WebMethod]
		public int SetDistributionListPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
		{
			return ExchangeServerController.SetDistributionListPrimaryEmailAddress(itemId, accountId, emailAddress);
		}

		[WebMethod]
		public int DeleteDistributionListEmailAddresses(int itemId, int accountId, string[] emailAddresses)
		{
			return ExchangeServerController.DeleteDistributionListEmailAddresses(itemId, accountId, emailAddresses);
		}

        [WebMethod]
        public ResultObject SetDistributionListPermissions(int itemId, int accountId, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            return ExchangeServerController.SetDistributionListPermissions(itemId, accountId, sendAsAccounts, sendOnBehalfAccounts);
        }

        [WebMethod]
        public ExchangeDistributionListResult GetDistributionListPermissions(int itemId, int accountId)
        {
            return ExchangeServerController.GetDistributionListPermissions(itemId, accountId);
        }
        
        #endregion

		#region Public Folders
		[WebMethod]
		public int CreatePublicFolder(int itemId, string parentFolder, string folderName,
			bool mailEnabled, string accountName, string domain)
		{
			return ExchangeServerController.CreatePublicFolder(itemId, parentFolder, folderName,
				mailEnabled, accountName, domain);
		}

		[WebMethod]
		public int DeletePublicFolders(int itemId, int[] accountIds)
		{
			return ExchangeServerController.DeletePublicFolders(itemId, accountIds);
		}

		[WebMethod]
		public int DeletePublicFolder(int itemId, int accountId)
		{
			return ExchangeServerController.DeletePublicFolder(itemId, accountId);
		}

		[WebMethod]
		public int EnableMailPublicFolder(int itemId, int accountId,
			string name, string domain)
		{
			return ExchangeServerController.EnableMailPublicFolder(itemId, accountId, name, domain);
		}

		[WebMethod]
		public int DisableMailPublicFolder(int itemId, int accountId)
		{
			return ExchangeServerController.DisableMailPublicFolder(itemId, accountId);
		}

		[WebMethod]
		public ExchangePublicFolder GetPublicFolderGeneralSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetPublicFolderGeneralSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetPublicFolderGeneralSettings(int itemId, int accountId, string newName,
			bool hideAddressBook, string[] authorAccounts)
		{
			return ExchangeServerController.SetPublicFolderGeneralSettings(itemId, accountId, newName,
				hideAddressBook, authorAccounts);
		}

		[WebMethod]
		public ExchangePublicFolder GetPublicFolderMailFlowSettings(int itemId, int accountId)
		{
			return ExchangeServerController.GetPublicFolderMailFlowSettings(itemId, accountId);
		}

		[WebMethod]
		public int SetPublicFolderMailFlowSettings(int itemId, int accountId,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			return ExchangeServerController.SetPublicFolderMailFlowSettings(itemId, accountId,
				acceptAccounts, rejectAccounts, requireSenderAuthentication);
		}

		[WebMethod]
		public ExchangeEmailAddress[] GetPublicFolderEmailAddresses(int itemId, int accountId)
		{
			return ExchangeServerController.GetPublicFolderEmailAddresses(itemId, accountId);
		}

		[WebMethod]
		public int AddPublicFolderEmailAddress(int itemId, int accountId, string emailAddress)
		{
			return ExchangeServerController.AddPublicFolderEmailAddress(itemId, accountId, emailAddress);
		}

		[WebMethod]
		public int SetPublicFolderPrimaryEmailAddress(int itemId, int accountId, string emailAddress)
		{
			return ExchangeServerController.SetPublicFolderPrimaryEmailAddress(itemId, accountId, emailAddress);
		}

		[WebMethod]
		public int DeletePublicFolderEmailAddresses(int itemId, int accountId, string[] emailAddresses)
		{
			return ExchangeServerController.DeletePublicFolderEmailAddresses(itemId, accountId, emailAddresses);
		}
		#endregion

        #region MobileDevice

        [WebMethod]
        public ExchangeMobileDevice[] GetMobileDevices(int itemId, int accountId)
        {
            return ExchangeServerController.GetMobileDevices(itemId, accountId);
        }

        [WebMethod]
        public ExchangeMobileDevice GetMobileDevice(int itemId, string deviceId)
        {
            return ExchangeServerController.GetMobileDevice(itemId, deviceId);
        }

        [WebMethod]
        public void WipeDataFromDevice(int itemId, string deviceId)
        {
            ExchangeServerController.WipeDataFromDevice(itemId, deviceId);
        }

        [WebMethod]
        public void CancelRemoteWipeRequest(int itemId, string deviceId)
        {
            ExchangeServerController.CancelRemoteWipeRequest(itemId, deviceId);
        }

        [WebMethod]
        public void RemoveDevice(int itemId, string deviceId)
        {
            ExchangeServerController.RemoveDevice(itemId, deviceId);
        }

        #endregion

    }
}
