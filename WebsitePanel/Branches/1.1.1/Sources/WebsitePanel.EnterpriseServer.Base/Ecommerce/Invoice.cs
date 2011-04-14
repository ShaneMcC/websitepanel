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
	public class InvoiceResult
	{
		public bool Succeed;
		public int InvoiceId;
		public bool NotificationSent;
		public string ErrorMessage;
	}

	[Serializable]
	public class Invoice
	{
		public const string INVOICE_ITEM_TYPE = "INVOICE";
		public const int INVOICE_ITEM_SEVERITY = 2;

		private int invoiceId;
		private string contractId;
		private DateTime created;
		private DateTime dueDate;
		private string invoiceNumber;
        private decimal total;
		private decimal subTotal;
		private int taxationId;
		private decimal taxAmount;
		private string currency;
        private string username;
        private int customerId;
		private bool paid;

		public int InvoiceId
		{
			get { return this.invoiceId; }
			set { this.invoiceId = value; }
		}

        public int CustomerId
        {
            get { return this.customerId; }
            set { this.customerId = value; }
        }

		public string ContractId
		{
			get { return this.contractId; }
			set { this.contractId = value; }
		}

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

		public System.DateTime Created
		{
			get { return this.created; }
			set { this.created = value; }
		}

		public System.DateTime DueDate
		{
			get { return this.dueDate; }
			set { this.dueDate = value; }
		}

		public string InvoiceNumber
		{
			get { return this.invoiceNumber; }
			set { this.invoiceNumber = value; }
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

		public int TaxationId
		{
			get { return this.taxationId; }
			set { this.taxationId = value; }
		}

		public decimal TaxAmount
		{
			get { return this.taxAmount; }
			set { this.taxAmount = value; }
		}

		public string Currency
		{
			get { return this.currency; }
			set { this.currency = value; }
		}

		public bool Paid
		{
			get { return this.paid; }
			set { this.paid = value; }
		}
	}
}
