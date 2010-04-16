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
    /// Summary description for ecStorefront
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class ecStorefront : System.Web.Services.WebService
    {
        #region E-Commerce v 2.4.0 changes

        [WebMethod]
        public bool UsernameExists(string username)
        {
            return UserController.UserExists(username);
        }

        [WebMethod]
        public GenericResult AddContract(int resellerId, ContractAccount accountSettings)
        {
            return ContractSystem.ContractController.AddContract(resellerId, accountSettings);
        }

        [WebMethod]
        public OrderResult SubmitCustomerOrder(string contractId, OrderItem[] orderItems, KeyValueBunch extraArgs)
        {
            return StorefrontController.SubmitCustomerOrder(contractId, orderItems, extraArgs);
        }

        [WebMethod]
        public string GetContractInvoiceFormatted(string contractId, int invoiceId, string cultureName)
        {
			return StorefrontController.GetContractInvoiceFormatted(contractId, invoiceId, cultureName);
        }

        [WebMethod]
        public ContractAccount GetContractAccount(string contractId)
        {
            return ContractSystem.ContractController.GetContractAccountSettings(contractId);
        }

        [WebMethod]
        public CheckoutFormParams GetCheckoutFormParams(string contractId, int invoiceId,
            string methodName, KeyValueBunch options)
        {
            return SystemPluginController.GetCheckoutFormParams(contractId, invoiceId, methodName, options);
        }

        [WebMethod]
        public CheckoutResult CompleteCheckout(string contractId, int invoiceId, string methodName,
            CheckoutDetails details)
        {
            return StorefrontController.CompleteCheckout(contractId, invoiceId, methodName, details);
        }

        #endregion

        #region Ecommerce v 2.1.0

        [WebMethod]
		public string GetProductTypeControl(int typeId, string controlKey)
		{
			return StorehouseController.GetProductTypeControl(typeId, controlKey);
		}

		[WebMethod]
		public List<Product> GetStorefrontProductsByType(int resellerId, int typeId)
		{
			return StorefrontController.GetStorefrontProductsByType(resellerId, typeId);
		}

		[WebMethod]
		public List<HostingPlanCycle> GetHostingPlanCycles(int resellerId, int productId)
		{
			return StorehouseController.GetHostingPlanCycles(resellerId, productId);
		}

		[WebMethod]
		public HostingPlan GetHostingPlan(int resellerId, int productId)
		{
			return StorehouseController.GetHostingPlan(resellerId, productId, true);
		}

		[WebMethod]
		public List<HostingAddon> GetHostingPlanAddons(int resellerId, int planId)
		{
			return StorefrontController.GetHostingPlanAddons(resellerId, planId);
		}

		[WebMethod]
		public List<HostingPlanCycle> GetHostingAddonCycles(int resellerId, int addonId)
		{
			return StorehouseController.GetHostingAddonCycles(resellerId, addonId);
		}

		[WebMethod]
		public List<string> GetProductHighlights(int resellerId, int productId)
		{
			return StorehouseController.GetProductHighlights(resellerId, productId);
		}

		[WebMethod]
		public List<DomainNameCycle> GetTopLevelDomainCycles(int resellerId, int productId)
		{
			return StorehouseController.GetTopLevelDomainCycles(resellerId, productId);
		}

		[WebMethod]
		public List<PaymentMethod> GetResellerPaymentMethods(int resellerId)
		{
			return StorefrontController.GetResellerPaymentMethods(resellerId);
		}

		[WebMethod]
		public PaymentMethod GetContractPaymentMethod(string contractId, string methodName)
		{
			return StorefrontController.GetContractPaymentMethod(contractId, methodName);
		}

		[WebMethod]
		public KeyValueBunch GetHostingPlanQuotas(int resellerId, int planId)
		{
			return StorefrontController.GetHostingPlansQuotas(resellerId, planId);
		}

		[WebMethod]
		public CheckDomainResult CheckDomain(int resellerId, string domain, string tld)
		{
			return StorefrontController.CheckDomain(resellerId, domain, tld);
		}

		[WebMethod]
		public string GetBaseCurrency(int resellerId)
		{
			return StorehouseController.GetBaseCurrency(resellerId);
		}

		[WebMethod]
		public bool HasTopLeveDomainsInStock(int resellerId)
		{
			return StorehouseController.HasTopLeveDomainsInStock(resellerId);
		}

		#endregion

		#region Storefront routines

		[WebMethod]
		public List<Category> GetStorefrontCategories(int resellerId, int parentId)
		{
			return StorefrontController.GetStorefrontCategories(
				resellerId,
				parentId
			);
		}

		[WebMethod]
		public Category GetStorefrontCategory(int resellerId, int categoryId)
		{
			return StorefrontController.GetStorefrontCategory(
				resellerId,
				categoryId
			);
		}

		[WebMethod]
		public List<Category> GetStorefrontPath(int resellerId, int categoryId)
		{
			return StorefrontController.GetStorefrontPath(
				resellerId,
				categoryId
			);
		}

		#endregion

		[WebMethod]
		public string GetStorefrontTermsAndConditions(int resellerId)
		{
			return StorehouseController.GetStorefrontTermsAndConditions(resellerId);
		}

		[WebMethod]
		public bool GetStorefrontSecurePayments(int resellerId)
		{
			return StorehouseController.GetStorefrontSecurePayments(resellerId);
		}

		[WebMethod]
		public string GetStorefrontWelcomeMessage(int resellerId)
		{
			return StorehouseController.GetStorefrontWelcomeMessage(resellerId);
		}

		[WebMethod]
		public List<Product> GetProductsInCategory(int resellerId, int categoryId)
		{
			return StorefrontController.GetStorefrontProductsInCategory(
				resellerId, categoryId);
		}

		[WebMethod]
		public Product GetStorefrontProduct(int resellerId, int productId)
		{
			return StorefrontController.GetStorefrontProduct(resellerId, productId);
		}
	}
}
