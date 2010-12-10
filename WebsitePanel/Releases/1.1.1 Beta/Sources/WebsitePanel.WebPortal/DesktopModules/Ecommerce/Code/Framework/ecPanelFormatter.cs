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

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
    /// <summary>
    /// Summary description for PanelFormatter.
    /// </summary>
    public class ecPanelFormatter
    {
        public static string GetBooleanName(bool value)
        {
            return ecGetLocalizedString(String.Concat("Boolean.", value));
        }

		public static string GetTransactionStatusName(object status)
		{
			return ecGetLocalizedString(String.Concat("TransactionStatus.", status));
		}

		public static string GetSvcItemTypeName(object value)
		{
			return ecGetLocalizedString(String.Concat("ProductType.", value));
		}

        public static string GetServiceStatusName(object status)
        {
            return ecGetLocalizedString(String.Concat("ServiceStatus.", status));
        }

		public static string GetTaxationType(object value)
		{
			return ecGetLocalizedString(String.Concat("TaxationType.", ((TaxationType)value).ToString()));
		}

		public static string GetTaxationFormat(object format, object value)
		{
			switch ((TaxationType)format)
			{
				case TaxationType.Fixed:
					return String.Format("{0} {1:C}", EcommerceSettings.CurrencyCodeISO, value);
				case TaxationType.Percentage:
				case TaxationType.TaxIncluded:
					return String.Format("{0:0.00}%", value);
			}
			//
			return String.Empty;
		}

        public static string ecGetLocalizedString(string key)
        {
            return PortalUtils.GetSharedLocalizedString(Keys.ModuleName, key);
        }

		public static string GetTaxationStatus(bool value)
		{
			return ecGetLocalizedString(String.Concat("TaxationStatus.", value.ToString()));
		}

		public static string GetLastModified(object value)
		{
			if (DateTime.Equals(value, DateTime.MinValue))
				return "&nbsp;";

			return Convert.ToString(value);
		}
    }
}
