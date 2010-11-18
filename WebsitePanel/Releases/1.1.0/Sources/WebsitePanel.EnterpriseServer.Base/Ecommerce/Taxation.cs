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
	public enum TaxationType
	{
		Fixed = 1,
		Percentage = 2,
		TaxIncluded = 3
	};

	[Serializable]
	public class Taxation
	{
		private int taxationId;
		private int resellerId;
		private string country;
		private string state;
		private string description;
		private int typeId;
		private decimal amount;
		private bool active;

		public int TaxationId
		{
			get { return this.taxationId; }
			set { this.taxationId = value; }
		}

		public int ResellerId
		{
			get { return this.resellerId; }
			set { this.resellerId = value; }
		}

		public string Country
		{
			get { return this.country; }
			set { this.country = value; }
		}

		public string State
		{
			get { return this.state; }
			set { this.state = value; }
		}

		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

		public int TypeId
		{
			get { return this.typeId; }
			set { this.typeId = value; }
		}

		public TaxationType Type
		{
			get { return (TaxationType)typeId; }
			set { typeId = (int)value; }
		}
	
		public decimal Amount
		{
			get { return this.amount; }
			set { this.amount = value; }
		}

		public bool Active
		{
			get { return this.active; }
			set { this.active = value; }
		}
	}
}
