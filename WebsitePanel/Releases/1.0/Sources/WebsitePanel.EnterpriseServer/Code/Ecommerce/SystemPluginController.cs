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
using System.Reflection;
using System.Collections.Generic;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class SystemPluginController
	{
		#region Ecommerce v 2.1.0

        /// <summary>
        /// Gets contracts' payment provider
        /// </summary>
        /// <param name="contractInfo"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        internal static SystemPluginBase GetContractPaymentMethod(Contract contractInfo, string methodName)
        {
            // Load supported plugin
            SupportedPlugin plugin = GetResellerPMPlugin(contractInfo.ResellerId, methodName);
            //
            return GetSystemPluginInstance(contractInfo, plugin, true);
        }

		internal static void SetupSystemPlugin(int resellerId, SystemPluginBase pluginObj)
		{
			// load plugin settings
			pluginObj.PluginSettings = GetPluginPropertiesInternal(resellerId, pluginObj.PluginId,
				false, pluginObj.SecureSettings);
			// check whether plugin settings should be decrypted
			if (pluginObj.SecureSettings != null && pluginObj.SecureSettings.Length > 0)
			{
				// decrypt secret settings
				foreach (string keyName in pluginObj.SecureSettings)
				{
					// call WebsitePanel Crypto API
					pluginObj.PluginSettings[keyName] = CryptoUtils.Decrypt(pluginObj.PluginSettings[keyName]);
				}
			}
		}

		internal static SystemPluginBase GetSystemPluginInstance(string contractId, SupportedPlugin plugin,
			bool setupPlugin)
		{
			Contract contract = ContractSystem.ContractController.GetContract(contractId);
			//
			return GetSystemPluginInstance(contract, plugin, setupPlugin);
		}

		internal static SystemPluginBase GetSystemPluginInstance(Contract contract, SupportedPlugin plugin, 
			bool setupPlugin)
		{
			// load plugin
			SystemPluginBase pluginObj = GetSystemPluginInstance(plugin);
			//
			if (setupPlugin)
				SetupSystemPlugin(contract.ResellerId, pluginObj);
			//
			return pluginObj;
		}

		internal static SystemPluginBase GetSystemPluginInstance(int resellerId, int pluginId, bool setupPlugin)
		{
			// load plugin
			SystemPluginBase pluginObj = GetSystemPluginInstance(pluginId);
			//
			if (setupPlugin)
				SetupSystemPlugin(resellerId, pluginObj);
			//
			return pluginObj;
		}

		internal static SystemPluginBase GetSystemPluginInstance(string contractId, int pluginId, bool setupPlugin)
		{
			Contract contract = ContractSystem.ContractController.GetContract(contractId);
			// load plugin
			SystemPluginBase pluginObj = GetSystemPluginInstance(pluginId);
			//
			if (setupPlugin)
				SetupSystemPlugin(contract.ResellerId, pluginObj);
			//
			return pluginObj;
		}

		internal static SystemPluginBase GetSystemPluginInstance(int pluginId)
		{
			// load plugin
			SupportedPlugin plugin = GetSupportedPluginById(pluginId);
			//
			return GetSystemPluginInstance(plugin);
		}

		internal static SystemPluginBase GetSystemPluginInstance(SupportedPlugin plugin)
		{
			// check plugin not empty
			if (plugin == null)
				throw new ArgumentNullException("plugin");
			// create system plugin instance
			SystemPluginBase pluginObj = (SystemPluginBase)Activator.CreateInstance(Type.GetType(plugin.TypeName));
			// copy fields
			pluginObj.PluginId = plugin.PluginId;
			pluginObj.PluginName = plugin.PluginName;
			// return
			return pluginObj;
		}

		internal static SupportedPlugin GetResellerPMPlugin(int resellerId, string methodName)
		{
            SupportedPlugin plugin = ObjectUtils.FillObjectFromDataReader<SupportedPlugin>(
				EcommerceProvider.GetResellerPMPlugin(resellerId, methodName));
            //
            if (plugin == null)
                throw new Exception("RESELLER_PM_PLUGIN_NOT_FOUND");
            //
            return plugin;
		}

		internal static KeyValueBunch GetPluginPropertiesInternal(int userId, int pluginId, bool skipSensitive, 
			string[] secureSettings)
		{
			// load plugin settings
			KeyValueBunch properties = SettingsHelper.FillProperties(EcommerceProvider.GetPluginProperties(
				SecurityContext.User.UserId, userId, pluginId));
			// cleanup plugin settings
			if (skipSensitive && secureSettings != null)
			{
				// iterate through secure settings
				foreach (string keyName in secureSettings)
				{
					// remove secure setting
					properties.RemoveKey(keyName);
				}
			}
			// return plugin settings
			return properties;
		}

		public static KeyValueBunch GetPluginProperties(int userId, int pluginId)
		{
			// get plugin
			SystemPluginBase pluginObj = GetSystemPluginInstance(pluginId);
			// get settings
			return GetPluginPropertiesInternal(userId, pluginId, true, pluginObj.SecureSettings);
		}

		public static int SetPluginProperties(int userId, int pluginId, KeyValueBunch props)
		{
			string xmlText = String.Empty;
			string[][] pluginProps = null;
			SecurityResult result = StorehouseController.CheckAccountNotDemoAndActive();
			//
			if (!result.Success)
				return result.ResultCode;
			if (props != null)
			{
				// create system plugin
				SystemPluginBase pluginObj = GetSystemPluginInstance(userId, pluginId, false);
				// crypt sensitive data
				foreach (string keyName in props.GetAllKeys())
				{
					//
					string keyValue = props[keyName];
					//
					if (pluginObj.SecureSettings != null)
					{
						int indexOf = Array.IndexOf(pluginObj.SecureSettings, keyName);
						// crypt sensitive data
						if (indexOf > -1)
							keyValue = CryptoUtils.Encrypt(keyValue);
					}
					//
					props[keyName] = keyValue;
				}

				// load old properties
				KeyValueBunch oldProps = GetPluginPropertiesInternal(userId, pluginId,
					false, pluginObj.SecureSettings);
				// merge old props with new props
				foreach (string keyName in props.GetAllKeys())
				{
					// copy
					oldProps[keyName] = props[keyName];
				}
				//
				pluginProps = oldProps.KeyValueArray;
			}
			// build update xml
			xmlText = SettingsHelper.ConvertObjectSettings(pluginProps, "properties", "property");
			//
			return EcommerceProvider.SetPluginProperties(SecurityContext.User.UserId, userId, pluginId, xmlText);
		}

        public static int LogContractPayment(Contract contract, string methodName, string rawData)
        {
            SupportedPlugin plugin = GetResellerPMPlugin(contract.ResellerId, methodName);
            //
            return EcommerceProvider.WriteSupportedPluginLog(contract.ContractId, plugin.PluginId, 0, rawData);
        }

		private static int WriteSupportedPluginLog(string contractId, int pluginId, int recordType, string rawData)
		{
			return EcommerceProvider.WriteSupportedPluginLog(contractId, pluginId, recordType, rawData);
		}

		public static List<SupportedPlugin> GetSupportedPluginsByGroup(string groupName)
		{
			return ObjectUtils.CreateListFromDataReader<SupportedPlugin>(
				EcommerceProvider.GetSupportedPluginsByGroup(groupName));
		}

		public static SupportedPlugin GetSupportedPlugin(string pluginName, string groupName)
		{
			return ObjectUtils.FillObjectFromDataReader<SupportedPlugin>(
				EcommerceProvider.GetSupportedPlugin(pluginName, groupName));
		}

		public static SupportedPlugin GetSupportedPluginById(int pluginId)
		{
			return ObjectUtils.FillObjectFromDataReader<SupportedPlugin>(
				EcommerceProvider.GetSupportedPluginById(pluginId));
		}

		internal static CheckoutFormParams GetCheckoutFormParams(string contractId, int invoiceId, 
			string methodName, KeyValueBunch options)
		{
			Contract contractInfo = ContractSystem.ContractController.GetContract(contractId);
            // Impersonate
            ContractSystem.ContractController.ImpersonateAsContractReseller(contractInfo);
			//
			SupportedPlugin pmPlugin = GetResellerPMPlugin(contractInfo.ResellerId, methodName);
			//
			if (pmPlugin == null)
				throw new Exception("Incorrect payment method has been specified");
			// create instance of plugin
			IInteractivePaymentGatewayProvider provider = (IInteractivePaymentGatewayProvider)
				SystemPluginController.GetSystemPluginInstance(contractInfo, pmPlugin, true);
			//
			Invoice userInvoice = InvoiceController.GetCustomerInvoiceInternally(invoiceId);
			//
			List<InvoiceItem> invoiceLines = InvoiceController.GetCustomerInvoiceItems(invoiceId);
			// load contract account
            ContractAccount account = ContractSystem.ContractController.GetContractAccountSettings(contractId);

			// build form parameters
			FormParameters formParams = new FormParameters();
			// copy reseller id
			formParams[FormParameters.CONTRACT] = userInvoice.ContractId;
			// copy invoice number
			formParams[FormParameters.INVOICE] = userInvoice.InvoiceId.ToString();
			// copy invoice amount
			formParams[FormParameters.AMOUNT] = userInvoice.Total.ToString("0.00");
			// copy invoice tax amount
			formParams[FormParameters.TAX_AMOUNT] = userInvoice.TaxAmount.ToString("0.00");
			// copy invoice currency
			formParams[FormParameters.CURRENCY] = userInvoice.Currency;

			// copy first name
            formParams[FormParameters.FIRST_NAME] = account[ContractAccount.FIRST_NAME];
			// copy last name
            formParams[FormParameters.LAST_NAME] = account[ContractAccount.LAST_NAME];
			// copy email
            formParams[FormParameters.EMAIL] = account[ContractAccount.EMAIL];
			// copy address
            formParams[FormParameters.ADDRESS] = account[ContractAccount.ADDRESS];
			// copy country
            formParams[FormParameters.COUNTRY] = account[ContractAccount.COUNTRY];
			// copy phone number
            formParams[FormParameters.PHONE] = account[ContractAccount.PHONE_NUMBER];
			// copy city
            formParams[FormParameters.CITY] = account[ContractAccount.CITY];
			// copy state
            formParams[FormParameters.STATE] = account[ContractAccount.STATE];
			// copy zip
            formParams[FormParameters.ZIP] = account[ContractAccount.ZIP];
			// copy options if any
			if (options != null)
			{
				foreach (string keyName in options.GetAllKeys())
					formParams[keyName] = options[keyName];
			}

			// return result
			return provider.GetCheckoutFormParams(formParams, invoiceLines.ToArray());
		}

		#endregion
	}
}