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
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.PaymentMethods
{
	public partial class Offline_Payment : ecControlBase, IPaymentMethod
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		#region IPaymentMethod Members

		public void CheckSupportedItems(string supportedItems)
		{
			// do nothing
		}

		public void LoadContractAccount(ContractAccount account)
		{
            ltrFirstName.Text = account[ContractAccount.FIRST_NAME];
            ltrLastName.Text = account[ContractAccount.LAST_NAME];
            ltrEmail.Text = account[ContractAccount.EMAIL];
            ltrCompany.Text = account[ContractAccount.COMPANY_NAME];
            ltrAddress.Text = account[ContractAccount.ADDRESS];
            ltrPostalCode.Text = account[ContractAccount.ZIP];
            ltrCity.Text = account[ContractAccount.CITY];
            ltrState.Text = account[ContractAccount.STATE];
            ltrCountry.Text = account[ContractAccount.COUNTRY];
            ltrPhoneNumber.Text = account[ContractAccount.PHONE_NUMBER];
            ltrFaxNumber.Text = account[ContractAccount.FAX_NUMBER];
		}

		public CheckoutDetails GetCheckoutDetails()
		{
			return new CheckoutDetails();
		}

		#endregion
	}
}