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
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace WebsitePanel.Providers.Web
{
    public class WebVirtualDirectory : ServiceProviderItem
	{
		#region Web Management Service Constants

		public const string WmSvcAvailable = "WmSvcAvailable";
		public const string WmSvcSiteEnabled = "WmSvcSiteEnabled";
		public const string WmSvcAccountName = "WmSvcAccountName";
		public const string WmSvcAccountPassword = "WmSvcAccountPassword";
		public const string WmSvcServiceUrl = "WmSvcServiceUrl";
		public const string WmSvcServicePort = "WmSvcServicePort";
		public const string WmSvcDefaultPort = "8172";

		#endregion

		public string AnonymousUsername { get; set; }

		public string AnonymousUserPassword { get; set; }

		public string ContentPath { get; set; }

		public string HttpRedirect { get; set; }

		public string DefaultDocs { get; set; }

		public MimeMap[] MimeMaps { get; set; }

		public HttpError[] HttpErrors { get; set; }

		public string ApplicationPool { get; set; }

		public bool EnableParentPaths { get; set; }

		public HttpHeader[] HttpHeaders { get; set; }

		public bool EnableWritePermissions { get; set; }

		public bool EnableDirectoryBrowsing { get; set; }

		public bool EnableAnonymousAccess { get; set; }

		public bool EnableWindowsAuthentication { get; set; }

		public bool EnableBasicAuthentication { get; set; }

		public bool AspInstalled { get; set; }

		public string AspNetInstalled { get; set; }

		public string PhpInstalled { get; set; }

		public bool PerlInstalled { get; set; }

		public bool PythonInstalled { get; set; }

		public bool ColdFusionInstalled { get; set; }

		public bool DedicatedApplicationPool { get; set; }

		public string ParentSiteName { get; set; }

		public bool RedirectExactUrl { get; set; }

		public bool RedirectDirectoryBelow { get; set; }

		public bool RedirectPermanent { get; set; }

		public bool CgiBinInstalled { get; set; }

		public bool SharePointInstalled { get; set; }

		public bool IIs7 { get; set; }

		#region Web Deploy Publishing Properties
		/// <summary>
		/// Gets or sets Web Deploy publishing account name
		/// </summary>
		[Persistent]
		public string WebDeployPublishingAccount { get; set; }

		/// <summary>
		/// Gets or sets Web Deploy publishing password
		/// </summary>
		[Persistent]
		public string WebDeployPublishingPassword { get; set; }

		/// <summary>
		/// Gets or sets whether Web Deploy publishing is enabled on the server
		/// </summary>
		public bool WebDeployPublishingAvailable { get; set; }

		/// <summary>
		/// Gets or sets whether Web Deploy publishing is enabled for this particular web site
		/// </summary>
		[Persistent]
		public bool WebDeploySitePublishingEnabled { get; set; }

		/// <summary>
		/// Gets or sets Web Deploy publishing profile data for this particular web site
		/// </summary>
		[Persistent]
		public string WebDeploySitePublishingProfile { get; set; }

		#endregion

		/// <summary>
		/// Gets fully qualified name which consists of parent website name if present and virtual directory name.
		/// </summary>
		[XmlIgnore]
    	public string VirtualPath
    	{
    		get
    		{
                // virtual path is rooted
                if (String.IsNullOrEmpty(ParentSiteName))
                    return "/"; //
                else if (!Name.StartsWith("/"))
                    return "/" + Name;
                //
                return Name;
    		}
    	}

        /// <summary>
        /// Gets fully qualified name which consists of parent website name if present and virtual directory name.
        /// </summary>
        [XmlIgnore]
        public string FullQualifiedPath
        {
            get
            {
                if (String.IsNullOrEmpty(ParentSiteName))
                    return Name;
                else if (Name.StartsWith("/"))
                    return ParentSiteName + Name;
                else
                    return ParentSiteName + "/" + Name;
            }
        }
	}
}
