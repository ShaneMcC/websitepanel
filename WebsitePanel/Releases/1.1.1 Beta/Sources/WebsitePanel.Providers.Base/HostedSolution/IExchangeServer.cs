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

namespace WebsitePanel.Providers.HostedSolution
{
	public interface IExchangeServer
	{
	    
        bool CheckAccountCredentials(string username, string password);
		// Organizations

        string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, ExchangeAccountType accountType, 
	                                string mailboxDatabase, string offlineAddressBook,
	                                string accountName, bool enablePOP, bool enableIMAP,
	                                bool enableOWA, bool enableMAPI, bool enableActiveSync,
	                                int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB,
	                                int keepDeletedItemsDays);
        
        Organization ExtendToExchangeOrganization(string organizationId, string securityGroup);
		string GetOABVirtualDirectory();
		Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir);
		void UpdateOrganizationOfflineAddressBook(string id);
		bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string offlineAddressBook, string securityGroup);
		void SetOrganizationStorageLimits(string organizationDistinguishedName, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays);
		ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName);

		// Domains
        void AddAuthoritativeDomain(string domain);
        void DeleteAuthoritativeDomain(string domain);
	    string[] GetAuthoritativeDomains();

		// Mailboxes
		string CreateMailbox(string organizationId, string organizationDistinguishedName, string mailboxDatabase, string securityGroup, string offlineAddressBook, ExchangeAccountType accountType, string displayName, string accountName, string name, string domain, string password, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync,
			int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays); 
		void DeleteMailbox(string accountName);
        void DisableMailbox(string id);
		ExchangeMailbox GetMailboxGeneralSettings(string accountName);
		void SetMailboxGeneralSettings(string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes);
		ExchangeMailbox GetMailboxMailFlowSettings(string accountName);
		void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool requireSenderAuthentication);
		ExchangeMailbox GetMailboxAdvancedSettings(string accountName);
		void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays);
		ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName);
		void SetMailboxEmailAddresses(string accountName, string[] emailAddresses);
		void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress);
	    void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts);
		ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName);
		ExchangeMailboxStatistics GetMailboxStatistics(string accountName);

		// Contacts
        void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain); 
		void DeleteContact(string accountName);
		ExchangeContact GetContactGeneralSettings(string accountName);
        void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain);
		ExchangeContact GetContactMailFlowSettings(string accountName);
		void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);

		// Distribution Lists
		void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy);
		void DeleteDistributionList(string accountName);
		ExchangeDistributionList GetDistributionListGeneralSettings(string accountName);
		void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] memebers, string notes);
		void AddDistributionListMembers(string accountName, string[] memberAccounts);
		void RemoveDistributionListMembers(string accountName, string[] memberAccounts);
		ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName);
		void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
		ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName);
		void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses);
		void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress);
		ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName);
		void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts);

		// Public Folders
		void CreatePublicFolder(string organizationId, string securityGroup, string parentFolder, string folderName, bool mailEnabled, string accountName, string name, string domain);
		void DeletePublicFolder(string folder);
		void EnableMailPublicFolder(string organizationId, string folder, string accountName, string name, string domain);
		void DisableMailPublicFolder(string folder);
		ExchangePublicFolder GetPublicFolderGeneralSettings(string folder);
		void SetPublicFolderGeneralSettings(string folder, string newFolderName, string[] authorAccounts, bool hideFromAddressBook);
		ExchangePublicFolder GetPublicFolderMailFlowSettings(string folder);
		void SetPublicFolderMailFlowSettings(string folder, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication);
		ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string folder);
		void SetPublicFolderEmailAddresses(string folder, string[] emailAddresses);
		void SetPublicFolderPrimaryEmailAddress(string folder, string emailAddress);
		ExchangeItemStatistics[] GetPublicFoldersStatistics(string[] folders);
		string[] GetPublicFoldersRecursive(string parent);
		long GetPublicFolderSize(string folder);

		//ActiveSync
		void CreateOrganizationActiveSyncPolicy(string organizationId);
		ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId);
		void SetActiveSyncPolicy(string organizationId, bool allowNonProvisionableDevices, bool attachmentsEnabled,
			int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled,
			bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled,
			bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin,
			int passwordExpirationDays, int passwordHistory, int refreshInterval);

		//Mobile Devices
		ExchangeMobileDevice[] GetMobileDevices(string accountName);
		ExchangeMobileDevice GetMobileDevice(string id);
		void WipeDataFromDevice(string id);
		void CancelRemoteWipeRequest(string id);
		void RemoveDevice(string id); 
	}
}
