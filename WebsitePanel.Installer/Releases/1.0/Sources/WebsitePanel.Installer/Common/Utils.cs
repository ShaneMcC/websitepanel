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
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

using WebsitePanel.Installer.Configuration;
using System.Security.Principal;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.Threading;

namespace WebsitePanel.Installer.Common
{
	static class Utils
	{
		#region DB

		/// <summary>
		/// Converts db value to string
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>string</returns>
		public static string GetDbString(object val)
		{
			string ret = string.Empty;
			if ((val != null) && (val != DBNull.Value))
				ret = (string)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to short
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>short</returns>
		public static short GetDbShort(object val)
		{
			short ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (short)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to int
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>int</returns>
		public static int GetDbInt32(object val)
		{
			int ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (int)val;
			return ret;
		}

		/// <summary>
		/// Converts db value to bool
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>bool</returns>
		public static bool GetDbBool(object val)
		{
			bool ret = false;
			if ((val != null) && (val != DBNull.Value))
				ret = Convert.ToBoolean(val);
			return ret;
		}

		/// <summary>
		/// Converts db value to decimal
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>decimal</returns>
		public static decimal GetDbDecimal(object val)
		{
			decimal ret = 0;
			if ((val != null) && (val != DBNull.Value))
				ret = (decimal)val;
			return ret;
		}


		/// <summary>
		/// Converts db value to datetime
		/// </summary>
		/// <param name="val">Value</param>
		/// <returns>DateTime</returns>
		public static DateTime GetDbDateTime(object val)
		{
			DateTime ret = DateTime.MinValue;
			if ((val != null) && (val != DBNull.Value))
				ret = (DateTime)val;
			return ret;
		}

		#endregion

		public static bool IsThreadAbortException(Exception ex)
		{
			Exception innerException = ex;
			while (innerException != null)
			{
				if (innerException is System.Threading.ThreadAbortException)
					return true;
				innerException = innerException.InnerException;
			}

			string str = ex.ToString();
			return str.Contains("System.Threading.ThreadAbortException");
		}

		public static bool IsSecurityException(Exception ex)
		{
			if (ex is System.Security.SecurityException)
				return true;

			Exception innerException = ex;
			while (innerException != null)
			{
				if (innerException is System.Security.SecurityException)
					return true;
				innerException = innerException.InnerException;
			}

			string str = ex.ToString();
			return str.Contains("System.Security.SecurityException");
		}

		internal static bool IsWin64()
		{
			return (IntPtr.Size == 8);
		}

		internal static void SetObjectProperty(DirectoryEntry oDE, string name, object value)
		{
			if (value != null)
			{
				if (oDE.Properties.Contains(name))
				{
					oDE.Properties[name][0] = value;
				}
				else
				{
					oDE.Properties[name].Add(value);
				}
			}
		}

		internal static object GetObjectProperty(DirectoryEntry entry, string name)
		{
			if (entry.Properties.Contains(name))
				return entry.Properties[name][0];
			else
				return null;
		}

		internal static bool IIS32Enabled()
		{
			bool enabled = false;
			DirectoryEntry obj = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools");
			object objProperty = GetObjectProperty(obj, "Enable32bitAppOnWin64");
			if (objProperty != null)
			{
				enabled = (bool)objProperty;
			}
			return enabled;
		}

		internal static void CheckWin64(ApplicationForm form)
		{
			if (IsWin64())
			{
				Log.WriteInfo("x64 detected");
				string check = form.AppConfiguration.GetStringSetting(ConfigKeys.Settings_IIS64);
				if (check == "False")
					return;
				
				//ignore win64 check on IIS7
				if (Global.IISVersion.Major == 7)
					return;

				if (!IIS32Enabled())
				{
					Log.WriteInfo("IIS 32-bit mode disabled");
					
					DialogResult result = MessageBox.Show(
                        "WebsitePanel Installer has detected that Microsoft Internet Information Services (IIS) "+
                        "are running in 64-bit mode. It is recommended to switch IIS to 32-bit mode to " +
						"enable compatibility with 32-bit applications. "+
                        "Please note that 64-bit web applications will not work in 32-bit mode.\n" +
                        "Do you want WebsitePanel Installer to switch IIS to 32-bit mode?", "WebsitePanel Installer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					
					if (form.AppConfiguration.Settings[ConfigKeys.Settings_IIS64] != null)
						form.AppConfiguration.Settings[ConfigKeys.Settings_IIS64].Value = "False";
					else
						form.AppConfiguration.Settings.Add(ConfigKeys.Settings_IIS64, "False");
					form.SaveConfiguration(false);

					if (result == DialogResult.Yes)
					{
						EnableIIS32();
					}
				}
				else
				{
					Log.WriteInfo("IIS 32-bit mode enabled");
				}
			}
			else
			{
				Log.WriteInfo("x86 detected");
			}
		}

		private static void EnableIIS32()
		{
            Log.WriteStart("Enabling IIS 32-bit mode");
			DirectoryEntry obj = new DirectoryEntry("IIS://LocalHost/W3SVC/AppPools");
			SetObjectProperty(obj, "Enable32bitAppOnWin64", true);
			obj.CommitChanges();
            Log.WriteEnd("Enabled IIS 32-bit mode");


            Log.WriteStart("Starting aspnet_regiis -i");
			string path = Path.Combine(OS.GetWindowsDirectory(), @"Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe");
            ProcessStartInfo info = new ProcessStartInfo(path, "-i");
            //info.WindowStyle = ProcessWindowStyle.Minimized;
            Process process = Process.Start(info);
            process.WaitForExit();
            Log.WriteEnd("Finished aspnet_regiis -i");
			
            Log.WriteStart("Enabling Web Service Extension");
            DirectoryEntry iis = new DirectoryEntry("IIS://LocalHost/W3SVC");
            iis.Invoke("EnableWebServiceExtension", "ASP.NET v2.0.50727 (32-bit)");
            iis.CommitChanges();
            Log.WriteEnd("Enabled Web Service Extension");
		}

		/// <summary>
		/// Check security permissions
		/// </summary>
		internal static bool CheckSecurity()
		{
			try
			{
				PermissionSet set = new PermissionSet(PermissionState.Unrestricted);
				set.Demand();
			}
			catch
			{
				return false;
			}
			return true;
		}

		internal static bool IsAdministrator()
		{
			WindowsIdentity user = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(user);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

		private static Mutex mutex = null;

		internal static void SaveMutex()
		{
			GC.KeepAlive(mutex);
		}

		internal static bool IsNewInstance()
		{
			//check only one instance
			bool createdNew = true;
			mutex = new Mutex(true, "WebsitePanel Installer", out createdNew);
			return createdNew;
		}

		internal static void ShowRunningInstance()
		{
			Process currentProcess = Process.GetCurrentProcess();
			foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
			{
				if (process.Id != currentProcess.Id)
				{
					//set focus
					User32.SetForegroundWindow(process.MainWindowHandle);
					break;
				}
			}
		}


	}
}
