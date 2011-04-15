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
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public abstract class SystemPluginBase : ISystemPlugin
	{
		private int resellerId;
		private int pluginId;
		private string pluginName;
		private KeyValueBunch pluginSettings;

		public static Hashtable NexusCategories;
		public static Hashtable AppPurposes;

		static SystemPluginBase()
		{
			// setup nexus categories
			NexusCategories = new Hashtable();
			NexusCategories.Add("C11", "US Citizen");
			NexusCategories.Add("C12", "Permanent Resident");
			NexusCategories.Add("C21", "Business Entity");
			NexusCategories.Add("C31", "Foreign Entity");
			NexusCategories.Add("C32", "US Based Office");

			// setup app purposes
			AppPurposes = new Hashtable();
			AppPurposes.Add("P1", "For Profit");
			AppPurposes.Add("P2", "Non-profit");
			AppPurposes.Add("P3", "Personal");
			AppPurposes.Add("P4", "Educational");
			AppPurposes.Add("P5", "Government");
		}

		#region ISystemPlugin Members

		public int ResellerId
		{
			get { return resellerId; }
			set { resellerId = value; }
		}

		public int PluginId
		{
			get { return pluginId; }
			set { pluginId = value; }
		}

		public string PluginName
		{
			get { return pluginName; }
			set { pluginName = value; }
		}

		public KeyValueBunch PluginSettings
		{
			get { return pluginSettings; }
			set { pluginSettings = value; }
		}

		public virtual string[] SecureSettings
		{
			get { return null; }
		}

		#endregion

		protected string GetDomainName(string fqdn)
		{
			//
			int indexOf = fqdn.IndexOf('.');
			return fqdn.Substring(0, indexOf);
		}

		protected string GetDomainTLD(string fqdn)
		{
			//
			int indexOf = fqdn.IndexOf('.');
			return fqdn.Substring(indexOf + 1);
		}
	}
}