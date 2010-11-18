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
using System.Management.Automation.Runspaces;
using System.Text;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Providers.HostedSolution
{
	/// <summary>
	/// Exchange Log Helper Methods
	/// </summary>
	internal class ExchangeLog
	{
		internal static string LogPrefix = "Exchange";

		internal static void LogStart(string message, params object[] args)
		{
			string text = String.Format(message, args);
			Log.WriteStart("{0} {1}", LogPrefix, text);
		}

		internal static void LogEnd(string message, params object[] args)
		{
			string text = String.Format(message, args);
			Log.WriteEnd("{0} {1}", LogPrefix, text);
		}

		internal static void LogInfo(string message, params object[] args)
		{
			string text = String.Format(message, args);
			Log.WriteInfo("{0} {1}", LogPrefix, text);
		}

		internal static void LogWarning(string message, params object[] args)
		{
			string text = String.Format(message, args);
			Log.WriteWarning("{0} {1}", LogPrefix, text);
		}

		internal static void LogError(Exception ex)
		{
			Log.WriteError(LogPrefix, ex);
		}

		internal static void LogError(string message, Exception ex)
		{
			string text = String.Format("{0} {1}", LogPrefix, message);
			Log.WriteError(text, ex);
		}

		internal static void DebugInfo(string message, params object[] args)
		{
#if DEBUG
			string text = String.Format(message, args);
			Log.WriteInfo("{0} {1}", LogPrefix, text);
#endif
		}

		internal static void DebugCommand(Command cmd)
		{
#if DEBUG
			StringBuilder sb = new StringBuilder(cmd.CommandText);
			foreach (CommandParameter parameter in cmd.Parameters)
			{
				string formatString = " -{0} {1}";
				if (parameter.Value is string) 
					formatString = " -{0} '{1}'";
				else if (parameter.Value is bool)
					formatString = " -{0} ${1}";
				sb.AppendFormat(formatString, parameter.Name, parameter.Value);
			}
			Log.WriteInfo("{0} {1}", LogPrefix, sb.ToString());
#endif
		}
	}
}
