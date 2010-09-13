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
using System.Collections;
using System.Collections.Generic;

using ES = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
    public class ActivateInvoiceTrigger : TriggerHandlerBase
    {
        public const string NAMESPACE = "INVOICE";
        public const string STATUS_AWAITING_PAYMENT = "AWAITING_PAYMENT";

        public override string TriggerNamespace
        {
            get { return NAMESPACE; }
        }

        public override void ExecuteTrigger(TriggerEventArgs eventArgs)
        {
            //
            try
            {
                ActivatePaidInvoicesTask activator = new ActivatePaidInvoicesTask();
                // Load invoice items to activate
                List<InvoiceItem> items = InvoiceController.GetCustomerInvoiceItems(Convert.ToInt32(ReferenceId));
                //
                foreach (InvoiceItem item in items)
                {
                    try
                    {
                        ES.TaskManager.Write("Activating service");
                        // activating
                        GenericSvcResult result = activator.ActivateInvoiceItem(item);
                        // LOG ERROR
                        if (!result.Succeed)
                        {
                            ES.TaskManager.WriteError(result.Error);
                            if (!String.IsNullOrEmpty(result.ErrorCode))
                                ES.TaskManager.WriteParameter("Error code", result.ErrorCode);
                            ES.TaskManager.WriteParameter("Result code", result.ResultCode);
                            // go to next item
                            continue;
                        }
                        //
                        ES.TaskManager.Write("Activated");
                    }
                    catch (Exception ex)
                    {
                        ES.TaskManager.WriteError(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                TriggerStatus = "ERROR";
                ES.TaskManager.WriteError(ex);
            }
        }
    }

    public class ActivateContractTrigger : TriggerHandlerBase
    {
        public const string NAMESPACE = "CONTRACT";
        public const string STATUS_AWAITING_PAYMENT = "AWAITING_PAYMENT";

        public override string TriggerNamespace
        {
            get { return NAMESPACE; }
        }

        public override void ExecuteTrigger(TriggerEventArgs eventArgs)
        {
            //
            try
            {
                Contract contract = ContractSystem.ContractController.GetContract(ReferenceId);
                //
                if (contract.Status == ContractStatus.Pending)
                {
                    //
                    ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(ReferenceId);
                    //
                    // create user account
                    ES.UserInfo userInfo = new ES.UserInfo();
                    userInfo.Username = account[ContractAccount.USERNAME];
                    userInfo.Password = account[ContractAccount.PASSWORD];
                    userInfo.Email = account[ContractAccount.EMAIL];
                    userInfo.FirstName = account[ContractAccount.FIRST_NAME];
                    userInfo.LastName = account[ContractAccount.LAST_NAME];
                    userInfo.HtmlMail = (account[ContractAccount.MAIL_FORMAT] == "HTML");
                    userInfo.Address = account[ContractAccount.ADDRESS];
                    userInfo.City = account[ContractAccount.CITY];
                    userInfo.CompanyName = account[ContractAccount.COMPANY_NAME];
                    userInfo.Country = account[ContractAccount.COUNTRY];
                    userInfo.State = account[ContractAccount.STATE];
                    userInfo.PrimaryPhone = account[ContractAccount.PHONE_NUMBER];
                    userInfo.Fax = account[ContractAccount.FAX_NUMBER];
                    userInfo.InstantMessenger = account[ContractAccount.INSTANT_MESSENGER];
                    userInfo.Zip = account[ContractAccount.ZIP];
                    userInfo.Role = ES.UserRole.User;
                    userInfo.Status = ES.UserStatus.Active;
                    // set account parent
                    userInfo.OwnerId = contract.ResellerId;
                    userInfo.Created = DateTime.Now;
                    // create account
                    int resultCode = ES.UserController.AddUser(userInfo, true);
                    //
                    if (resultCode > 0)
                    {
                        ContractSystem.ContractController.UpdateContract(ReferenceId, resultCode, contract.AccountName,
                            ContractStatus.Active, 0m, contract.FirstName, contract.LastName, contract.Email,
                            contract.CompanyName, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                TriggerStatus = "ERROR";
                ES.TaskManager.WriteError(ex);
            }
        }
    }
}