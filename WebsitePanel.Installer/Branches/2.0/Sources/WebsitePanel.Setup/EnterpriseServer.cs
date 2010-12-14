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
using WebsitePanel.Installer.Common;
using WebsitePanel.Setup.Actions;

namespace WebsitePanel.Setup
{
	public class EnterpriseServer : BaseSetup
	{
		public static object Install(object obj)
		{
			return InstallBase(obj, "1.0.1");
		}

		internal static object InstallBase(object obj, string minimalInstallerVersion)
		{
			var args = Utils.GetSetupParameters(obj);
			//check CS version
			var shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
			var shellMode = Utils.GetStringSetupParameter(args, Global.Parameters.ShellMode);
			var version = new Version(shellVersion);

			var setupVariables = new SetupVariables
			{
				ConnectionString = "server={0};database={1};uid={2};pwd={3};",
				DatabaseServer = "localhost\\sqlexpress",
				Database = "WebsitePanel",
				CreateDatabase = true,
				WebSiteIP = "127.0.0.1",
				WebSitePort = "9002",
				WebSiteDomain = String.Empty,
				NewWebSite = true,
				NewVirtualDirectory = false,
				ConfigurationFile = "web.config",
				UpdateServerAdminPassword = true,
				ServerAdminPassword = ""
			};
			//
			InitInstall(args, setupVariables);
			//
			setupVariables.UserMembership = (setupVariables.IISVersion.Major == 7 )? 
				new string[] { "IIS_IUSRS" } :
				new string[] { "IIS_WPG" };

			//
			var eam = new EntServerActionManager(setupVariables);
			//
			eam.PrepareDistributiveDefaults();
			//
			if (shellMode.Equals(Global.SilentInstallerShell, StringComparison.OrdinalIgnoreCase))
			{
				if (version < new Version(minimalInstallerVersion))
				{
					Console.WriteLine(String.Format(Global.Messages.InstallerObsoleteMessage, minimalInstallerVersion));
					//
					return false;
				}

				try
				{
					//
					setupVariables.ServerAdminPassword = Utils.GetStringSetupParameter(args, Global.Parameters.ServerAdminPassword);
					//
					eam.ActionError += new EventHandler<ActionErrorEventArgs>((object sender, ActionErrorEventArgs e) =>
					{
						Log.WriteError(e.ErrorMessage);
					});
					//
					eam.Start();
					//
					return true;
				}
				catch (Exception ex)
				{
					Log.WriteError("Failed to install the component", ex);
					//
					return false;
				}
			}
			else
			{
				if (version < new Version(minimalInstallerVersion))
				{
					MessageBox.Show(string.Format(Global.Messages.InstallerObsoleteMessage, minimalInstallerVersion), "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return DialogResult.Cancel;
				}

				InstallerForm form = new InstallerForm();
				Wizard wizard = form.Wizard;
				wizard.SetupVariables = setupVariables;
				wizard.ActionManager = eam;

				//Unattended setup
				LoadSetupVariablesFromSetupXml(wizard.SetupVariables.SetupXml, wizard.SetupVariables);
				//create wizard pages
				var introPage = new IntroductionPage();
				var licPage = new LicenseAgreementPage();
				var page1 = new ConfigurationCheckPage();
				//
				ConfigurationCheck check1 = new ConfigurationCheck(CheckTypes.OperationSystem, "Operating System Requirement");
				ConfigurationCheck check2 = new ConfigurationCheck(CheckTypes.IISVersion, "IIS Requirement");
				ConfigurationCheck check3 = new ConfigurationCheck(CheckTypes.ASPNET, "ASP.NET Requirement");
				//
				page1.Checks.AddRange(new ConfigurationCheck[] { check1, check2, check3 });
				//
				var page2 = new InstallFolderPage();
				var page3 = new WebPage();
				var page4 = new UserAccountPage();
				var page5 = new DatabasePage();
				var passwordPage = new ServerAdminPasswordPage();
				//
				var page6 = new ExpressInstallPage2();
				//
				var page7 = new FinishPage();
				wizard.Controls.AddRange(new Control[] { introPage, licPage, page1, page2, page3, page4, page5, passwordPage, page6, page7 });
				wizard.LinkPages();
				wizard.SelectedPage = introPage;

				//show wizard
				IWin32Window owner = args["ParentForm"] as IWin32Window;
				return form.ShowModal(owner);
			}
		}

		public static DialogResult Uninstall(object obj)
		{
			return UninstallBase(obj);
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
			LoadSetupVariablesFromConfig(wizard.SetupVariables, componentId);
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
			ServerAdminPasswordPage page2 = new ServerAdminPasswordPage();
			ExpressInstallPage page3 = new ExpressInstallPage();
			//create install currentScenario
			InstallAction action = new InstallAction(ActionTypes.UpdateWebSite);
			action.Description = "Updating web site...";
			page3.Actions.Add(action);

			action = new InstallAction(ActionTypes.UpdateServerAdminPassword);
			action.Description = "Updating serveradmin password...";
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

            // check installer version
            //string shellVersion = Utils.GetStringSetupParameter(args, "ShellVersion");
            //Release version = new Release(shellVersion);
            //if (version < new Release("1.0.1"))
            //{
            //    MessageBox.Show("WebsitePanel Installer 1.0.1 or higher required.", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return DialogResult.Cancel;
            //}

			string componentId = Utils.GetStringSetupParameter(args, "ComponentId");

			AppConfig.LoadConfiguration();

			InstallerForm form = new InstallerForm();
			Wizard wizard = form.Wizard;
			LoadSetupVariablesFromConfig(wizard.SetupVariables, componentId);
            //if (wizard.SetupVariables.Release != "1.5.3")
            //{
            //    MessageBox.Show("Please update to version 1.5.3", "Setup Wizard", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return DialogResult.Cancel;
            //}

			wizard.SetupVariables.SetupAction = SetupActions.Update;
			wizard.SetupVariables.BaseDirectory = Utils.GetStringSetupParameter(args, "BaseDirectory");
			wizard.SetupVariables.UpdateVersion = Utils.GetStringSetupParameter(args, "UpdateVersion");
			wizard.SetupVariables.InstallerFolder = Utils.GetStringSetupParameter(args, "InstallerFolder");
			wizard.SetupVariables.Installer = Utils.GetStringSetupParameter(args, "Installer");
			wizard.SetupVariables.InstallerType = Utils.GetStringSetupParameter(args, "InstallerType");
			wizard.SetupVariables.InstallerPath = Utils.GetStringSetupParameter(args, "InstallerPath");
			wizard.SetupVariables.IISVersion = Utils.GetVersionSetupParameter(args, "IISVersion");

			IntroductionPage introPage = new IntroductionPage();
			LicenseAgreementPage licPage = new LicenseAgreementPage();
			ExpressInstallPage page2 = new ExpressInstallPage();
			//create install currentScenario
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

			action = new InstallAction(ActionTypes.ExecuteSql);
			action.Description = "Updating database...";
			action.Path = "setup\\update_db.sql";
			page2.Actions.Add(action);


			action = new InstallAction(ActionTypes.UpdateConfig);
			action.Description = "Updating system configuration...";
			page2.Actions.Add(action);

			FinishPage page3 = new FinishPage();
			wizard.Controls.AddRange(new Control[] { introPage, licPage, page2, page3 });
			wizard.LinkPages();
			wizard.SelectedPage = introPage;

			//show wizard
			Form parentForm = args["ParentForm"] as Form;
			return form.ShowDialog(parentForm);
		}
	}
}
