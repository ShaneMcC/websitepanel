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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ES = WebsitePanel.EnterpriseServer;
using System.Threading;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Templates;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
    public class InvoiceController
    {
        private InvoiceController()
        {
        }

        #region Ecommerce v2.1.0 routines

        #region Helper methods

        public static string BuildAddXmlForInvoiceItems(List<InvoiceItem> items)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("items");

            foreach (InvoiceItem item in items)
            {
                XmlElement node = xml.CreateElement("item");

                node.SetAttribute("serviceid", item.ServiceId.ToString());
                node.SetAttribute("itemname", item.ItemName);
                node.SetAttribute("typename", item.TypeName);
                node.SetAttribute("quantity", item.Quantity.ToString());
                node.SetAttribute("total", item.Total.ToString(CultureInfo.InvariantCulture));
                node.SetAttribute("subtotal", item.SubTotal.ToString(CultureInfo.InvariantCulture));
                node.SetAttribute("unitprice", item.UnitPrice.ToString(CultureInfo.InvariantCulture));

                root.AppendChild(node);
            }

            return root.OuterXml;
        }

        public static List<InvoiceItem> CalculateInvoiceLinesForServices(List<int> services)
        {
            List<InvoiceItem> lines = new List<InvoiceItem>();

            foreach (int serviceId in services)
            {
                ProductType svc_type = ServiceController.GetServiceItemType(serviceId);
                //
                IServiceProvisioning controller = (IServiceProvisioning)Activator.CreateInstance(
                    Type.GetType(svc_type.ProvisioningController));
                //
                InvoiceItem[] ilines = controller.CalculateInvoiceLines(serviceId);
                foreach (InvoiceItem iline in ilines)
                {
                    iline.SubTotal = iline.UnitPrice * iline.Quantity;
                    iline.Total = iline.SubTotal;
                }
                //
                lines.AddRange(ilines);
            }
            //
            return lines;
        }

        #endregion

        public static int VoidCustomerInvoice(int invoiceId)
        {
            SecurityResult result = StorehouseController.CheckAccountIsAdminOrReseller();
            if (!result.Success)
                return result.ResultCode;
            // void
            EcommerceProvider.VoidCustomerInvoice(ES.SecurityContext.User.UserId, invoiceId);
            //
            return 0;
        }

        public static Taxation GetCustomerTaxation(string contractId, string country, string state)
        {
            return ES.ObjectUtils.FillObjectFromDataReader<Taxation>(
                EcommerceProvider.GetCustomerTaxation(contractId, country, state));
        }

        public static void CalculateInvoiceChargeAmounts(Taxation tax, List<InvoiceItem> items,
            out decimal totalAmount, out decimal subTotalAmount, out decimal taxAmount)
        {
            totalAmount = subTotalAmount = taxAmount = 0;
            // calculate all invoice items
            foreach (InvoiceItem item in items)
                subTotalAmount += item.SubTotal;
            //
            totalAmount = subTotalAmount;
            // tax applies
            if (tax != null)
            {
                switch (tax.Type)
                {
                    case TaxationType.Fixed:
                        taxAmount = tax.Amount;
                        totalAmount += taxAmount;
                        break;
                    case TaxationType.Percentage:
                        taxAmount = (subTotalAmount / 100) * tax.Amount;
                        totalAmount += taxAmount;
                        break;
                    case TaxationType.TaxIncluded:
                        taxAmount = totalAmount - totalAmount * 100 / (100M + tax.Amount);
                        subTotalAmount -= taxAmount;
                        break;
                }
            }
        }

        public static int AddInvoice(string contractId, List<InvoiceItem> invoiceLines, KeyValueBunch extraArgs)
        {
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(contractId);
            // read customer tax
            Taxation tax = GetCustomerTaxation(contractId, account[ContractAccount.COUNTRY], account[ContractAccount.STATE]);
            int taxationId = (tax == null) ? -1 : tax.TaxationId;

            // Calculate invoice amounts
            decimal totalAmount = 0, subTotalAmount = 0, taxAmount = 0;
            CalculateInvoiceChargeAmounts(tax, invoiceLines, out totalAmount, out subTotalAmount, out taxAmount);

            // align svcs suspend date
            int[] svcs = new int[invoiceLines.Count];
            for (int i = 0; i < invoiceLines.Count; i++)
                svcs[i] = invoiceLines[i].ServiceId;
            DateTime sdateAligned = ServiceController.GetSvcsSuspendDateAligned(svcs, DateTime.Now);
            //
            StoreSettings settings = StorehouseController.GetStoreSettings(ES.SecurityContext.User.UserId,
                StoreSettings.SYSTEM_SETTINGS);
            // get invoice grace period in days
            int gracePeriod = Common.Utils.Utils.ParseInt(settings["InvoiceGracePeriod"], 0);
            //
            if (gracePeriod < 0) gracePeriod = 0;
            //
            DateTime created = DateTime.Now;
            DateTime dueDate = sdateAligned.AddDays(gracePeriod);
            //
            return AddInvoice(contractId, created, dueDate, taxationId, totalAmount, subTotalAmount, taxAmount,
                invoiceLines, extraArgs);
        }

        public static int AddInvoice(string contractId, DateTime created, DateTime dueDate,
            int taxationId, decimal totalAmount, decimal subTotalAmount, decimal taxAmount, List<InvoiceItem> invoiceLines, KeyValueBunch extraArgs)
        {
            //
            try
            {
                Contract contract = ContractSystem.ContractController.GetContract(contractId);
                //
                ES.TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.TASK_ADD_INVOICE);
                // build xml representation
                string invoiceLinesXml = BuildAddXmlForInvoiceItems(invoiceLines);
                // add invoice
                int result = EcommerceProvider.AddInvoice(contractId, created, dueDate,
                    taxationId, totalAmount, subTotalAmount, taxAmount, invoiceLinesXml,
                    StorehouseController.GetBaseCurrency(contract.ResellerId));

                // check error
                if (result < 1)
                    return result; // EXIT

                // build invoice number
                Invoice invoice = GetCustomerInvoiceInternally(result);
                StoreSettings settings = StorehouseController.GetStoreSettings(contract.ResellerId, StoreSettings.SYSTEM_SETTINGS);
                if (!String.IsNullOrEmpty(settings["InvoiceNumberFormat"]))
                {
                    Hashtable options = new Hashtable();
                    options["ID"] = result;
                    invoice.InvoiceNumber = StorehouseController.ApplyStringCustomFormat(
                        settings["InvoiceNumberFormat"], options);
                }
                else
                {
                    invoice.InvoiceNumber = result.ToString();
                }
                // update invoice
                InvoiceController.UpdateInvoice(invoice.InvoiceId, invoice.InvoiceNumber, invoice.DueDate,
                    invoice.Total, invoice.SubTotal, invoice.TaxationId, invoice.TaxAmount, invoice.Currency);
                //
                ES.TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT] = contract;
                ES.TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE] = invoice;
                ES.TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE_LINES] = invoiceLines;
                ES.TaskManager.TaskParameters[SystemTaskParams.PARAM_EXTRA_ARGS] = extraArgs;
                //
                return result;
            }
            catch (Exception ex)
            {
                throw ES.TaskManager.WriteError(ex);
            }
            finally
            {
                ES.TaskManager.CompleteTask();
            }
        }

        public static int UpdateInvoice(int invoiceId, string invoiceNumber, DateTime dueDate,
            decimal total, decimal subTotal, int taxationId, decimal taxAmount, string currency)
        {
            return EcommerceProvider.UpdateInvoice(ES.SecurityContext.User.UserId, invoiceId,
                invoiceNumber, dueDate, total, subTotal, taxationId, taxAmount, currency);
        }

        internal static Invoice GetCustomerInvoiceInternally(int invoiceId)
        {
            return ES.ObjectUtils.FillObjectFromDataReader<Invoice>(
                EcommerceProvider.GetCustomerInvoice(ES.SecurityContext.User.UserId, invoiceId));
        }

        public static List<InvoiceItem> GetCustomerInvoiceItems(int invoiceId)
        {
            return ES.ObjectUtils.CreateListFromDataReader<InvoiceItem>(
                EcommerceProvider.GetCustomerInvoiceItems(ES.SecurityContext.User.UserId, invoiceId));
        }

        internal static string GetCustomerInvoiceFormattedInternally(int invoiceId, string cultureName)
        {
            Invoice invoice = GetCustomerInvoiceInternally(invoiceId);
            //
            return GetCustomerInvoiceFormattedInternally(invoice.ContractId, invoiceId, cultureName);
        }

        internal static string GetCustomerInvoiceFormattedInternally(string contractId, int invoiceId, string cultureName)
        {
            Contract contract = ContractSystem.ContractController.GetContract(contractId);
            ContractAccount accountSettings = ContractSystem.ContractController.GetContractAccountSettings(contractId);
            //
            return GetCustomerInvoiceFormattedInternally(contract, invoiceId, accountSettings, cultureName);
        }

        internal static string GetCustomerInvoiceFormattedInternally(Contract contract, int invoiceId, ContractAccount accountSettings, string cultureName)
        {
            //
            Invoice invoiceInfo = GetCustomerInvoiceInternally(invoiceId);
            // impersonate
            ES.SecurityContext.SetThreadPrincipal(contract.ResellerId);
            // load settings
            StoreSettings settings = StorehouseController.GetStoreSettings(contract.ResellerId, StoreSettings.NEW_INVOICE);
            //
            string templateBody = settings["HtmlBody"];
            //
            Taxation invoiceTax = StorehouseController.GetTaxation(contract.ResellerId, invoiceInfo.TaxationId);
            //
            List<InvoiceItem> invoiceLines = GetCustomerInvoiceItems(invoiceId);
            Dictionary<int, Service> invoiceServices = ServiceController.GetServicesDictionary(invoiceLines);
            //
            Template tm = new Template(templateBody);
            tm["Invoice"] = invoiceInfo;
            tm["InvoiceLines"] = invoiceLines;
            tm["InvoiceServices"] = invoiceServices;
            tm["Customer"] = accountSettings;
            tm["Tax"] = invoiceTax;

            StringWriter writer = new StringWriter();
            try
            {
                // Preserve an original thread culture
                CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;
                //
                if (!String.IsNullOrEmpty(cultureName))
                {
                    try
                    {
                        CultureInfo formattingCulture = new CultureInfo(cultureName);
						// Empty currency symbol to supporty 3-letters ISO format 
						// hardcorded in HTML templates
						formattingCulture.NumberFormat.CurrencySymbol = String.Empty;
                        // Set formatting culture
                        Thread.CurrentThread.CurrentCulture = formattingCulture;
                    }
                    catch (Exception ex)
                    {
                        TaskManager.WriteWarning("Wrong culture name has been provided. Culture name: {0}.", cultureName);
                        TaskManager.WriteWarning(ex.StackTrace);
                        TaskManager.WriteWarning(ex.Message);
                    }
                }

                // Process template
                tm.Evaluate(writer);
                templateBody = writer.ToString();

                // Revert the original thread's culture back
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
            catch (ParserException ex)
            {
                return String.Format("Error in template (Line {0}, Column {1}): {2}",
                    ex.Line, ex.Column, ex.Message);
            }

            return templateBody;
        }

        public static List<Invoice> GetUnpaidInvoices(int resellerId)
        {
            return ES.ObjectUtils.CreateListFromDataReader<Invoice>(
                EcommerceProvider.GetUnpaidInvoices(ES.SecurityContext.User.UserId, resellerId));
        }

        #endregion

        #region New implementation

        public static List<InvoiceItem> GetInvoicesItemsToActivate(int resellerId)
        {
            return ES.ObjectUtils.CreateListFromDataReader<InvoiceItem>(
                EcommerceProvider.GetInvoicesItemsToActivate(ES.SecurityContext.User.UserId, resellerId));
        }

        public static List<InvoiceItem> GetInvoicesItemsOverdue(int resellerId, DateTime dueDate)
        {
            return ES.ObjectUtils.CreateListFromDataReader<InvoiceItem>(
                EcommerceProvider.GetInvoicesItemsOverdue(ES.SecurityContext.User.UserId, resellerId, dueDate));
        }

        public static int SetInvoiceItemProcessed(int invoiceId, int itemId)
        {
            return EcommerceProvider.SetInvoiceItemProcessed(invoiceId, itemId);
        }

        #endregion

        #region Provisioning routines

        #endregion
    }
}
