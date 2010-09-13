using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsitePanel.Server.Utils;
using System.Management.Automation.Runspaces;

namespace WebsitePanel.Providers.ExchangeHostedEdition
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
