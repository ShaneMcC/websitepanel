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

﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Net;
using System.IO;
using WebsitePanel.Server.Utils;
using WebsitePanel.Providers.WebAppGallery;
using System.Text.RegularExpressions;
using System.Web;
using System.Reflection;
using Microsoft.Web.Deployment;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography;
using System.Collections;

namespace WebsitePanel.Providers.Web
{
	public sealed class WebApplicationGallery
	{
		private CacheManager cache;
		private static DeploymentSkipDirective SkipMsSQL = new DeploymentSkipDirective("skipSqlDirective", "objectName=dbFullSql");
		private static DeploymentSkipDirective SkipMySQL = new DeploymentSkipDirective("skipSqlDirective", "objectName=dbMySql");

		public const string AtomFeedNamespace = "http://www.w3.org/2005/Atom";

		public const string WEB_PI_USER_AGENT_HEADER = "Platform-Installer/2.0.0.0({0})";
		public const string WEB_PI_APP_PACK_ROOT_INSTALLER_ITEM_MISSING = "Root installer item for the {0} application could not be found. Please contact your Web Application Gallery feed provider to resolve the error.";
		public const string WEB_PI_APP_PACK_DISPLAY_URL_MISSING = "Web application '{0}' could not be downloaded as installer displayURL is empty or missing.";

		// web application gallery
		public const string WAG_XML_FEED_CACHE_KEY = "WAG_XML_FEED_CACHE_KEY";
		public const int WEB_APPLICATIONS_CACHE_STORE_MINUTES = 60;
		public const int XML_FEED_RECOVERY_ATTEMPTS = 10;
		public const string WAG_DEFAULT_FEED_URL = "http://www.microsoft.com/web/webpi/2.0/WebApplicationList.xml";

		private string feedXmlURI;

		public WebApplicationGallery()
            : this(WAG_DEFAULT_FEED_URL)
		{
		}

		public WebApplicationGallery(string feedXmlURI)
		{
			cache = CacheFactory.GetCacheManager();
			//
			this.feedXmlURI = feedXmlURI;
		}

