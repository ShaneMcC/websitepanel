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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	[Serializable]
	public class Service
	{
		public const string SERVICE_ITEM_TYPE = "ECSERVICE";
		public const int SERVICE_ITEM_SEVERITY = 2;

		private int serviceId;
        private int customerId;
        private string username;
        private int resellerId;
		private string contractId;
		private int parentId;
		private string serviceName;
		private int typeId;
		private ServiceStatus status;
		private DateTime created;
		private DateTime modified;

		public int ServiceId
		{
			get { return this.serviceId; }
			set { this.serviceId = value; }
		}

        public int CustomerId
        {
            get { return this.customerId; }
            set { this.customerId = value; }
        }

        public int ResellerId
        {
            get { return this.resellerId; }
            set { this.resellerId = value; }
        }

		public string ContractId
		{
			get { return this.contractId; }
			set { this.contractId = value; }
		}

        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

		public int ParentId
		{
			get { return this.parentId; }
			set { this.parentId = value; }
		}

		public string ServiceName
		{
			get { return this.serviceName; }
			set { this.serviceName = value; }
		}

		public int TypeId
		{
			get { return this.typeId; }
			set { this.typeId = value; }
		}

		public ServiceStatus Status
		{
			get { return this.status; }
			set { this.status = value; }
		}

		public System.DateTime Created
		{
			get { return this.created; }
			set { this.created = value; }
		}

		public System.DateTime Modified
		{
			get { return this.modified; }
			set { this.modified = value; }
		}
	}

	[Serializable]
	public class HostingPackageSvc : Service
	{
		#region Fields

		private int productId;
		private int planId;
		private int packageId;
		private UserRole userRole;
		private PackageStatus initialStatus;
		private int svcCycleId;
		private string cycleName;
		private string billingPeriod;
		private int periodLength;
		private decimal setupFee;
		private decimal recurringFee;
		private string currency;

		#endregion

		#region Properties

		public int ProductId
		{
			get { return this.productId; }
			set { this.productId = value; }
		}

		public int PlanId
		{
			get { return this.planId; }
			set { this.planId = value; }
		}

		public int PackageId
		{
			get { return this.packageId; }
			set { this.packageId = value; }
		}

		public WebsitePanel.EnterpriseServer.UserRole UserRole
		{
			get { return this.userRole; }
			set { this.userRole = value; }
		}

		public WebsitePanel.EnterpriseServer.PackageStatus InitialStatus
		{
			get { return this.initialStatus; }
			set { this.initialStatus = value; }
		}

		public int SvcCycleId
		{
			get { return svcCycleId; }
			set { svcCycleId = value; }
		}

		public string CycleName
		{
			get { return this.cycleName; }
			set { this.cycleName = value; }
		}

		public string BillingPeriod
		{
			get { return this.billingPeriod; }
			set { this.billingPeriod = value; }
		}

		public int PeriodLength
		{
			get { return this.periodLength; }
			set { this.periodLength = value; }
		}

		public decimal SetupFee
		{
			get { return this.setupFee; }
			set { this.setupFee = value; }
		}

		public decimal RecurringFee
		{
			get { return this.recurringFee; }
			set { this.recurringFee = value; }
		}

		public string Currency
		{
			get { return this.currency; }
			set { this.currency = value; }
		}

		#endregion
	}

	[Serializable]
	public class HostingAddonSvc : Service
	{
		#region Fields

		private int productId;
		private int planId;
		private int packageAddonId;
		private int quantity;
		private bool recurring;
		private bool dummyAddon;
		private int svcCycleId;
		private string cycleName;
		private string billingPeriod;
		private int periodLength;
		private decimal setupFee;
		private decimal cyclePrice;
		private string currency;

		#endregion

		#region Properties

		public int ProductId
		{
			get { return this.productId; }
			set { this.productId = value; }
		}

		public int PlanId
		{
			get { return this.planId; }
			set { this.planId = value; }
		}

		public int PackageAddonId
		{
			get { return this.packageAddonId; }
			set { this.packageAddonId = value; }
		}

		public int Quantity
		{
			get { return this.quantity; }
			set { this.quantity = value; }
		}

		public bool Recurring
		{
			get { return this.recurring; }
			set { this.recurring = value; }
		}

		public bool DummyAddon
		{
			get { return this.dummyAddon; }
			set { this.dummyAddon = value; }
		}

		public int SvcCycleId
		{
			get { return this.svcCycleId; }
			set { this.svcCycleId = value; }
		}

		public string CycleName
		{
			get { return this.cycleName; }
			set { this.cycleName = value; }
		}

		public string BillingPeriod
		{
			get { return this.billingPeriod; }
			set { this.billingPeriod = value; }
		}

		public int PeriodLength
		{
			get { return this.periodLength; }
			set { this.periodLength = value; }
		}

		public decimal SetupFee
		{
			get { return this.setupFee; }
			set { this.setupFee = value; }
		}

		public decimal CyclePrice
		{
			get { return this.cyclePrice; }
			set { this.cyclePrice = value; }
		}

		public string Currency
		{
			get { return this.currency; }
			set { this.currency = value; }
		}

		#endregion
	}

	[Serializable]
	public class DomainNameSvc : Service, IKeyValueBunch
	{
		public const string SPF_UPDATE_NS_ACTION = "UPDATE_NS";
		public const string SPF_TRANSFER_ACTION = "TRANSFER";
		public const string SPF_REGISTER_ACTION = "REGISTER";

		#region Fields

		private int productId;
		private int domainId;
		private string providerName;
		private int pluginId;
		private string fqdn;
		private int svcCycleId;
		private string cycleName;
		private string billingPeriod;
		private int periodLength;
		private decimal setupFee;
		private decimal recurringFee;
		private string currency;
		private string propertyNames;
		private string propertyValues;

		#endregion

		#region Properties

		public int ProductId
		{
			get { return this.productId; }
			set { this.productId = value; }
		}

		public int DomainId
		{
			get { return this.domainId; }
			set { this.domainId = value; }
		}

		public string ProviderName
		{
			get { return this.providerName; }
			set { this.providerName = value; }
		}

		public int PluginId
		{
			get { return this.pluginId; }
			set { this.pluginId = value; }
		}

		public string Fqdn
		{
			get { return this.fqdn; }
			set { this.fqdn = value; }
		}

		public int SvcCycleId
		{
			get { return this.svcCycleId; }
			set { this.svcCycleId = value; }
		}

		public string CycleName
		{
			get { return this.cycleName; }
			set { this.cycleName = value; }
		}

		public string BillingPeriod
		{
			get { return this.billingPeriod; }
			set { this.billingPeriod = value; }
		}

		public int PeriodLength
		{
			get { return this.periodLength; }
			set { this.periodLength = value; }
		}

		public decimal SetupFee
		{
			get { return this.setupFee; }
			set { this.setupFee = value; }
		}

		public decimal RecurringFee
		{
			get { return this.recurringFee; }
			set { this.recurringFee = value; }
		}

		public string Currency
		{
			get { return this.currency; }
			set { this.currency = value; }
		}

		[XmlIgnore]
		public string PropertyNames
		{
			get { return this.propertyNames; }
			set { this.propertyNames = value; }
		}

		[XmlIgnore]
		public string PropertyValues
		{
			get { return this.propertyValues; }
			set { this.propertyValues = value; }
		}

		#endregion

		private NameValueCollection properties;

		public string[][] KeyValueArray;

		[XmlIgnore]
		NameValueCollection Properties
		{
			get
			{
				//
				SyncCollectionsState(true);
				//
				return properties;
			}
		}

		public string this[string settingName]
		{
			get
			{
				return Properties[settingName];
			}
			set
			{
				// set setting
				Properties[settingName] = value;
				//
				SyncCollectionsState(false);
			}
		}

		private void SyncCollectionsState(bool inputSync)
		{
			if (inputSync)
			{
				if (properties == null)
				{
					// create new dictionary
					properties = new NameValueCollection();

					// fill dictionary
					if (KeyValueArray != null)
					{
						foreach (string[] pair in KeyValueArray)
							properties.Add(pair[0], pair[1]);
					}
				}
			}
			else
			{
				// rebuild array
				KeyValueArray = new string[Properties.Count][];
				//
				for (int i = 0; i < Properties.Count; i++)
				{
					KeyValueArray[i] = new string[] { Properties.Keys[i], Properties[Properties.Keys[i]] };
				}
			}
		}

		public string[] GetAllKeys()
		{
			if (Properties != null)
				return Properties.AllKeys;

			return null;
		}
	}

	[Serializable]
	public class ServiceHistoryRecord
	{
		private string cycleName;
		private string billingPeriod;
		private int periodLength;
		private decimal setupFee;
		private decimal recurringFee;
		private string currency;
		private DateTime startDate;
		private DateTime endDate;

		public string CycleName
		{
			get { return cycleName; }
			set { cycleName = value; }
		}

		public string BillingPeriod
		{
			get { return billingPeriod; }
			set { billingPeriod = value; }
		}

		public int PeriodLength
		{
			get { return periodLength; }
			set { periodLength = value; }
		}

		public decimal SetupFee
		{
			get { return setupFee; }
			set { setupFee = value; }
		}

		public decimal RecurringFee
		{
			get { return recurringFee; }
			set { recurringFee = value; }
		}

		public string Currency
		{
			get { return currency; }
			set { currency = value; }
		}

		public DateTime StartDate
		{
			get { return startDate; }
			set { startDate = value; }
		}

		public DateTime EndDate
		{
			get { return endDate; }
			set { endDate = value; }
		}
	}

    public enum ServiceStatus
    {
        Ordered = 0,
        Active = 1,
		Suspended = 2,
		Cancelled = 3
    };
}
