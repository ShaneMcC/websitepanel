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
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.WebAppGallery;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Providers.Web
{
	/// <summary>
	/// Summary description for IWebServer.
	/// </summary>
	public interface IWebServer
	{
		// sites
		void ChangeSiteState(string siteId, ServerState state);
        ServerState GetSiteState(string siteId);
		bool SiteExists(string siteId);
		string[] GetSites();
        string GetSiteId(string siteName);
		WebSite GetSite(string siteId);
        string[] GetSitesAccounts(string[] siteIds);
        ServerBinding[] GetSiteBindings(string siteId);
		string CreateSite(WebSite site);
		void UpdateSite(WebSite site);
        void UpdateSiteBindings(string siteId, ServerBinding[] bindings);
		void DeleteSite(string siteId);

		// virtual directories
		bool VirtualDirectoryExists(string siteId, string directoryName);
		WebVirtualDirectory[] GetVirtualDirectories(string siteId);
		WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName);
		void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory);
		void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory);
		void DeleteVirtualDirectory(string siteId, string directoryName);

		// FrontPage
		bool IsFrontPageSystemInstalled();
		bool IsFrontPageInstalled(string siteId);
		bool InstallFrontPage(string siteId, string username, string password);
		void UninstallFrontPage(string siteId, string username);
        void ChangeFrontPagePassword(string username, string password);

        //ColdFusion
	    bool IsColdFusionSystemInstalled();
	    //bool IsColdFusionEnabled(string siteId);

        // permissions
        void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission);

        // Secured folders
        void InstallSecuredFolders(string siteId);
        void UninstallSecuredFolders(string siteId);

        List<WebFolder> GetFolders(string siteId);
        WebFolder GetFolder(string siteId, string folderPath);
        void UpdateFolder(string siteId, WebFolder folder);
        void DeleteFolder(string siteId, string folderPath);

        List<WebUser> GetUsers(string siteId);
        WebUser GetUser(string siteId, string userName);
        void UpdateUser(string siteId, WebUser user);
        void DeleteUser(string siteId, string userName);

        List<WebGroup> GetGroups(string siteId);
        WebGroup GetGroup(string siteId, string groupName);
        void UpdateGroup(string siteId, WebGroup group);
        void DeleteGroup(string siteId, string groupName);

		// web app gallery
		bool IsMsDeployInstalled();
		GalleryCategoriesResult GetGalleryCategories();
		GalleryApplicationsResult GetGalleryApplications(string categoryId);
		GalleryApplicationResult GetGalleryApplication(string id);
		GalleryWebAppStatus GetGalleryApplicationStatus(string id);
		GalleryWebAppStatus DownloadGalleryApplication(string id);
		DeploymentParametersResult GetGalleryApplicationParameters(string id);
		StringResultObject InstallGalleryApplication(string id, List<DeploymentParameter> updatedValues);

		//
		void GrantWebManagementAccess(string siteId, string accountName, string accountPassword);
		void RevokeWebManagementAccess(string siteId, string accountName);
		void ChangeWebManagementAccessPassword(string accountName, string accountPassword);
		bool CheckWebManagementAccountExists(string accountName);
		ResultObject CheckWebManagementPasswordComplexity(string accountPassword);
	}
}
