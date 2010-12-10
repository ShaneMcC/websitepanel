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
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Reflection;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.EnterpriseServer
{
    public class TaskManager
    {
        private static Hashtable tasks = Hashtable.Synchronized(new Hashtable());
        private static Hashtable eventHandlers = null;

        // purge timer, used for killing old tasks from the hash
        static Timer purgeTimer = new Timer(new TimerCallback(PurgeCompletedTasks),
                                            null,
                                            60000, // start from 1 minute
                                            60000); // invoke each minute

        private static BackgroundTask RootTask
        {
            get { return TasksStack.Count > 0 ? TasksStack[0] : null; }
        }

        private static BackgroundTask TopTask
        {
            get { return TasksStack.Count > 0 ? TasksStack[TasksStack.Count - 1] : null; }
        }

        private static List<BackgroundTask> TasksStack
        {
            get
            {
                List<BackgroundTask> stack = (List<BackgroundTask>)Thread.GetData(Thread.GetNamedDataSlot("BackgroundTasksStack"));
                if (stack == null)
                {
                    stack = new List<BackgroundTask>();
                    Thread.SetData(Thread.GetNamedDataSlot("BackgroundTasksStack"), stack);
                }
                return stack;
            }
        }

        public static void StartTask(string source, string taskName)
        {
            StartTask(null, source, taskName, null);
        }

        public static void StartTask(string source, string taskName, object itemName)
        {
            StartTask(null, source, taskName, itemName);
        }

        public static void StartTask(string taskId, string source, string taskName, object itemName)
        {
            // create new task object
            BackgroundTask task = new BackgroundTask();
            task.TaskId = String.IsNullOrEmpty(taskId) ? Guid.NewGuid().ToString("N") : taskId;
            task.UserId = SecurityContext.User.UserId;
            task.EffectiveUserId = SecurityContext.User.IsPeer ? SecurityContext.User.OwnerId : task.UserId;
            task.StartDate = DateTime.Now;
            task.Source = source;
            task.TaskName = taskName;
            task.ItemName = itemName != null ? itemName.ToString() : "";
            task.Severity = 0; //info
            task.TaskThread = Thread.CurrentThread;

            // new "parent" task?
            if (TasksStack.Count == 0)
            {
                // register task globally
                tasks[task.TaskId] = task;
            }
            else
            {
                // child task
                // add log record to the root task
                BackgroundTaskLogRecord logRecord = new BackgroundTaskLogRecord();
                logRecord.InnerTaskStart = true;
                logRecord.Text = source + "_" + taskName;
                logRecord.TextParameters = new string[] { itemName != null ? itemName.ToString() : "" };
                logRecord.TextIdent = TasksStack.Count - 1;
                RootTask.LogRecords.Add(logRecord);

                // change log records destination
                // for nested task
                task.LogRecords = RootTask.LogRecords;
            }

            // call event handler
            CallTaskEventHandler(task, false);

            // push task on the stack
            TasksStack.Add(task);
        }

        public static void WriteParameter(string parameterName, object parameterValue)
        {
            string val = parameterValue != null ? parameterValue.ToString() : "";
            WriteLogRecord(0, parameterName + ": " + val, null, null);
        }

		public static void Write(string text, params string[] textParameters)
        {
            // INFO
            WriteLogRecord(0, text, null, textParameters);
        }

		public static void WriteWarning(string text, params string[] textParameters)
        {
            // WARNING
            WriteLogRecord(1, text, null, textParameters);
        }

        public static Exception WriteError(Exception ex)
        {
            // ERROR
            WriteLogRecord(2, ex.Message, ex.StackTrace);
            return new Exception(String.Format("Error executing '{0}' task on '{1}' {2}",
                TopTask.TaskName, TopTask.ItemName, TopTask.Source), ex);
        }

        public static void WriteError(Exception ex, string text, params string[] textParameters)
        {
            // ERROR
            string[] prms = new string[] { ex.Message };
            if (textParameters != null && textParameters.Length > 0)
            {
                prms = new string[textParameters.Length + 1];
                Array.Copy(textParameters, 0, prms, 1, textParameters.Length);
                prms[0] = ex.Message;
            }

            WriteLogRecord(2, text, ex.Message + "\n" + ex.StackTrace, prms);
        }

		public static void WriteError(string text, params string[] textParameters)
        {
            // ERROR
            WriteLogRecord(2, text, null, textParameters);
        }

        private static void WriteLogRecord(int severity, string text, string stackTrace, params string[] textParameters)
        {
            BackgroundTaskLogRecord logRecord = new BackgroundTaskLogRecord();
            logRecord.Severity = severity;
            logRecord.Text = text;
            logRecord.TextParameters = textParameters;
            logRecord.TextIdent = TasksStack.Count - 1;
            logRecord.ExceptionStackTrace = stackTrace;
            RootTask.LogRecords.Add(logRecord);
            RootTask.LastLogRecord = logRecord;

            // change entire task severity
            if (severity > RootTask.Severity)
                RootTask.Severity = severity;
        }

        public static void CompleteTask()
        {
			if (TasksStack.Count == 0)
				return;

            // call event handler
            CallTaskEventHandler(TopTask, true);

            // finish task
            TopTask.FinishDate = DateTime.Now;
            TopTask.Completed = true;

            // write task execution result to database
            if (TasksStack.Count == 1) // single task
            {
                // unregister task globally
                // tasks.Remove(TopTask.TaskId);

                // write to database
                string itemName = TopTask.ItemName != null ? TopTask.ItemName.ToString() : null;
                string executionLog = FormatExecutionLog(TopTask);
                UserInfo user = UserController.GetUserInternally(TopTask.UserId);
                string username = user != null ? user.Username : null;

                AuditLog.AddAuditLogRecord(TopTask.TaskId, TopTask.Severity, TopTask.UserId,
                    username, TopTask.PackageId, TopTask.ItemId,
                    itemName, TopTask.StartDate, TopTask.FinishDate, TopTask.Source,
                    TopTask.TaskName, executionLog);
            }

            // remove task from the stack
            TasksStack.RemoveAt(TasksStack.Count - 1);
        }

        static string FormatExecutionLog(BackgroundTask task)
        {
            StringWriter sw = new StringWriter();
            XmlWriter writer = new XmlTextWriter(sw);

            writer.WriteStartElement("log");
            
            // parameters
            writer.WriteStartElement("parameters");
            foreach (string name in task.Parameters.Keys)
            {
                string val = task.Parameters[name] != null ? task.Parameters[name].ToString() : "";
                writer.WriteStartElement("parameter");
                writer.WriteAttributeString("name", name);
                writer.WriteString(val);
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); // parameters

            // records
            writer.WriteStartElement("records");
            foreach (BackgroundTaskLogRecord record in task.LogRecords)
            {
                writer.WriteStartElement("record");
                writer.WriteAttributeString("severity", record.Severity.ToString());
                writer.WriteAttributeString("date", record.Date.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("ident", record.TextIdent.ToString());

                // text
                writer.WriteElementString("text", record.Text);

                // text parameters
                if (record.TextParameters != null && record.TextParameters.Length > 0)
                {
                    writer.WriteStartElement("textParameters");
                    foreach (string prm in record.TextParameters)
                    {
                        writer.WriteElementString("value", prm);
                    }
                    writer.WriteEndElement(); // textParameters
                }

                // stack trace
                writer.WriteElementString("stackTrace", record.ExceptionStackTrace);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();

            return sw.ToString();
        }

        static void PurgeCompletedTasks(object obj)
        {
            // remove completed tasks
            List<string> completedTasks = new List<string>();
            foreach (BackgroundTask task in tasks.Values)
            {
                if (task.MaximumExecutionTime != -1
                    && ((TimeSpan)(DateTime.Now - task.StartDate)).TotalSeconds > task.MaximumExecutionTime)
                {
                    // terminate task
                    try
                    {
                        task.TaskThread.Abort();
                    }
                    catch
                    {
                        // nope
                    }

                    // add to the list
                    completedTasks.Add(task.TaskId);
                }

                if ((task.FinishDate != DateTime.MinValue
                    && ((TimeSpan)(DateTime.Now - task.FinishDate)).TotalMinutes > 2))
                {
                    // add to the list
                    completedTasks.Add(task.TaskId);
                }
            }

            // remove tasks
            foreach (string taskId in completedTasks)
                tasks.Remove(taskId);
        }

        public static int PackageId
        {
            get { return TopTask.PackageId; }
            set { TopTask.PackageId = value; }
        }

        public static int ItemId
        {
            get { return TopTask.ItemId; }
            set { TopTask.ItemId = value; }
        }

        public static string ItemName
        {
            get { return TopTask.ItemName; }
            set { TopTask.ItemName = value; }
        }

        public static string TaskName
        {
            get { return TopTask.TaskName; }
        }

        public static string TaskSource
        {
            get { return TopTask.Source; }
        }

        public static int IndicatorMaximum
        {
            get { return TopTask.IndicatorMaximum; }
            set { TopTask.IndicatorMaximum = value; }
        }

        public static int IndicatorCurrent
        {
            get { return TopTask.IndicatorCurrent; }
            set { TopTask.IndicatorCurrent = value; }
        }

        public static int MaximumExecutionTime
        {
            get { return TopTask.MaximumExecutionTime; }
            set { TopTask.MaximumExecutionTime = value; }
        }

        public static int ScheduleId
        {
            get { return TopTask.ScheduleId; }
            set { TopTask.ScheduleId = value; }
        }

        public static bool HasErrors
        {
            get { return (TopTask.Severity == 2); }
        }

        public static BackgroundTask GetTask(string taskId)
        {
            BackgroundTask task = (BackgroundTask)tasks[taskId];
            if (task == null)
                return null;

            task.LastLogRecords.Clear();
            return task;
        }

        public static BackgroundTask GetTaskWithLogRecords(string taskId, DateTime startLogTime)
        {
            BackgroundTask task = GetTask(taskId);
            if (task == null)
                return null;

            // fill log records
            foreach (BackgroundTaskLogRecord record in task.LogRecords)
            {
                if (record.Date >= startLogTime)
                    task.LastLogRecords.Add(record);
            }

            return task;
        }

        public static Dictionary<int, BackgroundTask> GetScheduledTasks()
        {
            Dictionary<int, BackgroundTask> scheduledTasks = new Dictionary<int, BackgroundTask>();
            foreach (BackgroundTask task in tasks.Values)
            {
                if (task.ScheduleId > 0
					&& !task.Completed
					&& !scheduledTasks.ContainsKey(task.ScheduleId))
                    scheduledTasks.Add(task.ScheduleId, task);
            }
            return scheduledTasks;
        }

        public static void SetTaskNotifyOnComplete(string taskId)
        {
            BackgroundTask task = (BackgroundTask)tasks[taskId];
            if (task == null)
                return;

            task.NotifyOnComplete = true;
        }

        public static void StopTask(string taskId)
        {
            BackgroundTask task = (BackgroundTask)tasks[taskId];
            if (task == null)
                return;

            try
            {
                task.TaskThread.Abort();
            }
            catch
            {
                // nope
            }

            // remove it from stack
            tasks.Remove(taskId);
        }

        public static Hashtable TaskParameters
        {
            get { return TopTask.Parameters; }
        }

        internal static int GetTasksNumber()
        {
            return tasks.Count;
        }

        internal static List<BackgroundTask> GetUserTasks(int userId)
        {
            List<BackgroundTask> list = new List<BackgroundTask>();

            // try to get user first
            UserInfo user = UserController.GetUser(userId);
            if (user == null)
                return list; // prohibited user

            // get user tasks
            foreach (BackgroundTask task in tasks.Values)
            {
                if(task.EffectiveUserId == userId && !task.Completed)
                    list.Add(task);
            }
            return list;
        }

        internal static List<BackgroundTask> GetUserCompletedTasks(int userId)
        {
            // get user tasks
            List<BackgroundTask> list = GetUserTasks(userId);

            // extract completed only
            List<BackgroundTask> completedTasks = new List<BackgroundTask>();
            foreach (BackgroundTask task in list)
            {
                if (task.Completed && task.NotifyOnComplete)
                {
                    // add to the list
                    completedTasks.Add(task);

                    // remove from hash
                    tasks.Remove(task.TaskId);
                }
            }
            return completedTasks;
        }

        #region Private Helpers
        private static void CallTaskEventHandler(BackgroundTask task, bool onComplete)
        {
            string[] taskHandlers = GetTaskEventHandlers(task.Source, task.TaskName);
            if (taskHandlers != null)
            {
                foreach(string taskHandler in taskHandlers)
                {
                    try
                    {
                        Type handlerType = Type.GetType(taskHandler);
                        TaskEventHandler handler = (TaskEventHandler)Activator.CreateInstance(handlerType);

                        if (handler != null)
                        {
                            if (onComplete)
                                handler.OnComplete();
                            else
                                handler.OnStart();
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex, "Error executing task event handler: {0}", ex.Message);
                    }
                }
            }
        }

        private static string[] GetTaskEventHandlers(string source, string taskName)
        {
            // load configuration
            string appRoot = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(appRoot, "TaskEventHandlers.config");

            if (eventHandlers == null)
            {
                eventHandlers = Hashtable.Synchronized(new Hashtable());

                // load from XML
                if (File.Exists(path))
                {
                    List<XmlDocument> xmlConfigs = new List<XmlDocument>();
                    xmlConfigs.Add(new XmlDocument());
                    xmlConfigs[0].Load(path);
                    // Lookup for external references first
                    XmlNodeList xmlReferences = xmlConfigs[0].SelectNodes("//reference");
                    foreach (XmlElement xmlReference in xmlReferences)
                    {
                        string referencePath = Path.Combine(appRoot, xmlReference.GetAttribute("src"));
                        if (File.Exists(referencePath))
                        {
                            XmlDocument xmldoc = new XmlDocument();
                            xmldoc.Load(referencePath);
                            xmlConfigs.Add(xmldoc);
                        }
                    }

                    // parse XML
                    foreach (XmlDocument xml in xmlConfigs)
                    {
                        XmlNodeList xmlHandlers = xml.SelectNodes("//handler");
                        foreach (XmlNode xmlHandler in xmlHandlers)
                        {
                            string keyName = xmlHandler.ParentNode.Attributes["source"].Value
                                + xmlHandler.ParentNode.Attributes["name"].Value;

                            // get handlers collection
                            List<string> taskHandlers = (List<string>)eventHandlers[keyName];
                            if (taskHandlers == null)
                            {
                                taskHandlers = new List<string>();
                                eventHandlers[keyName] = taskHandlers;
                            }

                            string handlerType = xmlHandler.Attributes["type"].Value;
                            taskHandlers.Add(handlerType);
                        }
                    }
                }
            }

            string fullTaskName = source + taskName;
            List<string> handlersList = (List<string>)eventHandlers[fullTaskName];
            return handlersList == null ? null : handlersList.ToArray();
        }
        #endregion


        #region ResultTasks

        public static void CompleteResultTask(ResultObject res, string errorCode, Exception ex, string errorMessage)
        {
            if (res != null)
            {
                res.IsSuccess = false;

                if (!string.IsNullOrEmpty(errorCode))
                    res.ErrorCodes.Add(errorCode);
            }

            if (ex != null)
                TaskManager.WriteError(ex);

            if (!string.IsNullOrEmpty(errorMessage))
                TaskManager.WriteError(errorMessage);

            //LogRecord.
            CompleteTask();


        }

        public static void CompleteResultTask(ResultObject res, string errorCode, Exception ex)
        {
            CompleteResultTask(res, errorCode, ex, null);
        }

        public static void CompleteResultTask(ResultObject res, string errorCode)
        {
            CompleteResultTask(res, errorCode, null, null);
        }

        public static void CompleteResultTask(ResultObject res)
        {
            CompleteResultTask(res, null);
        }

        public static void CompleteResultTask()
        {
            CompleteResultTask(null);
        }

        public static T StartResultTask<T>(string source, string taskName) where T : ResultObject, new()
        {
            StartTask(source, taskName);
            T res = new T();
            res.IsSuccess = true;
            return res;
        }
        
        #endregion 
    }
}
