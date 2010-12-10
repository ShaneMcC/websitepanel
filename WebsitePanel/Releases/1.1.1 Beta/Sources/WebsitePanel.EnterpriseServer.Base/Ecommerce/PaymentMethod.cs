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

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class PaymentMethod
	{
		public const string CREDIT_CARD = "CREDITCARD";
		public const string TCO = "2CO";
		public const string PP_ACCOUNT = "PPACCOUNT";
		public const string OFFLINE = "OFFLINE";

		private int resellerId;
		private string methodName;
		private int pluginId;
		private string displayName;
		private bool interactive;
		private string supportedItems;

		public string MethodName
		{
			get { return methodName; }
			set { methodName = value; }
		}

		public int PluginId
		{
			get { return pluginId; }
			set { pluginId = value; }
		}
		
		public string DisplayName
		{
			get { return displayName; }
			set { displayName = value; }
		}

		public int ResellerId
		{
			get { return resellerId; }
			set { resellerId = value; }
		}

		public bool Interactive
		{
			get { return interactive; }
			set { interactive = value; }
		}

		public string SupportedItems
		{
			get { return supportedItems; }
			set { supportedItems = value; }
		}
	}
}
