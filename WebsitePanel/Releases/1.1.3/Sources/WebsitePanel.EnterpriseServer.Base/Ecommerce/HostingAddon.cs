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
	public class HostingAddon
	{
		private int productId;
		private string productName;
		private string productSku;
		private string description;
		private DateTime created;
		private bool enabled;
		private int resellerId;
		private int planId;
		private bool recurring;
		private bool dummyAddon;
		private bool countable;
		private decimal setupFee;
		private decimal oneTimeFee;
        private bool taxInclusive;

        public bool TaxInclusive
        {
            get { return this.taxInclusive; }
            set { this.taxInclusive = value; }
        }

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

		public bool Enabled
		{
			get { return this.enabled; }
			set { this.enabled = value; }
		}

		public int ResellerId
		{
			get { return this.resellerId; }
			set { this.resellerId = value; }
		}

		public int PlanId
		{
			get { return this.planId; }
			set { this.planId = value; }
		}

		public bool Recurring
		{
			get { return this.recurring; }
			set { this.recurring = value; }
		}

		public bool DummyAddon
		{
			get { return this.dummyAddon; }
			set { this.dummyAddon = value; }
		}

		public bool Countable
		{
			get { return this.countable; }
			set { this.countable = value; }
		}

		public decimal SetupFee
		{
			get { return this.setupFee; }
			set { this.setupFee = value; }
		}

		public decimal OneTimeFee
		{
			get { return this.oneTimeFee; }
			set { this.oneTimeFee = value; }
		}
	}
}