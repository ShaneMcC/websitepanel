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
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.EnterpriseServer
{
	public class SystemController
	{
		private SystemController()
		{
		}

		public static SystemSettings GetSystemSettings(string settingsName)
		{
			// check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.IsActive);
			if (accountCheck < 0)
				return null;

			bool isDemoAccount = (SecurityContext.CheckAccount(DemandAccount.NotDemo) < 0);

			return GetSystemSettingsInternal(settingsName, !isDemoAccount);
		}

		internal static SystemSettings GetSystemSettingsInternal(string settingsName, bool decryptPassword)
		{
			// create settings object
			SystemSettings settings = new SystemSettings();

			// get service settings
			IDataReader reader = null;

			try
			{
				// get service settings
				reader = DataProvider.GetSystemSettings(settingsName);

				while (reader.Read())
				{
					string name = (string)reader["PropertyName"];
					string val = (string)reader["PropertyValue"];

					if (name.ToLower().IndexOf("password") != -1 && decryptPassword)
						val = CryptoUtils.Decrypt(val);

					settings[name] = val;
				}


			}
			finally
			{
				if (reader != null && !reader.IsClosed)
					reader.Close();
			}

			return settings;
		}

		public static int SetSystemSettings(string settingsName, SystemSettings settings)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin
				| DemandAccount.IsActive);
			if (accountCheck < 0) return accountCheck;

			XmlDocument xmldoc = new XmlDocument();
			XmlElement root = xmldoc.CreateElement("properties");

			foreach (string[] pair in settings.SettingsArray)
			{
				string name = pair[0];
				string val = pair[1];

				if (name.ToLower().IndexOf("password") != -1)
					val = CryptoUtils.Encrypt(val);

				XmlElement property = xmldoc.CreateElement("property");

				property.SetAttribute("name", name);
				property.SetAttribute("value", val);

				root.AppendChild(property);
			}

			DataProvider.SetSystemSettings(settingsName, root.OuterXml);

			return 0;
		}
	}
}