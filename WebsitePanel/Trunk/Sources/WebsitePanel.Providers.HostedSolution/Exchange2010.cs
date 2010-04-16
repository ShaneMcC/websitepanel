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
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using System.Globalization;

using System.DirectoryServices;
using System.Security;
using System.Security.Principal;
using System.Security.AccessControl;

using System.Management.Automation;
using System.Management.Automation.Runspaces;

using WebsitePanel.Providers;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.Utils;
using WebsitePanel.Server.Utils;
using Microsoft.Exchange.Data.Directory.Recipient;
using Microsoft.Win32;

using Microsoft.Exchange.Data;
using Microsoft.Exchange.Data.Directory;
using Microsoft.Exchange.Data.Storage;

namespace WebsitePanel.Providers.HostedSolution
{
	public class Exchange2010 : Exchange2007
	{
		#region Static constructor
		static Exchange2010()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveExchangeAssembly);
			ExchangeRegistryPath = "SOFTWARE\\Microsoft\\ExchangeServer\\v14\\Setup";
		}
		#endregion

		#region Mailboxes

		internal override void SetCalendarSettings(Runspace runspace, string id)
		{
			ExchangeLog.LogStart("SetCalendarSettings");
			Command cmd = new Command("Set-CalendarProcessing");
			cmd.Parameters.Add("Identity", id);
			cmd.Parameters.Add("AutomateProcessing", CalendarProcessingFlags.AutoAccept);
			ExecuteShellCommand(runspace, cmd);
			ExchangeLog.LogEnd("SetCalendarSettings");
		}

		#endregion

		#region Distribution Lists
		internal override string GetGroupManager(PSObject group)
		{
			string ret = null;
			MultiValuedProperty<ADObjectId> ids =
				(MultiValuedProperty<ADObjectId>)GetPSObjectProperty(group, "ManagedBy");
			if ( ids.Count > 0 )
				ret = ObjToString(ids[0]);
			return ret;
		}

		internal override void RemoveDistributionGroup(Runspace runSpace, string id)
		{
			ExchangeLog.LogStart("RemoveDistributionGroup");
			Command cmd = new Command("Remove-DistributionGroup");
			cmd.Parameters.Add("Identity", id);
			cmd.Parameters.Add("Confirm", false);
			cmd.Parameters.Add("BypassSecurityGroupManagerCheck");
			ExecuteShellCommand(runSpace, cmd);
			ExchangeLog.LogEnd("RemoveDistributionGroup");
		}

		internal override void SetDistributionGroup(Runspace runSpace, string id, string displayName, bool hideFromAddressBook)
		{
			Command cmd = new Command("Set-DistributionGroup");
			cmd.Parameters.Add("Identity", id);
			cmd.Parameters.Add("DisplayName", displayName);
			cmd.Parameters.Add("HiddenFromAddressListsEnabled", hideFromAddressBook);
			cmd.Parameters.Add("BypassSecurityGroupManagerCheck"); 
			ExecuteShellCommand(runSpace, cmd);
		}

		internal override void SetGroup(Runspace runSpace, string id, string managedBy, string notes)
		{
			Command cmd = new Command("Set-Group");
			cmd.Parameters.Add("Identity", id);
			cmd.Parameters.Add("ManagedBy", managedBy);
			cmd.Parameters.Add("Notes", notes);
			cmd.Parameters.Add("BypassSecurityGroupManagerCheck"); 
			ExecuteShellCommand(runSpace, cmd);
		}

		internal override void RemoveDistributionGroupMember(Runspace runSpace, string group, string member)
		{
			Command cmd = new Command("Remove-DistributionGroupMember");
			cmd.Parameters.Add("Identity", group);
			cmd.Parameters.Add("Member", member);
			cmd.Parameters.Add("Confirm", false);
			cmd.Parameters.Add("BypassSecurityGroupManagerCheck"); 
			ExecuteShellCommand(runSpace, cmd);
		}

		internal override void AddDistributionGroupMember(Runspace runSpace, string group, string member)
		{
			Command cmd = new Command("Add-DistributionGroupMember");
			cmd.Parameters.Add("Identity", group);
			cmd.Parameters.Add("Member", member);
			cmd.Parameters.Add("BypassSecurityGroupManagerCheck"); 
			ExecuteShellCommand(runSpace, cmd);
		}

		internal override void SetDistributionListSendOnBehalfAccounts(Runspace runspace, string accountName, string[] sendOnBehalfAccounts)
		{
			ExchangeLog.LogStart("SetDistributionListSendOnBehalfAccounts");
			Command cmd = new Command("Set-DistributionGroup");
			cmd.Parameters.Add("Identity", accountName);
			cmd.Parameters.Add("GrantSendOnBehalfTo", SetSendOnBehalfAccounts(runspace, sendOnBehalfAccounts));
			cmd.Parameters.Add("BypassSecurityGroupManagerCheck"); 
			ExecuteShellCommand(runspace, cmd);
			ExchangeLog.LogEnd("SetDistributionListSendOnBehalfAccounts");
		}
		#endregion

		#region PowerShell integration
		internal override string ExchangeSnapInName
		{
			get { return "Microsoft.Exchange.Management.PowerShell.E2010"; }
		}

		internal override Runspace OpenRunspace()
		{
			Runspace runspace = base.OpenRunspace();
			Command cmd = new Command("Set-ADServerSettings");
			cmd.Parameters.Add("PreferredServer", PrimaryDomainController);
			ExecuteShellCommand(runspace, cmd, false);
			return runspace;
		}

		private static Assembly ResolveExchangeAssembly(object p, ResolveEventArgs args)
		{
			//Add path for the Exchange 2007 DLLs
			if (args.Name.Contains("Microsoft.Exchange"))
			{
				string exchangePath = GetExchangePath();
				if (string.IsNullOrEmpty(exchangePath))
					return null;

				string path = Path.Combine(exchangePath, args.Name.Split(',')[0] + ".dll");
				if (!File.Exists(path))
					return null;
				
				ExchangeLog.DebugInfo("Resolved assembly: {0}", path);

				return Assembly.LoadFrom(path);
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region Storage
		internal override string CreateStorageGroup(Runspace runSpace, string name, string server)
		{
			return string.Empty;
		}

		internal override string CreateMailboxDatabase(Runspace runSpace, string name, string storageGroup)
		{
			ExchangeLog.LogStart("CreateMailboxDatabase");
			string id;
			Command cmd = new Command("Get-MailboxDatabase");
			cmd.Parameters.Add("Identity", name);
			Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);
			if (result != null && result.Count > 0)
			{
				id = GetResultObjectIdentity(result);
			}
			else
			{
				throw new Exception(string.Format("Mailbox database {0} not found", name));
			}

			ExchangeLog.LogEnd("CreateMailboxDatabase");
			return id;
		}
		#endregion

		
        public override bool IsInstalled()
        {
			int value = 0;
			RegistryKey root = Registry.LocalMachine;
			RegistryKey rk = root.OpenSubKey(ExchangeRegistryPath);
			if (rk != null)
			{
				value = (int)rk.GetValue("MsiProductMajor", null);
				rk.Close();
			}
			return value == 14;
        }
	}
}

