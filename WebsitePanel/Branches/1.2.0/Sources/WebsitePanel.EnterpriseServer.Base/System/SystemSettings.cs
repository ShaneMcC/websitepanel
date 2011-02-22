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
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace WebsitePanel.EnterpriseServer
{
	[Serializable]
	public class SystemSettings
	{
		public const string SMTP_SETTINGS = "SmtpSettings";
		public const string BACKUP_SETTINGS = "BackupSettings";
		public const string SETUP_SETTINGS = "SetupSettings";

		public static readonly SystemSettings Empty = new SystemSettings { SettingsArray = new string[][] {} };

		private NameValueCollection settingsHash = null;
		public string[][] SettingsArray;

		[XmlIgnore]
		NameValueCollection Settings
		{
			get
			{
				if (settingsHash == null)
				{
					// create new dictionary
					settingsHash = new NameValueCollection();

					// fill dictionary
					if (SettingsArray != null)
					{
						foreach (string[] pair in SettingsArray)
							settingsHash.Add(pair[0], pair[1]);
					}
				}
				return settingsHash;
			}
		}

		[XmlIgnore]
		public string this[string settingName]
		{
			get
			{
				return Settings[settingName];
			}
			set
			{
				// set setting
				Settings[settingName] = value;

				// rebuild array
				SettingsArray = new string[Settings.Count][];
				for (int i = 0; i < Settings.Count; i++)
				{
					SettingsArray[i] = new string[] { Settings.Keys[i], Settings[Settings.Keys[i]] };
				}
			}
		}

		public int GetInt(string settingName)
		{
			return Int32.Parse(Settings[settingName]);
		}
	}
}