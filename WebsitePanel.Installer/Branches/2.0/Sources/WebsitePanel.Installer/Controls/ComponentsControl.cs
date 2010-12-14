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
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using WebsitePanel.Installer.Common;
using WebsitePanel.Installer.Services;
using WebsitePanel.Installer.Core;
using WebsitePanel.Installer.Configuration;

namespace WebsitePanel.Installer.Controls
{
	/// <summary>
	/// Components control
	/// </summary>
	internal partial class ComponentsControl : ResultViewControl
	{
		delegate void SetGridDataSourceCallback(object dataSource, string dataMember);

		private string componentCode = null;
		private string componentVersion = null;
		private string componentSettingsXml = null;

		public ComponentsControl()
		{
			InitializeComponent();
			grdComponents.AutoGenerateColumns = false;
		}

		/// <summary>
		/// Action on click Install link
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnInstallLinkClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == grdComponents.Columns.IndexOf(colLink))
			{
				DataRowView row = grdComponents.Rows[e.RowIndex].DataBoundItem as DataRowView;
				if (row != null)
				{
					StartInstaller(row);
					StartLoadingComponents();
				}
			}
		}

		private void StartInstaller(DataRowView row)
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
				AppContext.AppForm.ShowWarning("Component or its part is already installed.");
				return;
			}
			try
			{
				// download installer
				Loader form = new Loader(this.AppContext, fileName);
				DialogResult result = form.ShowDialog(this);

				if (result == DialogResult.OK)
				{
					string tmpFolder = FileUtils.GetTempDirectory();
					string path = Path.Combine(tmpFolder, installerPath);
					Update();
					string method = "Install";
					Log.WriteStart(string.Format("Running installer {0}.{1} from {2}", installerType, method, path));

					//prepare installer args
					Hashtable args = new Hashtable();

					args["ComponentName"] = componentName;
					args["ApplicationName"] = applicationName;
					args["ComponentCode"] = componentCode;
					args["ComponentDescription"] = componentDescription;
					args["Version"] = version;
					args["InstallerFolder"] = tmpFolder;
					args["InstallerPath"] = installerPath;
					args["InstallerType"] = installerType;
					args["Installer"] = Path.GetFileName(fileName);
					args[Global.Parameters.ShellVersion] = AssemblyLoader.GetShellVersion();
					args["BaseDirectory"] = FileUtils.GetCurrentDirectory();
					args[Global.Parameters.ShellMode] = Global.VisualInstallerShell;
					args["IISVersion"] = Global.IISVersion;
					args["SetupXml"] = this.componentSettingsXml;
					args["ParentForm"] = FindForm();

					//run installer
					DialogResult res = (DialogResult)AssemblyLoader.Execute(path, installerType, method, new object[] { args });
					Log.WriteInfo(string.Format("Installer returned {0}", res));
					Log.WriteEnd("Installer finished");
					Update();
					if (res == DialogResult.OK)
					{
						AppContext.AppForm.ReloadApplication();
					}
					FileUtils.DeleteTempDirectory();
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Installer error", ex);
				AppContext.AppForm.ShowError(ex);
			}
			finally
			{
				this.componentSettingsXml = null;
				this.componentCode = null;
			}

		}

		private bool CheckForInstalledComponent(string componentCode)
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

		/// <summary>
		/// Displays component description when entering grid row
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRowEnter(object sender, DataGridViewCellEventArgs e)
		{
			DataRowView row = grdComponents.Rows[e.RowIndex].DataBoundItem as DataRowView;
			if (row != null)
			{
				lblDescription.Text = Utils.GetDbString(row["ComponentDescription"]);
			}
		}

		/// <summary>
		/// Start new thread to load components
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnLoadComponentsClick(object sender, EventArgs e)
		{
			StartLoadingComponents();
		}

		private void StartLoadingComponents()
		{
			//load list of available components in the separate thread
			AppContext.AppForm.StartAsyncProgress("Connecting...", true);
			ThreadStart threadDelegate = new ThreadStart(LoadComponents);
			Thread newThread = new Thread(threadDelegate);
			newThread.Start();
		}

		/// <summary>
		/// Loads list of available components via web service
		/// </summary>
		private void LoadComponents()
		{
			try
			{
				Log.WriteStart("Loading list of available components");
				lblDescription.Text = string.Empty;
				//load components via web service
				var webService = ServiceProviderProxy.GetInstallerWebService();
				DataSet dsComponents = webService.GetAvailableComponents();
				//remove already installed components
				foreach (DataRow row in dsComponents.Tables[0].Rows)
				{
					string componentCode = Utils.GetDbString(row["ComponentCode"]);
					if (CheckForInstalledComponent(componentCode))
					{
						row.Delete();
					}
				}
				dsComponents.AcceptChanges();
				Log.WriteEnd("Available components loaded");
				SetGridDataSource(dsComponents, dsComponents.Tables[0].TableName);
				AppContext.AppForm.FinishProgress();
			}
			catch (Exception ex)
			{
				Log.WriteError("Web service error", ex);
				AppContext.AppForm.FinishProgress();
				AppContext.AppForm.ShowServerError();
			}
		}

		/// <summary>
		/// Thread safe grid binding.
		/// </summary>
		/// <param name="dataSource">Data source</param>
		/// <param name="dataMember">Data member</param>
		private void SetGridDataSource(object dataSource, string dataMember)
		{
			// InvokeRequired required compares the thread ID of the
			// calling thread to the thread ID of the creating thread.
			// If these threads are different, it returns true.
			if (this.grdComponents.InvokeRequired)
			{
				SetGridDataSourceCallback callBack = new SetGridDataSourceCallback(SetGridDataSource);
				this.grdComponents.Invoke(callBack, new object[] { dataSource, dataMember });
			}
			else
			{
				this.grdComponents.DataSource = dataSource;
				this.grdComponents.DataMember = dataMember;
			}
		}

		/// <summary>
		/// Installs component during unattended setup
		/// </summary>
		/// <param name="componentCode"></param>
		internal void InstallComponent(string componentCode, string componentVersion, string settingsXml)
		{
			//load list of available components in the separate thread
			this.componentCode = componentCode;
			this.componentVersion = componentVersion;
			this.componentSettingsXml = settingsXml;
			AppContext.AppForm.StartAsyncProgress("Connecting...", true);
			ThreadStart threadDelegate = new ThreadStart(Install);
			Thread newThread = new Thread(threadDelegate);
			newThread.Start();
		}

		/// <summary>
		/// Loads list of available components via web service and install specified component
		/// during unattended setup
		/// </summary>
		private void Install()
		{
			LoadComponents();
			foreach (DataGridViewRow gridRow in grdComponents.Rows)
			{
				DataRowView row = gridRow.DataBoundItem as DataRowView;
				if (row != null)
				{
					string code = Utils.GetDbString(row["ComponentCode"]);
					string version = Utils.GetDbString(row["Version"]);
					if (code == componentCode)
					{
						//check component version if specified
						if (!string.IsNullOrEmpty(componentVersion))
						{
							if (version != componentVersion)
								continue;
						}
						StartInstaller(row);
						AppContext.AppForm.ProceedUnattendedSetup();
						break;
					}
				}
			}
		}
	}
}
