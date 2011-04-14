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
	[Serializable]
	public class Product
	{
		public const int HOSTING_PLAN = 1;
		public const int HOSTING_ADDON = 2;
		public const int TOP_LEVEL_DOMAIN = 3;

		public const string HOSTING_PLAN_NAME = "Hosting Plan";
		public const string HOSTING_ADDON_NAME = "Hosting Addon";
		public const string TOP_LEVEL_DOMAIN_NAME = "Domain Name";

		private int productId;
		private string productName;
		private string productSku;
		private int typeId;
		private string description;
		private DateTime created;
		private bool enabled;
        private bool taxInclusive;
		private int resellerId;

		public int ProductId
		{
			get { return this.productId; }
			set { this.productId = value; }
		}

		public string ProductName
		{
			get { return this.productName; }
			set { this.productName = value; }
		}

		public string ProductSku
		{
			get { return this.productSku; }
			set { this.productSku = value; }
		}

		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		public System.DateTime Created
		{
			get { return this.created; }
			set { this.created = value; }
		}

		public int TypeId
		{
			get { return this.typeId; }
			set { this.typeId = value; }
		}

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

        public bool TaxInclusive
        {
            get { return this.taxInclusive; }
            set { this.taxInclusive = value; }
        }

		public int ResellerId
		{
			get { return this.resellerId; }
			set { this.resellerId = value; }
		}
	}
}