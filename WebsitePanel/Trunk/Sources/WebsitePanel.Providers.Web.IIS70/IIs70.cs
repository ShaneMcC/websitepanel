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
using System.Collections;
using System.Management;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WebsitePanel.Providers.OS;
using WebsitePanel.Providers.Utils;
using WebsitePanel.Providers.Web.Handlers;
using WebsitePanel.Providers.Web.HttpRedirect;
using WebsitePanel.Providers.Web.Iis.Authentication;
using WebsitePanel.Providers.Web.Iis.ClassicAsp;
using WebsitePanel.Providers.Web.Iis.DefaultDocuments;
using WebsitePanel.Providers.Web.Iis.DirectoryBrowse;
using WebsitePanel.Providers.Web.Iis.WebObjects;
using WebsitePanel.Providers.Web.Iis.Extensions;
using WebsitePanel.Providers.Web.MimeTypes;
using WebsitePanel.Providers.Web.Iis.Utility;
using WebsitePanel.Server.Utils;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

using WebsitePanel.Providers.Utils.LogParser;
using Microsoft.Win32;
using WebsitePanel.Providers.Common;
using System.Collections.Specialized;
using WebsitePanel.Providers.Web.WebObjects;
using WebsitePanel.Providers.Web.Iis.Common;

namespace WebsitePanel.Providers.Web
{
	internal abstract class Constants
	{
		public const string CachingSection = "system.webServer/caching";
		public const string DefaultDocumentsSection = "system.webServer/defaultDocument";
		public const string DirectoryBrowseSection = "system.webServer/directoryBrowse";
		public const string FactCgiSection = "system.webServer/fastCgi";
		public const string HandlersSection = "system.webServer/handlers";
		public const string HttpErrorsSection = "system.webServer/httpErrors";
		public const string HttpProtocolSection = "system.webServer/httpProtocol";
		public const string HttpRedirectSection = "system.webServer/httpRedirect";
		public const string AnonymousAuthenticationSection = "system.webServer/security/authentication/anonymousAuthentication";
		public const string BasicAuthenticationSection = "system.webServer/security/authentication/basicAuthentication";
		public const string WindowsAuthenticationSection = "system.webServer/security/authentication/windowsAuthentication";
		public const string StaticContentSection = "system.webServer/staticContent";
		public const string ModulesSection = "system.webServer/modules";
		public const string IsapiCgiRestrictionSection = "system.webServer/security/isapiCgiRestriction";

		public const string APP_POOL_NAME_FORMAT_STRING = "#SITE-NAME# #IIS7-ASPNET-VERSION# (#PIPELINE-MODE#)";

		public const string AspNet11Pool = "AspNet11Pool";
		public const string ClassicAspNet20Pool = "ClassicAspNet20Pool";
		public const string IntegratedAspNet20Pool = "IntegratedAspNet20Pool";
		public const string ClassicAspNet40Pool = "ClassicAspNet40Pool";
		public const string IntegratedAspNet40Pool = "IntegratedAspNet40Pool";

		public const string AspPathSetting = "AspPath";
		public const string AspNet11PathSetting = "AspNet11Path";
		public const string AspNet20PathSetting = "AspNet20Path";
		public const string AspNet20x64PathSetting = "AspNet20x64Path";
		public const string AspNet40PathSetting = "AspNet40Path";
		public const string AspNet40x64PathSetting = "AspNet40x64Path";

		public const string WEBSITEPANEL_IISMODULES = "WebsitePanel.IIsModules";

		public const string IsapiModule = "IsapiModule";
		public const string FastCgiModule = "FastCgiModule";
		public const string CgiModule = "CgiModule";

		internal abstract class PhpMode
		{
			public const string FastCGI = "FastCGI";
			public const string CGI = "CGI";
			public const string ISAPI = "ISAPI";
		}

		internal abstract class HandlerAlias
		{
			public const string CLASSIC_ASP = "ASPClassic (*{0})";
			public const string PHP_FASTCGI = "PHP via FastCGI (*{0})";
			public const string PHP_CGI = "PHP via CGI (*{0})";
			public const string PERL_ISAPI = "Perl via ISAPI (*{0})";
			public const string PHP_ISAPI = "PHP via ISAPI (*{0})";
			public const string COLDFUSION = "ColdFusion (*{0})";
		}

		public static bool X64Environment
		{
			get { return (IntPtr.Size == 8); }
		}
	}

	public class WebAppPool
	{
		public static readonly Dictionary<SiteAppPoolMode, string> AspNetVersions = new Dictionary<SiteAppPoolMode, string>
		{
			{ SiteAppPoolMode.dotNetFramework1, "v1.1" },
			{ SiteAppPoolMode.dotNetFramework2, "v2.0" },
			{ SiteAppPoolMode.dotNetFramework4, "v4.0" }
		};

		public string AspNetInstalled { get; set; }
		public SiteAppPoolMode Mode { get; set; }
		public string Name { get; set; }
	}

	public class WebAppPoolHelper
	{
		private List<WebAppPool> supportedAppPools;

		public List<WebAppPool> SupportedAppPools
		{
			get
			{
				return supportedAppPools;
			}
		}

		public static Dictionary<string, SiteAppPoolMode> SupportedAppPoolModes = new Dictionary<string, SiteAppPoolMode>
		{
			{ "1",	SiteAppPoolMode.dotNetFramework1 | SiteAppPoolMode.Classic },
			{ "2",	SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.Classic },
			{ "2I", SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.Integrated },
			{ "4",	SiteAppPoolMode.dotNetFramework4 | SiteAppPoolMode.Classic },
			{ "4I", SiteAppPoolMode.dotNetFramework4 | SiteAppPoolMode.Integrated }
		};

		//
		public WebAppPoolHelper(ServiceProviderSettings s)
		{
			SetupSupportedAppPools(s);
		}
		//
		public SiteAppPoolMode dotNetVersion(SiteAppPoolMode m)
		{
			return m & (SiteAppPoolMode.dotNetFramework1 | SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.dotNetFramework4);
		}
		//
		public SiteAppPoolMode pipeline(SiteAppPoolMode m)
		{
			return m & (SiteAppPoolMode.Classic | SiteAppPoolMode.Integrated);
		}
		//
		public SiteAppPoolMode isolation(SiteAppPoolMode m)
		{
			return m & (SiteAppPoolMode.Shared | SiteAppPoolMode.Dedicated);
		}
		//
		public string aspnet_runtime(SiteAppPoolMode m)
		{
			return WebAppPool.AspNetVersions[dotNetVersion(m)];
		}
		//
		public ManagedPipelineMode runtime_pipeline(SiteAppPoolMode m)
		{
			return (pipeline(m) == SiteAppPoolMode.Classic) ? ManagedPipelineMode.Classic : ManagedPipelineMode.Integrated;
		}
		//
		public bool is_shared_pool(string poolName)
		{
			return Array.Exists<WebAppPool>(supportedAppPools.ToArray(),
				x => x.Name.Equals(poolName) && isolation(x.Mode) == SiteAppPoolMode.Shared);
		}
		//
		public WebAppPool match_webapp_pool(WebVirtualDirectory vdir)
		{
			// Detect isolation mode
			SiteAppPoolMode sisMode = is_shared_pool(vdir.ApplicationPool) ? 
				SiteAppPoolMode.Shared : SiteAppPoolMode.Dedicated;
			// Match proper app pool
			return Array.Find<WebAppPool>(SupportedAppPools.ToArray(),
				x => x.AspNetInstalled.Equals(vdir.AspNetInstalled) && isolation(x.Mode) == sisMode);
		}
		//
		private void SetupSupportedAppPools(ServiceProviderSettings s)
		{
			supportedAppPools = new List<WebAppPool>();

			#region Populate Shared Application Pools
			// ASP.NET 1.1
			if (!String.IsNullOrEmpty(s[Constants.AspNet11Pool]))
			{
				supportedAppPools.Add(new WebAppPool
				{
					AspNetInstalled = "1",
					Mode = SiteAppPoolMode.dotNetFramework1 | SiteAppPoolMode.Classic | SiteAppPoolMode.Shared,
					Name = s[Constants.AspNet11Pool].Trim()
				});
			}
			// ASP.NET 2.0 (Classic pipeline)
			if (!String.IsNullOrEmpty(s[Constants.ClassicAspNet20Pool]))
			{
				supportedAppPools.Add(new WebAppPool
				{
					AspNetInstalled = "2",
					Mode = SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.Classic | SiteAppPoolMode.Shared,
					Name = s[Constants.ClassicAspNet20Pool].Trim()
				});
			}
			// ASP.NET 2.0 (Integrated pipeline)
			if (!String.IsNullOrEmpty(s[Constants.IntegratedAspNet20Pool]))
			{
				supportedAppPools.Add(new WebAppPool
				{
					AspNetInstalled = "2I",
					Mode = SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.Integrated | SiteAppPoolMode.Shared,
					Name = s[Constants.IntegratedAspNet20Pool].Trim()
				});
			}
			// ASP.NET 4.0 (Classic pipeline)
			if (!String.IsNullOrEmpty(s[Constants.ClassicAspNet40Pool]))
			{
				supportedAppPools.Add(new WebAppPool
				{
					AspNetInstalled = "4",
					Mode = SiteAppPoolMode.dotNetFramework4 | SiteAppPoolMode.Classic | SiteAppPoolMode.Shared,
					Name = s[Constants.ClassicAspNet40Pool].Trim()
				});
			}
			// ASP.NET 4.0 (Integrated pipeline)
			if (!String.IsNullOrEmpty(s[Constants.IntegratedAspNet40Pool]))
			{
				supportedAppPools.Add(new WebAppPool
				{
					AspNetInstalled = "4I",
					Mode = SiteAppPoolMode.dotNetFramework4 | SiteAppPoolMode.Integrated | SiteAppPoolMode.Shared,
					Name = s[Constants.IntegratedAspNet40Pool].Trim()
				});
			}
			#endregion

			#region Populate Dedicated Application Pools
			// ASP.NET 1.1
			supportedAppPools.Add(new WebAppPool
			{
				AspNetInstalled = "1",
				Mode = SiteAppPoolMode.dotNetFramework1 | SiteAppPoolMode.Classic | SiteAppPoolMode.Dedicated,
				Name = Constants.APP_POOL_NAME_FORMAT_STRING
			});
			// ASP.NET 2.0 (Classic pipeline)
			supportedAppPools.Add(new WebAppPool
			{
				AspNetInstalled = "2",
				Mode = SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.Classic | SiteAppPoolMode.Dedicated,
				Name = Constants.APP_POOL_NAME_FORMAT_STRING
			});
			// ASP.NET 2.0 (Integrated pipeline)
			supportedAppPools.Add(new WebAppPool
			{
				AspNetInstalled = "2I",
				Mode = SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.Integrated | SiteAppPoolMode.Dedicated,
				Name = Constants.APP_POOL_NAME_FORMAT_STRING
			});
			// ASP.NET 4.0 (Classic pipeline)
			supportedAppPools.Add(new WebAppPool
			{
				AspNetInstalled = "4",
				Mode = SiteAppPoolMode.dotNetFramework4 | SiteAppPoolMode.Classic | SiteAppPoolMode.Dedicated,
				Name = Constants.APP_POOL_NAME_FORMAT_STRING
			});
			// ASP.NET 4.0 (Integrated pipeline)
			supportedAppPools.Add(new WebAppPool
			{
				AspNetInstalled = "4I",
				Mode = SiteAppPoolMode.dotNetFramework4 | SiteAppPoolMode.Integrated | SiteAppPoolMode.Dedicated,
				Name = Constants.APP_POOL_NAME_FORMAT_STRING
			});
			#endregion

			// Make some corrections for frameworks with the version number greater or less than 2.0 ...
			
			#region No ASP.NET 1.1 has been found - so remove extra pools
			if (String.IsNullOrEmpty(s[Constants.AspNet11PathSetting]))
				supportedAppPools.RemoveAll(x => dotNetVersion(x.Mode) == SiteAppPoolMode.dotNetFramework1); 
			#endregion

			#region No ASP.NET 4.0 has been found - so remove extra pools
			var aspNet40PathSetting = (Constants.X64Environment)
					? Constants.AspNet40x64PathSetting : Constants.AspNet40PathSetting;
			//
			if (String.IsNullOrEmpty(s[aspNet40PathSetting]))
				supportedAppPools.RemoveAll(x => dotNetVersion(x.Mode) == SiteAppPoolMode.dotNetFramework4); 
			#endregion
		}
	}

