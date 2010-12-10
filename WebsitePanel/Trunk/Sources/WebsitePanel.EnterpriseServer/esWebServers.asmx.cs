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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;

using WebsitePanel.Providers;
using WebsitePanel.Providers.Web;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for esApplicationsInstaller
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/enterpriseserver")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class esWebServers : System.Web.Services.WebService
    {
        [WebMethod]
        public DataSet GetRawWebSitesPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return WebServerController.GetRawWebSitesPaged(packageId, filterColumn, filterValue,
                sortColumn, startRow, maximumRows);
        }

        [WebMethod]
        public List<WebSite> GetWebSites(int packageId, bool recursive)
        {
            return WebServerController.GetWebSites(packageId, recursive);
        }

        [WebMethod]
        public WebSite GetWebSite(int siteItemId)
        {
            return WebServerController.GetWebSite(siteItemId);
        }

        [WebMethod]
        public List<WebVirtualDirectory> GetVirtualDirectories(int siteItemId)
        {
            return WebServerController.GetVirtualDirectories(siteItemId);
        }

        [WebMethod]
        public WebVirtualDirectory GetVirtualDirectory(int siteItemId, string vdirName)
        {
            return WebServerController.GetVirtualDirectory(siteItemId, vdirName);
        }

        [WebMethod]
        public List<DomainInfo> GetWebSitePointers(int siteItemId)
        {
            return WebServerController.GetWebSitePointers(siteItemId);
        }

        [WebMethod]
        public int AddWebSitePointer(int siteItemId, int domainId)
        {
            return WebServerController.AddWebSitePointer(siteItemId, domainId);
        }

        [WebMethod]
        public int DeleteWebSitePointer(int siteItemId, int domainId)
        {
            return WebServerController.DeleteWebSitePointer(siteItemId, domainId);
        }

        [WebMethod]
        public int AddWebSite(int packageId, int domainId, int ipAddressId)
        {
            return WebServerController.AddWebSite(packageId, domainId, ipAddressId, true);
        }

        [WebMethod]
        public int AddVirtualDirectory(int siteItemId, string vdirName, string vdirPath, string aspNetVersion)
        {
            return WebServerController.AddVirtualDirectory(siteItemId, vdirName, vdirPath);
        }

        [WebMethod]
        public int UpdateWebSite(WebSite site)
        {
            return WebServerController.UpdateWebSite(site);
        }

        [WebMethod]
        public int InstallFrontPage(int siteItemId, string username, string password)
        {
            return WebServerController.InstallFrontPage(siteItemId, username, password);
        }

        [WebMethod]
        public int UninstallFrontPage(int siteItemId)
        {
            return WebServerController.UninstallFrontPage(siteItemId);
        }

        [WebMethod]
        public int ChangeFrontPagePassword(int siteItemId, string password)
        {
            return WebServerController.ChangeFrontPagePassword(siteItemId, password);
        }

        [WebMethod]
        public int RepairWebSite(int siteItemId)
        {
            return WebServerController.RepairWebSite(siteItemId);
        }

        [WebMethod]
        public int UpdateVirtualDirectory(int siteItemId, WebVirtualDirectory vdir)
        {
            return WebServerController.UpdateVirtualDirectory(siteItemId, vdir);
        }

        [WebMethod]
        public int DeleteWebSite(int siteItemId)
        {
            return WebServerController.DeleteWebSite(siteItemId);
        }

        [WebMethod]
        public int DeleteVirtualDirectory(int siteItemId, string vdirName)
        {
            return WebServerController.DeleteVirtualDirectory(siteItemId, vdirName);
        }

        [WebMethod]
        public int ChangeSiteState(int siteItemId, ServerState state)
        {
            return WebServerController.ChangeSiteState(siteItemId, state);
        }

        #region Shared SSL Folders
        [WebMethod]
        public List<string> GetSharedSSLDomains(int packageId)
        {
            return WebServerController.GetSharedSSLDomains(packageId);
        }

        [WebMethod]
        public DataSet GetRawSSLFoldersPaged(int packageId,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return WebServerController.GetRawSSLFoldersPaged(packageId, filterColumn, filterValue, sortColumn,
                startRow, maximumRows);
        }

        [WebMethod]
        public List<SharedSSLFolder> GetSharedSSLFolders(int packageId, bool recursive)
        {
            return WebServerController.GetSharedSSLFolders(packageId, recursive);
        }

        [WebMethod]
        public SharedSSLFolder GetSharedSSLFolder(int itemId)
        {
            return WebServerController.GetSharedSSLFolder(itemId);
        }

        [WebMethod]
        public int AddSharedSSLFolder(int packageId, string sslDomain, int siteId, string vdirName, string vdirPath)
        {
            return WebServerController.AddSharedSSLFolder(packageId, sslDomain, siteId, vdirName, vdirPath);
        }

        [WebMethod]
        public int UpdateSharedSSLFolder(SharedSSLFolder vdir)
        {
            return WebServerController.UpdateSharedSSLFolder(vdir);
        }

        [WebMethod]
        public int DeleteSharedSSLFolder(int itemId)
        {
            return WebServerController.DeleteSharedSSLFolder(itemId);
        }
        #endregion

        #region Secured Folders
        [WebMethod]
        public int InstallSecuredFolders(int siteItemId)
        {
            return WebServerController.InstallSecuredFolders(siteItemId);
        }

        [WebMethod]
        public int UninstallSecuredFolders(int siteItemId)
        {
            return WebServerController.UninstallSecuredFolders(siteItemId);
        }

        [WebMethod]
        public WebFolder[] GetSecuredFolders(int siteItemId)
        {
            return WebServerController.GetFolders(siteItemId);
        }

        [WebMethod]
        public WebFolder GetSecuredFolder(int siteItemId, string folderPath)
        {
            return WebServerController.GetFolder(siteItemId, folderPath);
        }

        [WebMethod]
        public int UpdateSecuredFolder(int siteItemId, WebFolder folder)
        {
            return WebServerController.UpdateFolder(siteItemId, folder);
        }

        [WebMethod]
        public int DeleteSecuredFolder(int siteItemId, string folderPath)
        {
            return WebServerController.DeleteFolder(siteItemId, folderPath);
        }
        #endregion

        #region Secured Users
        [WebMethod]
        public WebUser[] GetSecuredUsers(int siteItemId)
        {
            return WebServerController.GetUsers(siteItemId);
        }

        [WebMethod]
        public WebUser GetSecuredUser(int siteItemId, string userName)
        {
            return WebServerController.GetUser(siteItemId, userName);
        }

        [WebMethod]
        public int UpdateSecuredUser(int siteItemId, WebUser user)
        {
            return WebServerController.UpdateUser(siteItemId, user);
        }

        [WebMethod]
        public int DeleteSecuredUser(int siteItemId, string userName)
        {
            return WebServerController.DeleteUser(siteItemId, userName);
        }
        #endregion

        #region Secured Groups
        [WebMethod]
        public WebGroup[] GetSecuredGroups(int siteItemId)
        {
            return WebServerController.GetGroups(siteItemId);
        }

        [WebMethod]
        public WebGroup GetSecuredGroup(int siteItemId, string groupName)
        {
            return WebServerController.GetGroup(siteItemId, groupName);
        }

        [WebMethod]
        public int UpdateSecuredGroup(int siteItemId, WebGroup group)
        {
            return WebServerController.UpdateGroup(siteItemId, group);
        }

        [WebMethod]
        public int DeleteSecuredGroup(int siteItemId, string groupName)
        {
            return WebServerController.DeleteGroup(siteItemId, groupName);
        }
        #endregion

		#region WebManagement Access

		[WebMethod]
		public ResultObject GrantWebManagementAccess(int siteItemId, string accountName, string accountPassword)
		{
			return WebServerController.GrantWebManagementAccess(siteItemId, accountName, accountPassword);
		}

		[WebMethod]
		public void RevokeWebManagementAccess(int siteItemId)
		{
			WebServerController.RevokeWebManagementAccess(siteItemId);
		}

		[WebMethod]
		public ResultObject ChangeWebManagementAccessPassword(int siteItemId, string accountPassword)
		{
			return WebServerController.ChangeWebManagementAccessPassword(siteItemId, accountPassword);
		}
		
		#endregion

        #region SSL
        [WebMethod]
        public SSLCertificate CertificateRequest(SSLCertificate certificate,int siteItemId)
        {
            return WebServerController.CertificateRequest(certificate, siteItemId);
        }
        [WebMethod]
        public ResultObject InstallCertificate(SSLCertificate certificate, int siteItemId)
        {
            return WebServerController.InstallCertificate(certificate, siteItemId);
        }
        [WebMethod]
        public ResultObject InstallPfx(byte[] certificate, int siteItemId,string password)
        {
            return WebServerController.InstallPfx(certificate, siteItemId,password);
        }
        [WebMethod]
        public List<SSLCertificate> GetPendingCertificates(int siteItemId)
        {
            return WebServerController.GetPendingCertificates(siteItemId);
        }
        [WebMethod]
        public SSLCertificate GetSSLCertificateByID(int Id)
        {
            return WebServerController.GetSslCertificateById(Id);
        }
        [WebMethod]
        public SSLCertificate GetSiteCert(int siteID)
        {
            return WebServerController.GetSiteCert(siteID);
        }
        [WebMethod]
        public int CheckSSLForWebsite(int siteID,bool renewal)
        {
            return WebServerController.CheckSSL(siteID, renewal);
        }
        [WebMethod]
        public ResultObject CheckSSLForDomain(string domain,int siteID)
        {
            return WebServerController.CheckSSLForDomain(domain, siteID);
        }
        [WebMethod]
        public byte[] ExportCertificate(int siteId, string serialNumber, string password)
        {
            return WebServerController.ExportCertificate(siteId,serialNumber, password);
        }
        [WebMethod]
        public List<SSLCertificate> GetCertificatesForSite(int siteId)
        {
            return WebServerController.GetCertificatesForSite(siteId);
        }
        [WebMethod]
        public ResultObject DeleteCertificate(int siteId,SSLCertificate certificate)
        {
            return WebServerController.DeleteCertificate(siteId, certificate);
        }
        [WebMethod]
        public ResultObject ImportCertificate(int siteId)
        {
            return WebServerController.ImportCertificate(siteId);
        }
        [WebMethod]
        public ResultObject CheckCertificate(int siteId)
        {
            return WebServerController.CheckCertificate(siteId);
        }
        [WebMethod]
        public ResultObject DeleteCertificateRequest(int siteId,int csrID)
        {
            return WebServerController.DeleteCertificateRequest(siteId,csrID);
        }
		
		#endregion
    }
}
