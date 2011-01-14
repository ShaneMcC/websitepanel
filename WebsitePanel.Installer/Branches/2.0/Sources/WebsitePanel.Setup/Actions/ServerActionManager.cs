// Copyright (c) 2011, SMB SAAS Systems Inc.
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
// - Neither  the  name of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
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
using System.IO;
using WebsitePanel.Setup.Web;
using WebsitePanel.Setup.Windows;
using Ionic.Zip;
using System.Xml;

namespace WebsitePanel.Setup.Actions
{
	#region Actions

	public class CheckOperatingSystemAction : Action, IPrerequisiteAction
	{
		bool IPrerequisiteAction.Run(SetupVariables vars)
		{
			throw new NotImplementedException();
		}

		event EventHandler<ActionProgressEventArgs<bool>> IPrerequisiteAction.Complete
		{
			add { throw new NotImplementedException(); }
			remove { throw new NotImplementedException(); }
		}
	}

	public class CreateWindowsAccountAction : Action, IInstallAction, IUninstallAction
	{
		public const string UserAccountExists = "Account already exists";
		public const string UserAccountDescription = "{0} account for anonymous access to Internet Information Services";
		public const string LogStartMessage = "Creating Windows user account...";
		public const string LogInfoMessage = "Creating Windows user account \"{0}\"";
		public const string LogEndMessage = "Created windows user account";
		public const string InstallLogMessageLocal = "- Created a new Windows user account \"{0}\"";
		public const string InstallLogMessageDomain = "- Created a new Windows user account \"{0}\" in \"{1}\" domain";
		public const string LogStartRollbackMessage = "Removing Windows user account...";
		public const string LogInfoRollbackMessage = "Deleting user account \"{0}\"";
		public const string LogEndRollbackMessage = "User account has been removed";
		public const string LogInfoRollbackMessageDomain = "Could not find user account '{0}' in domain '{1}', thus consider it removed";
		public const string LogInfoRollbackMessageLocal = "Could not find user account '{0}', thus consider it removed";
		public const string LogErrorRollbackMessage = "Could not remove Windows user account";

