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

namespace WebsitePanel.Ecommerce.Portal
{
	public abstract class PagesKeys
	{
		public const string ADDONS = "ecAddons";
		public const string ORDER_CHECKOUT = "ecOrderCheckout";
	}

	public abstract class ModulesKeys
	{
		public const string EDIT_ITEM = "EditItem";
		public const string ADD_ITEM = "AddItem";
		public const string CONFIGURE_ITEM = "ConfigItem";
	}

	public abstract class RequestKeys
	{
        public const string CONTRACT_ID = "ContractId";
		public const string PRODUCT_ID = "ProductId";
		public const string PRODUCT_TYPE_ID = "TypeId";
		public const string PLUGIN_ID = "PluginId";
		public const string RESELLER_ID = "ResellerId";
		public const string USER_ID = "UserId";
		public const string INVOICE_ID = "InvoiceId";
		public const string PAYMENT_METHOD = "Method";
	}

	public abstract class MessagesKeys
	{
		public const string ADD_PRODUCT_ADDON = "ADD_PRODUCT_ADDON";
		public const string EDIT_PRODUCT_ADDON = "EDIT_PRODUCT_ADDON";
		public const string DELETE_PRODUCT_ADDON = "DELETE_PRODUCT_ADDON";

		public const string ADD_CATEGORY = "ADD_CATEGORY";
		public const string EDIT_CATEGORY = "EDIT_CATEGORY";
		public const string DELETE_CATEGORY = "DELETE_CATEGORY";

		public const string ADD_PRODUCT = "ADD_PRODUCT";
		public const string EDIT_PRODUCT = "EDIT_PRODUCT";
		public const string DELETE_PRODUCT = "DELETE_PRODUCT";

		public const string ADD_PRODUCT_TYPE = "ADD_PRODUCT_TYPE";
		public const string EDIT_PRODUCT_TYPE = "EDIT_PRODUCT_TYPE";
		public const string DELETE_PRODUCT_TYPE = "DELETE_PRODUCT_TYPE";
		public const string LOAD_PRODUCT_TYPE_CONTROLS = "LOAD_PRODUCT_TYPE_CONTROLS";
		public const string LOAD_PRODUCT_TYPE_DEFINITION = "LOAD_PRODUCT_TYPE_DEFINITION";

		public const string ADD_PLUGIN = "ADD_PLUGIN";
		public const string EDIT_PLUGIN = "EDIT_PLUGIN";
		public const string DELETE_PLUGIN = "DELETE_PLUGIN";
		public const string LOAD_PLUGIN = "LOAD_PLUGIN";
		public const string LOAD_PLUGIN_SETTINGS_CONTROL = "LOAD_PLUGIN_SETTINGS_CONTROL";
		public const string SAVE_PLUGIN_SETTINGS = "SAVE_PLUGIN_SETTINGS";

		public const string ADD_TLD_EXTENSION = "ADD_TLD_EXTENSION";
		public const string EDIT_TLD_EXTENSION = "EDIT_TLD_EXTENSION";
		public const string DELETE_TLD_EXTENSION = "DELETE_TLD_EXTENSION";
		public const string LOAD_TLD_EXTENSION = "LOAD_TLD_EXTENSION";
		public const string LOAD_TLD_REGISTRARS = "LOAD_TLD_REGISTRARS";

		public const string ACTIVATE_SERVICE = "ACTIVATE_SERVICE";
		public const string SUSPEND_SERVICE = "SUSPEND_SERVICE";
		public const string CANCEL_SERVICE = "CANCEL_SERVICE";

		public const string SAVE_NOTIFICATION_TEMPLATE = "SAVE_NOTIFICATION_TEMPLATE";
		public const string LOAD_NOTIFICATION_TEMPLATE = "LOAD_NOTIFICATION_TEMPLATE";

		public const string SAVE_SYSTEM_SETTINGS = "SAVE_SYSTEM_SETTINGS";
		public const string SAVE_SIGNUP_SETTINGS = "SAVE_SIGNUP_SETTINGS";
		public const string LOAD_SPACE_SETTINGS = "LOAD_SPACE_SETTINGS";
		public const string CREATE_SHOPPING_SPACE = "CREATE_SHOPPING_SPACE";