		public XmlDocument GetServiceXmlFeed()
		{
			XmlDocument xmldoc = (XmlDocument)cache[WAG_XML_FEED_CACHE_KEY];
			//
			if (xmldoc == null)
			{
				// First trying to load as usual...
				try
				{
					//
					xmldoc = new XmlDocument();
					xmldoc.Load(feedXmlURI);
					//
					CleanupUnknownDependencies(xmldoc);
					// Add XML to the cache
					cache.Add(WAG_XML_FEED_CACHE_KEY, xmldoc);
				}
				catch (Exception ex)
				{
					Log.WriteError(
						String.Format(@"Could not load xml feed in a usual way from '{0}', 
thus proceed to XML-FEED-RECOVERY section to try download it again in an advanced way. 
No action is required the message for information purposes only.", feedXmlURI), ex);
				}
				// Unable to load feed in the usual way, so lets try to "fix" content-encoding issue
				if (!xmldoc.HasChildNodes)
				{
					//
					int numOfRetries = 0;
					//
					try
					{
						WebClient wc = new WebClient();
						wc.Headers.Add(HttpRequestHeader.UserAgent, String.Format(WEB_PI_USER_AGENT_HEADER, Environment.OSVersion));
						// Setting response encoding explicitly
						wc.Encoding = Encoding.UTF8;
						//
						string feedXmlString = wc.DownloadString(feedXmlURI);
						// Loading XML for several times with shift the from the beginning may helps 
						// to eliminate encoding issue (somtimes several starting bytes are not recognized 
						// by the parser and thus feed load is failed)
						do
						{
							try
							{
								xmldoc.LoadXml(feedXmlString.Substring(numOfRetries));
								//
								CleanupUnknownDependencies(xmldoc);
								// Add XML to the cache
								cache.Add(WAG_XML_FEED_CACHE_KEY, xmldoc);
								//
								Log.WriteInfo("XML feed has been successfully added into the cache. See its content below.");
								Log.WriteInfo(xmldoc.OuterXml);
								// Exit from the loop if XML is loaded successfully
								break;
							}
							catch(Exception ex)
							{
								// Log an exception
								Log.WriteError(
									String.Format("XML-FEED-RECOVERY is failed at {0} attempt. {1} attempts left.", numOfRetries, XML_FEED_RECOVERY_ATTEMPTS-numOfRetries), ex);
								//
								numOfRetries++;
							}
						}
						while (numOfRetries <= XML_FEED_RECOVERY_ATTEMPTS);
					}
					catch (Exception ex)
					{
						// Log an exception
						Log.WriteError(@"XML-FEED-RECOVERY is failed to recover the feed automatically. 
Please ensure that the feed is a correct XML file if you use a custom one, 
otherwise contact WebsitePanel Software for further assistance.", ex);
					}
				}
				//
				XmlNamespaceManager nsmgr = GetXmlNsManager(xmldoc.NameTable);
				//
				XmlNode rootNode = xmldoc.DocumentElement.SelectSingleNode("atom:dependencies", nsmgr);
				//
				XmlNodeList idRefNodes = rootNode.SelectNodes(".//atom:dependency[@idref]", nsmgr);
				//
				foreach (XmlNode idRefNode in idRefNodes)
				{
					//
					XmlNode idRefRepo = xmldoc.DocumentElement.SelectSingleNode(
						String.Format("//atom:dependency[@id='{0}']", idRefNode.Attributes["idref"].Value), nsmgr);
					//
					if (idRefRepo != null)
					{
						idRefNode.ParentNode.ReplaceChild(idRefRepo.Clone(), idRefNode);
					}
				}

				//
				idRefNodes = xmldoc.DocumentElement.SelectNodes("//atom:dependency[@idref]", nsmgr);
				//
				foreach (XmlNode idRefNode in idRefNodes)
				{
					//
					XmlNode idRefRepo = xmldoc.DocumentElement.SelectSingleNode(
						String.Format("//atom:dependency[@id='{0}']", idRefNode.Attributes["idref"].Value), nsmgr);
					//
					if (idRefRepo != null)
					{
						idRefNode.ParentNode.ReplaceChild(idRefRepo.Clone(), idRefNode);
					}
				}
			}
			//
			return xmldoc;
		}

		public bool IsMsDeployInstalled()
		{
			bool result = false;
			//
			try
			{
				Assembly assembly = Assembly.LoadWithPartialName("Microsoft.Web.Deployment");
				result = (assembly != null);
			}
			catch
			{
				// nothing to-do
			}
			//
			return result;
		}

		public List<GalleryCategory> GetCategories()
		{
			XmlDocument xmldoc = GetServiceXmlFeed();
			//
			if (xmldoc == null)
				return null;
			//
			List<GalleryCategory> list = new List<GalleryCategory>();
			//
			XmlNamespaceManager nsmgr = GetXmlNsManager(xmldoc.NameTable);
			//
			foreach (XmlNode node in xmldoc.SelectNodes("/atom:feed/atom:keywords/atom:keyword", nsmgr))
			{
				list.Add(new GalleryCategory { Id = node.Attributes["id"].Value, Name = node.InnerText });
			}
			//
			return list;
		}

		public List<GalleryApplication> GetApplications(string categoryName)
		{
			XmlDocument xmldoc = GetServiceXmlFeed();
			//
			if (xmldoc == null)
				return null;
			//
			XmlNamespaceManager nsmgr = GetXmlNsManager(xmldoc.NameTable);
			//
			string xQuery = String.IsNullOrEmpty(categoryName) ? "//atom:entry[@type='application']"
				: String.Format("//atom:entry[@type='application' and atom:keywords[atom:keywordId='{0}']]", categoryName);
			//
			List<GalleryApplication> appList = new List<GalleryApplication>();
			//
			foreach (XmlNode node in xmldoc.SelectNodes(xQuery, nsmgr))
			{
				appList.Add(DeserializeGalleryApplication(node, nsmgr));
			}
			//
			return appList;
		}

		public GalleryApplication GetApplicationByProductId(string productId)
		{
			XmlDocument xmldoc = GetServiceXmlFeed();
			//
			if (xmldoc == null)
				return null;
			//
			XmlNamespaceManager nsmgr = GetXmlNsManager(xmldoc.NameTable);
			//
			string xQuery = String.Format("//atom:entry[@type='application' and atom:productId='{0}']", productId);
			//
			XmlNode node = xmldoc.SelectSingleNode(xQuery, nsmgr);
			//
			GalleryApplication app = DeserializeGalleryApplication(node, nsmgr);
			//
			return app;
		}

		public string GetApplicationPackagePath(string productId)
		{
			return GetApplicationPackagePath(GetApplicationByProductId(productId));
		}

		public string GetApplicationPackagePath(GalleryApplication appObject)
		{
			//
			string appPackagePath = null;
			//
			if (appObject != null)
			{
				InstallerFile installerFile = null;
				// Acquire root installer item
				#region Atom Feed Version 0.2
				if (appObject.InstallerItems.Count > 0)
				{
					InstallerItem installerItem_0 = appObject.InstallerItems[0];
					if (installerItem_0 == null)
					{
						Log.WriteWarning(WEB_PI_APP_PACK_ROOT_INSTALLER_ITEM_MISSING, appObject.Title);
						return appPackagePath;
					}
					// Ensure web app package can be reached
					installerFile = installerItem_0.InstallerFile;
				}
				#endregion

				#region Atom Feed Version 2.0.1.0
				else if (appObject.Installers.Count > 0)
				{
					Installer installerItem_0 = appObject.Installers[0];
					if (installerItem_0 == null)
					{
						Log.WriteWarning(WEB_PI_APP_PACK_ROOT_INSTALLER_ITEM_MISSING, appObject.Title);
						return appPackagePath;
					}
					// Ensure web app package can be reached
					installerFile = installerItem_0.InstallerFile;
				}
				#endregion
				
				if (installerFile == null || String.IsNullOrEmpty(installerFile.InstallerUrl))
				{
					Log.WriteWarning(WEB_PI_APP_PACK_DISPLAY_URL_MISSING, appObject.Title);
					return appPackagePath;
				}
				//
				Log.WriteInfo("Web App Download URL: {0}", installerFile.InstallerUrl);
				// Trying to match the original file name
				HttpWebRequest webReq = (HttpWebRequest)HttpWebRequest.Create(installerFile.InstallerUrl);
				{
					//
					Regex regex = new Regex("filename=\"(?<packageName>.{0,})\"");
					string packageName = null;
					//
					webReq.UserAgent = String.Format(WEB_PI_USER_AGENT_HEADER, Environment.OSVersion.VersionString);
					//
					using (HttpWebResponse webResp = (HttpWebResponse)webReq.GetResponse())
					{
						string httpHeader = webResp.Headers["Content-Disposition"];
						//
						if (!String.IsNullOrEmpty(httpHeader))
						{
							string fileName = Array.Find<string>(httpHeader.Split(';'),
								x => x.Trim().StartsWith("filename="));
							//
							Match match = regex.Match(fileName);
							// Match has been acquired
							if (match != null && match.Success)
							{
								packageName = match.Groups["packageName"].Value;
							}
						}
					}
					// Download URL points to the download package directly
					if (String.IsNullOrEmpty(packageName))
					{
						packageName = Path.GetFileName(installerFile.InstallerUrl);
					}
					//
					if (HttpContext.Current != null)
					{
						appPackagePath = HttpContext.Current.Server.MapPath(String.Format("~/App_Cache/{0}", packageName));
					}
					else
					{
						string assemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
						appPackagePath = Path.Combine(assemblyPath, String.Format(@"App_Cache\{0}", packageName));
					}
				}
			}
			//
			return appPackagePath;
		}

		public List<DeploymentParameter> GetApplicationParameters(string productId)
		{
			string packageFile = GetApplicationPackagePath(productId);
			//
			if (String.IsNullOrEmpty(packageFile))
				return null;
			//
			List<DeploymentParameter> appParams = new List<DeploymentParameter>();
			//
			DeploymentObject iisApplication = null;
			//
			try
			{
				iisApplication = DeploymentManager.CreateObject(DeploymentWellKnownProvider.Package, packageFile);
				//
				foreach (DeploymentSyncParameter parameter in iisApplication.SyncParameters)
				{
					appParams.Add(new DeploymentParameter
					{
						Name = parameter.Name,
						FriendlyName = parameter.FriendlyName,
						Value = parameter.Value,
						DefaultValue = parameter.DefaultValue,
						Description = parameter.Description,
						Tags = parameter.Tags
					});
				}
			}
			catch (Exception ex)
			{
				// Log an error
				Log.WriteError(
					String.Format("Could not read deployment parameters from '{0}' package.", packageFile), ex);
				//
				throw;
			}
			finally
			{
				if (iisApplication != null)
					iisApplication.Dispose();
			}
			//
			return appParams;
		}

		public string InstallApplication(string productId, List<DeploymentParameter> updatedValues)
		{
			string packageFile = GetApplicationPackagePath(productId);
			string applicationPath = null;
			//
			if (String.IsNullOrEmpty(packageFile))
				return null;
			//
			Log.WriteInfo("WebApp Package Path: {0}", packageFile);
			//
			if (!File.Exists(packageFile))
				throw new Exception(GalleryErrorCodes.FILE_NOT_FOUND);
			//
			// Setup source deployment options
			DeploymentBaseOptions sourceOptions = new DeploymentBaseOptions();
			// Add tracing capabilities
			sourceOptions.Trace += new EventHandler<DeploymentTraceEventArgs>(sourceOptions_Trace);
			sourceOptions.TraceLevel = TraceLevel.Verbose;
			// Setup deployment provider
			DeploymentProviderOptions providerOptions = new DeploymentProviderOptions(DeploymentWellKnownProvider.Package);
			// Set the package path location
			providerOptions.Path = packageFile;
			// Prepare the package deployment procedure
			using (DeploymentObject iisApplication = DeploymentManager.CreateObject(providerOptions, sourceOptions))
			{
				// Setup destination deployment options
				DeploymentBaseOptions destinationOptions = new DeploymentBaseOptions();
				// Add tracing capabilities
				destinationOptions.Trace += new EventHandler<DeploymentTraceEventArgs>(sourceOptions_Trace);
				destinationOptions.TraceLevel = TraceLevel.Verbose;

				// MSDEPLOY TEAM COMMENTS: For each parameter that was specified in the UI, set its value 
				foreach (DeploymentParameter updatedValue in updatedValues)
				{
					DeploymentSyncParameter parameter = null;
					// Assert whether a parameter assigned a value
					Trace.Assert(!String.IsNullOrEmpty(updatedValue.Name));
					if (!iisApplication.SyncParameters.TryGetValue(updatedValue.Name, out parameter))
					{
						throw new InvalidOperationException("Could not find a parameter with the name " + updatedValue.Name);
					}
					parameter.Value = updatedValue.Value;
				}

				// Find database parameter received in updated parameters arrays
				DeploymentParameter dbNameParam = Array.Find<DeploymentParameter>(updatedValues.ToArray(),
					p => MatchParameterByNames(p, DeploymentParameter.DATABASE_NAME_PARAMS) 
						|| MatchParameterTag(p, DeploymentParameter.DB_NAME_PARAM_TAG));

				// MSDEPLOY TEAM COMMENTS: There may be a bunch of hidden parameters that never got set.
				// set these to their default values (which will probably be calculated based on the other
				// parameters that were set).
				foreach (DeploymentSyncParameter parameter in iisApplication.SyncParameters)
				{
					// Ensure all syncronization parameters are set
					if (parameter.Value == null)
						throw new InvalidOperationException("Parameter '" + parameter.Name + "' value was not set. This indicates an issue with the package itself. Contact your system administrator.");
					// Detect application path parameter in order to return it to the client
					if (MatchParameterTag(parameter, DeploymentParameter.IISAPP_PARAM_TAG))
					{
						applicationPath = parameter.Value;
						continue;
					}
					//
					if (dbNameParam == null)
						continue;
					
					//
					if (!MatchParameterTag(parameter, DeploymentParameter.DB_NAME_PARAM_TAG)
						&& !MatchParameterByNames(parameter, DeploymentParameter.DATABASE_NAME_PARAMS))
						continue;

					// Application supports both databases MySQL & MSSQL - one of them should be skipped
					if (MatchParameterTag(parameter, DeploymentParameter.MYSQL_PARAM_TAG)
						&& MatchParameterTag(parameter, DeploymentParameter.SQL_PARAM_TAG))
					{
						if (dbNameParam.Tags.ToLowerInvariant().StartsWith("mssql"))
							sourceOptions.SkipDirectives.Add(SkipMySQL);
						// Skip MySQL database scripts
						else if (dbNameParam.Tags.ToLowerInvariant().StartsWith("mysql"))
							sourceOptions.SkipDirectives.Add(SkipMsSQL);
					}
				}
				// Setup deployment options
				DeploymentSyncOptions syncOptions = new DeploymentSyncOptions();
				// Add tracing capabilities
				//syncOptions..Action += new EventHandler<DeploymentActionEventArgs>(syncOptions_Action);
				// Issue a syncronization signal between the parties
				iisApplication.SyncTo(DeploymentWellKnownProvider.Auto, applicationPath, destinationOptions, syncOptions);
				//
				Log.WriteInfo("{0}: {1}", DeploymentParameter.APPICATION_PATH_PARAM, applicationPath);
				//
			}
			//
			return applicationPath;
		}

		#region Helper methods

		private void CleanupUnknownDependencies(XmlDocument xmldoc)
		{
			List<string> knownIds = new List<string>(SupportedAppDependencies._ALL_DEPENDENCIES);
			//
			for (int i = 0; i < knownIds.Count; i++)
				knownIds[i] = String.Format("atom:productId='{0}'", knownIds[i]);
			//
			string xPath = String.Concat("//atom:dependency[(atom:productId) and not(", 
				String.Join(" or ", knownIds.ToArray()), ")]");
			//
			XmlNamespaceManager nsmgr = GetXmlNsManager(xmldoc.NameTable);
			//
			XmlNodeList uknownXmlNodes = xmldoc.SelectNodes(xPath, nsmgr);
			// Cleanup unknown nodes found
			foreach (XmlNode uxn in uknownXmlNodes)
				uxn.ParentNode.RemoveChild(uxn);
			//
		}

		private XmlNamespaceManager GetXmlNsManager(XmlNameTable nt)
		{
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);
			nsmgr.AddNamespace("atom", "http://www.w3.org/2005/Atom");
			//
			return nsmgr;
		}

		private GalleryApplication DeserializeGalleryApplication(XmlNode node, XmlNamespaceManager nsmgr)
		{
			XmlSerializer xs = new XmlSerializer(typeof(GalleryApplication), AtomFeedNamespace);
			GalleryApplication app = (GalleryApplication)xs.Deserialize(new XmlNodeReader(node));
			//
			app.LastUpdated = XmlToDateTime(GetAtomNodeText(node, nsmgr, "updated"), "yyyy-M-dTHH:mm:ssZ");
			app.Published = XmlToDateTime(GetAtomNodeText(node, nsmgr, "published"), "yyyy-M-dTHH:mm:ssZ");
			app.Link = GetAtomNodeAttribute(node, nsmgr, "link", "href");
			//
			app.IconUrl = GetAtomNodeText(node, nsmgr, "images/atom:icon");
			//
			return app;
		}

		private string GetAtomNodeText(XmlNode app, XmlNamespaceManager nsmgr, string nodeName)
		{
			XmlNode node = app.SelectSingleNode("atom:" + nodeName, nsmgr);
			if (node == null)
				return null;
			return node.InnerText;
		}

		private string GetAtomNodeAttribute(XmlNode app, XmlNamespaceManager nsmgr, string nodeName, string attributeName)
		{
			XmlNode node = app.SelectSingleNode("atom:" + nodeName, nsmgr);
			if (node == null)
				return null;
			string ret = null;
			XmlAttribute attribute = node.Attributes[attributeName];
			if (attribute != null)
				ret = attribute.Value;
			return ret;
		}

		private DateTime XmlToDateTime(string val, string format)
		{
			DateTime ret = DateTime.MinValue;

			try
			{
				if (!string.IsNullOrEmpty(val))
					ret = XmlConvert.ToDateTime(val, format);
			}
			catch (Exception) { }
			return ret;
		}

		private static string GetAtomNodeText(XmlNode node)
		{
			if (node == null)
				return null;
			return node.InnerText;
		}

		private bool MatchParameterByNames<T>(T param, string[] nameMatches)
		{
			foreach (var nameMatch in nameMatches)
			{
				if (MatchParameterName<T>(param, nameMatch))
					return true;
			}
			//
			return false;
		}

		private bool MatchParameterName<T>(T param, string nameMatch)
		{
			if (param == null || String.IsNullOrEmpty(nameMatch))
				return false;
			//
			Type paramTypeOf = typeof(T);
			//
			PropertyInfo namePropInfo = paramTypeOf.GetProperty("Name");
			//
			String paramName = Convert.ToString(namePropInfo.GetValue(param, null));
			//
			if (String.IsNullOrEmpty(paramName))
				return false;
			// Compare for tag name match
			return (paramName.ToLowerInvariant() == nameMatch.ToLowerInvariant());
		}

		private bool MatchParameterTag<T>(T param, string tagMatch)
		{
			if (param == null || String.IsNullOrEmpty(tagMatch))
				return false;
			//
			Type paramTypeOf = typeof(T);
			//
			PropertyInfo tagsPropInfo = paramTypeOf.GetProperty("Tags");
			//
			String strTags = Convert.ToString(tagsPropInfo.GetValue(param, null));
			//
			if (String.IsNullOrEmpty(strTags))
				return false;
			// Lookup for specific tags within the parameter
			return Array.Exists<string>(strTags.ToLowerInvariant().Split(','), x => x.Trim() == tagMatch.ToLowerInvariant());
		}

		private void sourceOptions_Trace(object sender, DeploymentTraceEventArgs e)
		{
			Log.WriteInfo(e.Message);
			Log.WriteInfo("Event Level: " + e.EventLevel);
			Log.WriteInfo("Event Data: ");
			//
			foreach (string keyName in e.EventData.Keys)
			{
				Log.WriteInfo(keyName + " => " + e.EventData[keyName]);
			}
		}

		/*private void syncOptions_Action(object sender, DeploymentActionEventArgs e)
		{
			Log.WriteInfo(e.Message);
			Log.WriteInfo("Operation Type: " + e.OperationType);
			Log.WriteInfo("Event Data: ");
			//
			foreach (string keyName in e.EventData.Keys)
			{
				Log.WriteInfo(keyName + " => " + e.EventData[keyName]);
			}
		}*/

		#endregion
	}

	public class DownloadQueueItem
	{
		public String ItemKey;
		public WebClient ConnectionPoint;
		public DownloadProgressChangedEventArgs Progress;
		public string DownloadItemURI;
		public string LocalFilePathToStore;

		public void StartItemDownloadAsync(Object state)
		{
			ConnectionPoint.DownloadFileAsync(new Uri(DownloadItemURI), LocalFilePathToStore);
		}
	}

	public sealed class AppPackagesDownloader
	{
		private static List<DownloadQueueItem> downloadQueue;

		static AppPackagesDownloader()
		{
			downloadQueue = new List<DownloadQueueItem>();
		}

		//
		public static bool CheckApplicationPackageHashSum_MD5(string localFilePath, string md5)
		{
			if (!File.Exists(localFilePath))
				return false;
			//
			bool md5OK = false;
			//
			try
			{
				//
				MD5 md5File = new MD5CryptoServiceProvider();
				//
				string fileMd5Sum = BitConverter.ToString(
					md5File.ComputeHash(File.ReadAllBytes(localFilePath))).Replace("-", String.Empty);
				//
				md5OK = String.Equals(md5, fileMd5Sum, StringComparison.InvariantCultureIgnoreCase);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Failed to compute MD5 sum for {0} package.", localFilePath), ex);
			}
			//
			return md5OK;
		}

		//
		public static bool CheckApplicationPackageHashSum_SHA1(string localFilePath, string sha1)
		{
			if (!File.Exists(localFilePath))
				return false;
			//
			bool sha1OK = false;
			//
			try
			{
				//
				SHA1 sha1File = new SHA1CryptoServiceProvider();
				//
				string fileSha1Sum = BitConverter.ToString(
					sha1File.ComputeHash(File.ReadAllBytes(localFilePath))).Replace("-", String.Empty);
				//
				sha1OK = String.Equals(sha1, fileSha1Sum, StringComparison.InvariantCultureIgnoreCase);
                //
                if (!sha1OK)
                    Log.WriteWarning("SHA1-XML FEED: {0}; SHA1-FILE ON DISK: {1};", sha1, fileSha1Sum); 
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Failed to compute SHA1 sum for {0} package.", localFilePath), ex);
			}
			//
			return sha1OK;
		}

		//
		public static bool IsApplicationInDownloadQueue(string appName)
		{
			bool exists = false;
			//
			if (!String.IsNullOrEmpty(appName))
			{
				ICollection ic = downloadQueue as ICollection;
				//
				lock (ic.SyncRoot)
				{
					exists = Array.Exists<DownloadQueueItem>(downloadQueue.ToArray(),
						x => x.ItemKey == appName.ToLower());
				}
			}
			//
			return exists;
		}

		//
		public static void StartApplicationDownload(string appName, string packageUrl, string localPathToStore)
		{
			//
			bool appInQueue = IsApplicationInDownloadQueue(appName);
			//
			if (!appInQueue)
			{
				ICollection ic = downloadQueue as ICollection;
				//
				lock (ic.SyncRoot)
				{
					//
					DownloadQueueItem qi = new DownloadQueueItem
					{
						ItemKey = appName.ToLower(),
						ConnectionPoint = new WebClient(),
						Progress = null
					};
					//
					qi.DownloadItemURI = packageUrl;
					qi.LocalFilePathToStore = localPathToStore;
					qi.ConnectionPoint.Headers.Add(HttpRequestHeader.UserAgent, String.Format(WebApplicationGallery.WEB_PI_USER_AGENT_HEADER, Environment.OSVersion));
					qi.ConnectionPoint.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(wc_DownloadFileCompleted);
					qi.ConnectionPoint.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
					//
					downloadQueue.Add(qi);
					// Start async download (10 attempts)
					int numOfAttempt = 0;
					do
					{
						try
						{
							bool success = ThreadPool.QueueUserWorkItem(
								new WaitCallback(qi.StartItemDownloadAsync));
							// Exit the loop if the item successfuly queued 
							if (success)
								break;
						}
						catch (Exception ex)
						{
							numOfAttempt++;
							// Log an exception
							Log.WriteError(String.Format("Could not start distibutive download from the following URI: {0}", qi.DownloadItemURI), ex);
						}
					}
					while (numOfAttempt <= 10);
				}
			}
		}

		public static int GetApplicationDownloadProgress(string appName)
		{
			int appProgress = 0;
			//
			bool appInQueue = IsApplicationInDownloadQueue(appName);
			//
			if (appInQueue)
			{
				ICollection ic = downloadQueue as ICollection;
				//
				lock (ic.SyncRoot)
				{
					DownloadQueueItem qi = Array.Find<DownloadQueueItem>(downloadQueue.ToArray(),
						x => x.ItemKey == appName.ToLower());
					//
					if (qi.Progress != null)
						appProgress = qi.Progress.ProgressPercentage;
				}
			}
			//
			return appProgress;
		}

		static void wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
		{
			ICollection ic = downloadQueue as ICollection;
			//
			lock (ic.SyncRoot)
			{
				DownloadQueueItem qi = Array.Find<DownloadQueueItem>(downloadQueue.ToArray(),
					x => x.ConnectionPoint == sender);
				//
				if (e.Error != null)
				{
					//
					if (qi != null)
						Log.WriteError(String.Format("Could not download app {0}", qi.ItemKey), e.Error);
					else
						Log.WriteError(e.Error);
				}
				//
				downloadQueue.Remove(qi);
			}
		}

		static void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			ICollection ic = downloadQueue as ICollection;
			//
			lock (ic.SyncRoot)
			{
				DownloadQueueItem qi = Array.Find<DownloadQueueItem>(downloadQueue.ToArray(),
					x => x.ConnectionPoint == sender);

				if (qi != null)
					qi.Progress = e;
			}
		}
	}
}