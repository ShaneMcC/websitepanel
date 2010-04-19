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

﻿using System.Xml.Serialization;
using System;
namespace WebsitePanel.Providers.WebAppGallery
{
	[Serializable]
    public class DeploymentParameter
	{
		private string friendlyName;

		public string FriendlyName
		{
			get
			{
				if (string.IsNullOrEmpty(friendlyName))
					return Name;
				//
				return friendlyName;
			}
			set
			{
				friendlyName = value;
			}
		}

		public string Name { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }

		public string DefaultValue { get; set; }

		public string Tags { get; set; }

		#region Constants for PARAMETERS.XML schema file
		[XmlIgnore]
		public const string MYSQL_PARAM_TAG = "MySQL";
		
		[XmlIgnore]
		public const string HIDDEN_PARAM_TAG = "Hidden";
		
		[XmlIgnore]
		public const string SQL_PARAM_TAG = "SQL";
		
		[XmlIgnore]
		public const string IISAPP_PARAM_TAG = "iisApp";

		[XmlIgnore]
		public const string PASSWORD_PARAM_TAG = "Password";

		[XmlIgnore]
		public const string NEW_PARAM_TAG = "New";

		[XmlIgnore]
		public const string DB_NAME_PARAM_TAG = "DbName";

		[XmlIgnore]
		public const string DB_PASSWORD_PARAM_TAG = "DbPassword";

		[XmlIgnore]
		public const string DB_USERNAME_PARAM_TAG = "DbUsername";

		[XmlIgnore]
		public const string DB_SERVER_PARAM_TAG = "dbServer";
		
		[XmlIgnore]
		public const string DB_ADMIN_PASSWORD_PARAM_TAG = "DbAdminPassword";
		
		[XmlIgnore]
		public const string DB_ADMIN_USERNAME_PARAM_TAG = "DbAdminUsername";
		
		[XmlIgnore]
		public const string APPICATION_PATH_PARAM = "Application Path";
		
		[XmlIgnore]
		public const string DATABASE_SERVER_PARAM = "Database Server";

		[XmlIgnore]
		public const string DATABASE_HOST_PARAM = "Database Host";

		[XmlIgnore]
		public const string SQL_SERVER_HOSTNAME_PARAM = "SQL Server Hostname";
		
		[XmlIgnore]
		public const string DATABASE_NAME_PARAM = "Database Name";
		
		[XmlIgnore]
		public const string DATABASE_USERNAME_PARAM = "Database Username";

		[XmlIgnore]
		public const string DB_USER_PARAM = "DbUser";
		
		[XmlIgnore]
		public const string DATABASE_USERPWD_PARAM = "Database Password";

		[XmlIgnore]
		public const string SQLDATABASE_NAME_PARAM = "SQL Server Database Name";

		[XmlIgnore]
		public const string SQLDATABASE_USERNAME_PARAM = "SQL Server Username";

		[XmlIgnore]
		public const string SQLDATABASE_USERPWD_PARAM = "SQL Server Password";
		
		[XmlIgnore]
		public const string DATABASE_ADMINLOGIN_PARAM = "Database Server Administrator Username";
		
		[XmlIgnore]
		public const string DATABASE_ADMINPWD_PARAM = "Database Server Administrator Password";

		[XmlIgnore]
		public const string DATABASE_ADMINISTRATOR_PARAM = "Database Administrator";

		[XmlIgnore]
		public const string SQL_SERVER_ADMINISTRATOR_PARAM = "SQL Server Administrator";

		[XmlIgnore]
		public const string SQL_SERVER_ADMINISTRATORPWD_PARAM = "SQL Server Administrator Password";

		[XmlIgnore]
		public const string DATABASE_ADMINISTRATORPWD_PARAM = "Database Administrator Password";

		[XmlIgnore]
		public static string[] NON_PUBLIC_PARAM_TAGS = new string[] { 
			HIDDEN_PARAM_TAG,
			IISAPP_PARAM_TAG,
			DB_ADMIN_PASSWORD_PARAM_TAG,
			DB_ADMIN_USERNAME_PARAM_TAG,
			DB_SERVER_PARAM_TAG
		};

		[XmlIgnore]
		public static string[] NON_PUBLIC_PARAM_NAMES = new string[] { 
			DATABASE_ADMINLOGIN_PARAM,
			DATABASE_ADMINPWD_PARAM,
			DATABASE_SERVER_PARAM,
			DATABASE_ADMINISTRATOR_PARAM,
			DATABASE_ADMINISTRATORPWD_PARAM,
			DATABASE_HOST_PARAM,
			SQL_SERVER_HOSTNAME_PARAM,
			SQL_SERVER_ADMINISTRATOR_PARAM,
			SQL_SERVER_ADMINISTRATORPWD_PARAM
		};

		[XmlIgnore]
		public static string[] DB_ADMIN_PARAMS = new string[] {
			DATABASE_ADMINLOGIN_PARAM,
			DATABASE_ADMINISTRATOR_PARAM,
			SQL_SERVER_ADMINISTRATOR_PARAM,
			DB_ADMIN_USERNAME_PARAM_TAG,
			"DatabaseAdministrator"
		};

		[XmlIgnore]
		public static string[] DB_ADMINPWD_PARAMS = new string[] {
			DATABASE_ADMINPWD_PARAM,
			DATABASE_ADMINISTRATORPWD_PARAM,
			SQL_SERVER_ADMINISTRATORPWD_PARAM,
			DB_ADMIN_PASSWORD_PARAM_TAG,
			"DatabaseAdministratorPassword"
		};

		[XmlIgnore]
		public static string[] DB_SERVER_PARAMS = new string[] {
			DATABASE_SERVER_PARAM,
			DATABASE_HOST_PARAM,
			SQL_SERVER_HOSTNAME_PARAM,
			DB_SERVER_PARAM_TAG
		};

		[XmlIgnore]
		public static string[] DATABASE_NAME_PARAMS = new string[] {
			DATABASE_NAME_PARAM,
			SQLDATABASE_NAME_PARAM,
			DB_NAME_PARAM_TAG,
			"DatabaseName"
		};

		[XmlIgnore]
		public static string[] DATABASE_USERNAME_PARAMS = new string[] {
			DATABASE_USERNAME_PARAM,
			SQLDATABASE_USERNAME_PARAM,
			DB_USERNAME_PARAM_TAG,
			DB_USER_PARAM
		};

		[XmlIgnore]
		public static string[] DATABASE_USERPWD_PARAMS = new string[] {
			DATABASE_USERPWD_PARAM,
			SQLDATABASE_USERPWD_PARAM,
			DB_PASSWORD_PARAM_TAG
		};
		#endregion

		//public string Type { get; set; }
    }
}
