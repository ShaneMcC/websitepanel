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
using System.IO;
using System.Xml;
using System.Configuration;
using System.Windows.Forms;
using System.Collections;
using System.Text;
using WebsitePanel.Setup.Web;

namespace WebsitePanel.Setup
{
	public class StandaloneServerSetup : BaseSetup
	{
		public static DialogResult Install(object obj)
		{
			return InstallBase(obj, "1.0.6");
		}

		internal static DialogResult InstallBase(object obj, string minimalInstallerVersion)
		{
			Hashtable args = Utils.GetSetupParameters(obj);

			//check CS version
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			Version version = new Version(shellVersion);
			if (version < new Version(minimalInstallerVersion))
			{
				MessageBox.Show(
					string.Format("WebsitePanel Installer {0} or higher required.", minimalInstallerVersion),
					"Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return DialogResult.Cancel;
			}

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			//load config file
			AppConfig.LoadConfiguration();

			//general settings
			wizard.SetupVariables.ApplicationName = Utils.GetStringSetupParameter(args, "ApplicationName");
			wizard.SetupVariables.Version = Utils.GetStringSetupParameter(args, "Version");
			wizard.SetupVariables.Installer = Utils.GetStringSetupParameter(args, "Installer");
			wizard.SetupVariables.InstallerPath = Utils.GetStringSetupParameter(args, "InstallerPath");
			wizard.SetupVariables.IISVersion = Utils.GetVersionSetupParameter(args, "IISVersion");
			wizard.SetupVariables.SetupAction = SetupActions.Install;
			wizard.SetupVariables.SetupXml = Utils.GetStringSetupParameter(args, "SetupXml");
		

			//********************  Server ****************
			//general settings 
			string serverId = Guid.NewGuid().ToString();
			wizard.SetupVariables.ServerComponentId = serverId;
			wizard.SetupVariables.ComponentId = serverId;
			wizard.SetupVariables.Instance = string.Empty;
			wizard.SetupVariables.ComponentName = "Server";
			wizard.SetupVariables.ComponentCode = "server";
			wizard.SetupVariables.ComponentDescription = "WebsitePanel Server is a set of services running on the remote server to be controlled. Server application should be reachable from Enterprise Server one.";
			wizard.SetupVariables.InstallerFolder = Path.Combine(Utils.GetStringSetupParameter(args, "InstallerFolder"), "Server");
			wizard.SetupVariables.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType").Replace("StandaloneServerSetup", "Server");
			wizard.SetupVariables.InstallationFolder = Path.Combine(Utils.GetSystemDrive(), "WebsitePanel\\Server");
			//web settings
			wizard.SetupVariables.UserAccount = "WPServer";
			wizard.SetupVariables.UserPassword = Guid.NewGuid().ToString("P");
			wizard.SetupVariables.UserDomain = null;
			wizard.SetupVariables.WebSiteIP = "127.0.0.1";
			wizard.SetupVariables.WebSitePort = "9003";
			wizard.SetupVariables.WebSiteDomain = string.Empty;
			wizard.SetupVariables.NewWebSite = true;
			wizard.SetupVariables.NewVirtualDirectory = false;
			wizard.SetupVariables.UserMembership = (wizard.SetupVariables.IISVersion.Major == 7) ?
				new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_IUSRS" } :
				new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_WPG" };
			wizard.SetupVariables.ConfigurationFile = "web.config";
			
			wizard.SetupVariables.UpdateServerPassword = true;

			//Unattended setup
			LoadComponentVariablesFromSetupXml(wizard.SetupVariables.ComponentCode, wizard.SetupVariables.SetupXml, wizard.SetupVariables);

			//create component settings node
			wizard.SetupVariables.ComponentConfig = AppConfig.CreateComponentConfig(serverId);
			//write component settings to xml
			CreateComponentSettingsFromSetupVariables(wizard.SetupVariables, serverId);
			//save settings
			SetupVariables serverSetupVariables = wizard.SetupVariables.Clone();

			//server password
			string serverPassword = serverSetupVariables.ServerPassword;
			if ( string.IsNullOrEmpty(serverPassword))
			{
				serverPassword = Guid.NewGuid().ToString("N").Substring(0, 10);
			}
			serverSetupVariables.ServerPassword = Utils.ComputeSHA1(serverPassword);
			//add password to config
			AppConfig.SetComponentSettingStringValue(serverSetupVariables.ComponentId, "Password", serverSetupVariables.ServerPassword);
			wizard.SetupVariables.RemoteServerPassword = serverPassword;
			//server url
			wizard.SetupVariables.RemoteServerUrl = GetUrl(serverSetupVariables.WebSiteDomain, serverSetupVariables.WebSiteIP, serverSetupVariables.WebSitePort);

			//********************  Portal ****************
			//general settings
			string portalId = Guid.NewGuid().ToString();
			wizard.SetupVariables.PortalComponentId = portalId;
			wizard.SetupVariables.ComponentId = portalId;
			wizard.SetupVariables.Instance = string.Empty;
			wizard.SetupVariables.ComponentName = "Portal";
			wizard.SetupVariables.ComponentCode = "portal";
			wizard.SetupVariables.ComponentDescription = "WebsitePanel Portal is a control panel itself with user interface which allows managing user accounts, hosting spaces, web sites, FTP accounts, files, etc.";
			wizard.SetupVariables.InstallerFolder = Path.Combine(Utils.GetStringSetupParameter(args, "InstallerFolder"), "Portal");
			wizard.SetupVariables.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType").Replace("StandaloneServerSetup", "Portal");
			wizard.SetupVariables.InstallationFolder = Path.Combine(Utils.GetSystemDrive(), "WebsitePanel\\Portal");
			//web settings
			wizard.SetupVariables.UserAccount = "WPPortal";
			wizard.SetupVariables.UserPassword = Guid.NewGuid().ToString("P");
			wizard.SetupVariables.UserDomain = null;
			wizard.SetupVariables.WebSiteIP = string.Empty; //empty - to detect IP 
			wizard.SetupVariables.WebSitePort = "9001";
			wizard.SetupVariables.WebSiteDomain = string.Empty;
			wizard.SetupVariables.NewWebSite = true;
			wizard.SetupVariables.NewVirtualDirectory = false;
			wizard.SetupVariables.UserMembership = (wizard.SetupVariables.IISVersion.Major == 7) ?
				new string[] { "IIS_IUSRS" } :
				new string[] { "IIS_WPG" };
			wizard.SetupVariables.ConfigurationFile = "web.config";
			//ES url
			wizard.SetupVariables.EnterpriseServerURL = "http://127.0.0.1:9002";
			
			//Unattended setup
			LoadComponentVariablesFromSetupXml(wizard.SetupVariables.ComponentCode, wizard.SetupVariables.SetupXml, wizard.SetupVariables);

			//create component settings node
			wizard.SetupVariables.ComponentConfig = AppConfig.CreateComponentConfig(portalId);
			//add default component settings
			CreateComponentSettingsFromSetupVariables(wizard.SetupVariables, portalId);
			//save settings
			SetupVariables portalSetupVariables = wizard.SetupVariables.Clone();

			//********************  Enterprise Server ****************
			//general settings 
			string enterpriseServerId = Guid.NewGuid().ToString();
			wizard.SetupVariables.EnterpriseServerComponentId = enterpriseServerId;
			wizard.SetupVariables.ComponentId = enterpriseServerId;
			wizard.SetupVariables.Instance = string.Empty;
			wizard.SetupVariables.ComponentName = "Enterprise Server";
			wizard.SetupVariables.ComponentCode = "enterprise server";
			wizard.SetupVariables.ComponentDescription = "Enterprise Server is the heart of WebsitePanel system. It includes all business logic of the application. Enterprise Server should have access to Server and be accessible from Portal applications.";
			wizard.SetupVariables.InstallerFolder = Path.Combine(Utils.GetStringSetupParameter(args, "InstallerFolder"), "Enterprise Server");
			wizard.SetupVariables.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType").Replace("StandaloneServerSetup", "EnterpriseServer");
			wizard.SetupVariables.InstallationFolder = Path.Combine(Utils.GetSystemDrive(), "WebsitePanel\\Enterprise Server");
			//web settings
			wizard.SetupVariables.UserAccount = "WPEnterpriseServer";
			wizard.SetupVariables.UserPassword = Guid.NewGuid().ToString("P");
			wizard.SetupVariables.UserDomain = null;
			wizard.SetupVariables.WebSiteIP = "127.0.0.1";
			wizard.SetupVariables.WebSitePort = "9002";
			wizard.SetupVariables.WebSiteDomain = string.Empty;
			wizard.SetupVariables.NewWebSite = true;
			wizard.SetupVariables.NewVirtualDirectory = false;
			wizard.SetupVariables.UserMembership = (wizard.SetupVariables.IISVersion.Major == 7) ?
				new string[] { "IIS_IUSRS" } :
				new string[] { "IIS_WPG" };
			wizard.SetupVariables.ConfigurationFile = "web.config";
			//db settings
			wizard.SetupVariables.DatabaseServer = "localhost\\sqlexpress";
			wizard.SetupVariables.Database = "WebsitePanel";
			wizard.SetupVariables.CreateDatabase = true;
			//serveradmin settings
			wizard.SetupVariables.UpdateServerAdminPassword = true;
			wizard.SetupVariables.ServerAdminPassword = string.Empty;
			
			//Unattended setup
			LoadComponentVariablesFromSetupXml(wizard.SetupVariables.ComponentCode, wizard.SetupVariables.SetupXml, wizard.SetupVariables);

			//create component settings node
			wizard.SetupVariables.ComponentConfig = AppConfig.CreateComponentConfig(enterpriseServerId);
			//add default component settings
			CreateComponentSettingsFromSetupVariables(wizard.SetupVariables, enterpriseServerId);
			//save settings
			SetupVariables enterpiseServerSetupVariables = wizard.SetupVariables.Clone();
			///////////////////////
		
			//create wizard pages
			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ConfigurationCheckPage page2 = new ConfigurationCheckPage();
			ConfigurationCheck check1 = new ConfigurationCheck(CheckTypes.OperationSystem, "Operating System Requirement");
			ConfigurationCheck check2 = new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement");
			ConfigurationCheck check3 = new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement");
			ConfigurationCheck check4 = new ConfigurationCheck(CheckTypes.WPServer, "WebsitePanel Server Requirement");
			check4.SetupVariables = serverSetupVariables;
			ConfigurationCheck check5 = new ConfigurationCheck(CheckTypes.WPEnterpriseServer, "WebsitePanel Enterprise Server Requirement");
			check5.SetupVariables = enterpiseServerSetupVariables;
			ConfigurationCheck check6 = new ConfigurationCheck(CheckTypes.WPPortal, "WebsitePanel Portal Requirement");
			check6.SetupVariables = portalSetupVariables;

			page2.Checks.AddRange(new ConfigurationCheck[] { check1, check2, check3, check4, check5, check6 });
			WebPage page3 = new WebPage();
			//use portal settings for this page
			page3.SetupVariables = portalSetupVariables;
			DatabasePage page4 = new DatabasePage();
			//use ES settings for this page
			page4.SetupVariables = enterpiseServerSetupVariables;
			ServerAdminPasswordPage page5 = new ServerAdminPasswordPage();
			page5.SetupVariables = enterpiseServerSetupVariables;
			page5.NoteText = "Note: Both serveradmin and admin accounts will use this password. You can always change password for serveradmin or admin accounts through control panel."; 
			ExpressInstallPage page6 = new ExpressInstallPage();
			wizard.SetupVariables.ComponentName = string.Empty;

			//create install actions

			//************ Server **************
			InstallAction action = new InstallAction(ActionTypes.InitSetupVariables);
			action.Description = "Installing WebsitePanel Server...";
			action.SetupVariables = serverSetupVariables;
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateWebSite);
			action.Description = "Creating web site...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.FolderPermissions);
			action.Description = "Configuring folder permissions...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.ServerPassword);
			action.Description = "Setting server password...";
			page6.Actions.Add(action);

			//************* Enterprise server *********
			action = new InstallAction(ActionTypes.InitSetupVariables);
			action.Description = "Installing WebsitePanel Enterprise Server...";
			action.SetupVariables = enterpiseServerSetupVariables;
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateWebSite);
			action.Description = "Creating web site...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CryptoKey);
			action.Description = "Generating crypto key...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateDatabase);
			action.Description = "Creating SQL Server database...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateDatabaseUser);
			action.Description = "Creating SQL Server user...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.ExecuteSql);
			action.Description = "Creating database objects...";
			action.Path = "setup\\install_db.sql";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateWPServerLogin);
			action.Description = "Creating WebsitePanel login...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerAdminPassword);
			action.Description = "Updating serveradmin password...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateLicenseInformation);
			action.Description = "Updating license information...";
			page6.Actions.Add(action);

