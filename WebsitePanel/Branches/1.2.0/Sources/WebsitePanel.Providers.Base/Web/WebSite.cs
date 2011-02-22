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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WebsitePanel.Providers.Web
{
	/// <summary>
	/// Summary description for WebSiteItem.
	/// </summary>
	[Serializable]
	public class WebSite : WebVirtualDirectory
    {
        #region String constants
        public const string IIS7_SITE_ID = "WebSiteId_IIS7";
		public const string IIS7_LOG_EXT_FILE_FIELDS = "IIS7_LogExtFileFields";
        #endregion

        private string siteId;
		private string siteIPAddress;
        private int siteIPAddressId;
		private string logsPath;
        private string dataPath;
		private ServerBinding[] bindings;
        private bool frontPageAvailable;
        private bool frontPageInstalled;
	    private bool coldFusionAvailable;
	    private bool createCFVirtualDirectories;
	    private string frontPageAccount;
        private string frontPagePassword;
	    private string coldFusionVersion;
        private ServerState siteState;
        private bool securedFoldersInstalled;
        private bool heliconApeInstalled;
	    private bool heliconApeEnabled;

		public WebSite()
		{
		}

		[Persistent]
		public string SiteId
		{
			get { return siteId; }
			set { siteId = value; }
		}

		public string SiteIPAddress
		{
			get { return siteIPAddress; }
			set { siteIPAddress = value; }
		}

        [Persistent]
        public int SiteIPAddressId
        {
            get { return siteIPAddressId; }
            set { siteIPAddressId = value; }
        }

		public string LogsPath
		{
			get { return logsPath; }
            set { logsPath = value; }
		}

        [Persistent]
        public string DataPath
        {
            get { return dataPath; }
            set { dataPath = value; }
        }

        public ServerBinding[] Bindings
		{
			get { return bindings; }
			set { bindings = value; }
		}

        [Persistent]
        public string FrontPageAccount
        {
            get { return this.frontPageAccount; }
            set { this.frontPageAccount = value; }
        }

        [Persistent]
        public string FrontPagePassword
        {
            get { return this.frontPagePassword; }
            set { this.frontPagePassword = value; }
        }

        public bool FrontPageAvailable
        {
            get { return this.frontPageAvailable; }
            set { this.frontPageAvailable = value; }
        }

        public bool FrontPageInstalled
        {
            get { return this.frontPageInstalled; }
            set { this.frontPageInstalled = value; }
        }

	    public bool ColdFusionAvailable
	    {
            get { return this.coldFusionAvailable; }
            set { this.coldFusionAvailable = value; }
	    }

	    public string ColdFusionVersion
	    {
            get { return this.coldFusionVersion; }
            set { this.coldFusionVersion = value; }
	    }

	    public bool CreateCFVirtualDirectories
	    {
            get { return this.createCFVirtualDirectories; }
            set { this.createCFVirtualDirectories = value; }
	    }

	    public ServerState SiteState
        {
            get { return this.siteState; }
            set { this.siteState = value; }
        }

        public bool SecuredFoldersInstalled
        {
            get { return this.securedFoldersInstalled; }
            set { this.securedFoldersInstalled = value; }
        }

        public bool HeliconApeInstalled
        {
            get { return this.heliconApeInstalled; }
            set { this.heliconApeInstalled = value; }
        }


	    public bool HeliconApeEnabled
	    {
            get { return this.heliconApeEnabled; }
            set { this.heliconApeEnabled = value; }
	    }
	}

	[Flags]
	public enum SiteAppPoolMode
	{
		Dedicated = 1,
		Shared = 2,
		Classic = 4,
		Integrated = 8,
		dotNetFramework1 = 16,
		dotNetFramework2 = 32,
		dotNetFramework4 = 64
	};

	public class WSHelper
	{
		public const int IIS6 = 0;
		public const int IIS7 = 1;

		//
		public static Dictionary<SiteAppPoolMode, string[]> MMD = new Dictionary<SiteAppPoolMode, string[]>
		{
			{ SiteAppPoolMode.dotNetFramework1, new string[] {"", "v1.1"} },
			{ SiteAppPoolMode.dotNetFramework2, new string[] {"2.0", "v2.0"} },
			{ SiteAppPoolMode.dotNetFramework4, new string[] {"4.0", "v4.0"} },
		};

		public static string InferAppPoolName(string formatString, string siteName, SiteAppPoolMode NHRT)
		{
			if (String.IsNullOrEmpty(formatString))
				throw new ArgumentNullException("formatString");
			//
			NHRT |= SiteAppPoolMode.Dedicated;
			//
			formatString = formatString.Replace("#SITE-NAME#", siteName);
			//
			foreach (var fwVersionKey in MMD.Keys)
			{
				if ((NHRT & fwVersionKey) == fwVersionKey)
				{
					formatString = formatString.Replace("#IIS6-ASPNET-VERSION#", MMD[fwVersionKey][IIS6]);
					formatString = formatString.Replace("#IIS7-ASPNET-VERSION#", MMD[fwVersionKey][IIS7]);
					//
					break;
				}
			}
			//
			SiteAppPoolMode pipeline = NHRT & (SiteAppPoolMode.Classic | SiteAppPoolMode.Integrated);
			formatString = formatString.Replace("#PIPELINE-MODE#", pipeline.ToString());
			//
			return formatString.Trim();
		}

		public static string WhatFrameworkVersionIs(SiteAppPoolMode value)
		{
			SiteAppPoolMode dotNetVersion = value & (SiteAppPoolMode.dotNetFramework1 
				| SiteAppPoolMode.dotNetFramework2 | SiteAppPoolMode.dotNetFramework4);
			//
			return String.Format("v{0}", MMD[dotNetVersion][IIS7]);
		}
	}
}
