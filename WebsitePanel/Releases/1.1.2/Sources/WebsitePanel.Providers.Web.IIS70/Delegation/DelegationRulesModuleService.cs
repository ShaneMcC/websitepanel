using System;
using System.Collections.Generic;
using System.Text;
using WebsitePanel.Providers.Web.Iis.Common;
using Microsoft.Web.Administration;

namespace WebsitePanel.Providers.Web.Delegation
{
	internal sealed class DelegationRulesModuleService : ConfigurationModuleService
	{
		public void AddDelegationRule(string providers, string path, string pathType, string identityType, string userName, string userPassword)
		{
			var predicate = new Predicate<ConfigurationElement>(x => { 
				return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path); });
			//
			using (var srvman = GetServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();
				//
				var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Update rule if exists
				foreach (var rule in rulesCollection)
				{
					//
					if(predicate.Invoke(rule) == true)
					{
						if (identityType.Equals("SpecificUser"))
						{
							var runAsElement = rule.ChildElements["runAs"];
							//
							runAsElement.SetAttributeValue("userName", userName);
							runAsElement.SetAttributeValue("password", userPassword);
							// Ensure the rules is enabled
							if (rule.Attributes["enabled"].Equals(false))
							{
								rule.SetAttributeValue("enabled", true);
							}
							//
							srvman.CommitChanges();
						}
						//
						return; // Exit
					}
				}
				// Create new rule if none exists
				var newRule = rulesCollection.CreateElement("rule");
				newRule.SetAttributeValue("providers", providers);
				newRule.SetAttributeValue("actions", "*"); // Any
				newRule.SetAttributeValue("path", path);
				newRule.SetAttributeValue("pathType", pathType);
				newRule.SetAttributeValue("enabled", true);
				// Run rule as SpecificUser
				if (identityType.Equals("SpecificUser"))
				{
					var runAs = newRule.GetChildElement("runAs");
					//
					runAs.SetAttributeValue("identityType", "SpecificUser");
					runAs.SetAttributeValue("userName", userName);
					runAs.SetAttributeValue("password", userPassword);
				}
				else // Run rule as CurrentUser
				{
					var runAs = newRule.GetChildElement("runAs");
					//
					runAs.SetAttributeValue("identityType", "CurrentUser");
				}
				// Establish permissions
				var permissions = newRule.GetCollection("permissions");
				var user = permissions.CreateElement("user");
				user.SetAttributeValue("name", "*");
				user.SetAttributeValue("accessType", "Allow");
				user.SetAttributeValue("isRole", false);
				permissions.Add(user);
				//
				rulesCollection.Add(newRule);
				//
				srvman.CommitChanges();
			}
		}

		public void RemoveDelegationRule(string providers, string path)
		{
			var predicate = new Predicate<ConfigurationElement>(x =>
			{
				return x.Attributes["providers"].Value.Equals(providers) && x.Attributes["path"].Value.Equals(path);
			});
			//
			using (var srvman = GetServerManager())
			{
				var adminConfig = srvman.GetAdministrationConfiguration();
				//
				var delegationSection = adminConfig.GetSection("system.webServer/management/delegation");
				//
				var rulesCollection = delegationSection.GetCollection();
				// Remove rule if exists
				foreach (var rule in rulesCollection)
				{
					// Match rule against predicate
					if (predicate.Invoke(rule) == true)
					{
						rulesCollection.Remove(rule);
						//
						srvman.CommitChanges();
						//
						return; // Exit
					}
				}
			}
		}
	}
}
