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
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

using Microsoft.Web.Services3;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	/// <summary>
	/// Summary description for ecStorehouse
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[Policy("ServerPolicy")]
	[ToolboxItem(false)]
	public class ecStorehouse : System.Web.Services.WebService
	{
		#region Ecommerce v 2.1.0
		[WebMethod]
		public bool CheckCustomerContractExists()
		{
			return ContractSystem.ContractController.CheckCustomerContractExists();
		}

        [WebMethod]
        public Contract GetCustomerContract(int customerId)
        {
            return ContractSystem.ContractController.GetCustomerContract(customerId);
        }

		[WebMethod]
		public int AddBillingCycle(int userId, string cycleName, string billingPeriod, int periodLength)
		{
			return StorehouseController.AddBillingCycle(userId, cycleName, billingPeriod, periodLength);
		}

		[WebMethod]
		public int UpdateBillingCycle(int userId, int cycleId, string cycleName, string billingPeriod, int periodLength)
		{
			return StorehouseController.UpdateBillingCycle(userId, cycleId, cycleName, billingPeriod, periodLength);
		}

		[WebMethod]
		public int DeleteBillingCycle(int userId, int cycleId)
		{
			return StorehouseController.DeleteBillingCycle(userId, cycleId);
		}

		[WebMethod]
		public BillingCycle GetBillingCycle(int userId, int cycleId)
		{
			return StorehouseController.GetBillingCycle(userId, cycleId);
		}

		[WebMethod]
		public int GetBillingCyclesCount(int userId)
		{
			return StorehouseController.GetBillingCyclesCount(userId);
		}

		[WebMethod]
		public List<BillingCycle> GetBillingCyclesPaged(int userId, int maximumRows, int startRowIndex)
		{
			return StorehouseController.GetBillingCyclesPaged(userId, maximumRows, startRowIndex);
		}

		[WebMethod]
		public List<BillingCycle> GetBillingCyclesFree(int userId, int[] cyclesTaken)
		{
			return StorehouseController.GetBillingCyclesFree(userId, cyclesTaken);
		}

		[WebMethod]
		public int AddHostingPlan(int userId, string planName, string productSku, bool taxInclusive, int planId,
			int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, HostingPlanCycle[] planCycles, 
			string[] planHighlights, int[] planCategories)
		{
			return StorehouseController.AddHostingPlan(userId, planName, productSku, taxInclusive, planId, 
				userRole, initialStatus, domainOption, enabled, planDescription, planCycles, 
				planHighlights, planCategories);
		}

		[WebMethod]
		public int AddTopLevelDomain(int userId, string topLevelDomain, string productSku, bool taxInclusive, int pluginId,
			bool enabled, bool whoisEnabled, DomainNameCycle[] domainCycles)
		{
			return StorehouseController.AddTopLevelDomain(userId, topLevelDomain, productSku, taxInclusive, pluginId,
				enabled, whoisEnabled, domainCycles);
		}

		[WebMethod]
		public int AddHostingAddon(int userId, string addonName, string productSku, bool taxInclusive, int planId, 
			bool recurring, bool dummyAddon, bool countable, bool enabled, string description, HostingPlanCycle[] addonCycles, int[] addonProducts)
		{
			return StorehouseController.AddHostingAddon(userId, addonName, productSku, taxInclusive, planId,
				recurring, dummyAddon, countable, enabled, description, addonCycles, addonProducts);
		}

		[WebMethod]
		public int UpdateHostingPlan(int userId, int productId, string planName, string productSku, bool taxInclusive, int planId,
			int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, HostingPlanCycle[] planCycles,
			string[] planHighlights, int[] planCategories)
		{
			return StorehouseController.UpdateHostingPlan(userId, productId, planName, productSku, taxInclusive, planId,
				userRole, initialStatus, domainOption, enabled, planDescription, planCycles, 
				planHighlights, planCategories);
		}

		[WebMethod]
		public int UpdateHostingAddon(int userId, int productId, string addonName, string productSku, bool taxInclusive, int planId,
			bool recurring, bool dummyAddon, bool countable, bool enabled, string description, HostingPlanCycle[] addonCycles, int[] addonProducts)
		{
			return StorehouseController.UpdateHostingAddon(userId, productId, addonName, productSku, taxInclusive, planId,
				recurring, dummyAddon, countable, enabled, description, addonCycles, addonProducts);
		}

		[WebMethod]
		public int UpdateTopLevelDomain(int userId, int productId, string topLevelDomain, string productSku, bool taxInclusive, int pluginId,
			bool enabled, bool whoisEnabled, DomainNameCycle[] domainCycles)
		{
			return StorehouseController.UpdateTopLevelDomain(userId, productId, topLevelDomain, 
				productSku, taxInclusive, pluginId, enabled, whoisEnabled, domainCycles);
		}

		[WebMethod]
		public int GetProductsCountByType(int userId, int typeId)
		{
			return StorehouseController.GetProductsCountByType(userId, typeId);
		}

		[WebMethod]
		public List<Product> GetProductsPagedByType(int userId, int typeId, int maximumRows, int startRowIndex)
		{
			return StorehouseController.GetProductsPagedByType(userId, typeId, maximumRows, startRowIndex);
		}

		[WebMethod]
		public List<int> GetHostingPlansTaken(int userId)
		{
			return StorehouseController.GetHostingPlansTaken(userId);
		}

		[WebMethod]
		public List<int> GetHostingAddonsTaken(int userId)
		{
			return StorehouseController.GetHostingAddonsTaken(userId);
		}

		[WebMethod]
		public HostingPlan GetHostingPlan(int userId, int productId)
		{
			return StorehouseController.GetHostingPlan(userId, productId, false);
		}

		[WebMethod]
		public HostingAddon GetHostingAddon(int userId, int productId)
		{
			return StorehouseController.GetHostingAddon(userId, productId);
		}

		[WebMethod]
		public List<HostingPlanCycle> GetHostingPlanCycles(int userId, int productId)
		{
			return StorehouseController.GetHostingPlanCycles(userId, productId);
		}

		[WebMethod]
		public List<HostingPlanCycle> GetHostingAddonCycles(int userId, int productId)
		{
			return StorehouseController.GetHostingAddonCycles(userId, productId);
		}

		[WebMethod]
		public List<string> GetProductHighlights(int resellerId, int productId)
		{
			return StorehouseController.GetProductHighlights(resellerId, productId);
		}

		[WebMethod]
		public List<Category> GetProductCategories(int userId, int productId)
		{
			return StorehouseController.GetProductCategories(userId, productId);
		}

		[WebMethod]
		public List<int> GetProductCategoriesIds(int userId, int productId)
		{
			return StorehouseController.GetProductCategoriesIds(userId, productId);
		}

		[WebMethod]
		public int DeleteProduct(int userId, int productId)
		{
			return StorehouseController.DeleteProduct(userId, productId);
		}

		[WebMethod]
		public List<Product> GetAddonProducts(int userId, int productId)
		{
			return StorehouseController.GetAddonProducts(userId, productId);
		}

		[WebMethod]
		public List<int> GetAddonProductsIds(int userId, int productId)
		{
			return StorehouseController.GetAddonProductsIds(userId, productId);
		}

		[WebMethod]
		public List<Product> GetProductsByType(int userId, int typeId)
		{
			return StorehouseController.GetProductsByType(userId, typeId);
		}

		[WebMethod]
		public List<TopLevelDomain> GetTopLevelDomainsPaged(int userId, int maximumRows, int startRowIndex)
		{
			return StorehouseController.GetTopLevelDomainsPaged(userId, maximumRows, startRowIndex);
		}

		[WebMethod]
		public TopLevelDomain GetTopLevelDomain(int userId, int productId)
		{
			return StorehouseController.GetTopLevelDomain(userId, productId);
		}

		[WebMethod]
		public List<DomainNameCycle> GetTopLevelDomainCycles(int userId, int productId)
		{
			return StorehouseController.GetTopLevelDomainCycles(userId, productId);
		}

		[WebMethod]
		public List<SupportedPlugin> GetSupportedPluginsByGroup(int userId, string groupName)
		{
			return SystemPluginController.GetSupportedPluginsByGroup(groupName);
		}

		[WebMethod]
		public PaymentMethod GetPaymentMethod(int userId, string methodName)
		{
			return StorehouseController.GetPaymentMethod(userId, methodName);
		}

		[WebMethod]
		public void SetPaymentProfile(string contractId, CheckoutDetails profile)
		{
			StorehouseController.SetPaymentProfile(contractId, profile);
		}

		[WebMethod]
		public CheckoutDetails GetPaymentProfile(string contractId)
		{
			return StorehouseController.GetPaymentProfile(contractId);
		}

		[WebMethod]
		public int DeletePaymentProfile(string contractId)
		{
			return StorehouseController.DeletePaymentProfile(contractId);
		}

		[WebMethod]
		public bool PaymentProfileExists(string contractId)
		{
			return StorehouseController.PaymentProfileExists(contractId);
		}

		[WebMethod]
		public KeyValueBunch GetPluginProperties(int userId, int pluginId)
		{
			return SystemPluginController.GetPluginProperties(userId, pluginId);
		}

		[WebMethod]
		public int SetPluginProperties(int userId, int pluginId, KeyValueBunch props)
		{
			return SystemPluginController.SetPluginProperties(userId, pluginId, props);
		}

		[WebMethod]
		public int SetPaymentMethod(int userId, string methodName, string displayName, int pluginId)
		{
			return StorehouseController.SetPaymentMethod(userId, methodName, displayName, pluginId);
		}

		[WebMethod]
		public int DeletePaymentMethod(int userId, string methodName)
		{
			return StorehouseController.DeletePaymentMethod(userId, methodName);
		}

		[WebMethod]
		public int AddTaxation(int userId, string country, string state, string description,
			int typeId, decimal amount, bool active)
		{
			return StorehouseController.AddTaxation(userId, country, state, description, typeId,
				amount, active);
		}

		[WebMethod]
		public int UpdateTaxation(int userId, int taxationId, string country, string state, string description,
			int typeId, decimal amount, bool active)
		{
			return StorehouseController.UpdateTaxation(userId, taxationId, country, state, description,
				typeId, amount, active);
		}

		[WebMethod]
		public int DeleteTaxation(int userId, int taxationId)
		{
			return StorehouseController.DeleteTaxation(userId, taxationId);
		}

		[WebMethod]
		public Taxation GetTaxation(int userId, int taxationId)
		{
			return StorehouseController.GetTaxation(userId, taxationId);
		}

		[WebMethod]
		public int GetTaxationsCount(int userId)
		{
			return StorehouseController.GetTaxationsCount(userId);
		}

		[WebMethod]
		public List<Taxation> GetTaxationsPaged(int userId, int maximumRows, int startRowIndex)
		{
			return StorehouseController.GetTaxationsPaged(userId, maximumRows, startRowIndex);
		}

		[WebMethod]
		public StoreSettings GetStoreSettings(int userId, string settingsName)
		{
			return StorehouseController.GetStoreSettings(userId, settingsName);
		}

        [WebMethod]
        public int SetStoreSettings(int userId, string settingsName, StoreSettings settings)
        {
            return StorehouseController.SetStoreSettings(userId, settingsName, settings);
        }

		[WebMethod]
		public RoutineResult SetInvoiceNotification(int userId, string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return StorehouseController.SetInvoiceNotification(userId, fromEmail, ccEmail, subject, htmlBody, textBody);
		}

		[WebMethod]
		public RoutineResult SetPaymentReceivedNotification(int userId, string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return StorehouseController.SetPaymentReceivedNotification(userId, fromEmail, ccEmail, subject, htmlBody, textBody);
		}

		[WebMethod]
		public RoutineResult SetServiceActivatedNotification(int userId, string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return StorehouseController.SetServiceActivatedNotification(userId, fromEmail, ccEmail, subject, htmlBody, textBody);
		}

		[WebMethod]
		public RoutineResult SetServiceCancelledNotification(int userId, string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return StorehouseController.SetServiceCancelledNotification(userId, fromEmail, ccEmail, subject, htmlBody, textBody);
		}

		[WebMethod]
		public RoutineResult SetServiceSuspendedNotification(int userId, string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return StorehouseController.SetServiceSuspendedNotification(userId, fromEmail, ccEmail, subject, htmlBody, textBody);
		}

		[WebMethod]
		public bool IsSupportedPluginActive(int resellerId, int pluginId)
		{
			return StorehouseController.IsSupportedPluginActive(resellerId, pluginId);
		}

		[WebMethod]
		public bool IsInvoiceProcessed(int invoiceId)
		{
			return StorehouseController.IsInvoiceProcessed(invoiceId);
		}

		[WebMethod]
		public int GetCustomersPaymentsCount(int userId, bool isReseller)
		{
			return StorehouseController.GetCustomersPaymentsCount(userId, isReseller);
		}

		[WebMethod]
		public List<CustomerPayment> GetCustomersPaymentsPaged(int userId, bool isReseller, int maximumRows, int startRowIndex)
		{
			return StorehouseController.GetCustomersPaymentsPaged(userId, isReseller, 
				maximumRows, startRowIndex);
		}

		[WebMethod]
		public int DeleteCustomerPayment(int paymentId)
		{
			return StorehouseController.DeleteCustomerPayment(paymentId);
		}

		[WebMethod]
		public int ChangeCustomerPaymentStatus(int paymentId, TransactionStatus status)
		{
			return StorehouseController.ChangeCustomerPaymentStatus(paymentId, status);
		}

		[WebMethod]
		public int GetCustomersInvoicesCount(int userId, bool isReseller)
		{
			return StorehouseController.GetCustomersInvoicesCount(userId, isReseller);
		}

		[WebMethod]
		public List<Invoice> GetCustomersInvoicesPaged(int userId, bool isReseller, int maximumRows, int startRowIndex)
		{
			return StorehouseController.GetCustomersInvoicesPaged(userId, isReseller,
				maximumRows, startRowIndex);
		}

		[WebMethod]
		public string GetCustomerInvoiceFormatted(int invoiceId, string cultureName)
		{
            return InvoiceController.GetCustomerInvoiceFormattedInternally(invoiceId, cultureName);
		}

		[WebMethod]
		public Invoice GetCustomerInvoice(int invoiceId)
		{
			return InvoiceController.GetCustomerInvoiceInternally(invoiceId);
		}

		[WebMethod]
		public int AddCustomerPayment(string contractId, int invoiceId, string transactionId, decimal amount, 
			string currency, string methodName, TransactionStatus status)
		{
			return StorehouseController.AddCustomerPayment(contractId, invoiceId, transactionId, amount, 
				currency, methodName, status);
		}

		[WebMethod]
		public int GetCustomersServicesCount(int userId, bool isReseller)
		{
			return ServiceController.GetCustomersServicesCount(userId, isReseller);
		}

		[WebMethod]
		public List<Service> GetCustomersServicesPaged(int userId, bool isReseller, int maximumRows, 
			int startRowIndex)
		{
			return ServiceController.GetCustomersServicesPaged(userId, isReseller,
				maximumRows, startRowIndex);
		}

		[WebMethod]
		public HostingPackageSvc GetHostingPackageService(int serviceId)
		{
			return (HostingPackageSvc)ServiceController.GetService(serviceId);
		}

		[WebMethod]
		public HostingAddonSvc GetHostingAddonService(int serviceId)
		{
			return (HostingAddonSvc)ServiceController.GetService(serviceId);
		}

		[WebMethod]
		public DomainNameSvc GetDomainNameService(int serviceId)
		{
			return (DomainNameSvc)ServiceController.GetService(serviceId);
		}

		[WebMethod]
		public Service GetRawCustomerService(int serviceId)
		{
			return ServiceController.GetRawCustomerService(serviceId);
		}

		[WebMethod]
		public ServiceHistoryRecord[] GetServiceHistory(int serviceId)
		{
			return ServiceController.GetServiceHistory(serviceId);
		}

		[WebMethod]
		public GenericSvcResult ActivateService(int serviceId, bool sendMail)
		{
			return ServiceController.ActivateService(serviceId, sendMail);
		}

		[WebMethod]
		public GenericSvcResult SuspendService(int serviceId, bool sendMail)
		{
			return ServiceController.SuspendService(serviceId, sendMail);
		}

		[WebMethod]
		public GenericSvcResult CancelService(int serviceId, bool sendMail)
		{
			return ServiceController.CancelService(serviceId, sendMail);
		}

		[WebMethod]
		public void VoidCustomerInvoice(int invoiceId)
		{
			InvoiceController.VoidCustomerInvoice(invoiceId);
		}

		[WebMethod]
		public List<GenericSvcResult> ActivateInvoice(int invoiceId)
		{
			return ServiceController.ActivateInvoice(invoiceId, true);
		}

		[WebMethod]
		public int DeleteCustomerService(int serviceId)
		{
			return ServiceController.DeleteCustomerService(serviceId);
		}

		#endregion

		#region Category routines

		[WebMethod]
		public int AddCategory(int userId, string categoryName, string categorySku, int parentId, string shortDescription, string fullDescription)
		{
			return CategoryController.AddCategory(
				userId,
				categoryName,
				categorySku,
				parentId,
				shortDescription,
				fullDescription
			);
		}

		[WebMethod]
		public int UpdateCategory(int userId, int categoryId, string categoryName, string categorySku, int parentId, string shortDescription, string fullDescription)
		{
			return CategoryController.UpdateCategory(
				userId,
				categoryId,
				categoryName,
				categorySku,
				parentId,
				shortDescription,
				fullDescription
			);
		}

		[WebMethod]
		public int DeleteCategory(int userId, int categoryId)
		{
			return CategoryController.DeleteCategory(userId, categoryId);
		}

		[WebMethod]
		public Category GetCategory(int userId, int categoryId)
		{
			return CategoryController.GetCategory(userId, categoryId);
		}

		[WebMethod]
		public List<Category> GetCategoriesPaged(int userId, int parentId, int maximumRows, int startRowIndex)
		{
			return CategoryController.GetCategoriesPaged(userId, parentId, maximumRows, startRowIndex);
		}

		[WebMethod]
		public int GetCategoriesCount(int userId, int parentId)
		{
			return CategoryController.GetCategoriesCount(userId, parentId);
		}

		[WebMethod]
		public DataSet GetWholeCategoriesSet(int userId)
		{
			return CategoryController.GetWholeCategoriesSet(userId);
		}

		#endregion
	}
}
