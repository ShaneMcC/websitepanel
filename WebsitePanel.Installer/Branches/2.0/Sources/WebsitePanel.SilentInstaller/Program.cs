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
using System.Text;
using WebsitePanel.Installer.Core;
using WebsitePanel.Installer.Common;
using System.Data;
using System.Threading;
using WebsitePanel.Installer.Configuration;
using System.IO;
using System.Collections;

namespace WebsitePanel.SilentInstaller
{
	class Program
	{
		public const string ComponentNameParam = "cname";

		[STAThread]
		static void Main(string[] args)
		{
			//
			//check security permissions
			if (!Utils.CheckSecurity())
			{
				ShowSecurityError();
				Console.ReadKey();
				return;
			}

			//check administrator permissions
			if (!Utils.IsAdministrator())
			{
				ShowSecurityError();
				Console.ReadKey();
				return;
			}

			//check for running instance
			if (!Utils.IsNewInstance())
			{
				ShowInstanceRunningErrorMessage();
				return;
			}

			Log.WriteApplicationStart();

			//check OS version
			OS.WindowsVersion version = OS.GetVersion();
			Global.OSVersion = version;
			Log.WriteInfo(string.Format("{0} detected", version));

			//check IIS version
			Version iisVersion = RegistryUtils.GetIISVersion();
			if (iisVersion.Major == 0)
				Log.WriteError("IIS not found.");
			else
				Log.WriteInfo(string.Format("IIS {0} detected", iisVersion));

			Global.IISVersion = iisVersion;
			//
			var cname = GetCommandLineArgumentValue(ComponentNameParam);

			var service = ServiceProviderProxy.GetInstallerWebService();
			var record = default(DataRow);
			//
			var ds = service.GetAvailableComponents();
			//
			foreach (DataRow row in ds.Tables[0].Rows)
			{
				string componentCode = Utils.GetDbString(row["ComponentCode"]);
				//
				if (!String.Equals(componentCode, cname, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				//
				record = row;
				break;
			}
			//
			if (record == null)
			{
				Log.WriteError(String.Format("{0} => {1}", ComponentNameParam, cname));
				Console.WriteLine("Incorrect component name specified");
				return;
			}
			//
			var cli_args = ParseInputFromCLI(cname);
			//Utils.SaveMutex();
			StartInstaller(record, cli_args);
			//
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		private static void ShowInstanceRunningErrorMessage()
		{
			ShowConsoleErrorMessage(Global.Messages.
		}

		private static void ShowSecurityError()
		{
			ShowConsoleErrorMessage(Global.Messages.NotEnoughPermissionsError);
		}

		private static void ShowConsoleErrorMessage(string p)
		{
			Console.WriteLine(p);
		}

		static Hashtable ParseInputFromCLI(string cname)
		{
			if (cname.Equals("server", StringComparison.OrdinalIgnoreCase))
			{
				//
				return LoadInputFromCLI(
					input: new Hashtable
						{
							{ "ip", Global.Parameters.WebSiteIP },
							{ "s", Global.Parameters.ServerPassword },
							{ "p", Global.Parameters.UserPassword },
							{ "d", Global.Parameters.UserDomain },
							{ "u", Global.Parameters.UserAccount },
							{ "port", Global.Parameters.WebSitePort },
							{ "h", Global.Parameters.WebSiteDomain }
						},
					output: new Hashtable
						{
							{ Global.Parameters.WebSiteIP, "127.0.0.1" },
							{ Global.Parameters.WebSiteDomain, String.Empty },
							{ Global.Parameters.WebSitePort, "9003" },
							{ Global.Parameters.UserAccount, "WSPServer" },
							{ Global.Parameters.UserDomain, String.Empty },
							{ Global.Parameters.UserPassword, String.Empty },
							{ Global.Parameters.ServerPassword, null }
						}
				);
			}
			//
			throw new Exception("Wrong component code!");
		}

		/// <summary>
		/// Loads an input from the command-line interface into a Hashtable instance to use for calls to the Installer and checks whether or not the required parameters are set.
		/// </summary>
		/// <param name="input">Input from the command-line interface (CLI)</param>
		/// <param name="output">An output storage to put the input data into</param>
		/// <returns>This method</returns>
		static Hashtable LoadInputFromCLI(Hashtable input, Hashtable output)
		{
			//
			foreach (var item in input.Keys)
			{
				var cli_argv = GetCommandLineArgumentValue(item as String);
				//
				if (String.IsNullOrEmpty(cli_argv))
					continue;
				// Assign argument value from CLI
				output[input[item]] = cli_argv;
			}
			// Ensure all required parameters are set
			foreach (var item in output.Keys)
			{
				if (output[item] == null)
				{
					throw new ArgumentNullException(item as String);
				}
			}
			//
			return output;
		}

		static void StartInstaller(DataRow row, Hashtable args)
		{
			string applicationName = Utils.GetDbString(row["ApplicationName"]);
			string componentName = Utils.GetDbString(row["ComponentName"]);
			string componentCode = Utils.GetDbString(row["ComponentCode"]);
			string componentDescription = Utils.GetDbString(row["ComponentDescription"]);
			string component = Utils.GetDbString(row["Component"]);
			string version = Utils.GetDbString(row["Version"]);
			string fileName = row["FullFilePath"].ToString();
			string installerPath = Utils.GetDbString(row["InstallerPath"]);
			string installerType = Utils.GetDbString(row["InstallerType"]);

			if (CheckForInstalledComponent(componentCode))
			{
				//AppContext.AppForm.ShowWarning("Component or its part is already installed.");
				return;
			}
			try
			{
				// download installer
				var loader = new Loader(fileName);
				//
				loader.OperationCompleted += new EventHandler<EventArgs>((object sender, EventArgs e) =>
				{
					Console.WriteLine("Download completed!");
					//
					string tmpFolder = FileUtils.GetTempDirectory();
					string path = Path.Combine(tmpFolder, installerPath);
					//Update();
					string method = "Install";
					Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", installerType, method, path));

					//prepare installer args
					args[Global.Parameters.ComponentName] = componentName;
					args[Global.Parameters.ApplicationName] = applicationName;
					args[Global.Parameters.ComponentCode] = componentCode;
					args[Global.Parameters.ComponentDescription] = componentDescription;
					args[Global.Parameters.Version] = version;
					args[Global.Parameters.InstallerFolder] = tmpFolder;
					args[Global.Parameters.InstallerPath] = installerPath;
					args[Global.Parameters.InstallerType] = installerType;
					args[Global.Parameters.Installer] = Path.GetFileName(fileName);
					args[Global.Parameters.BaseDirectory] = FileUtils.GetCurrentDirectory();
					args[Global.Parameters.IISVersion] = Global.IISVersion;
					args[Global.Parameters.ShellVersion] = AssemblyLoader.GetShellVersion();
					args[Global.Parameters.ShellMode] = Global.SilentInstallerShell;
					args["SetupXml"] = String.Empty;

					// Run the installer
					var res = AssemblyLoader.Execute(path, installerType, method, new object[] { args });
					Log.WriteInfo(string.Format("Installer returned {0}", res));
					Log.WriteEnd("Installer finished");
					// Remove temporary directory
					FileUtils.DeleteTempDirectory();
				});

				loader.OperationFailed += new EventHandler<LoaderEventArgs<Exception>>(loader_OperationFailed);
				loader.ProgressChanged += new EventHandler<LoaderEventArgs<int>>(loader_ProgressChanged);
				loader.StatusChanged += new EventHandler<LoaderEventArgs<string>>(loader_StatusChanged);
				//
				loader.LoadAppDistributive();
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				//AppContext.AppForm.ShowError(ex);
			}
			finally
			{
				//this.componentSettingsXml = null;
				//this.componentCode = null;
			}

		}

		static bool CheckForInstalledComponent(string componentCode)
		{
			bool ret = false;
			List<string> installedComponents = new List<string>();
			foreach (ComponentConfigElement componentConfig in AppConfigManager.AppConfiguration.Components)
			{
				string code = componentConfig.Settings["ComponentCode"].Value;
				installedComponents.Add(code);
				if (code == componentCode)
				{
					ret = true;
					break;
				}
			}
			if (componentCode == "standalone")
			{
				if (installedComponents.Contains("server") ||
					installedComponents.Contains("enterprise server") ||
					installedComponents.Contains("portal"))
					ret = true;
			}
			return ret;
		}

		static void loader_StatusChanged(object sender, LoaderEventArgs<string> e)
		{
			if (String.IsNullOrEmpty(e.EventData))
				Console.WriteLine(e.StatusMessage);
			else
				Console.WriteLine("{0} {1}", e.StatusMessage, e.EventData);
		}

		static void loader_ProgressChanged(object sender, LoaderEventArgs<int> e)
		{
			if (!String.IsNullOrEmpty(e.StatusMessage))
				Console.WriteLine("{0} {1}%", e.StatusMessage, e.EventData);
			else
				Console.WriteLine("Current progress is {0}%", e.EventData);
		}

		static void loader_OperationFailed(object sender, LoaderEventArgs<Exception> e)
		{
			Console.WriteLine(e.EventData.ToString());
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static bool CheckCommandLineArgument(string argName)
		{
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i];
				if (string.Equals(arg, argName, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Check for existing command line argument
		/// </summary>
		private static string GetCommandLineArgumentValue(string argName)
		{
			string key = "/" + argName.ToLower() + ":";
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i].ToLower();
				if (arg.StartsWith(key))
				{
					return arg.Substring(key.Length);
				}
			}
			return null;
		}
	}
}
