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

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Version 1.0
	/// </summary>
	public class Server10 : Server
	{
		public static new DialogResult Install(object obj)
		{
			return Server.InstallBase(obj, "1.0.0");
		}

		public static new DialogResult Uninstall(object obj)
		{
			return Server.Uninstall(obj);
		}

		public static new DialogResult Setup(object obj)
		{
			return Server.Setup(obj);
		}
	}
	
	public class Server : BaseSetup
	{
		public static DialogResult Install(object obj)
		{
			return InstallBase(obj, "1.0.1");
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
			InitInstall(args, wizard);

			//web settings
			wizard.SetupVariables.WebSiteIP = "127.0.0.1";
			wizard.SetupVariables.WebSitePort = "9003";
			wizard.SetupVariables.WebSiteDomain = string.Empty;
			wizard.SetupVariables.NewWebSite = true;
			wizard.SetupVariables.NewVirtualDirectory = false;
			if(wizard.SetupVariables.IISVersion.Major == 7)
				wizard.SetupVariables.UserMembership = new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_IUSRS" };
			else
				wizard.SetupVariables.UserMembership = new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_WPG" };
			wizard.SetupVariables.ConfigurationFile = "web.config";

			//Unattended setup
			LoadSetupVariablesFromSetupXml(wizard.SetupVariables.SetupXml, wizard.SetupVariables);

			//create wizard pages
			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ConfigurationCheckPage page1 = new ConfigurationCheckPage();
			ConfigurationCheck check1 = new ConfigurationCheck(CheckTypes.OperationSystem, "Operating System Requirement");
			ConfigurationCheck check2 = new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement");
			ConfigurationCheck check3 = new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement");
			page1.Checks.AddRange(new ConfigurationCheck[] { check1, check2, check3 });
			InstallFolderPage page2 = new InstallFolderPage();
			WebPage page3 = new WebPage();
			UserAccountPage page4 = new UserAccountPage();
			ServerPasswordPage page5 = new ServerPasswordPage();
			ExpressInstallPage page6 = new ExpressInstallPage();
		
			//create install actions
			InstallAction action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page6.Actions.Add(action);


			action = new InstallAction(ActionTypes.CreateWebSite);
			action.Description = "Creating web site...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.FolderPermissions);
			action.Description = "Configuring folder permissions...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.ServerPassword);
			action.Description = "Updating server password...";
			page6.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page6.Actions.Add(action);

			
			FinishPage page7 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, page6, page7 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;
			
			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}



		public static DialogResult Uninstall(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			string componentId = Utils.GetStringSetupParameter(args, "ComponentId");
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables.SetupAction = SetupActions.Uninstall;
			wizard.SetupVariables.IISVersion = Utils.GetVersionSetupParameter(args, "IISVersion");
			if (wizard.SetupVariables.IISVersion.Major == 7)
				wizard.SetupVariables.UserMembership = new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_IUSRS" };
			else
				wizard.SetupVariables.UserMembership = new string[] { "AD:Domain Admins", "SID:" + SystemSID.ADMINISTRATORS, "IIS_WPG" };

			LoadSetupVariablesFromConfig(wizard, componentId);

			IntroductionPage page1 = new IntroductionPage();
			ConfirmUninstallPage page2 = new ConfirmUninstallPage();
			UninstallPage page3 = new UninstallPage();
			page2.UninstallPage = page3;
			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;

			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult Setup(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			string componentId = Utils.GetStringSetupParameter(args, "ComponentId");
			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			wizard.SetupVariables.SetupAction = SetupActions.Setup;
			LoadSetupVariablesFromConfig(wizard, componentId);
			wizard.SetupVariables.WebSiteId = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteId");
			wizard.SetupVariables.WebSiteIP = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteIP");
			wizard.SetupVariables.WebSitePort = AppConfig.GetComponentSettingStringValue(componentId, "WebSitePort");
			wizard.SetupVariables.WebSiteDomain = AppConfig.GetComponentSettingStringValue(componentId, "WebSiteDomain");
			wizard.SetupVariables.NewWebSite = AppConfig.GetComponentSettingBooleanValue(componentId, "NewWebSite");
			wizard.SetupVariables.NewVirtualDirectory = AppConfig.GetComponentSettingBooleanValue(componentId, "NewVirtualDirectory");
			wizard.SetupVariables.VirtualDirectory = AppConfig.GetComponentSettingStringValue(componentId, "VirtualDirectory");
			wizard.SetupVariables.IISVersion = Utils.GetVersionSetupParameter(args, "IISVersion");
			wizard.SetupVariables.ConfigurationFile = "web.config";

			//IntroductionPage page1 = new IntroductionPage();
			WebPage page1 = new WebPage();
			ServerPasswordPage page2 = new ServerPasswordPage();
			ExpressInstallPage page3 = new ExpressInstallPage();
			//create install actions
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page3.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerPassword);
			action.Description = "Updating server password...";
			page3.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page3.Actions.Add(action);

			FinishPage page4 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { page1, page2, page3, page4 });
			wizard.LinkPages();
			wizard.SelectedPage = page1;


			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}

		public static DialogResult Update(object obj)
		{
			Hashtable args = Utils.GetSetupParameters(obj);
			string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");

			Version version = new Version(shellVersion);
			if ( version < new Version("1.0.1"))
			{
				MessageBox.Show("WebsitePanel Installer 1.0.1 or higher required.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return DialogResult.Cancel;
			}
			string componentId = Utils.GetStringSetupParameter(args, "ComponentId");

			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			LoadSetupVariablesFromConfig(wizard, componentId);
			if (wizard.SetupVariables.Version != "1.5.3")
			{
				MessageBox.Show("Please update to version 1.5.3", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return DialogResult.Cancel;
			}

			wizard.SetupVariables.SetupAction = SetupActions.Update;
			wizard.SetupVariables.BaseDirectory = Utils.GetStringSetupParameter(args, "BaseDirectory");
			wizard.SetupVariables.UpdateVersion = Utils.GetStringSetupParameter(args, "UpdateVersion");
			wizard.SetupVariables.InstallerFolder = Utils.GetStringSetupParameter(args, "InstallerFolder");
			wizard.SetupVariables.Installer = Utils.GetStringSetupParameter(args, "Installer");
			wizard.SetupVariables.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType");
			wizard.SetupVariables.InstallerPath = Utils.GetStringSetupParameter(args, "InstallerPath");

			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ExpressInstallPage page2 = new ExpressInstallPage();
			//create install actions
			InstallAction action = new InstallAction(ActionTypes.Backup);
			action.Description = "Backing up...";
			page2.Actions.Add(action);

			action = new InstallAction(ActionTypes.DeleteFiles);
			action.Description = "Deleting files...";
			action.Path = "setup\\delete.txt";
			page2.Actions.Add(action);
			
			action = new InstallAction(ActionTypes.CopyFiles);
			action.Description = "Copying files...";
			page2.Actions.Add(action);


			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			IWin32Window owner = args["ParentForm"] as IWin32Window;
			return form.ShowModal(owner);
		}
	}
}
