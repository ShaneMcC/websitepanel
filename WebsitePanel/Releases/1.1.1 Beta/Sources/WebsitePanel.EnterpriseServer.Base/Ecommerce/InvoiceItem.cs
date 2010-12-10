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
	public class InvoiceItem
	{
		private int itemId;
		private int invoiceId;
		private int serviceId;
		private string itemName;
		private string typeName;
		private int quantity;
		private decimal total;
		private decimal subTotal;
		private decimal unitPrice;
		private bool processed;

		public int ItemId
		{
			get { return this.itemId; }
			set { this.itemId = value; }
		}

		public int InvoiceId
		{
			get { return this.invoiceId; }
			set { this.invoiceId = value; }
		}

		public int ServiceId
		{
			get { return this.serviceId; }
			set { this.serviceId = value; }
		}

		public string ItemName
		{
			get { return this.itemName; }
			set { this.itemName = value; }
		}

		public string TypeName
		{
			get { return this.typeName; }
			set { this.typeName = value; }
		}

		public int Quantity
		{
			get { return this.quantity; }
			set { this.quantity = value; }
		}

		public decimal Total
		{
			get { return this.total; }
			set { this.total = value; }
		}

		public decimal SubTotal
		{
			get { return this.subTotal; }
			set { this.subTotal = value; }
		}

		public decimal UnitPrice
		{
			get { return this.unitPrice; }
			set { this.unitPrice = value; }
		}

		public bool Processed
		{
			get { return this.processed; }
			set { this.processed = value; }
		}
	}
}