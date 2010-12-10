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

using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.PaymentMethods
{
	public partial class PPAccount_Payment : ecControlBase, IPaymentMethod
	{
		#region IPaymentMethod Members

		public void CheckSupportedItems(string supportedItems)
		{
			// nothing to check...
		}

		public void LoadContractAccount(ContractAccount account)
		{
            litAddress.Text = account[ContractAccount.ADDRESS];
            litCity.Text = account[ContractAccount.CITY];
            ltrCompany.Text = account[ContractAccount.COMPANY_NAME];
            litCountry.Text = account[ContractAccount.COUNTRY];
            litEmail.Text = account[ContractAccount.EMAIL];
            litFaxNumber.Text = account[ContractAccount.FAX_NUMBER];
            litFirstName.Text = account[ContractAccount.FIRST_NAME];
            litLastName.Text = account[ContractAccount.LAST_NAME];
            litPhoneNumber.Text = account[ContractAccount.PHONE_NUMBER];
            litPostalCode.Text = account[ContractAccount.ZIP];
            litState.Text = account[ContractAccount.STATE];
		}

		public CheckoutDetails GetCheckoutDetails()
		{
			// nothing to get here...
			return null;
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			//
			RegisterClientScript();
			//
			PrepearePayPalCheckoutParams();
		}

		private void RegisterClientScript()
		{
			// load type
			Type typeOf = typeof(PPAccount_Payment);
			// check before
			if (!Page.ClientScript.IsClientScriptIncludeRegistered("EcommerceUtils"))
			{
				Page.ClientScript.RegisterClientScriptInclude("EcommerceUtils",
					Page.ClientScript.GetWebResourceUrl(typeOf, "WebsitePanel.Ecommerce.Portal.Scripts.EcommerceUtils.js"));
			}
		}

		private void PrepearePayPalCheckoutParams()
		{
			// setup checkout options
			KeyValueBunch options = new KeyValueBunch();
			options["target_site"] = EcommerceSettings.AbsoluteAppPath;
			// load checkout params
            CheckoutFormParams fParams = StorefrontHelper.GetCheckoutFormParams(ecPanelRequest.ContractId, 
                ecPanelRequest.InvoiceId, ecPanelRequest.PaymentMethod, options);
			// register all hidden fields
			foreach (string keyName in fParams.GetAllKeys())
			{
				Page.ClientScript.RegisterHiddenField(keyName, fParams[keyName]);
			}
			// build bootstrap javascript
			string bootstrapJs = String.Format(
				"var checkout_form_method = '{0}', checkout_routine_url = '{1}';",
				fParams.Method,
				fParams.Action
			);
			// bootstrap checkout form
			Page.ClientScript.RegisterStartupScript(
				GetType(),
				"BootStrapCheckoutForm",
				bootstrapJs,
				true
			);
		}
	}
}