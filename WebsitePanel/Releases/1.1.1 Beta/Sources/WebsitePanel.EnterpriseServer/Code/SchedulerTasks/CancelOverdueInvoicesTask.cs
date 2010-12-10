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
	public class CancelOverdueInvoicesTask : SchedulerTask
	{
		public override void DoWork()
		{
			// cancel overdue services
			CancelOverdueServices();
		}

		public void CancelOverdueServices()
		{
			// load store settings
			StoreSettings settings = StorehouseController.GetStoreSettings(SecurityContext.User.UserId,
				StoreSettings.SYSTEM_SETTINGS);
			//
			int threshold = Convert.ToInt32(settings["SvcCancelThreshold"]);
			//
			TimeSpan ts = new TimeSpan(threshold, 0, 0, 0);
			// calculate actual suspend date
			DateTime dueDate = DateTime.Now.Subtract(ts);
			// lookup for overdue invoices
			List<InvoiceItem> items = InvoiceController.GetInvoicesItemsOverdue(SecurityContext.User.UserId, dueDate);
			// TRACE
			TaskManager.Write("Cancel overdue services");
			TaskManager.WriteParameter("Items found", items.Count);
			//
			foreach (InvoiceItem item in items)
			{
				try
				{
					TaskManager.Write("Cancelling service");
					// cancelling
					GenericSvcResult result = CancelService(item.ServiceId, true);
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
					TaskManager.Write("Cancelled");
				}
				catch (Exception ex)
				{
					TaskManager.WriteError(ex);
				}
			}
		}

		public GenericSvcResult CancelService(int serviceId, bool sendEmail)
		{
			GenericSvcResult result = null;
			// load svc type
			ProductType svc_type = ServiceController.GetServiceItemType(serviceId);
			// instantiate svc controller
			IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
				Type.GetType(svc_type.ProvisioningController));
			// create context
			ProvisioningContext context = controller.GetProvisioningContext(serviceId, sendEmail);
			// cancel svc
			result = controller.CancelService(context);
			//
			return result;
		}
	}
}