			//************* Portal *********
			action = new InstallAction(ActionTypes.InitSetupVariables);
			action.Description = "Installing WebsitePanel Portal...";
			action.SetupVariables = portalSetupVariables;
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CopyWebConfig);
			action.Description = "Copying web.config...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateWebSite);
			action.Description = "Creating web site...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateEnterpriseServerUrl);
			action.Description = "Updating site settings...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.CreateShortcuts);
			action.Description = "Creating shortcut...";
			page6.Actions.Add(action);

			//************* Standalone server provisioning *********
			action = new InstallAction(ActionTypes.InitSetupVariables);
			action.SetupVariables = enterpiseServerSetupVariables;
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.ConfigureStandaloneServerData);
			action.Description = "Configuring server data...";
			action.Url = portalSetupVariables.EnterpriseServerURL;
			page6.Actions.Add(action);

			SetupCompletePage page7 = new SetupCompletePage();
			page7.SetupVariables = portalSetupVariables;
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3, page4, page5, page6, page7 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult Uninstall(object obj)
		{
			MessageBox.Show("Functionality is not supported.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return DialogResult.Cancel;
		}

		public static DialogResult Setup(object obj)
		{
			MessageBox.Show("Functionality is not supported.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return DialogResult.Cancel;
		}

		public static DialogResult Update(object obj)
		{
			MessageBox.Show("Functionality is not supported.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return DialogResult.Cancel;
		}

		protected static void LoadComponentVariablesFromSetupXml(string componentCode, string xml, SetupVariables setupVariables)
		{
			if (string.IsNullOrEmpty(componentCode))
				return;

			if (string.IsNullOrEmpty(xml))
				return;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);

				string xpath = string.Format("components/component[@code=\"{0}\"]", componentCode);

				XmlNode componentNode = doc.SelectSingleNode(xpath);
				if (componentNode != null)
				{
					LoadSetupVariablesFromSetupXml(componentNode.InnerXml, setupVariables);
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Unattended setup error", ex);
				throw;
			}
		}

		private static string GetUrl(string domain, string ip, string port)
		{
			string address = "http://";
			string server = string.Empty;
			string ipPort = string.Empty;
			//server 
			if (domain != null && domain.Trim().Length > 0)
			{
				//domain 
				server = domain.Trim();
			}
			else
			{
				//ip
				if (ip != null && ip.Trim().Length > 0)
				{
					server = ip.Trim();
				}
			}
			//port
			if (server.Length > 0 &&
				ip.Trim().Length > 0 &&
				ip.Trim() != "80")
			{
				ipPort = ":" + port.Trim();
			}

			//address string
			address += server + ipPort;
			return address;
		}

		

	}
}
