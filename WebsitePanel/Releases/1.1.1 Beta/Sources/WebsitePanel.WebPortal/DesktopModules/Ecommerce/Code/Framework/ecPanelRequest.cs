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
using System.Web;

namespace WebsitePanel.Ecommerce.Portal
{
	public class ecPanelRequest
	{
		public static int GetInt(string key)
		{
			return GetInt(key, 0);
		}

		public static string GetString(string key)
		{
			return HttpContext.Current.Request[key];
		}

		public static int GetInt(string key, int defaultValue)
		{
			int result = defaultValue;
			try { result = Int32.Parse(HttpContext.Current.Request[key]); }
			catch { /* do nothing */ }
			return result;
		}

		#region E-Commerce module definitions

        public static string ContractId
        {
            get { return GetString("ContractId"); }
        }

		public static int TaxationId
		{
			get { return GetInt("TaxationId"); }
		}

		public static string PaymentMethod
		{
			get { return GetString("Method"); }
		}

		public static int ResellerId
		{
			get { return GetInt("ResellerID"); }
		}

		public static int UserId
		{
			get { return GetInt("UserId"); }
		}

		public static string TLD
		{
			get { return HttpContext.Current.Request["TLD"]; }
		}

		public static int BillingCycleId
		{
			get { return GetInt("CycleId"); }
		}

		public static int ServiceId
		{
			get { return GetInt("ServiceId"); }
		}

		public static int PluginId
		{
			get { return GetInt("PluginId"); }
		}

		public static int ProductTypeID
		{
			get { return GetInt("TypeId"); }
		}

		public static int ProductId
		{
			get { return GetInt("ProductId"); }
		}

		public static int CategoryId
		{
			get { return GetInt("CategoryId"); }
		}

		public static int ParentCategoryId
		{
			get { return GetInt(Keys.ParentCategoryId); }
		}

		public static int AddonId
		{
			get { return GetInt("AddonId"); }
		}

		public static int CartItemId
		{
			get { return GetInt("CartItemId"); }
		}

		public static int GatewayId
		{
			get { return GetInt(Keys.Gateway, Keys.DefaultInt); }
		}

		public static int InvoiceId
		{
			get { return GetInt(Keys.Invoice, Keys.DefaultInt); }
		}

		public static int ActiveServiceId
		{
			get { return GetInt(Keys.Service, Keys.DefaultInt); }
		}

		public static int PaymentId
		{
			get { return GetInt(Keys.PaymentId, Keys.DefaultInt); }
		}
		#endregion
	}
}