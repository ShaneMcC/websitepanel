using System;
using System.Collections.Generic;
using System.Text;
using WebsitePanel.Installer.Common;
using System.Configuration;
using WebsitePanel.Installer.Configuration;

namespace WebsitePanel.Installer.Core
{
	public sealed class AppConfigManager
	{
		private static System.Configuration.Configuration appConfig;
		public const string AppConfigFileNameWithoutExtension = "WebsitePanel.Installer.exe";

		static AppConfigManager()
		{
			LoadConfiguration();
		}

		#region Core.Configuration
		/// <summary>
		/// Loads application configuration
		/// </summary>
		public static void LoadConfiguration()
		{
			Log.WriteStart("Loading application configuration");
			//appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			appConfig = ConfigurationManager.OpenExeConfiguration(AppConfigFileNameWithoutExtension);
			//
			Log.WriteEnd("Application configuration loaded");
		}

		/// <summary>
		/// Returns application configuration section
		/// </summary>
		public static InstallerSection AppConfiguration
		{
			get
			{
				return appConfig.GetSection("installer") as InstallerSection;
			}
		}

		/// <summary>
		/// Saves application configuration
		/// </summary>
		public static void SaveConfiguration(bool showAlert)
		{
			if (appConfig != null)
			{
				try
				{
					Log.WriteStart("Saving application configuration");
					appConfig.Save();
					Log.WriteEnd("Application configuration saved");
					if (showAlert)
					{
						//ShowInfo("Application settings updated successfully.");
					}
				}
				catch (Exception ex)
				{
					Log.WriteError("Core.Configuration error", ex);
					if (showAlert)
					{
						//ShowError(ex);
					}
				}
			}
		}

		#endregion
	}
}
