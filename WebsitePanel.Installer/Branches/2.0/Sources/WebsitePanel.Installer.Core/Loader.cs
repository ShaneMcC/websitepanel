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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using Ionic.Zip;

using WebsitePanel.Installer.Common;

namespace WebsitePanel.Installer.Core
{
	public class LoaderEventArgs<T> : EventArgs
	{
		public string StatusMessage { get; set; }
		public T EventData { get; set; }
	}

	/// <summary>
	/// Loader form.
	/// </summary>
	public partial class Loader
	{
		public const string ConnectingRemotServiceMessage = "Connecting...";
		public const string DownloadingSetupFilesMessage = "Downloading setup files...";
		public const string CopyingSetupFilesMessage = "Copying setup files...";
		public const string PreparingSetupFilesMessage = "Please wait while Setup prepares the necessary files...";
		public const string DownloadProgressMessage = "{0} KB of {1} KB";
		public const string PrepareSetupProgressMessage = "{0}%";

		private const int ChunkSize = 262144;
		private Thread thread;
		private string localFile;
		private string remoteFile;
		private string componentCode;
		private string version;

		public event EventHandler<LoaderEventArgs<String>> StatusChanged;
		public event EventHandler<LoaderEventArgs<Exception>> OperationFailed;
		public event EventHandler<LoaderEventArgs<Int32>> ProgressChanged;
		public event EventHandler<EventArgs> OperationCompleted;

		public Loader(string remoteFile)
		{
			this.remoteFile = remoteFile;
		}

		public Loader(string localFile, string componentCode, string version)
		{
			this.localFile = localFile;
			this.componentCode = componentCode;
			this.version = version;
		}

		public void LoadAppDistributive()
		{
			thread = new Thread(new ThreadStart(LoadAppDistributiveInternal));
			thread.Start();
		}

		private void RaiseOnStatusChangedEvent(string statusMessage)
		{
			RaiseOnStatusChangedEvent(statusMessage, String.Empty);
		}

		private void RaiseOnStatusChangedEvent(string statusMessage, string eventData)
		{
			if (StatusChanged == null)
			{
				return;
			}
			// No event data for status updates
			StatusChanged(this, new LoaderEventArgs<String> { StatusMessage = statusMessage, EventData = eventData });
		}

		private void RaiseOnProgressChangedEvent(int eventData)
		{
			if (ProgressChanged == null)
			{
				return;
			}
			//
			ProgressChanged(this, new LoaderEventArgs<int> { EventData = eventData });
		}

		private void RaiseOnOperationFailedEvent(Exception ex)
		{
			if (OperationFailed == null)
			{
				return;
			}
			//
			OperationFailed(this, new LoaderEventArgs<Exception> { EventData = ex });
		}

		private void RaiseOnOperationCompletedEvent()
		{
			if (OperationCompleted == null)
			{
				return;
			}
			//
			OperationCompleted(this, EventArgs.Empty);
		}

		/// <summary>
		/// Displays process progress.
		/// </summary>
		private void LoadAppDistributiveInternal()
		{
			//
			try
			{
				var service = ServiceProviderProxy.GetInstallerWebService();
				//
				string dataFolder = FileUtils.GetDataDirectory();
				string tmpFolder = FileUtils.GetTempDirectory();

				if (!Directory.Exists(dataFolder))
				{
					Directory.CreateDirectory(dataFolder);
					Log.WriteInfo("Data directory created");
				}

				if (Directory.Exists(tmpFolder))
				{
					FileUtils.DeleteTempDirectory();
				}

				if (!Directory.Exists(tmpFolder))
				{
					Directory.CreateDirectory(tmpFolder);
					Log.WriteInfo("Tmp directory created");
				}

				string fileToDownload = null;
				if (!string.IsNullOrEmpty(localFile))
				{
					fileToDownload = localFile;
				}
				else
				{
					fileToDownload = Path.GetFileName(remoteFile);
				}

				string destinationFile = Path.Combine(dataFolder, fileToDownload);
				string tmpFile = Path.Combine(tmpFolder, fileToDownload);

				//check whether file already downloaded
				if (!File.Exists(destinationFile))
				{
					if (string.IsNullOrEmpty(remoteFile))
					{
						//need to get remote file name
						RaiseOnStatusChangedEvent(ConnectingRemotServiceMessage);
						//
						RaiseOnProgressChangedEvent(0);
						//
						DataSet ds = service.GetReleaseFileInfo(componentCode, version);
						//
						RaiseOnProgressChangedEvent(100);
						//
						if (ds != null &&
							ds.Tables.Count > 0 &&
							ds.Tables[0].Rows.Count > 0)
						{
							DataRow row = ds.Tables[0].Rows[0];
							remoteFile = row["FullFilePath"].ToString();
							fileToDownload = Path.GetFileName(remoteFile);
							destinationFile = Path.Combine(dataFolder, fileToDownload);
							tmpFile = Path.Combine(tmpFolder, fileToDownload);
						}
						else
						{
							throw new Exception("Installer not found");
						}
					}

					// download file to tmp folder
					RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage);
					//
					RaiseOnProgressChangedEvent(0);
					//
					DownloadFile(remoteFile, tmpFile);
					//
					RaiseOnProgressChangedEvent(100);

					// copy downloaded file to data folder
					RaiseOnStatusChangedEvent(CopyingSetupFilesMessage);
					//
					RaiseOnProgressChangedEvent(0);

					// Ensure that the target does not exist.
					if (File.Exists(destinationFile))
						FileUtils.DeleteFile(destinationFile);
					File.Move(tmpFile, destinationFile);
					//
					RaiseOnProgressChangedEvent(100);
				}

				// unzip file
				RaiseOnStatusChangedEvent(PreparingSetupFilesMessage);
				//
				RaiseOnProgressChangedEvent(0);
				//
				UnzipFile(destinationFile, tmpFolder);
				//
				RaiseOnProgressChangedEvent(100);
				//
				RaiseOnOperationCompletedEvent();
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				Log.WriteError("Loader module error", ex);
				//
				RaiseOnOperationFailedEvent(ex);
			}
		}