	public class IIs70 : IIs60, IWebServer//, IDefaultApplicationPoolNameProvider
	{
        private WebObjectsModuleService webObjectsSvc;
        private DirectoryBrowseModuleService dirBrowseSvc;
        private AnonymAuthModuleService anonymAuthSvc;
        private WindowsAuthModuleService winAuthSvc;
        private BasicAuthModuleService basicAuthSvc;
        private DefaultDocsModuleService defaultDocSvc;
		private CustomHttpErrorsModuleService customErrorsSvc;
		private CustomHttpHeadersModuleService customHeadersSvc;
        private ClassicAspModuleService classicAspSvc;
		private MimeTypesModuleService mimeTypesSvc;
        private ExtensionsModuleService extensionsSvc;
		private HandlersModuleService handlersSvc;
		private HttpRedirectModuleService httpRedirectSvc;
		//

        #region Constants

        public const string IIS_IUSRS_GROUP = "IIS_IUSRS";

        //
		public const string FPSE2002_OWSADM_PATH_x86 = @"%CommonProgramFiles%\Microsoft Shared\Web Server Extensions\50\bin\owsadm.exe";
		public const string FPSE2002_OWSADM_PATH_x64 = @"%CommonProgramFiles(x86)%\Microsoft Shared\Web Server Extensions\50\bin\owsadm.exe";
		//
		public const string FRONTPAGE_2002_REGLOC_x86 = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\5.0";
		public const string FRONTPAGE_2002_REGLOC_x64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\5.0";
		//
		public const string FRONTPAGE_ALLPORTS_REGLOC_x86 = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\All Ports\";
		public const string FRONTPAGE_ALLPORTS_REGLOC_x64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\All Ports\";
		//
		public const string FRONTPAGE_PORT_REGLOC_x86 = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\Ports\";
		public const string FRONTPAGE_PORT_REGLOC_x64 = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\Ports\";

		private string[] INSTALL_SECTIONS_ALLOWED = new string[] {
			Constants.CachingSection,
			Constants.DefaultDocumentsSection,
			Constants.DirectoryBrowseSection,
			Constants.FactCgiSection,
			Constants.HandlersSection,
			Constants.HttpErrorsSection,
			Constants.HttpProtocolSection,
			Constants.HttpRedirectSection,
			Constants.AnonymousAuthenticationSection,
			Constants.BasicAuthenticationSection,
			Constants.WindowsAuthenticationSection,
			Constants.StaticContentSection
		};

        #endregion

		#region Helper Properties

		protected string FPSE2002_OWSADM_PATH
		{
			get
			{
				if (Constants.X64Environment)
					return FPSE2002_OWSADM_PATH_x64;
				else
					return FPSE2002_OWSADM_PATH_x86;
			}
		}

		new protected string FRONTPAGE_2002_REGLOC
		{
			get
			{
				if (Constants.X64Environment)
					return FRONTPAGE_2002_REGLOC_x64;
				else
					return FRONTPAGE_2002_REGLOC_x86;
			}
		}

		new protected string FRONTPAGE_ALLPORTS_REGLOC
		{
			get
			{
				if (Constants.X64Environment)
					return FRONTPAGE_ALLPORTS_REGLOC_x64;
				else
					return FRONTPAGE_ALLPORTS_REGLOC_x86;
			}
		}

		new protected string FRONTPAGE_PORT_REGLOC
		{
			get
			{
				if (Constants.X64Environment)
					return FRONTPAGE_PORT_REGLOC_x64;
				else
					return FRONTPAGE_PORT_REGLOC_x86;
			}
		}

		#endregion

		#region Provider Properties

		public bool Enable32BitAppOnWin64
        {
            get { return ProviderSettings["AspNetBitnessMode"] == "32"; }
        }

        /// <summary>
        /// Gets shared iisAppObject pool name (Classic Managed Pipeline)
        /// </summary>
        public string ClassicAspNet20Pool
        {
            get { return ProviderSettings["ClassicAspNet20Pool"]; }
        }

        /// <summary>
        /// Gets shared iisAppObject pool name (Integrated Managed Pipeline)
        /// </summary>
        public string IntegratedAspNet20Pool
        {
            get { return ProviderSettings["IntegratedAspNet20Pool"]; }
        }

		/// <summary>
		/// Gets shared iisAppObject pool name (Classic Managed Pipeline)
		/// </summary>
		public string ClassicAspNet40Pool
		{
			get { return ProviderSettings["ClassicAspNet40Pool"]; }
		}

		/// <summary>
		/// Gets shared iisAppObject pool name (Integrated Managed Pipeline)
		/// </summary>
		public string IntegratedAspNet40Pool
		{
			get { return ProviderSettings["IntegratedAspNet40Pool"]; }
		}

        public string PhpMode
        {
            get { return ProviderSettings["PhpMode"]; }
        }

