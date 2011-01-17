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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.DirectoryServices;
using WebsitePanel.Setup.Web;
using System.IO;
using System.Management;
using WebsitePanel.Setup.Actions;

namespace WebsitePanel.Setup
{
	public partial class ConfigurationCheckPage : BannerWizardPage
	{
		private Thread thread;
		private List<ConfigurationCheck> checks;

		public ConfigurationCheckPage()
		{
			InitializeComponent();
			checks = new List<ConfigurationCheck>();
			this.CustomCancelHandler = true;
		}

		public List<ConfigurationCheck> Checks
		{
			get
			{
				return checks;
			}
		}

		protected override void InitializePageInternal()
		{
			base.InitializePageInternal();
			this.Text = "System Configuration Check";
			this.Description = "Wait while the system is checked for potential installation problems.";

			this.imgError.Visible = false;
			this.imgOk.Visible = false;
			this.lblResult.Visible = false;
		}

		protected internal override void OnBeforeDisplay(EventArgs e)
		{
			base.OnBeforeDisplay(e);

			this.AllowMoveBack = false;
			this.AllowMoveNext = false;


		}

		protected internal override void OnAfterDisplay(EventArgs e)
		{
			base.OnAfterDisplay(e);
			thread = new Thread(new ThreadStart(this.Start));
			thread.Start();
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		public void Start()
		{
			bool pass = true;
			try
			{

				lvCheck.Items.Clear();
				this.imgError.Visible = false;
				this.imgOk.Visible = false;
				this.lblResult.Visible = false;

				foreach (ConfigurationCheck check in Checks)
				{
					AddListViewItem(check);
				}
				this.Update();
				CheckStatuses status = CheckStatuses.Success;
				string details = string.Empty;
				//
				foreach (ListViewItem item in lvCheck.Items)
				{
					ConfigurationCheck check = (ConfigurationCheck)item.Tag;
					item.ImageIndex = 0;
					item.SubItems[2].Text = "Running";
					this.Update();

					#region Previous Prereq Verification
					switch (check.CheckType)
					{
						case CheckTypes.OperationSystem:
							status = CheckOS(out details);
							break;
						case CheckTypes.IISVersion:
							status = CheckIISVersion(out details);
							break;
						case CheckTypes.ASPNET:
							status = CheckASPNET(out details);
							break;
						case CheckTypes.WPServer:
							status = CheckWPServer(check.SetupVariables, out details);
							break;
						case CheckTypes.WPEnterpriseServer:
							status = CheckWPEnterpriseServer(check.SetupVariables, out details);
							break;
						case CheckTypes.WPPortal:
							status = CheckWPPortal(check.SetupVariables, out details);
							break;
						default:
							status = CheckStatuses.Warning;
							break;
					}

					#endregion

					switch (status)
					{
						case CheckStatuses.Success:
							item.ImageIndex = 1;
							item.SubItems[2].Text = "Success";
							break;
						case CheckStatuses.Warning:
							item.ImageIndex = 2;
							item.SubItems[2].Text = "Warning";
							break;
						case CheckStatuses.Error:
							item.ImageIndex = 3;
							item.SubItems[2].Text = "Error";
							pass = false;
							break;
					}
					item.SubItems[3].Text = details;
					this.Update();
				}
				//
				//actionManager.PrerequisiteComplete += new EventHandler<ActionProgressEventArgs<bool>>((object sender, ActionProgressEventArgs<bool> e) =>
				//{
					//
				//});
				//
				//actionManager.VerifyDistributivePrerequisites();

				ShowResult(pass);
				if (pass)
				{
					//unattended setup
					if (!string.IsNullOrEmpty(Wizard.SetupVariables.SetupXml) && AllowMoveNext)
						Wizard.GoNext();
				}
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				ShowError();
				return;
			}
		}



		private void ShowResult(bool success)
		{
			this.AllowMoveNext = success;
			this.imgError.Visible = !success;
			this.imgOk.Visible = success;

			this.lblResult.Text = success ? "Success" : "Error";
			this.lblResult.Visible = true;
			Update();
		}

		private void AddListViewItem(ConfigurationCheck check)
		{
			lvCheck.BeginUpdate();
			ListViewItem item = new ListViewItem(string.Empty);
			item.SubItems.AddRange(new string[] { check.Action, string.Empty, string.Empty });
			item.Tag = check;
			lvCheck.Items.Add(item);
			lvCheck.EndUpdate();
			Update();

		}

		private CheckStatuses CheckOS(out string details)
		{
			details = string.Empty;
			try
			{
				//check OS version
				OS.WindowsVersion version = OS.GetVersion();
				details = OS.GetName(version);
				if (Utils.IsWin64())
					details += " x64";
				Log.WriteInfo(string.Format("OS check: {0}", details));

				if (!(version == OS.WindowsVersion.WindowsServer2003 ||
					version == OS.WindowsVersion.WindowsServer2008))
				{
					details = "Windows Server 2003 or Windows Server 2008 required.";
					Log.WriteError(string.Format("OS check: {0}", details), null);
#if DEBUG
					return CheckStatuses.Warning;
#endif
#if !DEBUG
					return CheckStatuses.Error;
#endif
				}
				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}
		private CheckStatuses CheckIISVersion(out string details)
		{
			details = string.Empty;
			try
			{
				details = string.Format("IIS {0}", SetupVariables.IISVersion.ToString(2));
				if (SetupVariables.IISVersion.Major == 6 &&
					Utils.IsWin64() && Utils.IIS32Enabled())
				{
					details += " (32-bit mode)";
				}

				Log.WriteInfo(string.Format("IIS check: {0}", details));
				if (SetupVariables.IISVersion.Major < 6)
				{
					details = "IIS 6.0 or greater required.";
					Log.WriteError(string.Format("IIS check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private CheckStatuses CheckASPNET(out string details)
		{
			details = "ASP.NET 2.0 is installed.";
			CheckStatuses ret = CheckStatuses.Success;
			try
			{
				if (SetupVariables.IISVersion.Major == 6)
				{
					//iis 6
					WebExtensionStatus status = GetASPNETStatus();
					switch (status)
					{
						case WebExtensionStatus.NotInstalled:
						case WebExtensionStatus.Prohibited:
							InstallASPNET();
							EnableASPNET();
							ret = CheckStatuses.Warning;
							details = "ASP.NET 2.0 has been installed.";
							break;
					}
				}
				else
				{
					//IIS 7 on Windows 2008 and higher
					if (!IsWebServerRoleInstalled())
					{
						details = "Web Server (IIS) role is not installed on your server. Run Server Manager to add Web Server (IIS) role.";
						Log.WriteInfo(string.Format("ASP.NET check: {0}", details));
						return CheckStatuses.Error;
					}
					if (!IsAspNetRoleServiceInstalled())
					{
						details = "ASP.NET role service is not installed on your server. Run Server Manager to add ASP.NET role service.";
						Log.WriteInfo(string.Format("ASP.NET check: {0}", details));
						return CheckStatuses.Error;
					}
				}
				Log.WriteInfo(string.Format("ASP.NET check: {0}", details));
				return ret;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
#if DEBUG
				return CheckStatuses.Warning;
#endif
#if !DEBUG
				return CheckStatuses.Error;
#endif
			}
		}

		private WebExtensionStatus GetASPNETStatus()
		{
			WebExtensionStatus status = WebExtensionStatus.Allowed;
			if (SetupVariables.IISVersion.Major == 6)
			{
				status = WebExtensionStatus.NotInstalled;
				string path;
				if (Utils.IsWin64() && !Utils.IIS32Enabled())
				{
					//64-bit
					path = Path.Combine(OS.GetWindowsDirectory(), @"Microsoft.NET\Framework64\v2.0.50727\aspnet_isapi.dll");
				}
				else
				{
					//32-bit
					path = Path.Combine(OS.GetWindowsDirectory(), @"Microsoft.NET\Framework\v2.0.50727\aspnet_isapi.dll");
				}
				path = path.ToLower();
				using (DirectoryEntry iis = new DirectoryEntry("IIS://LocalHost/W3SVC"))
				{
					PropertyValueCollection values = iis.Properties["WebSvcExtRestrictionList"];
					for (int i = 0; i < values.Count; i++)
					{
						string val = values[i] as string;
						if (!string.IsNullOrEmpty(val))
						{
							string strVal = val.ToString().ToLower();

							if (strVal.Contains(path))
							{
								if (strVal[0] == '1')
								{
									status = WebExtensionStatus.Allowed;
								}
								else
								{
									status = WebExtensionStatus.Prohibited;
								}
								break;
							}
						}
					}
				}
			}
			return status;
		}

		private CheckStatuses CheckIIS32Mode(out string details)
		{
			details = string.Empty;
			CheckStatuses ret = CheckIISVersion(out details);
			if (ret == CheckStatuses.Error)
				return ret;

			try
			{
				//IIS 6
				if (SetupVariables.IISVersion.Major == 6)
				{
					//x64
					if (Utils.IsWin64())
					{
						if (!Utils.IIS32Enabled())
						{
							Log.WriteInfo("IIS 32-bit mode disabled");
							EnableIIS32Mode();
							details = "IIS 32-bit mode has been enabled.";
							Log.WriteInfo(string.Format("IIS 32-bit mode check: {0}", details));
							return CheckStatuses.Warning;
						}
					}
				}
				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private bool SiteBindingsExist(SetupVariables setupVariables)
		{
			bool iis7 = (setupVariables.IISVersion.Major == 7);
			string ip = setupVariables.WebSiteIP;
			string port = setupVariables.WebSitePort;
			string domain = setupVariables.WebSiteDomain;

			string siteId = iis7 ?
				WebUtils.GetIIS7SiteIdByBinding(ip, port, domain) :
				WebUtils.GetSiteIdByBinding(ip, port, domain);
			return (siteId != null);
		}

		private bool AccountExists(SetupVariables setupVariables)
		{
			string domain = setupVariables.UserDomain;
			string username = setupVariables.UserAccount;
			return SecurityUtils.UserExists(domain, username);
		}

		private CheckStatuses CheckWPServer(SetupVariables setupVariables, out string details)
		{
			details = "";
			try
			{
				if (SiteBindingsExist(setupVariables))
				{
					details = string.Format("Site with specified bindings already exists (ip: {0}, port: {1}, domain: {2})",
							setupVariables.WebSiteIP, setupVariables.WebSitePort, setupVariables.WebSiteDomain);
					Log.WriteError(string.Format("Site bindings check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (AccountExists(setupVariables))
				{
					details = string.Format("Windows account already exists: {0}\\{1}",
							   setupVariables.UserDomain, setupVariables.UserAccount);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(setupVariables, out details))
				{
					Log.WriteError(string.Format("Disk space check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private bool CheckDiskSpace(SetupVariables setupVariables, out string details)
		{
			details = string.Empty;

			long spaceRequired = FileUtils.CalculateFolderSize(setupVariables.InstallerFolder);

			if (string.IsNullOrEmpty(setupVariables.InstallationFolder))
			{
				details = "Installation folder is not specified.";
				return false;
			}
			string drive = null;
			try
			{
				drive = Path.GetPathRoot(Path.GetFullPath(setupVariables.InstallationFolder));
			}
			catch
			{
				details = "Installation folder is invalid.";
				return false;
			}

			ulong freeBytesAvailable, totalBytes, freeBytes;
			if (FileUtils.GetDiskFreeSpaceEx(drive, out freeBytesAvailable, out totalBytes, out freeBytes))
			{
				long freeSpace = Convert.ToInt64(freeBytesAvailable);
				if (spaceRequired > freeSpace)
				{
					details = string.Format("There is not enough space on the disk ({0} required, {1} available)",
						FileUtils.SizeToMB(spaceRequired), FileUtils.SizeToMB(freeSpace));
					return false;
				}
			}
			else
			{
				details = "I/O error";
				return false;
			}
			return true;
		}

		private CheckStatuses CheckWPEnterpriseServer(SetupVariables setupVariables, out string details)
		{
			details = "";
			try
			{
				if (SiteBindingsExist(setupVariables))
				{
					details = string.Format("Site with specified bindings already exists (ip: {0}, port: {1}, domain: {2})",
							setupVariables.WebSiteIP, setupVariables.WebSitePort, setupVariables.WebSiteDomain);
					Log.WriteError(string.Format("Site bindings check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (AccountExists(setupVariables))
				{
					details = string.Format("Windows account already exists: {0}\\{1}",
							   setupVariables.UserDomain, setupVariables.UserAccount);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(setupVariables, out details))
				{
					Log.WriteError(string.Format("Disk space check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private CheckStatuses CheckWPPortal(SetupVariables setupVariables, out string details)
		{
			details = "";
			try
			{
				if (AccountExists(setupVariables))
				{
					details = string.Format("Windows account already exists: {0}\\{1}",
							   setupVariables.UserDomain, setupVariables.UserAccount);
					Log.WriteError(string.Format("Account check: {0}", details), null);
					return CheckStatuses.Error;
				}

				if (!CheckDiskSpace(setupVariables, out details))
				{
					Log.WriteError(string.Format("Disk space check: {0}", details), null);
					return CheckStatuses.Error;
				}

				return CheckStatuses.Success;
			}
			catch (Exception ex)
			{
				if (!Utils.IsThreadAbortException(ex))
					Log.WriteError("Check error", ex);
				details = "Unexpected error";
				return CheckStatuses.Error;
			}
		}

		private static void EnableIIS32Mode()
		{
			Log.WriteStart("Enabling IIS 32-bit mode");
			using (DirectoryEntry iisService = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools"))
			{
				Utils.SetObjectProperty(iisService, "Enable32bitAppOnWin64", true);
				iisService.CommitChanges();
			}
			Log.WriteEnd("Enabled IIS 32-bit mode");
		}

		private static void InstallASPNET()
		{
			Log.WriteStart("Starting aspnet_regiis -i");
			string util = (Utils.IsWin64() && !Utils.IIS32Enabled()) ?
				@"Microsoft.NET\Framework64\v2.0.50727\aspnet_regiis.exe" :
				@"Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe";

			string path = Path.Combine(OS.GetWindowsDirectory(), util);
			ProcessStartInfo info = new ProcessStartInfo(path, "-i");
			info.WindowStyle = ProcessWindowStyle.Minimized;
			Process process = Process.Start(info);
			process.WaitForExit();
			Log.WriteEnd("Finished aspnet_regiis -i");
		}

		private static void EnableASPNET()
		{
			Log.WriteStart("Enabling ASP.NET Web Service Extension");
			string name = (Utils.IsWin64() && Utils.IIS32Enabled()) ?
				"ASP.NET v2.0.50727 (32-bit)" :
				"ASP.NET v2.0.50727";
			using (DirectoryEntry iisService = new DirectoryEntry("IIS://LocalHost/W3SVC"))
			{
				iisService.Invoke("EnableWebServiceExtension", name);
				iisService.CommitChanges();
			}
			Log.WriteEnd("Enabled ASP.NET Web Service Extension");
		}

		private static bool IsWebServerRoleInstalled()
		{
			WmiHelper wmi = new WmiHelper("root\\cimv2");
			using (ManagementObjectCollection roles = wmi.ExecuteQuery("SELECT NAME FROM Win32_ServerFeature WHERE ID=2"))
			{
				return (roles.Count > 0);
			}
		}

		private static bool IsAspNetRoleServiceInstalled()
		{
			WmiHelper wmi = new WmiHelper("root\\cimv2");
			using (ManagementObjectCollection roles = wmi.ExecuteQuery("SELECT NAME FROM Win32_ServerFeature WHERE ID=148"))
			{
				return (roles.Count > 0);
			}
		}





	}
}
