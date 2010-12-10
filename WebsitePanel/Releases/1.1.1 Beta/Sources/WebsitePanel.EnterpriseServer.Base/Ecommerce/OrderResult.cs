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
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	[Serializable]
	public class OrderItem : KeyValueBunch
	{
		private int productId;
		private int typeId;
		private bool recurring;
		private int billingCycle;
		private string itemName;
		private int parentIndex = -1;
		private int parentSvcId;
		private int quantity;
		
		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}

		public int ProductId
		{
			get { return this.productId; }
			set { this.productId = value; }
		}

		public int TypeId
		{
			get { return this.typeId; }
			set { this.typeId = value; }
		}

		public int BillingCycle
		{
			get { return this.billingCycle; }
			set { this.billingCycle = value; }
		}

		public bool Recurring
		{
			get { return this.recurring; }
			set { this.recurring = value; }
		}

		public string ItemName
		{
			get { return this.itemName; }
			set { this.itemName = value; }
		}

		public int ParentIndex
		{
			get { return this.parentIndex; }
			set { this.parentIndex = value; }
		}

		public int ParentSvcId
		{
			get { return this.parentSvcId; }
			set { this.parentSvcId = value; }
		}

		public static OrderItem GetHostingPlanItem(int productId, string planName, int cycleId)
		{
			OrderItem plan = new OrderItem();
			plan.productId = productId;
			plan.billingCycle = cycleId;
			plan.itemName = planName;
			plan.recurring = true;
			plan.quantity = 1;
			plan.typeId = Product.HOSTING_PLAN;
			//
			return plan;
		}

		public static OrderItem GetTopLevelDomainItem(int productId, int cycleId, string itemName)
		{
			OrderItem item = new OrderItem();
			item.recurring = true;
			item.typeId = Product.TOP_LEVEL_DOMAIN;
			item.productId = productId;
			item.quantity = 1;
			item.billingCycle = cycleId;
			item.itemName = itemName;

			return item;
		}

		public static OrderItem GetHostingAddonItem(int productId, int cycleId, int quantity, string itemName)
		{
			OrderItem item = new OrderItem();
			item.billingCycle = cycleId;
			item.recurring = cycleId > 0;
			item.quantity = quantity;
			item.productId = productId;
			item.typeId = Product.HOSTING_ADDON;
			item.itemName = itemName;

			return item;
		}

		public static OrderItem GetHostingAddonItem(int productId, int quantity, string itemName)
		{
			return GetHostingAddonItem(productId, quantity, 0, itemName);
		}
	}

	public class OrderResult
	{
		private bool succeed;
		private int resultCode;

		private int orderInvoice;

		public bool Succeed
		{
			get { return this.succeed; }
			set { this.succeed = value; }
		}

		public int ResultCode
		{
			get { return this.resultCode; }
			set { this.resultCode = value; }
		}

		public int OrderInvoice
		{
			get { return this.orderInvoice; }
			set { this.orderInvoice = value; }
		}
	}
}