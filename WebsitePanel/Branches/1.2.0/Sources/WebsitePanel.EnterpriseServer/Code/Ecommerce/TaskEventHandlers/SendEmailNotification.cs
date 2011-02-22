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
using System.Collections.Generic;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer.TaskEventHandlers
{
    public class SendEmailNotification : TaskEventHandler
    {
        public override void OnStart()
        {
            // Nothing to do
        }

        /// <summary>
        /// Sends corresponding notifications.
        /// </summary>
        public override void OnComplete()
        {
            if (!TaskManager.HasErrors)
            {
                switch (TaskManager.TaskName)
                {
					case SystemTasks.SVC_SUSPEND:
					case SystemTasks.SVC_CANCEL:
                    case SystemTasks.SVC_ACTIVATE:
                        SendServiceStatusChangedNotification();
                        break;
                    case SystemTasks.TASK_ADD_INVOICE:
                        SendInvoiceNotification();
                        break;
                    case SystemTasks.TASK_UPDATE_PAYMENT:
                    case SystemTasks.TASK_ADD_PAYMENT:
                        SendPaymentNotification();
                        break;
                    default:
                        break;
                }
            }
        }

        private void SendServiceStatusChangedNotification()
        {
            // send an e-mail notification
            try
            {
				bool sendNotification = (bool)TaskManager.TaskParameters[SystemTaskParams.PARAM_SEND_EMAIL];
				// Ensure notification is required
				if (!sendNotification)
				{
					TaskManager.Write("Notification send is not required, thus skipped");
					return;
				}

                Service service = (Service)TaskManager.TaskParameters[SystemTaskParams.PARAM_SERVICE];
                int smtpResult = 0;
                switch (service.Status)
                {
                    case ServiceStatus.Active:
                        smtpResult = MiscController.SendServiceActivatedNotification(service.ServiceId);
                        break;
                    case ServiceStatus.Suspended:
                        smtpResult = MiscController.SendServiceSuspendedNotification(service.ServiceId);
                        break;
                    case ServiceStatus.Cancelled:
                        smtpResult = MiscController.SendServiceCanceledNotification(service.ServiceId);
                        break;
                }
                //
                if (smtpResult != 0)
                {
                    TaskManager.WriteWarning("Unable to send e-mail notification");
                    TaskManager.WriteParameter("SMTP Status", smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
        }

        private void SendPaymentNotification()
        {
            //
            try
            {
                // Read task parameters
                Invoice invoice = (Invoice)TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE];
                CustomerPayment payment = (CustomerPayment)TaskManager.TaskParameters[SystemTaskParams.PARAM_PAYMENT];
                //
                if (payment.Status == TransactionStatus.Approved)
                {
                    //
                    int smtpResult = MiscController.SendPaymentReceivedNotification(payment);
                    //
                    if (smtpResult != 0)
                    {
                        TaskManager.WriteWarning("Unable to send e-mail notification");
                        TaskManager.WriteParameter("SMTP Status", smtpResult);
                    }
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
        }

        private void SendInvoiceNotification()
        {
            //
            try
            {
                // Read task parameters
                Contract contract = (Contract)TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT];
                Invoice invoice = (Invoice)TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE];
                List<InvoiceItem> invoiceLines = (List<InvoiceItem>)TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE_LINES];
                KeyValueBunch extraArgs = (KeyValueBunch)TaskManager.TaskParameters[SystemTaskParams.PARAM_EXTRA_ARGS];
                // modify invoice direct url
                if (extraArgs != null && !String.IsNullOrEmpty(extraArgs["InvoiceDirectURL"]))
                    extraArgs["InvoiceDirectURL"] += "&InvoiceId=" + invoice.InvoiceId;
                //
                int smtpResult = MiscController.SendNewInvoiceNotification(invoice, invoiceLines, extraArgs);
                //
                if (smtpResult != 0)
                {
                    TaskManager.WriteWarning("Unable to send e-mail notification");
                    TaskManager.WriteParameter("SMTP Status", smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteError(ex);
            }
        }
    }
}