		private void CreateUserAccount(SetupVariables vars)
		{
			//SetProgressText("Creating windows user account...");

			var domain = vars.UserDomain;
			var userName = vars.UserAccount;
			//
			var description = String.Format(UserAccountDescription, vars.ComponentName);
			var memberOf = vars.UserMembership;
			var password = vars.UserPassword;

			Log.WriteStart(LogStartMessage);

			Log.WriteInfo(String.Format(LogInfoMessage, userName));

			// create account
			SystemUserItem user = new SystemUserItem
			{
				Domain = domain,
				Name = userName,
				FullName = userName,
				Description = description,
				MemberOf = memberOf,
				Password = password,
				PasswordCantChange = true,
				PasswordNeverExpires = true,
				AccountDisabled = false,
				System = true
			};

			//
			SecurityUtils.CreateUser(user);

			// add rollback action
			//RollBack.RegisterUserAccountAction(domain, userName);

			// update log
			Log.WriteEnd(LogEndMessage);

			// update install log
			if (String.IsNullOrEmpty(domain))
				InstallLog.AppendLine(String.Format(InstallLogMessageLocal, userName));
			else
				InstallLog.AppendLine(String.Format(InstallLogMessageDomain, userName, domain));
		}

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			// Exit with an error if Windows account with the same name already exists
			if (SecurityUtils.UserExists(vars.UserDomain, vars.UserAccount))
				throw new Exception(UserAccountExists);
			//
			CreateUserAccount(vars);
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart(LogStartRollbackMessage);
				Log.WriteInfo(String.Format(LogInfoRollbackMessage, vars.UserAccount));
				//
				if (SecurityUtils.UserExists(vars.UserDomain, vars.UserAccount))
				{
					SecurityUtils.DeleteUser(vars.UserDomain, vars.UserAccount);
				}
				else
				{
					if (!String.IsNullOrEmpty(vars.UserDomain))
					{
						Log.WriteInfo(String.Format(LogInfoRollbackMessageDomain, vars.UserAccount, vars.UserDomain));
					}
					else
					{
						Log.WriteInfo(String.Format(LogInfoRollbackMessageLocal, vars.UserAccount));
					}
				}
				//
				Log.WriteEnd(LogEndRollbackMessage);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
				{
					return;
				}
				//
				Log.WriteError(LogErrorRollbackMessage, ex);
				throw;
			}
		}
	}

	public class ConfigureAspNetTempFolderPermissionsAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				string path;
				if (vars.IISVersion.Major == 6)
				{
					// IIS_WPG -> C:\WINDOWS\Temp
					path = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
					SetFolderPermission(path, "IIS_WPG", NtfsPermission.Modify);

					// IIS_WPG - > C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\Temporary ASP.NET Files
					path = Path.Combine(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory(),
						"Temporary ASP.NET Files");
					if (Utils.IsWin64() && Utils.IIS32Enabled())
						path = path.Replace("Framework64", "Framework");
					SetFolderPermission(path, "IIS_WPG", NtfsPermission.Modify);
				}
				// NETWORK_SERVICE -> C:\WINDOWS\Temp
				path = Environment.GetEnvironmentVariable("TMP", EnvironmentVariableTarget.Machine);
				//
				SetFolderPermissionBySid(path, SystemSID.NETWORK_SERVICE, NtfsPermission.Modify);

			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void SetFolderPermission(string path, string account, NtfsPermission permission)
		{
			try
			{
				if (!FileUtils.DirectoryExists(path))
				{
					FileUtils.CreateDirectory(path);
					Log.WriteInfo(string.Format("Created {0} folder", path));
				}

				Log.WriteStart(string.Format("Setting '{0}' permission for '{1}' folder for '{2}' account", permission, path, account));
				SecurityUtils.GrantNtfsPermissions(path, null, account, permission, true, true);
				Log.WriteEnd("Set security permissions");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}

		private void SetFolderPermissionBySid(string path, string account, NtfsPermission permission)
		{
			try
			{
				if (!FileUtils.DirectoryExists(path))
				{
					FileUtils.CreateDirectory(path);
					Log.WriteInfo(string.Format("Created {0} folder", path));
				}

				Log.WriteStart(string.Format("Setting '{0}' permission for '{1}' folder for '{2}' account", permission, path, account));
				SecurityUtils.GrantNtfsPermissionsBySid(path, account, permission, true, true);
				Log.WriteEnd("Set security permissions");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Security error", ex);
			}
		}
	}

	public class SetNtfsPermissionsAction : Action, IInstallAction
	{
		public const string LogStartInstallMessage = "Configuring folder permissions...";
		public const string LogEndInstallMessage = "NTFS permissions has been applied to the application folder...";
		public const string LogInstallErrorMessage = "Could not set content folder NTFS permissions";
		public const string FqdnIdentity = "{0}\\{1}";

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			string contentPath = vars.InstallationFolder;

			//creating user account
			string userName = vars.UserAccount;
			string userDomain = vars.UserDomain;
			string netbiosDomain = userDomain;
			//
			try
			{
				Begin(LogStartInstallMessage);
				//
				Log.WriteStart(LogStartInstallMessage);
				//
				if (!String.IsNullOrEmpty(userDomain))
				{
					netbiosDomain = SecurityUtils.GetNETBIOSDomainName(userDomain);
				}
				//
				WebUtils.SetWebFolderPermissions(contentPath, netbiosDomain, userName);
				//
				Log.WriteEnd(LogEndInstallMessage);
				//
				Finish(LogStartInstallMessage);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError(LogInstallErrorMessage, ex);

				throw;
			}
		}
	}

	public class CreateWebApplicationPoolAction : Action, IInstallAction, IUninstallAction
	{
		public const string AppPoolNameFormatString = "{0} Pool";
		public const string LogStartInstallMessage = "Creating application pool for the web site...";
		public const string LogStartUninstallMessage = "Removing application pool...";
		public const string LogUninstallAppPoolNotFoundMessage = "Application pool not found";

		public static string GetWebIdentity(SetupVariables vars)
		{
			var userDomain = vars.UserDomain;
			var netbiosDomain = userDomain;
			var userName = vars.UserAccount;
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major == 7);
			//
			if (!String.IsNullOrEmpty(userDomain))
			{
				netbiosDomain = SecurityUtils.GetNETBIOSDomainName(userDomain);
				//
				if (iis7)
				{
					//for iis7 we use fqdn\user
					return String.Format(SetNtfsPermissionsAction.FqdnIdentity, userDomain, userName);
				}
				else
				{
					//for iis6 we use netbiosdomain\user
					return String.Format(SetNtfsPermissionsAction.FqdnIdentity, netbiosDomain, userName);
				}
			}
			//
			return userName;
		}

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			var appPoolName = String.Format(AppPoolNameFormatString, vars.ComponentFullName);
			var userDomain = vars.UserDomain;
			var netbiosDomain = userDomain;
			var userName = vars.UserAccount;
			var userPassword = vars.UserPassword;
			var identity = GetWebIdentity(vars);
			var componentId = vars.ComponentId;
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major == 7);
			var poolExists = false;

			//
			vars.WebApplicationPoolName = appPoolName;

			// Maintain backward compatibility
			if (iis7)
			{
				poolExists = WebUtils.IIS7ApplicationPoolExists(appPoolName);
			}
			else
			{
				poolExists = WebUtils.ApplicationPoolExists(appPoolName);
			}

			// This flag is the opposite of poolExists flag
			vars.NewWebApplicationPool = !poolExists;

			if (poolExists)
			{
				//update app pool
				Log.WriteStart("Updating application pool");
				Log.WriteInfo(String.Format("Updating application pool \"{0}\"", appPoolName));
				//
				if (iis7)
				{
					WebUtils.UpdateIIS7ApplicationPool(appPoolName, userName, userPassword);
				}
				else
				{
					WebUtils.UpdateApplicationPool(appPoolName, userName, userPassword);
				}

				//
				//update log
				Log.WriteEnd("Updated application pool");

				//update install log
				InstallLog.AppendLine(String.Format("- Updated application pool named \"{0}\"", appPoolName));
			}
			else
			{
				// create app pool
				Log.WriteStart("Creating application pool");
				Log.WriteInfo(String.Format("Creating application pool \"{0}\"", appPoolName));
				//
				if (iis7)
				{
					WebUtils.CreateIIS7ApplicationPool(appPoolName, userName, userPassword);
				}
				else
				{
					WebUtils.CreateApplicationPool(appPoolName, userName, userPassword);
				}

				//update log
				Log.WriteEnd("Created application pool");

				//update install log
				InstallLog.AppendLine(String.Format("- Created a new application pool named \"{0}\"", appPoolName));
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				var appPoolName = String.Format(AppPoolNameFormatString, vars.ComponentFullName);
				var iisVersion = vars.IISVersion;
				var iis7 = (iisVersion.Major == 7);
				var poolExists = false;
				//
				Log.WriteStart(LogStartUninstallMessage);
				//
				vars.WebApplicationPoolName = appPoolName;

				// Maintain backward compatibility
				if (iis7)
				{
					poolExists = WebUtils.IIS7ApplicationPoolExists(appPoolName);
				}
				else
				{
					poolExists = WebUtils.ApplicationPoolExists(appPoolName);
				}

				if (!poolExists)
				{
					Log.WriteInfo(LogUninstallAppPoolNotFoundMessage);
					return;
				}
				//
				if (iis7)
				{
					WebUtils.DeleteIIS7ApplicationPool(appPoolName);
				}
				else
				{
					WebUtils.DeleteApplicationPool(appPoolName);
				}
				//update install log
				InstallLog.AppendLine(String.Format("- Removed application pool named \"{0}\"", appPoolName));
			}
			finally
			{
				//update log
				//Log.WriteEnd(LogEndUninstallMessage);
			}
		}
	}

	public class CreateWebSiteAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartMessage = "Creating web site...";
		public const string LogEndMessage = "";

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			var siteName = vars.ComponentFullName;
			var ip = vars.WebSiteIP;
			var port = vars.WebSitePort;
			var domain = vars.WebSiteDomain;
			var contentPath = vars.InstallationFolder;
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major == 7);
			var userName = CreateWebApplicationPoolAction.GetWebIdentity(vars);
			var userPassword = vars.UserPassword;
			var appPool = vars.WebApplicationPoolName;
			var componentId = vars.ComponentId;
			var newSiteId = String.Empty;
			//
			Begin(LogStartMessage);
			//
			Log.WriteStart(LogStartMessage);
			//
			Log.WriteInfo(String.Format("Creating web site \"{0}\" ( IP: {1}, Port: {2}, Domain: {3} )", siteName, ip, port, domain));

			//check for existing site
			var oldSiteId = iis7 ? WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) : WebUtils.GetSiteIdByBinding(ip, port, domain);
			//
			if (oldSiteId != null)
			{
				// get site name
				string oldSiteName = iis7 ? oldSiteId : WebUtils.GetSite(oldSiteId).Name;
				throw new Exception(
					String.Format("'{0}' web site already has server binding ( IP: {1}, Port: {2}, Domain: {3} )",
					oldSiteName, ip, port, domain));
			}

			// create site
			var site = new WebSiteItem
			{
				Name = siteName,
				SiteIPAddress = ip,
				ContentPath = contentPath,
				AllowExecuteAccess = false,
				AllowScriptAccess = true,
				AllowSourceAccess = false,
				AllowReadAccess = true,
				AllowWriteAccess = false,
				AnonymousUsername = userName,
				AnonymousUserPassword = userPassword,
				AllowDirectoryBrowsingAccess = false,
				AuthAnonymous = true,
				AuthWindows = true,
				DefaultDocs = null,
				HttpRedirect = "",
				InstalledDotNetFramework = AspNetVersion.AspNet20,
				ApplicationPool = appPool,
				//
				Bindings = new ServerBinding[] {
					new ServerBinding(ip, port, domain)
				},
			};

			// create site
			if (iis7)
			{
				newSiteId = WebUtils.CreateIIS7Site(site);
			}
			else
			{
				newSiteId = WebUtils.CreateSite(site);
			}

			vars.VirtualDirectory = String.Empty;
			vars.NewWebSite = true;
			vars.NewVirtualDirectory = false;

			// update setup variables
			vars.WebSiteId = newSiteId;

			//update log
			Log.WriteEnd("Created web site");
			//
			Finish(LogStartMessage);

			//update install log
			InstallLog.AppendLine(string.Format("- Created a new web site named \"{0}\" ({1})", siteName, newSiteId));
			InstallLog.AppendLine("  You can access the application by the following URLs:");
			string[] urls = Utils.GetApplicationUrls(ip, domain, port, null);
			foreach (string url in urls)
			{
				InstallLog.AppendLine("  http://" + url);
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			var iisVersion = vars.IISVersion;
			var iis7 = (iisVersion.Major == 7);
			var siteId = vars.WebSiteId;
			//
			try
			{
				Log.WriteStart("Deleting web site");
				Log.WriteInfo(String.Format("Deleting web site \"{0}\"", siteId));
				if (iis7)
				{
					if (WebUtils.IIS7SiteExists(siteId))
					{
						WebUtils.DeleteIIS7Site(siteId);
						Log.WriteEnd("Deleted web site");
					}
				}
				else
				{
					if (WebUtils.SiteIdExists(siteId))
					{
						WebUtils.DeleteSite(siteId);
						Log.WriteEnd("Deleted web site");
					}
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Web site delete error", ex);
				throw;
			}
		}
	}

	public class CopyFilesAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartInstallMessage = "Copying files...";
		public const string LogStartUninstallMessage = "Deleting files copied...";

		internal void DoFilesCopyProcess(string source, string destination)
		{
			var sourceFolder = new DirectoryInfo(source);
			var destFolder = new DirectoryInfo(destination);
			// unzip
			long totalSize = FileUtils.CalculateFolderSize(sourceFolder.FullName);
			long copied = 0;

			int i = 0;
			List<DirectoryInfo> folders = new List<DirectoryInfo>();
			List<FileInfo> files = new List<FileInfo>();

			DirectoryInfo di = null;
			//FileInfo fi = null;
			string path = null;

			// Part 1: Indexing
			folders.Add(sourceFolder);
			while (i < folders.Count)
			{
				foreach (DirectoryInfo info in folders[i].GetDirectories())
				{
					if (!folders.Contains(info))
						folders.Add(info);
				}
				foreach (FileInfo info in folders[i].GetFiles())
				{
					files.Add(info);
				}
				i++;
			}

			// Part 2: Destination Folders Creation
			///////////////////////////////////////////////////////
			for (i = 0; i < folders.Count; i++)
			{
				if (folders[i].Exists)
				{
					path = destFolder.FullName +
						Path.DirectorySeparatorChar +
						folders[i].FullName.Remove(0, sourceFolder.FullName.Length);

					di = new DirectoryInfo(path);

					// Prevent IOException
					if (!di.Exists)
						di.Create();
				}
			}

			// Part 3: Source to Destination File Copy
			///////////////////////////////////////////////////////
			for (i = 0; i < files.Count; i++)
			{
				if (files[i].Exists)
				{
					path = destFolder.FullName +
						Path.DirectorySeparatorChar +
						files[i].FullName.Remove(0, sourceFolder.FullName.Length + 1);
					FileUtils.CopyFile(files[i], path);
					copied += files[i].Length;
					if (totalSize != 0)
					{
						// Update progress
						OnInstallProgressChanged(files[i].Name, Convert.ToInt32(copied * 100 / totalSize));
					}
				}
			}
		}

		public override bool Indeterminate
		{
			get { return false; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Begin(LogStartInstallMessage);
				//
				var source = vars.InstallerFolder;
				var destination = vars.InstallationFolder;
				//
				string component = vars.ComponentFullName;
				//
				Log.WriteStart(LogStartInstallMessage);
				Log.WriteInfo(String.Format("Copying files from \"{0}\" to \"{1}\"", source, destination));
				//showing copy process
				DoFilesCopyProcess(source, destination);
				//
				InstallLog.AppendLine(String.Format("- Copied {0} files", component));
				// rollback
				//RollBack.RegisterDirectoryAction(destination);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				Log.WriteError("Copy error", ex);
				throw;
			}
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				var path = vars.InstallationFolder;
				//
				Log.WriteStart(LogStartUninstallMessage);
				Log.WriteInfo(String.Format("Deleting directory \"{0}\"", path));

				if (FileUtils.DirectoryExists(path))
				{
					FileUtils.DeleteDirectory(path);
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Directory delete error", ex);

				throw;
			}
		}
	}

	public class SetServerPasswordAction : Action, IInstallAction
	{
		public const string LogStartInstallMessage = "Setting server password...";

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Begin(LogStartInstallMessage);
				//
				Log.WriteStart("Updating configuration file (server password)");
				Log.WriteInfo(String.Format("Server password is: '{0}'", vars.ServerPassword));
				Log.WriteInfo("Single quotes are added for clarity purposes");
				//
				string file = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
				string hash = Utils.ComputeSHA1(vars.ServerPassword);
				// load file
				string content = string.Empty;
				using (StreamReader reader = new StreamReader(file))
				{
					content = reader.ReadToEnd();
				}

				// expand variables
				content = Utils.ReplaceScriptVariable(content, "installer.server.password", hash);

				// save file
				using (StreamWriter writer = new StreamWriter(file))
				{
					writer.Write(content);
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Configuration file update error", ex);

				throw;
			}
		}
	}

	public class SetCommonDistributiveParamsAction : Action, IPrepareDefaultsAction
	{
		public override bool Indeterminate
		{
			get { return true; }
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.InstallationFolder))
				vars.InstallationFolder = String.Format(@"C:\WebsitePanel\{0}", vars.ComponentName);
			//
			if (String.IsNullOrEmpty(vars.WebSiteDomain))
				vars.WebSiteDomain = String.Empty;
			// Force create new web site
			vars.NewWebSite = true;
			vars.NewVirtualDirectory = false;
			//
			if (String.IsNullOrEmpty(vars.ConfigurationFile))
				vars.ConfigurationFile = "web.config";
		}
	}

	public class EnsureServiceAccntSecured : Action, IPrepareDefaultsAction
	{
		public const string LogStartMessage = "Verifying setup parameters...";

		public override bool Indeterminate
		{
			get { return true; }
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//Begin(LogStartMessage);
			//
			if (!String.IsNullOrEmpty(vars.UserPassword))
				return;
			//
			vars.UserPassword = Guid.NewGuid().ToString();
			//
			//Finish(LogEndMessage);
		}
	}

	public class SetServerDefaultInstallationSettingsAction : Action, IPrepareDefaultsAction
	{
		public override bool Indeterminate
		{
			get { return true; }
		}

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSiteIP))
				vars.WebSiteIP = Global.Server.DefaultIP;
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = Global.Server.DefaultPort;
			//
			if (string.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = Global.Server.ServiceAccount;
		}
	}

	public class SaveComponentConfigSettingsAction : Action, IInstallAction, IUninstallAction
	{
		#region Uninstall
		public const string LogStartUninstallMessage = "Removing \"{0}\" component's configuration details";
		public const string LogUninstallInfoMessage = "Deleting \"{0}\" component settings";
		public const string LogErrorUninstallMessage = "Failed to remove the component configuration details";
		public const string LogEndUninstallMessage = "Component's configuration has been removed";
		#endregion

		public const string LogStartInstallMessage = "Updating system configuration...";

		void IInstallAction.Run(SetupVariables vars)
		{
			Begin(LogStartInstallMessage);
			//
			Log.WriteStart(LogStartInstallMessage);
			//
			AppConfig.EnsureComponentConfig(vars.ComponentId);
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ApplicationName", vars.ApplicationName);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ComponentCode", vars.ComponentCode);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ComponentName", vars.ComponentName);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ComponentDescription", vars.ComponentDescription);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "Release", vars.Version);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "Instance", vars.Instance);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "InstallFolder", vars.InstallationFolder);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "Installer", vars.Installer);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "InstallerType", vars.InstallerType);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "InstallerPath", vars.InstallerPath);
			// update config setings
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewApplicationPool", vars.NewWebApplicationPool);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ApplicationPool", vars.WebApplicationPoolName);
			// update config setings
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "WebSiteId", vars.WebSiteId);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "WebSiteIP", vars.WebSiteIP);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "WebSitePort", vars.WebSitePort);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "WebSiteDomain", vars.WebSiteDomain);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "VirtualDirectory", vars.VirtualDirectory);
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewWebSite", vars.NewWebSite);
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewVirtualDirectory", vars.NewVirtualDirectory);
			//
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewUserAccount", true);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "UserAccount", vars.UserAccount);
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "Domain", vars.UserDomain);
			//
			AppConfig.SaveConfiguration();
		}

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart(LogStartUninstallMessage);

				Log.WriteInfo(String.Format(LogUninstallInfoMessage, vars.ComponentFullName));

				XmlUtils.RemoveXmlNode(AppConfig.GetComponentConfig(vars.ComponentId));

				AppConfig.SaveConfiguration();

				Log.WriteEnd(LogEndUninstallMessage);

				InstallLog.AppendLine("- Updated system configuration");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
				{
					return;
				}
				//
				Log.WriteError(LogErrorUninstallMessage, ex);
				throw;
			}
		}
	}

	#endregion

	public class RaiseExceptionAction : Action, IInstallAction
	{
		public override bool Indeterminate
		{
			get { return false; }
		}

		void IInstallAction.Run(SetupVariables vars)
		{
			throw new NotImplementedException();
		}
	}

	public class ServerActionManager : BaseActionManager
	{
		public static readonly List<Action> InstallScenario = new List<Action>
		{
			new SetCommonDistributiveParamsAction(),
			new SetServerDefaultInstallationSettingsAction(),
			new EnsureServiceAccntSecured(),
			new CopyFilesAction(),
			new SetServerPasswordAction(),
			new CreateWindowsAccountAction(),
			new ConfigureAspNetTempFolderPermissionsAction(),
			new SetNtfsPermissionsAction(),
			new CreateWebApplicationPoolAction(),
			new CreateWebSiteAction(),
			new SaveComponentConfigSettingsAction()
		};

		public ServerActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(ServerActionManager_Initialize);
		}

		void ServerActionManager_Initialize(object sender, EventArgs e)
		{
			//
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install: // Install
					LoadInstallationScenario();
					break;
				default:
					break;
			}
		}

		protected virtual void LoadInstallationScenario()
		{
			CurrentScenario.AddRange(InstallScenario);
		}
	}	
}
