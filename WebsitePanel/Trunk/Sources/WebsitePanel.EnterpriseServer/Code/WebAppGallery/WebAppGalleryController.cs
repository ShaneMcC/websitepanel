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
using WebsitePanel.Providers.WebAppGallery;
using System.Threading;
using WebsitePanel.Providers.Web;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.Database;
using System.Collections.Specialized;
using System.Reflection;
using WebsitePanel.Providers.Common;
using System.Diagnostics;
using System.IO;

namespace WebsitePanel.EnterpriseServer
{
	/// <summary>
	/// Summary description for ConfigSettings.
	/// </summary>
	public class WebAppGalleryController
	{
		#region Constants
		public const string NO_DB_PARAMETER_MATCHES_MSG = "Could not match the parameter by either the following {0} {1}. Please check parameters.xml file within the application package.";
		public const string PARAMETER_IS_NULL_OR_EMPTY = "{0} parameter is either not set or empty";
		public const string CANNOT_SET_PARAMETER_VALUE = "Parameter '{0}' has an empty value. Please check the database service provider configuration.\r\n" +
			"- For Microsoft SQL database server ensure you do not use Trusted connection option.\r\n" +
			"- For MySQL database server ensure database administrator credentials are set.";
		public const string TAGS_MATCH = "tags";
		public const string NAMES_MATCH = "names";
		public const string TASK_MANAGER_SOURCE = "WAG_INSTALLER";
		public const string GET_APP_PARAMS_TASK = "GET_APP_PARAMS_TASK";
		public const string GET_GALLERY_APPS_TASK = "GET_GALLERY_APPS_TASK";
		public const string GET_SRV_GALLERY_APPS_TASK = "GET_SRV_GALLERY_APPS_TASK";
		public const string GET_GALLERY_APP_DETAILS_TASK = "GET_GALLERY_APP_DETAILS_TASK";
		public const string GET_GALLERY_CATEGORIES_TASK = "GET_GALLERY_CATEGORIES_TASK";
		#endregion

