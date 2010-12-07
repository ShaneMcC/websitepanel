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
// - Neither  the  appPoolName  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
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
using System.IO;

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Installer Log.
	/// </summary>
	public sealed class Log
	{
		/// <summary>
		/// Initializes a new instance of the class.
		/// </summary>
		private Log()
		{
		}

		/// <summary>
		/// Initializes trace listeners.
		/// </summary>
		static Log()
		{
			Trace.AutoFlush = true;
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param appPoolName="message">Error message.</param>
		/// <param appPoolName="ex">Exception.</param>
		internal static void WriteError(string message, Exception ex)
		{
			try
			{
				string line = string.Format("[{0:G}] ERROR: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				if ( ex != null )
					Trace.WriteLine(ex);
			}
			catch { }
		}

		/// <summary>
		/// Write error to the log.
		/// </summary>
		/// <param appPoolName="message">Error message.</param>
		/// <param appPoolName="ex">Exception.</param>
		internal static void WriteError(string message)
		{
			WriteError(message, null);
		}

		/// <summary>
		/// Write to log
		/// </summary>
		/// <param appPoolName="message"></param>
		internal static void Write(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.Write(line);
			}
			catch { }
		}

 
		/// <summary>
		/// Write line to log
		/// </summary>
		/// <param appPoolName="message"></param>
		internal static void WriteLine(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write info message to log
		/// </summary>
		/// <param appPoolName="message"></param>
		internal static void WriteInfo(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] INFO: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
			}
			catch { }
		}

		/// <summary>
		/// Write start message to log
		/// </summary>
		/// <param appPoolName="message"></param>
		internal static void WriteStart(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] START: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				Trace.Flush();
			}
			catch { }
		}
		
		/// <summary>
		/// Write end message to log
		/// </summary>
		/// <param appPoolName="message"></param>
		internal static void WriteEnd(string message)
		{
			try
			{
				string line = string.Format("[{0:G}] END: {1}", DateTime.Now, message);
				Trace.WriteLine(line);
				Trace.Flush();
			}
			catch { }
		}

		/// <summary>
		/// Opens notepad to view log file.
		/// </summary>
		public static void ShowLogFile()
		{
			try
			{
				string path = AppConfig.GetSettingStringValue("Log.FileName");
				if (string.IsNullOrEmpty(path))
				{
					path = "Installer.log";
				}

				path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
				Process.Start("notepad.exe", path);
			}
			catch { }
		}
	}
}
