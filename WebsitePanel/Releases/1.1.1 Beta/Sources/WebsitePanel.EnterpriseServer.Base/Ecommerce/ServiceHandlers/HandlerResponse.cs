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
	public class HandlerResponse
	{
		private int responseId;
		private string serviceId;
		private int resellerId;
		private int invoiceId;
		private string contractId;
		private string textResponse;
		private DateTime received;
		private string errorMessage;
		private string methodName;

		public int ResponseId
		{
			get { return responseId; }
			set { responseId = value; }
		}

		public string ServiceId
		{
			get { return serviceId; }
			set { serviceId = value; }
		}

		public int ResellerId
		{
			get { return resellerId; }
			set { resellerId = value; }
		}

		public int InvoiceId
		{
			get { return invoiceId; }
			set { invoiceId = value; }
		}

		public string ContractId
		{
			get { return contractId; }
			set { contractId = value; }
		}

		public string TextResponse
		{
			get { return textResponse; }
			set { textResponse = value; }
		}

		public DateTime Received
		{
			get { return received; }
			set { received = value; }
		}

		public string ErrorMessage
		{
			get { return errorMessage; }
			set { errorMessage = value; }
		}

		public string MethodName
		{
			get { return methodName; }
			set { methodName = value; }
		}
	}
}
