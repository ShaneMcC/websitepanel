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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class Memento
	{
		private string mStateObjectType;
		private Hashtable mementoState;

		public Memento()
		{
			//
			mementoState = new Hashtable();
		}

		/// <summary>
		/// Saves object public properties state using reflection. Does not support indexed properties.
		/// </summary>
		/// <param name="objectKey"></param>
		/// <param name="value"></param>
		public void SaveObjectState(object value)
		{
			//
			Type valueType = value.GetType();
			//
			PropertyInfo[] properties = valueType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			//
			if (properties != null && properties.Length > 0)
			{
				//
				Hashtable stateHash = new Hashtable();
				//
				foreach (PropertyInfo property in properties)
				{
					// copy property value
					if (property.GetIndexParameters() == null || property.GetIndexParameters().Length == 0)
						mementoState.Add(property.Name, property.GetValue(value, null));
				}
				// save object full-qualified name
				mStateObjectType = valueType.AssemblyQualifiedName;
			}
		}

		public object RestoreObjectState()
		{
			// create object instance
			object keyObject = Activator.CreateInstance(Type.GetType(mStateObjectType));
			//
			Type objectType = keyObject.GetType();
			//
			PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			//
			if (properties != null && properties.Length > 0)
			{
				// load object state back
				foreach (PropertyInfo property in properties)
				{
					// restore property value back
					if (property.GetIndexParameters() == null || property.GetIndexParameters().Length == 0)
						property.SetValue(keyObject, mementoState[property.Name], null);
				}
			}
			//
			return keyObject;
		}
	}

	public abstract class ServiceProvisioningBase
	{
		// Contants
		public const string SERVICE_INFO = "ServiceInfo";
		public const string CONSUMER_INFO = "ConsumerInfo";

		#region Error Messages

		public const string ERROR_SVC_UPDATE_MSG = "Could not update service data";
		public const string PARENT_SVC_NOT_FOUND_MSG = "Could not find parent service assigned";
		public const string ERROR_ROLLBACK_SVC_MSG = "Could not rollback service changes";
		public const string ERROR_CLIENT_OPERATION_PERMISSIONS = "Account does not have enough permissions to do this operation";
		public const string ERROR_CLIENT_OPERATION_STATUS = "Account is demo or suspended and not allowed to do this operation";

		#endregion

		#region Trace Messages

		public const string ROLLBACK_SUCCEED_MSG = "Rollback succeed";

		#endregion

		#region Trace Parameters

		public const string CONTRACT_PARAM = "ContractID";
		public const string USERNAME_PARAM = "Username";
		public const string SVC_PARAM = "Service";
		public const string SVC_ID_PARAM = "ServiceID";
		public const string RESULT_CODE_PARAM = "ResultCode";
		public const string PCKG_PARAM = "Package";
		public const string PCKG_ID_PARAM = "PackageID";

		#endregion

		public bool CheckOperationClientPermissions(GenericSvcResult result)
		{
			// 1. Do security checks
			SecurityResult secResult = StorehouseController.CheckAccountIsAdminOrReseller();
			// ERROR
			if (!secResult.Success)
			{
				result.Succeed = false;
				result.ResultCode = secResult.ResultCode;
				//
				return false;
			}
			//
			return true;
		}

		public bool CheckOperationClientStatus(GenericSvcResult result)
		{
			// 2. Check account status
			SecurityResult secResult = StorehouseController.CheckAccountNotDemoAndActive();
			// ERROR
			if (!secResult.Success)
			{
				result.Succeed = false;
				result.ResultCode = secResult.ResultCode;
				//
				return false;
			}
			//
			return true;
		}

		public ProvisioningContext GetProvisioningContext(int serviceId, bool sendEmail)
		{
			IServiceProvisioning controller = (IServiceProvisioning)this;
			Service serviceInfo = controller.GetServiceInfo(serviceId);
			Contract contractInfo = ContractSystem.ContractController.GetContract(serviceInfo.ContractId);
            ContractAccount consumerInfo = ContractSystem.ContractController.GetContractAccountSettings(
				serviceInfo.ContractId, true);
			// load parent svc
			Service parentSvcInfo = (serviceInfo.ParentId == 0) ? null : 
				ServiceController.GetService(serviceInfo.ParentId);
			// return prepeared context
            ProvisioningContext ctx = new ProvisioningContext(contractInfo, serviceInfo, consumerInfo, parentSvcInfo);
			//
			ctx.SendEmail = sendEmail;
			//
			return ctx;
		}

		//
		private Dictionary<string, Memento> undoSteps = new Dictionary<string, Memento>();

		protected void SaveObjectState(string objectKey, object value)
		{
			//
			Memento memento = new Memento();
			//
			memento.SaveObjectState(value);
			//
			undoSteps.Add(objectKey, memento);
		}

		protected object RestoreObjectState(string objectKey)
		{
			//
			if (!undoSteps.ContainsKey(objectKey))
				return null;
			//
			Memento memento = undoSteps[objectKey];
			//
			return memento.RestoreObjectState();
		}

		protected void LogServiceUsage(Service service, int svcCycleId, string billingPeriod, int periodLength)
		{
			// define start date
			DateTime startDate = ServiceController.GetServiceSuspendDate(service.ServiceId);
			//
			DateTime endDate = startDate;
			//
			switch (billingPeriod)
			{
				case "day":
					endDate = startDate.AddDays(periodLength);
					break;
				case "month":
					endDate = endDate.AddMonths(periodLength);
					break;
				case "year":
					endDate = endDate.AddYears(periodLength);
					break;
			}
			// add service usage record
			EcommerceProvider.AddServiceUsageRecord(SecurityContext.User.UserId, service.ServiceId, 
				svcCycleId, startDate, endDate);
		}

        protected void SetOutboundParameters(ProvisioningContext context)
        {
            // set task outbound parameters
            TaskManager.TaskParameters[SystemTaskParams.PARAM_SERVICE] = context.ServiceInfo;
            TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT] = context.ContractInfo;
            TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT_ACCOUNT] = context.ConsumerInfo;
			TaskManager.TaskParameters[SystemTaskParams.PARAM_SEND_EMAIL] = context.SendEmail;
        }
	}
	
	/*public abstract class ProvisioningController
	{
		// user info (context)
		// product general info
		// cart prov settings
		// product prov settings
		// product type prov settings
		// 
		#region Private vars

		private string notificationTemplate;
		private bool notificationLoaded;

		private UserInfo userInfo;
		private Service serviceInfo;
		private KeyValueBunch serviceSettings;

		#endregion

		#region Public properties

		public string NotificationTemplate
		{
			get
			{
				if (!notificationLoaded)

				return notificationTemplate;
			}
		}

		public UserInfo UserInfo
		{
			get { return userInfo; }
		}

		public Service ServiceInfo
		{
			get { return serviceInfo; }
		}

		public KeyValueBunch ServiceSettings
		{
			get { return serviceSettings; }
		}

		#endregion

		#region Abstract routines

		//
		public abstract GenericSvcResult ActivateService(Service service);
		//
		public abstract GenericSvcResult SuspendService();
		//
		public abstract GenericSvcResult CancelService();
		//
		public abstract void RollbackOperation();

		#endregion

		protected ProvisioningController(Service serviceInfo)
		{
			this.serviceInfo = serviceInfo;

			Initialize();
		}

		protected virtual void Initialize()
		{
			// get user profile
			userInfo = UserController.GetUser(serviceInfo.UserId);
		}

		protected DateTime CalculateNextSuspendDate(DateTime startDate, string cyclePeriod, int cycleLength)
		{
			// calculate next suspend date
			switch (cyclePeriod)
			{
				case "day":
					return startDate.AddDays(cycleLength);

				case "month":
					return startDate.AddMonths(cycleLength);

				case "year":
					return startDate.AddYears(cycleLength);
			}
			//
			return startDate;
		}

		public virtual void LoadProvisioningSettings()
		{
			throw new NotImplementedException();
			// get service settings
			/*serviceSettings = ServiceController.GetServiceSettings(
				serviceInfo.SpaceId,
				serviceInfo.ServiceId
			);*/
		/*}

		public virtual void SaveProvisioningSettings()
		{
			if (ServiceSettings.HasPendingChanges)
			{
				/*int result = ServiceController.SetServiceSettings(
					serviceInfo.SpaceId,
					serviceInfo.ServiceId,
					serviceSettings
				);

				if (result < 0)
					throw new Exception("Unable to save provisioning settings. Status code: " + result);*/
			/*}
		}
	}*/
}