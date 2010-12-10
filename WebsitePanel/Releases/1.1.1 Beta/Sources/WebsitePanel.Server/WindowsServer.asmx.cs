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
using System.Data;
using System.Web;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ServiceProcess;
using System.Management;
using Microsoft.Web.Services3;

using WebsitePanel.Providers.Utils;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for WindowsServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class WindowsServer : System.Web.Services.WebService
    {
        #region Terminal connections
        [WebMethod]
        public TerminalSession[] GetTerminalServicesSessions()
        {
            try
            {
                Log.WriteStart("GetTerminalServicesSessions");
                List<TerminalSession> sessions = new List<TerminalSession>();
                string ret = FileUtils.ExecuteSystemCommand("qwinsta", "");

                // parse returned string
                StringReader reader = new StringReader(ret);
                string line = null;
				int lineIndex = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    /*if (line.IndexOf("USERNAME") != -1 )
                        continue;*/
					//
					if (lineIndex == 0)
					{
						lineIndex++;
						continue;
					}

                    Regex re = new Regex(@"(\S+)\s+", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    MatchCollection matches = re.Matches(line);

                    // add row to the table
                    string username = "";
                    if (matches.Count > 4)
                        username = matches[1].Value.Trim();

                    if (username != "")
                    {
                        TerminalSession session = new TerminalSession();
						//
                        session.SessionId = Int32.Parse(matches[2].Value.Trim());
                        session.Username = username;
                        session.Status = matches[3].Value.Trim();

                        sessions.Add(session);
                    }
					//
					lineIndex++;
                }
                reader.Close();

                Log.WriteEnd("GetTerminalServicesSessions");
                return sessions.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetTerminalServicesSessions", ex);
                throw;
            }
        }

        [WebMethod]
        public void CloseTerminalServicesSession(int sessionId)
        {
            try
            {
                Log.WriteStart("CloseTerminalServicesSession");
                FileUtils.ExecuteSystemCommand("rwinsta", sessionId.ToString());
                Log.WriteEnd("CloseTerminalServicesSession");
            }
            catch (Exception ex)
            {
                Log.WriteError("CloseTerminalServicesSession", ex);
                throw;
            }
        }
        #endregion

        #region Windows Processes
        [WebMethod]
        public WindowsProcess[] GetWindowsProcesses()
        {
            try
            {
                Log.WriteStart("GetWindowsProcesses");

                List<WindowsProcess> winProcesses = new List<WindowsProcess>();

                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objProcesses = wmi.ExecuteQuery(
                    "SELECT * FROM Win32_Process");

                foreach (ManagementObject objProcess in objProcesses)
                {
                    int pid = Int32.Parse(objProcess["ProcessID"].ToString());
                    string name = objProcess["Name"].ToString();

                    // get user info
                    string[] methodParams = new String[2];
                    objProcess.InvokeMethod("GetOwner", (object[])methodParams);
                    string username = methodParams[0];

                    WindowsProcess winProcess = new WindowsProcess();
                    winProcess.Pid = pid;
                    winProcess.Name = name;
                    winProcess.Username = username;
                    winProcess.MemUsage = Int64.Parse(objProcess["WorkingSetSize"].ToString());

                    winProcesses.Add(winProcess);
                }

                Log.WriteEnd("GetWindowsProcesses");
                return winProcesses.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWindowsProcesses", ex);
                throw;
            }
        }

        [WebMethod]
        public void TerminateWindowsProcess(int pid)
        {
            try
            {
                Log.WriteStart("TerminateWindowsProcess");

                Process[] processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.Id == pid)
                        process.Kill();
                }

                Log.WriteEnd("TerminateWindowsProcess");
            }
            catch (Exception ex)
            {
                Log.WriteError("TerminateWindowsProcess", ex);
                throw;
            }
        }
        #endregion

        #region Windows Services
        [WebMethod]
        public WindowsService[] GetWindowsServices()
        {
            try
            {
                Log.WriteStart("GetWindowsServices");
                List<WindowsService> winServices = new List<WindowsService>();

                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    WindowsService winService = new WindowsService();
                    winService.Id = service.ServiceName;
                    winService.Name = service.DisplayName;
                    winService.CanStop = service.CanStop;
                    winService.CanPauseAndContinue = service.CanPauseAndContinue;

                    WindowsServiceStatus status = WindowsServiceStatus.ContinuePending;
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.ContinuePending: status = WindowsServiceStatus.ContinuePending; break;
                        case ServiceControllerStatus.Paused: status = WindowsServiceStatus.Paused; break;
                        case ServiceControllerStatus.PausePending: status = WindowsServiceStatus.PausePending; break;
                        case ServiceControllerStatus.Running: status = WindowsServiceStatus.Running; break;
                        case ServiceControllerStatus.StartPending: status = WindowsServiceStatus.StartPending; break;
                        case ServiceControllerStatus.Stopped: status = WindowsServiceStatus.Stopped; break;
                        case ServiceControllerStatus.StopPending: status = WindowsServiceStatus.StopPending; break;
                    }
                    winService.Status = status;

                    winServices.Add(winService);
                }

                Log.WriteEnd("GetWindowsServices");
                return winServices.ToArray();
            }
            catch (Exception ex)
            {
                Log.WriteError("GetWindowsServices", ex);
                throw;
            }
        }

        [WebMethod]
        public void ChangeWindowsServiceStatus(string id, WindowsServiceStatus status)
        {
            try
            {
                Log.WriteStart("ChangeWindowsServiceStatus");
                // get all services
                ServiceController[] services = ServiceController.GetServices();

                // find required service
                foreach (ServiceController service in services)
                {
                    if (String.Compare(service.ServiceName, id, true) == 0)
                    {
                        if (status == WindowsServiceStatus.Paused
                            && service.Status == ServiceControllerStatus.Running)
                            service.Pause();
                        else if (status == WindowsServiceStatus.Running
                            && service.Status == ServiceControllerStatus.Stopped)
                            service.Start();
                        else if (status == WindowsServiceStatus.Stopped
                            && ((service.Status == ServiceControllerStatus.Running) ||
                                (service.Status == ServiceControllerStatus.Paused)))
                            service.Stop();
                        else if (status == WindowsServiceStatus.ContinuePending
                            && service.Status == ServiceControllerStatus.Paused)
                            service.Continue();
                    }
                }
                Log.WriteEnd("ChangeWindowsServiceStatus");
            }
            catch (Exception ex)
            {
                Log.WriteError("ChangeWindowsServiceStatus", ex);
                throw;
            }
        }
        #endregion

        #region Event Viewer
        [WebMethod]
        public List<string> GetLogNames()
        {
            List<string> logs = new List<string>();
            EventLog[] eventLogs = EventLog.GetEventLogs();
            foreach (EventLog eventLog in eventLogs)
            {
                logs.Add(eventLog.LogDisplayName);
            }
            return logs;
        }

        [WebMethod]
        public List<SystemLogEntry> GetLogEntries(string logName)
        {
            SystemLogEntriesPaged result = new SystemLogEntriesPaged();
            List<SystemLogEntry> entries = new List<SystemLogEntry>();

            if (String.IsNullOrEmpty(logName))
                return entries;
			
            EventLog log = new EventLog(logName);
            EventLogEntryCollection logEntries = log.Entries;
            int count = logEntries.Count;

            // iterate in reverse order
            for (int i = count - 1; i >= 0; i--)
                entries.Add(CreateLogEntry(logEntries[i], false));

            return entries;
        }

        [WebMethod]
        public SystemLogEntriesPaged GetLogEntriesPaged(string logName, int startRow, int maximumRows)
        {
            SystemLogEntriesPaged result = new SystemLogEntriesPaged();
            List<SystemLogEntry> entries = new List<SystemLogEntry>();

            if (String.IsNullOrEmpty(logName))
            {
                result.Count = 0;
                result.Entries = new SystemLogEntry[] { };
                return result;
            }
			
            EventLog log = new EventLog(logName);
            EventLogEntryCollection logEntries = log.Entries;
            int count = logEntries.Count;
            result.Count = count;

            // iterate in reverse order
            startRow = count - 1 - startRow;
            int endRow = startRow - maximumRows + 1;
            if (endRow < 0)
                endRow = 0;

            for (int i = startRow; i >= endRow; i--)
                entries.Add(CreateLogEntry(logEntries[i], true));

            result.Entries = entries.ToArray();

            return result;
        }

        [WebMethod]
        public void ClearLog(string logName)
        {
			EventLog log = new EventLog(logName);
			log.Clear();
        }

        private SystemLogEntry CreateLogEntry(EventLogEntry logEntry, bool includeMessage)
        {
            SystemLogEntry entry = new SystemLogEntry();
            switch (logEntry.EntryType)
            {
                case EventLogEntryType.Error: entry.EntryType = SystemLogEntryType.Error; break;
                case EventLogEntryType.Warning: entry.EntryType = SystemLogEntryType.Warning; break;
                case EventLogEntryType.Information: entry.EntryType = SystemLogEntryType.Information; break;
                case EventLogEntryType.SuccessAudit: entry.EntryType = SystemLogEntryType.SuccessAudit; break;
                case EventLogEntryType.FailureAudit: entry.EntryType = SystemLogEntryType.FailureAudit; break;
            }

            entry.Created = logEntry.TimeGenerated;
            entry.Source = logEntry.Source;
            entry.Category = logEntry.Category;
            entry.EventID = logEntry.InstanceId;
            entry.UserName = logEntry.UserName;
            entry.MachineName = logEntry.MachineName;

            if (includeMessage)
                entry.Message = logEntry.Message;

            return entry;
        }
        #endregion

        #region Reboot
        [WebMethod]
        public void RebootSystem()
        {
            try
            {
                Log.WriteStart("RebootSystem");
                WmiHelper wmi = new WmiHelper("root\\cimv2");
                ManagementObjectCollection objOses = wmi.ExecuteQuery("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject objOs in objOses)
                {
                    objOs.Scope.Options.EnablePrivileges = true;
                    objOs.InvokeMethod("Reboot", null);
                }
                Log.WriteEnd("RebootSystem");
            }
            catch (Exception ex)
            {
                Log.WriteError("RebootSystem", ex);
                throw;
            }
        }
        #endregion

        #region System Commands
        [WebMethod]
        public string ExecuteSystemCommand(string path, string args)
        {
            try
            {
                Log.WriteStart("ExecuteSystemCommand");
                string result = FileUtils.ExecuteSystemCommand(path, args);
                Log.WriteEnd("ExecuteSystemCommand");
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError("ExecuteSystemCommand", ex);
                throw;
            }
        }
        #endregion
    }
}
