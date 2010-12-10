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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using ES = WebsitePanel.EnterpriseServer;
using WebsitePanel.Templates;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
    public class MiscController
    {
        private MiscController()
        {
        }

        public static int SendNewInvoiceNotification(int invoiceId)
        {
            Invoice invoice = InvoiceController.GetCustomerInvoiceInternally(invoiceId);
            List<InvoiceItem> invoiceLines = InvoiceController.GetCustomerInvoiceItems(invoiceId);

            return SendNewInvoiceNotification(invoice, invoiceLines, null);
        }

        public static int SendNewInvoiceNotification(Invoice invoice)
        {
            List<InvoiceItem> invoiceLines = InvoiceController.GetCustomerInvoiceItems(invoice.InvoiceId);

            return SendNewInvoiceNotification(invoice, invoiceLines, null);
        }

        public static int SendNewInvoiceNotification(Invoice invoice, List<InvoiceItem> invoiceLines, KeyValueBunch extraArgs)
        {
            Contract contract = ContractSystem.ContractController.GetContract(invoice.ContractId);
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(invoice.ContractId);
			Dictionary<int, Service> invoiceServices = ServiceController.GetServicesDictionary(invoiceLines);
            Hashtable items = new Hashtable();

            items["Invoice"] = invoice;
            items["InvoiceLines"] = invoiceLines;
			items["InvoiceServices"] = invoiceServices;
            items["Tax"] = StorehouseController.GetTaxation(contract.ResellerId, invoice.TaxationId);
            items["Customer"] = account;
            items["IsEmail"] = "1";
            if (extraArgs != null)
                items["ExtraArgs"] = extraArgs;

            return SendSystemNotification(StoreSettings.NEW_INVOICE, account, items, "HtmlBody", "TextBody");
        }

        public static int SendServiceActivatedNotification(int serviceId)
        {
            Hashtable items = new Hashtable();
            //
            Service serviceInfo = ServiceController.GetService(serviceId);
            //
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(serviceInfo.ContractId);

            items["Service"] = serviceInfo;
            items["Customer"] = account;

            return SendSystemNotification(StoreSettings.SERVICE_ACTIVATED, account, items, "HtmlBody", "TextBody");
        }

        public static int SendServiceSuspendedNotification(int serviceId)
        {
            Hashtable items = new Hashtable();

            Service serviceInfo = ServiceController.GetService(serviceId);
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(serviceInfo.ContractId);

            items["Service"] = serviceInfo;
            items["Customer"] = account;

            return SendSystemNotification(StoreSettings.SERVICE_SUSPENDED, account, items, "HtmlBody", "TextBody");
        }

        public static int SendServiceCanceledNotification(int serviceId)
        {
            Hashtable items = new Hashtable();

            Service serviceInfo = ServiceController.GetService(serviceId);
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(serviceInfo.ContractId);

            items["Service"] = serviceInfo;
            items["Customer"] = account;

            return SendSystemNotification(StoreSettings.SERVICE_CANCELLED, account, items, "HtmlBody", "TextBody");
        }

        public static int SendPaymentReceivedNotification(CustomerPayment payment)
        {
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(payment.ContractId);

            Hashtable items = new Hashtable();
            items["Payment"] = payment;
            items["Customer"] = account;

            return SendSystemNotification(StoreSettings.PAYMENT_RECEIVED, account, items, "HtmlBody", "TextBody");
        }

        protected static int SendSystemNotification(string settingsName, ContractAccount recipient, Hashtable items,
            string htmlKeyName, string textKeyName)
        {
            // load e-mail template
            StoreSettings settings = StorehouseController.GetStoreSettings(ES.SecurityContext.User.UserId,
                settingsName);
            //
            bool htmlMail = (recipient[ContractAccount.MAIL_FORMAT] == "HTML");
            string email = recipient[ContractAccount.EMAIL];
            //
            string messageBody = htmlMail ? settings[htmlKeyName] : settings[textKeyName];

            Template tmp = new Template(messageBody);

            if (items != null)
            {
                foreach (string key in items.Keys)
                    tmp[key] = items[key];
            }

            StringWriter writer = new StringWriter();
            try
            {
                tmp.Evaluate(writer);
            }
            catch (ParserException ex)
            {
                writer.WriteLine(String.Format("Error in template (Line {0}, Column {1}): {2}",
                    ex.Line, ex.Column, ex.Message));
            }
            // evaluate message body
            messageBody = writer.ToString();

            return ES.MailHelper.SendMessage(settings["From"], email, settings["CC"], settings["Subject"],
                messageBody, htmlMail);
        }
    }
}
