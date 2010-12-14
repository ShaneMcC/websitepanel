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
using System.Xml;

namespace WebsitePanel.Installer.Common
{
	public class Global
	{
		public const string VisualInstallerShell = "VisualInstallerShell";
		public const string SilentInstallerShell = "SilentInstallerShell";

		public abstract class Parameters
		{
			public const string ShellMode = "ShellMode";
			public const string ShellVersion = "ShellVersion";
			public const string IISVersion = "IISVersion";
			public const string BaseDirectory = "BaseDirectory";
			public const string Installer = "Installer";
			public const string InstallerType = "InstallerType";
			public const string InstallerPath = "InstallerPath";
			public const string InstallerFolder = "InstallerFolder";
			public const string Version = "Version";
			public const string ComponentDescription = "ComponentDescription";
			public const string ComponentCode = "ComponentCode";
			public const string ApplicationName = "ApplicationName";
			public const string ComponentName = "ComponentName";
			public const string WebSiteIP = "WebSiteIP";
			public const string WebSitePort = "WebSitePort";
			public const string WebSiteDomain = "WebSiteDomain";
			public const string ServerPassword = "ServerPassword";
			public const string UserDomain = "UserDomain";
			public const string UserAccount = "UserAccount";
			public const string UserPassword = "UserPassword";
			public const string CryptoKey = "CryptoKey";
			public const string ServerAdminPassword = "ServerAdminPassword";
		}

		public abstract class Messages
		{
			public const string NotEnoughPermissionsError = "You do not have the appropriate permissions to perform this operation. Make sure you are running the application from the local disk and you have local system administrator privileges.";
			public const string InstallerObsoleteMessage = "WebsitePanel Installer {0} or higher required.";
		}

		private Global()
		{
		}

		public static Version IISVersion { get; set; }
		public static OS.WindowsVersion OSVersion { get; set; }
		public static XmlDocument SetupXmlDocument { get; set; }
	}

	public class SetupEventArgs<T> : EventArgs
	{
		public T EventData { get; set; }
		public string EventMessage { get; set; }
	}
}
