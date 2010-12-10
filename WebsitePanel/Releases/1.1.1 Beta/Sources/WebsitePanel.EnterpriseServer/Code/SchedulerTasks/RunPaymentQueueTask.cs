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
using System.Collections;
using System.Collections.Generic;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class RunPaymentQueueTask : SchedulerTask
	{
		public override void DoWork()
		{
			// run payment queue
			RunPaymentQueue();
		}

		public void RunPaymentQueue()
		{
			int resellerId = SecurityContext.User.UserId;
			// 1. load unpaid invoices
			List<Invoice> invoices = InvoiceController.GetUnpaidInvoices(resellerId);
			// TRACE
			TaskManager.Write("Running payment queue");
			TaskManager.WriteParameter("Items found", invoices.Count);
			// 2. load payment profile for each customer
			foreach (Invoice invoice in invoices)
			{
				try
				{
					// load payment profile
					CheckoutDetails details = StorehouseController.GetPaymentProfileInternally(invoice.ContractId);
					//
					if (details != null)
					{
						// TRACE
						TaskManager.Write("Trying to submit payment");
						TaskManager.WriteParameter("InvoiceID", invoice.InvoiceId);
						// 3. submit payment for each invoice if profile exists
						CheckoutResult result = PaymentGatewayController.CheckOut(invoice.ContractId,
							invoice.InvoiceId, PaymentMethod.CREDIT_CARD, details);
						// ERROR
						if (!result.Succeed)
						{
							TaskManager.WriteError("Payment failed");
							TaskManager.WriteParameter("Result code", result.StatusCode);
							continue;
						}
						// OK
						TaskManager.Write("Payment OK");
					}
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex, "Payment failed");
				}
			}
		}
	}
}