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
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;

using WebsitePanel.Installer.Services;
using WebsitePanel.Installer.Common;

namespace WebsitePanel.Installer.Controls
{
	public delegate void OperationProgressDelegate(int percentage);

	/// <summary>
	/// Loader form.
	/// </summary>
	internal partial class Loader : Form
	{
		private AppContext appContext;
		private Core.Loader appLoader;

		public Loader()
		{
			InitializeComponent();
			DialogResult = DialogResult.Cancel;
		}

		public Loader(AppContext context, string remoteFile)
			: this()
		{
			this.appContext = context;
			//
			appLoader = new Core.Loader(remoteFile);
			//
			Start();
		}

		public Loader(AppContext context, string localFile, string componentCode, string version)
			: this()
		{
			this.appContext = context;
			//
			appLoader = new Core.Loader(localFile, componentCode, version);
			//
			Start();
		}

		private void Start()
		{
			//
			appLoader.OperationFailed += new EventHandler<Core.LoaderEventArgs<Exception>>(appLoader_OperationFailed);
			appLoader.ProgressChanged += new EventHandler<Core.LoaderEventArgs<Int32>>(appLoader_ProgressChanged);
			appLoader.StatusChanged += new EventHandler<Core.LoaderEventArgs<String>>(appLoader_StatusChanged);
			appLoader.OperationCompleted += new EventHandler<EventArgs>(appLoader_OperationCompleted);
			//
			appLoader.LoadAppDistributive();
		}

		void appLoader_OperationCompleted(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		void appLoader_StatusChanged(object sender, Core.LoaderEventArgs<String> e)
		{
			lblProcess.Text = e.StatusMessage;
			lblValue.Text = e.EventData;
			// Adjust Cancel button availability for an operation being performed
			if (btnCancel.Enabled != e.Cancellable)
			{
				btnCancel.Enabled = e.Cancellable;
			}
			// This check allows to avoid extra form redrawing operations
			if (ControlBox != e.Cancellable)
			{
				ControlBox = e.Cancellable;
			}
		}

		void appLoader_ProgressChanged(object sender, Core.LoaderEventArgs<Int32> e)
		{
			progressBar.Value = e.EventData;
			// Adjust Cancel button availability for an operation being performed
			if (btnCancel.Enabled != e.Cancellable)
			{
				btnCancel.Enabled = e.Cancellable;
			}
			// This check allows to avoid extra form redrawing operations
			if (ControlBox != e.Cancellable)
			{
				ControlBox = e.Cancellable;
			}
		}

		void appLoader_OperationFailed(object sender, Core.LoaderEventArgs<Exception> e)
		{
			appContext.AppForm.ShowError(e.EventData);
			//
			DialogResult = DialogResult.Abort;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Log.WriteInfo("Execution was canceled by user");
			Close();
		}

		private void OnLoaderFormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.Cancel)
			{
				appLoader.AbortOperation();
			}
			// Remove event handlers
			appLoader.OperationFailed -= new EventHandler<Core.LoaderEventArgs<Exception>>(appLoader_OperationFailed);
			appLoader.ProgressChanged -= new EventHandler<Core.LoaderEventArgs<Int32>>(appLoader_ProgressChanged);
			appLoader.StatusChanged -= new EventHandler<Core.LoaderEventArgs<String>>(appLoader_StatusChanged);
			appLoader.OperationCompleted -= new EventHandler<EventArgs>(appLoader_OperationCompleted);
		}
	}
}