		public const string EMPTY_STORE_OWNER = "EMPTY_STORE_OWNER";
		public const string STORE_OWNER_ROLE_MISMATCH = "STORE_OWNER_ROLE_MISMATCH";

		public const string SAVE_INVOICE_TEMPLATE = "SAVE_INVOICE_TEMPLATE";
		public const string LOAD_INVOICE_TEMPLATE = "LOAD_INVOICE_TEMPLATE";

		public const string SAVE_TERMS_AND_CONDS = "SAVE_TERMS_AND_CONDS";
		public const string LOAD_TERMS_AND_CONDS = "LOAD_TERMS_AND_CONDS";

		public const string ADD_INVOICE_PAYMENT = "ADD_INVOICE_PAYMENT";
		public const string ACTIVATE_INVOICE_SERVICES = "ACTIVATE_INVOICE_SERVICES";
	}

	public class Keys
	{
		public const string INVOICE_DIRECT_URL = "InvoiceDirectURL";

		public const string CurrencyCodeISO = "CurrencyCodeISO";
		public const string CurrencySymbol = "CurrencySymbol";

		public const string CustomerProfile = "CustomerProfile";

		#region Action keys

		public const string AddItem = "AddItem";
		public const string DeleteItem = "DeleteItem";
		public const string EditItem = "EditItem";
		public const string GetItem = "GetItem";
        public const string ConfigItem = "ConfigItem";

		#endregion

		#region Product Type keys

		public const string CartExtension = "CartControl";
		public const string ProductExtension = "ProductControl";
		public const string TypeExtension = "TypeControl";
		public const string ControllerExtension = "Controller";

		#endregion

		#region Email Notification settings keys

		public const string FromField = "FromField";
		public const string BccField = "BccField";
		public const string SmtpServer = "SmtpServer";
		public const string SmtpAuthentication = "SmtpAuthentication";
		public const string SmtpUsername = "SmtpUsername";
		public const string SmtpPassword = "SmtpPassword";

		#endregion

		#region Payment keys

		public const string AddPayment = "AddOfflineMode";

		#endregion

		#region Service keys

		public const string ServiceEdit = "EditServiceMode";

		#endregion

		#region Invoice keys

		public const string InvoiceEdit = "EditInvoiceMode";

		#endregion

		#region Gateway Log keys

		public const string GatewayLogEntry = "GatewayLogEntry";

		#endregion

		#region OrderCheckout module keys

		public const string PerformPayment = "PerformPayment";
		public const string PaymentSucceed = "PaymentSucceed";
		public const string PaymentFailed = "PaymentFailed";
		public const string PaymentId = "PaymentId";

		#endregion

		public const string ErrorMessage = "ErrorMessage";

		public const string ParentCategoryId = "ParentCategoryId";

		public const string CacheCryptoKey = "CacheCryptoKey";

		public const string Service = "ServiceId";
		public const string Invoice = "InvoiceId";

		public const string InvoiceDetailsMode = "InvoiceDetailsMode";
		public const string DisplayMode = "DisplayMode";
		public const string InvoiceMode = "InvoiceMode";
		public const string PaymentMode = "PaymentMode";
		public const string ServiceMode = "ServiceMode";

		public const string CartPage = "CartPageTabID";
		public const string CheckoutPage = "CheckoutPage";
		public const string CartModuleMode = "CartMode";
		public const string esOwner = "OwnerID";
		public const string OwnerSpaceId = "OwnerSpaceID";
        public const string OwnerSpace = "OwnerSpace";
		public const string CustomerCart = "CustomerCartID";
		public const string PaymentProvider = "PaymentProvID";
		public const string RegistrationPage = "RegistrationPageTabID";
		public const string UserInfo = "UserInfo";
		public const string UserInfoTemp = "UserInfo_Temp";
		public const string PanelLogin = "WebsitePanelLogin";

		public const string ContactInfo = "ContactInfo";
		public const string SignInMode = "SignInMode";
		public const string CreateAccountMode = "CreateAccountMode";

