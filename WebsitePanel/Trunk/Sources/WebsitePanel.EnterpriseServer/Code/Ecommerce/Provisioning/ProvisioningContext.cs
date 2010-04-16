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
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	/// <summary>
	/// Represents context for provisioning controllers
	/// </summary>
	public class ProvisioningContext
	{
        private Contract contractInfo;
		private ContractAccount consumerInfo;
		private Service serviceInfo;
		private Service parentSvcInfo;
		private bool sendEmail;

		public bool SendEmail
		{
			get { return sendEmail; }
			set { sendEmail = value; }
		}

        /// <summary>
        /// Gets service consumer contract information
        /// </summary>
        public Contract ContractInfo
        {
            get { return contractInfo; }
        }

		/// <summary>
		/// Gets service consumer information
		/// </summary>
        public ContractAccount ConsumerInfo
		{
			get { return consumerInfo; }
		}
		/// <summary>
		/// Get service information
		/// </summary>
		public Service ServiceInfo
		{
			get { return serviceInfo; }
		}
		/// <summary>
		/// Get parent service information
		/// </summary>
		public Service ParentSvcInfo
		{
			get { return parentSvcInfo; }
		}
		/// <summary>
		/// Ctor.
		/// </summary>
		/// <param name="service"></param>
		/// <param name="consumer"></param>
		public ProvisioningContext(Contract contract, Service service, ContractAccount consumer, Service parentSvc)
		{
            this.contractInfo = contract;
			this.serviceInfo = service;
			this.consumerInfo = consumer;
			this.parentSvcInfo = parentSvc;
		}
	}
}
