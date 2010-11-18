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
using System.Web;
using System.Data;
using System.Configuration;
using System.Collections.Generic;

using WebsitePanel.EnterpriseServer;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.Portal;

namespace WebsitePanel.Ecommerce.Portal
{
	public class StorefrontHelper
	{
		private StorefrontHelper()
		{
		}

		#region Ecommerce v 2.1.0

        public static bool UsernameExists(string username)
        {
            return EC.Services.Storefront.UsernameExists(username);
        }

        public static ContractAccount GetContractAccount(string contractId)
        {
            return EC.Services.Storefront.GetContractAccount(contractId);
        }

		public static Product[] GetStorefrontProductsByType(int resellerId, int typeId)
		{
			return EC.Services.Storefront.GetStorefrontProductsByType(resellerId, typeId);
		}

		public static HostingPlanCycle[] GetHostingPlanCycles(int resellerId, int productId)
		{
			return EC.Services.Storefront.GetHostingPlanCycles(resellerId, productId);
		}

		public static HostingPlanCycle[] GetHostingAddonCycles(int resellerId, int addonId)
		{
			return EC.Services.Storefront.GetHostingAddonCycles(resellerId, addonId);
		}

		public static HostingPlan GetHostingPlan(int resellerId, int productId)
		{
			return EC.Services.Storefront.GetHostingPlan(resellerId, productId);
		}

		public static DomainNameCycle[] GetTopLevelDomainCycles(int resellerId, int productId)
		{
			return EC.Services.Storefront.GetTopLevelDomainCycles(resellerId, productId);
		}

        public static GenericResult AddContract(ContractAccount accountSettings)
        {
            return EC.Services.Storefront.AddContract(ecPanelRequest.ResellerId, accountSettings);
        }

		public static GenericResult AddContract(int resellerId, ContractAccount accountSettings)
		{
			return EC.Services.Storefront.AddContract(resellerId, accountSettings);
		}

		public static OrderResult SubmitCustomerOrder(string contractId, 
			OrderItem[] orderItems, KeyValueBunch extraArgs)
		{
			//
			return EC.Services.Storefront.SubmitCustomerOrder(contractId, orderItems, extraArgs);
		}

		public static PaymentMethod[] GetResellerPaymentMethods(int resellerId)
		{
			return EC.Services.Storefront.GetResellerPaymentMethods(resellerId);
		}

		public static PaymentMethod GetContractPaymentMethod(string contractId, string methodName)
		{
            return EC.Services.Storefront.GetContractPaymentMethod(contractId, methodName);
		}

		public static string GetContractInvoiceTemplated(string contractId, int invoiceId)
		{
			return EC.Services.Storefront.GetContractInvoiceFormatted(contractId, invoiceId, PortalUtils.CurrentUICulture.Name);
		}

        public static CheckoutFormParams GetCheckoutFormParams(string contractId, int invoiceId, 
			string methodName, KeyValueBunch options)
		{
			return EC.Services.Storefront.GetCheckoutFormParams(contractId, invoiceId, methodName, options);
		}

		public static CheckoutResult CompleteCheckout(string contractId, int invoiceId, 
			string methodName, CheckoutDetails details)
		{
			return EC.Services.Storefront.CompleteCheckout(contractId, invoiceId, methodName, details);
		}

		public static string[] GetProductHighlights(int resellerId, int productId)
		{
			return EC.Services.Storefront.GetProductHighlights(resellerId, productId);
		}

		public static KeyValueBunch GetHostingPlanQuotas(int resellerId, int planId)
		{
			return EC.Services.Storefront.GetHostingPlanQuotas(resellerId, planId);
		}

		public static CheckDomainResult CheckDomain(int resellerId, string domain, string tld)
		{
			return EC.Services.Storefront.CheckDomain(resellerId, domain, tld);
		}

		public static Category[] GetStorefrontPath(int categoryId)
		{
			return EC.Services.Storefront.GetStorefrontPath(ecPanelRequest.ResellerId, categoryId);
		}

		public static Category[] GetStorefrontCategories(int parentId)
		{
			return EC.Services.Storefront.GetStorefrontCategories(
				ecPanelRequest.ResellerId,
				parentId
			);
		}

		public static Category GetStorefrontCategory(int categoryId)
		{
			return EC.Services.Storefront.GetStorefrontCategory(
				ecPanelRequest.ResellerId,
				categoryId
			);
		}

		public static Product[] GetProductsInCategory(int resellerId, int categoryId)
		{
			return EC.Services.Storefront.GetProductsInCategory(resellerId, categoryId);
		}

		public static Product GetStorefrontProduct(int resellerId, int productId)
		{
			return EC.Services.Storefront.GetStorefrontProduct(resellerId, productId);
		}

		public static HostingAddon[] GetHostingPlanAddons(int resellerId, int planId)
		{
			return EC.Services.Storefront.GetHostingPlanAddons(resellerId, planId);
		}

		public static string GetBaseCurrency(int resellerId)
		{
			return EC.Services.Storefront.GetBaseCurrency(resellerId);
		}

		public static string GetTermsAndConditions(int resellerId)
		{
			return EC.Services.Storefront.GetStorefrontTermsAndConditions(resellerId);
		}

		public static string GetWelcomeMessage(int resellerId)
		{
			return EC.Services.Storefront.GetStorefrontWelcomeMessage(resellerId);
		}

		public static bool HasTopLevelDomainsInStock(int resellerId)
		{
			return EC.Services.Storefront.HasTopLeveDomainsInStock(resellerId);
		}

		public static bool GetSecurePayments(int resellerId)
		{
			return EC.Services.Storefront.GetStorefrontSecurePayments(resellerId);
		}

		#endregion
	}
}