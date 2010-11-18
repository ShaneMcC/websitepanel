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
using System.Text;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
    public enum ContractStatus
    {
        Pending = 0,
        Active = 1,
        Suspended = 2,
        Closed = 3
    };

    [Serializable]
    public class ContractAccount : KeyValueBunchBase
    {
        public const string USERNAME = "Username";
        public const string PASSWORD = "Password";
        public const string COMPANY_NAME = "CompanyName";
        public const string FIRST_NAME = "FirstName";
        public const string LAST_NAME = "LastName";
        public const string EMAIL = "Email";
        public const string ADDRESS = "Address";
        public const string CITY = "City";
        public const string COUNTRY = "Country";
        public const string STATE = "State";
        public const string ZIP = "Zip";
        public const string PHONE_NUMBER = "PhoneNumber";
        public const string FAX_NUMBER = "FaxNumber";
        public const string INSTANT_MESSENGER = "InstantMessenger";
        public const string MAIL_FORMAT = "MailFormat";
		public const string CUSTOMER_ID = "CustomerId";
    }

    [Serializable]
    public class Contract
    {
        private string accountName;
        private decimal balance;
        private DateTime closedDate;
        private string companyName;
        private string contractId;
        private int customerId;
        private int resellerId;
        private string email;
        private string firstName;
        private string lastName;
        private DateTime openedDate;
        private int statusId;

        public string AccountName
        {
            get
            {
                return accountName;
            }
            set
            {
                accountName = value;
            }
        }

        public decimal Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        public DateTime ClosedDate
        {
            get
            {
                return closedDate;
            }
            set
            {
                closedDate = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
            }
        }

        public string ContractId
        {
            get
            {
                return contractId;
            }
            set
            {
                contractId = value;
            }
        }

        public int CustomerId
        {
            get
            {
                return customerId;
            }
            set
            {
                customerId = value;
            }
        }

        public int ResellerId
        {
            get
            {
                return resellerId;
            }
            set
            {
                resellerId = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        public DateTime OpenedDate
        {
            get
            {
                return openedDate;
            }
            set
            {
                openedDate = value;
            }
        }

        public int StatusId
        {
            get
            {
                return statusId;
            }
            set
            {
                statusId = value;
            }
        }

        public ContractStatus Status
        {
            get { return (ContractStatus)statusId;}
            set { statusId = (int)value; }
        }
    }
}