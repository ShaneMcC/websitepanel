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
using WebsitePanel.Portal;

namespace WebsitePanel.Ecommerce.Portal
{
	public abstract class CheckoutBasePage : Page
	{
		/// <summary>
		/// Order complete uri
		/// </summary>
		public const string OrderCompleteUri = "/Default.aspx?pid=ecOrderComplete";
		/// <summary>
		/// Order failed uri
		/// </summary>
		public const string OrderFailedUri = "/Default.aspx?pid=ecOrderFailed";

		private string redirectUrl;
		private string methodName;
		private int invoiceId;
		private string contractKey;

		/// <summary>
		/// Gets invoice number
		/// </summary>
		protected int InvoiceId
		{
			get { return invoiceId; }
			set { invoiceId = value; }
		}

        /// <summary>
		/// Gets redirect url
		/// </summary>
		protected string RedirectUrl
		{
			get { return redirectUrl; }
		}

		protected CheckoutBasePage(string methodName, string contractKey)
		{
			// validate plugin is not empty
			if (String.IsNullOrEmpty(methodName))
				throw new ArgumentNullException("methodName");
			if (String.IsNullOrEmpty(contractKey))
				throw new ArgumentNullException("contractKey");
			// set plugin name
			this.methodName = methodName;
			this.contractKey = contractKey;
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			// start to serve payment processor response
			ServeProcessorResponse();
		}

		protected virtual void ServeProcessorResponse()
		{
			// create an instance
			CheckoutDetails orderDetails = new CheckoutDetails();
			// copy whole request keys
			foreach (string keyName in Request.Form.AllKeys)
			{
				orderDetails[keyName] = Request.Form[keyName];
			}
			// check target_site variable
			if (!String.IsNullOrEmpty(orderDetails[CheckoutFormParams.TARGET_SITE]))
				redirectUrl = orderDetails[CheckoutFormParams.TARGET_SITE];
			else
				redirectUrl = EcommerceSettings.AbsoluteAppPath;

			// process checkout details
			ProcessCheckout(methodName, orderDetails);
		}

		protected virtual void PreProcessCheckout(CheckoutDetails details)
		{
		}

		protected virtual void ProcessCheckout(string methodName, CheckoutDetails details)
		{
			try
			{
				PreProcessCheckout(details);
				// perform order payment
				CheckoutResult result = StorefrontHelper.CompleteCheckout(details[contractKey], invoiceId, methodName, details);
				// post process order result
				PostProcessCheckout(result);
			}
			catch (Exception ex)
			{
				// Output error message into the trace
				Trace.Write("ECOMMERCE", "COMPLETE_CHECKOUT_ERROR", ex);
				// display raw stack trace in case of error
				Response.Write(PortalUtils.GetSharedLocalizedString("Ecommerce", "Error.CHECKOUT_GENERAL_FAILURE"));
			}
		}

		protected virtual void PostProcessCheckout(CheckoutResult result)
		{
			// check order payment result status
			if (result.Succeed)
				// go to the order success page
				Response.Redirect(RedirectUrl + OrderCompleteUri);
			else
				// go to order failed page
				Response.Redirect(RedirectUrl + OrderFailedUri);
		}
	}
}