		public static GalleryCategoriesResult GetGalleryCategories(int packageId)
		{
			GalleryCategoriesResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_CATEGORIES_TASK);
				//
				WebServer webServer = GetAssociatedWebServer(packageId);
				// ERROR: WAG is unavailable;
				if (!webServer.IsMsDeployInstalled())
				{
					return WAG_MODULE_NOT_AVAILABLE<GalleryCategoriesResult>();
				}
				//
				result = webServer.GetGalleryCategories();
				//
				if (!result.IsSuccess)
				{
					//
					foreach (string errorMessage in result.ErrorCodes)
						TaskManager.WriteError(errorMessage);
					//
					return WAG_GENERIC_MODULE_ERROR<GalleryCategoriesResult>();
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return WAG_GENERIC_MODULE_ERROR<GalleryCategoriesResult>();
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static GalleryApplicationsResult GetGalleryApplicationsByServiceId(int serviceId)
		{
			GalleryApplicationsResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_SRV_GALLERY_APPS_TASK);
				//
				if (SecurityContext.CheckAccount(DemandAccount.IsAdmin) != 0)
				{
					return WAG_MODULE_NOT_AVAILABLE<GalleryApplicationsResult>();
				}
				//
				WebServer webServer = WebServerController.GetWebServer(serviceId);
				// ERROR: WAG is unavailable
				if (!webServer.IsMsDeployInstalled())
				{
					return WAG_MODULE_NOT_AVAILABLE<GalleryApplicationsResult>();
				}
				//
				result = webServer.GetGalleryApplications(String.Empty);
				//
				if (!result.IsSuccess)
				{
					foreach (string errorMessage in result.ErrorCodes)
						TaskManager.WriteError(errorMessage);
					//
					return WAG_GENERIC_MODULE_ERROR<GalleryApplicationsResult>();
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return WAG_GENERIC_MODULE_ERROR<GalleryApplicationsResult>();
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static GalleryApplicationsResult GetGalleryApplications(int packageId, string categoryId)
		{
			GalleryApplicationsResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_APPS_TASK);
				//
				WebServer webServer = GetAssociatedWebServer(packageId);
				// ERROR: WAG is unavailable
				if (!webServer.IsMsDeployInstalled())
				{
					return WAG_MODULE_NOT_AVAILABLE<GalleryApplicationsResult>();
				}
				//
				result = webServer.GetGalleryApplications(categoryId);
				//
				if (!result.IsSuccess)
				{
					foreach (string errorMessage in result.ErrorCodes)
						TaskManager.WriteError(errorMessage);
					//
					return WAG_GENERIC_MODULE_ERROR<GalleryApplicationsResult>();
				}
				//
				PackageContext context = PackageController.GetPackageContext(packageId);

				//
				List<string> appsFilter = new List<string>();
				// if either ASP.NET 1.1 or 2.0 enabled in the hosting plan
				if (context.Quotas[Quotas.WEB_ASPNET11].QuotaAllocatedValue == 1 ||
					context.Quotas[Quotas.WEB_ASPNET20].QuotaAllocatedValue == 1 ||
					context.Quotas[Quotas.WEB_ASPNET40].QuotaAllocatedValue == 1)
				{
					appsFilter.AddRange(SupportedAppDependencies.ASPNET_SCRIPTING);
				}
				// if either PHP 4 or 5 enabled in the hosting plan
				if (context.Quotas[Quotas.WEB_PHP4].QuotaAllocatedValue == 1 ||
					context.Quotas[Quotas.WEB_PHP5].QuotaAllocatedValue == 1)
				{
					appsFilter.AddRange(SupportedAppDependencies.PHP_SCRIPTING);
				}
				// if either MSSQL 2000, 2005 or 2008 enabled in the hosting plan
				if (context.Groups.ContainsKey(ResourceGroups.MsSql2000) ||
					context.Groups.ContainsKey(ResourceGroups.MsSql2005) ||
					context.Groups.ContainsKey(ResourceGroups.MsSql2008))
				{
					appsFilter.AddRange(SupportedAppDependencies.MSSQL_DATABASE);
				}
				// if either MySQL 4 or 5 enabled in the hosting plan
				if (context.Groups.ContainsKey(ResourceGroups.MySql4) ||
					context.Groups.ContainsKey(ResourceGroups.MySql5))
				{
					appsFilter.AddRange(SupportedAppDependencies.MYSQL_DATABASE);
				}
				// Match applications based on the hosting plan restrictions collected
				result.Value = new List<GalleryApplication>(Array.FindAll<GalleryApplication>(result.Value.ToArray(),
					x => MatchGalleryAppDependencies(x.Dependency, appsFilter.ToArray())
						|| MatchMenaltoGalleryApp(x, appsFilter.ToArray())));

				{
					int userId = SecurityContext.User.UserId;
					//
					SecurityContext.SetThreadSupervisorPrincipal();
					//
					string[] filteredApps = GetServiceAppsCatalogFilter(packageId);
					//
					if (filteredApps != null)
					{
						result.Value = new List<GalleryApplication>(Array.FindAll(result.Value.ToArray(),
							x => !Array.Exists(filteredApps, 
								z => z.Equals(x.Id, StringComparison.InvariantCultureIgnoreCase))));
					}
					//
					SecurityContext.SetThreadPrincipal(userId);
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return WAG_GENERIC_MODULE_ERROR<GalleryApplicationsResult>();
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		/// <summary>
		/// Matches Gallery 2 app from menalto.com author. This method has been developed to eliminate
		/// a discrepancy between the application dependencies (requirements) definition - because it's the only
		/// distributive that can be run on both MySQL and MSSQL db server.
		/// </summary>
		/// <param name="appObject">Gallery application object.</param>
		/// <param name="dependencyIds">Provides information of dependencies available.</param>
		/// <returns>True or false if either PHP + (MySQL or MSSQL) are available through dependencyIds.</returns>
		internal static bool MatchMenaltoGalleryApp(GalleryApplication appObject, string[] dependencyIds)
		{
			if (appObject.AuthorName == "menalto.com" && appObject.Title == "Gallery")
			{
				// Ensure PHP is allowed in a hosting plan
				if (Array.Exists<String>(dependencyIds, x =>
					Array.Exists<String>(SupportedAppDependencies.PHP_SCRIPTING, n =>
						String.Equals(n, x, StringComparison.InvariantCultureIgnoreCase))))
				{
					// Lookup for MSSQL dependencies
					bool mssqlEnabled = Array.Exists<String>(dependencyIds, x =>
							Array.Exists<String>(SupportedAppDependencies.MSSQL_DATABASE, n =>
								String.Equals(n, x, StringComparison.InvariantCultureIgnoreCase)));
					// Lookup for MySQL dependencies
					bool mysqlEnabled = Array.Exists<String>(dependencyIds, x =>
						Array.Exists<String>(SupportedAppDependencies.MYSQL_DATABASE, n =>
							String.Equals(n, x, StringComparison.InvariantCultureIgnoreCase)));
					// If either one of db type is enabled, return true
					if (mysqlEnabled || mssqlEnabled)
						return true;
				}
			}
			//
			return false;
		}

		internal static bool MatchParticularAppDependency(Dependency dependency, string[] dependencyIds)
		{
			List<Dependency> nested = null;
			// Web PI ver. 0.2
			if (dependency.LogicalAnd.Count > 0)
				nested = dependency.LogicalAnd;
			else if (dependency.LogicalOr.Count > 0)
				nested = dependency.LogicalOr;
			// Web PI ver. 2.0.1.0
			else if (dependency.And.Count > 0)
				nested = dependency.And;
			else if (dependency.Or.Count > 0)
				nested = dependency.Or;

			if (nested != null)
			{
				// Check conditions
				foreach (Dependency ndep in nested)
					if (MatchGalleryAppDependencies(ndep, dependencyIds))
						return true;
				//
				return false;
			}
			// Non-empty dependencies should be filtered out if do not match
			if (!String.IsNullOrEmpty(dependency.ProductId))
				return Array.Exists<string>(dependencyIds, x => String.Equals(x, dependency.ProductId,
					StringComparison.InvariantCultureIgnoreCase));

			// Empty should not match everything when checking a certain dependencies
			return false;
		}

		internal static bool MatchGalleryAppDependencies(Dependency dependency, string[] dependencyIds)
		{
			List<Dependency> nested = null;
			// Web PI ver. 0.2
			if (dependency.LogicalAnd.Count > 0)
				nested = dependency.LogicalAnd;
			else if (dependency.LogicalOr.Count > 0)
				nested = dependency.LogicalOr;
			// Web PI ver. 2.0.1.0
			else if (dependency.And.Count > 0)
				nested = dependency.And;
			else if (dependency.Or.Count > 0)
				nested = dependency.Or;

			if (nested != null)
			{
				// Check LogicalAnd conditions
				if (nested == dependency.LogicalAnd || nested == dependency.And)
				{
					foreach (Dependency ndep in nested)
						if (!MatchGalleryAppDependencies(ndep, dependencyIds))
							return false;
					//
					return true;
				}
				//
				if (nested == dependency.LogicalOr || nested == dependency.Or)
				{
					bool matchOK = false;
					//
					foreach (Dependency ndep in nested)
						if (MatchGalleryAppDependencies(ndep, dependencyIds))
							matchOK = true;
					//
					return matchOK;
				}
			}
			// Non-empty dependencies should be filtered out if do not match
			if (!String.IsNullOrEmpty(dependency.ProductId))
				return Array.Exists<string>(dependencyIds, x => String.Equals(x, dependency.ProductId,
					StringComparison.InvariantCultureIgnoreCase));

			// Empty dependencies always match everything
			return true;
		}

		public static GalleryApplicationResult GetGalleryApplicationDetails(int packageId, string applicationId)
		{
			GalleryApplicationResult result;
			//
			try
			{
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_GALLERY_APP_DETAILS_TASK);
				//
				WebServer webServer = GetAssociatedWebServer(packageId);
				// ERROR: WAG is not available
				if (!webServer.IsMsDeployInstalled())
				{
					return WAG_MODULE_NOT_AVAILABLE<GalleryApplicationResult>();
				}
				//
				result = webServer.GetGalleryApplication(applicationId);
				//
				if (!result.IsSuccess)
				{
					foreach (string errorMessage in result.ErrorCodes)
						TaskManager.WriteError(errorMessage);
					//
					return WAG_GENERIC_MODULE_ERROR<GalleryApplicationResult>();
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return WAG_GENERIC_MODULE_ERROR<GalleryApplicationResult>();
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static DeploymentParametersResult GetGalleryApplicationParams(int packageId, string webAppId)
		{
			DeploymentParametersResult appParamsResult = GetGalleryApplicationParamsInternally(packageId, webAppId);
			//
			if (appParamsResult.IsSuccess)
			{
				List<DeploymentParameter> appParams = new List<DeploymentParameter>();
				//
				foreach (DeploymentParameter parameter in appParamsResult.Value)
				{
					// Exclude non-public parameters
					if (MatchNonPublicParamByTags(parameter))
						continue;
					if (MatchNonPublicParamByNames(parameter))
						continue;
					//
					appParams.Add(parameter);
				}
				//
				appParamsResult.Value = appParams;
			}
			//
			return appParamsResult;
		}

		public static DeploymentParametersResult GetGalleryApplicationParamsInternally(int packageId, string webAppId)
		{
			DeploymentParametersResult result = null;
			//
			try
			{
				//
				TaskManager.StartTask(TASK_MANAGER_SOURCE, GET_APP_PARAMS_TASK);
				//
				WebServer webServer = GetAssociatedWebServer(packageId);
				//
				if (!webServer.IsMsDeployInstalled())
					return WAG_MODULE_NOT_AVAILABLE<DeploymentParametersResult>();
				//
				result = webServer.GetGalleryApplicationParameters(webAppId);
				//
				if (!result.IsSuccess)
				{
					//
					foreach (string errorMessage in result.ErrorCodes)
						TaskManager.WriteError(errorMessage);
					//
					return WAG_GENERIC_MODULE_ERROR<DeploymentParametersResult>();
				}
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
				//
				return WAG_GENERIC_MODULE_ERROR<DeploymentParametersResult>();
			}
			finally
			{
				TaskManager.CompleteTask();
			}
			//
			return result;
		}

		public static StringResultObject Install(int packageId, string webAppId, string siteName, string virtualDir, List<DeploymentParameter> updatedParameters)
		{
			StringResultObject result = new StringResultObject();
			//
			int dbItemResult = -1, dbUserResult = -1;
			WebSite webSite = default(WebSite);
			WebVirtualDirectory webVirtualDir = default(WebVirtualDirectory);
			WebVirtualDirectory iisAppNode = default(WebVirtualDirectory);
			//
			try
			{
				SecurityContext.SetThreadSupervisorPrincipal();
				//
				TaskManager.StartTask(TASK_MANAGER_SOURCE, "INSTALL_WEB_APP");
				//
				TaskManager.WriteParameter("Package ID", packageId);
				TaskManager.WriteParameter("Site Name", siteName);

				//
				WebServer webServer = GetAssociatedWebServer(packageId);

				// ERROR: WAG is not available
				if (!webServer.IsMsDeployInstalled())
					return WAG_MODULE_NOT_AVAILABLE<StringResultObject>();

				//
				GalleryApplicationResult appResult = webServer.GetGalleryApplication(webAppId);

				#region Preparations and tracing
				// Trace at least Web Application Id for troubleshooting purposes
				if (!appResult.IsSuccess || appResult.Value == null)
				{
					TaskManager.WriteError("Could not find an application to install with ID: {0}.", webAppId);
					//
					return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
				}
				// Assign web app pack title to the currently running task
				TaskManager.ItemName = appResult.Value.Title;
				// Trace additional details from the feed
				TraceGalleryAppPackInfo(appResult.Value);

				// Trace out all deployment parameters
				Array.ForEach<DeploymentParameter>(updatedParameters.ToArray(), p => TaskManager.WriteParameter(p.Name, p.Value));
				// Check account
				int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsActive);
				if (accountCheck < 0)
				{
					TaskManager.WriteError("Account check has been failed. Status: {0};", accountCheck.ToString());
					//
					return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
				}
				// Check package
				int packageCheck = SecurityContext.CheckPackage(packageId, DemandPackage.IsActive);
				if (packageCheck < 0)
				{
					TaskManager.WriteError("Package check has been failed. Status: {0};", packageCheck.ToString());
					//
					return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
				}
				// Check web site for existence
				webSite = WebServerController.GetWebSite(packageId, siteName);
				if (webSite == null)
				{
					TaskManager.WriteError("Web site check has been failed. Status: {0};", BusinessErrorCodes.ERROR_WEB_INSTALLER_WEBSITE_NOT_EXISTS.ToString());
					//
					return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
				}
				#endregion

				DeploymentParametersResult appParamsResult = GetGalleryApplicationParamsInternally(packageId, webAppId);

				//
				if (!appParamsResult.IsSuccess)
				{
					foreach (string errorMessage in appParamsResult.ErrorCodes)
						TaskManager.WriteError(errorMessage);
					//
					return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
				}

				List<DeploymentParameter> origParams = appParamsResult.Value;
				//
				if (Array.Exists<DeploymentParameter>(origParams.ToArray(),
					p => MatchParameterTag(p, DeploymentParameter.SQL_PARAM_TAG)
						|| MatchParameterTag(p, DeploymentParameter.MYSQL_PARAM_TAG)))
				{
					// Match input parameters from the client
					DeploymentParameter dbNameParam = Array.Find<DeploymentParameter>(updatedParameters.ToArray(),
						p => MatchParameterByNames(p, DeploymentParameter.DATABASE_NAME_PARAMS)
							|| MatchParameterTag(p, DeploymentParameter.DB_NAME_PARAM_TAG));
					//
					DeploymentParameter dbUserParam = Array.Find<DeploymentParameter>(updatedParameters.ToArray(),
						p => MatchParameterByNames(p, DeploymentParameter.DATABASE_USERNAME_PARAMS)
							|| MatchParameterTag(p, DeploymentParameter.DB_USERNAME_PARAM_TAG));
					//
					DeploymentParameter dbUserPwParam = Array.Find<DeploymentParameter>(updatedParameters.ToArray(),
						p => MatchParameterByNames(p, DeploymentParameter.DATABASE_USERPWD_PARAMS)
							|| MatchParameterTag(p, DeploymentParameter.DB_PASSWORD_PARAM_TAG));

					#region Pre-conditions verification...
					//
					if (dbNameParam == null)
					{
						//
						TaskManager.WriteError(PARAMETER_IS_NULL_OR_EMPTY, DeploymentParameter.DATABASE_NAME_PARAM);
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					//
					if (String.IsNullOrEmpty(dbNameParam.Tags))
					{
						//
						TaskManager.WriteError("{0} parameter tags does not contain information about the database resource group should be used", DeploymentParameter.DATABASE_NAME_PARAM);
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					//
					int dbServiceId = PackageController.GetPackageServiceId(packageId, dbNameParam.Tags);
					//
					if (dbServiceId <= 0)
					{
						//
						TaskManager.WriteError("{0} parameter tags contains wrong information about the database resource group should be used. Resource group: " + dbNameParam.Tags, DeploymentParameter.DATABASE_NAME_PARAM);
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					#endregion
					//
					DeploymentParameter dbServerParam = Array.Find<DeploymentParameter>(origParams.ToArray(),
						p => MatchParameterByNames(p, DeploymentParameter.DB_SERVER_PARAMS)
							|| MatchParameterTag(p, DeploymentParameter.DB_SERVER_PARAM_TAG));
					//
					DeploymentParameter dbAdminParam = Array.Find<DeploymentParameter>(origParams.ToArray(),
						p => MatchParameterByNames(p, DeploymentParameter.DB_ADMIN_PARAMS)
							|| MatchParameterTag(p, DeploymentParameter.DB_ADMIN_USERNAME_PARAM_TAG));
					//
					DeploymentParameter dbAdminPwParam = Array.Find<DeploymentParameter>(origParams.ToArray(),
						p => MatchParameterByNames(p, DeploymentParameter.DB_ADMINPWD_PARAMS)
							|| MatchParameterTag(p, DeploymentParameter.DB_ADMIN_PASSWORD_PARAM_TAG));

					#region Pre-conditions verification...
					//
					if (dbAdminParam == null)
					{
						//
						TaskManager.WriteError(NO_DB_PARAMETER_MATCHES_MSG, NAMES_MATCH,
							String.Join(", ", DeploymentParameter.DB_ADMIN_PARAMS));
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					//
					if (dbServerParam == null)
					{
						//
						TaskManager.WriteError(NO_DB_PARAMETER_MATCHES_MSG, NAMES_MATCH,
							String.Join(", ", DeploymentParameter.DB_SERVER_PARAMS));
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					//
					if (dbAdminPwParam == null)
					{
						//
						TaskManager.WriteError(NO_DB_PARAMETER_MATCHES_MSG, NAMES_MATCH,
							String.Join(", ", DeploymentParameter.DB_ADMINPWD_PARAMS));
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					#endregion

					#region Read & substitute database server settings
					//
					StringDictionary dbSettings = ServerController.GetServiceSettingsAdmin(dbServiceId);

					// InternalAddress setting is common for all DB service providers
					dbServerParam.Value = dbSettings["InternalAddress"];
					// Set database administrator login
					if (!String.IsNullOrEmpty(dbSettings["RootLogin"]))
						dbAdminParam.Value = dbSettings["RootLogin"];
					else if (!String.IsNullOrEmpty(dbSettings["SaLogin"]))
						dbAdminParam.Value = dbSettings["SaLogin"];
					else
					{
						//
						TaskManager.WriteError(CANNOT_SET_PARAMETER_VALUE, dbAdminParam.Name);
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					// Set database administrator password
					if (!String.IsNullOrEmpty(dbSettings["RootPassword"]))
						dbAdminPwParam.Value = dbSettings["RootPassword"];
					else if (!String.IsNullOrEmpty(dbSettings["SaPassword"]))
						dbAdminPwParam.Value = dbSettings["SaPassword"];
					else
					{
						//
						TaskManager.WriteError(CANNOT_SET_PARAMETER_VALUE, dbAdminPwParam.Name);
						//
						return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
					}
					#endregion

					//
					updatedParameters.AddRange(new List<DeploymentParameter> { dbServerParam, dbAdminParam, dbAdminPwParam });

					#region Create database and db user account if new selected
					//
					SqlDatabase db = PackageController.GetPackageItemByName(packageId, dbNameParam.Value,
						typeof(SqlDatabase)) as SqlDatabase;
					//
					if (db == null)
					{
						db = new SqlDatabase();
						db.PackageId = packageId;
						db.Name = dbNameParam.Value;
						//
						dbItemResult = DatabaseServerController.AddSqlDatabase(db, dbNameParam.Tags);
						//
						if (dbItemResult < 0)
						{
							// Put specific error message into the trace log
							TaskManager.WriteError("Could not create {0} database. Error code: {1}.", dbNameParam.Tags, dbItemResult.ToString());
							// Return generic error
							return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
						}
					}

					//
					SqlUser user = PackageController.GetPackageItemByName(packageId, dbUserParam.Value,
						typeof(SqlUser)) as SqlUser;
					//
					if (user == null)
					{
						user = new SqlUser();
						user.PackageId = packageId;
						user.Name = dbUserParam.Value;
						user.Databases = new string[] { dbNameParam.Value };
						user.Password = dbUserPwParam.Value;
						//
						dbUserResult = DatabaseServerController.AddSqlUser(user, dbNameParam.Tags);
						//
						if (dbUserResult < 0)
						{
							// Rollback and remove db if created
							if (dbItemResult > 0)
								DatabaseServerController.DeleteSqlDatabase(dbItemResult);
							// Put specific error message into the trace log
							TaskManager.WriteError("Could not create {0} user account. Error code: {1}.", dbNameParam.Tags, dbUserResult.ToString());
							// Return generic error
							return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
						}
					}
					#endregion
				}

				//
				DeploymentParameter appPathParam = Array.Find<DeploymentParameter>(origParams.ToArray(),
					p => MatchParameterName(p, DeploymentParameter.APPICATION_PATH_PARAM)
						|| MatchParameterTag(p, DeploymentParameter.IISAPP_PARAM_TAG));
				//
				if (String.IsNullOrEmpty(virtualDir))
					appPathParam.Value = siteName;
				else
					appPathParam.Value = String.Format("{0}/{1}", siteName, virtualDir);
				//
				updatedParameters.Add(appPathParam);

				//
				result = webServer.InstallGalleryApplication(webAppId, updatedParameters.ToArray());
				//
				#region Rollback in case of failure
				// Rollback - remove resources have been created previously
				if (!result.IsSuccess)
				{
					//
					if (dbUserResult > 0)
						DatabaseServerController.DeleteSqlUser(dbUserResult);
					//
					if (dbItemResult > 0)
						DatabaseServerController.DeleteSqlDatabase(dbItemResult);
					//
					foreach (string errorCode in result.ErrorCodes)
						TaskManager.WriteError(errorCode);
					//
					return result;
				}
				#endregion

				// Reload web site details
				webSite = WebServerController.GetWebSite(packageId, siteName);
				// Reload virtual directory defaults
				if (!String.IsNullOrEmpty(virtualDir))
					webVirtualDir = WebServerController.GetVirtualDirectory(webSite.Id, virtualDir);
				
				// We are going to install application on website or virtual directory
				iisAppNode = (webVirtualDir != null) ? webVirtualDir : webSite;
				// Put correct ASP.NET version depending on a web server's version
				iisAppNode.AspNetInstalled = (iisAppNode.IIs7) ? "2I" : "2";
				
				//
				if (MatchParticularAppDependency(appResult.Value.Dependency, SupportedAppDependencies.PHP_SCRIPTING))
				{
					// Enable PHP 5 extensions for web site
					iisAppNode.PhpInstalled = "5";
					// Set the correct default document for PHP apps
					if (iisAppNode.DefaultDocs.IndexOf("index.php", StringComparison.InvariantCultureIgnoreCase) == -1)
						iisAppNode.DefaultDocs += ",index.php";
					//
					int docsResult = 0;
					//
					if (webVirtualDir == null)
						docsResult = WebServerController.UpdateWebSite(webSite);
					else
						docsResult = WebServerController.UpdateVirtualDirectory(webSite.Id, webVirtualDir);
					//
					if (docsResult < 0)
						TaskManager.WriteWarning("Could not update website/virtual directory default documents with the value of index.php. Result code: {0}", docsResult.ToString());
				}
				//
				if (MatchParticularAppDependency(appResult.Value.Dependency, SupportedAppDependencies.ASPNET_SCRIPTING))
				{
					// Set the correct default document for ASP.NET apps
					if (iisAppNode.DefaultDocs.IndexOf("Default.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
						iisAppNode.DefaultDocs += ",Default.aspx";
					//
					int aspnetResult = 0;
					//
					if (webVirtualDir == null)
						aspnetResult = WebServerController.UpdateWebSite(webSite);
					else
						aspnetResult = WebServerController.UpdateVirtualDirectory(webSite.Id, webVirtualDir);
					//
					if (aspnetResult < 0)
						TaskManager.WriteWarning("Could not set default documents/enable ASP.NET 2.0 (Integrated Mode) for website/virtual directory. Result code: {0}", aspnetResult.ToString());
				}

				//
				return result;
			}
			catch (Exception ex)
			{
				//
				TaskManager.WriteError(ex);
				//
				return WAG_INSTALL_GENERIC_MODULE_ERROR<StringResultObject>();
			}
			finally
			{
				TaskManager.CompleteTask();
			}
		}

		public static GalleryWebAppStatus GetGalleryApplicationStatus(int packageId, string webAppId)
		{
			try
			{
				WebServer webServer = GetAssociatedWebServer(packageId);
				//
				if (!webServer.IsMsDeployInstalled())
					throw new ApplicationException(WAG_MODULE_NOT_AVAILABLE<StringResultObject>().ErrorCodes[0]);
				//
				GalleryWebAppStatus appStatus = webServer.GetGalleryApplicationStatus(webAppId);
				//
				if (appStatus == GalleryWebAppStatus.NotDownloaded)
				{
					GalleryApplicationResult appResult = webServer.GetGalleryApplication(webAppId);
					// Start app download in new thread
					WebAppGalleryAsyncWorker async = new WebAppGalleryAsyncWorker();
					async.GalleryApp = appResult.Value;
					async.WebAppId = webAppId;
					async.PackageId = packageId;
					async.UserId = SecurityContext.User.UserId;
					async.DownloadGalleryWebApplicationAsync();
					//
					return GalleryWebAppStatus.Downloading;
				}
				//
				return appStatus;
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.StackTrace);
				//
				return GalleryWebAppStatus.Failed;
			}
		}

		internal static void TraceGalleryAppPackInfo(GalleryApplication webAppPack)
		{
			if (webAppPack != null)
			{
				//
				TaskManager.WriteParameter("Title", webAppPack.Title);
				TaskManager.WriteParameter("Version", webAppPack.Version);
				TaskManager.WriteParameter("Download URL", webAppPack.DownloadUrl);
				TaskManager.WriteParameter("Author", webAppPack.AuthorName);
				TaskManager.WriteParameter("Last Updated", webAppPack.LastUpdated);
			}
		}

		internal static WebServer GetAssociatedWebServer(int packageId)
		{
			int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
			//
			return WebServerController.GetWebServer(serviceId);
		}

		internal static string[] GetServiceAppsCatalogFilter(int packageId)
		{
			int serviceId = PackageController.GetPackageServiceId(packageId, ResourceGroups.Web);
			//
			StringDictionary serviceSettings = ServerController.GetServiceSettings(serviceId);
			//
			string galleryAppsFilterStr = serviceSettings["GalleryAppsFilter"];
			//
			if (String.IsNullOrEmpty(galleryAppsFilterStr))
				return null;
			//
			return galleryAppsFilterStr.Split(new string[] { "," }, 
				StringSplitOptions.RemoveEmptyEntries);
		}

		public static bool MatchNonPublicParamByTags(DeploymentParameter param)
		{
			foreach (string nonPublicParamTag in DeploymentParameter.NON_PUBLIC_PARAM_TAGS)
				if (MatchParameterTag(param, nonPublicParamTag))
					return true;
			//
			return false;
		}

		public static bool MatchNonPublicParamByNames(DeploymentParameter param)
		{
			return MatchParameterByNames(param, DeploymentParameter.NON_PUBLIC_PARAM_NAMES);
		}

		public static bool MatchParameterByNames(DeploymentParameter param, string[] namesMatches)
		{
			foreach (string nameMatch in namesMatches)
				if (MatchParameterName(param, nameMatch))
					return true;
			//
			return false;
		}

		public static bool MatchParameterTag(DeploymentParameter param, string tagMatch)
		{
			if (param == null || String.IsNullOrEmpty(tagMatch))
				return false;
			//
			if (String.IsNullOrEmpty(param.Tags))
				return false;
			// Lookup for specific tags within the parameter
			return Array.Exists<string>(param.Tags.ToLowerInvariant().Split(','), x => x.Trim() == tagMatch.ToLowerInvariant());
		}

		public static bool MatchParameterName(DeploymentParameter param, string nameMatch)
		{
			if (param == null || String.IsNullOrEmpty(nameMatch))
				return false;
			//
			if (String.IsNullOrEmpty(param.Name))
				return false;
			// Match parameter name
			return (param.Name.ToLowerInvariant() == nameMatch.ToLowerInvariant());
		}

		internal static T WAG_GENERIC_MODULE_ERROR<T>()
		{
			Type typeOf = typeof(T);
			//
			T resultObj = Activator.CreateInstance<T>();
			//
			ResultObject _ro = resultObj as ResultObject;
			if (_ro != null)
			{
				_ro.ErrorCodes = new List<string> { "WAG_GENERIC_MODULE_ERROR" };
				_ro.IsSuccess = false;
			}
			//
			return resultObj;
		}

		internal static T WAG_MODULE_NOT_AVAILABLE<T>()
		{
			Type typeOf = typeof(T);
			//
			T resultObj = Activator.CreateInstance<T>();
			//
			ResultObject _ro = resultObj as ResultObject;
			if (_ro != null)
			{
				_ro.ErrorCodes = new List<string> { "WAG_MODULE_NOT_AVAILABLE" };
				_ro.IsSuccess = false;
			}
			//
			return resultObj;
		}

		internal static T WAG_INSTALL_GENERIC_MODULE_ERROR<T>()
		{
			Type typeOf = typeof(T);
			//
			T resultObj = Activator.CreateInstance<T>();
			//
			ResultObject _ro = resultObj as ResultObject;
			if (_ro != null)
			{
				_ro.ErrorCodes = new List<string> { "WAG_INSTALL_GENERIC_MODULE_ERROR", "APP_PACK_INSTALLATION_FAILURE" };
				_ro.IsSuccess = false;
			}
			//
			return resultObj;
		}
	}

	public class WebAppGalleryAsyncWorker
	{
		public int PackageId { get; set; }
		public string WebAppId { get; set; }
		public GalleryApplication GalleryApp { get; set; }
		public int UserId { get; set; }

		public void DownloadGalleryWebApplicationAsync()
		{
			Thread t = new Thread(new ThreadStart(DownloadGalleryWebApplication));
			t.Start();
		}

		public void DownloadGalleryWebApplication()
		{
			SecurityContext.SetThreadPrincipal(UserId);
			//
			TaskManager.StartTask(WebAppGalleryController.TASK_MANAGER_SOURCE, "DOWNLOAD_WEB_APP", GalleryApp.Title);
			TaskManager.WriteParameter("Version", GalleryApp.Version);
			TaskManager.WriteParameter("Download URL", GalleryApp.DownloadUrl);
			TaskManager.WriteParameter("Author", GalleryApp.AuthorName);
			TaskManager.WriteParameter("Last Updated", GalleryApp.LastUpdated);
			TaskManager.WriteParameter("Web App ID", WebAppId);
			//
			try
			{
				//
				WebServer webServer = WebAppGalleryController.GetAssociatedWebServer(PackageId);
				//
				TaskManager.Write("Application package download has been started");
				//
				GalleryWebAppStatus appStatus = webServer.DownloadGalleryApplication(WebAppId);
				//
				if (appStatus == GalleryWebAppStatus.Failed)
				{
					TaskManager.WriteError("Could not download application package requested");
					TaskManager.WriteError("Please check WebsitePanel Server log for further information on this issue");
					TaskManager.WriteParameter("Status returned", appStatus);
					return;
				}
				//
				TaskManager.Write("Application package download has been started successfully");
			}
			catch (Exception ex)
			{
				TaskManager.WriteError(ex);
			}
			finally
			{
				//
				TaskManager.CompleteTask();
			}
		}
	}
}
