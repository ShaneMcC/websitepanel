using System;
using System.Collections.Generic;
using System.Text;
using WebsitePanel.Installer.Configuration;
using System.Net;
using WebsitePanel.Installer.Services;

namespace WebsitePanel.Installer.Core
{
	public class ServiceProviderProxy
	{
		public static InstallerService GetInstallerWebService()
		{
			var webService = new InstallerService();

			string url = AppConfigManager.AppConfiguration.GetStringSetting(ConfigKeys.Web_Service);
			if (!String.IsNullOrEmpty(url))
			{
				webService.Url = url;
			}
			else
			{
				webService.Url = "http://www.websitepanel.net/Services/InstallerService.asmx";
			}

			// check if we need to add a proxy to access Internet
			bool useProxy = AppConfigManager.AppConfiguration.GetBooleanSetting(ConfigKeys.Web_Proxy_UseProxy);
			if (useProxy)
			{
				string proxyServer = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_Address].Value;
				if (!String.IsNullOrEmpty(proxyServer))
				{
					IWebProxy proxy = new WebProxy(proxyServer);
					string proxyUsername = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_UserName].Value;
					string proxyPassword = AppConfigManager.AppConfiguration.Settings[ConfigKeys.Web_Proxy_Password].Value;
					if (!String.IsNullOrEmpty(proxyUsername))
						proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
					webService.Proxy = proxy;
				}
			}

			return webService;
		}
	}
}