        public string PhpExecutablePath
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["PhpPath"]); }
        }

		public string SecureFoldersModuleAssembly
		{
			get { return ProviderSettings["SecureFoldersModuleAssembly"]; }
		}

		protected override string ProtectedAccessFile
		{
			get
			{
				return ".htaccess";
			}
		}

		protected override string ProtectedFoldersFile
		{
			get
			{
				return ".htfolders";
			}
		}

        #endregion

        public IIs70()
        {
            if (IsIISInstalled())
            {
				// New implementation avoiding locks and other sync issues
				winAuthSvc = new WindowsAuthModuleService();
				anonymAuthSvc = new AnonymAuthModuleService();
				basicAuthSvc = new BasicAuthModuleService();
				defaultDocSvc = new DefaultDocsModuleService();
				classicAspSvc = new ClassicAspModuleService();
				httpRedirectSvc = new HttpRedirectModuleService();
				extensionsSvc = new ExtensionsModuleService();
				customErrorsSvc = new CustomHttpErrorsModuleService();
				customHeadersSvc = new CustomHttpHeadersModuleService();
				webObjectsSvc = new WebObjectsModuleService();
				dirBrowseSvc = new DirectoryBrowseModuleService();
				mimeTypesSvc = new MimeTypesModuleService();
				handlersSvc = new HandlersModuleService();
            }
        }

		#region Helper methods

		private void CreateWebSiteAnonymousAccount(WebSite site)
		{
			// anonymous user groups
			List<string> webGroups = new List<string>();
			webGroups.Add(WebGroupName);

			// create web site anonymous account
			SystemUser user = new SystemUser();
			user.Name = GetNonQualifiedAccountName(site.AnonymousUsername);
			user.FullName = GetNonQualifiedAccountName(site.AnonymousUsername);

			// Fix. Import web site that runs under NETWORK_SERVICE identity fails.
			// WebsitePanel cannot create anonymous account.
			/*if (!user.Name.Contains(site.Name.Replace(".", "")))
			{
				user.Name = user.FullName = site.Name.Replace(".", "") + "_web";
			}*/

			//check is user name less than 20 symbols (Windows name length restriction)
			if (user.Name.Length > 20)
			{
				int separatorPlace = user.Name.IndexOf("_");
				user.Name = user.Name.Remove(separatorPlace - (user.Name.Length - 20), user.Name.Length - 20);
			}

			site.AnonymousUsername = user.Name;

			user.Description = "WebsitePanel System Account";
			user.MemberOf = webGroups.ToArray();

			//set new password for created Anonymous Account
			if (String.IsNullOrEmpty(site.AnonymousUserPassword))
			{
				site.AnonymousUserPassword = Guid.NewGuid().ToString();
			}

			user.Password = site.AnonymousUserPassword;
			user.PasswordCantChange = true;
			user.PasswordNeverExpires = true;
			user.AccountDisabled = false;
			user.System = true;
			// create in the system
			try
			{
				SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);
			}
			catch (Exception ex)
			{
				// the possible reason the account already exists
				// check this
				if (SecurityUtils.UserExists(user.Name, ServerSettings, UsersOU))
				{
					// yes
					// try to give it original name
					for (int i = 2; i < 99; i++)
					{
						string username = user.Name + i.ToString();
						if (!SecurityUtils.UserExists(username, ServerSettings, UsersOU))
						{
							user.Name = username;
							site.AnonymousUsername = username;

							// try to create again
							SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);
							break;
						}
					}
				}
				else
				{
					throw ex;
				}
			}
		}

		private void FillVirtualDirectoryFromIISObject(WebVirtualDirectory virtualDir)
		{
			// Set physical path.
            virtualDir.ContentPath = webObjectsSvc.GetPhysicalPath(virtualDir);
			// load iisDirObject browse
			PropertyBag bag = dirBrowseSvc.GetDirectoryBrowseSettings(virtualDir.FullQualifiedPath);
			virtualDir.EnableDirectoryBrowsing = (bool)bag[DirectoryBrowseGlobals.Enabled];
			
            // load anonym auth
			bag = anonymAuthSvc.GetAuthenticationSettings(virtualDir.FullQualifiedPath);
			virtualDir.AnonymousUsername = (string)bag[AuthenticationGlobals.AnonymousAuthenticationUserName];
			virtualDir.AnonymousUserPassword = (string)bag[AuthenticationGlobals.AnonymousAuthenticationPassword];
			virtualDir.EnableAnonymousAccess = (bool)bag[AuthenticationGlobals.Enabled];
			
            // load windows auth 
			bag = winAuthSvc.GetAuthenticationSettings(virtualDir.FullQualifiedPath);
			virtualDir.EnableWindowsAuthentication = (bool)bag[AuthenticationGlobals.Enabled];
            // load basic auth
            basicAuthSvc.GetAuthenticationSettings(virtualDir);
			
            // load default docs
			virtualDir.DefaultDocs = defaultDocSvc.GetDefaultDocumentSettings(virtualDir.FullQualifiedPath);

			// load classic asp
			bag = classicAspSvc.GetClassicAspSettings(virtualDir.FullQualifiedPath);
			virtualDir.EnableParentPaths = (bool)bag[ClassicAspGlobals.EnableParentPaths];
            //
            virtualDir.IIs7 = true;
		}

		private void FillIISObjectFromVirtualDirectory(WebVirtualDirectory virtualDir)
        {
            dirBrowseSvc.SetDirectoryBrowseEnabled(virtualDir.FullQualifiedPath, virtualDir.EnableDirectoryBrowsing);
            //
            SetAnonymousAuthentication(virtualDir);
            // 
            winAuthSvc.SetEnabled(virtualDir.FullQualifiedPath, virtualDir.EnableWindowsAuthentication);
            //
            basicAuthSvc.SetAuthenticationSettings(virtualDir);
            //
            defaultDocSvc.SetDefaultDocumentSettings(virtualDir.FullQualifiedPath, virtualDir.DefaultDocs);
            //
            classicAspSvc.SetClassicAspSettings(virtualDir);
        }

        private void FillVirtualDirectoryRestFromIISObject(WebVirtualDirectory virtualDir)
        {
            // HTTP REDIRECT
			httpRedirectSvc.LoadHttpRedirectSettings(virtualDir);

            // HTTP HEADERS
            customHeadersSvc.GetCustomHttpHeaders(virtualDir);

            // HTTP ERRORS
			customErrorsSvc.GetCustomErrors(virtualDir);

            // MIME MAPPINGS
			mimeTypesSvc.GetMimeMaps(virtualDir);

            // SCRIPT MAPS
			// Load installed script maps.
			HandlerActionCollection installedHandlers = this.handlersSvc.GetHandlers(virtualDir);
			virtualDir.AspInstalled = false; // not installed
			virtualDir.PhpInstalled = ""; // none
			virtualDir.PerlInstalled = false; // not installed
			virtualDir.PythonInstalled = false; // not installed
            virtualDir.ColdFusionInstalled = false; // not installed

			// Loop through available maps and fill installed processors
			foreach(HandlerAction action in installedHandlers)
			{
				// Extract and evaluate scripting processor path
				string processor = FileUtils.EvaluateSystemVariables(action.ScriptProcessor);
				
				// Detect whether ASP scripting is enabled
				if (!String.IsNullOrEmpty(AspPath) && String.Equals(AspPath, processor, StringComparison.InvariantCultureIgnoreCase))
					virtualDir.AspInstalled = true;
				
				// Detect whether PHP 5 scripting is enabled
                if (!String.IsNullOrEmpty(PhpExecutablePath) && String.Equals(PhpExecutablePath, processor, StringComparison.InvariantCultureIgnoreCase))
					virtualDir.PhpInstalled = PHP_5;

				// Detect whether PHP 4 scripting is enabled
				if (!String.IsNullOrEmpty(Php4Path) && String.Equals(Php4Path, processor, StringComparison.InvariantCultureIgnoreCase))
					virtualDir.PhpInstalled = PHP_4;

                // Detect whether ColdFusion scripting is enabled
                if (!String.IsNullOrEmpty(ColdFusionPath) && String.Compare(ColdFusionPath, processor, true) == 0 && action.Name.Contains(".cfm"))
                    virtualDir.ColdFusionInstalled = true;
				
				// Detect whether Perl scripting is enabled
				if (!String.IsNullOrEmpty(PerlPath) && String.Equals(PerlPath, processor, StringComparison.InvariantCultureIgnoreCase))
					virtualDir.PerlInstalled = true;
			}
			//
			string fqPath = virtualDir.FullQualifiedPath;
			if (!fqPath.EndsWith(@"/"))
				fqPath += "/";
			//
			fqPath += CGI_BIN_FOLDER;
			//
			HandlerAccessPolicy policy = handlersSvc.GetHandlersAccessPolicy(fqPath);
			virtualDir.CgiBinInstalled = (policy & HandlerAccessPolicy.Execute) > 0;
			
			// ASP.NET
			FillAspNetSettingsFromIISObject(virtualDir);
        }

		private void FillAspNetSettingsFromIISObject(WebVirtualDirectory vdir)
		{
			// Read ASP.NET settings
			if (String.IsNullOrEmpty(vdir.ApplicationPool))
				return;
			//
			try
			{
				//
				using (var srvman = webObjectsSvc.GetServerManager())
				{
					var appool = srvman.ApplicationPools[vdir.ApplicationPool];
					//
					var aphl = new WebAppPoolHelper(ProviderSettings);
					// ASP.NET 2.0 pipeline is supposed by default
					var dotNetVersion = SiteAppPoolMode.dotNetFramework2;
					//
					#region Iterate over managed runtime keys of the helper class to properly evaluate ASP.NET version installed
					foreach (var k in WebAppPool.AspNetVersions)
					{
						if (k.Value.Equals(appool.ManagedRuntimeVersion))
						{
							dotNetVersion = k.Key;
							break;
						}
					}
					#endregion
					// Detect pipeline mode being used
					if (appool.ManagedPipelineMode == ManagedPipelineMode.Classic)
						dotNetVersion |= SiteAppPoolMode.Classic;
					else
						dotNetVersion |= SiteAppPoolMode.Integrated;
					//
					var aspNetVersion = String.Empty;
					#region Iterate over supported ASP.NET versions based on result of the previous runtime version assesement
					foreach (var item in WebAppPoolHelper.SupportedAppPoolModes)
					{
						if (item.Value == dotNetVersion)
						{
							// Obtain ASP.NET version installed
							aspNetVersion = item.Key;
							//
							break;
						}
					}
					#endregion
					// Assign the result of assesement
					vdir.AspNetInstalled = aspNetVersion;
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Failed to read ASP.NET settings from {0}.", vdir.Name), ex);
				// Re-throw
				throw (ex);
			}
		}

		private void DeleteDedicatedPoolsAllocated(string siteName)
		{
			try
			{
				WebAppPoolHelper aphl = new WebAppPoolHelper(ProviderSettings);
				//
				var dedicatedPools = Array.FindAll<WebAppPool>(aphl.SupportedAppPools.ToArray(),
					x => aphl.isolation(x.Mode) == SiteAppPoolMode.Dedicated);
				// cleanup app pools
				foreach (var item in dedicatedPools)
				{
					using (var srvman = webObjectsSvc.GetServerManager())
					{
						//
						string poolName = WSHelper.InferAppPoolName(item.Name, siteName, item.Mode);
						//
						ApplicationPool pool = srvman.ApplicationPools[poolName];
						if (pool == null)
							continue;
						//
						srvman.ApplicationPools.Remove(pool);
						//
						srvman.CommitChanges();
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				throw (ex);
			}
		}

        private void FillIISObjectFromVirtualDirectoryRest(WebVirtualDirectory virtualDir)
        {
            // TO-DO: HTTP REDIRECT
			httpRedirectSvc.SetHttpRedirectSettings(virtualDir);

            // TO-DO: HTTP HEADERS
			customHeadersSvc.SetCustomHttpHeaders(virtualDir);

            // TO-DO: HTTP ERRORS
			customErrorsSvc.SetCustomErrors(virtualDir);

            // TO-DO: MIME MAPPINGS
			mimeTypesSvc.SetMimeMaps(virtualDir);

			// Revert script mappings to the parent to simplify script mappings cleanup
			handlersSvc.InheritScriptMapsFromParent(virtualDir.FullQualifiedPath);

            // TO-DO: SCRIPT MAPS
			#region ASP script mappings
			if (!String.IsNullOrEmpty(AspPath) && File.Exists(AspPath))
			{
				// Classic ASP
				if (virtualDir.AspInstalled)
				{
					handlersSvc.AddScriptMaps(virtualDir, ASP_EXTENSIONS, AspPath,
						Constants.HandlerAlias.CLASSIC_ASP, Constants.IsapiModule);
				}
				else
				{
					handlersSvc.RemoveScriptMaps(virtualDir, ASP_EXTENSIONS, AspPath);
				}
			}
			#endregion

			#region PHP 5 script mappings
			if (!String.IsNullOrEmpty(PhpExecutablePath) && File.Exists(PhpExecutablePath))
			{
				if (virtualDir.PhpInstalled == PHP_5)
				{
					switch (PhpMode)
					{
						case Constants.PhpMode.FastCGI:
							handlersSvc.AddScriptMaps(virtualDir, PHP_EXTENSIONS,
								PhpExecutablePath, Constants.HandlerAlias.PHP_FASTCGI, Constants.FastCgiModule);
							break;
						case Constants.PhpMode.CGI:
							handlersSvc.AddScriptMaps(virtualDir, PHP_EXTENSIONS,
								PhpExecutablePath, Constants.HandlerAlias.PHP_CGI, Constants.CgiModule);
							break;
						case Constants.PhpMode.ISAPI:
							handlersSvc.AddScriptMaps(virtualDir, PHP_EXTENSIONS,
								PhpExecutablePath, Constants.HandlerAlias.PHP_ISAPI, Constants.IsapiModule);
							break;
					}
				}
				else
				{
					handlersSvc.RemoveScriptMaps(virtualDir, PHP_EXTENSIONS, PhpExecutablePath);
				}
			}
			//
			#endregion

			#region PHP 4 script mappings (IsapiModule only)
			if (!String.IsNullOrEmpty(Php4Path) && File.Exists(Php4Path))
			{
				if (virtualDir.PhpInstalled == PHP_4)
				{
					handlersSvc.AddScriptMaps(virtualDir, PHP_EXTENSIONS, Php4Path,
						Constants.HandlerAlias.PHP_ISAPI, Constants.IsapiModule);
				}
				else
				{
					handlersSvc.RemoveScriptMaps(virtualDir, PHP_EXTENSIONS, Php4Path);
				}
			}
			//
			#endregion

			#region PERL scrit mappings
			//
			if (!String.IsNullOrEmpty(PerlPath) && File.Exists(PerlPath))
			{
				// Start from building Perl-specific CGI-executable definition
				// IsapiModule
				if (virtualDir.PerlInstalled)
				{
					// Perl works only in 32-bit mode on x64 environment
					webObjectsSvc.ForceEnableAppPoolWow6432Mode(virtualDir.ApplicationPool);
					//
					handlersSvc.AddScriptMaps(virtualDir, PERL_EXTENSIONS, PerlPath, 
						Constants.HandlerAlias.PERL_ISAPI, Constants.IsapiModule);
				}
				else
				{
					handlersSvc.RemoveScriptMaps(virtualDir, PERL_EXTENSIONS, PerlPath);
				}
			}
			//
			#endregion

			#region ColdFusion script mappings
			//ColdFusion
			if (virtualDir.ColdFusionInstalled)
			{
				handlersSvc.AddScriptMaps(virtualDir, COLDFUSION_EXTENSIONS, ColdFusionPath, 
					Constants.HandlerAlias.COLDFUSION, Constants.IsapiModule);
			}
			else
			{
				handlersSvc.RemoveScriptMaps(virtualDir, COLDFUSION_EXTENSIONS, ColdFusionPath);
			}
			#endregion
			// TO-DO: REST
        }

		/// <summary>
		/// Update CGI-Bin
		/// </summary>
		/// <param name="installedHandlers">Already installed scrip maps.</param>
		/// <param name="extensions">Extensions to check.</param>
		/// <param name="processor">Extensions processor.</param>
		private void UpdateCgiBinFolder(WebVirtualDirectory virtualDir)
		{
			//
			string fqPath = virtualDir.FullQualifiedPath;
			if (!fqPath.EndsWith(@"/"))
				fqPath += "/";
			//
			fqPath += CGI_BIN_FOLDER;
			string cgiBinPath = Path.Combine(virtualDir.ContentPath, CGI_BIN_FOLDER);
			//
			HandlerAccessPolicy policy = handlersSvc.GetHandlersAccessPolicy(fqPath);
			policy &= ~HandlerAccessPolicy.Execute;
			//
			if (virtualDir.CgiBinInstalled)
			{
				// create folder if not exists
				if (!FileUtils.DirectoryExists(cgiBinPath))
					FileUtils.CreateDirectory(cgiBinPath);
				//
				policy |= HandlerAccessPolicy.Execute;
			}
			//
			if (FileUtils.DirectoryExists(cgiBinPath))
				handlersSvc.SetHandlersAccessPolicy(fqPath, policy);
		}

		/// <summary>
        /// Sets anonymous authentication for the virtual iisDirObject
        /// </summary>
        /// <param name="vdir"></param>
        private void SetAnonymousAuthentication(WebVirtualDirectory virtualDir)
        {
            // set anonymous credentials
            anonymAuthSvc.SetAuthenticationSettings(virtualDir.FullQualifiedPath, 
                    GetQualifiedAccountName(virtualDir.AnonymousUsername), 
					virtualDir.AnonymousUserPassword, virtualDir.EnableAnonymousAccess);
			// configure "Connect As" feature
			if (virtualDir.ContentPath.StartsWith(@"\\"))
				webObjectsSvc.ConfigureConnectAsFeature(virtualDir);
        }

		/// <summary>
		/// Creates if needed dedicated iisAppObject pools and assigns to specified site iisAppObject pool according to 
		/// selected ASP.NET version.
		/// </summary>
		/// <param name="site">WEb site to operate on.</param>
		/// <param name="createAppPools">A value which shows whether iisAppObject pools has to be created.</param>
        private void SetWebSiteApplicationPool(WebSite site, bool createAppPools)
        {
			var aphl = new WebAppPoolHelper(ProviderSettings);
			// Site isolation mode
			var sisMode = site.DedicatedApplicationPool ? SiteAppPoolMode.Dedicated : SiteAppPoolMode.Shared;
			// Create dedicated iisAppObject pool name for the site with installed ASP.NET version
			if (createAppPools && site.DedicatedApplicationPool)
			{
				// Find dedicated app pools
				var dedicatedPools = Array.FindAll<WebAppPool>(aphl.SupportedAppPools.ToArray(),
					x => aphl.isolation(x.Mode) == SiteAppPoolMode.Dedicated);
				// Generate dedicated iisAppObject pools names and create them.
				foreach (var item in dedicatedPools)
				{
					// Retrieve .NET Framework version
					var dotNetVersion = aphl.dotNetVersion(item.Mode);
					//
					var enable32BitAppOnWin64 = Enable32BitAppOnWin64;
					// Force "enable32BitAppOnWin64" set to true for .NET v1.1
					if (dotNetVersion == SiteAppPoolMode.dotNetFramework1)
						enable32BitAppOnWin64 = true;
					//
					var poolName = WSHelper.InferAppPoolName(item.Name, site.Name, item.Mode);
					//
					using (var srvman = webObjectsSvc.GetServerManager())
					{
						// Create iisAppObject pool
						var pool = srvman.ApplicationPools.Add(poolName);
						pool.ManagedRuntimeVersion = aphl.aspnet_runtime(item.Mode);
						pool.ManagedPipelineMode = aphl.runtime_pipeline(item.Mode);
						pool.Enable32BitAppOnWin64 = enable32BitAppOnWin64;
						pool.AutoStart = true;
						// Identity
						pool.ProcessModel.IdentityType = ProcessModelIdentityType.SpecificUser;
						pool.ProcessModel.UserName = GetQualifiedAccountName(site.AnonymousUsername);
						pool.ProcessModel.Password = site.AnonymousUserPassword;
						// Commit changes
						srvman.CommitChanges();
					}
				}
			}
			// Find
			var siteAppPool = Array.Find<WebAppPool>(aphl.SupportedAppPools.ToArray(),
				x => x.AspNetInstalled.Equals(site.AspNetInstalled) && aphl.isolation(x.Mode) == sisMode);
			// Assign iisAppObject pool according to ASP.NET version installed and isolation mode specified.
			site.ApplicationPool = WSHelper.InferAppPoolName(siteAppPool.Name, site.Name, siteAppPool.Mode);
        }

        private void CheckEnableWritePermissions(WebVirtualDirectory virtualDir)
        {
            string anonymousUsername = virtualDir.AnonymousUsername;
            //
            if (virtualDir.DedicatedApplicationPool)
            {
                ApplicationPool appPool = webObjectsSvc.GetApplicationPool(virtualDir);
                //
                if (appPool != null)
                    anonymousUsername = appPool.ProcessModel.UserName;
            }
            //
            if (!String.IsNullOrEmpty(anonymousUsername))
                virtualDir.EnableWritePermissions = CheckWriteAccessEnabled(virtualDir.ContentPath,
                    GetNonQualifiedAccountName(anonymousUsername));
        }

		#endregion

		#region IWebServer Members
		 
		public override void ChangeSiteState(string siteId, ServerState state)
		{
            webObjectsSvc.ChangeSiteState(siteId, state);
		}

        public override ServerState GetSiteState(string siteId)
		{
            return webObjectsSvc.GetSiteState(siteId);
		}

        public override bool SiteExists(string siteId)
		{
            return webObjectsSvc.SiteExists(siteId);
		}

        public override string[] GetSites()
		{
            return webObjectsSvc.GetSites();
		}

        public new string GetSiteId(string siteName)
		{
            return webObjectsSvc.GetWebSiteNameFromIIS(siteName);
		}

        public override WebSite GetSite(string siteId)
		{
			WebAppPoolHelper aphl = new WebAppPoolHelper(ProviderSettings);
            //
			WebSite site = webObjectsSvc.GetWebSiteFromIIS(siteId);
            //
            site.Bindings = webObjectsSvc.GetSiteBindings(siteId);
			//
			FillVirtualDirectoryFromIISObject(site);
            //
            FillVirtualDirectoryRestFromIISObject(site);

            // check frontpage
            site.FrontPageAvailable = IsFrontPageSystemInstalled();
            site.FrontPageInstalled = IsFrontPageInstalled(siteId);

            //check ColdFusion
            if (IsColdFusionSystemInstalled())
            {
                if (IsColdFusion7Installed())
                {
                    site.ColdFusionVersion = "7";
                    site.ColdFusionAvailable = true;
                }
                else
                {
                    if (IsColdFusion8Installed())
                    {
                        site.ColdFusionVersion = "8";
                        site.ColdFusionAvailable = true;
                    }
                }

                if (IsColdFusion9Installed())
                {
                    site.ColdFusionVersion = "9";
                    site.ColdFusionAvailable = true;
                }
            }
            else
            {
                site.ColdFusionAvailable = false;
            }

            site.CreateCFVirtualDirectories = ColdFusionDirectoriesAdded(siteId);
            
            //site.ColdFusionInstalled = IsColdFusionEnabledOnSite(GetSiteId(site.Name));

            // check sharepoint
            site.SharePointInstalled = false;
            //
            site.DedicatedApplicationPool = !aphl.is_shared_pool(site.ApplicationPool);
            //
            CheckEnableWritePermissions(site);
			//
			ReadWebManagementAccessDetails(site);
            //
            site.SecuredFoldersInstalled = IsSecuredFoldersInstalled(siteId);
            //
            site.SiteState = GetSiteState(siteId);
			//
			return site;
		}

        public new string[] GetSitesAccounts(string[] siteIds)
		{
			List<string> accounts = new List<string>();
            //
            for (int i = 0; i < siteIds.Length; i++)
            {
				try
				{
					accounts.Add((string)anonymAuthSvc.GetAuthenticationSettings(siteIds[i])[AuthenticationGlobals.AnonymousAuthenticationUserName]);
				}
				catch (Exception ex)
				{
					Log.WriteError(String.Format("Web site {0} is either deleted or doesn't exist", siteIds[i]), ex);
				}
            }
            //
            return accounts.ToArray();
		}

        public override ServerBinding[] GetSiteBindings(string siteId)
		{
            return webObjectsSvc.GetSiteBindings(siteId);
		}

        public override string CreateSite(WebSite site)
		{
            // assign site id
            site.SiteId = site.Name;

            // create anonymous user
            CreateWebSiteAnonymousAccount(site);

			// Grant IIS_WPG group membership to site's anonymous account
			SecurityUtils.GrantLocalGroupMembership(site.AnonymousUsername, IIS_IUSRS_GROUP, ServerSettings);

            // set iisAppObject pools
            SetWebSiteApplicationPool(site, true);

			// set folder permissions
			SetWebFolderPermissions(site.ContentPath, GetNonQualifiedAccountName(site.AnonymousUsername),
				site.EnableWritePermissions, site.DedicatedApplicationPool);

			// set DATA folder permissions
			SetWebFolderPermissions(site.DataPath, GetNonQualifiedAccountName(site.AnonymousUsername),
				true, site.DedicatedApplicationPool);

            // qualify account name with AD Domain
            site.AnonymousUsername = GetQualifiedAccountName(site.AnonymousUsername);

			//
            try
            {
				// Create site
                webObjectsSvc.CreateSite(site);
				// Update web site bindings
				webObjectsSvc.UpdateSiteBindings(site.SiteId, site.Bindings);
				// Set web site logging settings
				webObjectsSvc.SetWebSiteLoggingSettings(site);
            }
            catch (Exception ex)
            {
                Log.WriteError(ex);
            }
            //
            SetAnonymousAuthentication(site);

            // create logs folder if not exists
            if (!FileUtils.DirectoryExists(site.LogsPath))
                FileUtils.CreateDirectory(site.LogsPath);
			
			//
			FillIISObjectFromVirtualDirectory(site);
			//
			FillIISObjectFromVirtualDirectoryRest(site);
			//
			UpdateCgiBinFolder(site);
            //
            try
            {
                webObjectsSvc.ChangeSiteState(site.SiteId, ServerState.Started);
            }
            catch(Exception ex)
            {
                Log.WriteError(ex);
            }
            //
            return site.SiteId;
		}

        public override void UpdateSite(WebSite site)
		{
            // load original site settings
            WebSite origSite = GetSite(site.SiteId);

			// Get non-qualified anonymous account user name (eq. without domain name or machine name)
			string anonymousAccount = GetNonQualifiedAccountName(site.AnonymousUsername);
			string origAnonymousAccount = GetNonQualifiedAccountName(origSite.AnonymousUsername);

            // if folder has been changed
            if (String.Compare(origSite.ContentPath, site.ContentPath, true) != 0)
                RemoveWebFolderPermissions(origSite.ContentPath, origAnonymousAccount);

            // ensure anonumous user account exists
            if (!SecurityUtils.UserExists(anonymousAccount, ServerSettings, UsersOU))
            {
                CreateWebSiteAnonymousAccount(site);
            }

			anonymousAccount = GetNonQualifiedAccountName(site.AnonymousUsername);

			// Grant IIS_IUSRS group membership
			if (!SecurityUtils.HasLocalGroupMembership(anonymousAccount, IIS_IUSRS_GROUP, ServerSettings, UsersOU))
			{
				SecurityUtils.GrantLocalGroupMembership(anonymousAccount, IIS_IUSRS_GROUP, ServerSettings);
			}

            //
            bool appPoolFlagChanged = origSite.DedicatedApplicationPool != site.DedicatedApplicationPool;
            // check if we need to remove dedicated app pools
            bool deleteDedicatedPools = (appPoolFlagChanged && !site.DedicatedApplicationPool);
            //
            SetWebSiteApplicationPool(site, false);
            //
            if (!webObjectsSvc.IsApplicationPoolExist(site.ApplicationPool) &&
                site.DedicatedApplicationPool)
            {
                // CREATE dedicated pool
                SetWebSiteApplicationPool(site, true);
            }
            //
			FillIISObjectFromVirtualDirectory(site);
            //
            FillIISObjectFromVirtualDirectoryRest(site);

            // set logs folder permissions
            if (!FileUtils.DirectoryExists(site.LogsPath))
                FileUtils.CreateDirectory(site.LogsPath);
            // Update website
            webObjectsSvc.UpdateSite(site);
			// Update website bindings
			webObjectsSvc.UpdateSiteBindings(site.SiteId, site.Bindings);
			// Set website logging settings
			webObjectsSvc.SetWebSiteLoggingSettings(site);
			//
			UpdateCgiBinFolder(site);

            // TO-DO
            // update all child virtual directories to use new pool
            if (appPoolFlagChanged)
            {
                WebVirtualDirectory[] dirs = GetVirtualDirectories(site.SiteId);
                foreach (WebVirtualDirectory dir in dirs)
                {
                    // set dedicated pool flag
                    //dir.DedicatedApplicationPool = site.DedicatedApplicationPool;
                    WebVirtualDirectory vdir = GetVirtualDirectory(site.SiteId, dir.Name);
					vdir.AspNetInstalled = site.AspNetInstalled;
					vdir.ApplicationPool = site.ApplicationPool;
                    // update iisDirObject
                    UpdateVirtualDirectory(site.SiteId, vdir);
                }
            }

			#region ColdFusion Virtual Directories
            if (ColdFusionDirectoriesAdded(site.SiteId))
            {
                if (!site.CreateCFVirtualDirectories)
                {
                    DeleteCFVirtualDirectories(site.SiteId);
                    site.CreateCFVirtualDirectories = false;
                }
            }
            else
            {
                if (site.CreateCFVirtualDirectories)
                {
                    CreateCFVirtualDirectories(site.SiteId);
                    site.CreateCFVirtualDirectories = true;
                }
            }
            #endregion 
            
            // remove dedicated pools if any
			if (deleteDedicatedPools)
				DeleteDedicatedPoolsAllocated(site.Name);

			// set WEB folder permissions
			SetWebFolderPermissions(site.ContentPath, anonymousAccount, site.EnableWritePermissions, site.DedicatedApplicationPool);

			// set DATA folder permissions
			SetWebFolderPermissions(site.DataPath, anonymousAccount, true, site.DedicatedApplicationPool);
		}

		/// <summary>
		/// Updates site's bindings with supplied ones.
		/// </summary>
		/// <param name="siteId">Site's id to update bindings for.</param>
		/// <param name="bindings">Bindings information.</param>
        public override void UpdateSiteBindings(string siteId, ServerBinding[] bindings)
		{
			this.webObjectsSvc.UpdateSiteBindings(siteId, bindings);
		}

		/// <summary>
		/// Deletes site with specified id.
		/// </summary>
		/// <param name="siteId">Site's id to be deleted.</param>
        public override void DeleteSite(string siteId)
		{
			// Load site description.
        	WebSite site = this.GetSite(siteId);

			#region Fix for bug #594
			// Disable Remote Management Access and revoke access permissions if any
			if (IsWebManagementServiceInstalled() && IsWebManagementAccessEnabled(site))
			{
				RevokeWebManagementAccess(siteId, site[WebSite.WmSvcAccountName]);
			}
			#endregion

			// Get non-qualified account name
			string anonymousAccount = GetNonQualifiedAccountName(site.AnonymousUsername);
			// Remove unnecessary permissions.
			RemoveWebFolderPermissions(site.ContentPath, anonymousAccount);

			// Stop web site
            webObjectsSvc.ChangeSiteState(site.Name, ServerState.Stopped);
			Log.WriteInfo(String.Format("Site {0} was stopped before deleting.", site.Name));
            // Delete site in IIS
            webObjectsSvc.DeleteSite(siteId);

			// Delete dedicated pool if required
			if (site.DedicatedApplicationPool)
			{
				DeleteDedicatedPoolsAllocated(site.Name);
			}
			// ensure anonymous account is shared account
			if (!String.Equals("IUSR", anonymousAccount))
			{
				// Revoke IIS_IUSRS membership first
				if (SecurityUtils.HasLocalGroupMembership(anonymousAccount, IIS_IUSRS_GROUP, ServerSettings, UsersOU))
				{
					SecurityUtils.RevokeLocalGroupMembership(anonymousAccount, IIS_IUSRS_GROUP, ServerSettings);
				}

				// delete anonymous user account
				if (SecurityUtils.UserExists(anonymousAccount, ServerSettings, UsersOU))
				{
					SecurityUtils.DeleteUser(anonymousAccount, ServerSettings, UsersOU);
				}
			}
		}

		/// <summary>
		/// Checks whether virtual iisDirObject with supplied name under specified site exists.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="directoryName">Directory name to check.</param>
		/// <returns>true - if it exists; false - otherwise.</returns>
        public override bool VirtualDirectoryExists(string siteId, string directoryName)
		{
        	return this.webObjectsSvc.VirtualDirectoryExists(siteId, directoryName);
		}

		/// <summary>
		/// Gets virtual directories that belong to site with supplied id.
		/// </summary>
		/// <param name="siteId">Site's id to get virtual directories for.</param>
		/// <returns>virtual directories that belong to site with supplied id.</returns>
        public override WebVirtualDirectory[] GetVirtualDirectories(string siteId)
		{
            // get all virt dirs
        	WebVirtualDirectory[] virtDirs = webObjectsSvc.GetVirtualDirectories(siteId);

            // filter
            string sharedToolsFolder = GetMicrosoftSharedFolderPath();
            List<WebVirtualDirectory> result = new List<WebVirtualDirectory>();
            foreach (WebVirtualDirectory dir in virtDirs)
            {
                // check if this is a system (FrontPage or SharePoint) virtual iisDirObject
                if (!String.IsNullOrEmpty(sharedToolsFolder)
                    && dir.ContentPath.ToLower().StartsWith(sharedToolsFolder.ToLower()))
                    continue;
                result.Add(dir);
            }
            return result.ToArray();
		}

        /// <summary>
        /// Gets virtual iisDirObject description that belongs to site with supplied id and has specified name.
        /// </summary>
        /// <param name="siteId">Site's id that owns virtual iisDirObject.</param>
        /// <param name="directoryName">Directory's name to get description for.</param>
        /// <returns>virtual iisDirObject description that belongs to site with supplied id and has specified name.</returns>
        public override WebVirtualDirectory GetVirtualDirectory(string siteId, string directoryName)
        {
			WebAppPoolHelper aphl = new WebAppPoolHelper(ProviderSettings);
			//
            WebVirtualDirectory webVirtualDirectory = webObjectsSvc.GetVirtualDirectory(siteId, directoryName);
            //
            this.FillVirtualDirectoryFromIISObject(webVirtualDirectory);
            this.FillVirtualDirectoryRestFromIISObject(webVirtualDirectory);
            //
            webVirtualDirectory.DedicatedApplicationPool = !aphl.is_shared_pool(webVirtualDirectory.ApplicationPool);
            //
            CheckEnableWritePermissions(webVirtualDirectory);
			//
			ReadWebManagementAccessDetails(webVirtualDirectory);
            //
            return webVirtualDirectory;
        }

		/// <summary>
		/// Creates virtual iisDirObject under site with specified id.
		/// </summary>
		/// <param name="siteId">Site's id to create virtual iisDirObject under.</param>
		/// <param name="iisDirObject">Virtual iisDirObject description.</param>
        public override void CreateVirtualDirectory(string siteId, WebVirtualDirectory directory)
		{
			// Create iisDirObject folder if not exists.
			if (!FileUtils.DirectoryExists(directory.ContentPath))
			{
				FileUtils.CreateDirectory(directory.ContentPath);
			}
			//
			WebSite webSite = GetSite(siteId);
			// copy props from parent site
			directory.ParentSiteName = siteId;
			directory.AspNetInstalled = webSite.AspNetInstalled;
			directory.ApplicationPool = webSite.ApplicationPool;
			// Create record in IIS's configuration.
			webObjectsSvc.CreateVirtualDirectory(siteId, directory.VirtualPath, directory.ContentPath);
			//
			PropertyBag bag = anonymAuthSvc.GetAuthenticationSettings(siteId);
			directory.AnonymousUsername = (string)bag[AuthenticationGlobals.AnonymousAuthenticationUserName];
			directory.AnonymousUserPassword = (string)bag[AuthenticationGlobals.AnonymousAuthenticationPassword];
			directory.EnableAnonymousAccess = (bool)bag[AuthenticationGlobals.Enabled];
			// Update virtual iisDirObject.
			this.UpdateVirtualDirectory(siteId, directory);
		}

		/// <summary>
		/// Updates virtual iisDirObject settings.
		/// </summary>
		/// <param name="siteId">Site's id that owns supplied iisDirObject.</param>
		/// <param name="iisDirObject">Web iisDirObject that needs to be updated.</param>
        public override void UpdateVirtualDirectory(string siteId, WebVirtualDirectory directory)
		{
			if (this.webObjectsSvc.SiteExists(siteId))
			{
				WebAppPoolHelper aphl = new WebAppPoolHelper(ProviderSettings);
                //
                bool dedicatedPool = !aphl.is_shared_pool(directory.ApplicationPool);
				//
				SiteAppPoolMode sisMode = dedicatedPool ? SiteAppPoolMode.Dedicated : SiteAppPoolMode.Shared;
				//
				directory.ParentSiteName = siteId;
                //
                string origPath = webObjectsSvc.GetPhysicalPath(directory);
				// remove unnecessary permissions
				// if original folder has been changed
				if (String.Compare(origPath, directory.ContentPath, true) != 0)
					RemoveWebFolderPermissions(origPath, GetNonQualifiedAccountName(directory.AnonymousUsername));
				// set folder permissions
				SetWebFolderPermissions(directory.ContentPath, GetNonQualifiedAccountName(directory.AnonymousUsername),
						directory.EnableWritePermissions, dedicatedPool);
                //
				var pool = Array.Find<WebAppPool>(aphl.SupportedAppPools.ToArray(),
					x => x.AspNetInstalled.Equals(directory.AspNetInstalled) && aphl.isolation(x.Mode) == sisMode);
				// Assign to virtual iisDirObject iisAppObject pool 
				directory.ApplicationPool = WSHelper.InferAppPoolName(pool.Name, siteId, pool.Mode);
				//
                webObjectsSvc.UpdateVirtualDirectory(directory);
                //
				this.FillIISObjectFromVirtualDirectory(directory);
				this.FillIISObjectFromVirtualDirectoryRest(directory);
			}
		}

		/// <summary>
		/// Deletes virtual iisDirObject within specified site.
		/// </summary>
		/// <param name="siteId">Site id.</param>
		/// <param name="directoryName">Directory name to delete.</param>
		public override void DeleteVirtualDirectory(string siteId, string directoryName)
		{
			var virtualDir = new WebVirtualDirectory
			{
				ParentSiteName = siteId,
				Name = directoryName
			};
            //
            webObjectsSvc.DeleteVirtualDirectory(virtualDir);
            anonymAuthSvc.RemoveAuthenticationSettings(virtualDir.FullQualifiedPath);
		}

        public new void GrantWebSiteAccess(string path, string siteId, NTFSPermission permission)
		{
			// TODO
		}

		#endregion

        #region IISPassword

		protected override bool IsSecuredFoldersInstalled(string siteId)
		{
			using (var srvman = webObjectsSvc.GetServerManager())
			{
				//
				var appConfig = srvman.GetApplicationHostConfiguration();
				//
				var modulesSection = appConfig.GetSection(Constants.ModulesSection, siteId);
				//
				var modulesCollection = modulesSection.GetCollection();
				//
				foreach (var moduleEntry in modulesCollection)
				{
					if (String.Equals(moduleEntry["name"].ToString(), Constants.WEBSITEPANEL_IISMODULES, StringComparison.InvariantCultureIgnoreCase))
						return true;
				}
			}
			//
			return false;
		}

		protected override string GetSiteContentPath(string siteId)
		{
			var webSite = webObjectsSvc.GetWebSiteFromIIS(siteId);
			//
			if (webSite != null)
				return webObjectsSvc.GetPhysicalPath(webSite);
			//
			return String.Empty;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="System.ArgumentNullException" />
		/// <exception cref="System.ApplicationException" />
		/// <param name="siteId"></param>
        public override void InstallSecuredFolders(string siteId)
        {
			//
			if (String.IsNullOrEmpty(siteId))
				throw new ArgumentNullException("siteId");

			// WebsitePanel.IIsModules works for apps working in Integrated Pipeline mode
			#region Switch automatically to the app pool with Integrated Pipeline enabled
			var webSite = webObjectsSvc.GetWebSiteFromIIS(siteId);
			//
			if (webSite == null)
				throw new ApplicationException(String.Format("Could not find a web site with the following identifier: {0}.", siteId));
			//
			var aphl = new WebAppPoolHelper(ProviderSettings);
			// Fill ASP.NET settings
			FillAspNetSettingsFromIISObject(webSite);
			//
			var currentPool = aphl.match_webapp_pool(webSite);
			var dotNetVersion = aphl.dotNetVersion(currentPool.Mode);
			var sisMode = aphl.isolation(currentPool.Mode);
			// AT least ASP.NET 2.0 is allowed to provide such capabilities...
			if (dotNetVersion == SiteAppPoolMode.dotNetFramework1)
				dotNetVersion = SiteAppPoolMode.dotNetFramework2;
			// and Integrated pipeline...
			if (aphl.pipeline(currentPool.Mode) != SiteAppPoolMode.Integrated)
			{
				// Lookup for the opposite pool matching the criteria
				var oppositePool = Array.Find<WebAppPool>(aphl.SupportedAppPools.ToArray(),
					x => aphl.dotNetVersion(x.Mode) == dotNetVersion && aphl.isolation(x.Mode) == sisMode
						&& aphl.pipeline(x.Mode) == SiteAppPoolMode.Integrated);
				//
				webSite.AspNetInstalled = oppositePool.AspNetInstalled;
				//
				SetWebSiteApplicationPool(webSite, false);
				//
				using (var srvman = webObjectsSvc.GetServerManager())
				{
					var iisSiteObject = srvman.Sites[siteId];
					iisSiteObject.Applications["/"].ApplicationPoolName = webSite.ApplicationPool;
					//
					srvman.CommitChanges();
				}
			}
			#endregion

			#region Disable automatically Integrated Windows Authentication
			PropertyBag winAuthBag = winAuthSvc.GetAuthenticationSettings(siteId);
			//
			if ((bool)winAuthBag[AuthenticationGlobals.Enabled])
			{
				//
				using (var srvman = webObjectsSvc.GetServerManager())
				{
					Configuration config = srvman.GetApplicationHostConfiguration();

					ConfigurationSection windowsAuthenticationSection = config.GetSection(
						"system.webServer/security/authentication/windowsAuthentication",
						siteId);
					//
					windowsAuthenticationSection["enabled"] = false;
					//
					srvman.CommitChanges();
				}
			}
			#endregion
			
			//
			using (var srvman = webObjectsSvc.GetServerManager())
			{
				//
				Configuration appConfig = srvman.GetApplicationHostConfiguration();
				//
				ConfigurationSection modulesSection = appConfig.GetSection(Constants.ModulesSection, siteId);
				//
				ConfigurationElementCollection modulesCollection = modulesSection.GetCollection();
				//
				ConfigurationElement moduleAdd = modulesCollection.CreateElement("add");
				//
				moduleAdd["name"] = Constants.WEBSITEPANEL_IISMODULES;
				moduleAdd["type"] = SecureFoldersModuleAssembly;
				moduleAdd["preCondition"] = "managedHandler";
				//
				modulesCollection.Add(moduleAdd);
				//
				srvman.CommitChanges();
			}
			
        }

        public override void UninstallSecuredFolders(string siteId)
        {
			//
			if (String.IsNullOrEmpty(siteId))
				throw new ArgumentNullException("siteId");
			//
			using (var srvman = webObjectsSvc.GetServerManager())
			{
				//
				Configuration appConfig = srvman.GetApplicationHostConfiguration();
				//
				ConfigurationSection modulesSection = appConfig.GetSection(Constants.ModulesSection, siteId);
				//
				ConfigurationElementCollection modulesCollection = modulesSection.GetCollection();
				//
				ConfigurationElement iisModulesEntry = null;
				//
				foreach (ConfigurationElement moduleEntry in modulesCollection)
				{
					if (String.Equals(moduleEntry["name"].ToString(), Constants.WEBSITEPANEL_IISMODULES, StringComparison.InvariantCultureIgnoreCase))
					{
						iisModulesEntry = moduleEntry;
						break;
					}
				}
				//
				if (iisModulesEntry != null)
				{
					modulesCollection.Remove(iisModulesEntry);
					srvman.CommitChanges();
				}
			}
        }
        
		#endregion

        #region FrontPage

        public override bool IsFrontPageInstalled(string siteId)
        {
			// Get IIS web site id
			string m_webSiteId = webObjectsSvc.GetWebSiteIdFromIIS(siteId, "W3SVC/{0}");
			// site port
			RegistryKey sitePortKey = Registry.LocalMachine.OpenSubKey(String.Format("{0}Port /LM/{1}:",
				FRONTPAGE_PORT_REGLOC, m_webSiteId));

			if (sitePortKey == null)
				return false;

			// get required keys
			string keyAuthoring = (string)sitePortKey.GetValue("authoring");
			string keyFrontPageRoot = (string)sitePortKey.GetValue("frontpageroot");

			return (keyAuthoring != null && keyAuthoring.ToUpper() == "ENABLED" &&
				keyFrontPageRoot != null && keyFrontPageRoot.IndexOf("\\50") != -1);
        }

        public override bool InstallFrontPage(string siteId, string username, string password)
        {
			// Ensure requested user account doesn't exist
			if (SecurityUtils.UserExists(username, ServerSettings, UsersOU))
				return false;
			// Ensure a web site exists
			if (!SiteExists(siteId))
				return false;
			// create user account
			SystemUser user = new SystemUser
			{
				Name = username,
				FullName = username,
				Description = "WebsitePanel System Account",
				Password = password,
				PasswordCantChange = true,
				PasswordNeverExpires = true,
				AccountDisabled = false,
				System = true,
			};

			// create in the system
			SecurityUtils.CreateUser(user, ServerSettings, UsersOU, GroupsOU);

			try
			{
				string cmdPath = null;
				string cmdArgs = null;
				//
				string m_webSiteId = webObjectsSvc.GetWebSiteIdFromIIS(siteId, null);

				// try to install FPSE2002
				// add registry key for anonymous group if not exists
				RegistryKey portsKey = Registry.LocalMachine.OpenSubKey(FRONTPAGE_ALLPORTS_REGLOC, true);
				portsKey.SetValue("anonusergroupprefix", "anonfp");

				#region Create anonymous group to get FPSE work

				string groupName = "anonfp_" + m_webSiteId;
				if (!SecurityUtils.GroupExists(groupName, ServerSettings, GroupsOU))
				{
					SystemGroup fpseGroup = new SystemGroup();
					fpseGroup.Name = groupName;
					fpseGroup.Description = "Anonymous FPSE group for " + siteId + " web site";
					fpseGroup.Members = new string[] { username };
					SecurityUtils.CreateGroup(fpseGroup, ServerSettings, UsersOU, GroupsOU);
				}

				#endregion

				#region Install FPSE 2002 to the website by owsadm.exe install command

				cmdPath = Environment.ExpandEnvironmentVariables(FPSE2002_OWSADM_PATH);
				cmdArgs = String.Format("-o install -p /LM/W3SVC/{0} -u {1}", m_webSiteId, username);
				Log.WriteInfo("Command path: " + cmdPath);
				Log.WriteInfo("Command path: " + cmdArgs);
				Log.WriteInfo("FPSE2002 Install Log: " + FileUtils.ExecuteSystemCommand(cmdPath, cmdArgs));
				
				#endregion

				#region Enable Windows Authentication mode
				
				winAuthSvc.SetEnabled(siteId, true);

				#endregion
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				// Signal to the client installation request has been failed.
				return false;
			}

			return true;
        }

        public override void UninstallFrontPage(string siteId, string username)
        {
			// Ensure a web site exists
			if (!SiteExists(siteId))
				return;

			try
			{
				string m_webSiteId = webObjectsSvc.GetWebSiteIdFromIIS(siteId, null);

				// remove anonymous group
				string groupName = "anonfp_" + m_webSiteId;
				if (SecurityUtils.GroupExists(groupName, ServerSettings, GroupsOU))
					SecurityUtils.DeleteGroup(groupName, ServerSettings, GroupsOU);

				#region Trying to uninstall FPSE2002 from the web site

				//
				string cmdPath = null;
				string cmdArgs = null;

				cmdPath = Environment.ExpandEnvironmentVariables(FPSE2002_OWSADM_PATH);
				cmdArgs = String.Format("-o fulluninstall -p /LM/W3SVC/{0}", m_webSiteId);

				// launch system process
				Log.WriteInfo("FPSE2002 Uninstall Log: " + FileUtils.ExecuteSystemCommand(cmdPath, cmdArgs));

				#endregion

				// delete user account
				if (SecurityUtils.UserExists(username, ServerSettings, UsersOU))
					SecurityUtils.DeleteUser(username, ServerSettings, UsersOU);

				// Disable Windows Authentication mode
				winAuthSvc.SetEnabled(siteId, false);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("FPSE2002 uninstall error. Web site: {0}.", siteId), ex);
			}
        }

        public override bool IsFrontPageSystemInstalled()
        {
			// we will lookup in the registry for the required information
			// check for FPSE 2002
			RegistryKey keyFrontPage = Registry.LocalMachine.OpenSubKey(FRONTPAGE_2002_REGLOC);
			if (keyFrontPage == null)
				return false;

			string[] subKeys = keyFrontPage.GetSubKeyNames();
			if (subKeys != null && subKeys.Length > 0)
			{
				foreach (string key in subKeys)
				{
					if (key == IIs60.FRONTPAGE_2002_INSTALLED || key == IIs60.SHAREPOINT_INSTALLED)
						return true;
				}
			}

			return false;
        }

        #endregion

        #region ColdFusion

        public override void CreateCFVirtualDirectories(string siteId)
        {
            WebVirtualDirectory scriptsDirectory = new WebVirtualDirectory();
            scriptsDirectory.Name = "CFIDE";
            scriptsDirectory.ContentPath = CFScriptsDirectoryPath;
            scriptsDirectory.EnableAnonymousAccess = true;
            scriptsDirectory.EnableWindowsAuthentication = true;
            scriptsDirectory.EnableBasicAuthentication = false;
            scriptsDirectory.DefaultDocs = null; // inherit from service
            scriptsDirectory.HttpRedirect = "";
            scriptsDirectory.HttpErrors = null;
            scriptsDirectory.MimeMaps = null;

            if (!VirtualDirectoryExists(siteId, scriptsDirectory.Name))
            {
                CreateVirtualDirectory(siteId, scriptsDirectory);
            }

            WebVirtualDirectory flashRemotingDir = new WebVirtualDirectory();
            flashRemotingDir.Name = "JRunScripts";
            flashRemotingDir.ContentPath = CFFlashRemotingDirPath;
            flashRemotingDir.EnableAnonymousAccess = true;
            flashRemotingDir.EnableWindowsAuthentication = true;
            flashRemotingDir.EnableBasicAuthentication = false;
            flashRemotingDir.DefaultDocs = null; // inherit from service
            flashRemotingDir.HttpRedirect = "";
            flashRemotingDir.HttpErrors = null;
            flashRemotingDir.MimeMaps = null;

            if (!VirtualDirectoryExists(siteId, flashRemotingDir.Name))
            {
                CreateVirtualDirectory(siteId, flashRemotingDir);
            }
        }

        public override void DeleteCFVirtualDirectories(string siteId)
        {
            DeleteVirtualDirectory(siteId, "CFIDE");
            DeleteVirtualDirectory(siteId, "JRunScripts");

        }

        public bool ColdFusionDirectoriesAdded(string siteId)
        {
            int identifier = 0;

            WebVirtualDirectory[] dirs = GetVirtualDirectories(siteId);

            foreach (WebVirtualDirectory dir in dirs)
            {
                if (dir.FullQualifiedPath.Equals("CFIDE") || dir.FullQualifiedPath.Equals("JRunScripts"))
                    identifier++;
            }
            return identifier.Equals(2);
        }

        protected override bool IsColdFusionEnabledOnSite(string siteId)
        {
            bool isCFenabled = false;

            string ID = webObjectsSvc.GetWebSiteIdFromIIS(siteId, "{0}");

            if (IsColdFusionSystemInstalled())
            {
                string pathWsConfigSettings = Path.Combine(GetColdFusionRootPath(), @"runtime\lib\wsconfig\wsconfig.properties");
                StreamReader file = new StreamReader(pathWsConfigSettings);
                string line = String.Empty;
                int counter = 0;
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(String.Format("=IIS,{0},", ID)))
                    {
                        isCFenabled = true;
                        break;
                    }
                    counter++;
                }
                file.Close();
            }

            return isCFenabled;
        }
        
        #endregion

        #region HostingServiceProvider methods
        public override SettingPair[] GetProviderDefaultSettings()
        {
			List<SettingPair> allSettings = new List<SettingPair>();
            allSettings.AddRange(extensionsSvc.GetISAPIExtensionsInstalled());
			allSettings.AddRange(GetWmSvcServerSettings());
			//
			return allSettings.ToArray();
        }

		/// <summary>
		/// Installs the provider.
		/// </summary>
		/// <returns>Error messsages if any specified.</returns>
		public override string[] Install()
		{
			List<string> messages = new List<string>();

			string[] cfgMsgs = webObjectsSvc.GrantConfigurationSectionAccess(INSTALL_SECTIONS_ALLOWED);
			//
			if (cfgMsgs.Length > 0)
			{
				messages.AddRange(cfgMsgs);
				return messages.ToArray();
			}

			try
			{
				SecurityUtils.EnsureOrganizationalUnitsExist(ServerSettings, UsersOU, GroupsOU);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				messages.Add(String.Format("Could not check/create Organizational Units: {0}", ex.Message));
				return messages.ToArray();
			}

			// Create web group name.
			if (String.IsNullOrEmpty(WebGroupName))
			{
				messages.Add("Web Group can not be blank");
			}
			else
			{
				try
				{
					// create group
					if (!SecurityUtils.GroupExists(WebGroupName, ServerSettings, GroupsOU))
					{
						SystemGroup group = new SystemGroup();
						group.Name = WebGroupName;
						group.Members = new string[] { };
						group.Description = "WebsitePanel System Group";

						SecurityUtils.CreateGroup(group, ServerSettings, UsersOU, GroupsOU);
					}
				}
				catch (Exception ex)
				{
					Log.WriteError(ex);
					messages.Add(String.Format("There was an error while adding '{0}' group: {1}",
						WebGroupName, ex.Message));
				}
			}

			// Setting up shared iisAppObject pools.
			try
			{
				WebAppPoolHelper aphl = new WebAppPoolHelper(ProviderSettings);
				// Find shared pools
				var sharedPools = Array.FindAll<WebAppPool>(aphl.SupportedAppPools.ToArray(), 
					x => aphl.isolation(x.Mode) == SiteAppPoolMode.Shared);
				//
				foreach (var item in sharedPools)
				{
					using (var srvman = webObjectsSvc.GetServerManager())
					{
						// Local variables
						bool enable32BitAppOnWin64 = (aphl.dotNetVersion(item.Mode) == SiteAppPoolMode.dotNetFramework1) ? true : false;
						//
						if (srvman.ApplicationPools[item.Name] == null)
						{
							ApplicationPool pool = srvman.ApplicationPools.Add(item.Name);
							//
							pool.ManagedRuntimeVersion = aphl.aspnet_runtime(item.Mode);
							pool.ManagedPipelineMode = aphl.runtime_pipeline(item.Mode);
							pool.ProcessModel.IdentityType = ProcessModelIdentityType.NetworkService;
							pool.AutoStart = true;
							pool.Enable32BitAppOnWin64 = enable32BitAppOnWin64;
							//
							srvman.CommitChanges();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				//
				messages.Add(String.Format("There was an error while creating shared iisAppObject pools: {0}", ex.Message));
			}

			// Ensure logging settings are configured correctly on a web server level
			try
			{
				webObjectsSvc.SetWebServerDefaultLoggingSettings(LogExtFileFlags.SiteName
					| LogExtFileFlags.BytesRecv | LogExtFileFlags.BytesSent | LogExtFileFlags.Date);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				//
				messages.Add(String.Format(@"There was an error while configure web server's default 
					logging settings. Reason: {0}", ex.StackTrace));
			}

			// Ensure logging settings are configured correctly on a web server level
			try
			{
				webObjectsSvc.SetWebServerDefaultLoggingSettings(LogExtFileFlags.SiteName
					| LogExtFileFlags.BytesRecv | LogExtFileFlags.BytesSent | LogExtFileFlags.Date);
			}
			catch (Exception ex)
			{
				Log.WriteError(ex);
				//
				messages.Add(String.Format(@"There was an error while configure web server's default 
					logging settings. Reason: {0}", ex.StackTrace));
			}

			return messages.ToArray();
		}

		public override ServiceProviderItemBandwidth[] GetServiceItemsBandwidth(ServiceProviderItem[] items, DateTime since)
		{
			ServiceProviderItemBandwidth[] itemsBandwidth = new ServiceProviderItemBandwidth[items.Length];

			// update items with diskspace
			for (int i = 0; i < items.Length; i++)
			{
				ServiceProviderItem item = items[i];

				// create new bandwidth object
				itemsBandwidth[i] = new ServiceProviderItemBandwidth();
				itemsBandwidth[i].ItemId = item.Id;
				itemsBandwidth[i].Days = new DailyStatistics[0];

				if (item is WebSite)
				{
					try
					{
						WebSite site = GetSite(item.Name);
						string siteId = site[WebSite.IIS7_SITE_ID];
						string logsPath = Path.Combine(site.LogsPath, siteId);

						if (!Directory.Exists(logsPath))
							continue;

						// create parser object
						// and update statistics
						LogParser parser = new LogParser("Web", siteId, logsPath, "s-sitename");
						parser.ParseLogs();

						// get daily statistics
						itemsBandwidth[i].Days = parser.GetDailyStatistics(since, new string[] { siteId });
					}
					catch (Exception ex)
					{
						Log.WriteError(ex);
					}
				}
			}
			return itemsBandwidth;
		}


		public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
		{
			List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();

			// update items with diskspace
			foreach (ServiceProviderItem item in items)
			{
				if (item is WebSite)
				{
					try
					{
						Log.WriteStart(String.Format("Calculating '{0}' site logs size", item.Name));

						WebSite site = GetSite(item.Name);
						//
						string logsPath = Path.Combine(site.LogsPath, site[WebSite.IIS7_SITE_ID]);

						// calculate disk space
						ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
						diskspace.ItemId = item.Id;
						diskspace.DiskSpace = -1 * FileUtils.CalculateFolderSize(logsPath);
						itemsDiskspace.Add(diskspace);

						Log.WriteEnd(String.Format("Calculating '{0}' site logs size", item.Name));
					}
					catch (Exception ex)
					{
						Log.WriteError(ex);
					}
				}
			}
			return itemsDiskspace.ToArray();
		}

        #endregion

        public new bool IsIISInstalled()
        {
            int value = 0;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey("SOFTWARE\\Microsoft\\InetStp");
            if (rk != null)
            {
                value = (int)rk.GetValue("MajorVersion", null);
                rk.Close();
            }

            return value == 7;            
        }

        public override bool IsInstalled()
        {
            return IsIISInstalled();
        }

		#region Remote Management Access
		
		/// <summary>
		/// Provides Windows or IIS Management Users identities credentials mode
		/// </summary>
		public string IdentityCredentialsMode
		{
			get { return ProviderSettings["WmSvc.CredentialsMode"]; }
		}

		public new bool CheckWebManagementAccountExists(string accountName)
		{
			// Preserve setting to restore it back
			bool adEnabled = ServerSettings.ADEnabled;
			// !!! Bypass AD for WMSVC as it requires full-qualified username to authenticate user
			// against the web server
			ServerSettings.ADEnabled = false;

			if (IdentityCredentialsMode == "IISMNGR")
			{
				if (ManagementAuthentication.GetUser(accountName) != null)
					return true;
				else
					return false;
			}
			else
			{
				return SecurityUtils.UserExists(accountName, ServerSettings, String.Empty);
			}
		}

		public new ResultObject CheckWebManagementPasswordComplexity(string accountPassword)
		{
			// Preserve setting to restore it back
			bool adEnabled = ServerSettings.ADEnabled;
			// !!! Bypass AD for WMSVC as it requires full-qualified username to authenticate user
			// against the web server
			ServerSettings.ADEnabled = false;
			
			//
			ResultObject result = new ResultObject { IsSuccess = true };

			//
			if (IdentityCredentialsMode == "IISMNGR")
			{
				InvalidPasswordReason reason = ManagementAuthentication.IsPasswordStrongEnough(accountPassword);
				//
				if (reason != InvalidPasswordReason.NoError)
				{
					result.IsSuccess = false;
					result.AddError(reason.ToString(), new Exception("Password complexity check failed"));
				}
			}
			// Restore setting back
			ServerSettings.ADEnabled = adEnabled;

			//
			return result;
		}

		public new void GrantWebManagementAccess(string siteName, string accountName, string accountPassword)
		{
			// Preserve setting to restore it back
			bool adEnabled = ServerSettings.ADEnabled;
			// !!! Bypass AD for WMSVC as it requires full-qualified username to authenticate user
			// against the web server
			ServerSettings.ADEnabled = false;
			
			//
			string fqWebPath = String.Format("/{0}", siteName);

			// Trace input parameters
			Log.WriteInfo("Site Name: {0}; Account Name: {1}; Account Password: {2}; FqWebPath: {3};",
				siteName, accountName, accountPassword, fqWebPath);

			//
			if (IdentityCredentialsMode == "IISMNGR")
			{
				ManagementAuthentication.CreateUser(accountName, accountPassword);
			}
			else
			{
				// Create Windows user
				SecurityUtils.CreateUser(
					new SystemUser
					{
						Name = accountName,
						FullName = accountName,
						Description = "WMSVC Service Account created by WebsitePanel",
						PasswordCantChange = true,
						PasswordNeverExpires = true,
						AccountDisabled = false,
						Password = accountPassword,
						System = true
					},
					ServerSettings,
					String.Empty,
					String.Empty);
				
				// Convert account name to the full-qualified one
				accountName = GetFullQualifiedAccountName(accountName);
				//
				Log.WriteInfo("FQ Account Name: {0};", accountName);
			}
			//
			ManagementAuthorization.Grant(accountName, fqWebPath, false);
			//
			WebSite site = webObjectsSvc.GetWebSiteFromIIS(siteName);
			//
			string contentPath = webObjectsSvc.GetPhysicalPath(site);
			//
			Log.WriteInfo("Site Content Path: {0};", contentPath);
			//
			if (IdentityCredentialsMode == "IISMNGR")
				SecurityUtils.GrantNtfsPermissionsBySid(contentPath, SystemSID.LOCAL_SERVICE, NTFSPermission.Modify, true, true);
			else
				SecurityUtils.GrantNtfsPermissions(contentPath, accountName, NTFSPermission.Modify,
					true, true, ServerSettings, String.Empty, String.Empty);
			// Restore setting back
			ServerSettings.ADEnabled = adEnabled;
		}

		public new void ChangeWebManagementAccessPassword(string accountName, string accountPassword)
		{
			// Preserve setting to restore it back
			bool adEnabled = ServerSettings.ADEnabled;
			// !!! Bypass AD for WMSVC as it requires full-qualified username to authenticate user
			// against the web server
			ServerSettings.ADEnabled = false;

			// Trace input parameters
			Log.WriteInfo("Account Name: {0}; Account Password: {1};", accountName, accountPassword);

			if (IdentityCredentialsMode == "IISMNGR")
			{
				ManagementAuthentication.SetPassword(accountName, accountPassword);
			}
			else
			{
				//
				SystemUser user = SecurityUtils.GetUser(accountName, ServerSettings, String.Empty);
				//
				user.Password = accountPassword;
				//
				SecurityUtils.UpdateUser(user, ServerSettings, String.Empty, String.Empty);
			}
			// Restore setting back
			ServerSettings.ADEnabled = adEnabled;
		}

		public new void RevokeWebManagementAccess(string siteName, string accountName)
		{
			// Preserve setting to restore it back
			bool adEnabled = ServerSettings.ADEnabled;
			// !!! Bypass AD for WMSVC as it requires full-qualified username to authenticate user
			// against the web server
			ServerSettings.ADEnabled = false;
			//
			string fqWebPath = String.Format("/{0}", siteName);
			// Trace input parameters
			Log.WriteInfo("Site Name: {0}; Account Name: {1}; FqWebPath: {2};",
				siteName, accountName, fqWebPath);
			//
			WebSite site = webObjectsSvc.GetWebSiteFromIIS(siteName);
			//
			string contentPath = webObjectsSvc.GetPhysicalPath(site);
			//
			Log.WriteInfo("Site Content Path: {0};", contentPath);
			// Revoke access permissions
			if (IdentityCredentialsMode == "IISMNGR")
			{
				ManagementAuthorization.Revoke(accountName, fqWebPath);
				ManagementAuthentication.DeleteUser(accountName);
				SecurityUtils.RemoveNtfsPermissionsBySid(contentPath, SystemSID.LOCAL_SERVICE);
			}
			else
			{
				ManagementAuthorization.Revoke(GetFullQualifiedAccountName(accountName), fqWebPath);
				SecurityUtils.RemoveNtfsPermissions(contentPath, accountName, ServerSettings, String.Empty, String.Empty);
				SecurityUtils.DeleteUser(accountName, ServerSettings, String.Empty);
			}
			// Restore setting back
			ServerSettings.ADEnabled = adEnabled;
		}

		private bool? isWmSvcInstalled;

		protected void ReadWebManagementAccessDetails(WebVirtualDirectory iisObject)
		{
			bool wmSvcAvailable = IsWebManagementServiceInstalled();
			//
			iisObject.SetValue<bool>(WebSite.WmSvcAvailable, wmSvcAvailable);
			//
			if (wmSvcAvailable)
			{
				//
				iisObject.SetValue<bool>(
					WebVirtualDirectory.WmSvcSiteEnabled, 
					IsWebManagementAccessEnabled(iisObject));

				using (var serverManager = webObjectsSvc.GetServerManager())
				{
					//
					string fqWebPath = @"/" + iisObject.FullQualifiedPath;
					//
					Configuration config = serverManager.GetAdministrationConfiguration();
					ConfigurationSection authorizationSection = config.GetSection("system.webServer/management/authorization");
					ConfigurationElementCollection authorizationRulesCollection = authorizationSection.GetCollection("authorizationRules");

					ConfigurationElement scopeElement = FindElement(authorizationRulesCollection, "scope", "path", fqWebPath);

					Log.WriteInfo("FQ WebPath: " + fqWebPath);

					if (scopeElement != null)
					{
						ConfigurationElementCollection scopeCollection = scopeElement.GetCollection();
						// Retrieve account name
						if (scopeCollection.Count > 0)
						{
							iisObject.SetValue<string>(
								WebSite.WmSvcAccountName,
								GetNonQualifiedAccountName((String)scopeCollection[0]["name"]));
							//
							iisObject.SetValue<string>(
								WebSite.WmSvcServiceUrl, ProviderSettings["WmSvc.ServiceUrl"]);
							//
							iisObject.SetValue<string>(
								WebSite.WmSvcServicePort, ProviderSettings["WmSvc.Port"]);
						}
					}
				}
			}
		}

		protected ConfigurationElement FindElement(ConfigurationElementCollection collection, string elementTagName, params string[] keyValues)
		{
			foreach (ConfigurationElement element in collection)
			{
				if (String.Equals(element.ElementTagName, elementTagName, StringComparison.OrdinalIgnoreCase))
				{
					bool matches = true;
					for (int i = 0; i < keyValues.Length; i += 2)
					{
						object o = element.GetAttributeValue(keyValues[i]);
						string value = null;
						if (o != null)
						{
							value = o.ToString();
						}
						if (!String.Equals(value, keyValues[i + 1], StringComparison.OrdinalIgnoreCase))
						{
							matches = false;
							break;
						}
					}
					if (matches)
					{
						return element;
					}
				}
			}
			return null;
		}

		private bool IsWebManagementAccessEnabled(WebVirtualDirectory iisObject)
		{
			using (var serverManager = webObjectsSvc.GetServerManager())
			{
				//
				string fqWebPath = String.Format("/{0}", iisObject.FullQualifiedPath);
				//
				Log.WriteInfo("FQ Web Path: " + fqWebPath);
				//
				Configuration config = serverManager.GetAdministrationConfiguration();
				ConfigurationSection authorizationSection = config.GetSection("system.webServer/management/authorization");
				ConfigurationElementCollection authorizationRulesCollection = authorizationSection.GetCollection("authorizationRules");

				ConfigurationElement scopeElement = FindElement(authorizationRulesCollection, "scope", "path", fqWebPath);

				if (scopeElement != null)
				{
					// At least one authorization rule exists
					if (scopeElement.GetCollection().Count > 0)
					{
						return true;
					}
				}
			}

			//
			return false;
		}

		private bool IsWebManagementServiceInstalled()
		{
			if (!isWmSvcInstalled.HasValue)
			{
				try
				{
					// Software key has been found, so report the check as succeeded
					if (PInvoke.RegistryHive.HKLM.SubKeyExists_x64(@"SOFTWARE\Microsoft\WebManagement\Server"))
					{
						// Make sure Enable Remote Connections flag is turned on.
						int remoteConnEnabled = PInvoke.RegistryHive.HKLM.GetDwordSubKeyValue_x64(@"SOFTWARE\Microsoft\WebManagement\Server",
							"EnableRemoteManagement");
						//
						if (remoteConnEnabled == 1)
							isWmSvcInstalled = true;
					}
					else
						isWmSvcInstalled = false;
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to determine whether Web Management Service is installed", ex);
				}	
			}

			// Service not installed
			return isWmSvcInstalled.GetValueOrDefault();
		}

		protected SettingPair[] GetWmSvcServerSettings()
		{
			List<SettingPair> settings = new List<SettingPair>();

			try
			{
				// Trying to retrieve Web Management Server key
				if (PInvoke.RegistryHive.HKLM.SubKeyExists_x64(@"SOFTWARE\Microsoft\WebManagement\Server"))
				{
					// Retrieve service port number
					settings.Add(new SettingPair
					{
						Name = "WmSvc.Port",
						Value = PInvoke.RegistryHive.HKLM.GetDwordSubKeyValue_x64(
@"SOFTWARE\Microsoft\WebManagement\Server", "Port").ToString()
					});

					// Retrieve service IP Address
					settings.Add(new SettingPair
					{
						Name = "WmSvc.ServiceUrl",
						Value = PInvoke.RegistryHive.HKLM.GetSubKeyValue_x64(
@"SOFTWARE\Microsoft\WebManagement\Server", "IPAddress")
					});
					//

					// Retrieve service credentials mode
					settings.Add(new SettingPair
					{
						Name = "WmSvc.RequiresWindowsCredentials",
						Value = PInvoke.RegistryHive.HKLM.GetDwordSubKeyValue_x64(
@"SOFTWARE\Microsoft\WebManagement\Server", "RequiresWindowsCredentials").ToString()
					});
					//
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Failed to retrieve Web Management Service settings", ex);
			}
			//
			return settings.ToArray();
		}

		protected string GetFullQualifiedAccountName(string accountName)
		{
			//
			if (!ServerSettings.ADEnabled)
				return String.Format(@"{0}\{1}", Environment.MachineName, accountName);

			if (accountName.IndexOf("\\") != -1)
				return accountName; // already has domain information

			// DO IT FOR ACTIVE DIRECTORY MODE ONLY
			string domainName = null;
			try
			{
				DirectoryContext objContext = new DirectoryContext(DirectoryContextType.Domain, ServerSettings.ADRootDomain);
				Domain objDomain = Domain.GetDomain(objContext);
				domainName = objDomain.Name;
			}
			catch (Exception ex)
			{
				Log.WriteError("Get domain name error", ex);
			}

			return domainName != null ? domainName + "\\" + accountName : accountName;
		}

		#endregion
	}
}
