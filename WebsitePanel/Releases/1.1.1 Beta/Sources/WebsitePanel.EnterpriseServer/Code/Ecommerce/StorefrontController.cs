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
using System.Data;

using ES = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public sealed class StorefrontController
	{
		public const int EMPTY_ORDER_ITEMS_CODE = -15;

		private StorefrontController()
		{
		}

        public static string GetContractInvoiceFormatted(string contractId, int invoiceId, string cultureName)
        {
			//
            ContractSystem.ContractController.ImpersonateAsContractReseller(contractId);
            //
			return InvoiceController.GetCustomerInvoiceFormattedInternally(contractId, invoiceId, cultureName);
        }

		#region Ecommerce v 2.1.0 routines

		public static TopLevelDomain GetResellerTopLevelDomain(int resellerId, string tld)
		{
			return ES.ObjectUtils.FillObjectFromDataReader<TopLevelDomain>(
				EcommerceProvider.GetResellerTopLevelDomain(resellerId, tld));
		}

		public static List<Product> GetStorefrontProductsByType(int resellerId, int typeId)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Product>(
				EcommerceProvider.GetStorefrontProductsByType(resellerId, typeId)
			);
		}

		public static Product GetStorefrontProduct(int resellerId, int productId)
		{
			return ES.ObjectUtils.FillObjectFromDataReader<Product>(
				EcommerceProvider.GetStorefrontProduct(resellerId, productId));
		}

		public static List<HostingAddon> GetHostingPlanAddons(int resellerId, int planId)
		{
			return ES.ObjectUtils.CreateListFromDataReader<HostingAddon>(
				EcommerceProvider.GetStorefrontHostingPlanAddons(resellerId, planId));
		}

		public static OrderResult SubmitCustomerOrder(string contractId, OrderItem[] orderItems, KeyValueBunch extraArgs)
		{
            //
            Contract contract = ContractSystem.ContractController.GetContract(contractId);
            // Impersonate
            ContractSystem.ContractController.ImpersonateAsContractReseller(contract);
            //
			OrderResult oResult = new OrderResult();
			// check account
			SecurityResult sResult = StorehouseController.CheckAccountActive();
			//
			if (!sResult.Success)
			{
				//
				oResult.Succeed = false;
				//
				oResult.ResultCode = sResult.ResultCode;
				//
				return oResult;
			}
			// check order items not empty
			if (orderItems == null || orderItems.Length == 0)
			{
				//
				oResult.Succeed = false;
				//
				oResult.ResultCode = EMPTY_ORDER_ITEMS_CODE;
				//
				return oResult;
			}
			//
			ES.TaskManager.StartTask("Storefront", "SUBMIT_CUSTOMER_ORDER");

			//
			try
			{
                string currency = StorehouseController.GetBaseCurrency(contract.ResellerId);
				// ordered services
				List<int> orderedSvcs = new List<int>();
				// build services to be ordered
				for (int i = 0; i < orderItems.Length; i++)
				{
					// 
					OrderItem orderItem = orderItems[i];
					//
					int orderedSvcId = 0;
					//
					orderItem.ParentSvcId = (orderItem.ParentIndex > -1) ? orderedSvcs[orderItem.ParentIndex] : orderItem.ParentSvcId;
					// load svc type
					ProductType svcType = StorehouseController.GetProductType(orderItem.TypeId);
					//
					IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
						Type.GetType(svcType.ProvisioningController));
					// add service
					orderedSvcId = controller.AddServiceInfo(contractId, currency, orderItem);
					// check service controller result
					if (orderedSvcId < 1)
					{
						// ROLLBACK HERE
						StorehouseController.BulkServiceDelete(contractId, orderedSvcs.ToArray());
						oResult.Succeed = false;
						oResult.ResultCode = orderedSvcId;
						return oResult;
						// EXIT
					}
					// 
					orderedSvcs.Add(orderedSvcId);
				}
				// build invoice lines
				List<InvoiceItem> invoiceLines = InvoiceController.CalculateInvoiceLinesForServices(orderedSvcs);
				//
				int resultCode = InvoiceController.AddInvoice(contractId, invoiceLines, extraArgs);
				// ERROR
				if (resultCode < 1)
				{
					// ROLLBACK HERE
					StorehouseController.BulkServiceDelete(contractId, orderedSvcs.ToArray());
					oResult.Succeed = false;
					oResult.ResultCode = resultCode;
					return oResult;
				}
				// 
				oResult.OrderInvoice = resultCode;
				//
				oResult.Succeed = true;
			}
			catch (Exception ex)
			{
				//
				oResult.ResultCode = -1;
				//
				oResult.Succeed = false;
				//
				ES.TaskManager.WriteError(ex);
			}
			finally
			{
				//
				ES.TaskManager.CompleteTask();
			}
			//
			return oResult;
		}

		public static List<PaymentMethod> GetResellerPaymentMethods(int resellerId)
		{
			return ES.ObjectUtils.CreateListFromDataReader<PaymentMethod>(
				EcommerceProvider.GetResellerPaymentMethods(resellerId));
		}

		public static PaymentMethod GetContractPaymentMethod(string contractId, string methodName)
		{
            Contract contract = ContractSystem.ContractController.GetContract(contractId);

			return ES.ObjectUtils.FillObjectFromDataReader<PaymentMethod>(
				EcommerceProvider.GetResellerPaymentMethod(contract.ResellerId, methodName));
		}

		public static ES.UserInfo GetCustomerProfile(int resellerId, int userId, string sessionId)
		{
			//
			ES.SecurityContext.SetThreadPrincipal(resellerId);
			//
			return ES.UserController.GetUser(userId);
		}

		public static string GetCustomerInvoiceFormatted(int resellerId, int userId, int invoiceId, string sessionId)
		{
			return null;// InvoiceController.GetCustomerInvoiceFormattedInternally(resellerId, userId, invoiceId);
		}

		public static Invoice GetCustomerInvoice(int resellerId, int userId, int invoiceId, string sessionId)
		{
			return InvoiceController.GetCustomerInvoiceInternally(invoiceId);
		}

		public static CheckoutResult CompleteCheckout(string contractId, int invoiceId, 
			string methodName, CheckoutDetails details)
		{
            //
            return PaymentGatewayController.CheckOut(contractId, invoiceId, methodName, details);
		}

		public static KeyValueBunch GetHostingPlansQuotas(int resellerId, int planId)
		{
			KeyValueBunch hpQuotas = new KeyValueBunch();
			//
			ES.SecurityContext.SetThreadPrincipal(resellerId);
			//
			ES.HostingPlanContext ctx = ES.PackageController.GetHostingPlanContext(planId);
			//
			if (ctx != null)
			{
				//
				ES.QuotaValueInfo[] quotasArray = ctx.QuotasArray;
				//
				for (int i = 0; i < ctx.GroupsArray.Length; i++)
				{
					//
					ES.HostingPlanGroupInfo group = ctx.GroupsArray[i];
					//
					if (group.Enabled)
					{
						//
						List<string> planQuota = new List<string>();
						//
						foreach (ES.QuotaValueInfo quota in quotasArray)
						{
							//
							if (quota.QuotaName.StartsWith(group.GroupName))
							{
								// boolean quota
								if (quota.QuotaTypeId == 1)
								{
									// only enabled quotas will be displayed
									if (quota.QuotaAllocatedValue == 1)
										planQuota.Add(quota.QuotaName + "=Enabled");
								}
								// numeric
								else
								{
									if (quota.QuotaAllocatedValue > 0)
										planQuota.Add(quota.QuotaName + "=" + quota.QuotaAllocatedValue);
									else if (quota.QuotaAllocatedValue == -1)
										planQuota.Add(quota.QuotaName + "=Unlimited");
								}
							}
						}
						// only filled-up groups are allowed to display
						if (planQuota.Count > 0)
							hpQuotas[group.GroupName] = String.Join(",", planQuota.ToArray());
					}
				}
			}
			//
			return hpQuotas;
		}

		public static CheckDomainResult CheckDomain(int resellerId, string domain, string tld)
		{
			//
			CheckDomainResult result = new CheckDomainResult();
			//
			try
			{
				//
				TopLevelDomain rslTld = GetResellerTopLevelDomain(resellerId, tld);
				//
				if (rslTld != null && !rslTld.WhoisEnabled)
				{
					//
					result.Succeed = true;
					//
					result.ResultCode = 0;
					//
					return result;
				}

				// 
				WhoisResult wResult = WhoisLookup.Query(domain, tld);
				// query error
				if (!wResult.Success)
				{
					//
					result.ErrorMessage = wResult.ErrorMessage;
					//
					result.Succeed = false;
					//
					result.ResultCode = CheckDomainResult.QUERY_ERROR;
					//
					return result;
				}

				// whois record found
				if (wResult.RecordFound)
				{
					//
					result.ResultCode = CheckDomainResult.DOMAIN_BUSY;
					//
					result.Succeed = true;
					//
					return result;
				}

				// whois record not found - domain is available for purchase
				result.Succeed = true;
				//
				result.ResultCode = 0;
			}
			catch (Exception ex)
			{
				//
				result.ErrorMessage = ex.StackTrace;
				//
				result.Succeed = false;
				//
				result.ResultCode = CheckDomainResult.UNSPECIFIED_ERROR;
			}
			//
			return result;
		}

		#endregion

		#region Storefront Categories routines

		public static List<Category> GetStorefrontPath(int resellerId, int categoryId)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Category>(
				EcommerceProvider.GetStorefrontPath(resellerId, categoryId));
		}

		public static List<Category> GetStorefrontCategories(int resellerId, int parentId)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Category>(
				EcommerceProvider.GetStorefrontCategories(resellerId, parentId));
		}

		public static Category GetStorefrontCategory(int resellerId, int categoryId)
		{
			return ES.ObjectUtils.FillObjectFromDataReader<Category>(
				EcommerceProvider.GetStorefrontCategory(resellerId, categoryId));
		}

		public static List<Product> GetStorefrontProductsInCategory(int resellerId, int categoryId)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Product>(
				EcommerceProvider.GetStorefrontProductsInCategory(resellerId, categoryId));
		}

		#endregion
	}
}