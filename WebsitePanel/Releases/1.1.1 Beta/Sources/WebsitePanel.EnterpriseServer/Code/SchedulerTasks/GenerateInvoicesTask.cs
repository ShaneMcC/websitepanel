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
	public class GenerateInvoicesTask : SchedulerTask
	{
		public override void DoWork()
		{
			// invoice active services
			InvoiceActiveServices();
		}

		public void InvoiceActiveServices()
		{
			DateTime dateTimeNow = DateTime.Now;
			// load store settings
			StoreSettings settings = StorehouseController.GetStoreSettings(SecurityContext.User.UserId,
				StoreSettings.SYSTEM_SETTINGS);
			//
			int threshold = Convert.ToInt32(settings["SvcInvoiceThreshold"]);
			// get expiring services today
			List<Service> services = ServiceController.GetServicesToInvoice(SecurityContext.User.UserId,
				dateTimeNow, threshold);
			// group services by users
			Dictionary<string, List<int>> usersServices = new Dictionary<string, List<int>>();
			// iterate
			foreach (Service service in services)
			{
				if (!usersServices.ContainsKey(service.ContractId))
					usersServices.Add(service.ContractId, new List<int>());

				usersServices[service.ContractId].Add(service.ServiceId);
			}
			// generate invoice per contract
			foreach (string contractId in usersServices.Keys)
			{
				try
				{
					TaskManager.Write("Creating invoice");
					// TRACE
                    Contract contract = ContractSystem.ContractController.GetContract(contractId);
                    ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(contractId);
					TaskManager.WriteParameter("ContractID", contractId);
                    TaskManager.WriteParameter("Username", account[ContractAccount.USERNAME]);
					//
					List<int> userSvcs = usersServices[contractId];
					// build invoice items
					List<InvoiceItem> invoiceLines = InvoiceController.CalculateInvoiceLinesForServices(userSvcs);
					//
					int resultCode = InvoiceController.AddInvoice(contractId, invoiceLines, null);
					//
					if (resultCode < 1)
					{
						TaskManager.WriteParameter("ResultCode", resultCode);
						continue;
					}
					//
					if (ServiceController.SetUsageRecordsClosed(userSvcs.ToArray()) != 0)
						TaskManager.WriteWarning("Unable to close usage records automatically");
					// TRACE
					TaskManager.WriteParameter("InvoiceID", resultCode);
					TaskManager.Write("Succeed");
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex);
				}
			}
		}
	}
}
