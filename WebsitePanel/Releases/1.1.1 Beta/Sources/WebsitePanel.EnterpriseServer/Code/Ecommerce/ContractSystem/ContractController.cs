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
using System.Data;

using ES = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer.ContractSystem
{
    public class ContractController
    {
        public static GenericResult AddContract(int resellerId, ContractAccount accountSettings)
        {
            try
            {
                ES.SecurityContext.SetThreadPrincipal(resellerId);
                //
                ES.TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.TASK_ADD_CONTRACT);
                //
                GenericResult result = new GenericResult();
				//
				int customerId = -1;
				int contractStatus = (int)ContractStatus.Pending;
				//
				if (accountSettings.PropertyExists(ContractAccount.CUSTOMER_ID))
				{
					customerId = accountSettings.GetProperty<int>(ContractAccount.CUSTOMER_ID);
					//
					contractStatus = (int)ContractStatus.Active;
				}

				// Ensure customer not specified and then check requested username availability
				if (customerId == -1)
				{
					if (ES.UserController.UserExists(accountSettings[ContractAccount.USERNAME]))
					{
						result.Succeed = false;
						result.SetProperty("ResultCode", ES.BusinessErrorCodes.ERROR_USER_ALREADY_EXISTS);
						return result;
					}
					// EXIT
				}
				//
                string strNames = null;
				string strValues = null;
				//
				if (customerId == -1)
				{
					strNames = strValues = String.Empty;
					SecurityUtils.SerializeGenericProfile(ref strNames, ref strValues, accountSettings);
				}
                // emit the new contract
                string contractId = EcommerceProvider.AddContract(customerId, resellerId, accountSettings[ContractAccount.USERNAME],
					contractStatus, 0m, accountSettings[ContractAccount.FIRST_NAME], accountSettings[ContractAccount.LAST_NAME],
                    accountSettings[ContractAccount.EMAIL], accountSettings[ContractAccount.COMPANY_NAME],
                    strNames, strValues);
                //
                result.Succeed = true;
                result.SetProperty("ContractId", contractId);
                // Add contract object
                ES.TaskManager.TaskParameters[SystemTaskParams.PARAM_CONTRACT] = GetContract(contractId);
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

		public static bool CheckCustomerContractExists()
		{
			return EcommerceProvider.CheckCustomerContractExists(ES.SecurityContext.User.UserId);
		}

        public static GenericResult DeleteContract(string contractId)
        {
            return null;
        }

        public static void ImpersonateAsContractReseller(string contractId)
        {
            Contract contract = ContractSystem.ContractController.GetContract(contractId);
            // Impersonate
            ImpersonateAsContractReseller(contract);
        }

        public static void ImpersonateAsContractReseller(Contract contractInfo)
        {
            // Impersonate
            ES.SecurityContext.SetThreadPrincipal(contractInfo.ResellerId);
        }

        public static Contract GetCustomerContract(int customerId)
        {
            Contract contractInfo = ES.ObjectUtils.FillObjectFromDataReader<Contract>(
                EcommerceProvider.GetCustomerContract(customerId));
            //
            if (contractInfo == null)
                throw new Exception("Could not find customer contract.");
            //
            return contractInfo;
        }

		public static Contract GetContract(string contractId)
		{
			Contract contractInfo = ES.ObjectUtils.FillObjectFromDataReader<Contract>(EcommerceProvider.GetContract(contractId));
			//
			if (contractInfo == null)
				throw new Exception("Could not find the contract specified. ContractID: " + contractId);
			//
			return contractInfo;
		}

        public static int UpdateContract(string contractId, int customerId, string accountName, 
            ContractStatus status, decimal balance, string firstName, string lastName, string email, 
            string companyName, string propertyNames, string propertyValues)
        {
            return EcommerceProvider.UpdateContract(contractId, customerId, accountName, (int)status, balance,
                firstName, lastName, email, companyName, propertyNames, propertyValues);
        }

		public static ContractAccount GetContractAccountSettings(string contractId)
		{
			return GetContractAccountSettings(contractId, false);
		}

        public static ContractAccount GetContractAccountSettings(string contractId, bool internally)
        {
            //
            ContractAccount account = new ContractAccount();
            //
            IDataReader dr = null;
            //
            try
            {
                int customerId = -1;
                dr = EcommerceProvider.GetContract(contractId);
                //
                if (dr.Read())
                {
                    string propertyNames = Convert.ToString(dr["PropertyNames"]);
                    string propertyValues = Convert.ToString(dr["PropertyValues"]);
                    if (dr["CustomerID"] != DBNull.Value)
                        customerId = Convert.ToInt32(dr["CustomerID"]);
                    else
                        SecurityUtils.DeserializeGenericProfile(propertyNames, propertyValues, account);
                }
                //
                if (customerId > -1)
                {
					ES.UserInfo userInfo = (internally) ? ES.UserController.GetUserInternally(customerId) : 
						ES.UserController.GetUser(customerId);
					//
					if (internally)
						account[ContractAccount.PASSWORD] = userInfo.Password;
					//
                    account[ContractAccount.USERNAME] = userInfo.Username;
                    account[ContractAccount.FIRST_NAME] = userInfo.FirstName;
                    account[ContractAccount.LAST_NAME] = userInfo.LastName;
                    account[ContractAccount.EMAIL] = userInfo.Email;
                    account[ContractAccount.COMPANY_NAME] = userInfo.CompanyName;
                    account[ContractAccount.COUNTRY] = userInfo.Country;
                    account[ContractAccount.CITY] = userInfo.City;
                    account[ContractAccount.ADDRESS] = userInfo.Address;
                    account[ContractAccount.FAX_NUMBER] = userInfo.Fax;
                    account[ContractAccount.INSTANT_MESSENGER] = userInfo.InstantMessenger;
                    account[ContractAccount.PHONE_NUMBER] = userInfo.PrimaryPhone;
                    account[ContractAccount.STATE] = userInfo.State;
                    account[ContractAccount.ZIP] = userInfo.Zip;
                    account[ContractAccount.MAIL_FORMAT] = userInfo.HtmlMail ? "HTML" : "PlainText";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
            //
            return account;
        }
    }
}