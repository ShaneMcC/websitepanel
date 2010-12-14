using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WebsitePanel.Installer.Common;
using System.Xml;

namespace WebsitePanel.Setup.Actions
{
	public class SetEntServerWebSettingsAction : Action, IPrepareDefaultsAction
	{
		public const string LogStartMessage = "Retrieving default IP address of the component...";

		void IPrepareDefaultsAction.Run(SetupVariables vars)
		{
			//
			if (String.IsNullOrEmpty(vars.WebSiteIP))
				vars.WebSiteIP = "127.0.0.1";
			//
			if (String.IsNullOrEmpty(vars.WebSitePort))
				vars.WebSitePort = "9002";
			//
			if (String.IsNullOrEmpty(vars.UserAccount))
				vars.UserAccount = "WPEnterpriseServer";
		}
	}

	public class SetEntServerCryptoKeyAction : Action, IInstallAction
	{
		public const string LogStartInstallMessage = "Updating web.config file (crypto key)";
		public const string LogEndInstallMessage = "Updated web.config file";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				OnInstallProgressChanged(LogStartInstallMessage, 0);
				Log.WriteStart(LogStartInstallMessage);

				var file = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
				vars.CryptoKey = Utils.GetRandomString(20);

				// load file
				string content = string.Empty;
				using (StreamReader reader = new StreamReader(file))
				{
					content = reader.ReadToEnd();
				}

				// expand variables
				content = Utils.ReplaceScriptVariable(content, "installer.cryptokey", vars.CryptoKey);
				//
				Log.WriteInfo(String.Format("The following cryptographic key has been generated: '{0}'", vars.CryptoKey));

				// save file
				using (StreamWriter writer = new StreamWriter(file))
				{
					writer.Write(content);
				}
				//update log
				Log.WriteEnd(LogEndInstallMessage);

				AppConfig.SetComponentSettingStringValue(vars.ComponentId, Global.Parameters.CryptoKey, vars.CryptoKey);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Update web.config error", ex);
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

	public class CreateDatabaseAction : Action, IInstallAction, IUninstallAction
	{
		public const string LogStartInstallMessage = "Creating SQL Server database...";
		public const string LogStartUninstallMessage = "Deleting database";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Begin(LogStartInstallMessage);
				//
				var connectionString = vars.DbInstallConnectionString;
				var database = vars.Database;

				Log.WriteStart(LogStartInstallMessage);
				Log.WriteInfo(String.Format("SQL Server Database Name: \"{0}\"", database));
				//
				if (SqlUtils.DatabaseExists(connectionString, database))
				{
					throw new Exception(String.Format("SQL Server database \"{0}\" already exists", database));
				}
				SqlUtils.CreateDatabase(connectionString, database);
				//
				Log.WriteEnd("Created SQL Server database");
				//
				InstallLog.AppendLine(String.Format("- Created a new SQL Server database \"{0}\"", database));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Create database error", ex);
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

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart(LogStartUninstallMessage);
				//
				Log.WriteInfo(String.Format("Deleting database \"{0}\"", vars.Database));
				//
				if (SqlUtils.DatabaseExists(vars.DbInstallConnectionString, vars.Database))
				{
					SqlUtils.DeleteDatabase(vars.DbInstallConnectionString, vars.Database);
					//
					Log.WriteEnd("Deleted database");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database delete error", ex);
				throw;
			}
		}

		event EventHandler<ActionProgressEventArgs<int>> IUninstallAction.ProgressChange
		{
			add
			{
				lock (objectLock)
				{
					UninstallProgressChange += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					UninstallProgressChange -= value;
				}
			}
		}
	}

	public class CreateDatabaseUserAction : Action, IInstallAction, IUninstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				//
				Log.WriteStart(String.Format("Creating database user {0}", vars.Database));
				//
				vars.DatabaseUserPassword = Utils.GetRandomString(20);
				//user name should be the same as database
				vars.NewDatabaseUser = SqlUtils.CreateUser(vars.DbInstallConnectionString, vars.Database, vars.DatabaseUserPassword, vars.Database);
				//
				Log.WriteEnd("Created database user");
				InstallLog.AppendLine("- Created database user \"{0}\"", vars.Database);
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Create db user error", ex);
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

