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
using System.Data;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ES = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class PaymentGatewayController
	{
		public const string TASK_SOURCE = "PAYMENT_CONTROLLER";
		public const string CHECKOUT_TASK = "CHECKOUT";
		public const string GENERAL_FAILURE = "CHECKOUT_GENERAL_FAILURE";

		private PaymentGatewayController()
		{
		}

		#region Payment Gateway routines

		/// <summary>
		/// Performs checkout operation
		/// </summary>
		/// <param name="spaceId">Space.</param>
		/// <param name="gatewayId">Gateway.</param>
		/// <param name="invoiceId">Invoice.</param>
		/// <param name="details">Array of parameters.</param>
		/// <returns>Checkout result object.</returns>
		public static CheckoutResult CheckOut(string contractId, int invoiceId, string methodName,
			CheckoutDetails details)
		{
			CheckoutResult result = new CheckoutResult();

			try
			{
                Contract contractInfo = ContractSystem.ContractController.GetContract(contractId);
				// impersonate
                ContractSystem.ContractController.ImpersonateAsContractReseller(contractInfo);
				// TRACE
				ES.TaskManager.StartTask(TASK_SOURCE, CHECKOUT_TASK, methodName);
				ES.TaskManager.Write("Start accepting payment for invoice");
                ES.TaskManager.WriteParameter("ContractID", contractId);
				ES.TaskManager.WriteParameter("InvoiceID", invoiceId);

				// get user details
                ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(contractId);

				// try to load plugin type and throw an exception if type not found
                IPaymentGatewayProvider provider = (IPaymentGatewayProvider)SystemPluginController.GetContractPaymentMethod(
                    contractInfo, methodName);

				// add invoice details
				Invoice invoice = InvoiceController.GetCustomerInvoiceInternally(invoiceId);

				// append information for the provider
                details[CheckoutKeys.ContractNumber] = contractId;
				details[CheckoutKeys.Amount] = invoice.Total.ToString("0.00");
                details[CheckoutKeys.InvoiceNumber] = invoice.InvoiceNumber;
				details[CheckoutKeys.Currency] = invoice.Currency;

				ES.TaskManager.Write("Submitting payment transaction");
				// call checkout routine
				TransactionResult pgResult = provider.SubmitPaymentTransaction(details);
				// log provider response
				SystemPluginController.LogContractPayment(contractInfo, methodName, pgResult.RawResponse);
				// ERROR
				if (!pgResult.Succeed)
				{
					result.Succeed = false;
					result.StatusCode = pgResult.StatusCode;
					//
					ES.TaskManager.WriteError("Transaction failed");
					ES.TaskManager.WriteParameter("StatusCode", result.StatusCode);
					ES.TaskManager.WriteParameter("RawResponse", pgResult.RawResponse);
					// EXIT
					return result;
				}
				// OK
				ES.TaskManager.Write("Transaction is OK");

				// check whether the transaction already exists
				CustomerPayment tran = StorehouseController.LookupForTransaction(pgResult.TransactionId);

				// lookup for pending transaction
				if (tran == null)
				{
					// add payment record
					result.PaymentId = StorehouseController.AddCustomerPayment(contractId, invoice.InvoiceId,
						pgResult.TransactionId, invoice.Total, invoice.Currency, methodName,
						pgResult.TransactionStatus);
					// ERROR
					if (result.PaymentId < 1)
					{
						result.Succeed = false;
						result.StatusCode = result.PaymentId.ToString();
						//
						ES.TaskManager.WriteError("Could not add customer payment record to the db");
						ES.TaskManager.WriteParameter("ResultCode", result.StatusCode);
						// EXIT
						return result;
					}
				}
				// if transaction is already submitted just update it's status
				if (tran != null)
					StorehouseController.UpdateTransactionStatus(tran.PaymentId, pgResult.TransactionStatus);
				// OK
				result.Succeed = true;
				// ensure user requests to persist his payment details for credit card
				if (details.Persistent && methodName == PaymentMethod.CREDIT_CARD)
					StorehouseController.SetPaymentProfile(contractId, details);
			}
			catch (Exception ex)
			{
				result.Succeed = false;
				result.StatusCode = GENERAL_FAILURE;
				//
				ES.TaskManager.WriteError(ex);
			}
			finally
			{
				ES.TaskManager.CompleteTask();
			}
			// EXIT
			return result;
		}

		#endregion
    }
}
