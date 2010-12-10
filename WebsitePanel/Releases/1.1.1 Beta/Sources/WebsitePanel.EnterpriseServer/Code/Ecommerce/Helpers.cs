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
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Globalization;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class SettingsHelper
	{
		public static string ConvertObjectSettings(string[][] settings)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("settings");

			foreach (string[] pair in settings)
			{
				XmlElement s_item = doc.CreateElement("setting");
				s_item.SetAttribute("name", pair[0]);
				s_item.SetAttribute("value", pair[1]);
				root.AppendChild(s_item);
			}

			return root.OuterXml;
		}

		public static string ConvertObjectSettings(string[][] settings, string rootName, string childName)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement(rootName);
			// exit
			if (settings == null)
				return root.OuterXml;
			// iterate
			foreach (string[] pair in settings)
			{
				XmlElement s_item = doc.CreateElement(childName);
				s_item.SetAttribute("name", pair[0]);
				s_item.SetAttribute("value", pair[1]);
				root.AppendChild(s_item);
			}
			//
			return root.OuterXml;
		}

		public static string ConvertControlsBunch(string[][] bunch)
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("Controls");

			foreach (string[] pair in bunch)
			{
				XmlElement s_item = doc.CreateElement("Control");
				s_item.SetAttribute("Key", pair[0]);
				s_item.SetAttribute("Src", pair[1]);
				root.AppendChild(s_item);
			}

			return root.OuterXml;
		}

		public static KeyValueBunch FillSettingsBunch(IDataReader reader)
		{
			return FillSettingsBunch(
				reader,
				"SettingName",
				"SettingValue"
			);
		}

		public static T FillSettingsBunch<T>(IDataReader reader, string keyNameColumn, string keyValueColumn)
		{
			Type type = typeof(T);

			KeyValueBunch bunch = (KeyValueBunch)Activator.CreateInstance(type);

			try
			{
				while (reader.Read())
					bunch[(String)reader[keyNameColumn]] = (String)reader[keyValueColumn];
			}
			catch
			{
				bunch = null;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return (T)Convert.ChangeType(bunch, typeof(T));
		}

		public static KeyValueBunch FillSettingsBunch(IDataReader reader, 
			string keyNameColumn, string keyValueColumn)
		{
			KeyValueBunch bunch = null;

			try
			{
				bunch = new KeyValueBunch();

				while (reader.Read())
					bunch[(String)reader[keyNameColumn]] = (String)reader[keyValueColumn];
			}
			catch
			{
				bunch = null;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return bunch;
		}

		public static KeyValueBunch FillControlsBunch(IDataReader reader)
		{
			KeyValueBunch bunch = null;

			try
			{
				bunch = new KeyValueBunch();

				while (reader.Read())
					bunch[(String)reader["ControlKey"]] = (String)reader["ControlSrc"];
			}
			catch
			{
				bunch = null;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return bunch;
		}

		public static KeyValueBunch FillProperties(IDataReader reader)
		{
			KeyValueBunch settings = null;

			try
			{
				settings = new KeyValueBunch();

				while (reader.Read())
				{
					string name = (string)reader["PropertyName"];
					string value = (string)reader["PropertyValue"];
					//
					settings[name] = value;
				}
			}
			catch
			{
				settings = null;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return settings;
		}

		public static KeyValueBunch FillSettings(IDataReader reader)
		{
			KeyValueBunch settings = null;

			try
			{
				settings = new KeyValueBunch();

				while (reader.Read())
				{
					settings[(string)reader["SettingName"]] = (string)reader["SettingValue"];
				}
			}
			catch
			{
				settings = null;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return settings;
		}
	}

	class Currency
	{
		public string ISOCode;
		public string Symbol;
		public decimal Rate;

		public Currency()
		{
		}

		public Currency(string isocode, string symbol, decimal rate)
		{
			this.ISOCode = isocode;
			this.Symbol = symbol;
			this.Rate = rate;
		}
	}

	public class CurrenciesHelper
	{
		private const string ServiceUrl = "http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml";
		public const string HttpCacheKey = "__Currencies";

		private Dictionary<string, Currency> _currencies;

		public CurrenciesHelper()
		{
			Initialize();
		}

		public string[] GetSupportedCurrencies()
		{
			string[] array = new string[_currencies.Count];

			_currencies.Keys.CopyTo(array, 0);

			return array;
		}

		public string GetCurrencySymbol(string ISOCode)
		{
			ISOCode = ISOCode.ToUpper();

			if (_currencies.ContainsKey(ISOCode))
				return _currencies[ISOCode].Symbol;

			return ISOCode;
		}

		public decimal GetCurrencyRate(string ISOCode)
		{
			ISOCode = ISOCode.ToUpper();

			if (_currencies.ContainsKey(ISOCode))
				return _currencies[ISOCode].Rate;

			return Decimal.Zero;
		}

		private XmlDocument GetServiceResponse()
		{
			WebClient ecb = new WebClient();

			try
			{
				TaskManager.StartTask("CURRENCY_HELPER", "LOAD_CURRENCIES_RATES");

				XmlDocument xmlDoc = new XmlDocument();

				xmlDoc.LoadXml(
					ecb.DownloadString(
						ServiceUrl
					)
				);

				return xmlDoc;
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex, "Failed to get response from www.ecb.int");
			}
			finally
			{
				TaskManager.CompleteTask();
			}

			return null;
		}

		private Currency ConvertFromXml(XmlElement element, RegionInfo region)
		{
			string ISOCode = element.GetAttribute("currency");

			Currency currency = new Currency();
			currency.Rate = Decimal.Parse(element.GetAttribute("rate"));
			currency.ISOCode = ISOCode;

			if (region != null)
				currency.Symbol = region.CurrencySymbol;
			else
				currency.Symbol = ISOCode;

			return currency;
		}

		private void Initialize()
		{
			/*Cache cache = new Cache();
			_currencies = (Dictionary<string, Currency>)cache[HttpCacheKey];*/

			if (_currencies == null)
				_currencies = LoadRatesFromService();
		}

		private void OnCacheItem_Removed(string key, object value, CacheItemRemovedReason reason)
		{
			// if currencies rates were expired - renew them
			if (String.Compare(HttpCacheKey, key) == 0)
				_currencies = LoadRatesFromService();
		}

		private Dictionary<string, Currency> LoadRatesFromService()
		{
			XmlDocument ecbXml = GetServiceResponse();

			Dictionary<string, Currency> currencies = new Dictionary<string, Currency>();
			// add euro first
			currencies.Add("EUR", new Currency("EUR", "ˆ", 1));

			if (ecbXml != null)
			{
				XmlNode cube = ecbXml.SelectSingleNode("*[local-name()='Envelope']/*[local-name()='Cube']/*[local-name()='Cube']");

				if (cube != null)
				{
					CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

					IEnumerator iterator = cultures.GetEnumerator();

					// load symbols for currencies
					while (iterator.MoveNext() && cube.ChildNodes.Count > 0)
					{
						CultureInfo culture = (CultureInfo)iterator.Current;

						RegionInfo region = new RegionInfo(culture.LCID);

						string ISOCode = region.ISOCurrencySymbol;

						// find currency by ISO code
						XmlElement element = (XmlElement)cube.SelectSingleNode(
							String.Format(
								"*[local-name()='Cube' and @currency='{0}']",
								ISOCode
							)
						);

						if (element != null)
						{
							currencies.Add(ISOCode, ConvertFromXml(element, region));
							cube.RemoveChild(element);
						}
					}

					// we still have an unrecognized currencies
					if (cube.ChildNodes.Count > 0)
					{
						foreach (XmlElement element in cube.ChildNodes)
							currencies.Add(element.GetAttribute("currency"), ConvertFromXml(element, null));
					}
				}
			}

			// calculate 12.00 time span
			DateTime expire = DateTime.Now.AddDays(1).Subtract(DateTime.Now.TimeOfDay);

			// add currencies to the cache
			/*Cache cache = new Cache();
			cache.Add(
				HttpCacheKey,
				currencies,
				null,
				expire,
				Cache.NoSlidingExpiration,
				CacheItemPriority.Default,
				new CacheItemRemovedCallback(OnCacheItem_Removed)
			);*/

			return currencies;
		}
	}

	public class SecurityUtils
	{
		internal static void SerializeProfile(ref string propertyNames, ref string propertyValues,
			bool encrypt, CheckoutDetails profile)
		{
			// names
			StringBuilder namesBuilder = new StringBuilder();
			// string values
			StringBuilder valsBuilder = new StringBuilder();
			//
			string[] allKeys = profile.GetAllKeys();
			//
			foreach (string keyName in allKeys)
			{
				//
				int length = 0, position = 0;
				// get serialized property value
				string keyValue = profile[keyName];
				//
				if (String.IsNullOrEmpty(keyValue))
				{
					length = -1;
				}
				else
				{
					//
					length = keyValue.Length;
					//
					position = valsBuilder.Length;
					//
					valsBuilder.Append(keyValue);
				}
				//
				namesBuilder.Append(keyName + ":S:" + position.ToString(CultureInfo.InvariantCulture) + ":" + length.ToString(CultureInfo.InvariantCulture) + ":");
			}
			//
			propertyNames = (encrypt) ? CryptoUtils.Encrypt(namesBuilder.ToString()) : namesBuilder.ToString();
			//
			propertyValues = (encrypt) ? CryptoUtils.Encrypt(valsBuilder.ToString()) : valsBuilder.ToString();
		}

		internal static void DeserializeProfile(string propertyNames, string propertyValues,
			bool encrypted, CheckoutDetails details)
		{
			// Input format:
			// PROPERTY_NAME:S:START_INDEX:PROP_VALUE_LENGTH
			//
			if ((propertyNames != null && propertyValues != null) && details != null)
			{
				//
				try
				{
					// decrypt
					propertyNames = (encrypted) ? CryptoUtils.Decrypt(propertyNames) : propertyNames;
					//
					propertyValues = (encrypted) ? CryptoUtils.Decrypt(propertyValues) : propertyValues;
					//
					string[] names = propertyNames.Split(':');
					// divide names length by 4 parts
					int count = names.Length / 4;
					// iterate through
					for (int i = 0; i < count; i++)
					{
						// get property name
						string keyName = names[i * 4];
						//
						string keyValue = String.Empty;
						// calculate property value start index
						int startIndex = Int32.Parse(names[(i * 4) + 2], CultureInfo.InvariantCulture);
						// calculate property value length
						int length = Int32.Parse(names[(i * 4) + 3], CultureInfo.InvariantCulture);
						// ensure check property value not empty
						if (length != -1)
						{
							keyValue = propertyValues.Substring(startIndex, length);
						}
						//
						details[keyName] = keyValue;
					}
				}
				catch
				{
				}
			}
		}

		internal static void SerializeGenericProfile(ref string propertyNames, ref string propertyValues, IKeyValueBunch source)
		{
			// names
			StringBuilder namesBuilder = new StringBuilder();
			// string values
			StringBuilder valsBuilder = new StringBuilder();
			//
			string[] allKeys = source.GetAllKeys();
			//
			foreach (string keyName in allKeys)
			{
				//
				int length = 0, position = 0;
				// get serialized property value
				string keyValue = source[keyName];
				//
				if (String.IsNullOrEmpty(keyValue))
				{
					length = -1;
				}
				else
				{
					//
					length = keyValue.Length;
					//
					position = valsBuilder.Length;
					//
					valsBuilder.Append(keyValue);
				}
				//
				namesBuilder.Append(keyName + ":S:" + position.ToString(CultureInfo.InvariantCulture) + ":" + length.ToString(CultureInfo.InvariantCulture) + ":");
			}
			//
			propertyNames = namesBuilder.ToString();
			//
			propertyValues = valsBuilder.ToString();
		}

		internal static void DeserializeGenericProfile(string propertyNames, string propertyValues, IKeyValueBunch target)
		{
			// Input format:
			// PROPERTY_NAME:S:START_INDEX:PROP_VALUE_LENGTH
			//
			if ((propertyNames != null && propertyValues != null) && target != null)
			{
				//
				try
				{
					//
					string[] names = propertyNames.Split(':');
					// divide names length by 4 parts
					int count = names.Length / 4;
					// iterate through
					for (int i = 0; i < count; i++)
					{
						// get property name
						string keyName = names[i * 4];
						//
						string keyValue = String.Empty;
						// calculate property value start index
						int startIndex = Int32.Parse(names[(i * 4) + 2], CultureInfo.InvariantCulture);
						// calculate property value length
						int length = Int32.Parse(names[(i * 4) + 3], CultureInfo.InvariantCulture);
						// ensure check property value not empty
						if (length != -1)
						{
							keyValue = propertyValues.Substring(startIndex, length);
						}
						//
						target[keyName] = keyValue;
					}
				}
				catch
				{
				}
			}
		}
	}
}