		void IUninstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart("Deleting database user");
				Log.WriteInfo(String.Format("Deleting database user \"{0}\"", vars.Database));
				//
				if (SqlUtils.UserExists(vars.DbInstallConnectionString, vars.Database))
				{
					SqlUtils.DeleteUser(vars.DbInstallConnectionString, vars.Database);
					//
					Log.WriteEnd("Deleted database user");
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Database user delete error", ex);
				throw;
			}
		}

		event EventHandler<ActionProgressEventArgs<int>> IUninstallAction.ProgressChange
		{
			add
			{
				lock (objectLock)
				{
					UninstallProgressChange += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					UninstallProgressChange -= value;
				}
			}
		}
	}

	public class ExecuteInstallSqlAction : Action, IInstallAction
	{
		public const string SqlFilePath = @"Setup\install_db.sql";
		public const string ExecuteProgressMessage = "Creating database objects...";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				var component = vars.ComponentFullName;
				var componentId = vars.ComponentId;

				var path = Path.Combine(vars.InstallationFolder, SqlFilePath);

				if (!FileUtils.FileExists(path))
				{
					Log.WriteInfo(String.Format("File {0} not found", path));
					return;
				}
				//
				SqlProcess process = new SqlProcess(path, vars.DbInstallConnectionString, vars.Database);
				//
				process.ProgressChange += new EventHandler<ActionProgressEventArgs<int>>(process_ProgressChange);
				//
				process.Run();
				//
				InstallLog.AppendLine(string.Format("- Installed {0} database objects", component));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Run sql error", ex);
				//
				throw;
			}
		}

		void process_ProgressChange(object sender, ActionProgressEventArgs<int> e)
		{
			OnInstallProgressChanged(ExecuteProgressMessage, e.EventData);
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

	public class UpdateServeradminPasswAction : Action, IInstallAction
	{
		public const string SqlStatement = @"USE [{0}]; UPDATE [dbo].[Users] SET [Password] = '{1}' WHERE [UserID] = 1;";

		void IInstallAction.Run(SetupVariables vars)
		{
			try
			{
				Log.WriteStart("Updating serveradmin password");
				//
				var path = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
				var password = vars.ServerAdminPassword;

				if (!File.Exists(path))
				{
					Log.WriteInfo(String.Format("File {0} not found", path));
					return;
				}
				//
				bool encryptionEnabled = IsEncryptionEnabled(path);
				// Encrypt password
				if (encryptionEnabled)
				{
					password = Utils.Encrypt(vars.CryptoKey, password);
				}
				//
				SqlUtils.ExecuteQuery(vars.DbInstallConnectionString, String.Format(SqlStatement, vars.Database, password));
				//
				Log.WriteEnd("Updated serveradmin password");
				//
				InstallLog.AppendLine("- Updated password for the serveradmin account");
				//
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;
				//
				Log.WriteError("Update error", ex);
				throw;
			}
		}

		private bool IsEncryptionEnabled(string path)
		{
			var doc = new XmlDocument();
			doc.Load(path);
			//encryption enabled
			string xPath = "configuration/appSettings/add[@key=\"WebsitePanel.EncryptionEnabled\"]";
			XmlElement encryptionNode = doc.SelectSingleNode(xPath) as XmlElement;
			bool encryptionEnabled = false;
			//
			if (encryptionNode != null)
			{
				bool.TryParse(encryptionNode.GetAttribute("value"), out encryptionEnabled);
			}
			//
			return encryptionEnabled;
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

	public class SaveAspNetDbConnectionStringAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			Log.WriteStart("Updating web.config file (connection string)");
			//
			var file = Path.Combine(vars.InstallationFolder, vars.ConfigurationFile);
			//
			var content = String.Empty;
			// load file
			using (StreamReader reader = new StreamReader(file))
			{
				content = reader.ReadToEnd();
			}
			// Build connection string
			vars.ConnectionString = String.Format(vars.ConnectionString, vars.DatabaseServer, vars.Database, vars.Database, vars.DatabaseUserPassword);
			// Expand variables
			content = Utils.ReplaceScriptVariable(content, "installer.connectionstring", vars.ConnectionString);
			// Save file
			using (StreamWriter writer = new StreamWriter(file))
			{
				writer.Write(content);
			}
			//
			Log.WriteEnd(String.Format("Updated {0} file", vars.ConfigurationFile));
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

	public class SaveEntServerConfigSettingsAction : Action, IInstallAction
	{
		void IInstallAction.Run(SetupVariables vars)
		{
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "Database", vars.Database);
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewDatabase", true);
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "DatabaseUser", vars.Database);
			AppConfig.SetComponentSettingBooleanValue(vars.ComponentId, "NewDatabaseUser", vars.NewDatabaseUser);
			//
			AppConfig.SetComponentSettingStringValue(vars.ComponentId, "ConnectionString", vars.ConnectionString);
			//
			AppConfig.SaveConfiguration();
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

	public class EntServerActionManager : BaseActionManager
	{
		public EntServerActionManager(SetupVariables sessionVars) : base(sessionVars)
		{
			Initialize += new EventHandler(EntServerActionManager_PreInit);
		}

		void EntServerActionManager_PreInit(object sender, EventArgs e)
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
			AddAction(new SetEntServerWebSettingsAction());
			//
			AddAction(new EnsureServiceAccntSecured());
			//
			AddAction(new CopyFilesAction());
			//
			AddAction(new SetEntServerCryptoKeyAction());
			//
			AddAction(new CreateWindowsAccountAction());
			//
			AddAction(new SetNtfsPermissionsAction());
			//
			AddAction(new CreateWebApplicationPoolAction());
			//
			AddAction(new CreateWebSiteAction());
			//
			AddAction(new CreateDatabaseAction());
			//
			AddAction(new CreateDatabaseUserAction());
			//
			AddAction(new ExecuteInstallSqlAction());
			//
			AddAction(new UpdateServeradminPasswAction());
			//
			AddAction(new SaveAspNetDbConnectionStringAction());
			//
			AddAction(new SaveComponentConfigSettingsAction());
			//
			AddAction(new SaveEntServerConfigSettingsAction());
		}
	}
}
