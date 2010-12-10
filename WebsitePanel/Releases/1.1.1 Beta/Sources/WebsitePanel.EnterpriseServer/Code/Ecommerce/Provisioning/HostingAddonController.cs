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
	public class HostingAddonController : ServiceProvisioningBase, IServiceProvisioning
	{
		public const string SOURCE_NAME = "SPF_HOSTING_ADDN";

		#region Trace Messages

		public const string START_ACTIVATION_MSG = "Starting addon activation";
		public const string START_SUSPENSION_MSG = "Starting addon suspension";
		public const string START_CANCELLATION_MSG = "Starting addon cancellation";
		public const string START_ROLLBACK_MSG = "Trying rollback operation";

		public const string ADDON_PROVISIONED_MSG = "Addon has been provisioned";

		#endregion

		#region Error Messages

		public const string ERROR_CREATE_ADDON_MSG = "Could not create hosting addon";
		
		public const string ERROR_ACTIVATE_ADDON_MSG = "Could not activate hosting addon";
		public const string ERROR_SUSPEND_ADDON_MSG = "Could not suspend hosting addon";
		public const string ERROR_CANCEL_ADDON_MSG = "Could not cancel hosting addon";

		public const string ERROR_ROLLBACK_ORDER_MSG = "Could not rollback operation and delete addon";
		public const string ERROR_ROLLBACK_MSG = "Could not rollback operation and revert addon state";
		
		public const string ADDON_NOT_FOUND_MSG = "Could not find hosting addon";
		

		#endregion

		protected HostingAddonSvc GetHostingAddonSvc(int serviceId)
		{
			return ObjectUtils.FillObjectFromDataReader<HostingAddonSvc>(
				EcommerceProvider.GetHostingAddonSvc(SecurityContext.User.UserId, serviceId));
		}

		protected InvoiceItem GetAddonSvcFee(HostingAddonSvc service)
		{
			InvoiceItem line = new InvoiceItem();

			line.ItemName = service.ServiceName;
			line.ServiceId = service.ServiceId;
			line.Quantity = service.Quantity;
			line.UnitPrice = service.CyclePrice;

			if (service.Recurring && service.Status != ServiceStatus.Ordered)
				line.TypeName = "Recurring Fee";
			else
				line.TypeName = Product.HOSTING_ADDON_NAME;
			
			return line;
		}

		protected InvoiceItem GetAddonSvcSetupFee(HostingAddonSvc service)
		{
			InvoiceItem line = new InvoiceItem();

			line.ItemName = service.ServiceName;
			line.Quantity = service.Quantity;
			line.UnitPrice = service.SetupFee;
			line.TypeName = "Setup Fee";

			return line;
		}

		#region IServiceProvisioning Members

		public ServiceHistoryRecord[] GetServiceHistory(int serviceId)
		{
			List<ServiceHistoryRecord> history = ObjectUtils.CreateListFromDataReader<ServiceHistoryRecord>(
				EcommerceProvider.GetHostingAddonSvcHistory(SecurityContext.User.UserId, serviceId));
			//
			if (history != null)
				return history.ToArray();
			//
			return new ServiceHistoryRecord[] { };
		}

		public InvoiceItem[] CalculateInvoiceLines(int serviceId)
		{
			List<InvoiceItem> lines = new List<InvoiceItem>();
			//
			HostingAddonSvc addonSvc = GetHostingAddonSvc(serviceId);
			// addon svc fee
			lines.Add(GetAddonSvcFee(addonSvc));
			// addon svc setup fee
			if (addonSvc.SetupFee > 0M && addonSvc.Status == ServiceStatus.Ordered)
				lines.Add(GetAddonSvcSetupFee(addonSvc));
			//
			return lines.ToArray();
		}

		public int AddServiceInfo(string contractId, string currency, OrderItem orderItem)
		{
			// get hosting addon product
			HostingAddon addon = StorehouseController.GetHostingAddon(SecurityContext.User.UserId,
				orderItem.ProductId);
			// uncountable addons always have 1 for quantity
			int quantity = addon.Countable ? orderItem.Quantity : 1;
			// add hosting addon
			return EcommerceProvider.AddHostingAddonSvc(contractId, orderItem.ParentSvcId, orderItem.ProductId, 
				quantity, orderItem.ItemName, orderItem.BillingCycle, currency);
		}

		public Service GetServiceInfo(int serviceId)
		{
			return GetHostingAddonSvc(serviceId);
		}

		public int UpdateServiceInfo(Service serviceInfo)
		{
			HostingAddonSvc addonSvc = (HostingAddonSvc)serviceInfo;
			//
			return EcommerceProvider.UpdateHostingAddonSvc(SecurityContext.User.UserId, addonSvc.ServiceId, 
				addonSvc.ProductId, addonSvc.ServiceName, (int)addonSvc.Status, addonSvc.PlanId, 
				addonSvc.PackageAddonId, addonSvc.Recurring, addonSvc.DummyAddon);
		}

		public GenericSvcResult ActivateService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();

			// remeber svc state
			SaveObjectState(SERVICE_INFO, context.ServiceInfo);

			// concretize service to be provisioned
			HostingAddonSvc addonSvc = (HostingAddonSvc)context.ServiceInfo;
			// concretize parent svc
			HostingPackageSvc packageSvc = (HostingPackageSvc)context.ParentSvcInfo;

			//
			try
			{
				//
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_ACTIVATE);

				// LOG INFO
				TaskManager.Write(START_ACTIVATION_MSG);
				TaskManager.WriteParameter(CONTRACT_PARAM, addonSvc.ContractId);
				TaskManager.WriteParameter(SVC_PARAM, addonSvc.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, addonSvc.ServiceId);

				// 0. Do security checks
				if (!CheckOperationClientPermissions(result))
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_CLIENT_OPERATION_PERMISSIONS);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					// EXIT
					return result;
				}
				//
				if (!CheckOperationClientStatus(result))
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_CLIENT_OPERATION_STATUS);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					// EXIT
					return result;
				}

				// dummy addon should be just updated in metabase
				if (addonSvc.DummyAddon)
					goto UpdateSvcMetaInfo;

				if (addonSvc.Status == ServiceStatus.Ordered)
				{
					// error: hosting addon should have parent svc assigned
					if (packageSvc == null || packageSvc.PackageId == 0)
					{
						result.Succeed = false;
						//
						result.Error = PARENT_SVC_NOT_FOUND_MSG;
						//
						result.ResultCode = EcommerceErrorCodes.ERROR_PARENT_SVC_NOT_FOUND;

						// LOG ERROR
						TaskManager.WriteError(result.Error);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// EXIT
						return result;
					}

					// fill package add-on fields
					PackageAddonInfo addon = new PackageAddonInfo();
					//
					addon.PackageId = packageSvc.PackageId;
					//
					addon.PlanId = addonSvc.PlanId;
					// set addon quantity
					addon.Quantity = addonSvc.Quantity;
					//
					addon.StatusId = (int)PackageStatus.Active;
					//
					addon.PurchaseDate = DateTime.Now;

					// Create hosting addon through WebsitePanel API
					PackageResult apiResult = PackageController.AddPackageAddon(addon);
					// Failed to create addon
					if (apiResult.Result < 1)
					{
						result.Succeed = false;
						//
						result.ResultCode = apiResult.Result;

						// LOG ERROR
						TaskManager.WriteError(ERROR_CREATE_ADDON_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// EXIT
						return result;
					}
					// store package id
					addonSvc.PackageAddonId = apiResult.Result;
				}
				else
				{
					// load package addon
					PackageAddonInfo addonInfo = PackageController.GetPackageAddon(addonSvc.PackageAddonId);
					// package addon not found
					if (addonInfo == null)
					{
						result.Succeed = false;
						//
						result.ResultCode = EcommerceErrorCodes.ERROR_PCKG_ADDON_NOT_FOUND;
						//
						result.Error = ADDON_NOT_FOUND_MSG;

						// LOG ERROR
						TaskManager.WriteError(result.Error);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// EXIT
						return result;
					}

					// workaround for bug in GetPackageAddon routine
					//addonInfo.PackageAddonId = addonSvc.PackageAddonId;
					// change package add-on status
					addonInfo.StatusId = (int)PackageStatus.Active;

					// save hosting addon changes
					PackageResult apiResult = PackageController.UpdatePackageAddon(addonInfo);
					// check returned result
					if (apiResult.Result < 0)
					{
						result.Succeed = false;
						//
						result.ResultCode = apiResult.Result;
						
						// LOG ERROR
						TaskManager.WriteError(ERROR_ACTIVATE_ADDON_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// EXIT
						return result;
					}
				}

				UpdateSvcMetaInfo:
				// update status only if necessary
				if (addonSvc.Status != ServiceStatus.Active)
				{
					// change service status to active
					addonSvc.Status = ServiceStatus.Active;
					// put data into metabase
					int svcResult = UpdateServiceInfo(addonSvc);
					// failed to update metabase
					if (svcResult < 0)
					{
						result.ResultCode = svcResult;
						//
						result.Succeed = false;
						//
						result.Error = ERROR_SVC_UPDATE_MSG;

						// LOG ERROR
						TaskManager.WriteError(result.Error);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// ROLLBACK CHANGES
						RollbackOperation(addonSvc.PackageAddonId);

						// EXIT
						return result;
					}
				}
                //
                SetOutboundParameters(context);
				// LOG INFO
				TaskManager.Write(ADDON_PROVISIONED_MSG);
				//
				result.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);

				// ROLLBACK CHANGES
				RollbackOperation(addonSvc.PackageAddonId);

				//
				result.Succeed = false;
				//
				result.Error = ex.Message;
			}
			finally
			{
				// complete task
				TaskManager.CompleteTask();
			}

			//
			return result;
		}

		public GenericSvcResult SuspendService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();
			// 
			SaveObjectState(SERVICE_INFO, context.ServiceInfo);
			// concretize service to be provisioned
			HostingAddonSvc addonSvc = (HostingAddonSvc)context.ServiceInfo;
			//
			try
			{
				//
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_SUSPEND);
				// LOG INFO
				TaskManager.Write(START_SUSPENSION_MSG);
				TaskManager.WriteParameter(CONTRACT_PARAM, addonSvc.ContractId);
				TaskManager.WriteParameter(SVC_PARAM, addonSvc.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, addonSvc.ServiceId);

				// 0. Do security checks
				if (!CheckOperationClientPermissions(result))
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_CLIENT_OPERATION_PERMISSIONS);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					// EXIT
					return result;
				}
				//
				if (!CheckOperationClientStatus(result))
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_CLIENT_OPERATION_STATUS);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					// EXIT
					return result;
				}

				// dummy addon should be just updated in metabase
				if (addonSvc.DummyAddon)
					goto UpdateSvcMetaInfo;

				PackageAddonInfo addonInfo = PackageController.GetPackageAddon(addonSvc.PackageAddonId);
				addonInfo.StatusId = (int)PackageStatus.Suspended;
				// suspend hosting addon
				int apiResult = PackageController.UpdatePackageAddon(addonInfo).Result;

				// check WebsitePanel API result
				if (apiResult < 0)
				{
					result.ResultCode = apiResult;
					//
					result.Succeed = false;

					// LOG ERROR
					TaskManager.WriteError(ERROR_SUSPEND_ADDON_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

					// exit
					return result;
				}

				UpdateSvcMetaInfo:
					// change addon status to Suspended
					addonSvc.Status = ServiceStatus.Suspended;
					// put data into metabase
					int svcResult = UpdateServiceInfo(addonSvc);
					//
					if (svcResult < 0)
					{
						result.ResultCode = svcResult;
						//
						result.Succeed = false;

						// LOG ERROR
						TaskManager.WriteError(ERROR_SVC_UPDATE_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// ROLLBACK CHANGES
						RollbackOperation(addonSvc.PackageAddonId);

						// EXIT
						return result;
					}
                //
                SetOutboundParameters(context);
				// LOG INFO
				TaskManager.Write(ADDON_PROVISIONED_MSG);
				//
				result.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);

				// ROLLBACK CHANGES
				RollbackOperation(addonSvc.PackageAddonId);

				//
				result.Succeed = false;
				//
				result.Error = ex.Message;
			}
			finally
			{
				// complete task
				TaskManager.CompleteTask();
			}

			//
			return result;
		}

		public GenericSvcResult CancelService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();
			// 
			SaveObjectState(SERVICE_INFO, context.ServiceInfo);
			// concretize service to be provisioned
			HostingAddonSvc addonSvc = (HostingAddonSvc)context.ServiceInfo;
			//
			try
			{
				//
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_CANCEL);
				// LOG INFO
				TaskManager.Write(START_CANCELLATION_MSG);
				TaskManager.WriteParameter(CONTRACT_PARAM, addonSvc.ContractId);
				TaskManager.WriteParameter(SVC_PARAM, addonSvc.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, addonSvc.ServiceId);

				// 0. Do security checks
				if (!CheckOperationClientPermissions(result))
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_CLIENT_OPERATION_PERMISSIONS);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					// EXIT
					return result;
				}
				//
				if (!CheckOperationClientStatus(result))
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_CLIENT_OPERATION_STATUS);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					// EXIT
					return result;
				}

				// dummy addon should be just updated in metabase
				if (addonSvc.DummyAddon)
					goto UpdateSvcMetaInfo;

				PackageAddonInfo addonInfo = PackageController.GetPackageAddon(addonSvc.PackageAddonId);
				addonInfo.StatusId = (int)PackageStatus.Cancelled;
				// cancel hosting addon
				int apiResult = PackageController.UpdatePackageAddon(addonInfo).Result;

				// check WebsitePanel API result
				if (apiResult < 0)
				{
					result.ResultCode = apiResult;
					//
					result.Succeed = false;

					// LOG ERROR
					TaskManager.WriteError(ERROR_CANCEL_ADDON_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

					// exit
					return result;
				}

				UpdateSvcMetaInfo:
					// change addon status to Cancelled
					addonSvc.Status = ServiceStatus.Cancelled;
					// put data into metabase
					int svcResult = UpdateServiceInfo(addonSvc);
					//
					if (svcResult < 0)
					{
						result.ResultCode = svcResult;
						//
						result.Succeed = false;

						// LOG ERROR
						TaskManager.WriteError(ERROR_SVC_UPDATE_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// ROLLBACK CHANGES
						RollbackOperation(addonSvc.PackageAddonId);

						// EXIT
						return result;
					}
                //
                SetOutboundParameters(context);
				// LOG INFO
				TaskManager.Write(ADDON_PROVISIONED_MSG);
				//
				result.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);

				// ROLLBACK CHANGES
				RollbackOperation(addonSvc.PackageAddonId);

				//
				result.Succeed = false;
				//
				result.Error = ex.Message;
			}
			finally
			{
				// complete task
				TaskManager.CompleteTask();
			}

			//
			return result;
		}

		public void LogServiceUsage(ProvisioningContext context)
		{
			// concretize service to be logged
			HostingAddonSvc addonSvc = (HostingAddonSvc)context.ServiceInfo;

			// addon is recurring
			if (addonSvc.Recurring)
			{
				// log service usage
				base.LogServiceUsage(context.ServiceInfo, addonSvc.SvcCycleId,
					addonSvc.BillingPeriod, addonSvc.PeriodLength);
			}
		}

		#endregion

		protected void RollbackOperation(int packageAddonId)
		{
			// check input parameters first
			if (packageAddonId < 1)
				return; // exit

			//
			try
			{
				TaskManager.Write(START_ROLLBACK_MSG);
				// restore service
				HostingAddonSvc addonSvc = (HostingAddonSvc)RestoreObjectState(SERVICE_INFO);
				PackageAddonInfo addonInfo = PackageController.GetPackageAddon(addonSvc.PackageAddonId);
				//
				int apiResult = 0;

				// during rollback addon should be reverted to its original state
				// compensation logic - revert back addon status
				switch (addonSvc.Status)
				{
					// Active State
					case ServiceStatus.Active:
						addonInfo.StatusId = (int)PackageStatus.Active;
						apiResult = PackageController.UpdatePackageAddon(addonInfo).Result;
						break;
					// Suspended State
					case ServiceStatus.Suspended:
						addonInfo.StatusId = (int)PackageStatus.Suspended;
						apiResult = PackageController.UpdatePackageAddon(addonInfo).Result;
						break;
					// Cancelled State
					case ServiceStatus.Cancelled:
						addonInfo.StatusId = (int)PackageStatus.Cancelled;
						apiResult = PackageController.UpdatePackageAddon(addonInfo).Result;
						break;
					// service has been just ordered & during rollback should be removed
					case ServiceStatus.Ordered:
						// compensation logic - remove created package
						apiResult = PackageController.DeletePackageAddon(packageAddonId);
						break;
				}
				// check WebsitePanel API result
				if (apiResult < 0)
				{
					//
					if (addonSvc.Status == ServiceStatus.Ordered)
						TaskManager.WriteError(ERROR_ROLLBACK_ORDER_MSG);
					else
						TaskManager.WriteError(ERROR_ROLLBACK_MSG);
					//
					TaskManager.WriteParameter(RESULT_CODE_PARAM, apiResult);
				}

				// rollback service changes in EC metabase
				apiResult = UpdateServiceInfo(addonSvc);
				// check API result
				if (apiResult < 0)
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_ROLLBACK_SVC_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, apiResult);

					//
					return;
				}

				//
				TaskManager.Write(ROLLBACK_SUCCEED_MSG);
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
		}
	}
}