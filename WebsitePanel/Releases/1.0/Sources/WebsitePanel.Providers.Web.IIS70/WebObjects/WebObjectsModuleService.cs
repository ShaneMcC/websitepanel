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

namespace WebsitePanel.Providers.Web.Iis.WebObjects
{
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    using Common;
    using Utility;
    using WebsitePanel.Providers.Utils;

    using Microsoft.Web.Management;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
	using WebsitePanel.Providers.Web.Iis.Authentication;

    internal sealed class WebObjectsModuleService : ConfigurationModuleService
    {
		public void ForceEnableAppPoolWow6432Mode(string poolName)
		{
			using (var srvman = GetServerManager())
			{
				var appPool = srvman.ApplicationPools[poolName];
				//
				if (Constants.X64Environment && appPool.Enable32BitAppOnWin64 == false)
				{
					appPool.Enable32BitAppOnWin64 = true;
					srvman.CommitChanges();
				}
			}
		}

		public void SetWebServerDefaultLoggingSettings(LogExtFileFlags svrLoggingFlags)
		{
			using (var srvman = GetServerManager())
			{
				// Update logging settings
				srvman.SiteDefaults.LogFile.LogExtFileFlags |= svrLoggingFlags;
				//
				srvman.CommitChanges();
			}
		}

		public void SetWebSiteLoggingSettings(WebSite webSite)
		{
			using (var srvman = GetServerManager())
			{
				var iisObject = srvman.Sites[webSite.SiteId];
				// Website logging is enabled by default
				iisObject.LogFile.Enabled = true;
				// Set website logs folder
				if (!String.IsNullOrEmpty(webSite.LogsPath))
					iisObject.LogFile.Directory = webSite.LogsPath;
				//
				srvman.CommitChanges();
			}
		}

		public string[] GrantConfigurationSectionAccess(string[] sections)
		{
			List<string> messages = new List<string>();
			//
			if (sections != null && sections.Length > 0)
			{
				foreach (string sectionName in sections)
				{
					try
					{
						string cmd = FileUtils.EvaluateSystemVariables(@"%windir%\system32\inetsrv\appcmd.exe");
						//
						FileUtils.ExecuteSystemCommand(cmd,
							String.Format("unlock config -section:{0}", sectionName));
					}
					catch (Exception ex)
					{
						messages.Add(String.Format("Could not unlock section '{0}'. Reason: {1}",
							sectionName, ex.StackTrace));
					}
				}
			}
			//
			return messages.ToArray();
		}

		public void ConfigureConnectAsFeature(WebVirtualDirectory virtualDir)
		{
			// read website
			using (var srvman = GetServerManager())
			{
				var webSite = String.IsNullOrEmpty(virtualDir.ParentSiteName) ? srvman.Sites[virtualDir.Name] : srvman.Sites[virtualDir.ParentSiteName];
				//
				if (webSite != null)
				{
					// get root iisAppObject
					var webApp = webSite.Applications[virtualDir.VirtualPath];
					//
					if (webApp != null)
					{
						var vdir = webApp.VirtualDirectories["/"];
						//
						if (vdir != null)
						{
							vdir.LogonMethod = AuthenticationLogonMethod.ClearText;
							//
							if (virtualDir.DedicatedApplicationPool)
							{
								var appPool = GetApplicationPool(virtualDir);
								vdir.UserName = appPool.ProcessModel.UserName;
								vdir.Password = appPool.ProcessModel.Password;
							}
							else
							{
								vdir.UserName = virtualDir.AnonymousUsername;
								vdir.Password = virtualDir.AnonymousUserPassword;
							}
							//
							srvman.CommitChanges();
						}
					}
				}
			}
		}

        public ApplicationPool GetApplicationPool(WebVirtualDirectory virtualDir)
        {
            if (virtualDir == null)
                throw new ArgumentNullException("vdir");
            // read app pool
			using (var srvman = GetServerManager())
			{
				var appPool = srvman.ApplicationPools[virtualDir.ApplicationPool];
				//
				if (appPool == null)
					throw new ApplicationException("ApplicationPoolNotFound");
				//
				return appPool;
			}
        }