		private void DownloadFile(string sourceFile, string destinationFile)
		{
			try
			{
				var service = ServiceProviderProxy.GetInstallerWebService();
				//
				Log.WriteStart("Downloading file");
				Log.WriteInfo(string.Format("Downloading file \"{0}\" to \"{1}\"", sourceFile, destinationFile));
				
				long downloaded = 0;
				long fileSize = service.GetFileSize(sourceFile);
				if (fileSize == 0)
				{
					throw new FileNotFoundException("Service returned empty file.", sourceFile);
				}

				byte[] content;

				while (downloaded < fileSize)
				{
					content = service.GetFileChunk(sourceFile, (int)downloaded, ChunkSize);
					if (content == null)
					{
						throw new FileNotFoundException("Service returned NULL file content.", sourceFile);
					}
					FileUtils.AppendFileContent(destinationFile, content);
					downloaded += content.Length;
					//update progress bar
					RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage, 
						string.Format(DownloadProgressMessage, downloaded / 1024, fileSize / 1024));
					//
					RaiseOnProgressChangedEvent(Convert.ToInt32((downloaded * 100) / fileSize));

					if (content.Length < ChunkSize)
						break;
				}
				//
				RaiseOnStatusChangedEvent(DownloadingSetupFilesMessage, "100%");
				//
				Log.WriteEnd(string.Format("Downloaded {0} bytes", downloaded));
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				throw;
			}
		}

		private void UnzipFile(string zipFile, string destFolder)
		{
			try
			{
				int val = 0;
				Log.WriteStart("Unzipping file");
				Log.WriteInfo(string.Format("Unzipping file \"{0}\" to the folder \"{1}\"", zipFile, destFolder));

				long zipSize = 0;
				using (ZipFile zip = ZipFile.Read(zipFile))
				{
					foreach (ZipEntry entry in zip)
					{
						if (!entry.IsDirectory)
							zipSize += entry.UncompressedSize;
					}
				}

				RaiseOnProgressChangedEvent(0);

				long unzipped = 0;
				using (ZipFile zip = ZipFile.Read(zipFile))
				{
					foreach (ZipEntry entry in zip)
					{
						entry.Extract(destFolder, ExtractExistingFileAction.OverwriteSilently);
						if (!entry.IsDirectory)
							unzipped += entry.UncompressedSize;

						if (zipSize != 0)
						{
							val = Convert.ToInt32(unzipped * 100 / zipSize);
							//
							RaiseOnStatusChangedEvent(PreparingSetupFilesMessage, String.Format(PrepareSetupProgressMessage, val));
							//
							RaiseOnProgressChangedEvent(val);
						}
					}
				}
				//
				RaiseOnStatusChangedEvent(PreparingSetupFilesMessage, "100%");
				//
				Log.WriteEnd("Unzipped file");
			}
			catch (Exception ex)
			{
				if (Utils.IsThreadAbortException(ex))
					return;

				throw;
			}
		}

		public void AbortOperation()
		{
			if (thread != null)
			{
				if (thread.IsAlive)
				{
					thread.Abort();
				}
				thread.Join();
			}
		}
	}
}