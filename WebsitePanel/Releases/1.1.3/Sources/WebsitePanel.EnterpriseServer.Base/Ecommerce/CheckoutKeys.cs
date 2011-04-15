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
	public class CheckoutKeys
	{
		public const string PaymentProcessor = "PaymentProcessor";
		public const string CardNumber = "CardNumber";
		/// <summary>Visa, MasterCard, Discover, Amex, Switch, Solo</summary>
		public const string CardType = "CardType"; 		
		public const string VerificationCode = "VerificationCode";
		//
		public const string ExpireYear = "ExpireYear";
		public const string ExpireMonth = "ExpireMonth";
		//
		public const string StartMonth = "StartMonth";
		public const string StartYear = "StartYear";
		//
		public const string IssueNumber = "IssueNumber";
		public const string Amount = "Amount";
		public const string Currency = "Currency";
        public const string ContractNumber = "ContractNumber";
		public const string InvoiceNumber = "InvoiceNumber";
		public const string FirstName = "FirstName";
		public const string LastName = "LastName";
		public const string CustomerEmail = "CustomerEmail";
		public const string CompanyName = "CompanyName";
		public const string Address = "Address";
		public const string Zip = "Zip";
		public const string City = "City";
		public const string State = "State";
		public const string Country = "Country";
		public const string Phone = "Phone";
		public const string Fax = "Fax";
		public const string CustomerId = "CustomerId";
		public const string ShipToFirstName = "ShipToFirstName";
		public const string ShipToLastName = "ShipToLastName";
		public const string ShipToCompany = "ShipToCompany";
		public const string ShipToZip = "ShipToZip";
		public const string ShipToAddress = "ShipToAddress";
		public const string ShipToCity = "ShipToCity";
		public const string ShipToState = "ShipToState";
		public const string ShipToCountry = "ShipToCountry";
		public const string IPAddress = "IPAddress";
		
		private CheckoutKeys()
		{
		}
	}
}
