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
using System.Xml;
using System.Collections;
using System.Collections.Generic;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class ActivatePaidInvoicesTask : SchedulerTask
	{
		public override void DoWork()
		{
			//
			PickupReceivedTransactions();
			// activate paid services
			ActivatePaidServices();
		}

		public void PickupReceivedTransactions()
		{
			TaskManager.Write("Start looking for transactions submitted");
			//
			List<HandlerResponse> transactions = ServiceHandlerController.GetServiceHandlersResponsesByReseller(SecurityContext.User.UserId);
			//
			if (transactions.Count > 0)
			{
				XmlDocument xmldoc = new XmlDocument();
				XmlElement root = xmldoc.CreateElement("Result");
				XmlElement succeedNode = xmldoc.CreateElement("Succeed");
				XmlElement failedNode = xmldoc.CreateElement("Failed");
				root.AppendChild(succeedNode);
				root.AppendChild(failedNode);
				//
				List<HandlerResponse> succeedItems = new List<HandlerResponse>();
				List<HandlerResponse> failedItems = new List<HandlerResponse>();
				//
				TaskManager.Write("Found {0} transactions pending", transactions.Count.ToString());
				foreach (HandlerResponse transaction in transactions)
				{
					XmlElement responseNode = xmldoc.CreateElement("Response");
					responseNode.SetAttribute("ID", Convert.ToString(transaction.ResponseId));
					//
					try
					{
						CheckoutDetails details = new CheckoutDetails();
						//
						string[] dataPairs = transaction.TextResponse.Split('&');
						foreach (string dataPair in dataPairs)
						{
							string[] data = dataPair.Split('=');
							if (data.Length >= 2)
								details[data[0]] = data[1];
						}
						//
						CheckoutResult result = PaymentGatewayController.CheckOut(transaction.ContractId, transaction.InvoiceId,
							transaction.MethodName, details);
						//
						if (result.Succeed)
						{
							succeedNode.AppendChild(responseNode);
							succeedItems.Add(transaction);
						}
						else
						{
							responseNode.SetAttribute("Error", result.StatusCode);
							failedNode.AppendChild(responseNode);
							//
							transaction.ErrorMessage = result.StatusCode;
							failedItems.Add(transaction);
						}
					}
					catch (Exception ex)
					{
						//
						if (!failedItems.Contains(transaction))
						{
							responseNode.SetAttribute("Error", ex.StackTrace);
							failedNode.AppendChild(responseNode);
							//
							transaction.ErrorMessage = ex.StackTrace;
							failedItems.Add(transaction);
						}
						//
						TaskManager.WriteError(ex);
					}
				}
				// peform transactions update
				ServiceHandlerController.UpdateServiceHandlersResponses(SecurityContext.User.UserId, root.InnerXml);
			}
			else
			{
				TaskManager.Write("No transactions found");
			}
			TaskManager.Write("End looking for transactions submitted");
		}

		public void ActivatePaidServices()
		{
			// load paid invoice items
			List<InvoiceItem> items = InvoiceController.GetInvoicesItemsToActivate(SecurityContext.User.UserId);
			// TRACE
			TaskManager.Write("Activate paid services");
			TaskManager.WriteParameter("Items found", items.Count);
			// iterate
			foreach (InvoiceItem item in items)
			{
				try
				{
					TaskManager.Write("Activating service");
					// activating
					GenericSvcResult result = ActivateInvoiceItem(item);
					// LOG ERROR
					if (!result.Succeed)
					{
						TaskManager.WriteError(result.Error);
						if (!String.IsNullOrEmpty(result.ErrorCode))
							TaskManager.WriteParameter("Error code", result.ErrorCode);
						TaskManager.WriteParameter("Result code", result.ResultCode);
						// go to next item
						continue;
					}
					//
					TaskManager.Write("Activated");
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex);
				}
			}
		}

		public GenericSvcResult ActivateInvoiceItem(InvoiceItem item)
		{
			GenericSvcResult svc_result = ActivateService(item.ServiceId, true, true);
			//
			if (svc_result.Succeed)
				InvoiceController.SetInvoiceItemProcessed(item.InvoiceId, item.ItemId);
			//
			return svc_result;
		}

		public GenericSvcResult ActivateService(int serviceId, bool sendEmail, bool logSvcUsage)
		{
            GenericSvcResult result = null;
			// load svc type
			ProductType svc_type = ServiceController.GetServiceItemType(serviceId);
			// 
			if (svc_type == null)
			{
				result = new GenericSvcResult();
				result.Succeed = true;
				return result;
			}
			// instantiate svc controller
			IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
				Type.GetType(svc_type.ProvisioningController));
			// create context
			ProvisioningContext context = controller.GetProvisioningContext(serviceId, sendEmail);
			// activate svc
			result = controller.ActivateService(context);
			// check result
			if (result.Succeed)
			{
				// log svc usage
				if (logSvcUsage)
					controller.LogServiceUsage(context);
			}
			//
			return result;
		}
	}
}