		public const string CheckoutMode = "CheckoutMode";

		public const string AddGateway = "AddGateway";
		public const string EditGateway = "EditGateway";
		public const string ConfigureGateway = "ConfigGateway";

		public const string Gateway = "GatewayId";

		public const string ModuleName = "Ecommerce";

		public const int DefaultInt = -1;

		#region Payment Processor's key set

		public const string Processor = "PaymentProcessor";

		#endregion

		private Keys()
		{
		}
	}


	/// <summary>
	/// Authorize.NET Payment Provider keys set
	/// </summary>
	public class AuthNetKeys
	{
		/// <summary>
		/// 3.1
		/// </summary>
		public const string Version = "x_version";
		/// <summary>
		/// True
		/// </summary>
		public const string DelimData = "x_delim_data";
		/// <summary>
		/// False
		/// </summary>
		public const string RelayResponse = "x_relay_response";
		/// <summary>
		/// API login ID for the payment gateway account
		/// </summary>
		public const string Account = "x_login";
		/// <summary>
		/// Transaction key obtained from the Merchant Interface
		/// </summary>
		public const string TransactionKey = "x_tran_key";
		/// <summary>
		/// Amount of purchase inclusive of tax
		/// </summary>
		public const string Amount = "x_amount";
		/// <summary>
		/// Customer's card number
		/// </summary>
		public const string CardNumber = "x_card_num";
		/// <summary>
		/// Customer's card expiration date
		/// </summary>
		public const string ExpirationDate = "x_exp_date";
		/// <summary>
		/// Type of transaction (AUTH_CAPTURE, AUTH_ONLY, CAPTURE_ONLY, CREDIT, VOID, PRIOR_AUTH_CAPTURE)
		/// </summary>
		public const string TransactType = "x_type";
		/// <summary>
		/// 
		/// </summary>
		public const string DemoMode = "x_test_request";
		public const string DelimiterChar = "x_delim_char";
		public const string EncapsulationChar = "x_encap_char";
		public const string DuplicateWindow = "x_duplicate_window";

		public const string FirstName = "x_first_name";
		public const string LastName = "x_last_name";
		public const string Company = "x_company";
		public const string Address = "x_address";
		public const string City = "x_city";
		public const string State = "x_state";
		public const string Zip = "x_zip";
		public const string Country = "x_country";
		public const string Phone = "x_phone";
		public const string Fax = "x_fax";
		public const string CustomerId = "x_cust_id";
		public const string CustomerIP = "x_customer_ip";
		public const string CustomerTax = "x_customer_tax_id";
		public const string CustomerEmail = "x_email";
		public const string SendConfirmation = "x_email_customer";
		public const string MerchantEmail = "x_merchant_email";
		public const string InvoiceNumber = "x_invoice_num";
		public const string TransDescription = "x_description";

		public const string CurrencyCode = "x_currency_code";
		public const string PaymentMethod = "x_method";
		public const string RecurringBilling = "x_recurring_billing";
		public const string VerificationCode = "x_card_code";
		public const string AuthorizationCode = "x_auth_code";
		public const string AuthenticationIndicator = "x_authentication_indicator";
		public const string PurchaseOrder = "x_po_num";
		public const string Tax = "x_tax";

		public const string FpHash = "x_fp_hash";
		public const string FpSequence = "x_fp_sequence";
		public const string FpTimestamp = "x_fp_timestamp";

		public const string RelayUrl = "x_relay_url";

		public const string TransactId = "x_trans_id";
		public const string AuthCode = "x_auth_code";

		public const string MD5HashValue = "MD5HashValue";

		public const string ShipToFirstName = "x_ship_to_first_name";
		public const string ShipToLastName = "x_ship_to_last_name";
		public const string ShipToCompany = "x_ship_to_company";
		public const string ShipToAddress = "x_ship_to_address";
		public const string ShipToCity = "x_ship_to_city";
		public const string ShipToState = "x_ship_to_state";
		public const string ShipToZip = "x_ship_to_zip";
		public const string ShipToCountry = "x_ship_to_country";

		private AuthNetKeys()
		{
		}
	}
}