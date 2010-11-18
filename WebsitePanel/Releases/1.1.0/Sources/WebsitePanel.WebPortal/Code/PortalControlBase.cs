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
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace WebsitePanel.WebPortal
{
    public class PortalControlBase : UserControl
    {
        private Control containerControl;
        private PageModule module;
        public PageModule Module
        {
            get { return module; }
            set { module = value; }
        }

        public Control ContainerControl
        {
            get { return containerControl; }
            set { containerControl = value; }
        }

        protected int ModuleID
        {
            get { return module.ModuleId; }
        }

        protected Hashtable ModuleSettings
        {
            get { return module.Settings; }
        }

		public string EditUrl(string controlKey)
		{
			return EditUrl(null, null, controlKey);
		}

		public string EditUrl(string keyName, string keyValue, string controlKey)
		{
			return EditUrl(keyName, keyValue, controlKey, null);
		}

		public string EditUrl(string keyName, string keyValue, string controlKey, params string[] additionalParams)
		{
			List<string> url = new List<string>();

            string pageId = Request[DefaultPage.PAGE_ID_PARAM];

            if (!String.IsNullOrEmpty(pageId))
			    url.Add(String.Concat(DefaultPage.PAGE_ID_PARAM, "=", pageId));

			url.Add(String.Concat(DefaultPage.MODULE_ID_PARAM, "=", ModuleID));
			url.Add(String.Concat(DefaultPage.CONTROL_ID_PARAM, "=", controlKey));

			if (!String.IsNullOrEmpty(keyName) && !String.IsNullOrEmpty(keyValue))
			{
				url.Add(String.Concat(keyName, "=", keyValue));
			}

			if (additionalParams != null)
			{
				foreach(string additionalParam in additionalParams)
					url.Add(additionalParam);
			}

			return "~/Default.aspx?" + String.Join("&", url.ToArray());
		}
    }
}
