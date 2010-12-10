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
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebsitePanel.Providers;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Server.Utils;
using Microsoft.Web.Services3;

namespace WebsitePanel.Server
{
	/// <summary>
	/// Summary description for ExchangeServer
	/// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class ExchangeServer : HostingServiceProviderWebService
    {
        private IExchangeServer ES
        {
			get { return (IExchangeServer)Provider; }
		}
         
		[WebMethod, SoapHeader("settings")]
		public bool CheckAccountCredentials(string username, string password)
		{
			try
			{
				LogStart("CheckAccountCredentials");
				bool ret = ES.CheckAccountCredentials(username, password);
				LogEnd("CheckAccountCredentials");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("CheckAccountCredentials", ex);
				throw;
			}
		}

		#region Organizations

		[WebMethod, SoapHeader("settings")]
        public Organization ExtendToExchangeOrganization(string organizationId, string securityGroup)
		{
			try
			{
                LogStart("ExtendToExchangeOrganization");
                Organization ret = ES.ExtendToExchangeOrganization(organizationId, securityGroup);
                LogEnd("ExtendToExchangeOrganization");
				return ret;
			}
			catch (Exception ex)
			{
                LogError("ExtendToExchangeOrganization", ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public string CreateMailEnableUser(string upn, string organizationId, string organizationDistinguishedName, ExchangeAccountType accountType, 
            string mailboxDatabase,  string offlineAddressBook,            
            string accountName, bool enablePOP, bool enableIMAP,
            bool enableOWA, bool enableMAPI, bool enableActiveSync,
            int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays)
        {
            try
            {
                LogStart("CreateMailEnableUser");
                string ret = ES.CreateMailEnableUser(upn, organizationId, organizationDistinguishedName,accountType,
                                                           mailboxDatabase, offlineAddressBook,
                                                           accountName, enablePOP, enableIMAP,
                                                           enableOWA, enableMAPI, enableActiveSync,
                                                           issueWarningKB, prohibitSendKB, prohibitSendReceiveKB,
                                                           keepDeletedItemsDays);
                LogEnd("CreateMailEnableUser");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("ExtendToExchangeOrganization", ex);
                throw;
            }
        }

		/// <summary>
		/// Creates organization OAB
		/// </summary>
		/// <param name="organizationId"></param>
		/// <param name="securityGroup"></param>
		/// <param name="oabVirtualDir"></param>
		/// <returns></returns>
		[WebMethod, SoapHeader("settings")]
		public Organization CreateOrganizationOfflineAddressBook(string organizationId, string securityGroup, string oabVirtualDir)
		{
			try
			{
				LogStart("CreateOrganizationOfflineAddressBook");
				Organization ret = ES.CreateOrganizationOfflineAddressBook(organizationId, securityGroup, oabVirtualDir);
				LogEnd("CreateOrganizationOfflineAddressBook");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("CreateOrganizationOfflineAddressBook", ex);
				throw;
			}
		}

		/// <summary>
		/// Updates organization OAB
		/// </summary>
		/// <param name="id"></param>
		[WebMethod, SoapHeader("settings")]
		public void UpdateOrganizationOfflineAddressBook(string id)
		{
			try
			{
				LogStart("UpdateOrganizationOfflineAddressBook");
				ES.UpdateOrganizationOfflineAddressBook(id);
				LogEnd("UpdateOrganizationOfflineAddressBook");
			}
			catch (Exception ex)
			{
				LogError("UpdateOrganizationOfflineAddressBook", ex);
				throw;
			}
		}


		[WebMethod, SoapHeader("settings")]
		public string GetOABVirtualDirectory()
		{
			try
			{
				LogStart("GetOABVirtualDirectory");
				string ret = ES.GetOABVirtualDirectory();
				LogEnd("GetOABVirtualDirectory");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetOABVirtualDirectory", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public bool DeleteOrganization(string organizationId, string distinguishedName, string globalAddressList, string addressList, string offlineAddressBook, string securityGroup)
		{
			try
			{
				LogStart("DeleteOrganization");
				bool ret = ES.DeleteOrganization(organizationId, distinguishedName, globalAddressList, addressList, offlineAddressBook, securityGroup);
				LogEnd("DeleteOrganization");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("DeleteOrganization", ex);
				throw;
			}
		}



		[WebMethod, SoapHeader("settings")]
		public void SetOrganizationStorageLimits(string organizationDistinguishedName, int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays)
		{
			try
			{
				LogStart("SetOrganizationStorageLimits");
				ES.SetOrganizationStorageLimits(organizationDistinguishedName, issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
				LogEnd("SetOrganizationStorageLimits");
			}
			catch (Exception ex)
			{
				LogError("SetOrganizationStorageLimits", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeItemStatistics[] GetMailboxesStatistics(string organizationDistinguishedName)
		{
			try
			{
				LogStart("GetMailboxesStatistics");
				ExchangeItemStatistics[] ret = ES.GetMailboxesStatistics(organizationDistinguishedName);
				LogEnd("GetMailboxesStatistics");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMailboxesStatistics", ex);
				throw;
			}
		}
		#endregion

		#region Domains
		[WebMethod, SoapHeader("settings")]
        public void AddAuthoritativeDomain(string domain)
		{
			try
			{
                LogStart("AddAuthoritativeDomain");
                ES.AddAuthoritativeDomain(domain);
                LogEnd("AddAuthoritativeDomain");
			}
			catch (Exception ex)
			{
                LogError("AddAuthoritativeDomain", ex);
				throw;
			}
		}


        [WebMethod, SoapHeader("settings")]
        public string[] GetAuthoritativeDomains()
        {
            try
            {
                LogStart("GetAuthoritativeDomains");
                string []ret = ES.GetAuthoritativeDomains();
                LogEnd("GetAuthoritativeDomains");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetAuthoritativeDomain", ex);
                throw;
            }
        }

		[WebMethod, SoapHeader("settings")]
        public void DeleteAuthoritativeDomain(string domain)
		{
			try
			{
                LogStart("DeleteAuthoritativeDomain");
                ES.DeleteAuthoritativeDomain(domain);
                LogEnd("DeleteAuthoritativeDomain");
			}
			catch (Exception ex)
			{
                LogError("DeleteAuthoritativeDomain", ex);
				throw;
			}
		}
		#endregion

		#region Mailboxes
		[WebMethod, SoapHeader("settings")]
		public string CreateMailbox(string organizationId, string organizationDistinguishedName, string mailboxDatabase,
			string securityGroup, string offlineAddressBook, ExchangeAccountType accountType,
			string displayName, string accountName, string name,
			string domain, string password, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync,
			int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays)
		{
			try
			{
				LogStart("CreateMailbox");
				string ret = ES.CreateMailbox(organizationId, organizationDistinguishedName, mailboxDatabase, securityGroup,
					offlineAddressBook, accountType,
					displayName, accountName, name, domain, password, enablePOP, enableIMAP,
					enableOWA, enableMAPI, enableActiveSync,
					issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
				LogEnd("CreateMailbox");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("CreateMailbox", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void DeleteMailbox(string accountName)
		{
			try
			{
				LogStart("DeleteMailbox");
				ES.DeleteMailbox(accountName);
				LogEnd("DeleteMailbox");
			}
			catch (Exception ex)
			{
				LogError("DeleteMailbox", ex);
				throw;
			}
		}


        [WebMethod, SoapHeader("settings")]
        public void DisableMailbox(string accountName)
        {
            try
            {
                LogStart("DisableMailbox");
                ES.DisableMailbox(accountName);
                LogEnd("DisableMailbox");
            }
            catch (Exception ex)
            {
                LogError("DisableMailbox", ex);
                throw;
            }
        }

		[WebMethod, SoapHeader("settings")]
		public ExchangeMailbox GetMailboxGeneralSettings(string accountName)
		{
			try
			{
				LogStart("GetMailboxGeneralSettings");
				ExchangeMailbox ret = ES.GetMailboxGeneralSettings(accountName);
				LogEnd("GetMailboxGeneralSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMailboxGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetMailboxGeneralSettings(string accountName, string displayName, string password, bool hideFromAddressBook, bool disabled, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes)
		{
			try
			{
				LogStart("SetMailboxGeneralSettings");
				ES.SetMailboxGeneralSettings(accountName, displayName, password, hideFromAddressBook, disabled, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes);
				LogEnd("SetMailboxGeneralSettings");
			}
			catch (Exception ex)
			{
				LogError("SetMailboxGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeMailbox GetMailboxMailFlowSettings(string accountName)
		{
			try
			{
				LogStart("GetMailboxMailFlowSettings");
				ExchangeMailbox ret = ES.GetMailboxMailFlowSettings(accountName);
				LogEnd("GetMailboxMailFlowSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMailboxMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetMailboxMailFlowSettings(string accountName, bool enableForwarding, string forwardingAccountName, bool forwardToBoth, string[] sendOnBehalfAccounts, string[] acceptAccounts, string[] rejectAccounts, int maxRecipients, int maxSendMessageSizeKB, int maxReceiveMessageSizeKB, bool requireSenderAuthentication)
		{
			try
			{
				LogStart("SetMailboxMailFlowSettings");
				ES.SetMailboxMailFlowSettings(accountName, enableForwarding, forwardingAccountName, forwardToBoth, sendOnBehalfAccounts, acceptAccounts, rejectAccounts, maxRecipients, maxSendMessageSizeKB, maxReceiveMessageSizeKB, requireSenderAuthentication);
				LogEnd("SetMailboxMailFlowSettings");
			}
			catch (Exception ex)
			{
				LogError("SetMailboxMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeMailbox GetMailboxAdvancedSettings(string accountName)
		{
			try
			{
				LogStart("GetMailboxAdvancedSettings");
				ExchangeMailbox ret = ES.GetMailboxAdvancedSettings(accountName);
				LogEnd("GetMailboxAdvancedSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMailboxAdvancedSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetMailboxAdvancedSettings(string organizationId, string accountName, bool enablePOP, bool enableIMAP, bool enableOWA, bool enableMAPI, bool enableActiveSync,
			int issueWarningKB, int prohibitSendKB, int prohibitSendReceiveKB, int keepDeletedItemsDays)
		{
			try
			{
				LogStart("SetMailboxAdvancedSettings");
				ES.SetMailboxAdvancedSettings(organizationId, accountName, enablePOP, enableIMAP, enableOWA, enableMAPI, enableActiveSync,
					issueWarningKB, prohibitSendKB, prohibitSendReceiveKB, keepDeletedItemsDays);
				LogEnd("SetMailboxAdvancedSettings");
			}
			catch (Exception ex)
			{
				LogError("SetMailboxAdvancedSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeEmailAddress[] GetMailboxEmailAddresses(string accountName)
		{
			try
			{
				LogStart("GetMailboxEmailAddresses");
				ExchangeEmailAddress[] ret = ES.GetMailboxEmailAddresses(accountName);
				LogEnd("GetMailboxEmailAddresses");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMailboxEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetMailboxEmailAddresses(string accountName, string[] emailAddresses)
		{
			try
			{
				LogStart("SetMailboxEmailAddresses");
				ES.SetMailboxEmailAddresses(accountName, emailAddresses);
				LogEnd("SetMailboxEmailAddresses");
			}
			catch (Exception ex)
			{
				LogError("SetMailboxEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetMailboxPrimaryEmailAddress(string accountName, string emailAddress)
		{
			try
			{
				LogStart("SetMailboxPrimaryEmailAddress");
				ES.SetMailboxPrimaryEmailAddress(accountName, emailAddress);
				LogEnd("SetMailboxPrimaryEmailAddress");
			}
			catch (Exception ex)
			{
				LogError("SetMailboxPrimaryEmailAddress", ex);
				throw;
			}
		}


        [WebMethod, SoapHeader("settings")]
		public void SetMailboxPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] fullAccessAccounts)
        {
            try
            {
                LogStart("SetMailboxPermissions");
				ES.SetMailboxPermissions(organizationId, accountName, sendAsAccounts, fullAccessAccounts);
                LogEnd("SetMailboxPermissions");
            }
            catch (Exception ex)
            {
                LogError("SetMailboxPermissions", ex);
                throw;
            }
        }
        
        

        [WebMethod, SoapHeader("settings")]
		public ExchangeMailbox GetMailboxPermissions(string organizationId, string accountName)
        {
            try
            {
                LogStart("GetMailboxPermissions");
				ExchangeMailbox ret = ES.GetMailboxPermissions(organizationId, accountName);
                LogEnd("GetMailboxPermissions");
                return ret;
            }
            catch (Exception ex)
            {
                LogError("GetMailboxPermissions", ex);
                throw;
            }
        }

		[WebMethod, SoapHeader("settings")]
		public ExchangeMailboxStatistics GetMailboxStatistics(string accountName)
		{
			try
			{
				LogStart("GetMailboxStatistics");
				ExchangeMailboxStatistics ret = ES.GetMailboxStatistics(accountName);
				LogEnd("GetMailboxStatistics");
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError("GetMailboxStatistics", ex);
				throw;
			}
		}

        #endregion

		#region Contacts
		[WebMethod, SoapHeader("settings")]
        public void CreateContact(string organizationId, string organizationDistinguishedName, string contactDisplayName, string contactAccountName, string contactEmail, string defaultOrganizationDomain)
		{
			try
			{
				LogStart("CreateContact");
                ES.CreateContact(organizationId, organizationDistinguishedName, contactDisplayName, contactAccountName, contactEmail, defaultOrganizationDomain);
				LogEnd("CreateContact");
			}
			catch (Exception ex)
			{
				LogError("CreateContact", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void DeleteContact(string accountName)
		{
			try
			{
				LogStart("DeleteContact");
				ES.DeleteContact(accountName);
				LogEnd("DeleteContact");
			}
			catch (Exception ex)
			{
				LogError("DeleteContact", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
		public ExchangeContact GetContactGeneralSettings(string accountName)
		{
			try
			{
				LogStart("GetContactGeneralSettings");
				ExchangeContact ret = ES.GetContactGeneralSettings(accountName);
				LogEnd("GetContactGeneralSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetContactGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
        public void SetContactGeneralSettings(string accountName, string displayName, string email, bool hideFromAddressBook, string firstName, string initials, string lastName, string address, string city, string state, string zip, string country, string jobTitle, string company, string department, string office, string managerAccountName, string businessPhone, string fax, string homePhone, string mobilePhone, string pager, string webPage, string notes, int useMapiRichTextFormat, string defaultDomain)
		{
			try
			{
				LogStart("SetContactGeneralSettings");
				ES.SetContactGeneralSettings(accountName, displayName, email, hideFromAddressBook, firstName, initials, lastName, address, city, state, zip, country, jobTitle, company, department, office, managerAccountName, businessPhone, fax, homePhone, mobilePhone, pager, webPage, notes, useMapiRichTextFormat, defaultDomain);
				LogEnd("SetContactGeneralSettings");
			}
			catch (Exception ex)
			{
				LogError("SetContactGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeContact GetContactMailFlowSettings(string accountName)
		{
			try
			{
				LogStart("GetContactMailFlowSettings");
				ExchangeContact ret = ES.GetContactMailFlowSettings(accountName);
				LogEnd("GetContactMailFlowSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetContactMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetContactMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			try
			{
				LogStart("SetContactMailFlowSettings");
				ES.SetContactMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
				LogEnd("SetContactMailFlowSettings");
			}
			catch (Exception ex)
			{
				LogError("SetContactMailFlowSettings", ex);
				throw;
			}
		}
		#endregion

		#region Distribution Lists
		[WebMethod, SoapHeader("settings")]
		public void CreateDistributionList(string organizationId, string organizationDistinguishedName, string displayName, string accountName, string name, string domain, string managedBy)
		{
			try
			{
				LogStart("CreateDistributionList");
				ES.CreateDistributionList(organizationId, organizationDistinguishedName, displayName, accountName, name, domain, managedBy);
				LogEnd("CreateDistributionList");
			}
			catch (Exception ex)
			{
				LogError("CreateDistributionList", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
		public void DeleteDistributionList(string accountName)
		{
			try
			{
				LogStart("DeleteDistributionList");
				ES.DeleteDistributionList(accountName);
				LogEnd("DeleteDistributionList");
			}
			catch (Exception ex)
			{
				LogError("DeleteDistributionList", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeDistributionList GetDistributionListGeneralSettings(string accountName)
		{
			try
			{
				LogStart("GetDistributionListGeneralSettings");
				ExchangeDistributionList ret = ES.GetDistributionListGeneralSettings(accountName);
				LogEnd("GetDistributionListGeneralSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetDistributionListGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetDistributionListGeneralSettings(string accountName, string displayName, bool hideFromAddressBook, string managedBy, string[] members, string notes)
		{
			try
			{
				LogStart("SetDistributionListGeneralSettings");
				ES.SetDistributionListGeneralSettings(accountName, displayName, hideFromAddressBook, managedBy, members, notes);
				LogEnd("SetDistributionListGeneralSettings");
			}
			catch (Exception ex)
			{
				LogError("SetDistributionListGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeDistributionList GetDistributionListMailFlowSettings(string accountName)
		{
			try
			{
				LogStart("GetDistributionListMailFlowSettings");
				ExchangeDistributionList ret = ES.GetDistributionListMailFlowSettings(accountName);
				LogEnd("GetDistributionListMailFlowSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetDistributionListMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetDistributionListMailFlowSettings(string accountName, string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			try
			{
				LogStart("SetDistributionListMailFlowSettings");
				ES.SetDistributionListMailFlowSettings(accountName, acceptAccounts, rejectAccounts, requireSenderAuthentication);
				LogEnd("SetDistributionListMailFlowSettings");
			}
			catch (Exception ex)
			{
				LogError("SetDistributionListMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeEmailAddress[] GetDistributionListEmailAddresses(string accountName)
		{
			try
			{
				LogStart("GetDistributionListEmailAddresses");
				ExchangeEmailAddress[] ret = ES.GetDistributionListEmailAddresses(accountName);
				LogEnd("GetDistributionListEmailAddresses");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetDistributionListEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetDistributionListEmailAddresses(string accountName, string[] emailAddresses)
		{
			try
			{
				LogStart("SetDistributionListEmailAddresses");
				ES.SetDistributionListEmailAddresses(accountName, emailAddresses);
				LogEnd("SetDistributionListEmailAddresses");
			}
			catch (Exception ex)
			{
				LogError("SetDistributionListEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetDistributionListPrimaryEmailAddress(string accountName, string emailAddress)
		{
			try
			{
				LogStart("SetDistributionListPrimaryEmailAddress");
				ES.SetDistributionListPrimaryEmailAddress(accountName, emailAddress);
				LogEnd("SetDistributionListPrimaryEmailAddress");
			}
			catch (Exception ex)
			{
				LogError("SetDistributionListPrimaryEmailAddress", ex);
				throw;
			}
		}

        [WebMethod, SoapHeader("settings")]
        public void SetDistributionListPermissions(string organizationId, string accountName, string[] sendAsAccounts, string[] sendOnBehalfAccounts)
        {
            ES.SetDistributionListPermissions(organizationId, accountName, sendAsAccounts, sendOnBehalfAccounts);
        }
		
        [WebMethod, SoapHeader("settings")]
        public ExchangeDistributionList GetDistributionListPermissions(string organizationId, string accountName)
        {
            return ES.GetDistributionListPermissions(organizationId, accountName);
        }
        
        #endregion

		
        
        #region Public Folders
		[WebMethod, SoapHeader("settings")]
		public void CreatePublicFolder(string organizationId, string securityGroup, string parentFolder,
			string folderName, bool mailEnabled, string accountName, string name, string domain)
		{
			try
			{
				LogStart("CreatePublicFolder");
				ES.CreatePublicFolder(organizationId, securityGroup, parentFolder, folderName,
					mailEnabled, accountName, name, domain);

				LogEnd("CreatePublicFolder");
			}
			catch (Exception ex)
			{
				LogError("CreatePublicFolder", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
		public void DeletePublicFolder(string folder)
		{
			try
			{
				LogStart("DeletePublicFolder");
				ES.DeletePublicFolder(folder);
				LogEnd("DeletePublicFolder");
			}
			catch (Exception ex)
			{
				LogError("DeletePublicFolder", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void EnableMailPublicFolder(string organizationId, string folder, string accountName,
			string name, string domain)
		{
			try
			{
				LogStart("EnableMailPublicFolder");
				ES.EnableMailPublicFolder(organizationId, folder, accountName, name, domain);
				LogEnd("EnableMailPublicFolder");
			}
			catch (Exception ex)
			{
				LogError("EnableMailPublicFolder", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void DisableMailPublicFolder(string folder)
		{
			try
			{
				LogStart("DisableMailPublicFolder");
				ES.DisableMailPublicFolder(folder);
				LogEnd("DisableMailPublicFolder");
			}
			catch (Exception ex)
			{
				LogError("DisableMailPublicFolder", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangePublicFolder GetPublicFolderGeneralSettings(string folder)
		{
			try
			{
				LogStart("GetPublicFolderGeneralSettings");
				ExchangePublicFolder ret = ES.GetPublicFolderGeneralSettings(folder);
				LogEnd("GetPublicFolderGeneralSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFolderGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetPublicFolderGeneralSettings(string folder, string newFolderName,
			string[] authorAccounts, bool hideFromAddressBook)
		{
			try
			{
				LogStart("SetPublicFolderGeneralSettings");
				ES.SetPublicFolderGeneralSettings(folder, newFolderName, authorAccounts, hideFromAddressBook);
				LogEnd("SetPublicFolderGeneralSettings");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderGeneralSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangePublicFolder GetPublicFolderMailFlowSettings(string folder)
		{
			try
			{
				LogStart("GetPublicFolderMailFlowSettings");
				ExchangePublicFolder ret = ES.GetPublicFolderMailFlowSettings(folder);
				LogEnd("GetPublicFolderMailFlowSettings");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFolderMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetPublicFolderMailFlowSettings(string folder,
			string[] acceptAccounts, string[] rejectAccounts, bool requireSenderAuthentication)
		{
			try
			{
				LogStart("SetPublicFolderMailFlowSettings");
				ES.SetPublicFolderMailFlowSettings(folder, acceptAccounts, rejectAccounts, requireSenderAuthentication);
				LogEnd("SetPublicFolderMailFlowSettings");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderMailFlowSettings", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeEmailAddress[] GetPublicFolderEmailAddresses(string folder)
		{
			try
			{
				LogStart("GetPublicFolderEmailAddresses");
				ExchangeEmailAddress[] ret = ES.GetPublicFolderEmailAddresses(folder);
				LogEnd("GetPublicFolderEmailAddresses");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFolderEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetPublicFolderEmailAddresses(string folder, string[] emailAddresses)
		{
			try
			{
				LogStart("SetPublicFolderEmailAddresses");
				ES.SetPublicFolderEmailAddresses(folder, emailAddresses);
				LogEnd("SetPublicFolderEmailAddresses");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderEmailAddresses", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetPublicFolderPrimaryEmailAddress(string folder, string emailAddress)
		{
			try
			{
				LogStart("SetPublicFolderPrimaryEmailAddress");
				ES.SetPublicFolderPrimaryEmailAddress(folder, emailAddress);
				LogEnd("SetPublicFolderPrimaryEmailAddress");
			}
			catch (Exception ex)
			{
				LogError("SetPublicFolderPrimaryEmailAddress", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
		public ExchangeItemStatistics[] GetPublicFoldersStatistics(string[] folders)
		{
			try
			{
				LogStart("GetPublicFoldersStatistics");
				ExchangeItemStatistics[] ret = ES.GetPublicFoldersStatistics(folders);
				LogEnd("GetPublicFoldersStatistics");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFoldersStatistics", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public string[] GetPublicFoldersRecursive(string parent)
		{
			try
			{
				LogStart("GetPublicFoldersRecursive");
				string[] ret = ES.GetPublicFoldersRecursive(parent);
				LogEnd("GetPublicFoldersRecursive");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetPublicFoldersRecursive", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public long GetPublicFolderSize(string folder)
		{
			try
			{
				LogStart("GetPublicFolderSize");
				long ret = ES.GetPublicFolderSize(folder);
				LogEnd("GetPublicFolderSize");
				return ret;
			}
			catch (Exception ex)
			{
				Log.WriteError("GetPublicFolderSize", ex);
				throw;
			}
		}
		
        
        #endregion

		#region ActiveSync
		[WebMethod, SoapHeader("settings")]
		public void CreateOrganizationActiveSyncPolicy(string organizationId)
		{
			try
			{
				LogStart("CreateOrganizationActiveSyncPolicy");
				ES.CreateOrganizationActiveSyncPolicy(organizationId);
				LogEnd("CreateOrganizationActiveSyncPolicy");
			}
			catch (Exception ex)
			{
				LogError("CreateOrganizationActiveSyncPolicy", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeActiveSyncPolicy GetActiveSyncPolicy(string organizationId)
		{
			try
			{
				LogStart("GetActiveSyncPolicy");
				ExchangeActiveSyncPolicy ret = ES.GetActiveSyncPolicy(organizationId);
				LogEnd("GetActiveSyncPolicy");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetActiveSyncPolicy", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void SetActiveSyncPolicy(string id, bool allowNonProvisionableDevices, bool attachmentsEnabled,
			int maxAttachmentSizeKB, bool uncAccessEnabled, bool wssAccessEnabled, bool devicePasswordEnabled,
			bool alphanumericPasswordRequired, bool passwordRecoveryEnabled, bool deviceEncryptionEnabled,
			bool allowSimplePassword, int maxPasswordFailedAttempts, int minPasswordLength, int inactivityLockMin,
			int passwordExpirationDays, int passwordHistory, int refreshInterval)
		{
			try
			{
				LogStart("SetActiveSyncPolicy");
				ES.SetActiveSyncPolicy(id, allowNonProvisionableDevices, attachmentsEnabled,
					maxAttachmentSizeKB, uncAccessEnabled, wssAccessEnabled, devicePasswordEnabled, alphanumericPasswordRequired, passwordRecoveryEnabled,
					deviceEncryptionEnabled, allowSimplePassword, maxPasswordFailedAttempts,
					minPasswordLength, inactivityLockMin, passwordExpirationDays, passwordHistory, refreshInterval);
				LogEnd("SetActiveSyncPolicy");
			}
			catch (Exception ex)
			{
				LogError("SetActiveSyncPolicy", ex);
				throw;
			}
		}
		#endregion

		#region Mobile devices
		[WebMethod, SoapHeader("settings")]
		public ExchangeMobileDevice[] GetMobileDevices(string accountName)
		{
			try
			{
				LogStart("GetMobileDevices");
				ExchangeMobileDevice[] ret = ES.GetMobileDevices(accountName);
				LogEnd("GetMobileDevices");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMobileDevices", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public ExchangeMobileDevice GetMobileDevice(string id)
		{
			try
			{
				LogStart("GetMobileDevice");
				ExchangeMobileDevice ret = ES.GetMobileDevice(id);
				LogEnd("GetMobileDevice");
				return ret;
			}
			catch (Exception ex)
			{
				LogError("GetMobileDevice", ex);
				throw;
			}
		}
		
		[WebMethod, SoapHeader("settings")]
		public void WipeDataFromDevice(string id)
		{
			try
			{
				LogStart("WipeDataFromDevice");
				ES.WipeDataFromDevice(id);
				LogEnd("WipeDataFromDevice");
			}
			catch (Exception ex)
			{
				LogError("WipeDataFromDevice", ex);
				throw;
			}
		}

		[WebMethod, SoapHeader("settings")]
		public void CancelRemoteWipeRequest(string id)
		{
			try
			{
				LogStart("CancelRemoteWipeRequest");
				ES.CancelRemoteWipeRequest(id);
				LogEnd("CancelRemoteWipeRequest");
			}
			catch (Exception ex)
			{
				LogError("CancelRemoteWipeRequest", ex);
				throw;
			}
		}


		[WebMethod, SoapHeader("settings")]
		public void RemoveDevice(string id)
		{
			try
			{
				LogStart("RemoveDevice");
				ES.RemoveDevice(id);
				LogEnd("RemoveDevice");
			}
			catch (Exception ex)
			{
				LogError("RemoveDevice", ex);
				throw;
			}
		}
		#endregion
		
		protected void LogStart(string func)
		{
			Log.WriteStart("'{0}' {1}", ProviderSettings.ProviderName, func);
		}

		protected void LogEnd(string func)
		{
			Log.WriteEnd("'{0}' {1}", ProviderSettings.ProviderName, func);
		}

		protected void LogError(string func, Exception ex)
		{
			Log.WriteError(String.Format("'{0}' {1}", ProviderSettings.ProviderName, func), ex);
		}
 
	}
}
