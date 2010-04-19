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

namespace WebsitePanel.Providers.Web.Iis.Extensions
{
    using Providers.Utils;
    using Common;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Server;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
	using Microsoft.Win32;
using System.Collections.Specialized;

    internal sealed class ExtensionsModuleService : ConfigurationModuleService
    {
		public const string PathAttribute = "path";

		// Mappings collection to properly detect ISAPI modules registered in IIS.
		static NameValueCollection ISAPI_MODULES = new NameValueCollection
		{
			// Misc
			{ Constants.AspPathSetting, @"\inetsrv\asp.dll" },
			// ASP.NET x86
			{ Constants.AspNet11PathSetting, @"\Framework\v1.1.4322\aspnet_isapi.dll" },
			{ Constants.AspNet20PathSetting, @"\Framework\v2.0.50727\aspnet_isapi.dll" },
			{ Constants.AspNet40PathSetting, @"\Framework\v4.0.30128\aspnet_isapi.dll" },
			// ASP.NET x64
			{ Constants.AspNet20x64PathSetting, @"\Framework64\v2.0.50727\aspnet_isapi.dll" },
			{ Constants.AspNet40x64PathSetting, @"\Framework64\v4.0.30128\aspnet_isapi.dll" }
		};
		
        public SettingPair[] GetISAPIExtensionsInstalled()
        {
            List<SettingPair> settings = new List<SettingPair>();
			//
			using (var srvman = GetServerManager())
			{
				var config = srvman.GetApplicationHostConfiguration();
				//
				var section = config.GetSection(Constants.IsapiCgiRestrictionSection);
				//
				foreach (var item in section.GetCollection())
				{
					var isapiModulePath = Convert.ToString(item.GetAttributeValue(PathAttribute));
					//
					for (int i = 0; i < ISAPI_MODULES.Keys.Count; i++)
					{
						var pathExt = ISAPI_MODULES.Get(i);
						//
						if (isapiModulePath.EndsWith(pathExt))
						{
							settings.Add(new SettingPair
							{
								// Retrieve key name
								Name = ISAPI_MODULES.GetKey(i),
								// Evaluate ISAPI module path
								Value = isapiModulePath
							});
							//
							break;
						}
					}
				}
			}
            //
            return settings.ToArray();
        }
    }
}