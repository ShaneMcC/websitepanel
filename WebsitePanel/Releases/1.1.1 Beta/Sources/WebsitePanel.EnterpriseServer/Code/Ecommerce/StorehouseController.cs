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
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;
using ECU = Common.Utils;
using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.Templates;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
    public class SecurityResult
    {
        public bool Success;
        public int ResultCode;
    }

    public class StorehouseController
    {
        // sensitive keys definitions for supported providers
        public static string[] SENSITIVE_PROPS = { 
			AuthNetSettings.MD5_HASH, AuthNetSettings.TRANSACTION_KEY, 
			PayPalProSettings.SIGNATURE, PayPalProSettings.PASSWORD,
			ToCheckoutSettings.SECRET_WORD
		};

        public const int BUILD_SHADOW_CC_START = 0;
        public const int BUILD_SHADOW_CC_END = 4;

        public static string ApplyStringCustomFormat(string formatString, Hashtable formatOptions)
        {
            // 1. Process date and time variables if any
            formatString = DateTime.Now.ToString(formatString);
            // 2. Process custom variables specified
            foreach (string keyName in formatOptions.Keys)
            {
                formatString = formatString.Replace("[" + keyName + "]", Convert.ToString(formatOptions[keyName]));
            }
            //
            return formatString;
        }

        public static string BuildShadowCcNumber(string ccNumber, int fromStart, int fromEnd)
        {
            int ccLength = ccNumber.Length;
            // verify pre-conditions
            if (fromStart > 4) fromStart = 0;
            //
            if (fromStart == 4 && fromEnd > 2) fromEnd = 2;
            //
            if (fromStart <= 2 && fromEnd > 4) fromEnd = 4;

            //
            StringBuilder builder = new StringBuilder();
            //
            if (fromStart > 0)
                builder.Append(ccNumber.Substring(0, fromStart));
            //
            builder.Append('*', ccLength - fromStart - fromEnd);
            //
            if (fromEnd > 0)
                builder.Append(ccNumber.Substring(ccLength - fromEnd));
            //
            return builder.ToString();
        }

        internal static SecurityResult CheckAccountIsAdminOrReseller()
        {
            SecurityResult result = new SecurityResult();
            // check account
            if (SecurityContext.User.IsInRole(SecurityContext.ROLE_ADMINISTRATOR) ||
                SecurityContext.User.IsInRole(SecurityContext.ROLE_RESELLER))
            {
                result.Success = true;
                return result;
            }
            //
            result.Success = false;
            result.ResultCode = EcommerceErrorCodes.ERROR_INSUFFICIENT_USER_ROLE;
            // return
            return result;
        }

        internal static SecurityResult CheckAccountNotDemoAndActive()
        {
            //
            SecurityResult result = new SecurityResult();
            // check account
            result.ResultCode = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
            //
            if (result.ResultCode == 0)
                result.Success = true;
            // return
            return result;
        }

        internal static SecurityResult CheckAccountActive()
        {
            //
            SecurityResult result = new SecurityResult();
            // check account
            result.ResultCode = SecurityContext.CheckAccount(DemandAccount.IsActive);
            //
            if (result.ResultCode == 0)
                result.Success = true;
            // return
            return result;
        }

        public static int AddBillingCycle(int userId, string cycleName, string billingPeriod, int cycleLength)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            // check security
            if (!result.Success)
                return result.ResultCode;
            // add cycle
            return EcommerceProvider.AddBillingCycle(SecurityContext.User.UserId, userId, cycleName, billingPeriod, cycleLength);
        }

        public static int UpdateBillingCycle(int userId, int cycleId, string cycleName, string billingPeriod, int periodLength)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;
            // return result
            return EcommerceProvider.UpdateBillingCycle(SecurityContext.User.UserId, userId, cycleId, cycleName, billingPeriod, periodLength);
        }

        public static int DeleteBillingCycle(int userId, int cycleId)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;
            // return result
            return EcommerceProvider.DeleteBillingCycle(SecurityContext.User.UserId, userId, cycleId);
        }

        public static BillingCycle GetBillingCycle(int userId, int cycleId)
        {
            return ObjectUtils.FillObjectFromDataReader<BillingCycle>(
                EcommerceProvider.GetBillingCycle(SecurityContext.User.UserId, userId, cycleId)
            );
        }

        public static int GetBillingCyclesCount(int userId)
        {
            return EcommerceProvider.GetBillingCyclesCount(SecurityContext.User.UserId, userId);
        }

        public static List<BillingCycle> GetBillingCyclesPaged(int userId, int maximumRows, int startRowIndex)
        {
            return ObjectUtils.CreateListFromDataReader<BillingCycle>(
                EcommerceProvider.GetBillingCyclesPaged(SecurityContext.User.UserId, userId, maximumRows, startRowIndex)
            );
        }

        public static int AddHostingPlan(int userId, string planName, string productSku, bool taxInclusive, int planId,
            int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, HostingPlanCycle[] planCycles,
            string[] planHighlights, int[] planCategories)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;

            XmlDocument xmldoc = new XmlDocument();
            // build plan cycles
            XmlElement pc_root = xmldoc.CreateElement("PlanCycles");
            for (int i = 0; i < planCycles.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Cycle");
                elem.SetAttribute("ID", planCycles[i].CycleId.ToString());
                elem.SetAttribute("SetupFee", planCycles[i].SetupFee.ToString());
                elem.SetAttribute("RecurringFee", planCycles[i].RecurringFee.ToString());
                elem.SetAttribute("SortOrder", i.ToString());
                pc_root.AppendChild(elem);
            }
            // build plan highlights
            XmlElement hl_root = xmldoc.CreateElement("PlanHighlights");
            for (int i = 0; i < planHighlights.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Item");
                elem.SetAttribute("Text", planHighlights[i]);
                elem.SetAttribute("SortOrder", i.ToString());
                hl_root.AppendChild(elem);
            }
            // build plan categories
            XmlElement ct_root = xmldoc.CreateElement("PlanCategories");
            foreach (int categoryId in planCategories)
            {
                XmlElement elem = xmldoc.CreateElement("Category");
                elem.SetAttribute("ID", categoryId.ToString());
                ct_root.AppendChild(elem);
            }
            // add hosting plan
            return EcommerceProvider.AddHostingPlan(
                SecurityContext.User.UserId,
                userId,
                planName,
                productSku,
                taxInclusive,
                planId,
                userRole,
                initialStatus,
                domainOption,
                enabled,
                planDescription,
                pc_root.OuterXml,
                hl_root.OuterXml,
                ct_root.OuterXml
            );
        }

        public static int AddHostingAddon(int userId, string addonName, string productSku, bool taxInclusive, int planId,
            bool recurring, bool dummyAddon, bool countable, bool enabled, string description, HostingPlanCycle[] addonCycles,
            int[] addonProducts)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;

            XmlDocument xmldoc = new XmlDocument();
            // build addon cycles
            XmlElement pc_root = xmldoc.CreateElement("PlanCycles");
            for (int i = 0; i < addonCycles.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Cycle");
                elem.SetAttribute("ID", addonCycles[i].CycleId.ToString());
                elem.SetAttribute("SetupFee", addonCycles[i].SetupFee.ToString());
                elem.SetAttribute("RecurringFee", addonCycles[i].RecurringFee.ToString());
                elem.SetAttribute("SortOrder", i.ToString());
                pc_root.AppendChild(elem);
            }
            // build plan categories
            XmlElement ap_root = xmldoc.CreateElement("AssignedProducts");
            foreach (int productId in addonProducts)
            {
                XmlElement elem = xmldoc.CreateElement("Product");
                elem.SetAttribute("ID", productId.ToString());
                ap_root.AppendChild(elem);
            }
            // add hosting addon
            return EcommerceProvider.AddHostingAddon(
                SecurityContext.User.UserId,
                userId,
                addonName,
                productSku,
                taxInclusive,
                enabled,
                planId,
                recurring,
                dummyAddon,
                countable,
                description,
                pc_root.OuterXml,
                ap_root.OuterXml
            );
        }

        public static int AddTopLevelDomain(int userId, string topLevelDomain, string productSku, bool taxInclusive, int pluginId,
            bool enabled, bool whoisEnabled, DomainNameCycle[] domainCycles)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;

            XmlDocument xmldoc = new XmlDocument();
            // build plan cycles
            XmlElement dc_root = xmldoc.CreateElement("DomainCycles");
            for (int i = 0; i < domainCycles.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Cycle");
                elem.SetAttribute("ID", domainCycles[i].CycleId.ToString());
                elem.SetAttribute("SetupFee", domainCycles[i].SetupFee.ToString());
                elem.SetAttribute("RecurringFee", domainCycles[i].RecurringFee.ToString());
                elem.SetAttribute("TransferFee", domainCycles[i].TransferFee.ToString());
                elem.SetAttribute("SortOrder", i.ToString());
                dc_root.AppendChild(elem);
            }

            // add top level domain
            return EcommerceProvider.AddTopLevelDomain(SecurityContext.User.UserId, userId,
                topLevelDomain, productSku, taxInclusive, pluginId, enabled, whoisEnabled, dc_root.OuterXml);
        }

        public static int UpdateHostingPlan(int userId, int productId, string planName, string productSku, bool taxInclusive, int planId,
            int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, HostingPlanCycle[] planCycles,
            string[] planHighlights, int[] planCategories)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;

            XmlDocument xmldoc = new XmlDocument();
            // build plan cycles
            XmlElement pc_root = xmldoc.CreateElement("PlanCycles");
            for (int i = 0; i < planCycles.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Cycle");
                elem.SetAttribute("ID", planCycles[i].CycleId.ToString());
                elem.SetAttribute("SetupFee", planCycles[i].SetupFee.ToString());
                elem.SetAttribute("RecurringFee", planCycles[i].RecurringFee.ToString());
                elem.SetAttribute("SortOrder", i.ToString());
                pc_root.AppendChild(elem);
            }
            // build plan highlights
            XmlElement hl_root = xmldoc.CreateElement("PlanHighlights");
            for (int i = 0; i < planHighlights.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Item");
                elem.SetAttribute("Text", planHighlights[i]);
                elem.SetAttribute("SortOrder", i.ToString());
                hl_root.AppendChild(elem);
            }
            // build plan categories
            XmlElement ct_root = xmldoc.CreateElement("PlanCategories");
            foreach (int categoryId in planCategories)
            {
                XmlElement elem = xmldoc.CreateElement("Category");
                elem.SetAttribute("ID", categoryId.ToString());
                ct_root.AppendChild(elem);
            }
            // add hosting plan
            return EcommerceProvider.UpdateHostingPlan(
                SecurityContext.User.UserId,
                userId,
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
                pc_root.OuterXml,
                hl_root.OuterXml,
                ct_root.OuterXml
            );
        }

        public static int UpdateHostingAddon(int userId, int addonId, string addonName, string productSku, bool taxInclusive, int planId,
            bool recurring, bool dummyAddon, bool countable, bool enabled, string description, HostingPlanCycle[] addonCycles,
            int[] addonProducts)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;

            XmlDocument xmldoc = new XmlDocument();
            // build addon cycles
            XmlElement pc_root = xmldoc.CreateElement("PlanCycles");
            for (int i = 0; i < addonCycles.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Cycle");
                elem.SetAttribute("ID", addonCycles[i].CycleId.ToString());
                elem.SetAttribute("SetupFee", addonCycles[i].SetupFee.ToString());
                elem.SetAttribute("RecurringFee", addonCycles[i].RecurringFee.ToString());
                elem.SetAttribute("SortOrder", i.ToString());
                pc_root.AppendChild(elem);
            }
            // build plan categories
            XmlElement ap_root = xmldoc.CreateElement("AssignedProducts");
            foreach (int productId in addonProducts)
            {
                XmlElement elem = xmldoc.CreateElement("Product");
                elem.SetAttribute("ID", productId.ToString());
                ap_root.AppendChild(elem);
            }
            // add hosting addon
            return EcommerceProvider.UpdateHostingAddon(
                SecurityContext.User.UserId,
                userId,
                addonId,
                addonName,
                productSku,
                taxInclusive,
                enabled,
                planId,
                recurring,
                dummyAddon,
                countable,
                description,
                pc_root.OuterXml,
                ap_root.OuterXml
            );
        }

        public static int UpdateTopLevelDomain(int userId, int productId, string topLevelDomain, string productSku, bool taxInclusive, int pluginId,
            bool enabled, bool whoisEnabled, DomainNameCycle[] domainCycles)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;

            XmlDocument xmldoc = new XmlDocument();
            // build plan cycles
            XmlElement dc_root = xmldoc.CreateElement("DomainCycles");
            for (int i = 0; i < domainCycles.Length; i++)
            {
                XmlElement elem = xmldoc.CreateElement("Cycle");
                elem.SetAttribute("ID", domainCycles[i].CycleId.ToString());
                elem.SetAttribute("SetupFee", domainCycles[i].SetupFee.ToString());
                elem.SetAttribute("RecurringFee", domainCycles[i].RecurringFee.ToString());
                elem.SetAttribute("TransferFee", domainCycles[i].TransferFee.ToString());
                elem.SetAttribute("SortOrder", i.ToString());
                dc_root.AppendChild(elem);
            }

            // add top level domain
            return EcommerceProvider.UpdateTopLevelDomain(SecurityContext.User.UserId, userId,
                productId, topLevelDomain, productSku, taxInclusive, pluginId, enabled, whoisEnabled, dc_root.OuterXml);
        }

        public static List<BillingCycle> GetBillingCyclesFree(int userId, int[] cyclesTaken)
        {
            string cyclesTakenXml = Common.Utils.Utils.BuildIdentityXmlFromArray(cyclesTaken, "CyclesTaken", "Cycle");

            return ObjectUtils.CreateListFromDataReader<BillingCycle>(
                EcommerceProvider.GetBillingCyclesFree(SecurityContext.User.UserId, userId, cyclesTakenXml)
            );
        }

        public static int GetProductsCountByType(int userId, int typeId)
        {
            return EcommerceProvider.GetProductsCountByType(
                SecurityContext.User.UserId,
                userId,
                typeId
            );
        }

        public static List<Product> GetProductsPagedByType(int userId, int typeId, int maximumRows, int startRowIndex)
        {
            return ObjectUtils.CreateListFromDataReader<Product>(
                EcommerceProvider.GetProductsPagedByType(
                    SecurityContext.User.UserId,
                    userId,
                    typeId,
                    maximumRows,
                    startRowIndex
                )
            );
        }

        public static List<int> GetHostingPlansTaken(int userId)
        {
            List<int> plans = new List<int>();
            // load taken plans
            IDataReader reader = EcommerceProvider.GetHostingPlansTaken(
                SecurityContext.User.UserId,
                userId
            );

            while (reader.Read())
            {
                int ordinal = reader.GetOrdinal("PlanID");
                // check for DBNull
                if (!reader.IsDBNull(ordinal))
                    plans.Add(reader.GetInt32(ordinal));
            }

            reader.Close();

            return plans;
        }

        public static List<int> GetHostingAddonsTaken(int userId)
        {
            List<int> addons = new List<int>();
            // load taken plans
            IDataReader reader = EcommerceProvider.GetHostingAddonsTaken(
                SecurityContext.User.UserId,
                userId
            );

            while (reader.Read())
            {
                int ordinal = reader.GetOrdinal("PlanID");
                // check for DBNull
                if (!reader.IsDBNull(ordinal))
                    addons.Add(reader.GetInt32(ordinal));
            }

            reader.Close();

            return addons;
        }

        public static HostingPlan GetHostingPlan(int userId, int productId, bool enabledOnly)
        {
            HostingPlan hostingPlan = ObjectUtils.FillObjectFromDataReader<HostingPlan>(
                EcommerceProvider.GetHostingPlan(userId, productId));
            //
            if (enabledOnly && hostingPlan != null && !hostingPlan.Enabled)
                return null;
            //
            return hostingPlan;
        }

        public static HostingAddon GetHostingAddon(int userId, int productId)
        {
            return ObjectUtils.FillObjectFromDataReader<HostingAddon>(
                EcommerceProvider.GetHostingAddon(userId, productId)
            );
        }

        public static List<HostingPlanCycle> GetHostingPlanCycles(int userId, int productId)
        {
            return ObjectUtils.CreateListFromDataReader<HostingPlanCycle>(
                EcommerceProvider.GetHostingPlanCycles(userId, productId)
            );
        }

        public static List<HostingPlanCycle> GetHostingAddonCycles(int userId, int productId)
        {
            return ObjectUtils.CreateListFromDataReader<HostingPlanCycle>(
                EcommerceProvider.GetHostingAddonCycles(userId, productId)
            );
        }

        public static List<string> GetProductHighlights(int resellerId, int productId)
        {
            List<string> values = new List<string>();

            IDataReader reader = EcommerceProvider.GetProductHighlights(resellerId, productId);

            while (reader.Read())
                values.Add((string)reader["HighlightText"]);

            reader.Close();

            return values;
        }

        public static List<Category> GetProductCategories(int userId, int productId)
        {
            return ObjectUtils.CreateListFromDataReader<Category>(
                EcommerceProvider.GetProductCategories(
                    userId,
                    productId
                )
            );
        }

        public static List<int> GetProductCategoriesIds(int userId, int productId)
        {
            List<int> values = new List<int>();
            // load categories
            IDataReader reader = EcommerceProvider.GetProductCategoriesIds(
                userId,
                productId
            );
            // parse result
            while (reader.Read())
            {
                int ordinal = reader.GetOrdinal("CategoryID");
                if (!reader.IsDBNull(ordinal))
                    values.Add(reader.GetInt32(ordinal));
            }

            // close reader
            reader.Close();
            // return categories
            return values;
        }

        public static int DeleteProduct(int userId, int productId)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;
            //
            return EcommerceProvider.DeleteProduct(SecurityContext.User.UserId, userId, productId);
        }

        public static List<Product> GetProductsByType(int userId, int typeId)
        {
            return ObjectUtils.CreateListFromDataReader<Product>(
                EcommerceProvider.GetProductsByType(userId, typeId)
            );
        }

        public static List<Product> GetAddonProducts(int userId, int productId)
        {
            return ObjectUtils.CreateListFromDataReader<Product>(
                EcommerceProvider.GetAddonProducts(userId, productId)
            );
        }

        public static List<int> GetAddonProductsIds(int userId, int productId)
        {
            List<int> values = new List<int>();

            IDataReader reader = EcommerceProvider.GetAddonProductsIds(userId, productId);

            while (reader.Read())
            {
                int ordinal = reader.GetOrdinal("ProductID");
                // check for DBNull
                if (!reader.IsDBNull(ordinal))
                    values.Add(reader.GetInt32(ordinal));
            }

            reader.Close();

            return values;
        }

        public static List<TopLevelDomain> GetTopLevelDomainsPaged(int userId, int maximumRows, int startRowIndex)
        {
            return ObjectUtils.CreateListFromDataReader<TopLevelDomain>(
                EcommerceProvider.GetTopLevelDomainsPaged(
                    SecurityContext.User.UserId,
                    userId,
                    maximumRows,
                    startRowIndex
                )
            );
        }

        public static TopLevelDomain GetTopLevelDomain(int userId, int productId)
        {
            return ObjectUtils.FillObjectFromDataReader<TopLevelDomain>(
                EcommerceProvider.GetTopLevelDomain(
                    SecurityContext.User.UserId,
                    userId,
                    productId
                )
            );
        }

        public static List<DomainNameCycle> GetTopLevelDomainCycles(int userId, int productId)
        {
            return ObjectUtils.CreateListFromDataReader<DomainNameCycle>(
                EcommerceProvider.GetTopLevelDomainCycles(
                    userId,
                    productId
                )
            );
        }

        public static ProductType GetProductType(int typeId)
        {
            return ObjectUtils.FillObjectFromDataReader<ProductType>(
                EcommerceProvider.GetProductType(typeId));
        }

        public static PaymentMethod GetPaymentMethod(int userId, string methodName)
        {
            return ObjectUtils.FillObjectFromDataReader<PaymentMethod>(
                EcommerceProvider.GetPaymentMethod(SecurityContext.User.UserId, userId, methodName));
        }

        public static int SetPaymentMethod(int userId, string methodName, string displayName, int pluginId)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            return EcommerceProvider.SetPaymentMethod(SecurityContext.User.UserId, userId, methodName,
                displayName, pluginId);
        }

        public static int DeletePaymentMethod(int userId, string methodName)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            return EcommerceProvider.DeletePaymentMethod(SecurityContext.User.UserId, userId, methodName);
        }

        public static int AddTaxation(int userId, string country, string state, string description,
            int typeId, decimal amount, bool active)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            return EcommerceProvider.AddTaxation(SecurityContext.User.UserId, userId, country, state,
                description, typeId, amount, active);
        }

        public static Taxation GetTaxation(int resellerId, int taxationId)
        {
            return ObjectUtils.FillObjectFromDataReader<Taxation>(
                EcommerceProvider.GetTaxation(SecurityContext.User.UserId, resellerId, taxationId));
        }

        public static int GetTaxationsCount(int userId)
        {
            return EcommerceProvider.GetTaxationsCount(SecurityContext.User.UserId, userId);
        }

        public static List<Taxation> GetTaxationsPaged(int userId, int maximumRows, int startRowIndex)
        {
            return ObjectUtils.CreateListFromDataReader<Taxation>(EcommerceProvider.GetTaxationsPaged(
                SecurityContext.User.UserId, userId, maximumRows, startRowIndex));
        }

        public static int UpdateTaxation(int userId, int taxationId, string country, string state,
            string description, int typeId, decimal amount, bool active)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            return EcommerceProvider.UpdateTaxation(SecurityContext.User.UserId, userId, taxationId,
                country, state, description, typeId, amount, active);
        }

        public static int DeleteTaxation(int userId, int taxationId)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            return EcommerceProvider.DeleteTaxation(SecurityContext.User.UserId, userId, taxationId);
        }

        internal static void SetPaymentProfile(string contractId, CheckoutDetails newProfile)
        {
            try
            {
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.TASK_SET_PAYMENT_PROFILE);
                TaskManager.WriteParameter(SystemTaskParams.PARAM_CONTRACT, contractId);
                //
                string propertyNames = String.Empty, propertyValues = String.Empty;

                // cleanup newProfile
                newProfile.Remove(CheckoutKeys.Amount);
                //
                newProfile.Remove(CheckoutKeys.ContractNumber);
                //
                newProfile.Remove(CheckoutKeys.InvoiceNumber);
                //
                newProfile.Remove(CheckoutKeys.Currency);
                //
                newProfile.Remove(CheckoutKeys.IPAddress);

                #region Ensure CC number hasn't changed and will be save properly
                // Load original profile data
                CheckoutDetails oldProfile = GetPaymentProfileInternally(contractId);
                if (oldProfile != null)
                {
                    //
                    string newCc = newProfile[CheckoutKeys.CardNumber];
                    string oldCcShadow = BuildShadowCcNumber(oldProfile[CheckoutKeys.CardNumber],
                        BUILD_SHADOW_CC_START, BUILD_SHADOW_CC_END);
                    //
                    if (String.Equals(newCc, oldCcShadow, StringComparison.InvariantCultureIgnoreCase))
                        newProfile[CheckoutKeys.CardNumber] = oldProfile[CheckoutKeys.CardNumber];
                }
                #endregion
                //
                SecurityUtils.SerializeProfile(ref propertyNames, ref propertyValues, true, newProfile);
                // ensure newProfile serialized
                if (!String.IsNullOrEmpty(propertyNames) && !String.IsNullOrEmpty(propertyValues))
                {
                    EcommerceProvider.SetPaymentProfile(SecurityContext.User.UserId, contractId,
                        propertyNames, propertyValues);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static int DeletePaymentProfile(string contractId)
        {
            return EcommerceProvider.DeletePaymentProfile(SecurityContext.User.UserId, contractId);
        }

        public static CheckoutDetails GetPaymentProfile(string contractId)
        {
            CheckoutDetails profile = GetPaymentProfileInternally(contractId);
            // build shadow cc number
            profile[CheckoutKeys.CardNumber] = BuildShadowCcNumber(profile[CheckoutKeys.CardNumber],
                BUILD_SHADOW_CC_START, BUILD_SHADOW_CC_END);
            //
            return profile;
        }

        internal static CheckoutDetails GetPaymentProfileInternally(string contractId)
        {
            //
            CheckoutDetails details = null;
            //
            IDataReader reader = null;
            //
            try
            {
                //
                reader = EcommerceProvider.GetPaymentProfile(SecurityContext.User.UserId, contractId);
                //
                if (reader.Read())
                {
                    //
                    details = new CheckoutDetails();
                    //
                    string propertyNames = Convert.ToString(reader["PropertyNames"]);
                    //
                    string propertyValues = Convert.ToString(reader["PropertyValues"]);
                    // deserialize payment newProfile
                    SecurityUtils.DeserializeProfile(propertyNames, propertyValues, true, details);
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            //
            return details;
        }

        internal static bool PaymentProfileExists(string contractId)
        {
            return EcommerceProvider.PaymentProfileExists(SecurityContext.User.UserId, contractId);
        }

        public static StoreSettings GetStoreDefaultSettings(string settingsName)
        {
            return SettingsHelper.FillSettingsBunch<StoreSettings>(
                EcommerceProvider.GetStoreDefaultSettings(settingsName),
                "PropertyName",
                "PropertyValue"
            );
        }

        public static StoreSettings GetStoreSettings(int resellerId, string settingsName)
        {
            StoreSettings settings = SettingsHelper.FillSettingsBunch<StoreSettings>(
                EcommerceProvider.GetStoreSettings(resellerId, settingsName),
                "PropertyName",
                "PropertyValue"
            );
            // load default settings
            if (settings.IsEmpty)
                settings = GetStoreDefaultSettings(settingsName);

            return settings;
        }

        public static int SetStoreSettings(int userId, string settingsName, StoreSettings settings)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            string xml = SettingsHelper.ConvertObjectSettings(settings.KeyValueArray);
            //
            return EcommerceProvider.SetStoreSettings(SecurityContext.User.UserId, userId,
                settingsName, xml);
        }

        #region Email Notifications

        public static RoutineResult SetInvoiceNotification(int userId, string fromEmail, string ccEmail, string subject,
            string htmlBody, string textBody)
        {
            return SetNotificationTemplate(userId, StoreSettings.NEW_INVOICE, fromEmail, ccEmail, subject, htmlBody, textBody);
        }

        public static RoutineResult SetPaymentReceivedNotification(int userId, string fromEmail, string ccEmail, string subject,
            string htmlBody, string textBody)
        {
            return SetNotificationTemplate(userId, StoreSettings.PAYMENT_RECEIVED, fromEmail, ccEmail, subject, htmlBody, textBody);
        }

        public static RoutineResult SetServiceCancelledNotification(int userId, string fromEmail, string ccEmail, string subject,
            string htmlBody, string textBody)
        {
            return SetNotificationTemplate(userId, StoreSettings.SERVICE_CANCELLED, fromEmail, ccEmail, subject, htmlBody, textBody);
        }

        public static RoutineResult SetServiceActivatedNotification(int userId, string fromEmail, string ccEmail, string subject,
            string htmlBody, string textBody)
        {
            return SetNotificationTemplate(userId, StoreSettings.SERVICE_ACTIVATED, fromEmail, ccEmail, subject, htmlBody, textBody);
        }

        public static RoutineResult SetServiceSuspendedNotification(int userId, string fromEmail, string ccEmail, string subject,
            string htmlBody, string textBody)
        {
            return SetNotificationTemplate(userId, StoreSettings.SERVICE_SUSPENDED, fromEmail, ccEmail, subject, htmlBody, textBody);
        }

        internal static RoutineResult SetNotificationTemplate(int userId, string settingsName, string fromEmail, string ccEmail, string subject,
            string htmlBody, string textBody)
        {
            //
            RoutineResult result = new RoutineResult();
            //
            SecurityResult sec_result = CheckAccountNotDemoAndActive();
            //
            if (!sec_result.Success)
            {
                result.Succeed = false;
                result.ResultCode = sec_result.ResultCode;
                //
                return result;
            }

            // 1. Check notification subject correctness
            //
            try
            {
                Template tm = new Template(subject);
                tm.CheckSyntax();
            }
            catch (ParserException parserEx)
            {
                result.Succeed = false;
                //
                result.ResultCode = EcommerceErrorCodes.ERROR_NTFY_SUBJECT_TEMPLATE;
                //
                result.Message = "Line: " + parserEx.Line + "; Column: " + parserEx.Column + ";";
                //
                return result;
            }

            // 2. Check notification HTML body correctness
            try
            {
                Template tm = new Template(htmlBody);
                tm.CheckSyntax();
            }
            catch (ParserException parserEx)
            {
                result.Succeed = false;
                //
                result.ResultCode = EcommerceErrorCodes.ERROR_NTFY_HTML_TEMPLATE;
                //
                result.Message = "Line: " + parserEx.Line + "; Column: " + parserEx.Column + ";";
                //
                return result;
            }

            // 3. Check notification Plain Text body correctness
            try
            {
                Template tm = new Template(textBody);
                tm.CheckSyntax();
            }
            catch (ParserException parserEx)
            {
                result.Succeed = false;
                //
                result.ResultCode = EcommerceErrorCodes.ERROR_NTFY_TEXT_TEMPLATE;
                //
                result.Message = "Line: " + parserEx.Line + "; Column: " + parserEx.Column + ";";
                //
                return result;
            }

            //
            StoreSettings settings = new StoreSettings();
            //
            settings["From"] = fromEmail;
            //
            settings["CC"] = ccEmail;
            //
            settings["Subject"] = subject;
            //
            settings["HtmlBody"] = htmlBody;
            //
            settings["TextBody"] = textBody;

            //
            result.ResultCode = SetStoreSettings(userId, settingsName, settings);
            //
            if (result.ResultCode < 0)
                result.Succeed = false;
            else
                result.Succeed = true;
            //
            return result;
        }

        #endregion

        public static bool GetStorefrontSecurePayments(int resellerId)
        {
            StoreSettings settings = GetStoreSettings(resellerId, StoreSettings.SYSTEM_SETTINGS);
            //
            return Utils.ParseBool(settings["SecurePayments"], false);
        }

        public static string GetStorefrontTermsAndConditions(int resellerId)
        {
            StoreSettings settings = GetStoreSettings(resellerId, StoreSettings.TERMS_AND_CONDITIONS);
            //
            return settings["StatementTemplate"];
        }

        public static string GetStorefrontWelcomeMessage(int resellerId)
        {
            StoreSettings settings = GetStoreSettings(resellerId, StoreSettings.WELCOME_MESSAGE);
            //
            return settings["HtmlText"];
        }

        public static bool IsSupportedPluginActive(int resellerId, int pluginId)
        {
            return EcommerceProvider.IsSupportedPluginActive(SecurityContext.User.UserId,
                resellerId, pluginId);
        }

        public static int BulkServiceDelete(string contractId, int[] services)
        {
            string svcsXml = ECU.Utils.BuildIdentityXmlFromArray(services, "Svcs", "Svc");
            //
            return EcommerceProvider.BulkServiceDelete(SecurityContext.User.UserId, contractId, svcsXml);
        }

        public static int GetCustomersPaymentsCount(int userId, bool isReseller)
        {
            return EcommerceProvider.GetCustomersPaymentsCount(SecurityContext.User.UserId,
                userId, isReseller);
        }

        public static List<CustomerPayment> GetCustomersPaymentsPaged(int userId, bool isReseller,
            int maximumRows, int startRowIndex)
        {
            return ObjectUtils.CreateListFromDataReader<CustomerPayment>(EcommerceProvider.GetCustomersPaymentsPaged(
                SecurityContext.User.UserId, userId, isReseller, maximumRows, startRowIndex));
        }

        public static int GetCustomersInvoicesCount(int userId, bool isReseller)
        {
            return EcommerceProvider.GetCustomersInvoicesCount(SecurityContext.User.UserId, userId, isReseller);
        }

        public static List<Invoice> GetCustomersInvoicesPaged(int userId, bool isReseller,
            int maximumRows, int startRowIndex)
        {
            return ObjectUtils.CreateListFromDataReader<Invoice>(EcommerceProvider.GetCustomersInvoicesPaged(
                SecurityContext.User.UserId, userId, isReseller, maximumRows, startRowIndex));
        }

        public static int DeleteCustomerPayment(int paymentId)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            if (!result.Success)
                return result.ResultCode;
            // check user role
            result = CheckAccountIsAdminOrReseller();
            if (!result.Success)
                return result.ResultCode;
            // 
            return EcommerceProvider.DeleteCustomerPayment(SecurityContext.User.UserId, paymentId);
        }

        public static CustomerPayment GetCustomerPayment(int paymentId)
        {
            return ObjectUtils.FillObjectFromDataReader<CustomerPayment>(
                EcommerceProvider.GetCustomerPayment(SecurityContext.User.UserId, paymentId));
        }

        public static int ChangeCustomerPaymentStatus(int paymentId, TransactionStatus status)
        {
            SecurityResult result = CheckAccountNotDemoAndActive();
            //
            if (!result.Success)
                return result.ResultCode;
            //
            CustomerPayment payment = GetCustomerPayment(paymentId);
            //
            if (payment != null && payment.Status != status)
            {
                // change payment status
                payment.Status = status;

                // save payment
                UpdateCustomerPayment(payment.ContractId, paymentId, payment.InvoiceId, payment.TransactionId, payment.Total,
                    payment.Currency, payment.MethodName, payment.PluginId, payment.Status);
            }

            return 0;
        }

        internal static int UpdateCustomerPayment(string contractId, int paymentId, int invoiceId, string transactionId,
            decimal total, string currency, string methodName, int pluginId, TransactionStatus status)
        {
            try
            {
                SecurityResult result = CheckAccountNotDemoAndActive();
                //
                if (!result.Success)
                    return result.ResultCode;
                //
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.TASK_UPDATE_PAYMENT);
                //
                int resultCode = EcommerceProvider.UpdateCustomerPayment(SecurityContext.User.UserId, paymentId, invoiceId,
                    transactionId, total, currency, methodName, pluginId, (int)status);
                //
                TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT] = ContractSystem.ContractController.GetContract(contractId);
                TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE] = InvoiceController.GetCustomerInvoiceInternally(invoiceId);
                TaskManager.TaskParameters[SystemTaskParams.PARAM_PAYMENT] = GetCustomerPayment(paymentId);
                //
                return resultCode;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static string GetBaseCurrency(int resellerId)
        {
            // load settings
            StoreSettings settings = GetStoreSettings(resellerId, StoreSettings.SYSTEM_SETTINGS);
            //
            if (settings != null && !String.IsNullOrEmpty(settings["BaseCurrency"]))
                return settings["BaseCurrency"];

            return "USD";
        }

        public static int AddCustomerPayment(string contractId, int invoiceId, string transactionId, decimal total,
            string currency, string methodName, TransactionStatus transactionStatus)
        {
            //
            try
            {
                Contract contract = ContractSystem.ContractController.GetContract(contractId);
                // check user role
                SecurityResult result = StorehouseController.CheckAccountIsAdminOrReseller();
                if (!result.Success)
                    return result.ResultCode;
                //
                TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.TASK_ADD_PAYMENT);
                //
                Invoice invoice = InvoiceController.GetCustomerInvoiceInternally(invoiceId);
                if (invoice == null)
                    throw new Exception("Invoice not found");
                if (invoice.Paid)
                    throw new Exception("Invoice is already paid or processed.");
                // TRACE
                TaskManager.WriteParameter("TransactionID", transactionId);
                TaskManager.WriteParameter("Method", methodName);
                // save customer payment
                int resultCode = EcommerceProvider.AddCustomerPayment(SecurityContext.User.UserId, invoice.ContractId,
                    invoiceId, transactionId, total, currency, methodName, (int)transactionStatus);
                // ERROR
                if (resultCode < 0)
                {
                    TaskManager.WriteParameter("ResultCode", resultCode);
                    TaskManager.WriteError("Could not add customer payment");
                    //
                    return resultCode;
                }
                // TRACE
                TaskManager.WriteParameter("PaymentID", resultCode);
                //
                TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT] = contract;
                TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE] = invoice;
                TaskManager.TaskParameters[SystemTaskParams.PARAM_PAYMENT] = GetCustomerPayment(resultCode);
                //
                return resultCode;
            }
            catch (Exception ex)
            {
                throw TaskManager.WriteError(ex);
            }
            finally
            {
                TaskManager.CompleteTask();
            }
        }

        public static string GetProductTypeControl(int typeId, string controlKey)
        {
            return EcommerceProvider.GetProductTypeControl(typeId, controlKey);
        }

        public static bool IsInvoiceProcessed(int invoiceId)
        {
            bool processed = true;
            // load invoice lines
            List<InvoiceItem> lines = InvoiceController.GetCustomerInvoiceItems(invoiceId);
            foreach (InvoiceItem line in lines)
            {
                if (line.ServiceId > 0)
                    processed &= line.Processed;
            }
            //
            return processed;
        }

        public static bool HasTopLeveDomainsInStock(int resellerId)
        {
            List<Product> domains = GetProductsByType(resellerId, Product.TOP_LEVEL_DOMAIN);
            // check domains in stock
            if (domains != null && domains.Count > 0)
                return true;
            //
            return false;
        }

        public static CustomerPayment LookupForTransaction(string transactionId)
        {
            return ObjectUtils.FillObjectFromDataReader<CustomerPayment>(
                EcommerceProvider.LookupForTransaction(transactionId));
        }

        public static int UpdateTransactionStatus(int paymentId, TransactionStatus status)
        {
            CustomerPayment tran = GetCustomerPayment(paymentId);
            //
            return UpdateCustomerPayment(tran.ContractId, tran.PaymentId, tran.InvoiceId, tran.TransactionId, tran.Total,
                tran.Currency, tran.MethodName, tran.PluginId, status);
        }
    }
}