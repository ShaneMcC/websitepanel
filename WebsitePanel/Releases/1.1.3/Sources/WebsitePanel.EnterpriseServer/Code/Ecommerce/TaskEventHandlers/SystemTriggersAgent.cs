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
using System.Collections.Specialized;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer.TaskEventHandlers
{
    public class SystemTriggersAgent : TaskEventHandler
    {
        public override void OnStart()
        {
            // Nothing to do
        }

        public override void OnComplete()
        {
            if (!TaskManager.HasErrors)
            {
                switch (TaskManager.TaskName)
                {
                    case SystemTasks.TASK_ADD_INVOICE:
                        RegisterInvoiceActivationTrigger();
                        break;
                    case SystemTasks.TASK_ADD_CONTRACT:
                        RegisterContractActivationTrigger();
                        break;
                    case SystemTasks.TASK_UPDATE_PAYMENT:
                    case SystemTasks.TASK_ADD_PAYMENT:
                        ActivatePaymentSystemTriggers();
                        break;
                }
            }
        }

        private void RegisterInvoiceActivationTrigger()
        {
            // Read contract invoice
            Invoice invoice = (Invoice)TaskManager.TaskParameters[SystemTaskParams.PARAM_INVOICE];
            //
            TriggerSystem.TriggerController.AddSystemTrigger(invoice.InvoiceId.ToString(),
                ActivateInvoiceTrigger.STATUS_AWAITING_PAYMENT, typeof(ActivateInvoiceTrigger));
        }

        private void RegisterContractActivationTrigger()
        {
            // Ensure the contract has been registered successfully
            if (TaskManager.TaskParameters.ContainsKey(SystemTaskParams.PARAM_CONTRACT))
            {
                Contract contract = (Contract)TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT];
                //
				if (contract.Status == ContractStatus.Pending)
				{
					TriggerSystem.TriggerController.AddSystemTrigger(contract.ContractId,
						ActivateContractTrigger.STATUS_AWAITING_PAYMENT, typeof(ActivateContractTrigger));
				}
            }
        }

        private void ActivatePaymentSystemTriggers()
        {
            CustomerPayment payment = (CustomerPayment)TaskManager.TaskParameters[SystemTaskParams.PARAM_PAYMENT];
            Contract contract = (Contract)TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT];

            // Run activate contract trigger if the transaction was approved
            if (payment.Status == TransactionStatus.Approved)
            {
                // Activate contract
                TriggerSystem.TriggerController.ExecuteSystemTrigger(contract.ContractId,
                    ActivateContractTrigger.NAMESPACE, null);
                // Activate invoice
                TriggerSystem.TriggerController.ExecuteSystemTrigger(payment.InvoiceId.ToString(),
                    ActivateInvoiceTrigger.NAMESPACE, null);
            }
        }
    }
}