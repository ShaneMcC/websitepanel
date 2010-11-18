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
using System.IO;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using WebsitePanel.Ecommerce.EnterpriseServer;
using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.Portal;

namespace WebsitePanel.WebPortal.Services.Ecommerce
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public abstract class ServiceHandlerBase : IHttpHandler
	{
		#region Variables

		private string ServiceHandlerName;
		private bool RedirectRequired;
		private KeyValueBunch handlerSettings;

		#endregion

		#region Constants

		/// <summary>
		/// Order complete uri
		/// </summary>
		public const string ORDER_COMPLETE_URI = "/Default.aspx?pid=ecOrderComplete";
		public const string REDIRECT_URL = "RedirectUrl";
		public const string CONTRACT_ID = "ContractId";
		public const string INVOICE_ID = "InvoiceId";

		#endregion

		#region Properties

		public virtual bool IsReusable
		{
			get { return true; }
		}

		#endregion

		#region Events

		public event EventHandler PreProcessRequest;
		public event EventHandler PostProcessRequest;

		#endregion

		protected ServiceHandlerBase(string serviceHandlerName, bool redirectRequired)
		{
			//
			if (String.IsNullOrEmpty(serviceHandlerName))
				throw new ArgumentNullException("serviceHandlerName");
			//
			RedirectRequired = redirectRequired;
			//
			ServiceHandlerName = serviceHandlerName;
			//
			handlerSettings = new KeyValueBunch();
		}

		protected void RaisePreProcessRequestEvent(HttpContext sender)
		{
			if (PreProcessRequest != null)
				PreProcessRequest(sender, EventArgs.Empty);
		}

		protected void RaisePostProcessRequestEvent(HttpContext sender)
		{
			if (PostProcessRequest != null)
				PostProcessRequest(sender, EventArgs.Empty);
		}

		protected virtual string ProcessServiceHandlerRequest(HttpContext context)
		{
			//
			string serviceResponse = String.Empty;
			//
			using (StreamReader sr = new StreamReader(context.Request.InputStream))
			{
				serviceResponse = sr.ReadToEnd();
			}
			// Decode service response
			return HttpUtility.UrlDecode(serviceResponse);
		}

		private void InitializeServiceHandler(HttpContext context)
		{
			string targetSite = context.Request.Form[CheckoutFormParams.TARGET_SITE];
			// check target_site variable
			if (!String.IsNullOrEmpty(targetSite))
				targetSite = String.Concat(targetSite, ORDER_COMPLETE_URI);
			else
				targetSite = EcommerceSettings.AbsoluteAppPath;
			//
			SetProperty(REDIRECT_URL, targetSite);
		}

		protected virtual void DoTargetSiteRedirect(HttpContext context)
		{
			string targetSite = GetProperty<string>(REDIRECT_URL);
			// 
			if (!String.IsNullOrEmpty(targetSite))
				context.Response.Redirect(targetSite);
		}

		public T GetProperty<T>(string name)
		{
			return (T)Convert.ChangeType(handlerSettings[name], typeof(T));
		}

		public void SetProperty(string name, object value)
		{
			handlerSettings[name] = Convert.ToString(value);
		}

		public void ProcessRequest(HttpContext context)
		{
			// Do service handler initialization
			InitializeServiceHandler(context);

			// Do request pre-processing
			RaisePreProcessRequestEvent(context);

			// Do service handler processing
			string dataReceived = ProcessServiceHandlerRequest(context);
			string contractId = GetProperty<string>(CONTRACT_ID);
			int invoiceId = GetProperty<int>(INVOICE_ID);
			//
			if (String.IsNullOrEmpty(contractId))
				contractId = null;
			
			// Do service response submit
			EC.Services.ServiceHandler.AddServiceHandlerTextResponse(
				ServiceHandlerName, contractId, invoiceId, dataReceived);

			// Do request post-processing
			RaisePostProcessRequestEvent(context);

			// Do redirect if required
			if (RedirectRequired)
				DoTargetSiteRedirect(context);
		}
	}
}