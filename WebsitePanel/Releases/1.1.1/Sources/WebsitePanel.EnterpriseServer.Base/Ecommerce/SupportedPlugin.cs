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
	public class SupportedPlugin
	{
		public const string DOMAIN_REGISTRAR_GROUP = "DOMAIN_REGISTRAR";
		public const string CC_GATEWAY_GROUP = "CC_GATEWAY";
		public const string TCO_GROUP = "2CO";
		public const string PP_ACCOUNT_GROUP = "PP_ACCOUNT";
		public const string OFFLINE_GROUP = "OFFLINE";

		/*public const string PAYPAL_PRO = "PayPalPro";
		public const string AUTHORIZE_NET = "AuthorizeNet";
		public const string OFFLINE_PAYMENT = "OfflinePayment";
		public const string ENOM = "Enom";
		public const string DIRECTI = "Directi";
		public const string TO_CHECKOUT = "2Checkout";
		public const string PAYPAL_STANDARD = "PayPalStd";*/

		public const int AUTHORIZE_NET = 1;
		public const int PAYPAL_PRO = 2;
		public const int TO_CHECKOUT = 3;
		public const int PAYPAL_STANDARD = 4;
		public const int OFFLINE_PAYMENTS = 5;
		public const int ENOM = 6;
		public const int DIRECTI = 7;
		public const int OFFLINE_REGISTRAR = 8;
		

		private int pluginId;
		private string pluginName;
		private string displayName;
		private string pluginGroup;
		private string typeName;
		private bool interactive;

		public int PluginId
		{
			get { return this.pluginId; }
			set { this.pluginId = value; }
		}

		public string PluginName
		{
			get { return this.pluginName; }
			set { this.pluginName = value; }
		}

		public string DisplayName
		{
			get { return this.displayName; }
			set { this.displayName = value; }
		}

		public string PluginGroup
		{
			get { return this.pluginGroup; }
			set { this.pluginGroup = value; }
		}

		public string TypeName
		{
			get { return this.typeName; }
			set { this.typeName = value; }
		}

		public bool Interactive
		{
			get { return this.interactive; }
			set { this.interactive = value; }
		}
	}
}
