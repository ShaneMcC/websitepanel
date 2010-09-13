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
    [Flags]
    public enum DeploymentParameterValidationKind
    {
        None = 0,
        AllowEmpty = 1,
        RegularExpression = 2,
        Enumeration = 4,
        Boolean = 8,
    }

    [Flags]
    public enum DeploymentParameterWellKnownTag
    {
        None = 0,
        AppHostConfig = 1,
        AppPoolConfig = 2,
        Boolean = 4,
        ComObject32 = 8,
        ComObject64 = 16,
        DBAdminPassword = 32,
        DBAdminUserName = 64,
        DBConnectionString = 128,
        DBName = 256,
        DBServer = 512,
        DBUserName = 1024,
        DBUserPassword = 2048,
        FlatFile = 4096,
        Hidden = 8192,
        IisApp = 16384,
        MetaKey = 32768,
        MySql = 65536,
        MySqlConnectionString = 131072,
        New = 262144,
        RegKey = 524288,
        SetAcl = 1048576,
        Sql = 2097152,
        SqLite = 4194304,
        SqlConnectionString = 8388608,
        Password = 16777216,
        PhysicalPath = 33554432,
        VistaDB = 67108864,
        Validate = 134217728,
    }

	[Serializable]
    public class DeploymentParameter
    {
        #region Constants
        public const string ResourceGroupParameterName = "WSPResourceGroup";
        #endregion

        #region Properties
        public string FriendlyName { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
		public string DefaultValue { get; set; }
        public DeploymentParameterValidationKind ValidationKind { get; set; }
        public string ValidationString { get; set; }
        public DeploymentParameterWellKnownTag WellKnownTags { get; set; }
        #endregion

#if DEBUG
        public override string ToString()
        {
            return String.Format("{0}=\"{1}\", Tags={2}", Name, Value, WellKnownTags.ToString());
        }
#endif
    }
}
