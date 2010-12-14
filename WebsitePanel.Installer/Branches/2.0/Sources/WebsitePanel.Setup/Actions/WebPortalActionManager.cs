using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace WebsitePanel.Setup.Actions
{
	public class SetWebPortalWebSettingsAction : Action, IPrepareDefaultsAction
	{
		public const string LogStartMessage = "Retrieving default IP address of the component...";

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = "9003";
			//
			if (String.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = "WPPortal";
		}
	}

	public class UpdateEnterpriseServerUrlAction : Action, IInstallAction
	{
		public const string LogStartInstallMessage = "Updating site settings...";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Begin(LogStartInstallMessage);
				//
				Log.WriteStart(LogStartInstallMessage);
				//
				var path = Path.Combine(vars.InstallationFolder, @"App_Data\SiteSettings.config");
				//
				if (!File.Exists(path))
				{
					Log.WriteInfo(String.Format("File {0} not found", path));
					//
					return;
				}
				//
				var doc = new XmlDocument();
				doc.Load(path);
				//
				var urlNode = doc.SelectSingleNode("SiteSettings/EnterpriseServer") as XmlElement;
				if (urlNode == null)
				{
					Log.WriteInfo("EnterpriseServer setting not found");
					return;
				}

				urlNode.InnerText = vars.EnterpriseServerURL;
				doc.Save(path);
				//
				Log.WriteEnd("Updated site settings");
				//
				InstallLog.AppendLine("- Updated site settings");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Site settigs error", ex);
				//
				throw;
			}
		}

		event EventHandler<ActionProgressEventArgs<int>> IInstallAction.ProgressChange
		{
			add
			{
				lock (objectLock)
				{
					InstallProgressChange += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					InstallProgressChange -= value;
				}
			}
		}
	}

	public class CreateDesktopShortcutsAction : Action, IInstallAction
	{
		public const string LogStartInstallMessage = "Creating shortcut...";
		public const string ApplicationUrlNotFoundMessage = "Application url not found";
		public const string Path2 = "WebsitePanel Software";

		void IInstallAction.Run(SetupVariables vars)
		{
			//
			try
			{
				Begin(LogStartInstallMessage);
				//
				Log.WriteStart(LogStartInstallMessage);
				//
				var urls = Utils.GetApplicationUrls(vars.WebSiteIP, vars.WebSiteDomain, vars.WebSitePort, null);
				string url = null;

				if (urls.Length == 0)
				{
					Log.WriteInfo(ApplicationUrlNotFoundMessage);
					//
					return;
				}
				// Retrieve top-most url from the list
				url = "http://" + urls[0];
				//
				Log.WriteStart("Creating menu shortcut");
				//
				string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
				string fileName = "Login to WebsitePanel.url";
				string path = Path.Combine(programs, Path2);
				//
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				//
				WriteShortcutData(Path.Combine(path, fileName), url);
				//
				Log.WriteEnd("Created menu shortcut");
				//
				Log.WriteStart("Creating desktop shortcut");
				//
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				WriteShortcutData(Path.Combine(desktop, fileName), url);
				//
				Log.WriteEnd("Created desktop shortcut");
				//
				InstallLog.AppendLine("- Created application shortcuts");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Create shortcut error", ex);
			}
		}

		private static void WriteShortcutData(string filePath, string url)
		{
			string iconFile = Path.Combine(Environment.SystemDirectory, "url.dll");
			//
			using (StreamWriter sw = File.CreateText(filePath))
			{
				sw.WriteLine("[InternetShortcut]");
				sw.WriteLine("URL=" + url);
				sw.WriteLine("IconFile=" + iconFile);
				sw.WriteLine("IconIndex=0");
				sw.WriteLine("HotKey=0");
				//
				Log.WriteInfo(String.Format("Shortcut url: {0}", url));
			}
		}

		event EventHandler<ActionProgressEventArgs<int>> IInstallAction.ProgressChange
		{
			add
			{
				lock (objectLock)
				{
					InstallProgressChange += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					InstallProgressChange -= value;
				}
			}
		}
	}

	public class WebPortalActionManager : BaseActionManager
	{
		public WebPortalActionManager(SetupVariables sessionVars)
			: base(sessionVars)
		{
			Initialize += new EventHandler(WebPortalActionManager_Initialize);
		}

		void WebPortalActionManager_Initialize(object sender, EventArgs e)
		{
			//
			switch (SessionVariables.SetupAction)
			{
				case SetupActions.Install: // Install
					LoadInstallationScenario();
					break;
			}
		}

		private void LoadInstallationScenario()
		{
			AddAction(new SetCommonDistributiveParamsAction());
			//
			AddAction(new SetWebPortalWebSettingsAction());
			//
			AddAction(new EnsureServiceAccntSecured());
			//
			AddAction(new CopyFilesAction());
			//
			AddAction(new CreateWindowsAccountAction());
			//
			AddAction(new SetNtfsPermissionsAction());
			//
			AddAction(new CreateWebApplicationPoolAction());
			//
			AddAction(new CreateWebSiteAction());
			//
			AddAction(new UpdateEnterpriseServerUrlAction());
			//
			AddAction(new SaveComponentConfigSettingsAction());
			//
			AddAction(new CreateDesktopShortcutsAction());
		}
	}
}
