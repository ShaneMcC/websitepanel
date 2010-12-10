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
	public class HostingPackageController : ServiceProvisioningBase, IServiceProvisioning
	{
		//
		public const string SOURCE_NAME = "SPF_HOSTING_PKG";

		#region Trace Messages

		public const string START_ACTIVATION_MSG = "Starting package activation";
		public const string START_SUSPENSION_MSG = "Starting package suspension";
		public const string START_CANCELLATION_MSG = "Starting package cancellation";
		
		public const string CREATE_PCKG_MSG = "Creating new hosting package";
		public const string CREATE_PCKG_ERROR_MSG = "Could not create hosting package";
		
		public const string ERROR_ACTIVATE_PCKG_MSG = "Could not activate hosting package";
		public const string ERROR_SUSPEND_PCKG_MSG = "Could not suspend hosting package";
		public const string ERROR_CANCEL_PCKG_MSG = "Could not cancel hosting package";

		public const string START_USR_ACTIVATION_MSG = "Activating service consumer account";
		public const string START_CHANGE_USR_ROLE_MSG = "Changing consumer user role";
		public const string ERROR_USR_ACTIVATION_MSG = "Could not activate consumer account";
		public const string ERROR_CHANGE_USR_ROLE_MSG = "Could not change consumer user role";
		
		public const string PCKG_PROVISIONED_MSG = "Package has been provisioned";

		#endregion

		protected HostingPackageSvc GetHostingPackageSvc(int serviceId)
		{
			return ObjectUtils.FillObjectFromDataReader<HostingPackageSvc>(
				EcommerceProvider.GetHostingPackageSvc(SecurityContext.User.UserId, serviceId));
		}

		protected InvoiceItem GetSetupFeeInvoiceLine(HostingPackageSvc service)
		{
			InvoiceItem line = new InvoiceItem();

			line.ItemName = service.ServiceName;
			line.Quantity = 1;
			line.UnitPrice = service.SetupFee;
			line.TypeName = "Setup Fee";

			return line;
		}

		protected InvoiceItem GetRecurringFeeInvoiceLine(HostingPackageSvc service)
		{
			InvoiceItem line = new InvoiceItem();

			line.ItemName = service.ServiceName;
			line.ServiceId = service.ServiceId;
			line.Quantity = 1;
			line.UnitPrice = service.RecurringFee;
			line.TypeName = (service.Status == ServiceStatus.Ordered) ? Product.HOSTING_PLAN_NAME : "Recurring Fee";

			return line;
		}

		#region IServiceProvisioning Members

		public ServiceHistoryRecord[] GetServiceHistory(int serviceId)
		{
			List<ServiceHistoryRecord> history = ObjectUtils.CreateListFromDataReader<ServiceHistoryRecord>(
				EcommerceProvider.GetHostingPackageSvcHistory(SecurityContext.User.UserId, serviceId));

			if (history != null)
				return history.ToArray();

			return new ServiceHistoryRecord[] { };
		}

		public InvoiceItem[] CalculateInvoiceLines(int serviceId)
		{
			List<InvoiceItem> lines = new List<InvoiceItem>();
			// load svc
			HostingPackageSvc packageSvc = GetHostingPackageSvc(serviceId);

			// recurring fee
			lines.Add(GetRecurringFeeInvoiceLine(packageSvc));
			// setup fee
			if (packageSvc.Status == ServiceStatus.Ordered && packageSvc.SetupFee > 0M)
				lines.Add(GetSetupFeeInvoiceLine(packageSvc));

			return lines.ToArray();
		}

		public int AddServiceInfo(string contractId, string currency, OrderItem orderItem)
		{
			return EcommerceProvider.AddHostingPlanSvc(contractId, orderItem.ProductId, 
				orderItem.ItemName, orderItem.BillingCycle, currency);
		}

		public Service GetServiceInfo(int serviceId)
		{
			return GetHostingPackageSvc(serviceId);
		}

		public int UpdateServiceInfo(Service service)
		{
			HostingPackageSvc packageSvc = (HostingPackageSvc)service;
			//
			return EcommerceProvider.UpdateHostingPlanSvc(SecurityContext.User.UserId, packageSvc.ServiceId, 
				packageSvc.ProductId, packageSvc.ServiceName, (int)packageSvc.Status, packageSvc.PlanId, 
				packageSvc.PackageId, (int)packageSvc.UserRole, (int)packageSvc.InitialStatus);
		}

		public GenericSvcResult ActivateService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();

			// 
			SaveObjectState(SERVICE_INFO, context.ServiceInfo);
			// 
			SaveObjectState(CONSUMER_INFO, context.ConsumerInfo);

			// concretize service to be provisioned
			HostingPackageSvc packageSvc = (HostingPackageSvc)context.ServiceInfo;
			//
			try
			{
				//
				TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_ACTIVATE);

				// LOG INFO
				TaskManager.Write(START_ACTIVATION_MSG);
				TaskManager.WriteParameter(USERNAME_PARAM, context.ConsumerInfo[ContractAccount.USERNAME]);
				TaskManager.WriteParameter(SVC_PARAM, context.ServiceInfo.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, context.ServiceInfo.ServiceId);
			    TaskManager.TaskParameters[SystemTaskParams.PARAM_SEND_EMAIL] = context.SendEmail;

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

				// 1. Hosting package is just ordered
				if (context.ServiceInfo.Status == ServiceStatus.Ordered && context.ContractInfo.CustomerId > 0)
				{
					// LOG INFO
					TaskManager.Write(CREATE_PCKG_MSG);
					// create new package
					PackageResult apiResult = PackageController.AddPackage(context.ContractInfo.CustomerId, packageSvc.PlanId, 
						packageSvc.ServiceName, String.Empty, (int)packageSvc.InitialStatus, DateTime.Now, true, true);

					// failed to instantiate package
					if (apiResult.Result <= 0)
					{
						result.ResultCode = apiResult.Result;
						//
						result.Succeed = false;

						// LOG ERROR
						TaskManager.WriteError(CREATE_PCKG_ERROR_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
						
						// EXIT
						return result;
					}
					// save result PackageId
					packageSvc.PackageId = apiResult.Result;
				}
				else // 2. Package requires only to update its status
				{
					// LOG INFO
					TaskManager.Write(START_ACTIVATION_MSG);

					//
					int apiResult = PackageController.ChangePackageStatus(packageSvc.PackageId,
						PackageStatus.Active, false);
					//
					if (apiResult < 0)
					{
						result.ResultCode = apiResult;
						//
						result.Succeed = false;

						// LOG ERROR
						TaskManager.WriteError(ERROR_ACTIVATE_PCKG_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
						
						// EXIT
						return result;
					}
				}
				// check user role
				if (context.ContractInfo.CustomerId > 0)
				{
                    UserInfo user = UserController.GetUserInternally(context.ContractInfo.CustomerId);
                    // check user status
                    //
                    if (user.Status != UserStatus.Active)
                    {
                        // LOG INFO
                        TaskManager.Write(START_USR_ACTIVATION_MSG);

                        // trying to change user status
                        int userResult = UserController.ChangeUserStatus(context.ContractInfo.CustomerId,
                            UserStatus.Active);
                        // failed to activate user account
                        if (userResult < 0)
                        {
                            result.ResultCode = userResult;
                            //
                            result.Succeed = false;

                            // LOG ERROR
                            TaskManager.WriteError(ERROR_USR_ACTIVATION_MSG);
                            TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

                            // ROLLBACK CHANGES
                            RollbackOperation(result.ResultCode);

                            // EXIT
                            return result;
                        }
                    }
                    // check user role
                    if (user.Role != packageSvc.UserRole)
                    {
                        // LOG INFO
                        TaskManager.Write(START_CHANGE_USR_ROLE_MSG);
                        //
                        user.Role = packageSvc.UserRole;
                        // trying to change user role
                        int roleResult = UserController.UpdateUser(user);
                        // failed to change user role
                        if (roleResult < 0)
                        {
                            result.ResultCode = roleResult;
                            //
                            result.Succeed = false;

                            //
							TaskManager.WriteError(ERROR_CHANGE_USR_ROLE_MSG);
                            TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

                            // ROLLBACK CHANGES
                            RollbackOperation(result.ResultCode);

                            // EXIT
                            return result;
                        }
                    }
				}
				// update plan status if necessary
				if (packageSvc.Status != ServiceStatus.Active)
				{
					// change service status to active
					packageSvc.Status = ServiceStatus.Active;
					// put data into metabase
					int svcResult = UpdateServiceInfo(packageSvc);

					// error updating svc details
					if (svcResult < 0)
					{
						result.ResultCode = svcResult;
						//
						result.Succeed = false;

						// ROLLBACK CHANGES
						RollbackOperation(packageSvc.PackageId);

						// LOG ERROR
						TaskManager.WriteError(ERROR_SVC_UPDATE_MSG);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

						// EXIT
						return result;
					}
				}
                //
                SetOutboundParameters(context);
				// LOG INFO
				TaskManager.Write(PCKG_PROVISIONED_MSG);

				//
				result.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);

				// ROLLBACK CHANGES
				RollbackOperation(packageSvc.PackageId);

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
			// 
			SaveObjectState(CONSUMER_INFO, context.ConsumerInfo);

			// concretize service to be provisioned
			HostingPackageSvc packageSvc = (HostingPackageSvc)context.ServiceInfo;

			//
			try
			{
				//
				TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_SUSPEND);
				// LOG INFO
				TaskManager.Write(START_SUSPENSION_MSG);
                TaskManager.WriteParameter(CONTRACT_PARAM, context.ConsumerInfo[ContractAccount.USERNAME]);
				TaskManager.WriteParameter(SVC_PARAM, context.ServiceInfo.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, context.ServiceInfo.ServiceId);

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

				// suspend hosting package
				int apiResult = PackageController.ChangePackageStatus(packageSvc.PackageId, 
					PackageStatus.Suspended, false);

				// check WebsitePanel API result
				if (apiResult < 0)
				{
					result.ResultCode = apiResult;
					//
					result.Succeed = false;

					// LOG ERROR
					TaskManager.WriteError(ERROR_SUSPEND_PCKG_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					
					// exit
					return result;
				}

				// change service status to Suspended
				packageSvc.Status = ServiceStatus.Suspended;
				// put data into metabase
				int svcResult = UpdateServiceInfo(packageSvc);

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
					RollbackOperation(packageSvc.PackageId);
					
					//
					return result;
				}
                //
                SetOutboundParameters(context);
				// LOG INFO
				TaskManager.Write(PCKG_PROVISIONED_MSG);
				//
				result.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);

				// ROLLBACK CHANGES
				RollbackOperation(packageSvc.PackageId);

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
			// 
			SaveObjectState(CONSUMER_INFO, context.ConsumerInfo);

			// concretize service to be provisioned
			HostingPackageSvc packageSvc = (HostingPackageSvc)context.ServiceInfo;

			//
			try
			{
				//
				TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_CANCEL);
				// LOG INFO
				TaskManager.Write(START_CANCELLATION_MSG);
				TaskManager.WriteParameter(CONTRACT_PARAM, context.ConsumerInfo[ContractAccount.USERNAME]);
				TaskManager.WriteParameter(SVC_PARAM, context.ServiceInfo.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, context.ServiceInfo.ServiceId);

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

				// cancel hosting package
				int apiResult = PackageController.ChangePackageStatus(packageSvc.PackageId, 
					PackageStatus.Cancelled, false);

				// check WebsitePanel API result
				if (apiResult < 0)
				{
					//
					result.ResultCode = apiResult;
					//
					result.Succeed = false;

					// LOG ERROR
					TaskManager.WriteError(ERROR_CANCEL_PCKG_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
					
					// EXIT
					return result;
				}

				// change service status to Cancelled
				packageSvc.Status = ServiceStatus.Cancelled;
				// put data into metabase
				int svcResult = UpdateServiceInfo(packageSvc);

				//
				if (svcResult < 0)
				{
					result.ResultCode = svcResult;
					//
					result.Error = ERROR_SVC_UPDATE_MSG;
					//
					result.Succeed = false;

					// ROLLBACK CHANGES
					RollbackOperation(packageSvc.PackageId);

					// LOG ERROR
					TaskManager.WriteError(result.Error);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

					// EXIT
					return result;
				}
                //
                SetOutboundParameters(context);
				// LOG INFO
				TaskManager.Write(PCKG_PROVISIONED_MSG);
				//
				result.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);

				// ROLLBACK CHANGES
				RollbackOperation(packageSvc.PackageId);

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
			HostingPackageSvc packageSvc = (HostingPackageSvc)context.ServiceInfo;
			
			// log service usage
			base.LogServiceUsage(context.ServiceInfo, packageSvc.SvcCycleId, 
				packageSvc.BillingPeriod, packageSvc.PeriodLength);
		}

		protected void RollbackOperation(int packageId)
		{
			// check input parameters first
			if (packageId < 1)
				return; // exit

			//
			try
			{
				TaskManager.Write("Trying rollback operation");
				// restore service
				HostingPackageSvc packageSvc = (HostingPackageSvc)RestoreObjectState("ServiceInfo");
				// restore consumer
				UserInfo consumer = (UserInfo)RestoreObjectState("ConsumerInfo");

				//
				int apiResult = 0;

				// rollback consumer changes first
				apiResult = UserController.UpdateUser(consumer);
				// check WebsitePanel API result
				if (apiResult < 0)
				{
					//
					TaskManager.WriteError("Could not rollback consumer changes");
					//
					TaskManager.WriteParameter("ResultCode", apiResult);
				}

				// during rollback package should be reverted to its original state
				// compensation logic - revert back package status
				switch (packageSvc.Status)
				{
					// Active State
					case ServiceStatus.Active:
						apiResult = PackageController.ChangePackageStatus(packageId, PackageStatus.Active, false);
						break;
					// Suspended State
					case ServiceStatus.Suspended:
						apiResult = PackageController.ChangePackageStatus(packageId, PackageStatus.Suspended, false);
						break;
					// Cancelled State
					case ServiceStatus.Cancelled:
						apiResult = PackageController.ChangePackageStatus(packageId, PackageStatus.Cancelled, false);
						break;
					// service has been just ordered & during rollback should be removed
					case ServiceStatus.Ordered:
						// compensation logic - remove created package
						apiResult = PackageController.DeletePackage(packageId);
						break;
				}
				// check WebsitePanel API result
				if (apiResult < 0)
				{
					//
					if (packageSvc.Status == ServiceStatus.Ordered)
						TaskManager.WriteError("Could not rollback operation and delete package");
					else
						TaskManager.WriteError("Could not rollback operation and revert package state");
					//
					TaskManager.WriteParameter("ResultCode", apiResult);
				}

				// rollback service changes in EC metabase
				apiResult = UpdateServiceInfo(packageSvc);
				// check API result
				if (apiResult < 0)
				{
					//
					TaskManager.WriteError("Could not rollback service changes");
					//
					TaskManager.WriteParameter("ResultCode", apiResult);
				}

				//
				TaskManager.Write("Rollback succeed");
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
		}

		#endregion
	}
}