        public void CreateApplicationPool(string appPoolName, string appPoolUsername, 
            string appPoolPassword, string runtimeVersion, bool enable32BitOnWin64, 
            ManagedPipelineMode pipelineMode)
        {
            // ensure app pool name specified
            if (String.IsNullOrEmpty(appPoolName))
				throw new ArgumentNullException("appPoolName");
            
			// Create iisAppObject pool
			using (var srvman = GetServerManager())
			{
				// ensure app pool unique
				if (srvman.ApplicationPools[appPoolName] != null)
					throw new Exception("ApplicationPoolAlreadyExists");

				var element = srvman.ApplicationPools.Add(appPoolName);
				//
				element.ManagedPipelineMode = pipelineMode;
				// ASP.NET 2.0 by default
				if (!String.IsNullOrEmpty(runtimeVersion))
					element.ManagedRuntimeVersion = runtimeVersion;
				//
				element.Enable32BitAppOnWin64 = enable32BitOnWin64;
				// set iisAppObject pool identity
				if (!String.IsNullOrEmpty(appPoolUsername))
				{
					element.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
					element.ProcessModel.UserName = appPoolUsername;
					element.ProcessModel.Password = appPoolPassword;
				}
				else
				{
					element.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
				}
				//
				element.AutoStart = true;
				//
				srvman.CommitChanges();
			}
        }

        public string CreateSite(WebSite site)
        {
            // ensure site bindings
            if (site.Bindings == null || site.Bindings.Length == 0)
                throw new ApplicationException("SiteServerBindingsEmpty");
            // ensure site name
            if (String.IsNullOrEmpty(site.Name))
				throw new ApplicationException("SiteNameEmpty");
            // ensure physical site content path
            if (String.IsNullOrEmpty(site.ContentPath))
				throw new ApplicationException("SiteContentPathEmpty");
			
			using (var srvman = GetServerManager())
			{
				//
				var iisObject = srvman.Sites.Add(site.Name, site.ContentPath, 80);
				//
				iisObject.Applications[0].ApplicationPoolName = site.ApplicationPool;
				//
				site.SiteId = iisObject.Name;
				//
				iisObject.ServerAutoStart = true;
				//
				srvman.CommitChanges();
				//
				return iisObject.Name;
			}
        }

        public void UpdateSite(WebSite site)
        {
			// ensure physical site content path
			if (String.IsNullOrEmpty(site.ContentPath))
				throw new Exception("SiteContentPathEmpty");
			//
			using (var srvman = GetServerManager())
			{
				//
				var iisObject = srvman.Sites[site.Name];
				//
				iisObject.Applications[0].ApplicationPoolName = site.ApplicationPool;
				//
				iisObject.Applications[0].VirtualDirectories[0].PhysicalPath = site.ContentPath;
				//
				iisObject.ServerAutoStart = true;
				//
				srvman.CommitChanges();
			}
        }

        public void UpdateVirtualDirectory(WebVirtualDirectory virtualDir)
        {
            // ensure physical site content path
            if (String.IsNullOrEmpty(virtualDir.ContentPath))
                throw new Exception("VirtualDirContentPathEmpty");
            //
			using (var srvman = GetServerManager())
			{
				// Obtain parent web site
				var webSite = srvman.Sites[virtualDir.ParentSiteName];
				// Ensure web site has been found
				if (webSite == null)
					throw new ApplicationException("WebSiteNotFound");
				//
				var v_dir = webSite.Applications[virtualDir.VirtualPath];
				v_dir.ApplicationPoolName = virtualDir.ApplicationPool;
				v_dir.VirtualDirectories[0].PhysicalPath = virtualDir.ContentPath;
				//
				srvman.CommitChanges();
			}
        }

        public void DeleteApplicationPools(params string[] appPoolNames)
        {
			using (var srvman = GetServerManager())
			{
				//
				foreach (var poolName in appPoolNames)
				{
					// Lookup for an app pool
					int indexOf = srvman.ApplicationPools.IndexOf(srvman.ApplicationPools[poolName]);
					// Remove app pool if it is found
					if (indexOf > -1)
						srvman.ApplicationPools.RemoveAt(indexOf);
				}
				//
				srvman.CommitChanges();
			}
        }

        public void ChangeSiteState(string siteId, ServerState state)
        {
			using (var srvman = GetServerManager())
			{
				var webSite = srvman.Sites[siteId];
				//
				if (webSite == null)
					return;
				//
				switch (state)
				{
					case ServerState.Continuing:
					case ServerState.Started:
						webSite.Start();
						webSite.ServerAutoStart = true;
						break;
					case ServerState.Stopped:
					case ServerState.Paused:
						webSite.Stop();
						webSite.ServerAutoStart = false;
						break;
				}
				//
				srvman.CommitChanges();
			}
        }

