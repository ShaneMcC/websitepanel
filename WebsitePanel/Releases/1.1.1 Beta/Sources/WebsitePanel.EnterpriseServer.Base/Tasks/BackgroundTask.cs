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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebsitePanel.EnterpriseServer
{
    public class BackgroundTask
    {
        private string taskId;
        private int userId;
        private int packageId;
        private int effectiveUserId;
        private DateTime startDate = DateTime.MinValue;
        private DateTime finishDate = DateTime.MinValue;
        private int maximumExecutionTime = -1; // seconds
        private string source;
        private string taskName;
        private int scheduleId;
        private string itemName;
        private int itemId = 0;
        private int severity = 0; /* 0 - Info, 1 - Warning, 2 - Error */
        private List<BackgroundTaskLogRecord> logRecords = new List<BackgroundTaskLogRecord>();
        private List<BackgroundTaskLogRecord> lastLogRecords = new List<BackgroundTaskLogRecord>();
        private BackgroundTaskLogRecord lastLogRecord;
        private Hashtable parameters = new Hashtable();
        private int indicatorMaximum;
        private int indicatorCurrent;
        private bool completed;
        private bool notifyOnComplete;
        private Thread taskThread;

        public System.DateTime StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public System.DateTime FinishDate
        {
            get { return this.finishDate; }
            set { this.finishDate = value; }
        }

        public string Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        public string TaskName
        {
            get { return this.taskName; }
            set { this.taskName = value; }
        }

        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        public int PackageId
        {
            get { return this.packageId; }
            set { this.packageId = value; }
        }

        public int Severity
        {
            get { return this.severity; }
            set { this.severity = value; }
        }

        [XmlIgnore]
        public List<BackgroundTaskLogRecord> LogRecords
        {
            get { return this.logRecords; }
            set { this.logRecords = value; }
        }

        public List<BackgroundTaskLogRecord> LastLogRecords
        {
            get { return this.lastLogRecords; }
        }

        public string ItemName
        {
            get { return this.itemName; }
            set { this.itemName = value; }
        }

        public BackgroundTaskLogRecord LastLogRecord
        {
            get { return this.lastLogRecord; }
            set { this.lastLogRecord = value; }
        }

        public string TaskId
        {
            get { return this.taskId; }
            set { this.taskId = value; }
        }

        public int UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        [XmlIgnore]
        public Hashtable Parameters
        {
            get { return this.parameters; }
        }

        public int IndicatorMaximum
        {
            get { return this.indicatorMaximum; }
            set { this.indicatorMaximum = value; }
        }

        public int IndicatorCurrent
        {
            get { return this.indicatorCurrent; }
            set { this.indicatorCurrent = value; }
        }

        public bool Completed
        {
            get { return this.completed; }
            set { this.completed = value; }
        }

        public bool NotifyOnComplete
        {
            get { return this.notifyOnComplete; }
            set { this.notifyOnComplete = value; }
        }

        public int EffectiveUserId
        {
            get { return this.effectiveUserId; }
            set { this.effectiveUserId = value; }
        }

        [XmlIgnore]
        public System.Threading.Thread TaskThread
        {
            get { return this.taskThread; }
            set { this.taskThread = value; }
        }

        public int ScheduleId
        {
            get { return this.scheduleId; }
            set { this.scheduleId = value; }
        }

        public int MaximumExecutionTime
        {
            get { return this.maximumExecutionTime; }
            set { this.maximumExecutionTime = value; }
        }
    }
}
