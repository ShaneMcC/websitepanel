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
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;
using WebsitePanel.Portal;

using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public class StorehouseHelper
	{
		public static void VoidCustomerInvoice(int invoiceId)
		{
			EC.Services.Storehouse.VoidCustomerInvoice(invoiceId);
		}

        public static Contract GetCustomerContract(int customerId)
        {
            return EC.Services.Storehouse.GetCustomerContract(customerId);
        }

		public static bool CheckCustomerContractExists()
		{
			return EC.Services.Storehouse.CheckCustomerContractExists();
		}

		public static bool PaymentProfileExists(string contractId)
		{
            return EC.Services.Storehouse.PaymentProfileExists(contractId);
		}

		public static CheckoutDetails GetPaymentProfile(string contractId)
		{
			return EC.Services.Storehouse.GetPaymentProfile(contractId);
		}

		public static void SetPaymentProfile(string contractId, CheckoutDetails profile)
		{
			EC.Services.Storehouse.SetPaymentProfile(contractId, profile);
		}

		public static int DeletePaymentProfile(string contractId)
		{
            return EC.Services.Storehouse.DeletePaymentProfile(contractId);
		}

		public static int AddBillingCycle(string cycleName, string billingPeriod, int periodLength)
		{
			return EC.Services.Storehouse.AddBillingCycle(
				PanelSecurity.SelectedUserId,
				cycleName,
				billingPeriod,
				periodLength
			);
		}

		public static int UpdateBillingCycle(int cycleId, string cycleName, string billingPeriod, int periodLength)
		{
			return EC.Services.Storehouse.UpdateBillingCycle(
				PanelSecurity.SelectedUserId,
				cycleId,
				cycleName,
				billingPeriod,
				periodLength
			);
		}

		public static int DeleteBillingCycle(int cycleId)
		{
			return EC.Services.Storehouse.DeleteBillingCycle(PanelSecurity.SelectedUserId, cycleId);
		}

		public static BillingCycle GetBillingCycle(int cycleId)
		{
			return EC.Services.Storehouse.GetBillingCycle(PanelSecurity.SelectedUserId, cycleId);
		}

		public static int GetBillingCyclesCount()
		{
			return EC.Services.Storehouse.GetBillingCyclesCount(PanelSecurity.SelectedUserId);
		}

		public static BillingCycle[] GetBillingCyclesPaged(int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetBillingCyclesPaged(
				PanelSecurity.SelectedUserId,
				maximumRows,
				startRowIndex
			);
		}

		public static BillingCycle[] GetBillingCyclesFree(int[] cyclesTaken)
		{
			return EC.Services.Storehouse.GetBillingCyclesFree(PanelSecurity.SelectedUserId, cyclesTaken);
		}

        public static int AddHostingPlan(string planName, string productSku, bool taxInclusive, int planId, 
            int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, 
            HostingPlanCycle[] planCycles, string[] planHighlights, int[] planCategories)
		{
			return EC.Services.Storehouse.AddHostingPlan(
				PanelSecurity.SelectedUserId,
				planName,
				productSku,
                taxInclusive,
				planId,
				userRole,
				initialStatus,
				domainOption,
				enabled,
				planDescription,
				planCycles,
				planHighlights,
				planCategories
			);
		}

        public static int AddTopLevelDomain(string topLevelDomain, string productSku, bool taxInclusive, int pluginId, 
			bool enabled, bool whoisEnabled, DomainNameCycle[] domainCycles)
		{
			return EC.Services.Storehouse.AddTopLevelDomain(PanelSecurity.SelectedUserId,
				topLevelDomain, productSku, taxInclusive, pluginId, enabled, whoisEnabled, domainCycles);
		}

        public static int AddHostingAddon(string addonName, string productSku, bool taxInclusive, int planId, bool recurring, 
			bool dummyAddon, bool countable, bool enabled, string description, HostingPlanCycle[] addonCycles, int[] addonProducts)
		{
			return EC.Services.Storehouse.AddHostingAddon(
				PanelSecurity.SelectedUserId,
				addonName,
				productSku,
                taxInclusive,
				planId,
				recurring,
				dummyAddon,
				countable,
				enabled,
				description,
				addonCycles,
				addonProducts
			);
		}

		public static int GetHostingPlansCount()
		{
			return EC.Services.Storehouse.GetProductsCountByType(
				PanelSecurity.SelectedUserId,
				Product.HOSTING_PLAN
			);
		}

		public static Product[] GetHostingPlansPaged(int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetProductsPagedByType(
				PanelSecurity.SelectedUserId,
				Product.HOSTING_PLAN,
				maximumRows,
				startRowIndex
			);
		}

		public static int GetTopLevelDomainsCount()
		{
			return EC.Services.Storehouse.GetProductsCountByType(
				PanelSecurity.SelectedUserId,
				Product.TOP_LEVEL_DOMAIN
			);
		}

		public static TopLevelDomain[] GetTopLevelDomainsPaged(int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetTopLevelDomainsPaged(
				PanelSecurity.SelectedUserId,
				maximumRows,
				startRowIndex
			);
		}

		public static TopLevelDomain GetTopLevelDomain(int productId)
		{
			return EC.Services.Storehouse.GetTopLevelDomain(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static HostingAddon GetHostingAddon(int productId)
		{
			return EC.Services.Storehouse.GetHostingAddon(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static DomainNameCycle[] GetTopLevelDomainCycles(int productId)
		{
			return EC.Services.Storehouse.GetTopLevelDomainCycles(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static int[] GetHostingPlansTaken()
		{
			return EC.Services.Storehouse.GetHostingPlansTaken(
				PanelSecurity.SelectedUserId
			);
		}

		public static int[] GetHostingAddonsTaken()
		{
			return EC.Services.Storehouse.GetHostingAddonsTaken(
				PanelSecurity.SelectedUserId
			);
		}

		public static HostingPlan GetHostingPlan(int productId)
		{
			return EC.Services.Storehouse.GetHostingPlan(PanelSecurity.SelectedUserId, productId);
		}

		public static Category[] GetProductCategories(int productId)
		{
			return EC.Services.Storehouse.GetProductCategories(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static int[] GetProductCategoriesIds(int productId)
		{
			return EC.Services.Storehouse.GetProductCategoriesIds(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static string[] GetProductHighlights(int productId)
		{
			return EC.Services.Storehouse.GetProductHighlights(PanelSecurity.SelectedUserId, productId);
		}

		public static HostingPlanCycle[] GetHostingPlanCycles(int productId)
		{
			return EC.Services.Storehouse.GetHostingPlanCycles(PanelSecurity.SelectedUserId, productId);
		}

		public static HostingPlanCycle[] GetHostingAddonCycles(int productId)
		{
			return EC.Services.Storehouse.GetHostingAddonCycles(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

        public static int UpdateHostingPlan(int productId, string planName, string productSku, bool taxInclusive,
			int planId, int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, 
			HostingPlanCycle[] planCycles, string[] planHighlights, int[] planCategories)
		{
			return EC.Services.Storehouse.UpdateHostingPlan(
				PanelSecurity.SelectedUserId,
				productId,
				planName,
				productSku,
                taxInclusive,
				planId,
				userRole,
				initialStatus,
				domainOption,
				enabled,
				planDescription,
				planCycles,
				planHighlights,
				planCategories
			);
		}

        public static int UpdateHostingAddon(int productId, string addonName, string productSku, bool taxInclusive, int planId, bool recurring,
			bool dummyAddon, bool countable, bool enabled, string description, HostingPlanCycle[] addonCycles, int[] addonProducts)
		{
			return EC.Services.Storehouse.UpdateHostingAddon(PanelSecurity.SelectedUserId, productId, addonName, productSku,
				taxInclusive, planId, recurring, dummyAddon, countable, enabled, description, addonCycles, addonProducts);
		}

		public static int UpdateTopLevelDomain(int productId, string topLevelDomain, string productSku, 
			bool taxInclusive, int pluginId, bool enabled, bool whoisEnabled, DomainNameCycle[] domainCycles)
		{
			return EC.Services.Storehouse.UpdateTopLevelDomain(PanelSecurity.SelectedUserId,
				productId, topLevelDomain, productSku, taxInclusive, pluginId, enabled, whoisEnabled, domainCycles);
		}

		public static int DeleteProduct(int productId)
		{
			return EC.Services.Storehouse.DeleteProduct(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static Product[] GetProductsByType(int typeId)
		{
			return EC.Services.Storehouse.GetProductsByType(
				PanelSecurity.SelectedUserId,
				typeId
			);
		}

		public static Product[] GetAddonProducts(int productId)
		{
			return EC.Services.Storehouse.GetAddonProducts(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static int[] GetAddonProductsIds(int productId)
		{
			return EC.Services.Storehouse.GetAddonProductsIds(
				PanelSecurity.SelectedUserId,
				productId
			);
		}

		public static int GetHostingAddonsCount()
		{
			return EC.Services.Storehouse.GetProductsCountByType(
				PanelSecurity.SelectedUserId,
				Product.HOSTING_ADDON
			);
		}

		public static Product[] GetHostingAddonsPaged(int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetProductsPagedByType(
				PanelSecurity.SelectedUserId,
				Product.HOSTING_ADDON,
				maximumRows,
				startRowIndex
			);
		}

		public static SupportedPlugin[] GetSupportedPluginsByGroup(string groupName)
		{
			return EC.Services.Storehouse.GetSupportedPluginsByGroup(
				PanelSecurity.SelectedUserId, groupName);
		}

		public static PaymentMethod GetPaymentMethod(string methodName)
		{
			return EC.Services.Storehouse.GetPaymentMethod(
				PanelSecurity.SelectedUserId, methodName);
		}

		public static KeyValueBunch GetPluginProperties(int pluginId)
		{
			return EC.Services.Storehouse.GetPluginProperties(
				PanelSecurity.SelectedUserId, pluginId);
		}

		public static int SetPluginProperties(int pluginId, KeyValueBunch props)
		{
			return EC.Services.Storehouse.SetPluginProperties(
				PanelSecurity.SelectedUserId, pluginId, props);
		}

		public static int SetPaymentMethod(string methodName, string displayName, int pluginId)
		{
			return EC.Services.Storehouse.SetPaymentMethod(PanelSecurity.SelectedUserId, methodName,
				displayName, pluginId);
		}

		public static int DeletePaymentMethod(string methodName)
		{
			return EC.Services.Storehouse.DeletePaymentMethod(PanelSecurity.SelectedUserId, methodName);
		}

		public static int AddTaxation(string country, string state, string description, int typeId, 
			decimal amount, bool active)
		{
			return EC.Services.Storehouse.AddTaxation(PanelSecurity.SelectedUserId, country, state,
				description, typeId, amount, active);
		}

		public static int UpdateTaxation(int taxationId, string country, string state, string description, int typeId,
			decimal amount, bool active)
		{
			return EC.Services.Storehouse.UpdateTaxation(PanelSecurity.SelectedUserId, taxationId, 
				country, state, description, typeId, amount, active);
		}

		public static int DeleteTaxation(int taxationId)
		{
			return EC.Services.Storehouse.DeleteTaxation(PanelSecurity.SelectedUserId, taxationId);
		}

		public static int GetTaxationsCount()
		{
			return EC.Services.Storehouse.GetTaxationsCount(PanelSecurity.SelectedUserId);
		}

		public static Taxation[] GetTaxationsPaged(int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetTaxationsPaged(PanelSecurity.SelectedUserId,
				maximumRows, startRowIndex);
		}

		public static Taxation GetTaxation(int taxationId)
		{
			return EC.Services.Storehouse.GetTaxation(PanelSecurity.SelectedUserId, taxationId);
		}

		public static StoreSettings GetStoreSettings(string settingsName)
		{
			return EC.Services.Storehouse.GetStoreSettings(PanelSecurity.SelectedUserId, settingsName);
		}

        public static int SetStoreSettings(string settingsName, StoreSettings settings)
        {
            return EC.Services.Storehouse.SetStoreSettings(PanelSecurity.SelectedUserId, settingsName, 
                settings);
        }

		public static RoutineResult SetInvoiceNotification(string fromEmail, string ccEmail, string subject, 
			string htmlBody, string textBody)
		{
			return EC.Services.Storehouse.SetInvoiceNotification(PanelSecurity.SelectedUserId, fromEmail, ccEmail, 
				subject, htmlBody, textBody);
		}

		public static RoutineResult SetPaymentReceivedNotification(string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return EC.Services.Storehouse.SetPaymentReceivedNotification(PanelSecurity.SelectedUserId, fromEmail, ccEmail,
				subject, htmlBody, textBody);
		}

		public static RoutineResult SetServiceActivatedNotification(string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return EC.Services.Storehouse.SetServiceActivatedNotification(PanelSecurity.SelectedUserId, fromEmail, ccEmail,
				subject, htmlBody, textBody);
		}

		public static RoutineResult SetServiceSuspendedNotification(string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return EC.Services.Storehouse.SetServiceSuspendedNotification(PanelSecurity.SelectedUserId, fromEmail, ccEmail,
				subject, htmlBody, textBody);
		}

		public static RoutineResult SetServiceCancelledNotification(string fromEmail, string ccEmail, string subject,
			string htmlBody, string textBody)
		{
			return EC.Services.Storehouse.SetServiceCancelledNotification(PanelSecurity.SelectedUserId, fromEmail, ccEmail,
				subject, htmlBody, textBody);
		}

		public static bool IsSupportedPluginActive(int pluginId)
		{
			return EC.Services.Storehouse.IsSupportedPluginActive(PanelSecurity.SelectedUserId, pluginId);
		}

		public static Category[] GetStorehousePath(int categoryId)
		{
			return EC.Services.Storefront.GetStorefrontPath(PanelSecurity.SelectedUserId, categoryId);
		}

		public static int GetCustomersPaymentsCount(bool isReseller)
		{
			return EC.Services.Storehouse.GetCustomersPaymentsCount(PanelSecurity.SelectedUserId, isReseller);
		}

		public static CustomerPayment[] GetCustomersPaymentsPaged(bool isReseller, int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetCustomersPaymentsPaged(PanelSecurity.SelectedUserId, isReseller,
				maximumRows, startRowIndex);
		}

		public static int GetCustomersInvoicesCount(bool isReseller)
		{
			return EC.Services.Storehouse.GetCustomersInvoicesCount(PanelSecurity.SelectedUserId, isReseller);
		}

		public static Invoice[] GetCustomersInvoicesPaged(bool isReseller, int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetCustomersInvoicesPaged(PanelSecurity.SelectedUserId, isReseller,
				maximumRows, startRowIndex);
		}

		public static int DeleteCustomerPayment(int paymentId)
		{
			return EC.Services.Storehouse.DeleteCustomerPayment(paymentId);
		}

		public static int ChangeCustomerPaymentStatus(int paymentId, TransactionStatus status)
		{
			return EC.Services.Storehouse.ChangeCustomerPaymentStatus(paymentId, status);
		}

		public static string GetCustomerInvoiceFormatted(int invoiceId)
		{
			return EC.Services.Storehouse.GetCustomerInvoiceFormatted(invoiceId, PortalUtils.CurrentCulture.Name);
		}

		public static Invoice GetCustomerInvoice(int invoiceId)
		{
			return EC.Services.Storehouse.GetCustomerInvoice(invoiceId);
		}

		public static int AddCustomerPayment(string contractId, int invoiceId, string transactionId, 
			decimal amount, string currency, string methodName, TransactionStatus status)
		{
			return EC.Services.Storehouse.AddCustomerPayment(contractId, invoiceId, transactionId, amount, currency,
				methodName, status);
		}

		public static int GetCustomersServicesCount(bool isReseller)
		{
			return EC.Services.Storehouse.GetCustomersServicesCount(PanelSecurity.SelectedUserId, isReseller);
		}

		public static Service[] GetCustomersServicesPaged(bool isReseller, int maximumRows, int startRowIndex)
		{
			return EC.Services.Storehouse.GetCustomersServicesPaged(PanelSecurity.SelectedUserId, isReseller,
				maximumRows, startRowIndex);
		}

		public static DomainNameSvc GetDomainNameService(int serviceId)
		{
			return EC.Services.Storehouse.GetDomainNameService(serviceId);
		}

		public static HostingPackageSvc GetHostingPackageService(int serviceId)
		{
			return EC.Services.Storehouse.GetHostingPackageService(serviceId);
		}

		public static HostingAddonSvc GetHostingAddonService(int serviceId)
		{
			return EC.Services.Storehouse.GetHostingAddonService(serviceId);
		}

		public static Service GetRawCustomerService(int serviceId)
		{
			return EC.Services.Storehouse.GetRawCustomerService(serviceId);
		}

		public static ServiceHistoryRecord[] GetServiceHistory(int serviceId)
		{
			return EC.Services.Storehouse.GetServiceHistory(serviceId);
		}

		public static GenericSvcResult ActivateService(int serviceId, bool sendMail)
		{
			return EC.Services.Storehouse.ActivateService(serviceId, sendMail);
		}

		public static GenericSvcResult SuspendService(int serviceId, bool sendMail)
		{
			return EC.Services.Storehouse.SuspendService(serviceId, sendMail);
		}

		public static GenericSvcResult CancelService(int serviceId, bool sendMail)
		{
			return EC.Services.Storehouse.CancelService(serviceId, sendMail);
		}

		public static GenericSvcResult[] ActivateInvoice(int invoiceId)
		{
			return EC.Services.Storehouse.ActivateInvoice(invoiceId);
		}

		public static bool IsInvoiceProcessed(int invoiceId)
		{
			return EC.Services.Storehouse.IsInvoiceProcessed(invoiceId);
		}

		public static int DeleteCustomerService(int serviceId)
		{
			return EC.Services.Storehouse.DeleteCustomerService(serviceId);
		}
	}
}