        public ServerState GetSiteState(string siteId)
        {
			using (var srvman = GetServerManager())
			{
				// ensure website exists
				if (srvman.Sites[siteId] == null)
					return ServerState.Unknown;
				//
				var siteState = ServerState.Unknown;
				//
				switch (srvman.Sites[siteId].State)
				{
					case ObjectState.Started:
						siteState = ServerState.Started;
						break;
					case ObjectState.Starting:
						siteState = ServerState.Starting;
						break;
					case ObjectState.Stopped:
						siteState = ServerState.Stopped;
						break;
					case ObjectState.Stopping:
						siteState = ServerState.Stopping;
						break;
				}
				//
				return siteState;
			}
        }

        public bool SiteExists(string siteId)
        {
			using (var srvman = GetServerManager())
			{
				return (srvman.Sites[siteId] != null);
			}
        }

        public string[] GetSites()
        {
			using (var srvman = GetServerManager())
			{
				var iisObjects = new List<string>();
				//
				foreach (var item in srvman.Sites)
					iisObjects.Add(item.Name);
				//
				return iisObjects.ToArray();
			}
        }

        public string GetWebSiteNameFromIIS(string siteName)
        {
			using (var srvman = GetServerManager())
			{
				if (srvman.Sites[siteName] != null)
					return srvman.Sites[siteName].Name;
				//
				return null;
			}
        }

		public string GetWebSiteIdFromIIS(string siteId, string format)
		{
			using (var srvman = GetServerManager())
			{
				var iisObject = srvman.Sites[siteId];
				// Format string is empty
				if (String.IsNullOrEmpty(format))
					return Convert.ToString(iisObject.Id);
				//
				return String.Format(format, iisObject.Id);
			}
		}

        public WebSite GetWebSiteFromIIS(string siteId)
        {
			using (var srvman = GetServerManager())
			{
				var webSite = new WebSite();
				//
				var iisObject = srvman.Sites[siteId];
				//
				webSite.SiteId = webSite.Name = iisObject.Name;
				//
				if (iisObject.LogFile.Enabled)
				{
					webSite.LogsPath = iisObject.LogFile.Directory;
					webSite[WebSite.IIS7_LOG_EXT_FILE_FIELDS] = iisObject.LogFile.LogExtFileFlags.ToString();
				}
				// Read instant website id
				webSite[WebSite.IIS7_SITE_ID] = GetWebSiteIdFromIIS(siteId, "W3SVC{0}");
				// Read web site iisAppObject pool name
				webSite.ApplicationPool = iisObject.Applications["/"].ApplicationPoolName;
				//
				return webSite;
			}
        }

        public ServerBinding[] GetSiteBindings(string siteId)
        {
			using (var srvman = GetServerManager())
			{
				var iisObject = srvman.Sites[siteId];
				// get server bingings
				var bindings = new List<ServerBinding>();
				//
				foreach (var bindingObj in iisObject.Bindings)
				{
					// TO-DO: skip <All Unassigned> bindings
					if (String.Equals(bindingObj.Protocol, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) &&
						!bindingObj.BindingInformation.StartsWith("*"))
					{
						string[] bunch = bindingObj.BindingInformation.Split(':');
						// append binding
						bindings.Add(new ServerBinding(bunch[0], bunch[1], bunch[2]));
					}
				}
				//
				return bindings.ToArray();
			}
        }

		private void SyncWebSiteBindingsChanges(string siteId, ServerBinding[] bindings)
		{
			// ensure site bindings
			if (bindings == null || bindings.Length == 0)
				throw new Exception("SiteServerBindingsEmpty");
			
			using (var srvman = GetServerManager())
			{
				var iisObject = srvman.Sites[siteId];
				//
				lock (((ICollection)iisObject.ChildElements).SyncRoot)
				{
					var itemsToRemove = new List<Binding>();
					// Determine HTTP bindings to remove
					foreach (Binding element in iisObject.Bindings)
					{
						if (String.Equals(element.Protocol, Uri.UriSchemeHttp))
						{
							itemsToRemove.Add(element);
						}
					}
					// Remove bindings
					while (itemsToRemove.Count > 0)
					{
						iisObject.Bindings.Remove(itemsToRemove[0]);
						itemsToRemove.RemoveAt(0);
					}
					// Create HTTP bindings received
					foreach (var serverBinding in bindings)
					{
						var bindingInformation = String.Format("{0}:{1}:{2}", serverBinding.IP, serverBinding.Port, serverBinding.Host);
						iisObject.Bindings.Add(bindingInformation, Uri.UriSchemeHttp);
					}
				}
				//
				srvman.CommitChanges();
			}
		}

		public void UpdateSiteBindings(string siteId, ServerBinding[] bindings)
		{
			// Ensure web site exists
			if (!SiteExists(siteId))
				return;
			//
			SyncWebSiteBindingsChanges(siteId, bindings);
		}

