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
using System.Data;
using System.Collections;
using System.Collections.Generic;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class DomainNameController : ServiceProvisioningBase, IServiceProvisioning
	{
		public const string SOURCE_NAME = "SPF_DOMAIN_NAME";

		public static Dictionary<int, string> ApiErrorCodesMap;

		static DomainNameController()
		{
			ApiErrorCodesMap = new Dictionary<int, string>();
			ApiErrorCodesMap.Add(BusinessErrorCodes.ERROR_DOMAIN_QUOTA_LIMIT, ERROR_DOMAIN_QUOTA_EXCEEDED);
		}

		#region Trace Messages

		public const string START_ACTIVATION_MSG = "Starting domain name activation";
		public const string START_SUSPENSION_MSG = "Starting domain name suspension";
		public const string START_CANCELLATION_MSG = "Starting domain name cancellation";
		public const string START_ROLLBACK_MSG = "Trying rollback operation";

		public const string TLD_PROVISIONED_MSG = "Domain name has been provisioned";

		#endregion

		#region Error Messages

		public const string ERROR_UPDATE_USR_SETTINGS_MSG = "Could not update user settings";
		public const string ERROR_ADD_INTERNAL_DOMAIN = "Could not add internal domain";
		public const string ERROR_DOMAIN_QUOTA_EXCEEDED = "Domain quota has been exceeded in the customer's hosting plan";
		public const string ERROR_ROLLBACK_DOM_MSG = "Could not rollback added internal domain";

		#endregion

		protected DomainNameSvc GetDomainNameSvc(int serviceId)
		{
			// assemble svc instance
			DomainNameSvc domainSvc = ObjectUtils.FillObjectFromDataReader<DomainNameSvc>(
				EcommerceProvider.GetDomainNameSvc(SecurityContext.User.UserId, serviceId));
			// deserialize svc properties
			SecurityUtils.DeserializeGenericProfile(domainSvc.PropertyNames, domainSvc.PropertyValues, domainSvc);
			// return result
			return domainSvc;
		}

		protected InvoiceItem GetDomainSvcSetupFee(DomainNameSvc service)
		{
			InvoiceItem line = new InvoiceItem();

			line.ItemName = service.ServiceName;
			line.Quantity = 1;
			line.UnitPrice = service.SetupFee;
			line.TypeName = "Setup Fee";

			return line;
		}

		protected InvoiceItem GetDomainSvcFee(DomainNameSvc service)
		{
			InvoiceItem line = new InvoiceItem();

			line.ItemName = service.ServiceName;
			line.ServiceId = service.ServiceId;
			line.Quantity = 1;
			line.UnitPrice = service.RecurringFee;
			// define line type
			if (service.Status != ServiceStatus.Ordered)
			{
				line.TypeName = "Recurring Fee";
			}
			else
			{
				switch (service["SPF_ACTION"])
				{
					case DomainNameSvc.SPF_TRANSFER_ACTION:
						line.TypeName = String.Concat(Product.TOP_LEVEL_DOMAIN_NAME, " Transfer");
						break;
					case DomainNameSvc.SPF_REGISTER_ACTION:
						line.TypeName = String.Concat(Product.TOP_LEVEL_DOMAIN_NAME, " Registration");
						break;
				}
			}

			return line;
		}

		#region IServiceProvisioning Members

		public ServiceHistoryRecord[] GetServiceHistory(int serviceId)
		{
			List<ServiceHistoryRecord> history = ObjectUtils.CreateListFromDataReader<ServiceHistoryRecord>(
				EcommerceProvider.GetDomainNameSvcHistory(SecurityContext.User.UserId, serviceId));
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
			DomainNameSvc domainSvc = GetDomainNameSvc(serviceId);
			//
			if (domainSvc["SPF_ACTION"] != DomainNameSvc.SPF_UPDATE_NS_ACTION)
			{
				// domain svc fee
				lines.Add(GetDomainSvcFee(domainSvc));
				// setup fee
				if (domainSvc.SetupFee > 0M && domainSvc.Status == ServiceStatus.Ordered)
					lines.Add(GetDomainSvcSetupFee(domainSvc));
			}
			//
			return lines.ToArray();
		}

		public Service GetServiceInfo(int serviceId)
		{
			return GetDomainNameSvc(serviceId);
		}

		public int UpdateServiceInfo(Service serviceInfo)
		{
			DomainNameSvc domainSvc = (DomainNameSvc)serviceInfo;
			// serialize props & values
			string propertyNames = null;
			string propertyValues = null;
			SecurityUtils.SerializeGenericProfile(ref propertyNames, ref propertyValues, domainSvc);
			// update svc
			return EcommerceProvider.UpdateDomainNameSvc(SecurityContext.User.UserId, domainSvc.ServiceId, 
				domainSvc.ProductId, (int)domainSvc.Status, domainSvc.DomainId, domainSvc.Fqdn, 
				propertyNames, propertyValues);
		}

		public int AddServiceInfo(string contractId, string currency, OrderItem orderItem)
		{
			string propertyNames = null;
			string propertyValues = null;
			// deserialize
			SecurityUtils.SerializeGenericProfile(ref propertyNames, ref propertyValues, orderItem);
			//
			return EcommerceProvider.AddDomainNameSvc(contractId, orderItem.ParentSvcId,
				orderItem.ProductId, orderItem.ItemName, orderItem.BillingCycle, currency, propertyNames, propertyValues);
		}

		public GenericSvcResult ActivateService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();

			// remeber svc state
			SaveObjectState(SERVICE_INFO, context.ServiceInfo);

			// concretize service to be provisioned
			DomainNameSvc domainSvc = (DomainNameSvc)context.ServiceInfo;
			// concretize parent service
			HostingPackageSvc packageSvc = (HostingPackageSvc)context.ParentSvcInfo;

			try
			{
				// LOG INFO
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.SVC_ACTIVATE);
				TaskManager.WriteParameter(CONTRACT_PARAM, domainSvc.ContractId);
				TaskManager.WriteParameter(SVC_PARAM, domainSvc.ServiceName);
				TaskManager.WriteParameter(SVC_ID_PARAM, domainSvc.ServiceId);

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

				// first of all - create internal domain in WebsitePanel
				if (domainSvc.Status == ServiceStatus.Ordered)
				{
					// create domain info object
					DomainInfo domain = ServerController.GetDomain(domainSvc.Fqdn);
					//
					if (domain != null)
						domainSvc.DomainId = domain.DomainId;
					//
					if (domain == null)
					{
						domain = new DomainInfo();
						domain.DomainName = domainSvc.Fqdn;
						domain.HostingAllowed = false;
						domain.PackageId = packageSvc.PackageId;
						// add internal domain
						domainSvc.DomainId = ServerController.AddDomain(domain);
						// check API result
						if (domainSvc.DomainId < 1)
						{
							// ASSEMBLE ERROR
							result.Succeed = false;
							// try to find corresponding error code->error message mapping
							if (ApiErrorCodesMap.ContainsKey(domainSvc.DomainId))
								result.Error = ApiErrorCodesMap[domainSvc.DomainId];
							else
								result.Error = ERROR_ADD_INTERNAL_DOMAIN;
							// copy result code
							result.ResultCode = domainSvc.DomainId;

							// LOG ERROR
							TaskManager.WriteError(result.Error);
							TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);

							// EXIT
							return result;
						}
					}
				}

				// update nameservers only
				if (domainSvc["SPF_ACTION"] == "UPDATE_NS")
				{
					// remove service here...
					ServiceController.DeleteCustomerService(domainSvc.ServiceId);
					//
					result.Succeed = true;
					// EXIT
					return result;
				}
				
				// load registrar wrapper
				IDomainRegistrar registrar = (IDomainRegistrar)SystemPluginController.GetSystemPluginInstance(
					domainSvc.ContractId, domainSvc.PluginId, true);

				#region Commented operations
				// prepare consumer account information
				/*CommandParams cmdParams = PrepeareAccountParams(context.ConsumerInfo);
				// copy svc properties
				foreach (string keyName in domainSvc.GetAllKeys())
					cmdParams[keyName] = domainSvc[keyName];

				// check registrar requires sub-account to be created
				if (registrar.SubAccountRequired)
				{
					// 1. Load user's settings
					UserSettings userSettings = LoadUserSettings(context.ConsumerInfo.UserId, registrar.PluginName);
					// 2. Ensure user has account on registrar's side
					if (userSettings.SettingsArray == null || userSettings.SettingsArray.Length == 0)
					{
						// 3. Check account exists
						bool exists = registrar.CheckSubAccountExists(context.ConsumerInfo.Username, context.ConsumerInfo.Email);
						//
						AccountResult accResult = null;
						//
						if (!exists)
						{
							// 4. Create user account
							accResult = registrar.CreateSubAccount(cmdParams);
							// copy keys & values
							foreach (string keyName in accResult.AllKeys)
							{
								userSettings[keyName] = accResult[keyName];
							}
						}
						else
						{
							// 4a. Get sub-account info
							accResult = registrar.GetSubAccount(context.ConsumerInfo.Username, 
								context.ConsumerInfo.Email);
							//
							foreach (string keyName in accResult.AllKeys)
								userSettings[keyName] = accResult[keyName];
						}
						// 5. Update user settings
						int apiResult = UserController.UpdateUserSettings(userSettings);
						// check API result
						if (apiResult < 0)
						{
							// BUILD ERROR
							result.Error = ERROR_UPDATE_USR_SETTINGS_MSG;
							result.Succeed = false;
							result.ResultCode = apiResult;
							// LOG ERROR
							TaskManager.WriteError(result.Error);
							TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
							// ROLLBACK
							RollbackOperation(domainSvc.DomainId);
							// EXIT
							return result;
						}
					}
					// copy registrar-specific data
					foreach (string[] pair in userSettings.SettingsArray)
					{
						// copy 2
						cmdParams[pair[0]] = pair[1];
					}
				}*/
				#endregion

				// load NS settings
				PackageSettings nsSettings = PackageController.GetPackageSettings(packageSvc.PackageId, PackageSettings.NAME_SERVERS);
				// build name servers array
				string[] nameServers = null;
				if (!String.IsNullOrEmpty(nsSettings[PackageSettings.NAME_SERVERS]))
					nameServers = nsSettings[PackageSettings.NAME_SERVERS].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

				// register or renew domain
				if (domainSvc.Status == ServiceStatus.Ordered)
					// try to register domain
					registrar.RegisterDomain(domainSvc, context.ConsumerInfo, nameServers);
				else
					// try to renew domain
					registrar.RenewDomain(domainSvc, context.ConsumerInfo, nameServers);

				// change svc status to active
					domainSvc.Status = ServiceStatus.Active;
				// update service info
					int updResult = UpdateServiceInfo(domainSvc);
				// check update result for errors
					if (updResult < 0)
					{
						// BUILD ERROR
						result.ResultCode = updResult;
						result.Succeed = false;
						result.Error = ERROR_SVC_UPDATE_MSG;
						// LOG ERROR
						TaskManager.WriteError(result.Error);
						TaskManager.WriteParameter(RESULT_CODE_PARAM, result.ResultCode);
						// ROLLBACK
						RollbackOperation(domainSvc.DomainId);
						// EXIT
						return result;
					}
				//
				result.Succeed = true;
                //
                SetOutboundParameters(context);
			}
			catch (Exception ex)
			{
				// LOG ERROR
				TaskManager.WriteError(ex);
				result.Succeed = false;
				// ROLLBACK
				RollbackOperation(result.ResultCode);
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public GenericSvcResult SuspendService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();
			//
			result.Succeed = true;
            //
            DomainNameSvc service = (DomainNameSvc)context.ServiceInfo;
            service.Status = ServiceStatus.Suspended;
            //
            UpdateServiceInfo(service);
            //
            SetOutboundParameters(context);
			//
			return result;
		}

		public GenericSvcResult CancelService(ProvisioningContext context)
		{
			GenericSvcResult result = new GenericSvcResult();
			//
			result.Succeed = true;
            //
            DomainNameSvc service = (DomainNameSvc)context.ServiceInfo;
            service.Status = ServiceStatus.Cancelled;
            //
            UpdateServiceInfo(service);
            //
            SetOutboundParameters(context);
			//
			return result;
		}

		public void LogServiceUsage(ProvisioningContext context)
		{
			// concretize svc to be provisioned
			DomainNameSvc domainSvc = (DomainNameSvc)context.ServiceInfo;
			//
			base.LogServiceUsage(context.ServiceInfo, domainSvc.SvcCycleId,
				domainSvc.BillingPeriod, domainSvc.PeriodLength);
		}

		#endregion

		private void RollbackOperation(int domainId)
		{
			if (domainId < 1)
				return;

			try
			{
				// restore
				DomainNameSvc domainSvc = (DomainNameSvc)RestoreObjectState(SERVICE_INFO);

				int apiResult = 0;

				switch (domainSvc.Status)
				{
					// Service 
					case ServiceStatus.Ordered:
						apiResult = ServerController.DeleteDomain(domainId);
						break;
				}

				// check API result
				if (apiResult < 0)
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_ROLLBACK_DOM_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, apiResult);
				}

				// rollback service changes in EC metabase
				apiResult = UpdateServiceInfo(domainSvc);
				// check API result
				if (apiResult < 0)
				{
					// LOG ERROR
					TaskManager.WriteError(ERROR_ROLLBACK_SVC_MSG);
					TaskManager.WriteParameter(RESULT_CODE_PARAM, apiResult);
					// EXIT
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

		/// <summary>
		/// Loads user's plugin-specific settings
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="pluginName"></param>
		/// <returns></returns>
		private UserSettings LoadUserSettings(int userId, string pluginName)
		{
			// build settings name
			string settingsName = pluginName + "Settings";
			// load user settings
			UserSettings settings = UserController.GetUserSettings(userId, settingsName);
			// set settings name
			settings.SettingsName = settingsName;
			//
			return settings;
		}

		private CommandParams PrepeareAccountParams(UserInfo userInfo)
		{
			CommandParams args = new CommandParams();

			args[CommandParams.USERNAME] = userInfo.Username;
			args[CommandParams.PASSWORD] = userInfo.Password;
			args[CommandParams.FIRST_NAME] = userInfo.FirstName;
			args[CommandParams.LAST_NAME] = userInfo.LastName;
			args[CommandParams.EMAIL] = userInfo.Email;
			args[CommandParams.ADDRESS] = userInfo.Address;
			args[CommandParams.CITY] = userInfo.City;
			args[CommandParams.STATE] = userInfo.State;
			args[CommandParams.COUNTRY] = userInfo.Country;
			args[CommandParams.ZIP] = userInfo.Zip;
			args[CommandParams.PHONE] = userInfo.PrimaryPhone;
			args[CommandParams.FAX] = userInfo.Fax;

			return args;
		}
	}
}