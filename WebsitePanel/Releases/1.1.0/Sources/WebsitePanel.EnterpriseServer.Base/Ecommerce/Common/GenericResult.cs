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
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
    public class SystemTasks
    {
		public const string TASK_SET_PAYMENT_PROFILE = "SET_PAYMENT_PROFILE";
		public const string TASK_ADD_INVOICE = "ADD_INVOICE";
        public const string TASK_ADD_PAYMENT = "ADD_PAYMENT";
        public const string TASK_ADD_CONTRACT = "ADD_CONTRACT";
        public const string TASK_UPDATE_PAYMENT = "UPDATE_PAYMENT";
        public const string TASK_EXEC_SYSTEM_TRIGGER = "EXEC_SYSTEM_TRIGGER";
        public const string SVC_ACTIVATE = "SVC_ACTIVATE";
        public const string SVC_SUSPEND = "SVC_SUSPEND";
        public const string SVC_CANCEL = "SVC_CANCEL";

        public const string SOURCE_ECOMMERCE = "ECOMMERCE";
		public const string SOURCE_SPF = "SPF";
    }

    public class SystemTaskParams
    {
        public const string PARAM_SERVICE = "Service";
        public const string PARAM_PAYMENT = "Payment";
        public const string PARAM_INVOICE = "Invoice";
        public const string PARAM_CONTRACT = "Contract";
		public const string PARAM_CUSTOMER_ID = "CustomerId";
        public const string PARAM_CONTRACT_ACCOUNT = "ContractAccount";
        public const string PARAM_INVOICE_LINES = "InvoiceLines";
        public const string PARAM_EXTRA_ARGS = "ExtraArgs";
		public const string PARAM_SKIP_HANDLERS = "SkipHandlers";
		public const string PARAM_SEND_EMAIL = "SendEmail";
    }

    [Serializable]
    public class GenericResult : KeyValueBunchBase
    {
        private bool succeed;

        public bool Succeed
        {
            get { return succeed; }
            set { succeed = value; }
        }
    }

    [Serializable]
    public abstract class KeyValueBunchBase : IKeyValueBunch
    {
        private string propertyNames;
        private string propertyValues;

        [XmlIgnore]
        public string PropertyNames
        {
            get { return this.propertyNames; }
            set { this.propertyNames = value; }
        }

        [XmlIgnore]
        public string PropertyValues
        {
            get { return this.propertyValues; }
            set { this.propertyValues = value; }
        }

        public bool PropertyExists(string name)
        {
            string keyValue = Properties.Get(name);
            return keyValue != null;
        }

        public T GetProperty<T>(string name)
        {
            return (T)Convert.ChangeType(Properties[name], typeof(T));
        }

		public void SetProperty(string name, object value)
		{
			this[name] = Convert.ToString(value);
		}

        #region IKeyValueBunch Members

        public string this[string settingName]
        {
            get
            {
                return Properties[settingName];
            }
            set
            {
                // set setting
                Properties[settingName] = value;
                //
                SyncCollectionsState(false);
            }
        }

        public string[] GetAllKeys()
        {
            if (Properties != null)
                return Properties.AllKeys;

            return null;
        }

        #endregion

        private NameValueCollection properties;

        public string[][] KeyValueArray;

        [XmlIgnore]
        NameValueCollection Properties
        {
            get
            {
                //
                SyncCollectionsState(true);
                //
                return properties;
            }
        }

        private void SyncCollectionsState(bool inputSync)
        {
            if (inputSync)
            {
                if (properties == null)
                {
                    // create new dictionary
                    properties = new NameValueCollection();

                    // fill dictionary
                    if (KeyValueArray != null)
                    {
                        foreach (string[] pair in KeyValueArray)
                            properties.Add(pair[0], pair[1]);
                    }
                }
            }
            else
            {
                // rebuild array
                KeyValueArray = new string[Properties.Count][];
                //
                for (int i = 0; i < Properties.Count; i++)
                {
                    KeyValueArray[i] = new string[] { Properties.Keys[i], Properties[Properties.Keys[i]] };
                }
            }
        }
    }
}