    	public string GetPhysicalPath(WebVirtualDirectory virtualDir)
        {
			using (var srvman = GetServerManager())
			{
				string siteId = (virtualDir.ParentSiteName == null) 
					? virtualDir.Name : virtualDir.ParentSiteName;
				//
				var iisObject = srvman.Sites[siteId];
				
				if (iisObject == null)
					return null;

				//
				var iisAppObject = iisObject.Applications[virtualDir.VirtualPath];

				if (iisAppObject == null)
					return null;

				//
				var iisDirObject = iisAppObject.VirtualDirectories["/"];

				if (iisDirObject == null)
					return null;

				//
				return iisDirObject.PhysicalPath;
			}
        }

		public void DeleteApplicationPool(params string[] appPoolNames)
		{
			using (var srvman = GetServerManager())
			{
				foreach (var item in appPoolNames)
				{
					var indexOf = srvman.ApplicationPools.IndexOf(srvman.ApplicationPools[item]);
					//
					if (indexOf > -1)
						srvman.ApplicationPools.RemoveAt(indexOf);
				}
				//
				srvman.CommitChanges();
			}
		}

    	public bool IsApplicationPoolExist(string poolName)
		{
			if (String.IsNullOrEmpty(poolName))
				throw new ArgumentNullException("poolName");
			//
			using (var srvman = GetServerManager())
			{
				return (srvman.ApplicationPools[poolName] != null);
			}
		}

    	public void DeleteSite(string siteId)
		{
			if (!SiteExists(siteId))
				return;
			//
			using (var srvman = GetServerManager())
			{
				//
				var indexOf = srvman.Sites.IndexOf(srvman.Sites[siteId]);
				srvman.Sites.RemoveAt(indexOf);
				//
				srvman.CommitChanges();
			}
		}

    	public WebVirtualDirectory[] GetVirtualDirectories(string siteId)
		{
			if (!SiteExists(siteId))
				return new WebVirtualDirectory[] { };

			using (var srvman = GetServerManager())
			{
				var vdirs = new List<WebVirtualDirectory>();
				var iisObject = srvman.Sites[siteId];
				//
				foreach (var item in iisObject.Applications)
				{
					// Skip root application which is web site itself
					if (item.Path == "/")
						continue;
					//
					vdirs.Add(new WebVirtualDirectory
					{
						Name = ConfigurationUtility.GetNonQualifiedVirtualPath(item.Path),
						ContentPath = item.VirtualDirectories[0].PhysicalPath
					});
				}
				//
				return vdirs.ToArray();
			}
		}

		public WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
		{
			//
			if (String.IsNullOrEmpty(siteId))
				throw new ArgumentNullException("siteId");
			//
			if (String.IsNullOrEmpty(directoryName))
				throw new ArgumentNullException("directoryName");
			//
			if (!SiteExists(siteId))
				return null;
			//
			using (var srvman = GetServerManager())
			{
				var site = srvman.Sites[siteId];
				//
				var vdir = new WebVirtualDirectory
				{
					Name = directoryName,
					ParentSiteName = siteId
				};
				// We assume that we create only applications.
				vdir.ApplicationPool = site.Applications[vdir.VirtualPath].ApplicationPoolName;
				//
				return vdir;
			}
		}

		public void CreateVirtualDirectory(string siteId, string directoryName, string physicalPath)
		{
			if (!SiteExists(siteId))
				throw new ApplicationException();
			//
			using (var srvman = GetServerManager())
			{
				var iisSiteObject = srvman.Sites[siteId];
				var iisAppObject = iisSiteObject.Applications.Add(directoryName, physicalPath);
				//
				srvman.CommitChanges();
			}
		}

    	public bool VirtualDirectoryExists(string siteId, string directoryName)
		{
			if (!SiteExists(siteId))
				return false;

			using (var srvman = GetServerManager())
			{
				return (srvman.Sites[siteId].Applications[directoryName] != null);
			}
		}

    	public void DeleteVirtualDirectory(WebVirtualDirectory virtualDir)
		{
			if (!SiteExists(virtualDir.ParentSiteName))
				return;
			//
			using (var srvman = GetServerManager())
			{
				var iisSiteObject = srvman.Sites[virtualDir.ParentSiteName];
				var iisAppObject = iisSiteObject.Applications[virtualDir.VirtualPath];
				//
				if (iisAppObject != null)
					iisSiteObject.Applications.Remove(iisAppObject);
				//
				srvman.CommitChanges();
			}
        }
    }
}
