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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using ES = WebsitePanel.EnterpriseServer;
using Common.Utils;
using Microsoft.ApplicationBlocks.Data;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class ServiceController
	{
		private ServiceController()
		{
		}

		public static int SetUsageRecordsClosed(int[] serviceIds)
		{
			return EcommerceProvider.SetSvcsUsageRecordsClosed(
				ES.SecurityContext.User.UserId, serviceIds);
		}

		#region Ecommerce v 2.1.0 routines

		public static DateTime GetSvcsSuspendDateAligned(int[] services, DateTime defaultValue)
		{
			string svcsXml = Utils.BuildIdentityXmlFromArray(services, "Svcs", "Svc");
			return EcommerceProvider.GetSvcsSuspendDateAligned(ES.SecurityContext.User.UserId, 
				svcsXml, defaultValue);
		}

		public static int ChangeHostingPlanSvcCycle(int serviceId, int productId, int cycleId, string currency)
		{
			return EcommerceProvider.ChangeHostingPlanSvcCycle(ES.SecurityContext.User.UserId, serviceId, 
				productId, cycleId, currency);
		}

		public static ProductType GetServiceItemType(int serviceId)
		{
			return ES.ObjectUtils.FillObjectFromDataReader<ProductType>(
				EcommerceProvider.GetServiceItemType(serviceId));
		}

		public static Service GetRawCustomerService(int serviceId)
		{
			return ES.ObjectUtils.FillObjectFromDataReader<Service>(
				EcommerceProvider.GetCustomerService(ES.SecurityContext.User.UserId, serviceId));
		}

		public static Service GetService(int serviceId)
		{
			// read service type
			ProductType svcType = GetServiceItemType(serviceId);
			// create svc controller
			IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
				Type.GetType(svcType.ProvisioningController));
			//
			return controller.GetServiceInfo(serviceId);
		}

		public static Dictionary<int, Service> GetServicesDictionary(List<InvoiceItem> invoiceLines)
		{
			List<int> serviceIds = new List<int>();
			//
			foreach (InvoiceItem invoiceLine in Array.FindAll<InvoiceItem>(
				invoiceLines.ToArray(), x => x.ServiceId > 0))
			{
				serviceIds.Add(invoiceLine.ServiceId);
			}
			//
			return GetServicesDictionary(serviceIds);
		}

		public static Dictionary<int, Service> GetServicesDictionary(List<int> serviceIds)
		{
			Dictionary<int, Service> hash = new Dictionary<int, Service>();
			//
			foreach (int serviceId in serviceIds)
				hash.Add(serviceId, GetService(serviceId));
			//
			return hash;
		}

		public static int UpdateService(Service service)
		{
			// read service type
			ProductType svcType = GetServiceItemType(service.ServiceId);
			// create svc controller
			IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
				Type.GetType(svcType.ProvisioningController));
			//
			return controller.UpdateServiceInfo(service);
		}

		public static void AddServiceUsageRecord(int serviceId)
		{
			// read service type
			ProductType svcType = GetServiceItemType(serviceId);
			// create svc controller
			IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
				Type.GetType(svcType.ProvisioningController));
			//
			controller.LogServiceUsage(controller.GetProvisioningContext(serviceId, false));
		}

		public static ServiceHistoryRecord[] GetServiceHistory(int serviceId)
		{
			// read service type
			ProductType svcType = GetServiceItemType(serviceId);
			// create svc controller
			IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
				Type.GetType(svcType.ProvisioningController));
			//
			return controller.GetServiceHistory(serviceId);
		}

		public static GenericSvcResult ActivateService(int serviceId, bool sendEmail)
		{
			GenericSvcResult result = new GenericSvcResult();

			try
			{
				Service svc = GetRawCustomerService(serviceId);
				//
				ES.TaskManager.StartTask(SystemTasks.SOURCE_SPF, SystemTasks.SVC_ACTIVATE);
				ES.TaskManager.WriteParameter("Service", svc.ServiceName);
				//
				ActivatePaidInvoicesTask task = new ActivatePaidInvoicesTask();
				// obtain result
				result = task.ActivateService(serviceId, sendEmail, false);
			}
			catch (Exception ex)
			{
				ES.TaskManager.WriteError(ex);
			}
			finally
			{
				ES.TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static List<GenericSvcResult> ActivateInvoice(int invoiceId, bool sendEmail)
		{
			List<GenericSvcResult> results = new List<GenericSvcResult>();

			try
			{
				Invoice invoice = InvoiceController.GetCustomerInvoiceInternally(invoiceId);
				//
				ES.TaskManager.StartTask("SPF", "ACTIVATE_INVOICE");
				ES.TaskManager.WriteParameter("InvoiceID", invoiceId);
				//
				ActivatePaidInvoicesTask task = new ActivatePaidInvoicesTask();
				// load invoice lines
				List<InvoiceItem> lines = InvoiceController.GetCustomerInvoiceItems(invoiceId);
				// iterate and activate
				foreach (InvoiceItem line in lines)
				{
					if (!line.Processed && line.ServiceId > 0)
						results.Add(task.ActivateInvoiceItem(line));
				}
			}
			catch (Exception ex)
			{
				ES.TaskManager.WriteError(ex);
			}
			finally
			{
				ES.TaskManager.CompleteTask();
			}
			//
			return results;
		}

		public static GenericSvcResult SuspendService(int serviceId, bool sendEmail)
		{
			GenericSvcResult result = new GenericSvcResult();

			try
			{
				Service svc = GetRawCustomerService(serviceId);
				//
				ES.TaskManager.StartTask(SystemTasks.SOURCE_SPF, SystemTasks.SVC_SUSPEND);
				ES.TaskManager.WriteParameter("Service", svc.ServiceName);
				//
				SuspendOverdueInvoicesTask task = new SuspendOverdueInvoicesTask();
				// obtain result
				result = task.SuspendService(serviceId, sendEmail);
			}
			catch (Exception ex)
			{
				ES.TaskManager.WriteError(ex);
			}
			finally
			{
				ES.TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static GenericSvcResult CancelService(int serviceId, bool sendEmail)
		{
			GenericSvcResult result = new GenericSvcResult();

			try
			{
				Service svc = GetRawCustomerService(serviceId);
				//
				ES.TaskManager.StartTask(SystemTasks.SOURCE_SPF, SystemTasks.SVC_CANCEL);
				ES.TaskManager.WriteParameter("Service", svc.ServiceName);
				//
				CancelOverdueInvoicesTask task = new CancelOverdueInvoicesTask();
				// obtain result
				result = task.CancelService(serviceId, sendEmail);
			}
			catch (Exception ex)
			{
				ES.TaskManager.WriteError(ex);
			}
			finally
			{
				ES.TaskManager.CompleteTask();
			}
			//
			return result;
		}

		#endregion

		public static DateTime GetServiceSuspendDate(int serviceId)
		{
			// get service suspend date
			DateTime date = EcommerceProvider.GetServiceSuspendDate(ES.SecurityContext.User.UserId, serviceId);

			// check returned result
			if (date == DateTime.MinValue)
				date = DateTime.UtcNow;

			// return service suspend date
			return date;
		}

		public static List<Service> GetServicesToInvoice(int resellerId, DateTime todayDate, int daysOffset)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Service>(EcommerceProvider.GetServicesToInvoice(
				ES.SecurityContext.User.UserId, resellerId, todayDate,daysOffset));
		}

		public static int GetCustomersServicesCount(int userId, bool isReseller)
		{
			return EcommerceProvider.GetCustomersServicesCount(ES.SecurityContext.User.UserId, userId, isReseller);
		}

		public static List<Service> GetCustomersServicesPaged(int userId, bool isReseller,
			int maximumRows, int startRowIndex)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Service>(EcommerceProvider.GetCustomersServicesPaged(
				ES.SecurityContext.User.UserId, userId, isReseller, maximumRows, startRowIndex));
		}

		public static int DeleteCustomerService(int serviceId)
		{
			SecurityResult result = StorehouseController.CheckAccountNotDemoAndActive();
			//
			if (!result.Success)
				return result.ResultCode;
			//
			return EcommerceProvider.DeleteCustomerService(ES.SecurityContext.User.UserId, serviceId);
		}
	}
}